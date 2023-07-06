using Module.Persist.TPM.Enum;
using Module.Persist.TPM.Model.TPM;
using Persist;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module.Persist.TPM.Model.SimpleModel;
using Module.Frontend.TPM.FunctionalHelpers.RSPeriod;
using Module.Persist.TPM.Model.Interfaces;
using Module.Frontend.TPM.FunctionalHelpers.RA;
using System;
using Module.Frontend.TPM.FunctionalHelpers.HiddenMode;
using Persist.Model.Settings;
using Module.Persist.TPM.Utils;

namespace Module.Frontend.TPM.FunctionalHelpers.Scenario
{
    public static class ScenarioHelper
    {
        public static void CreateScenarioPeriod(Promo promo, DatabaseContext Context, TPMmode tPMmode)
        {
            ClientTree client = Context.Set<ClientTree>().FirstOrDefault(g => g.ObjectId == promo.ClientTreeId && g.EndDate == null);
            if (tPMmode == TPMmode.RS)
            {
                CreateRSPeriod(promo, client, Context, ScenarioType.RS);
            }
            if (tPMmode == TPMmode.RA)
            {
                CreateRAPeriod(promo, client, Context, ScenarioType.RA, true);
            }

            Context.SaveChanges();
        }
        public static void CreateScenarioPeriod(List<Promo> promoes, DatabaseContext Context, TPMmode tPMmode)
        {
            foreach (Promo promo in promoes)
            {
                CreateScenarioPeriod(promo, Context, tPMmode);
            }
        }
        private static void CreateRSPeriod(Promo promo, ClientTree client, DatabaseContext Context, ScenarioType scenarioType)
        {
            List<string> outStatuses = new List<string> { RSstateNames.WAITING, RSstateNames.APPROVED };
            StartEndModel startEndModel = RSPeriodHelper.GetRSPeriod(Context);

            RollingScenario rollingScenarioExist = Context.Set<RollingScenario>()
                .Include(g => g.Promoes)
                .FirstOrDefault(g => g.ClientTreeId == promo.ClientTreeKeyId && !g.Disabled && !outStatuses.Contains(g.RSstatus));

            if (rollingScenarioExist == null)
            {
                RollingScenario rollingScenario = new RollingScenario
                {
                    ScenarioType = scenarioType,
                    StartDate = startEndModel.StartDate,
                    EndDate = startEndModel.EndDate,
                    RSstatus = RSstateNames.DRAFT,
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
        }
        private static RollingScenario CreateRAPeriod(Promo promo, ClientTree client, DatabaseContext Context, ScenarioType scenarioType, bool withPromo)
        {
            List<string> outStatuses = new List<string> { RSstateNames.WAITING, RSstateNames.APPROVED };
            StartEndModel startEndModel = RAmodeHelper.GetRAPeriod();

            RollingScenario rollingScenarioExist = Context.Set<RollingScenario>()
                .Include(g => g.Promoes)
                .FirstOrDefault(g => g.ClientTreeId == client.Id && !g.Disabled && !outStatuses.Contains(g.RSstatus));

            if (rollingScenarioExist == null)
            {
                RollingScenario rollingScenario = new RollingScenario
                {
                    ScenarioType = scenarioType,
                    StartDate = startEndModel.StartDate,
                    EndDate = startEndModel.EndDate,
                    RSstatus = RSstateNames.DRAFT,
                    ClientTree = client,
                    Promoes = new List<Promo>()
                };
                if (withPromo)
                {
                    rollingScenario.Promoes.Add(promo);
                }
                Context.Set<RollingScenario>().Add(rollingScenario);
                return rollingScenario;
            }
            else
            {
                if (withPromo)
                {
                    rollingScenarioExist.Promoes.Add(promo);
                }
                return rollingScenarioExist;
            }
        }

        public static RollingScenario GetActiveScenario(int clientObjectId, DatabaseContext Context)
        {
            List<string> activeStatuses = new List<string> { RSstateNames.DRAFT, RSstateNames.ON_APPROVAL };
            return Context.Set<RollingScenario>().Include(x => x.Promoes).SingleOrDefault(x => !x.Disabled
                        && activeStatuses.Contains(x.RSstatus)
                        && x.ClientTree.ObjectId == clientObjectId);
        }
        public static ClientTree UploadSavedScenario(Guid savedScenarioId, DatabaseContext Context)
        {
            SavedScenario savedScenario = Context.Set<SavedScenario>()
                .Include(g => g.RollingScenario.ClientTree)
                .Include(g => g.Promoes)
                .FirstOrDefault(f => f.Id == savedScenarioId);
            ClientTree clientTree = savedScenario.RollingScenario.ClientTree;
            RollingScenario rollingScenario = GetActiveScenario(clientTree.ObjectId, Context);
            if (rollingScenario != null)
            {
                DeleteRAPeriod(rollingScenario.Id, Context);
                RestoreRAPeriod(savedScenario, Context, clientTree);
            }
            else
            {
                RestoreRAPeriod(savedScenario, Context, clientTree);
            }
            return clientTree;
        }
        public static StatusScenarioResult GetStatusScenario(Guid savedScenarioId, DatabaseContext Context)
        {
            SavedScenario savedScenario = Context.Set<SavedScenario>()
                .Include(g => g.RollingScenario.ClientTree)
                .FirstOrDefault(f => f.Id == savedScenarioId);
            ClientTree clientTree = savedScenario.RollingScenario.ClientTree;
            RollingScenario rollingScenario = GetActiveScenario(clientTree.ObjectId, Context);
            if (rollingScenario != null)
            {
                return new StatusScenarioResult
                {
                    ClientName = clientTree.FullPathName,
                    RSId = (int)rollingScenario.RSId
                };
            }
            return null;
        }
        public class StatusScenarioResult
        {
            public string ClientName { get; set; }
            public int RSId { get; set; }
        }
        public static void DeleteRAPeriod(Guid rollingScenarioId, DatabaseContext Context)
        {
            RollingScenario rollingScenario = Context.Set<RollingScenario>()
                                            .Include(g => g.Promoes)
                                            .FirstOrDefault(g => g.Id == rollingScenarioId);
            rollingScenario.IsSendForApproval = false;
            rollingScenario.Disabled = true;
            rollingScenario.DeletedDate = DateTimeOffset.Now;
            rollingScenario.RSstatus = RSstateNames.CANCELLED;
            Context.Set<Promo>().RemoveRange(rollingScenario.Promoes.Where(g => g.TPMmode == TPMmode.RA));
            Context.SaveChanges();
        }
        public static void RestoreRAPeriod(SavedScenario savedScenario, DatabaseContext Context, ClientTree clientTree)
        {
            List<Promo> promos = savedScenario.Promoes.ToList();
            RollingScenario rollingScenario = CreateRAPeriod(null, clientTree, Context, ScenarioType.RA, false);
            Context.SaveChanges();
            savedScenario.RollingScenarioId = rollingScenario.Id;
            HiddenModeHelper.CopyPromoesFromHiddenToRA(Context, promos, rollingScenario);
            Context.SaveChanges();
        }
        public static void OnApprovalScenarioPeriod(Guid rollingScenarioId, DatabaseContext Context)
        {
            RollingScenario RS = Context.Set<RollingScenario>()
                    .FirstOrDefault(g => g.Id == rollingScenarioId);
            RS.IsSendForApproval = true;
            RS.RSstatus = RSstateNames.ON_APPROVAL;
            DateTimeOffset expirationDate = TimeHelper.TodayEndDay();
            if (RS.ScenarioType == ScenarioType.RS)
            {
                RS.ExpirationDate = expirationDate.AddDays(14);
            }
            if (RS.ScenarioType == ScenarioType.RA)
            {
                string weeks = Context.Set<Setting>().Where(g => g.Name == "RA_END_APPROVE_WEEKS").FirstOrDefault().Value;
                if (Int32.TryParse(weeks, out int intweeks))
                {
                    RS.ExpirationDate = expirationDate.AddDays(intweeks * 7);
                }
            }

            Context.SaveChanges();
        }
        public static void DeleteScenarioPeriod(Guid rollingScenarioId, DatabaseContext Context)
        {
            RollingScenario rollingScenario = Context.Set<RollingScenario>()
                                            .Include(g => g.Promoes)
                                            .FirstOrDefault(g => g.Id == rollingScenarioId);
            rollingScenario.IsSendForApproval = false;
            rollingScenario.Disabled = true;
            rollingScenario.DeletedDate = DateTimeOffset.Now;
            rollingScenario.RSstatus = RSstateNames.CANCELLED;
            Context.Set<Promo>().RemoveRange(rollingScenario.Promoes);
            Context.SaveChanges();
        }
        public static void ApproveScenarioPeriod(Guid rollingScenarioId, DatabaseContext Context)
        {
            RollingScenario RS = Context.Set<RollingScenario>()
                    .Include(g => g.Promoes)
                    .FirstOrDefault(g => g.Id == rollingScenarioId);
            List<Guid> PromoRSIds = RS.Promoes.Select(f => f.Id).ToList();
            if (Context.Set<BlockedPromo>().Any(x => x.Disabled == false && PromoRSIds.Contains(x.PromoId)))
            {
                throw new System.Web.HttpException("there is a blocked Promo");
            }
            RS.IsCMManagerApproved = true;
            RS.RSstatus = RSstateNames.APPROVED;
            RSPeriodHelper.CopyBackPromoes(RS.Promoes.ToList(), Context);
            Context.Set<Promo>().RemoveRange(RS.Promoes);
            Context.SaveChanges();
        }
    }
}
