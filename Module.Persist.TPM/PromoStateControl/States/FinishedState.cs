using System;
using System.Collections.Generic;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl.RoleStateMap;

namespace Module.Persist.TPM.PromoStateControl
{
    public partial class PromoStateContext
    {
        public class FinishedState : IPromoState
        {
            private readonly PromoStateContext _stateContext;

            private readonly string Name = "Finished";

            private readonly List<string> Roles = new List<string> { "System", "Administrator", "CMManager", "CustomerMarketing", "DemandFinance", "DemandPlanning", "FunctionalExpert", "KeyAccountManager" };

            public FinishedState(PromoStateContext stateContext)
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
                    message = CheckPromoForErrors(promoModel);
                    // Go to: closedState
                    if (message != null && userRole != "SupportAdministrator")
                    {
                        return false;
                    }
                    else
                    {
                        _stateContext.Model = promoModel;
                        _stateContext.State = _stateContext._closedState;

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

            private string CheckPromoForErrors(Promo promoModel)
            {
                string message = null;

                if (!promoModel.ActualPromoLSV.HasValue || promoModel.ActualPromoLSV.Value <= 0)
                {
                    message += "Actual Promo LSV must be greater than 0. \r\n";
                }

                if (!promoModel.ActualPromoLSVSO.HasValue || promoModel.ActualPromoLSVSO.Value <= 0)
                {
                    message += "Actual Promo LSV SO must be greater than 0. \r\n";
                }

                if (promoModel.ActualPromoTIMarketing.HasValue && promoModel.ActualPromoTIMarketing.Value < 0)
                {
                    message += "Actual Marketing TI must be greater or equal 0. \r\n";
                }

                if (promoModel.ActualPromoCostProduction.HasValue && promoModel.ActualPromoCostProduction.Value < 0)
                {
                    message += "Actual Promo Cost Production must be greater or equal 0. \r\n";
                }

                return message;
            }
        }
    }
}
