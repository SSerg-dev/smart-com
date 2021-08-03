using System;
using System.Linq;
using System.Collections.Generic;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.PromoStateControl.RoleStateMap;
using Module.Persist.TPM.Utils;
using Core.Settings;
using Core.Dependency;
using NLog;

namespace Module.Persist.TPM.PromoStateControl
{
    public partial class PromoStateContext
    {
        public class DraftPublishedState : IPromoState
        {
            private readonly PromoStateContext _stateContext;

            private static readonly Logger logger = LogManager.GetCurrentClassLogger();

            private readonly string Name = "DraftPublished";

            private readonly List<string> Roles = new List<string> { "Administrator", "CMManager", "CustomerMarketing", "DemandFinance", "DemandPlanning", "FunctionalExpert", "KeyAccountManager" };

            public DraftPublishedState(PromoStateContext stateContext)
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
                logger.Trace($"ChangeState{Name} check began with user: {userRole}, promo status: {promoModel.PromoStatus}");
                message = string.Empty;

                PromoStatus promoStatus = _stateContext.dbContext.Set<PromoStatus>().Find(promoModel.PromoStatusId);
                string statusName = promoStatus.SystemName;

                bool isAvailable = PromoStateUtil.CheckAccess(GetAvailableStates(), statusName, userRole);
                bool isAvailableCurrent = PromoStateUtil.CheckAccess(Roles, userRole);

                logger.Trace($"isAvailable: {isAvailable}");
                logger.Trace($"isAvailableCurrent: {isAvailableCurrent}");
                if (isAvailable)
                {
                    // Go to: DraftState
                    if (statusName == "Draft")
                    {
                        logger.Trace($"Status change check returned true as status is Draft");
                        _stateContext.Model = promoModel;
                        _stateContext.State = _stateContext._draftState;

						if (userRole != "System")
                        {
                            logger.Trace($"Deleting reject incidents (user: {userRole})");
                            // Если в Draft переводит не система, то удаляем reject incident, если есть
                            var rejectIncidents = _stateContext.dbContext.Set<PromoOnRejectIncident>().Where(x => x.PromoId == promoModel.Id && !x.ProcessDate.HasValue);
							foreach (var incident in rejectIncidents)
							{
								incident.ProcessDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
							}
						}

						return true;
                    }
                    // Go to: OnApprovalState
                    else
                    {
                        logger.Trace($"Go to OnApprovalState");
                        if (userRole != "System")
                        {
                            logger.Trace($"Deleting reject incidents (user: {userRole})");
                            // Если в Draft переводит не система, то удаляем reject incident, если есть
                            var rejectIncidents = _stateContext.dbContext.Set<PromoOnRejectIncident>().Where(x => x.PromoId == promoModel.Id && !x.ProcessDate.HasValue);
							foreach (var incident in rejectIncidents)
							{
								incident.ProcessDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
							}
						}

						var oldIncidents = _stateContext.dbContext.Set<PromoOnApprovalIncident>().Where(x => x.PromoId == promoModel.Id && x.ProcessDate == null);
                        // Проверка на GA
                        if (promoModel.IsGrowthAcceleration)
                        {
                            logger.Trace($"Status change check returned true as promo is GA");

                            _stateContext.Model = promoModel;
                            _stateContext.State = _stateContext._onApprovalState;

                            // Закрываем все неактуальные инциденты
                            foreach (var incident in oldIncidents)
                            {
                                incident.ProcessDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                            }
                            _stateContext.dbContext.Set<PromoOnApprovalIncident>().Add(new PromoOnApprovalIncident()
                            {
                                PromoId = promoModel.Id,
                                ApprovingRole = "CMManager", // Или DemandPlanning? Артём должен уточнить
                                CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                                Promo = promoModel
                            });
                            return true;
                        }

						// Проверка на NoNego
						bool isNoNego = PromoStatusHelper.CheckNoNego(promoModel, _stateContext.dbContext);
                        if (isNoNego)
                        {
                            logger.Trace($"isNoNego: {isNoNego}");
                            ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                            var toApprovedDispatchDays = settingsManager.GetSetting<int>("TO_APPROVED_DISPATCH_DAYS_COUNT", 7 * 8);
                            bool isCorrectDispatchDifference = (promoModel.DispatchesStart - ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)).Value.Days >= toApprovedDispatchDays;

                            if (!promoModel.InOut.HasValue || !promoModel.InOut.Value)
                            {
                                if ((isCorrectDispatchDifference || userRole == UserRoles.FunctionalExpert.ToString())
                                    && promoModel.PlanPromoBaselineLSV.HasValue && promoModel.PlanPromoBaselineLSV > 0
                                    && promoModel.PlanPromoUpliftPercent.HasValue && promoModel.PlanPromoUpliftPercent > 0)
                                {
                                    logger.Trace($"Status change check returned true. Promo status changed to OnApproval.");
                                    logger.Trace($"PlanPromoBaselineLSV is null:{promoModel.PlanPromoBaselineLSV.HasValue}, PlanPromoUpliftPercent is null:{promoModel.PlanPromoUpliftPercent.HasValue}. User : {userRole}. isCorrectDispatchDifference : {isCorrectDispatchDifference}");
                                    PromoStatus approvedStatus = _stateContext.dbContext.Set<PromoStatus>().FirstOrDefault(e => e.SystemName == "Approved");
                                    promoModel.PromoStatusId = approvedStatus.Id;
                                    promoModel.IsAutomaticallyApproved = true;

                                    _stateContext.Model = promoModel;
                                    _stateContext.State = _stateContext._approvedState;

                                    return true;
                                }
                                else
                                {
                                    promoModel.IsCMManagerApproved = true;
                                    promoModel.IsDemandFinanceApproved = true;
                                    _stateContext.Model = promoModel;
                                    _stateContext.State = _stateContext._onApprovalState;

                                    logger.Trace($"Deleting reject incidents.");
                                    // Закрываем все неактуальные инциденты
                                    foreach (var incident in oldIncidents)
									{
										incident.ProcessDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
									}
									_stateContext.dbContext.Set<PromoOnApprovalIncident>().Add(new PromoOnApprovalIncident()
									{
										PromoId = promoModel.Id,
										ApprovingRole = "DemandPlanning",
										CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
										Promo = promoModel
									});
									return true;
                                }
                            }
                            else
                            {
                                if ((isCorrectDispatchDifference || userRole == UserRoles.FunctionalExpert.ToString())
                                    && promoModel.PlanPromoIncrementalLSV.HasValue && promoModel.PlanPromoIncrementalLSV.Value > 0)
                                {
                                    logger.Trace($"Status change check returned true. Promo status changed to Approved.");
                                    logger.Trace($"PlanPromoIncrementalLSV not null, PlanPromoIncrementalLSV > 0. User : {userRole}. isCorrectDispatchDifference : {isCorrectDispatchDifference}");
                                    PromoStatus approvedStatus = _stateContext.dbContext.Set<PromoStatus>().FirstOrDefault(e => e.SystemName == "Approved");
                                    promoModel.PromoStatusId = approvedStatus.Id;
                                    promoModel.IsAutomaticallyApproved = true;

                                    _stateContext.Model = promoModel;
                                    _stateContext.State = _stateContext._approvedState;

                                    return true;
                                }
                                else
                                {
                                    logger.Trace($"Status change check returned true. Promo status changed to OnApproval.");
                                    logger.Trace($"PlanPromoIncrementalLSV is null:{promoModel.PlanPromoIncrementalLSV.HasValue}. User : {userRole}. isCorrectDispatchDifference : {isCorrectDispatchDifference}");
                                    promoModel.IsCMManagerApproved = true;
                                    promoModel.IsDemandFinanceApproved = true;
                                    _stateContext.Model = promoModel;
                                    _stateContext.State = _stateContext._onApprovalState;

									foreach (var incident in oldIncidents)
									{
										incident.ProcessDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
									}
									_stateContext.dbContext.Set<PromoOnApprovalIncident>().Add(new PromoOnApprovalIncident()
									{
										PromoId = promoModel.Id,
										ApprovingRole = "DemandPlanning",
										CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
										Promo = promoModel
									});
									return true;
                                }
                            }
                        }
                        else
                        {
                            logger.Trace($"Deleting reject incidents.");
                            logger.Trace($"Status change check returned true (isNonego false)");
                            foreach (var incident in oldIncidents)
							{
								incident.ProcessDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
							}
							_stateContext.dbContext.Set<PromoOnApprovalIncident>().Add(new PromoOnApprovalIncident(){
								PromoId = promoModel.Id,
								ApprovingRole = "CMManager",
								CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
								Promo = promoModel
							});

							_stateContext.Model = promoModel;
                            _stateContext.State = _stateContext._onApprovalState;

							return true;
                        }
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
                    logger.Trace($"Status change check returned true. Change is available in current state. StatusName: {Name}, userRole: {userRole}");
                    _stateContext.Model = promoModel;

                    return true;
                }
                else
                {
                    logger.Trace($"Status change check returned false. StatusName: {Name}, userRole: {userRole}");
                    message = "Action is not available";

                    return false;
                }
            }

            public bool ChangeState(Promo promoModel, PromoStates promoState, string userRole, out string message)
            {
                bool isAvailable=false;
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
                    // Go to: DraftState
                    if (promoState == PromoStates.Draft)
                    {
                        Guid draftPromoStatusId = _stateContext.dbContext.Set<PromoStatus>().Where(x => x.SystemName == "Draft" && !x.Disabled).FirstOrDefault().Id;

                        _stateContext.Model.PromoStatusId = draftPromoStatusId;
                        _stateContext.Model.NeedRecountUplift = true;
                        _stateContext.State = _stateContext._draftState;

						if (userRole != "System")
						{
							// Если в Draft переводит не система, то удаляем reject incident, если есть
							var rejectIncidents = _stateContext.dbContext.Set<PromoOnRejectIncident>().Where(x => x.PromoId == promoModel.Id && !x.ProcessDate.HasValue);
							foreach (var incident in rejectIncidents)
							{
								incident.ProcessDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
							}
						}

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
