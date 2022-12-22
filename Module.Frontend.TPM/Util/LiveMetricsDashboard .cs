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
using Persist.ScriptGenerator.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace Module.Frontend.TPM.Util
{
    public static class LiveMetricsDashboard
    {

        public static string GetLiveMetricsDashboard(IAuthorizationManager authorizationManager, DatabaseContext Context, int ClientTreeId, long Period)
        {
            MarsDate marsDate = new MarsDate(Period);
            DateTimeOffset periodStartDate = marsDate.PeriodStartDate();
            DateTimeOffset periodEndDate = marsDate.PeriodEndDate();

            var promoes = GetConstraintedQueryPromo(authorizationManager, Context, ClientTreeId);

            var ppaMetric = GetPPA(promoes);
            var pctMetric = GetPCT(promoes);
            var padMetric = GetPAD(promoes);
            var psfaMetric = GetPSFA(promoes, marsDate);

            return JsonConvert.SerializeObject(new
            {
                PPA = ppaMetric.Value,
                PCT = pctMetric.Value,
                PAD = padMetric.Value,
                PSFA = psfaMetric.Value,
                PPA_LSV = ppaMetric.ValueLSV,
                PCT_LSV = pctMetric.ValueLSV,
                PAD_LSV = padMetric.ValueLSV,
                PSFA_LSV = psfaMetric.ValueLSV
            });
        }

        private static ModelReturn GetPPA(IQueryable<PromoGridView> promoes)
        {
            var readyStatuses = new string[] { "Approved", "Planned" };
            var negativeStatuses = new string[] { "On Approval", "Draft(published)" };
            var notCheckStatuses = new string[] { "Draft", "Cancelled", "Deleted" };

            var endDate = DateTime.Now.AddDays(7 * 8);

            var filteredPromoes = promoes.Where(x => x.DispatchesStart <= endDate && x.DispatchesStart >= DateTime.Now);

            var readyPromoes = filteredPromoes.Count(x => readyStatuses.Contains(x.PromoStatusName));
            var allPromoes = filteredPromoes.Count(x => !notCheckStatuses.Contains(x.PromoStatusName));

            if (allPromoes > 0)
            {
                var ppa = (double)readyPromoes / allPromoes;
                var ppaLsv = filteredPromoes.Where(x => readyStatuses.Contains(x.PromoStatusName)).Sum(x => x.PlanPromoLSV);

                return new ModelReturn { Value = Math.Round(ppa * 100, 0, MidpointRounding.AwayFromZero).ToString(), ValueLSV = Math.Round(ppaLsv.Value, 3, MidpointRounding.AwayFromZero).ToString() };
            }
            else
            {
                return new ModelReturn { Value = "0", ValueLSV = "0" };
            }
        }
        private static ModelReturn GetPCT(IQueryable<PromoGridView> promoes)
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

                return new ModelReturn { Value = Math.Round(pct * 100, 0, MidpointRounding.AwayFromZero).ToString(), ValueLSV = Math.Round(pctLsv.Value, 3, MidpointRounding.AwayFromZero).ToString() };
            }
            else
            {
                return new ModelReturn { Value = "0", ValueLSV = "0" };
            }
        }

        private static ModelReturn GetPAD(IQueryable<PromoGridView> promoes)
        {
            var checkStatuses = new string[] { "Closed", "Finished" };

            var endDate = DateTime.Now.AddDays(-7 * 7);
            var startDate = new DateTime(endDate.Year, 1, 1);

            var filteredPromoes = promoes.Where(x =>
                                    x.EndDate >= startDate && x.EndDate <= endDate
                                    && x.ActualPromoLSV != null && x.ActualPromoLSV != 0
                                    && x.ActualPromoLSVByCompensation != null && x.ActualPromoLSVByCompensation != 0
                                    && checkStatuses.Contains(x.PromoStatusName))
                                    .Select(x => new
                                    {
                                        x.ActualPromoLSV,
                                        x.ActualPromoLSVByCompensation,
                                        ActualPromoLSVdiff = Math.Abs(x.ActualPromoLSV.Value - x.ActualPromoLSVByCompensation.Value) / x.ActualPromoLSVByCompensation
                                    });
            var total = filteredPromoes.Count();
            filteredPromoes = filteredPromoes.Where(x => x.ActualPromoLSVdiff > 0.1);
            var pad = $"{filteredPromoes.Count()}/{total}";
            var padLsv = filteredPromoes.Sum(x => Math.Abs(x.ActualPromoLSV.Value - x.ActualPromoLSVByCompensation.Value));

            return new ModelReturn { Value = pad, ValueLSV = Math.Round(padLsv, 3, MidpointRounding.AwayFromZero).ToString() };
        }
        private static ModelReturn GetPSFA(IQueryable<PromoGridView> promoes, MarsDate marsDate)
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
                var sfaLsv = filteredPromoes.Sum(x => Math.Abs(x.ActualPromoIncrementalLSV.Value - x.PlanPromoIncrementalLSV.Value));
                var sfa = sfaLsv / filteredPromoes.Sum(x => x.PlanPromoIncrementalLSV.Value);
                sfa = (1 - sfa) * 100;

                return new ModelReturn { Value = Math.Round(sfa, 0, MidpointRounding.AwayFromZero).ToString(), ValueLSV = Math.Round(sfaLsv, 3, MidpointRounding.AwayFromZero).ToString() };
            }
            else
            {
                return new ModelReturn { Value = "0", ValueLSV = "0" };
            }
        }

        private static IQueryable<PromoGridView> GetConstraintedQueryPromo(IAuthorizationManager authorizationManager, DatabaseContext Context, int ClientTreeId)
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
                .Where(g=>g.ClientHierarchy.Contains(client.FullPathName));
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, TPMmode.Current, filters, FilterQueryModes.Active, canChangeStateOnly ? role : String.Empty);
            query = query.Where(x => !x.IsOnHold);
            // Не администраторы не смотрят чужие черновики
            if (role != "Administrator")
            {
                query = query.Where(e => e.PromoStatusSystemName != "Draft" || e.CreatorId == user.Id);
            }
            return query;
        }
        class ModelReturn
        {
            public string Value { get; set; }
            public string ValueLSV { get; set; }
        }
    }
}
