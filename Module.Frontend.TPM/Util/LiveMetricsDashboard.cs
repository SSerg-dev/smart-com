using Core.MarsCalendar;
using Core.Security;
using Core.Security.Models;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Interfaces;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Newtonsoft.Json;
using Persist;
using Persist.Model;
using Persist.Model.Settings;
using Persist.ScriptGenerator.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace Module.Frontend.TPM.Util
{
    public static class LiveMetricsDashboard
    {

        public static string GetLiveMetricsDashboard(IAuthorizationManager authorizationManager, DatabaseContext Context, int ClientTreeId, string Period)
        {
            MarsDate marsDate = new MarsDate(Period);
            DateTimeOffset periodStartDate = marsDate.PeriodStartDate();
            DateTimeOffset periodEndDate = marsDate.PeriodEndDate();
            MarsDate PW4D1 = marsDate.AddWeeks(-4);
            DateTimeOffset PW4D1Date = PW4D1.StartDate();
            MarsDate dateToday = new MarsDate(DateTimeOffset.Now);
            int days = 0;
            DateTimeOffset todayEnddate = dateToday.DayEndDate();
            todayEnddate = new DateTimeOffset(todayEnddate.Year, todayEnddate.Month, todayEnddate.Day -1, 23, 59, 59, 999, new TimeSpan(0, 0, 0));

            if (dateToday.Period == marsDate.Period && dateToday.Year == marsDate.Year)
            {
                days = Convert.ToInt32((todayEnddate - periodStartDate).TotalDays);
            }
            else
            {
                days = Convert.ToInt32((periodEndDate - periodStartDate).TotalDays);
            }


            ClientTree client = Context.Set<ClientTree>().FirstOrDefault(g => g.Id == ClientTreeId);
            IEnumerable<PromoGridView> promoes = GetConstraintedQueryPromo(authorizationManager, Context, ClientTreeId);
            ModelColor metricSettings = GetMetricSettings(Context.Set<Setting>().Where(g => g.Name.Contains("METRICS_")).ToList());
            List<MetricsLiveHistory> metricsLives = Context.Set<MetricsLiveHistory>().Where(g => g.Date >= periodStartDate && g.Date <= periodEndDate && g.ClientTreeId == client.ObjectId).ToList();
            ModelReturn ppaMetric = GetPPA(promoes);
            ModelReturn pctMetric = GetPCT(promoes);
            ModelReturn padMetric = GetPAD(promoes);
            ModelReturn ppaPeriodMetric = GetPPAperiod(metricsLives, days);
            ModelReturn pctPeriodMetric = GetPCTperiod(metricsLives, PW4D1Date);
            ModelReturn psfaMetric = GetPSFA(promoes, marsDate);

            return JsonConvert.SerializeObject(new
            {
                PPA = ppaMetric.Value,
                PCT = pctMetric.Value,
                PAD = padMetric.Value,
                PADDEN = padMetric.Value2,
                PSFA = psfaMetric.Value,
                PPA_LSV = ppaMetric.ValueLSV,
                PCT_LSV = pctMetric.ValueLSV,
                PAD_LSV = padMetric.ValueLSV,
                PSFA_LSV = psfaMetric.ValueLSV,
                PPA_YELLOW = metricSettings.PPAYellow,
                PPA_GREEN = metricSettings.PPAGreen,
                PCT_YELLOW = metricSettings.PCTYellow,
                PCT_GREEN = metricSettings.PCTGreen,
                PSFA_YELLOW = metricSettings.PSFAYellow,
                PSFA_GREEN = metricSettings.PSFAGreen,
                PAD_MIN = metricSettings.PADMin,
                PPA_PERIOD_YELLOW = metricSettings.PPAPeriodYellow,
                PPA_PERIOD_GREEN = metricSettings.PPAPeriodGreen,
                PCT_PERIOD_YELLOW = metricSettings.PCTPeriodYellow,
                PCT_PERIOD_GREEN = metricSettings.PCTPeriodGreen,
                PPA_PERIOD = ppaPeriodMetric.Value,
                PCT_PERIOD = pctPeriodMetric.Value,
                PPA_PERIOD_LSV = ppaPeriodMetric.ValueLSV,
                PCT_PERIOD_LSV = pctPeriodMetric.ValueLSV,
            });
        }

        public static ModelReturn GetPPA(IEnumerable<PromoGridView> promoes)
        {
            var readyStatuses = new string[] { "Approved", "Planned" };
            var negativeStatuses = new string[] { "On Approval", "Draft(published)" };
            var notCheckStatuses = new string[] { "Draft", "Cancelled", "Deleted", "Started", "Finished", "Closed" };

            var endDate = DateTime.Now.AddDays(7 * 8);

            var filteredPromoes = promoes.Where(x => x.DispatchesStart <= endDate && x.DispatchesStart >= DateTime.Now);

            var readyPromoes = filteredPromoes.Count(x => readyStatuses.Contains(x.PromoStatusName));
            var allPromoes = filteredPromoes.Count(x => !notCheckStatuses.Contains(x.PromoStatusName));

            if (allPromoes > 0)
            {
                var ppa = (double)readyPromoes / allPromoes;
                var ppaLsv = filteredPromoes.Where(x => negativeStatuses.Contains(x.PromoStatusName)).Sum(x => x.PlanPromoLSV);

                return new ModelReturn
                {
                    Value = Math.Truncate(ppa * 100),
                    ValueLSV = Math.Round(ppaLsv.Value, 2, MidpointRounding.AwayFromZero),
                    ValueReal = ppa,
                    ValueLSVReal = (double)ppaLsv
                };
            }
            else
            {
                return new ModelReturn { Value = 0, ValueLSV = 0, ValueReal = 0, ValueLSVReal = 0 };
            }
        }
        public static ModelReturn GetPCT(IEnumerable<PromoGridView> promoes)
        {
            var checkStatuses = new string[] { "Closed", "Finished" };

            var endDate = DateTime.Now.AddDays(-7 * 7);
            var startDate = new DateTime(endDate.Year, 1, 1);

            var filteredPromoes = promoes.Where(x => x.EndDate >= startDate && x.EndDate <= endDate);

            var closedPromoes = filteredPromoes.Count(x => x.PromoStatusName == "Closed");
            var allCheckPromoes = filteredPromoes.Count(x => checkStatuses.Contains(x.PromoStatusName));

            if (allCheckPromoes > 0)
            {
                var pct = (double)closedPromoes / allCheckPromoes;
                var pctLsv = filteredPromoes.Where(x => x.PromoStatusName == "Finished").Sum(x => x.PlanPromoLSV);

                return new ModelReturn
                {
                    Value = Math.Truncate(pct * 100),
                    ValueLSV = Math.Round(pctLsv.Value, 2, MidpointRounding.AwayFromZero),
                    ValueReal = pct,
                    ValueLSVReal = (double)pctLsv
                };
            }
            else
            {
                return new ModelReturn { Value = 0, ValueLSV = 0, ValueReal = 0, ValueLSVReal = 0 };
            }
        }

        private static ModelReturn GetPAD(IEnumerable<PromoGridView> promoes)
        {
            var checkStatuses = new string[] { "Closed", "Finished" };

            var endDate = DateTime.Now.AddDays(-7 * 7);
            var startDate = new DateTime(endDate.Year, 1, 1);

            var filteredPromoes = promoes.Where(x =>
                                    x.EndDate >= startDate && x.EndDate <= endDate
                                    && checkStatuses.Contains(x.PromoStatusName)
                                    && x.ActualPromoLSV != null && x.ActualPromoLSV != 0
                                    && x.ActualPromoLSVByCompensation != null && x.ActualPromoLSVByCompensation != 0);
            var total = filteredPromoes.Count();
            filteredPromoes = filteredPromoes.Where(x => x.ActualPromoLSVdiffPercent > 0.1);
            var padLsv = filteredPromoes.Select(x => new { ActualPromoLSV = x.ActualPromoLSV.Value, ActualPromoLSVByCompensation = x.ActualPromoLSVByCompensation.Value }).Sum(x => Math.Abs(x.ActualPromoLSV - x.ActualPromoLSVByCompensation));

            return new ModelReturn { Value = filteredPromoes.Count(), Value2 = total, ValueLSV = Math.Round(padLsv, 2, MidpointRounding.AwayFromZero) };
        }
        private static ModelReturn GetPPAperiod(List<MetricsLiveHistory> metricsLives, int days)
        {
            var metricsLivesPPA = metricsLives.Where(g => g.Type == TypeMetrics.PPA).ToList();
            if (metricsLivesPPA.Count > 0)
            {
                var ppa = metricsLivesPPA.Sum(g => g.Value) / days;
                var ppaLsv = metricsLivesPPA.Sum(g => g.ValueLSV) / days;

                return new ModelReturn { Value = Math.Truncate(ppa * 100), ValueLSV = Math.Round(ppaLsv, 2, MidpointRounding.AwayFromZero) };
            }
            else
            {
                return new ModelReturn { Value = 0, ValueLSV = 0 };
            }
        }
        private static ModelReturn GetPCTperiod(List<MetricsLiveHistory> metricsLives, DateTimeOffset PW4D1Date)
        {
            MetricsLiveHistory liveHistory = metricsLives.FirstOrDefault(g => g.Date >= PW4D1Date && g.Date <= PW4D1Date.AddDays(1) && g.Type == TypeMetrics.PCT);

            if (liveHistory != null)
            {
                var pct = liveHistory.Value;
                var pctLsv = liveHistory.ValueLSV;

                return new ModelReturn { Value = Math.Truncate(pct * 100), ValueLSV = Math.Round(pctLsv, 2, MidpointRounding.AwayFromZero) };
            }
            else
            {
                return new ModelReturn { Value = 0, ValueLSV = 0 };
            }
        }
        private static ModelReturn GetPSFA(IEnumerable<PromoGridView> promoes, MarsDate marsDate)
        {
            var checkStatuses = new string[] { "Closed" };

            var startDate = marsDate.PeriodStartDate();
            var endDate = marsDate.PeriodEndDate();

            var filteredPromoes = promoes.Where(x =>
                                    checkStatuses.Contains(x.PromoStatusName)
                                    && x.ActualPromoIncrementalLSV != null && x.ActualPromoIncrementalLSV != 0
                                    && x.PlanPromoIncrementalLSV != null && x.PlanPromoIncrementalLSV != 0
                                    && x.StartDate >= startDate && x.StartDate <= endDate);

            if (filteredPromoes.Count() > 0)
            {
                var sfaLsv = Math.Abs(filteredPromoes.Sum(x => x.ActualPromoIncrementalLSV.Value) - filteredPromoes.Sum(x => x.PlanPromoIncrementalLSVRaw.Value));
                var sfa = sfaLsv / filteredPromoes.Sum(x => x.PlanPromoIncrementalLSVRaw.Value);
                sfa = (1 - sfa) * 100;

                return new ModelReturn { Value = Math.Truncate(sfa), ValueLSV = Math.Round(sfaLsv, 2, MidpointRounding.AwayFromZero) };
            }
            else
            {
                return new ModelReturn { Value = 0, ValueLSV = 0 };
            }
        }

        private static IEnumerable<PromoGridView> GetConstraintedQueryPromo(IAuthorizationManager authorizationManager, DatabaseContext Context, int ClientTreeId)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            bool canChangeStateOnly = false;
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            ClientTree client = Context.Set<ClientTree>().FirstOrDefault(g => g.Id == ClientTreeId);
            IQueryable<PromoGridView> query = Context.Set<PromoGridView>()
                .AsNoTracking()
                .Where(g => g.ClientHierarchy.Contains(client.FullPathName));
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, TPMmode.Current, filters, FilterQueryModes.Active, canChangeStateOnly ? role : String.Empty);
            query = query.Where(x => !x.IsOnHold);
            // Не администраторы не смотрят чужие черновики
            if (role != "Administrator")
            {
                query = query.Where(e => e.PromoStatusSystemName != "Draft" || e.CreatorId == user.Id);
            }
            return query.ToList();
        }
        private static ModelColor GetMetricSettings(List<Setting> settings)
        {
            return new ModelColor
            {
                PPAGreen = int.Parse(settings.FirstOrDefault(g => g.Name.Contains("METRICS_PPA_GREEN")).Value),
                PPAYellow = int.Parse(settings.FirstOrDefault(g => g.Name.Contains("METRICS_PPA_YELLOW")).Value),
                PCTGreen = int.Parse(settings.FirstOrDefault(g => g.Name.Contains("METRICS_PCT_GREEN")).Value),
                PCTYellow = int.Parse(settings.FirstOrDefault(g => g.Name.Contains("METRICS_PCT_YELLOW")).Value),
                PADMin = int.Parse(settings.FirstOrDefault(g => g.Name.Contains("METRICS_PAD_MIN")).Value),
                PSFAGreen = int.Parse(settings.FirstOrDefault(g => g.Name.Contains("METRICS_PSFA_GREEN")).Value),
                PSFAYellow = int.Parse(settings.FirstOrDefault(g => g.Name.Contains("METRICS_PSFA_YELLOW")).Value),
                PPAPeriodGreen = int.Parse(settings.FirstOrDefault(g => g.Name.Contains("METRICS_PPA_GREEN")).Value),
                PPAPeriodYellow = int.Parse(settings.FirstOrDefault(g => g.Name.Contains("METRICS_PPA_YELLOW")).Value),
                PCTPeriodGreen = int.Parse(settings.FirstOrDefault(g => g.Name.Contains("METRICS_PCT_GREEN")).Value),
                PCTPeriodYellow = int.Parse(settings.FirstOrDefault(g => g.Name.Contains("METRICS_PCT_YELLOW")).Value),
            };
        }
        public class ModelReturn
        {
            public double Value { get; set; }
            public int Value2 { get; set; }
            public double ValueLSV { get; set; }
            public double ValueReal { get; set; }
            public double ValueLSVReal { get; set; }
        }
        public class ModelColor
        {
            public int PPAGreen { get; set; }
            public int PPAYellow { get; set; }
            public int PCTGreen { get; set; }
            public int PCTYellow { get; set; }
            public int PADMin { get; set; }
            public int PSFAGreen { get; set; }
            public int PSFAYellow { get; set; }
            public int PPAPeriodGreen { get; set; }
            public int PPAPeriodYellow { get; set; }
            public int PCTPeriodGreen { get; set; }
            public int PCTPeriodYellow { get; set; }
        }
    }
}
