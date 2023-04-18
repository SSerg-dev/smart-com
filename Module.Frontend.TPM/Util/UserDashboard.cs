using Core.Security;
using Core.Security.Models;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Interfaces;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Newtonsoft.Json;
using Persist;
using Persist.Model;
using Persist.ScriptGenerator.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace Module.Frontend.TPM.Util
{
    public static class UserDashboard
    {

        private static IQueryable<PromoGridView> GetConstraintedQueryPromo(IAuthorizationManager authorizationManager, DatabaseContext Context)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            bool canChangeStateOnly = false;
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<PromoGridView> query = Context.Set<PromoGridView>().AsNoTracking();
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, TPMmode.Current, filters, FilterQueryModes.Active, canChangeStateOnly ? role : String.Empty);

            // Не администраторы не смотрят чужие черновики
            if (role != "Administrator")
            {
                query = query.Where(e => e.PromoStatusSystemName != "Draft" || e.CreatorId == user.Id);
            }
            return query;
        }
        private static IQueryable<BTL> GetBTLConstraintedQuery(IAuthorizationManager authorizationManager, DatabaseContext Context)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IQueryable<BTL> query = Context.Set<BTL>().Where(e => !e.Disabled);

            return query;
        }
        private static IQueryable<PromoSupport> GetConstraintedQueryPromoSupport(IAuthorizationManager authorizationManager, DatabaseContext Context)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();

            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<PromoSupport> query = Context.Set<PromoSupport>().Where(e => !e.Disabled);
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();

            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

            return query;
        }
        private static IQueryable<NonPromoSupport> GetConstraintedQueryNonPromoSupport(IAuthorizationManager authorizationManager, DatabaseContext Context)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<NonPromoSupport> query = Context.Set<NonPromoSupport>().Where(e => !e.Disabled);
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);
            return query;
        }
        public static string GetKeyAccountManagerCount(IAuthorizationManager authorizationManager, DatabaseContext Context)
        {
            var promo = GetConstraintedQueryPromo(authorizationManager, Context);
            var promoSupport = GetConstraintedQueryPromoSupport(authorizationManager, Context);
            var nonPromoSupport = GetConstraintedQueryNonPromoSupport(authorizationManager, Context);
            var calculateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).GetValueOrDefault().AddHours(48d);
            var nowDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).GetValueOrDefault();
            var nowDateMinusDay = nowDate.AddDays(-1);
            //timecritical
            var timeCritical = promo.Where(p => (p.PromoStatusName.Equals("Draft(published)") && p.StartDate < calculateDate) ||
            (p.PromoStatusName.Equals("On Approval") && p.DispatchesStart < calculateDate) ||
            (p.PromoStatusName.Equals("Approved") && p.StartDate < calculateDate)).Count();
            //ToApporval
            calculateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).GetValueOrDefault().AddDays(61d);
            var toApporval = promo.Where(p => p.PromoStatusName.Equals("Draft(published)") && p.DispatchesStart < calculateDate).Count();
            //ToPlan 
            calculateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).GetValueOrDefault().AddDays(7 * 4);
            var toPlan = promo.Where(p => (p.PromoStatusName.Equals("Approved") && p.StartDate <= calculateDate)).Count();
            //UploadActuals
            var uploadActuals = promo.Where(p => p.PromoStatusName.Equals("Finished") && (p.ActualPromoLSVByCompensation == 0 || p.ActualPromoLSVByCompensation == null)).Count();
            // PromoToClose 
            calculateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).GetValueOrDefault().AddDays(-7 * 8);
            var promoToClose = promo.Where(p => p.PromoStatusName.Equals("Finished") && p.EndDate < calculateDate &&
                ((p.InOut == false && p.ActualPromoUpliftPercent != null && p.ActualPromoUpliftPercent != 0 && p.ActualPromoBaselineLSV != null && p.ActualPromoBaselineLSV != 0 &&
                p.ActualPromoIncrementalLSV != null && p.ActualPromoIncrementalLSV != 0 && p.ActualPromoLSV != null && p.ActualPromoLSV != 0 &&
                p.ActualPromoLSVByCompensation != null && p.ActualPromoLSVByCompensation != 0) ||
                (p.InOut == true && p.ActualPromoIncrementalLSV != null && p.ActualPromoIncrementalLSV != 0 && p.ActualPromoLSV != null && p.ActualPromoLSV != 0 &&
                p.ActualPromoLSVByCompensation != null && p.ActualPromoLSVByCompensation != 0)))
                .Count();
            //PromoTICost 
            var dayEnd = new DateTime(nowDateMinusDay.Year, nowDateMinusDay.Month, nowDateMinusDay.Day, 23, 59, 59);
            var promoTICost = promoSupport.Where(p => (p.ActualCostTE == 0 || p.ActualCostTE == null) && p.EndDate <= dayEnd).Count();
            //NonPromoTICost
            var nonPromoTICost = nonPromoSupport.Where(p => (p.ActualCostTE == 0 || p.ActualCostTE == null) && p.EndDate <= dayEnd).Count();

            return JsonConvert.SerializeObject(new { TimeCritical = timeCritical, ToApporval = toApporval, ToPlan = toPlan, UploadActuals = uploadActuals, PromoToClose = promoToClose, PromoTICost = promoTICost, NonPromoTICost = nonPromoTICost });
        }
        public static string GetDemandPlanningCount(IAuthorizationManager authorizationManager, DatabaseContext Context)
        {
            var promo = GetConstraintedQueryPromo(authorizationManager, Context);
            var calculateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).GetValueOrDefault().AddDays(7d);
            var nowDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).GetValueOrDefault();
            //timecritical
            var timeCritical = promo.Where(p => (p.PromoStatusName.Equals("On Approval") && p.IsCMManagerApproved == true && (p.IsDemandPlanningApproved == false || p.IsDemandPlanningApproved == null) && p.DispatchesStart < calculateDate)).Count();
            //OnApporval
            calculateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).GetValueOrDefault().AddDays(7 * 9);

            var onApporval = promo.Where(p => (p.PromoStatusName.Equals("On Approval") && p.IsCMManagerApproved == true && (p.IsDemandPlanningApproved == false || p.IsDemandPlanningApproved == null) && p.DispatchesStart < calculateDate) && (!p.IsGrowthAcceleration && !p.IsInExchange)).Count();
            //GaInExchangeApproval
            var gaInExchangeApproval = promo.Where(p => (p.PromoStatusName.Equals("On Approval") && p.IsCMManagerApproved == true && (p.IsDemandPlanningApproved == false || p.IsDemandPlanningApproved == null) && p.DispatchesStart < calculateDate && (p.IsInExchange || p.IsGrowthAcceleration))).Count();
            //AdjustData
            var adjustData = promo.Where(p => (p.PromoStatusName.Equals("On Approval") && p.IsCMManagerApproved == true && (p.IsDemandPlanningApproved == false || p.IsDemandPlanningApproved == null) && (((p.InOut == false) && (p.PlanPromoLSV == 0 || p.PlanPromoLSV == null ||
                            p.PlanPromoBaselineLSV == 0 || p.PlanPromoBaselineLSV == null ||
                            p.PlanPromoUpliftPercent == 0 || p.PlanPromoUpliftPercent == null)) || ((p.InOut == true) && ((p.PlanPromoLSV == 0 || p.PlanPromoLSV == null)))))).Count();
            //UploadActuals
            calculateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).GetValueOrDefault().AddDays(-50d);

            var uploadActuals = promo.Where(p => p.PromoStatusName.Equals("Finished") &&
                                (p.ActualPromoLSV == 0 || p.ActualPromoLSV == null) &&
                                (p.EndDate < calculateDate)).Count();


            return JsonConvert.SerializeObject(new { TimeCritical = timeCritical, OnApporval = onApporval, GaInExchangeApproval = gaInExchangeApproval, AdjustData = adjustData, UploadActuals = uploadActuals });
        }
        public static string GetDemandFinanceCount(IAuthorizationManager authorizationManager, DatabaseContext Context)
        {
            var promo = GetConstraintedQueryPromo(authorizationManager, Context);
            var promoSupport = GetConstraintedQueryPromoSupport(authorizationManager, Context);
            var nonPromoSupport = GetConstraintedQueryNonPromoSupport(authorizationManager, Context);
            var calculateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).GetValueOrDefault().AddHours(48d);
            var nowDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).GetValueOrDefault();
            var nowDateMinusDay = nowDate.AddDays(-1);
            //TimeCritical
            var timeCritical = promo.Where(p => (p.PromoStatusName.Equals("On Approval") && p.IsCMManagerApproved == true && p.IsDemandPlanningApproved == true && (p.IsDemandFinanceApproved == false || p.IsDemandFinanceApproved == null) && p.DispatchesStart < calculateDate)).Count();
            //NeedsMyApproval
            calculateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).GetValueOrDefault().AddDays(7 * 9);
            var needsMyApproval = promo.Where(p => (p.PromoStatusName.Equals("On Approval") && p.IsCMManagerApproved == true && p.IsDemandPlanningApproved == true && (p.IsDemandFinanceApproved == false || p.IsDemandFinanceApproved == null) && p.DispatchesStart < calculateDate) && (!p.IsGrowthAcceleration && !p.IsInExchange)).Count();
            //GaInExchangeApproval
            var gaInExchangeApproval = promo.Where(p => (p.PromoStatusName.Equals("On Approval") && p.IsCMManagerApproved == true && p.IsDemandPlanningApproved == true && (p.IsDemandFinanceApproved == false || p.IsDemandFinanceApproved == null) && p.DispatchesStart < calculateDate && (p.IsInExchange || p.IsGrowthAcceleration))).Count();
            //ActualShopperTI
            var actualShopperTI = promo.Where(p => (p.PromoStatusName.Equals("Finished") && (p.ActualPromoTIShopper == 0 || p.ActualPromoTIShopper == null))).Count();
            //PromoTICost
            var dayEnd = new DateTime(nowDateMinusDay.Year, nowDateMinusDay.Month, nowDateMinusDay.Day, 23, 59, 59);
            var promoTICost = promoSupport.Where(p => (p.ActualCostTE == 0 || p.ActualCostTE == null) && p.EndDate <= dayEnd).Count();
            //NonPromoTICost
            var nonPromoTICost = nonPromoSupport.Where(p => (p.ActualCostTE == 0 || p.ActualCostTE == null) && p.EndDate <= dayEnd).Count();

            var demandPlanningPlan = promo.Where(p => (p.PromoStatusName.Equals("On Approval") && (((p.InOut == false) && (p.PlanPromoLSV == 0 || p.PlanPromoLSV == null ||
                            p.PlanPromoBaselineLSV == 0 || p.PlanPromoBaselineLSV == null ||
                            p.PlanPromoUpliftPercent == 0 || p.PlanPromoUpliftPercent == null)) || ((p.InOut == true) && ((p.PlanPromoLSV == 0 || p.PlanPromoLSV == null)))))).Count();
            //DemandPlanningFact
            calculateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).GetValueOrDefault().AddDays(-50d);
            var demandPlanningFact = promo.Where(p => p.PromoStatusName.Equals("Finished") &&
                                (p.ActualPromoLSV == 0 || p.ActualPromoLSV == null) &&
                                (p.EndDate < calculateDate)).Count();
            return JsonConvert.SerializeObject(new { TimeCritical = timeCritical, NeedsMyApproval = needsMyApproval, GaInExchangeApproval = gaInExchangeApproval, ActualShopperTI = actualShopperTI, PromoTICost = promoTICost, NonPromoTICost = nonPromoTICost, DemandPlanningPlan = demandPlanningPlan, DemandPlanningFact = demandPlanningFact });
        }
        public static string GetCMManagerCount(IAuthorizationManager authorizationManager, DatabaseContext Context)
        {
            var promo = GetConstraintedQueryPromo(authorizationManager, Context);
            var nonPromoSupport = GetConstraintedQueryNonPromoSupport(authorizationManager, Context);
            var calculateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).GetValueOrDefault().AddHours(48d);
            var nowDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).GetValueOrDefault();
            //TimeCritical
            var timeCritical = promo.Where(p => (p.PromoStatusName.Equals("On Approval") && (p.IsCMManagerApproved == false || p.IsCMManagerApproved == null) && p.DispatchesStart < calculateDate)).Count();
            //NeedsMyApproval
            calculateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).GetValueOrDefault().AddDays(7 * 9);
            var needsMyApproval = promo.Where(p => p.PromoStatusName.Equals("On Approval") && (p.IsCMManagerApproved == false || p.IsCMManagerApproved == null) && p.DispatchesStart < calculateDate && (!p.IsGrowthAcceleration && !p.IsInExchange)).Count();
            var gaInExchangeApproval = promo.Where(p => p.PromoStatusName.Equals("On Approval") && (p.IsCMManagerApproved == false || p.IsCMManagerApproved == null) && p.DispatchesStart < calculateDate && (p.IsInExchange || p.IsGrowthAcceleration)).Count();
            return JsonConvert.SerializeObject(new { TimeCritical = timeCritical, NeedsMyApproval = needsMyApproval, GaInExchangeApproval = gaInExchangeApproval });
        }
        public static string GetCustomerMarketingCount(IAuthorizationManager authorizationManager, DatabaseContext Context)
        {
            var promoSupport = GetConstraintedQueryPromoSupport(authorizationManager, Context);
            var promoBTLSupport = GetBTLConstraintedQuery(authorizationManager, Context);
            var nowDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).GetValueOrDefault();
            var nowDateMinusDay = nowDate.AddDays(-1);
            //ProductionCost
            var dayEnd = new DateTime(nowDateMinusDay.Year, nowDateMinusDay.Month, nowDateMinusDay.Day, 23, 59, 59);
            var productionCost = promoSupport.Where(p => (p.ActualCostTE == 0 || p.ActualCostTE == null) && p.EndDate <= dayEnd).Count();
            //BTLCost 
            var bTLCost = promoBTLSupport.Where(p => (p.ActualBTLTotal == 0 || p.ActualBTLTotal == null) && p.EndDate <= dayEnd).Count();

            return JsonConvert.SerializeObject(new { ProductionCost = productionCost, BTLCost = bTLCost });
        }
        public static string GetGAManagerCount(IAuthorizationManager authorizationManager, DatabaseContext Context)
        {
            var promoIds = GetConstraintedQueryPromo(authorizationManager, Context).Select(f => f.Id).ToList();
            IQueryable<Promo> promoes = Context.Set<Promo>().Where(g => promoIds.Contains(g.Id));
            var calculateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).GetValueOrDefault().AddHours(48d);
            var nowDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).GetValueOrDefault();
            //TimeCritical
            var inExchangeCritical = promoes.Where(x => x.MasterPromoId != null & x.DispatchesStart < calculateDate).Select(x => x.MasterPromoId).Distinct().ToList();
            var timeCritical = promoes
                .Where(p => !inExchangeCritical.Contains(p.Id) && (p.PromoStatus.Name.Equals("On Approval") && (p.IsGAManagerApproved == false || p.IsGAManagerApproved == null) && p.DispatchesStart < calculateDate) && (p.IsInExchange || p.IsGrowthAcceleration))
                .Count() + inExchangeCritical.Count();
            //GAapproval
            calculateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).GetValueOrDefault().AddDays(7 * 9);
            int gaApproval = promoes.Where(p => p.PromoStatus.Name.Equals("On Approval") && (p.IsGAManagerApproved == false || p.IsGAManagerApproved == null) && p.DispatchesStart < calculateDate && (p.IsInExchange || p.IsGrowthAcceleration)).Count();
            return JsonConvert.SerializeObject(new { TimeCritical = timeCritical, GaApproval = gaApproval });
        }
    }
}
