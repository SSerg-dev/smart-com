using System;
using System.Linq;
using System.Collections.Generic;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl.RoleStateMap;

namespace Module.Persist.TPM.PromoStateControl
{
    public partial class PromoStateContext
    {
        public class PlannedState : IPromoState
        {
            private readonly PromoStateContext _stateContext;

            private readonly string Name = "Planned";

            private readonly List<string> Roles = new List<string> { "System", "Administrator", "CMManager", "CustomerMarketing", "FunctionalExpert", "KeyAccountManager" };

            public PlannedState(PromoStateContext stateContext)
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
                    // Go to: StartedState
                    if (statusName == "Started")
                    {
                        if (promoModel.DispatchesStart >= DateTimeOffset.Now)
                        {
                            _stateContext.Model = promoModel;
                            _stateContext.State = _stateContext._startedState;

                            return true;
                        }
                        else
                        {
                            _stateContext.Model = promoModel;

                            return false;
                        }
                    }
                    // Go to: DraftPublishedState
                    else
                    {
                        promoModel.IsCMManagerApproved = false;
                        promoModel.IsDemandPlanningApproved = false;
                        promoModel.IsDemandFinanceApproved = false;
                        promoModel.IsAutomaticallyApproved = false;
                        _stateContext.Model = promoModel;
                        _stateContext.State = _stateContext._draftPublishedState;

                        // Если были условия для возврата, то пытаемся перевести в approval
                        if (sendForApproval)
                        {
                            PromoStatus onApprovalStatus = _stateContext.dbContext.Set<PromoStatus>().First(n => n.SystemName == "OnApproval");
                            Promo promoDraftPublished = new Promo(promoModel);

                            _stateContext.Model = promoDraftPublished;
                            promoModel.PromoStatusId = onApprovalStatus.Id;
                            return _stateContext.ChangeState(promoModel, userRole, out message);
                        }

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
                message = string.Empty;

                bool isAvailable = PromoStateUtil.CheckAccess(GetAvailableStates(), promoState.ToString(), userRole);

                if (isAvailable)
                {
                    // Go to: StartedState
                    if (promoState == PromoStates.Started)
                    {
                        if (_stateContext.Model.StartDate <= DateTimeOffset.Now.Date)
                        {
                            Guid startedPromoStatusId = _stateContext.dbContext.Set<PromoStatus>().Where(x => x.SystemName == "Started" && !x.Disabled).FirstOrDefault().Id;

                            _stateContext.Model.PromoStatusId = startedPromoStatusId;
                            _stateContext.State = _stateContext._startedState;

                            return true;
                        }
                        else
                        {
                            return false;
                        }
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
