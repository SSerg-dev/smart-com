using System;
using System.Linq;
using System.Collections.Generic;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl.RoleStateMap;

namespace Module.Persist.TPM.PromoStateControl
{
    public partial class PromoStateContext
    {
        public class OnApprovalState : IPromoState
        {
            private readonly PromoStateContext _stateContext;

            private readonly string Name = "OnApproval";

            private readonly List<string> Roles = new List<string> { "Administrator", "CustomerMarketing", "DemandFinance", "DemandPlanning", "FunctionalExpert", "KeyAccountManager" };

            public OnApprovalState(PromoStateContext stateContext)
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

            public bool ChangeState(Promo promoModel, string userRole, out string massage)
            {
                massage = string.Empty;

                bool sendForApproval = false;
                PromoStatus promoStatus;

                // Условия для возврата
                if ((_stateContext.Model.MarsMechanicDiscount < promoModel.MarsMechanicDiscount) ||
                    (_stateContext.Model.MarsMechanic.SystemName == "VP" && promoModel.MarsMechanic.SystemName == "TPR"))
                {
                    promoStatus = _stateContext.dbContext.Set<PromoStatus>().First(n => n.SystemName == "DraftPublished");
                    promoModel.PromoStatusId = promoStatus.Id;
                    sendForApproval = true;
                }
                else
                {
                    promoStatus = _stateContext.dbContext.Set<PromoStatus>().Find(promoModel.PromoStatusId);
                }

                string statusName = promoStatus.SystemName;

                bool isAvailable = PromoStateUtil.CheckAccess(GetAvailableStates(), statusName, userRole);
                bool isAvailableCurrent = PromoStateUtil.CheckAccess(Roles, userRole);

                if (isAvailable)
                {
                    // Go to: DraftPublishedState (Rejected)
                    if (statusName == "DraftPublished")
                    {
                        promoModel.IsAutomaticallyApproved = false;
                        promoModel.IsCustomerMarketingApproved = false;
                        promoModel.IsDemandPlanningApproved = false;
                        promoModel.IsDemandFinanceApproved = false;
                        _stateContext.Model = promoModel;
                        _stateContext.State = _stateContext._draftPublishedState;

                        // Если были условия для возврата, то пытаемся перевести в approval
                        if (sendForApproval)
                        {
                            PromoStatus onApprovalStatus = _stateContext.dbContext.Set<PromoStatus>().First(n => n.SystemName == "OnApproval");
                            Promo promoDraftPublished = new Promo(promoModel);

                            _stateContext.Model = promoDraftPublished;
                            promoModel.PromoStatusId = onApprovalStatus.Id;
                            return _stateContext.ChangeState(promoModel, userRole, out massage);
                        }

                        return true;
                    }
                    else
                    {
                        // ToDo: проверка на ручное подтверждение
                        // Go to: Approval

                        // Переход в следующий статус только после утверждения
                        bool next = false;
                        switch (userRole)
                        {
                            case "CustomerMarketing":
                                promoModel.IsCustomerMarketingApproved = true;
                                break;

                            case "DemandPlanning":
                                promoModel.IsDemandPlanningApproved = promoModel.IsCustomerMarketingApproved;
                                // если промо прошло проверку на NoNego, но не прошло на 8 недель, то решает только DemandPlanning
                                // и в этом случае IsCustomerMarketingApproved и IsDemandFinanceApproved = true
                                next = promoModel.IsDemandFinanceApproved.HasValue && promoModel.IsDemandFinanceApproved.Value;  
                                break;

                            case "DemandFinance":
                                if (promoModel.IsCustomerMarketingApproved.HasValue && promoModel.IsDemandPlanningApproved.HasValue)
                                {
                                    promoModel.IsDemandFinanceApproved = promoModel.IsCustomerMarketingApproved.Value && promoModel.IsDemandPlanningApproved.Value;
                                    next = promoModel.IsDemandFinanceApproved.Value;
                                }
                                break;

                            default: break;
                        }

                        if (next)
                        {
                            _stateContext.State = _stateContext._approvedState;
                        }
                        else
                        {
                            // Если не все лица утвердили, то переход в новый статус не происходит
                            promoModel.PromoStatusId = _stateContext.Model.PromoStatus.Id;
                        }

                        _stateContext.Model = promoModel;

                        return true;
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
