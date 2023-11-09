using Interfaces.Implementation.Action;
using Module.Frontend.TPM.FunctionalHelpers.Scenario;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Utility.LogWriter;
using Module.Persist.TPM.Utils;
using Module.Frontend.TPM.FunctionalHelpers.RA;
using Module.Persist.TPM.Model.SimpleModel;
using Module.Frontend.TPM.FunctionalHelpers.HiddenMode;
using Module.Persist.TPM.Enum;
using Persist.Model.Settings;
using Module.Persist.TPM.Model.Interfaces;

namespace Module.Host.TPM.Actions
{
    class CopyPrevousYearAction : BaseAction
    {
        private LogWriter HandlerLogger { get; }
        private Guid UserId { get; }
        private Guid RoleId { get; }
        public string HandlerStatus { get; private set; }
        public int ObjectId { get; set; }
        public bool CheckedDate { get; set; }
        public Guid HandlerId { get; set; }

        readonly Stopwatch Stopwatch1 = new Stopwatch();
        public CopyPrevousYearAction(Guid handlerId, LogWriter Logger, Guid userId, Guid roleId, int objectId, bool checkedDate)
        {
            HandlerLogger = Logger;
            UserId = userId;
            RoleId = roleId;
            ObjectId = objectId;
            HandlerId = handlerId;
            CheckedDate = checkedDate;
        }

        public override void Execute()
        {
            using (DatabaseContext Context = new DatabaseContext())
            {
                Stopwatch1.Restart();
                ClientTree clientTree = Context.Set<ClientTree>().FirstOrDefault(g => g.ObjectId == ObjectId && g.EndDate == null);
                RollingScenario scenario = ScenarioHelper.GetActiveScenario(clientTree.ObjectId, Context);
                PromoHelper.ClientDispatchDays clientDispatchDays = PromoHelper.GetClientDispatchDays(clientTree);
                using (var transaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        DateTimeOffset expirationDate = TimeHelper.TodayEndDay();
                        string weeks = Context.Set<Setting>().Where(g => g.Name == "RA_END_APPROVE_WEEKS").FirstOrDefault().Value;
                        if (int.TryParse(weeks, out int intweeks))
                        {
                            expirationDate = expirationDate.AddDays(intweeks * 7);
                        }
                        StartEndModel startEndModel = RAmodeHelper.GetRAPeriod();
                        int budgetYear = TimeHelper.ThisBuggetYear();
                        int nextYear = TimeHelper.NextBuggetYear();
                        List<string> notStatus = new List<string> { "Draft", "Cancelled", "Deleted" };
                        Guid draftPublish = Context.Set<PromoStatus>().FirstOrDefault(f => f.SystemName == "DraftPublished").Id;
                        List <Promo> promos = Context.Set<Promo>()
                            .Include(g => g.PromoSupportPromoes)
                            .Include(g => g.PromoProductTrees)
                            .Include(g => g.IncrementalPromoes)
                            .Include(x => x.PromoProducts.Select(y => y.PromoProductsCorrections))
                            .Include(g => g.PromoPriceIncrease.PromoProductPriceIncreases.Select(f => f.ProductCorrectionPriceIncreases))
                            .Where(g => g.ClientTreeKeyId == clientTree.Id && g.BudgetYear == budgetYear && !notStatus.Contains(g.PromoStatus.SystemName) && !g.Disabled && g.TPMmode == TPMmode.Current)
                            .ToList();
                        CopyRAReturn copyRAReturn = HiddenModeHelper.CopyToPromoRA(Context, promos, nextYear, CheckedDate, clientDispatchDays, draftPublish);

                        if (copyRAReturn.Promos.Count > 0)
                        {
                            if (scenario != null)
                            {
                                ScenarioHelper.DeleteRAPeriod(scenario.Id, Context);
                            }
                            RollingScenario rollingScenario = new RollingScenario
                            {
                                HandlerId = HandlerId,
                                RSstatus = RSstateNames.CALCULATING,
                                TaskStatus = TaskStatusNames.INPROGRESS,
                                ScenarioType = ScenarioType.RA,
                                ExpirationDate = expirationDate,
                                StartDate = startEndModel.StartDate,
                                EndDate = startEndModel.EndDate,
                                ClientTreeId = clientTree.Id
                            };
                            List<ChangesIncident> changesIncidents = new List<ChangesIncident>();
                            foreach (var item in copyRAReturn.Promos)
                            {
                                var changesIncident = new ChangesIncident
                                {
                                    Id = Guid.NewGuid(),
                                    DirectoryName = nameof(Promo),
                                    ItemId = item.Id.ToString(),
                                    CreateDate = DateTimeOffset.Now,
                                    Disabled = false
                                };
                                changesIncidents.Add(changesIncident);
                            }
                            Context.Set<ChangesIncident>().AddRange(changesIncidents);
                            rollingScenario.Promoes = copyRAReturn.Promos;
                            Context.Set<RollingScenario>().Add(rollingScenario);
                            Context.SaveChanges();
                            HandlerLogger.Write(true, string.Format("Scenario: {0}. {1} added promo", rollingScenario.RSId, rollingScenario.Promoes.Count), "Message");
                        }

                        foreach (var item in copyRAReturn.Errors)
                        {
                            HandlerLogger.Write(true, item, "Warning");
                        }
                        Context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        HandlerLogger.Write(true, GetExceptionMessage.GetInnerException(ex).Message, "Error");
                        HandlerStatus = "HasErrors";
                        return;
                    }
                }




                //if ((DateTimeOffset)promo.DispatchesStart < startEndModel.StartDate || startEndModel.EndDate < (DateTimeOffset)promo.EndDate)
                //{
                //    HandlerLogger.Write(true, string.Format("ML Promo: {0} is not in the RS period, startdate: {1:yyyy-MM-dd HH:mm:ss}", inputMlId, promo.StartDate), "Warning");
                //    errorcount++;
                //}
            }
        }
    }
}
