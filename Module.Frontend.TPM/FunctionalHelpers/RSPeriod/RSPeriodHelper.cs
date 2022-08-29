using Module.Persist.TPM.Model.SimpleModel;
using Module.Persist.TPM.Model.TPM;
using Persist;
using Persist.Model.Settings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Module.Frontend.TPM.FunctionalHelpers.RSPeriod
{
    public static class RSPeriodHelper
    {
        public static StartEndModel GetRSPeriod(DatabaseContext Context)
        {
            string weeks = Context.Set<Setting>().Where(g => g.Name == "RS_START_WEEKS").FirstOrDefault().Value;
            DateTimeOffset today = DateTimeOffset.Now;
            DateTimeOffset endDate = new DateTimeOffset(today.Year, 12, 31, 23, 0, 0, new TimeSpan(0, 0, 0));
            StartEndModel startEndModel = new StartEndModel
            {
                EndDate = endDate
            };

            if (Int32.TryParse(weeks, out int intweeks))
            {

                DateTimeOffset RsStartDate = today.AddDays(intweeks * 7);
                startEndModel.StartDate = RsStartDate;

                return startEndModel;
            }
            else
            {
                startEndModel.StartDate = DateTimeOffset.MinValue;
                return startEndModel;
            }
        }
        public static void CreateRSPeriod(Promo promo, DatabaseContext Context)
        {
            RollingScenario rollingScenarioExist = Context.Set<RollingScenario>()
                .Include(g => g.Promoes)
                .FirstOrDefault(g => g.ClientTreeId == promo.ClientTreeKeyId && !g.Disabled);

            List<PromoStatus> promoStatuses = Context.Set<PromoStatus>().Where(g => !g.Disabled).ToList();
            StartEndModel startEndModel = GetRSPeriod(Context);
            RollingScenario rollingScenario = new RollingScenario();
            if (rollingScenarioExist == null)
            {
                ClientTree client = Context.Set<ClientTree>().FirstOrDefault(g => g.ObjectId == promo.ClientTreeId);
                rollingScenario = new RollingScenario
                {
                    StartDate = startEndModel.StartDate,
                    EndDate = startEndModel.EndDate,
                    PromoStatus = promoStatuses.FirstOrDefault(g => g.SystemName == "Draft"),
                    ClientTree = client,
                    Promoes = new List<Promo>()
                };
                rollingScenario.Promoes.Add(promo);
                Context.Set<RollingScenario>().Add(rollingScenario);
            }
            else
            {
                rollingScenarioExist.Promoes.Add(promo);
            }
            Context.SaveChanges();
        }
        public static void CreateRSPeriod(List<Promo> promoes, DatabaseContext Context)
        {
            foreach (Promo promo in promoes)
            {
                CreateRSPeriod(promo, Context);
            }
        }
        public static void EditRSPeriod(Promo promo, DatabaseContext Context)
        {

        }
        public static void EditRSPeriod(List<Promo> promoes, DatabaseContext Context)
        {

        }
        public static void DeleteRSPeriod(Guid rollingScenarioId, DatabaseContext Context)
        {
            RollingScenario rollingScenario = Context.Set<RollingScenario>()
                                            .Include(g => g.PromoStatus)
                                            .Include(g => g.Promoes)
                                            .FirstOrDefault(g => g.Id == rollingScenarioId);
            PromoStatus promoStatusCancelled = Context.Set<PromoStatus>().FirstOrDefault(v => v.SystemName == "Cancelled");
            rollingScenario.IsSendForApproval = false;
            rollingScenario.Disabled = true;
            rollingScenario.DeletedDate = DateTimeOffset.Now;
            rollingScenario.PromoStatus = promoStatusCancelled;
            Context.Set<Promo>().RemoveRange(rollingScenario.Promoes);
            Context.SaveChanges();
        }
        public static void MassApproveRSPeriod(List<RollingScenario> rollingScenarios, DatabaseContext Context)
        {

        }
        public static void ApproveRSPeriod(RollingScenario rollingScenario, DatabaseContext Context)
        {

        }
        public static void OnApprovalRSPeriod(RollingScenario rollingScenario, DatabaseContext Context)
        {

        }
    }
}
