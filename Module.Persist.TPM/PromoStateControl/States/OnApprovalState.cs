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

            private readonly List<string> Roles = new List<string> { "Administrator", "CMManager", "CustomerMarketing", "FunctionalExpert", "KeyAccountManager", "DemandPlanning" };

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

            public bool ChangeState(Promo promoModel, string userRole, out string message)
            {
                message = string.Empty;

                bool sendForApproval = false;
                PromoStatus promoStatus;

                Guid? stateIdVP = _stateContext.dbContext.Set<Mechanic>().FirstOrDefault(x => x.SystemName == "VP").Id;
                Guid? stateIdTPR = _stateContext.dbContext.Set<Mechanic>().FirstOrDefault(x => x.SystemName == "TPR").Id;

                bool isAvailable;
                bool isAvailableCurrent = PromoStateUtil.CheckAccess(Roles, userRole);

                // Условия для возврата
                if ((_stateContext.Model.MarsMechanicDiscount < promoModel.MarsMechanicDiscount) ||
                    (_stateContext.Model.MarsMechanicId == stateIdVP && promoModel.MarsMechanicId == stateIdTPR) ||
                    (_stateContext.Model.ProductHierarchy != promoModel.ProductHierarchy) ||
                    (_stateContext.Model.StartDate != promoModel.StartDate) || 
                    (_stateContext.Model.EndDate != promoModel.EndDate))
                {
                    promoStatus = _stateContext.dbContext.Set<PromoStatus>().First(n => n.SystemName == "DraftPublished");
                    promoModel.PromoStatusId = promoStatus.Id;
                    sendForApproval = true;
                    isAvailable = isAvailableCurrent;
                }
                else
                {
                    promoStatus = _stateContext.dbContext.Set<PromoStatus>().Find(promoModel.PromoStatusId);
                    isAvailable = PromoStateUtil.CheckAccess(GetAvailableStates(), promoStatus.SystemName, userRole);
                }

                string statusName = promoStatus.SystemName;                

                if (isAvailable)
                {
                    // Go to: DraftPublishedState (Rejected)
                    if (statusName == "DraftPublished")
                    {
                        promoModel.IsAutomaticallyApproved = false;
                        promoModel.IsCMManagerApproved = false;
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
                            return _stateContext.ChangeState(promoModel, "System", out message);
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
                            case "CMManager":
                                promoModel.IsCMManagerApproved = true;
                                break;

                            case "DemandPlanning":
                                // Если не InOut
                                if (!promoModel.InOut.HasValue || !promoModel.InOut.Value)
                                {
                                    // т.к. только Demand Planning может заполнить Plan Promo Uplift и загрузить Baseline на клиента,
                                    // то CMManager может согласовать промо без Uplift и Baseline, а Demand Planning уже не сможет
                                    bool okBaselineLSV = promoModel.PlanPromoBaselineLSV.HasValue && promoModel.PlanPromoBaselineLSV > 0;
                                    bool okUpliftPercent = promoModel.PlanPromoUpliftPercent.HasValue && promoModel.PlanPromoUpliftPercent > 0;

                                    if (okBaselineLSV && okUpliftPercent)
                                    {
                                        promoModel.IsDemandPlanningApproved = promoModel.IsCMManagerApproved;
                                        // если промо прошло проверку на NoNego, но не прошло на 8 недель, то решает только DemandPlanning
                                        // и в этом случае IsCMManagerApproved и IsDemandFinanceApproved = true
                                        next = promoModel.IsDemandFinanceApproved.HasValue && promoModel.IsDemandFinanceApproved.Value;
                                    }
                                    else
                                    {
                                        message = "";

                                        if (!okBaselineLSV)
                                            message += "The Plan Baseline LSV must not be empty or equal to zero. ";

                                        if (!okUpliftPercent)
                                            message += "The Plan Promo Uplift must not be empty or equal to zero.";
                                    }
                                }
                                else
                                {
                                    bool okPlanPromoIncrementalLSV = promoModel.PlanPromoIncrementalLSV.HasValue && promoModel.PlanPromoIncrementalLSV.Value > 0;

                                    if (okPlanPromoIncrementalLSV)
                                    {
                                        promoModel.IsDemandPlanningApproved = promoModel.IsCMManagerApproved;
                                        // если промо прошло проверку на NoNego, но не прошло на 8 недель, то решает только DemandPlanning
                                        // и в этом случае IsCMManagerApproved и IsDemandFinanceApproved = true
                                        next = promoModel.IsDemandFinanceApproved.HasValue && promoModel.IsDemandFinanceApproved.Value;
                                    }
                                    else
                                    {
                                        message = "";

                                        if (!okPlanPromoIncrementalLSV)
                                            message += "The Plan Promo Incremental LSV must not be empty or equal to zero. ";
                                    }
                                }
                                break;

                            case "DemandFinance":
                                if (promoModel.IsCMManagerApproved.HasValue && promoModel.IsDemandPlanningApproved.HasValue)
                                {
                                    promoModel.IsDemandFinanceApproved = promoModel.IsCMManagerApproved.Value && promoModel.IsDemandPlanningApproved.Value;
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
                    message = "Action is not available";

                    return false;
                }
            }

            public bool ChangeState(Promo promoModel, PromoStates promoState, string userRole, out string message)
            {
                throw new NotImplementedException();
            }
        }
    }
}
