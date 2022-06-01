using System;
using System.Linq;
using System.Collections.Generic;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl.RoleStateMap;
using Core.Settings;
using Core.Dependency;
using Module.Persist.TPM.Utils;

namespace Module.Persist.TPM.PromoStateControl
{
    public partial class PromoStateContext
    {
        public class OnApprovalState : IPromoState
        {
            private readonly PromoStateContext _stateContext;

            private readonly string Name = "OnApproval";

            private readonly List<string> Roles = new List<string> { "Administrator", "CMManager", "CustomerMarketing", "FunctionalExpert", "KeyAccountManager", "DemandPlanning", "DemandFinance" };

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
                        var oldIncidents = _stateContext.dbContext.Set<PromoOnApprovalIncident>().Where(x => x.PromoId == promoModel.Id && x.ProcessDate == null);
                        switch (userRole)
                        {
                            case "CMManager":
                                if (!promoModel.IsGrowthAcceleration && !promoModel.IsInExchange)
                                {
                                    promoModel.IsCMManagerApproved = true;

                                    // Закрываем все неактуальные инциденты
                                    foreach (var incident in oldIncidents)
                                    {
                                        incident.ProcessDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                                    }
                                    _stateContext.dbContext.Set<PromoOnApprovalIncident>().Add(new PromoOnApprovalIncident()
                                    {
                                        CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                                        PromoId = promoModel.Id,
                                        Promo = promoModel,
                                        ApprovingRole = "DemandPlanning"
                                    });
                                }
                                else
                                {
                                    promoModel.IsCMManagerApproved = true;

                                    next = promoModel.IsDemandFinanceApproved.HasValue && promoModel.IsDemandFinanceApproved.Value
                                            && promoModel.IsDemandPlanningApproved.HasValue && promoModel.IsDemandPlanningApproved.Value;
                                    if (!next)
                                    {
                                        // Закрываем все неактуальные инциденты
                                        foreach (var incident in oldIncidents)
                                        {
                                            incident.ProcessDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                                        }
                                        _stateContext.dbContext.Set<PromoOnApprovalIncident>().Add(new PromoOnApprovalIncident()
                                        {
                                            CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                                            PromoId = promoModel.Id,
                                            Promo = promoModel,
                                            ApprovingRole = "DemandPlanning"
                                        });
                                    }
                                }
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
                                        if (!promoModel.IsGrowthAcceleration && !promoModel.IsInExchange)
                                            next = promoModel.IsDemandPlanningApproved ?? false;
                                        // Если next = true, то финанса пропускаем
                                        if (!next)
                                        {
                                            foreach (var incident in oldIncidents)
                                            {
                                                incident.ProcessDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                                            }

                                            _stateContext.dbContext.Set<PromoOnApprovalIncident>().Add(new PromoOnApprovalIncident()
                                            {
                                                CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                                                PromoId = promoModel.Id,
                                                Promo = promoModel,
                                                ApprovingRole = "DemandFinance"
                                            });
                                        }
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
                                        if (!promoModel.IsGrowthAcceleration && !promoModel.IsInExchange)
                                            next = promoModel.IsDemandPlanningApproved ?? false;
                                        // Если next = true, то финанса пропускаем
                                        if (!next)
                                        {
                                            foreach (var incident in oldIncidents)
                                            {
                                                incident.ProcessDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                                            }
                                            _stateContext.dbContext.Set<PromoOnApprovalIncident>().Add(new PromoOnApprovalIncident()
                                            {
                                                CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                                                PromoId = promoModel.Id,
                                                Promo = promoModel,
                                                ApprovingRole = "DemandFinance"
                                            });
                                        }
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

                                    if (promoModel.IsGrowthAcceleration || promoModel.IsInExchange)
                                    {
                                        promoModel.IsCMManagerApproved = false;
                                        next = false;

                                        foreach (var incident in oldIncidents)
                                        {
                                            incident.ProcessDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                                        }
                                        _stateContext.dbContext.Set<PromoOnApprovalIncident>().Add(new PromoOnApprovalIncident()
                                        {
                                            CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                                            PromoId = promoModel.Id,
                                            Promo = promoModel,
                                            ApprovingRole = "CMManager"
                                        });
                                    }
                                }
                                break;

                            case "SupportAdministrator":
                                next = true;
                                break;

                            default: break;
                        }

                        if (next)
                        {
                            promoModel.LastApprovedDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);

                            _stateContext.State = _stateContext._approvedState;

                            var promoApprovedIncedents = _stateContext.dbContext.Set<PromoApprovedIncident>().Where(x => x.PromoId == promoModel.Id && x.ProcessDate == null);

                            if (promoApprovedIncedents.Count() > 0)
                            {
                                foreach (var incident in promoApprovedIncedents)
                                {
                                    incident.ProcessDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                                }
                            }
                            // Если промо уже перешло в Approved, то все инциденты по onApproval закрываем.
                            foreach (var incident in oldIncidents)
                            {
                                incident.ProcessDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                            }
                            _stateContext.dbContext.Set<PromoApprovedIncident>().Add(new PromoApprovedIncident()
                            {
                                CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                                Promo = promoModel,
                                PromoId = promoModel.Id
                            });
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
                    // Go to: DraftPublishedState
                    if (promoState == PromoStates.DraftPublished)
                    {
                        Guid draftPublishedPromoStatusId = _stateContext.dbContext.Set<PromoStatus>().Where(x => x.SystemName == "DraftPublished" && !x.Disabled).FirstOrDefault().Id;

                        _stateContext.Model.PromoStatusId = draftPublishedPromoStatusId;
                        _stateContext.Model.IsCMManagerApproved = false;
                        _stateContext.Model.IsDemandPlanningApproved = false;
                        _stateContext.Model.IsDemandFinanceApproved = false;
                        _stateContext.Model.IsAutomaticallyApproved = false;
                        _stateContext.State = _stateContext._draftPublishedState;

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
