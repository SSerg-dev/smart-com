﻿using System;
using System.Linq;
using System.Collections.Generic;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl.RoleStateMap;

namespace Module.Persist.TPM.PromoStateControl
{
    public partial class PromoStateContext
    {
        public class StartedState : IPromoState
        {
            private readonly PromoStateContext _stateContext;

            private readonly string Name = "Started";

            private readonly List<string> Roles = new List<string> { "System", "Administrator", "CMManager", "CustomerMarketing", "DemandFinance", "DemandPlanning", "FunctionalExpert", "KeyAccountManager" };

            public StartedState(PromoStateContext stateContext)
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

                PromoStatus promoStatus = _stateContext.dbContext.Set<PromoStatus>().Find(promoModel.PromoStatusId);
                string statusName = promoStatus.SystemName;

                bool isAvailable = PromoStateUtil.CheckAccess(GetAvailableStates(), statusName, userRole);
                bool isAvailableCurrent = PromoStateUtil.CheckAccess(Roles, userRole);

                // для этой роли не производится никаких проверок
                if (userRole == "SupportAdministrator")
                {
                    _stateContext.Model = promoModel;
                    _stateContext.State = _stateContext.GetPromoState(statusName);

                    return true;
                }

                if (isAvailable)
                {
                    // Go to: FinishedState (by Support Animistrator)
                    if (statusName == "Finished")
                    {
                        if (promoModel.DispatchesEnd <= DateTimeOffset.Now)
                        {
                            _stateContext.Model = promoModel;
                            _stateContext.State = _stateContext._finishedState;

                            return true;
                        }
                        else
                        {
                            message = "Error, dispatch not ended yet";
                            _stateContext.Model = promoModel;

                            return false;
                        }
                    }
                    else if (statusName == "Cancelled")
                    {
                        _stateContext.Model = promoModel;
                        _stateContext.State = _stateContext._finishedState;

                        return true;
                    }
                    else
                    {
                        message = "Error, Go To Cancel Status From Started";
                        _stateContext.Model = promoModel;

                        return false;
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
                    // Go to: FinishedState

                    if (_stateContext.Model.EndDate < DateTimeOffset.Now)
                    {
                        Guid finishedPromoStatusId = _stateContext.dbContext.Set<PromoStatus>().Where(x => x.SystemName == "Finished" && !x.Disabled).FirstOrDefault().Id;

                        _stateContext.Model.PromoStatusId = finishedPromoStatusId;
                        _stateContext.State = _stateContext._finishedState;

                        return true;
                    }
                    else
                    {
                        return false;
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
