using System;
using System.Linq;
using System.Collections.Generic;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.PromoStateControl.RoleStateMap;

namespace Module.Persist.TPM.PromoStateControl
{
    public partial class PromoStateContext
    {
        public class DraftPublishedState : IPromoState
        {
            private readonly PromoStateContext _stateContext;

            private readonly string Name = "DraftPublished";

            private readonly List<string> Roles = new List<string> { "Administrator", "CustomerMarketing", "DemandFinance", "DemandPlanning", "FunctionalExpert", "KeyAccountManager" };

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
                                    if (noNego.Mechanic == model.MarsMechanic)
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

            public bool ChangeState(Promo promoModel, string userRole, out string massage)
            {
                massage = string.Empty;

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

                        return true;
                    }
                    // Go to: OnApprovalState
                    else
                    {
                        // Проверка на NoNego
                        bool isNoNego = CheckNoNego(promoModel);
                        if (isNoNego)
                        {
                            TimeSpan difference = (DateTimeOffset)promoModel.StartDate - DateTimeOffset.Now;

                            if (difference.Days >= 56 || userRole == UserRoles.FunctionalExpert.ToString())
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
                                promoModel.IsCustomerMarketingApproved = true;
                                promoModel.IsDemandFinanceApproved = true;
                                _stateContext.Model = promoModel;
                                _stateContext.State = _stateContext._onApprovalState;

                                return true;
                            }
                        }
                        else
                        {
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
                    massage = "Action is not available";

                    return false;
                }
            }

            public bool ChangeState(Promo promoModel, PromoStates promoState, string userRole, out string massage)
            {
                throw new NotImplementedException();
            }
        }
    }
}
