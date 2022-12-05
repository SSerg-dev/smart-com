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

        public static string GetLiveMetricsDashboard(IAuthorizationManager authorizationManager, DatabaseContext Context)
        {
            var promoes = GetConstraintedQueryPromo(authorizationManager, Context);

            var ppaMetric = GetPPA(promoes);
            var pctMetric = GetPCT(promoes);
            var padMetric = GetPAD(promoes);

            return JsonConvert.SerializeObject(new { PPA = ppaMetric.Item1, PCT = pctMetric.Item1, PAD = padMetric.Item1, PPA_LSV = ppaMetric.Item2, PCT_LSV = pctMetric.Item2, PAD_LSV = padMetric.Item2 });
        }

        private static Tuple<double, double> GetPPA(IQueryable<PromoGridView> promoes)
        {
            var readyStatuses = new string[] { "Approved", "Planned" };
            var negativeStatuses = new string[] { "On Approval", "Draft(published)" };
            var notCheckStatuses = new string[] { "Draft", "Cancelled", "Deleted" };

            var startDate = DateTime.Now.AddDays(7 * 8);

            var filteredPromoes = promoes.Where(x => x.DispatchesStart >= startDate && x.DispatchesStart <= DateTime.Now);

            var readyPromoes = filteredPromoes.Count(x => readyStatuses.Contains(x.PromoStatusName));
            var allPromoes = filteredPromoes.Count(x => !notCheckStatuses.Contains(x.PromoStatusName));

            if (allPromoes > 0)
            {
                var ppa = (double)readyPromoes / allPromoes;
                var ppaLsv = filteredPromoes.Where(x => readyStatuses.Contains(x.PromoStatusName)).Sum(x => x.PlanPromoLSV);

                return new Tuple<double, double>(ppa * 100, ppaLsv.Value);
            }
            else
            {
                return new Tuple<double, double>(0, 0);
            }
        }
        private static Tuple<double, double> GetPCT(IQueryable<PromoGridView> promoes)
        {
            var checkStatuses = new string[] { "Closed", "Finished" };

            var endDate = DateTime.Now.AddDays(-7 * 7);
            var startDate = new DateTime(DateTime.Now.Year, 1, 1);

            var filteredPromoes = promoes.Where(x => x.EndDate >= startDate && x.EndDate <= endDate);

            var closedPromoes = filteredPromoes.Count(x => x.PromoStatusName == "Closed");
            var allCheckPromoes = filteredPromoes.Count(x => checkStatuses.Contains(x.PromoStatusName));

            if (allCheckPromoes > 0)
            {
                var pct = (double)closedPromoes / allCheckPromoes;
                var pctLsv = filteredPromoes.Where(x => x.PromoStatusName == "Finished").Sum(x => x.PlanPromoLSV);

                return new Tuple<double, double>(pct * 100, pctLsv.Value);
            }
            else
            {
                return new Tuple<double, double>(0, 0);
            }
        }

        private static Tuple<double, double> GetPAD(IQueryable<PromoGridView> promoes)
        {
            var checkStatuses = new string[] { "Closed", "Finished" };

            var endDate = DateTime.Now.AddDays(-7 * 7);
            var startDate = new DateTime(DateTime.Now.Year, 1, 1);

            var filteredPromoes = promoes.Where(x => 
                                    x.EndDate >= startDate && x.EndDate <= endDate
                                    && x.ActualPromoLSV != null && x.ActualPromoLSV != 0
                                    && x.ActualPromoLSVByCompensation != null && x.ActualPromoLSVByCompensation != 0
                                    && checkStatuses.Contains(x.PromoStatusName))
                                    .Select(x => new {
                                        x.ActualPromoLSV,
                                        x.ActualPromoLSVByCompensation,
                                        ActualPromoLSVdiff = Math.Abs(x.ActualPromoLSV.Value - x.ActualPromoLSVByCompensation.Value) / x.ActualPromoLSVByCompensation
                                    });
            filteredPromoes = filteredPromoes.Where(x => x.ActualPromoLSVdiff > 0.1);

            var pad = filteredPromoes.Count();
            var padLsv = filteredPromoes.Sum(x => Math.Abs(x.ActualPromoLSV.Value - x.ActualPromoLSVByCompensation.Value));

            return new Tuple<double, double>(pad, padLsv);
        }

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
            query = query.Where(x => !x.IsOnHold);
            // Не администраторы не смотрят чужие черновики
            if (role != "Administrator")
            {
                query = query.Where(e => e.PromoStatusSystemName != "Draft" || e.CreatorId == user.Id);
            }
            return query;
        }
    }
}
