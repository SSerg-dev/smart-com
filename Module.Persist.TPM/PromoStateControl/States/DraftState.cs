﻿using System;
using System.Collections.Generic;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl.RoleStateMap;

namespace Module.Persist.TPM.PromoStateControl
{
    public partial class PromoStateContext
    {
        public class DraftState : IPromoState
        {
            private readonly PromoStateContext _stateContext;

            private readonly string Name = "Draft";

            private readonly List<string> Roles = new List<string> { "Administrator", "CMManager", "CustomerMarketing", "DemandFinance", "DemandPlanning", "FunctionalExpert", "KeyAccountManager" };

            public DraftState(PromoStateContext stateContext)
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

                if (isAvailable)
                {
                    // Go to: DraftPublishedState
                    if (statusName == "DraftPublished")
                    {
                        _stateContext.Model = promoModel;
                        _stateContext.State = _stateContext._draftPublishedState;

                        return true;
                    }
                    // Go to: DeletedState
                    else
                    {
                        _stateContext.Model = promoModel;
                        _stateContext.State = _stateContext._deletedState;

                        return true;
                    }
                }
                else if (userRole == "SupportAdministrator")
                {
                    _stateContext.Model = promoModel;
                    _stateContext.State = _stateContext.GetPromoState(statusName);

                    return true;
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
