using Core.Dependency;
using Core.Settings;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.PromoStateControl
{
    public static class PromoStatusHelper
    {
        public static bool IsParametersChanged(Promo curPromo, Promo prevPromo, IEnumerable<Guid> stateIdVP, IEnumerable<Guid> stateIdTPR)
        {
            return ((prevPromo.MarsMechanicDiscount < curPromo.MarsMechanicDiscount) ||
                    (stateIdVP.Any(id => id == prevPromo.MarsMechanicId) && stateIdTPR.Any(id => id == curPromo.MarsMechanicId)) ||
                    (prevPromo.ProductHierarchy != curPromo.ProductHierarchy) ||
                    (prevPromo.StartDate != curPromo.StartDate) ||
                    (prevPromo.EndDate != curPromo.EndDate) ||
                    (prevPromo.IsGrowthAcceleration != curPromo.IsGrowthAcceleration));
        }

        public static bool IsDispatchChanged(bool isCorrectDispatchDifference, Promo curPromo, Promo prevPromo)
        {
            return !isCorrectDispatchDifference && (prevPromo.DispatchesEnd != curPromo.DispatchesEnd || prevPromo.DispatchesStart != curPromo.DispatchesStart);
        }

        public static bool CheckNoNego(Promo model, DatabaseContext context)
        {
            List<NoneNego> noNegoList = context.Set<NoneNego>().Where(x => !x.Disabled && x.FromDate <= model.StartDate && x.ToDate >= model.EndDate).ToList();

            ClientTreeHierarchyView clientTreeHierarchy = context.Set<ClientTreeHierarchyView>().FirstOrDefault(x => x.Id == model.ClientTreeId);

            // может быть выбрано несколько продуктов (subrange) в промо
            int[] productObjectIds = context.Set<PromoProductTree>().Where(n => n.PromoId == model.Id && !n.Disabled).Select(n => n.ProductTreeObjectId).ToArray();
            ProductTreeHierarchyView[] productTreeHierarchies = context.Set<ProductTreeHierarchyView>().Where(x => productObjectIds.Contains(x.Id)).ToArray();

            foreach (ProductTreeHierarchyView prodHierarchy in productTreeHierarchies)
            {
                bool resultForProduct = false;
                string productHierarchy = prodHierarchy.Hierarchy + "." + prodHierarchy.Id.ToString();
                int[] productHierarchyArr = Array.ConvertAll(productHierarchy.Split('.'), int.Parse);

                for (int i = (productHierarchyArr.Length - 1); i > 0 && !resultForProduct; i--)
                {
                    string clientHierarchy = clientTreeHierarchy.Hierarchy + "." + model.ClientTreeId.ToString();
                    int[] clientHierarchyArr = Array.ConvertAll(clientHierarchy.Split('.'), int.Parse);

                    for (int j = (clientHierarchyArr.Length - 1); j > 0 && !resultForProduct; j--)
                    {
                        List<NoneNego> noNegoForClientList = noNegoList.Where(x => x.ClientTree.ObjectId == clientHierarchyArr[j]).ToList();
                        foreach (NoneNego noNego in noNegoForClientList)
                        {
                            if (noNego.ProductTree.ObjectId == productHierarchyArr[i])
                            {
                                if (noNego.Mechanic != null && model.MarsMechanic != null && noNego.Mechanic.SystemName == model.MarsMechanic.SystemName)
                                {
                                    if (noNego.Discount >= model.MarsMechanicDiscount)
                                    {
                                        resultForProduct = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                // если хоть один subrange не прошел проверку, то отклоняем
                if (!resultForProduct)
                    return false;
            }

            return true;
        }

        public static void StableOnApprovalStatus(Promo promoModel, string userRole, DatabaseContext context)
        {
            promoModel.IsCMManagerApproved = false;
            promoModel.IsDemandFinanceApproved = false;
            promoModel.IsDemandFinanceApproved = false;

            var oldIncidents = context.Set<PromoOnApprovalIncident>().Where(x => x.PromoId == promoModel.Id && x.ProcessDate == null);
            if (promoModel.IsGrowthAcceleration)
            {
                // Закрываем все неактуальные инциденты
                foreach (var incident in oldIncidents)
                {
                    incident.ProcessDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                }
                context.Set<PromoOnApprovalIncident>().Add(new PromoOnApprovalIncident()
                {
                    PromoId = promoModel.Id,
                    ApprovingRole = "CMManager", // Или DemandPlanning? Артём должен уточнить
                    CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                    Promo = promoModel
                });
                return;
            }

            bool isNoNego = CheckNoNego(promoModel, context);
            if (isNoNego)
            {
                ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                var toApprovedDispatchDays = settingsManager.GetSetting<int>("TO_APPROVED_DISPATCH_DAYS_COUNT", 7 * 8);
                bool isCorrectDispatchDifference = (promoModel.DispatchesStart - ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)).Value.Days >= toApprovedDispatchDays;

                if (!promoModel.InOut.HasValue || !promoModel.InOut.Value)
                {
                    if ((isCorrectDispatchDifference || userRole == UserRoles.FunctionalExpert.ToString())
                        && promoModel.PlanPromoBaselineLSV.HasValue && promoModel.PlanPromoBaselineLSV > 0
                        && promoModel.PlanPromoUpliftPercent.HasValue && promoModel.PlanPromoUpliftPercent > 0)
                    {
                        PromoStatus approvedStatus = context.Set<PromoStatus>().FirstOrDefault(e => e.SystemName == "Approved");
                        promoModel.PromoStatusId = approvedStatus.Id;
                        promoModel.IsAutomaticallyApproved = true;

                        return;
                    }
                    else
                    {
                        promoModel.IsCMManagerApproved = true;
                        promoModel.IsDemandFinanceApproved = true;

                        // Закрываем все неактуальные инциденты
                        foreach (var incident in oldIncidents)
                        {
                            incident.ProcessDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                        }
                        context.Set<PromoOnApprovalIncident>().Add(new PromoOnApprovalIncident()
                        {
                            PromoId = promoModel.Id,
                            ApprovingRole = "DemandPlanning",
                            CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                            Promo = promoModel
                        });
                        return;
                    }
                }
                else
                {
                    if ((isCorrectDispatchDifference || userRole == UserRoles.FunctionalExpert.ToString())
                        && promoModel.PlanPromoIncrementalLSV.HasValue && promoModel.PlanPromoIncrementalLSV.Value > 0)
                    {
                        PromoStatus approvedStatus = context.Set<PromoStatus>().FirstOrDefault(e => e.SystemName == "Approved");
                        promoModel.PromoStatusId = approvedStatus.Id;
                        promoModel.IsAutomaticallyApproved = true;

                        return;
                    }
                    else
                    {
                        promoModel.IsCMManagerApproved = true;
                        promoModel.IsDemandFinanceApproved = true;

                        foreach (var incident in oldIncidents)
                        {
                            incident.ProcessDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                        }
                        context.Set<PromoOnApprovalIncident>().Add(new PromoOnApprovalIncident()
                        {
                            PromoId = promoModel.Id,
                            ApprovingRole = "DemandPlanning",
                            CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                            Promo = promoModel
                        });
                        return;
                    }
                }
            }
            else
            {
                foreach (var incident in oldIncidents)
                {
                    incident.ProcessDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                }
                context.Set<PromoOnApprovalIncident>().Add(new PromoOnApprovalIncident()
                {
                    PromoId = promoModel.Id,
                    ApprovingRole = "CMManager",
                    CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                    Promo = promoModel
                });

                return;
            }
        }
    }
}
