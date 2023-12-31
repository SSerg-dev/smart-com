﻿using System;
using System.Linq;
using System.Collections.Generic;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl.RoleStateMap;
using Core.Settings;
using Core.Dependency;
using Module.Persist.TPM.Utils;
using NLog;

namespace Module.Persist.TPM.PromoStateControl
{
    public partial class PromoStateContext
    {
        public class PlannedState : IPromoState
        {
            private readonly PromoStateContext _stateContext;

            private static readonly Logger logger = LogManager.GetCurrentClassLogger();

            private readonly string Name = "Planned";

            private readonly List<string> Roles = new List<string> { "System", "Administrator", "CMManager", "CustomerMarketing", "FunctionalExpert", "KeyAccountManager", "DemandPlanning", "DemandFinance" };

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
                logger.Trace($"ChangeState{Name} check began with user: {userRole}, promo status: {promoModel.PromoStatus.SystemName}");
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

                logger.Trace($"IsParametersChanged: {PromoStatusHelper.IsParametersChanged(promoModel, _stateContext.Model, stateIdVP, stateIdTPR)}");
                logger.Trace($"IsDispatchChanged: {PromoStatusHelper.IsDispatchChanged(isCorrectDispatchDifference, promoModel, _stateContext.Model)}");
                // Условия для возврата
                if ((PromoStatusHelper.IsParametersChanged(promoModel, _stateContext.Model, stateIdVP, stateIdTPR) ||
                    PromoStatusHelper.IsDispatchChanged(isCorrectDispatchDifference, promoModel, _stateContext.Model)
                    && userRole != "SupportAdministrator"))
                {
                    logger.Trace($"Promo status changed to DraftPublished. Marked SendForApproval");
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

                logger.Trace($"isAvailable: {isAvailable}");
                if (isAvailable)
                {
                    // Go to: StartedState (by Support Animistrator)
                    // для этой роли не производится никаких проверок на даты
                    if (statusName == "Started")
                    {
                        logger.Trace($"Status change check returned true as status is Started");
                        _stateContext.Model = promoModel;
                        _stateContext.State = _stateContext._startedState;

                        return true;
                    }
                    else if (statusName == PromoStates.OnApproval.ToString())
                    {
                        logger.Trace($"Status change check returned true as status is OnApproval");
                        var onApprovalPromoStatusId = _stateContext.dbContext.Set<PromoStatus>().Where(x => x.SystemName == PromoStates.OnApproval.ToString() && !x.Disabled).FirstOrDefault().Id;

                        promoModel.PromoStatusId = onApprovalPromoStatusId;
                        promoModel.IsCMManagerApproved = false;
                        promoModel.IsDemandPlanningApproved = false;
                        promoModel.IsDemandFinanceApproved = false;
                        promoModel.IsGAManagerApproved = false;
                        promoModel.IsAutomaticallyApproved = false;

                        _stateContext.State = _stateContext._onApprovalState;
                        _stateContext.Model = promoModel;

                        return true;
                    }
                    // Go to: DraftPublishedState
                    else
                    {
                        logger.Trace($"Status change check returned true. State context changed to draftpublished");
                        promoModel.IsCMManagerApproved = false;
                        promoModel.IsDemandPlanningApproved = false;
                        promoModel.IsDemandFinanceApproved = false;
                        promoModel.IsGAManagerApproved = false;
                        promoModel.IsAutomaticallyApproved = false;
                        _stateContext.Model = promoModel;
                        _stateContext.State = _stateContext._draftPublishedState;

                        // Если были условия для возврата, то пытаемся перевести в approval
                        if (sendForApproval)
                        {
                            logger.Trace($"Trying to send for approval");
                            PromoStatus onApprovalStatus = _stateContext.dbContext.Set<PromoStatus>().First(n => n.SystemName == "OnApproval");
                            Promo promoDraftPublished = AutomapperProfiles.PromoCopy(promoModel);

                            _stateContext.Model = promoDraftPublished;
                            promoModel.PromoStatusId = onApprovalStatus.Id;
                            return _stateContext.ChangeState(promoModel, userRole, out message);
                        }

                        return true;
                    }
                }
                else if (userRole == "SupportAdministrator")
                {
                    logger.Trace($"Status change check returned true as userRole is SupportAdmin");
                    _stateContext.Model = promoModel;
                    _stateContext.State = _stateContext.GetPromoState(statusName);

                    return true;
                }
                // Current state
                else if (isAvailableCurrent && statusName == Name)
                {
                    logger.Trace($"Status change check returned true. Change is available in current state. StatusName: {statusName}, userRole: {userRole}");
                    _stateContext.Model = promoModel;

                    return true;
                }
                else
                {
                    logger.Trace($"Status change check returned false. StatusName: {statusName}, userRole: {userRole}");
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
                        if (_stateContext.Model.StartDate <= DateTimeOffset.Now)
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
                    else if (promoState == PromoStates.OnApproval)
                    {
                        var onApprovalPromoStatusId = _stateContext.dbContext.Set<PromoStatus>().Where(x => x.SystemName == PromoStates.OnApproval.ToString() && !x.Disabled).FirstOrDefault().Id;

                        _stateContext.Model.PromoStatusId = onApprovalPromoStatusId;
                        _stateContext.Model.IsCMManagerApproved = false;
                        _stateContext.Model.IsDemandPlanningApproved = false;
                        _stateContext.Model.IsDemandFinanceApproved = false;
                        _stateContext.Model.IsGAManagerApproved = false;
                        _stateContext.Model.IsAutomaticallyApproved = false;
                        _stateContext.State = _stateContext._onApprovalState;

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
                    message = "Action is not available";

                    return false;
                }
            }
        }
    }
}
