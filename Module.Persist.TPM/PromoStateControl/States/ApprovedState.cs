using System;
using System.Linq;
using System.Collections.Generic;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl.RoleStateMap;
using Module.Persist.TPM.Utils;
using Persist.Utils;
using Core.Settings;
using Core.Dependency;

namespace Module.Persist.TPM.PromoStateControl
{
    public partial class PromoStateContext
    {
        public class ApprovedState : IPromoState
        {
            private readonly PromoStateContext _stateContext;

            private readonly string Name = "Approved";

            private readonly List<string> Roles = new List<string> { "Administrator", "CMManager", "CustomerMarketing", "FunctionalExpert", "KeyAccountManager", "DemandPlanning", "DemandFinance" };

            public ApprovedState(PromoStateContext stateContext)
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

                List<Guid> stateIdVP = _stateContext.dbContext.Set<Mechanic>().Where(x => x.SystemName == "VP").Select(x => x.Id).ToList();
                List<Guid> stateIdTPR = _stateContext.dbContext.Set<Mechanic>().Where(x => x.SystemName == "TPR").Select(x => x.Id).ToList();

                bool isAvailable;
                bool isAvailableCurrent = PromoStateUtil.CheckAccess(Roles, userRole);

                ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                var backToOnApprovalDispatchDays = settingsManager.GetSetting<int>("BACK_TO_ON_APPROVAL_DISPATCH_DAYS_COUNT", 7 * 8);
                bool isCorrectDispatchDifference = (promoModel.DispatchesStart - ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)).Value.Days >= backToOnApprovalDispatchDays;

                // Условия для возврата
                if ((PromoStatusHelper.IsParametersChanged(promoModel, _stateContext.Model, stateIdVP, stateIdTPR) ||
                    PromoStatusHelper.IsDispatchChanged(isCorrectDispatchDifference, promoModel, _stateContext.Model)
                    && userRole != "SupportAdministrator"))
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
                            Promo promoDraftPublished = AutomapperProfiles.PromoCopy(promoModel);

                            _stateContext.Model = promoDraftPublished;
                            promoModel.PromoStatusId = onApprovalStatus.Id;
                            return _stateContext.ChangeState(promoModel, "System", out message);
                        }

                        return true;
                    }
                    else if (statusName == "Planned")
                    {
                        // Go to: PlannedState

                        _stateContext.Model = promoModel;
                        _stateContext.State = _stateContext._plannedState;

                        return true;
                    }
                    else if (statusName == PromoStates.OnApproval.ToString())
                    {
                        var onApprovalPromoStatusId = _stateContext.dbContext.Set<PromoStatus>().Where(x => x.SystemName == PromoStates.OnApproval.ToString() && !x.Disabled).FirstOrDefault().Id;

                        promoModel.PromoStatusId = onApprovalPromoStatusId;
                        promoModel.IsCMManagerApproved = false;
                        promoModel.IsDemandPlanningApproved = false;
                        promoModel.IsDemandFinanceApproved = false;
                        promoModel.IsAutomaticallyApproved = false;

						_stateContext.State = _stateContext._onApprovalState;
                        _stateContext.Model = promoModel;

                        return true; 
                    }
                    else 
                    {
                        // Go to: CancelledState

                        _stateContext.Model = promoModel;
                        _stateContext.State = _stateContext._cancelledState;

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
				bool isAvailable = false;
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
					// Go to: Cancelled
					if (promoState == PromoStates.Cancelled)
					{
						Guid cancelledPromoStatusId = _stateContext.dbContext.Set<PromoStatus>().Where(x => x.SystemName == "Cancelled" && !x.Disabled).FirstOrDefault().Id;

						_stateContext.Model.PromoStatusId = cancelledPromoStatusId;
						_stateContext.State = _stateContext._cancelledState;

						return true;
					}
					else if (promoState == PromoStates.OnApproval)
                    {
                        var onApprovalPromoStatusId = _stateContext.dbContext.Set<PromoStatus>().Where(x => x.SystemName == PromoStates.OnApproval.ToString() && !x.Disabled).FirstOrDefault().Id;

                        _stateContext.Model.PromoStatusId = onApprovalPromoStatusId;
                        _stateContext.Model.IsCMManagerApproved = false;
                        _stateContext.Model.IsDemandPlanningApproved = false;
                        _stateContext.Model.IsDemandFinanceApproved = false;
                        _stateContext.Model.IsAutomaticallyApproved = false;
						_stateContext.State = _stateContext._onApprovalState;

                        return true; 
                    }
                    // Go to: DeletedState
                    else if (promoState == PromoStates.Deleted && promoModel.MasterPromoId != null)
                    {
                        return true;
                    }
                    else
					{
						message = $"Action for status {promoState.ToString()} in not implemented";

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
