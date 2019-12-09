using System;
using System.Linq;
using System.Collections.Generic;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.PromoStateControl.RoleStateMap;
using Module.Persist.TPM.Utils;
using Core.Settings;
using Core.Dependency;

namespace Module.Persist.TPM.PromoStateControl
{
    public partial class PromoStateContext
    {
        public class DraftPublishedState : IPromoState
        {
            private readonly PromoStateContext _stateContext;

            private readonly string Name = "DraftPublished";

            private readonly List<string> Roles = new List<string> { "Administrator", "CMManager", "CustomerMarketing", "DemandFinance", "DemandPlanning", "FunctionalExpert", "KeyAccountManager" };

            public DraftPublishedState(PromoStateContext stateContext)
            {
                _stateContext = stateContext;
            }

            public string GetName()
            {
                return Name;
            }

            public List<string> GetRoles()
            {
                return Roles;
            }

            public Promo GetModel()
            {
                return _stateContext.Model;
            }

            public Dictionary<string, List<string>> GetAvailableStates()
            {
                return RoleStateUtil.GetMapForStatus(Name);
            }

            private bool CheckNoNego(Promo model)
            {
                List<NoneNego> noNegoList = _stateContext.dbContext.Set<NoneNego>().Where(x => !x.Disabled && x.FromDate <= model.StartDate && x.ToDate >= model.EndDate).ToList();

                ClientTreeHierarchyView clientTreeHierarchy = _stateContext.dbContext.Set<ClientTreeHierarchyView>().FirstOrDefault(x => x.Id == model.ClientTreeId);

                // может быть выбрано несколько продуктов (subrange) в промо
                int[] productObjectIds = _stateContext.dbContext.Set<PromoProductTree>().Where(n => n.PromoId == model.Id && !n.Disabled).Select(n => n.ProductTreeObjectId).ToArray();
                ProductTreeHierarchyView[] productTreeHierarchies = _stateContext.dbContext.Set<ProductTreeHierarchyView>().Where(x => productObjectIds.Contains(x.Id)).ToArray();

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

            public bool ChangeState(Promo promoModel, string userRole, out string message)
            {
                message = string.Empty;

                PromoStatus promoStatus = _stateContext.dbContext.Set<PromoStatus>().Find(promoModel.PromoStatusId);
                string statusName = promoStatus.SystemName;

                bool isAvailable = PromoStateUtil.CheckAccess(GetAvailableStates(), statusName, userRole);
                bool isAvailableCurrent = PromoStateUtil.CheckAccess(Roles, userRole);

                if (isAvailable)
                {
                    // Go to: DraftState
                    if (statusName == "Draft")
                    {
                        _stateContext.Model = promoModel;
                        _stateContext.State = _stateContext._draftState;

						if (userRole != "System")
						{
							// Если в Draft переводит не система, то удаляем reject incident, если есть
							var rejectIncidents = _stateContext.dbContext.Set<PromoOnRejectIncident>().Where(x => x.PromoId == promoModel.Id && !x.ProcessDate.HasValue);
							foreach (var incident in rejectIncidents)
							{
								incident.ProcessDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
							}
						}

						return true;
                    }
                    // Go to: OnApprovalState
                    else
                    {
						if (userRole != "System")
						{
							// Если в Draft переводит не система, то удаляем reject incident, если есть
							var rejectIncidents = _stateContext.dbContext.Set<PromoOnRejectIncident>().Where(x => x.PromoId == promoModel.Id && !x.ProcessDate.HasValue);
							foreach (var incident in rejectIncidents)
							{
								incident.ProcessDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
							}
						}

						var oldIncidents = _stateContext.dbContext.Set<PromoOnApprovalIncident>().Where(x => x.PromoId == promoModel.Id && x.ProcessDate == null);
						// Проверка на NoNego
						bool isNoNego = CheckNoNego(promoModel);
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
                                    PromoStatus approvedStatus = _stateContext.dbContext.Set<PromoStatus>().FirstOrDefault(e => e.SystemName == "Approved");
                                    promoModel.PromoStatusId = approvedStatus.Id;
                                    promoModel.IsAutomaticallyApproved = true;

                                    _stateContext.Model = promoModel;
                                    _stateContext.State = _stateContext._approvedState;

                                    return true;
                                }
                                else
                                {
									promoModel.IsCMManagerApproved = true;
                                    promoModel.IsDemandFinanceApproved = true;
                                    _stateContext.Model = promoModel;
                                    _stateContext.State = _stateContext._onApprovalState;

									// Закрываем все неактуальные инциденты
									foreach (var incident in oldIncidents)
									{
										incident.ProcessDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
									}
									_stateContext.dbContext.Set<PromoOnApprovalIncident>().Add(new PromoOnApprovalIncident()
									{
										PromoId = promoModel.Id,
										ApprovingRole = "DemandPlanning",
										CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
										Promo = promoModel
									});
									return true;
                                }
                            }
                            else
                            {
                                if ((isCorrectDispatchDifference || userRole == UserRoles.FunctionalExpert.ToString())
                                    && promoModel.PlanPromoIncrementalLSV.HasValue && promoModel.PlanPromoIncrementalLSV.Value > 0)
                                {
                                    PromoStatus approvedStatus = _stateContext.dbContext.Set<PromoStatus>().FirstOrDefault(e => e.SystemName == "Approved");
                                    promoModel.PromoStatusId = approvedStatus.Id;
                                    promoModel.IsAutomaticallyApproved = true;

                                    _stateContext.Model = promoModel;
                                    _stateContext.State = _stateContext._approvedState;

                                    return true;
                                }
                                else
                                {
                                    promoModel.IsCMManagerApproved = true;
                                    promoModel.IsDemandFinanceApproved = true;
                                    _stateContext.Model = promoModel;
                                    _stateContext.State = _stateContext._onApprovalState;

									foreach (var incident in oldIncidents)
									{
										incident.ProcessDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
									}
									_stateContext.dbContext.Set<PromoOnApprovalIncident>().Add(new PromoOnApprovalIncident()
									{
										PromoId = promoModel.Id,
										ApprovingRole = "DemandPlanning",
										CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
										Promo = promoModel
									});
									return true;
                                }
                            }
                        }
                        else
                        {
							foreach (var incident in oldIncidents)
							{
								incident.ProcessDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
							}
							_stateContext.dbContext.Set<PromoOnApprovalIncident>().Add(new PromoOnApprovalIncident(){
								PromoId = promoModel.Id,
								ApprovingRole = "CMManager",
								CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
								Promo = promoModel
							});

							_stateContext.Model = promoModel;
                            _stateContext.State = _stateContext._onApprovalState;

							return true;
                        }
                    }
                }
                // Current state
                else if (isAvailableCurrent && statusName == Name)
                {
                    _stateContext.Model = promoModel;

                    return true;
                }
                else
                {
                    message = "Action is not available";

                    return false;
                }
            }

            public bool ChangeState(Promo promoModel, PromoStates promoState, string userRole, out string message)
            {
                bool isAvailable=false;
                message = string.Empty;

                if (userRole == "System")
                {
                    isAvailable = true;
                }
                else
                {
                    isAvailable = PromoStateUtil.CheckAccess(GetAvailableStates(), promoState.ToString(), userRole);
                }

                if (isAvailable)
                {
                    // Go to: DraftState
                    if (promoState == PromoStates.Draft)
                    {
                        Guid draftPromoStatusId = _stateContext.dbContext.Set<PromoStatus>().Where(x => x.SystemName == "Draft" && !x.Disabled).FirstOrDefault().Id;

                        _stateContext.Model.PromoStatusId = draftPromoStatusId;
                        _stateContext.Model.NeedRecountUplift = true;
                        _stateContext.State = _stateContext._draftState;

						if (userRole != "System")
						{
							// Если в Draft переводит не система, то удаляем reject incident, если есть
							var rejectIncidents = _stateContext.dbContext.Set<PromoOnRejectIncident>().Where(x => x.PromoId == promoModel.Id && !x.ProcessDate.HasValue);
							foreach (var incident in rejectIncidents)
							{
								incident.ProcessDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
							}
						}

						return true;
                    }
                    // Go to: DraftPublishedState
                    else
                    {
                        _stateContext.Model = promoModel;
                        _stateContext.State = _stateContext._draftPublishedState;

                        return true;
                    }
                }
                else
                {
                    message = "Action is not available";

                    return false;
                }
            }
        }
    }
}
