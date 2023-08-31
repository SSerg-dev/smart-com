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
using Core.History;
using Module.Frontend.TPM.FunctionalHelpers.HiddenMode;
using Module.Persist.TPM.MongoDB;
using Persist.Model.Settings;
using Module.Persist.TPM.Utils;
using Persist.Model.Interface;
using Module.Frontend.TPM.Util;

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
            List<int> promoHiddennumbers = savedScenario.Promoes.Select(h => h.Number).Cast<int>().ToList();
            List<Promo> promos = Context.Set<Promo>()
                    //.Include(g => g.BTLPromoes)
                    .Include(g => g.PromoSupportPromoes)
                    .Include(g => g.PromoProductTrees)
                    .Include(g => g.IncrementalPromoes)
                    .Include(x => x.PromoProducts.Select(y => y.PromoProductsCorrections))
                    .Include(g => g.PromoPriceIncrease.PromoProductPriceIncreases.Select(f => f.ProductCorrectionPriceIncreases))
                    .Where(x => promoHiddennumbers.Contains((int)x.Number) && x.TPMmode == TPMmode.Hidden).ToList();
            RollingScenario rollingScenario = CreateRAPeriod(null, clientTree, Context, ScenarioType.RA, false);
            Context.SaveChanges();
            savedScenario.RollingScenarioId = rollingScenario.Id;
            rollingScenario.RSstatus = RSstateNames.CALCULATING;
            rollingScenario.TaskStatus = TaskStatusNames.INPROGRESS;
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
                    .Include(g => g.ClientTree)
                    .FirstOrDefault(g => g.Id == rollingScenarioId);
            List<Guid> PromoRSIds = RS.Promoes.Select(f => f.Id).ToList();
            if (Context.Set<BlockedPromo>().Any(x => x.Disabled == false && PromoRSIds.Contains(x.PromoId)))
            {
                throw new System.Web.HttpException("there is a blocked Promo");
            }
            RS.IsCMManagerApproved = true;
            RS.RSstatus = RSstateNames.APPROVED;
            SaveOldPromoAfterApprove(RS, Context);
            var newPromoIds = RSPeriodHelper.CopyBackPromoes(RS.Promoes.ToList(), Context);

            if (newPromoIds.Count > 0)
            {
                var source = string.Format("{0} Scenario {1}", RS.ScenarioType.ToString(), RS.RSId);
                var mongoHelper = new MongoHelper<Guid>();
                mongoHelper.WriteScenarioPromoes(
                    source,
                    newPromoIds,
                    Context.AuthManager.GetCurrentUser(),
                    Context.AuthManager.GetCurrentRole(),
                    OperationType.Updated
                );
            }
        }
        public static void RemoveOldCreateNewRSPeriodML(int clientId, FileBuffer buffer, DatabaseContext Context)
        {
            ClientTree client = Context.Set<ClientTree>().FirstOrDefault(g => g.ObjectId == clientId && g.EndDate == null);
            List<RollingScenario> rollingScenarioExists = Context.Set<RollingScenario>()
                .Include(g => g.Promoes)
                .Where(g => g.ClientTreeId == client.Id && !g.Disabled && g.ScenarioType == ScenarioType.RS)
                .ToList();
            if (rollingScenarioExists.Any(g => g.RSstatus == RSstateNames.CALCULATING || (g.RSstatus == RSstateNames.DRAFT && g.TaskStatus == TaskStatusNames.COMPLETE) || (g.RSstatus == RSstateNames.ON_APPROVAL && g.TaskStatus == TaskStatusNames.COMPLETE)))
            {
                //удаляем записанный filebuffer
                Context.FileBuffers.Remove(buffer);
                Context.SaveChanges();
                return;
            }
            RollingScenario rollingScenarioExist = rollingScenarioExists.FirstOrDefault();
            if (rollingScenarioExist == null)
            {
                CreateMLRSperiod(clientId, buffer.Id, Context);
            }
            else
            {
                DeleteScenarioPeriod(rollingScenarioExist.Id, Context);
                CreateMLRSperiod(clientId, buffer.Id, Context);
            }
            Context.SaveChanges();
        }
        private static void CreateMLRSperiod(int clientId, Guid bufferId, DatabaseContext Context)
        {
            List<PromoStatus> promoStatuses = Context.Set<PromoStatus>().Where(g => !g.Disabled).ToList();
            StartEndModel startEndModel = RSPeriodHelper.GetRSPeriod(Context);
            ClientTree client = Context.Set<ClientTree>().FirstOrDefault(g => g.ObjectId == clientId && g.EndDate == null);
            RollingScenario rollingScenario = new RollingScenario
            {
                StartDate = startEndModel.StartDate,
                EndDate = startEndModel.EndDate,
                RSstatus = RSstateNames.WAITING,
                ClientTree = client,
                IsMLmodel = true,
                FileBufferId = bufferId,
                ScenarioType = ScenarioType.RS
            };
            Context.Set<RollingScenario>().Add(rollingScenario);
        }
        public static void RemoveOldCreateNewRAPeriodML(int clientId, FileBuffer buffer, DatabaseContext Context)
        {
            ClientTree client = Context.Set<ClientTree>().FirstOrDefault(g => g.ObjectId == clientId && g.EndDate == null);
            List<RollingScenario> rollingScenarioExists = Context.Set<RollingScenario>()
                .Include(g => g.Promoes)
                .Where(g => g.ClientTreeId == client.Id && !g.Disabled && g.ScenarioType == ScenarioType.RA)
                .ToList();
            if (rollingScenarioExists.Any(g => g.RSstatus == RSstateNames.CALCULATING || (g.RSstatus == RSstateNames.DRAFT && g.TaskStatus == TaskStatusNames.COMPLETE) || (g.RSstatus == RSstateNames.ON_APPROVAL && g.TaskStatus == TaskStatusNames.COMPLETE)))
            {
                //удаляем записанный filebuffer
                Context.FileBuffers.Remove(buffer);
                Context.SaveChanges();
                return;
            }
            RollingScenario rollingScenarioExist = rollingScenarioExists.FirstOrDefault();
            if (rollingScenarioExist == null)
            {
                CreateMLRAperiod(clientId, buffer.Id, Context);
            }
            else
            {
                DeleteScenarioPeriod(rollingScenarioExist.Id, Context);
                CreateMLRAperiod(clientId, buffer.Id, Context);
            }
            Context.SaveChanges();
        }
        private static void CreateMLRAperiod(int clientId, Guid bufferId, DatabaseContext Context)
        {
            List<PromoStatus> promoStatuses = Context.Set<PromoStatus>().Where(g => !g.Disabled).ToList();
            StartEndModel startEndModel = RAmodeHelper.GetRAPeriod();
            ClientTree client = Context.Set<ClientTree>().FirstOrDefault(g => g.ObjectId == clientId && g.EndDate == null);
            RollingScenario rollingScenario = new RollingScenario
            {
                StartDate = startEndModel.StartDate,
                EndDate = startEndModel.EndDate,
                RSstatus = RSstateNames.WAITING,
                ClientTree = client,
                IsMLmodel = true,
                FileBufferId = bufferId,
                ScenarioType = ScenarioType.RA
            };
            Context.Set<RollingScenario>().Add(rollingScenario);
        }
        private static void SaveOldPromoAfterApprove(RollingScenario RS, DatabaseContext Context)
        {
            SavedPromo savedPromoOld = Context.Set<SavedPromo>().FirstOrDefault(g => g.ClientTreeId == RS.ClientTreeId && g.StartDate == RS.StartDate && g.EndDate == RS.EndDate);
            if (savedPromoOld != null)
            {
                Context.Set<SavedPromo>().Remove(savedPromoOld);
                Context.SaveChanges();
            }
            SavedPromo savedPromo = new SavedPromo
            {
                ClientTreeId = RS.ClientTreeId,
                StartDate = RS.StartDate,
                EndDate = RS.EndDate,
                SavedPromoType = SavedPromoType.Scenario
            };
            PromoHelper.ClientDispatchDays clientDispatchDays = PromoHelper.GetClientDispatchDays(RS.ClientTree);

            HiddenModeHelper.CopyPromoesToSavedPromo(Context, GetCurrentPeriodPromoes(RS, Context, clientDispatchDays), savedPromo);
        }
        private static List<Promo> GetCurrentPeriodPromoes(RollingScenario RS, DatabaseContext Context, PromoHelper.ClientDispatchDays clientDispatchDays)
        {
            DateTimeOffset DispatchesStart;
            List<string> needStatuses = "OnApproval,Approved,DraftPublished".Split(',').ToList();            
            List<Promo> promos = new List<Promo>();

            if (RS.ScenarioType == ScenarioType.RS)
            {
                StartEndModel startEndModelRS = RSPeriodHelper.GetRSPeriod(Context);
                if (clientDispatchDays.IsStartAdd)
                {
                    DispatchesStart = startEndModelRS.StartDate.AddDays(clientDispatchDays.StartDays);
                }
                else
                {
                    DispatchesStart = startEndModelRS.StartDate.AddDays(-clientDispatchDays.StartDays);
                }
                promos = Context.Set<Promo>().Where(g => g.DispatchesStart > startEndModelRS.StartDate &&
                g.EndDate < startEndModelRS.EndDate && g.BudgetYear == startEndModelRS.BudgetYear &&
                needStatuses.Contains(g.PromoStatus.SystemName) && g.ClientTreeId == RS.ClientTree.ObjectId && !g.Disabled && g.TPMmode == TPMmode.Current)
                    .ToList();
            }
            if (RS.ScenarioType == ScenarioType.RA)
            {
                StartEndModel startEndModelRA = RAmodeHelper.GetRAPeriod();
                if (clientDispatchDays.IsStartAdd)
                {
                    DispatchesStart = startEndModelRA.StartDate.AddDays(clientDispatchDays.StartDays);
                }
                else
                {
                    DispatchesStart = startEndModelRA.StartDate.AddDays(-clientDispatchDays.StartDays);
                }
                promos = Context.Set<Promo>().Where(g => g.DispatchesStart > startEndModelRA.StartDate &&
                g.EndDate < startEndModelRA.EndDate && g.BudgetYear == startEndModelRA.BudgetYear &&
                needStatuses.Contains(g.PromoStatus.SystemName) && g.ClientTreeId == RS.ClientTree.ObjectId && !g.Disabled && g.TPMmode == TPMmode.Current)
                    .ToList();
            }
            return promos;
        }
    }
}
