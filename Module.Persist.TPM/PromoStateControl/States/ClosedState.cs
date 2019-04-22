using System;
using System.Collections.Generic;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl.RoleStateMap;

namespace Module.Persist.TPM.PromoStateControl
{
    public partial class PromoStateContext
    {
        public class ClosedState : IPromoState
        {
            private readonly PromoStateContext _stateContext;

            private readonly string Name = "Closed";

            private readonly List<string> Roles = new List<string> { "Administrator", "CustomerMarketing", "DemandFinance", "DemandPlanning", "FunctionalExpert", "KeyAccountManager" };

            private readonly Dictionary<string, List<string>> AvailableStates = new Dictionary<string, List<string>>();

            public ClosedState(PromoStateContext stateContext)
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
                massage = "Action is not available";

                return false;
            }

            public bool ChangeState(Promo promoModel, PromoStates promoState, string userRole, out string massage)
            {
                throw new NotImplementedException();
            }
        }
    }
}
