using System;
using System.Collections.Generic;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl.RoleStateMap;

namespace Module.Persist.TPM.PromoStateControl
{
    public partial class PromoStateContext
    {
        public class UndefinedState : IPromoState
        {
            private readonly PromoStateContext _stateContext;

            private readonly string Name = "Undefined";

            private readonly List<string> Roles = new List<string> { "Administrator", "CustomerMarketing", "DemandFinance", "DemandPlanning", "FunctionalExpert", "KeyAccountManager" };

            public UndefinedState(PromoStateContext stateContext)
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

                PromoStatus promoStatus = _stateContext.dbContext.Set<PromoStatus>().Find(promoModel.PromoStatusId);
                string statusName = promoStatus.SystemName;

                bool isAvailable = PromoStateUtil.CheckAccess(GetAvailableStates(), statusName, userRole);

                if (isAvailable)
                {
                    // Go to: DraftState
                    if (statusName == "Draft")
                    {
                        _stateContext.Model = promoModel;
                        _stateContext.State = _stateContext._draftState;

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
