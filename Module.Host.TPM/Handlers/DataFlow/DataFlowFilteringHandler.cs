using Core.Dependency;
using Core.Settings;
using Looper.Core;
using Looper.Parameters;
using Model.Host.TPM.Handlers.DataFlow;
using Module.Host.TPM.Handlers.DataFlow.Filters;
using Module.Host.TPM.Handlers.DataFlow.Modules;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Module.Persist.TPM.Utils.Filter;
using Persist;
using ProcessingHost.Handlers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.LogWriter;

namespace Module.Host.TPM.Handlers.DataFlow
{
    public class DataFlowFilteringHandler : BaseHandler
    {
        private int MaxDegreeOfParallelism = 2;

        public override void Action(HandlerInfo info, ExecuteData data)
        {
            var stopWatch = Stopwatch.StartNew();
            var handlerLogger = new FileLogWriter(info.HandlerId.ToString(), new Dictionary<string, string>() { ["Timing"] = "TIMING" });

            handlerLogger.Write(true, String.Format("The filtering of promoes began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");

            var context = new DatabaseContext();
            try
            {
                var dataFlowModuleCollection = new DataFlowModuleCollection(context);
                var dataFlowFilterCollection = new DataFlowFilterCollection(dataFlowModuleCollection);
                context.Database.CommandTimeout = 10000;

                var innerStopWatch = new Stopwatch();
                var settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                var statusesSetting = settingsManager.GetSetting<string>("NOT_CHECK_PROMO_STATUS_LIST", "Draft,Cancelled,Deleted,Closed");
                var statuses = statusesSetting.Split(',');

                var promoesToCheck = dataFlowModuleCollection.PromoDataFlowModule.Collection
                    .Where(x => !x.Disabled && x.StartDate.HasValue && !statuses.Contains(x.PromoStatusSystemName)).OrderBy(x => x.Number);

                var syncLock = new object();
                var promoesForRecalculating =  new List<PromoDataFlowModule.PromoDataFlowSimpleModel>();

                var promoesForRecalculatingForFilters = new ConcurrentDictionary<Guid, PromoDataFlowModule.PromoDataFlowSimpleModel>();
                foreach (var promo in promoesToCheck)
                {
                    promoesForRecalculatingForFilters.TryAdd(promo.Id, promo);
                }

                // --- ClientTree ---

                handlerLogger.Write(true, $"{nameof(ClientTreeDataFlowFilter)}: Amount of promoes to check: { promoesForRecalculatingForFilters.Count() }. " +
                    $"Amount of {nameof(ClientTree)} models: { dataFlowFilterCollection.ClientTreeDataFlowFilter.ChangedModels.Count() }", "Message");

                Parallel.ForEach(dataFlowFilterCollection.ClientTreeDataFlowFilter.ChangedModels.Where(x => !x.EndDate.HasValue), 
                    new ParallelOptions { MaxDegreeOfParallelism = this.MaxDegreeOfParallelism }, clientTree =>
                {
                    var applyResult = dataFlowFilterCollection.ClientTreeDataFlowFilter.Apply(clientTree, promoesForRecalculatingForFilters.Values);
                    if (applyResult.Item1.Count() > 0)
                    {
                        lock (syncLock)
                        {
                            promoesForRecalculating.AddRange(applyResult.Item1);
                            handlerLogger.Write(true, $"Promo numbers {string.Join(", ", applyResult.Item1.Select(x => x.Number))} were filtered by {applyResult.Item2}", "Message");
                        }

                        var promoDataFlowSimpleModel = new PromoDataFlowModule.PromoDataFlowSimpleModel();
                        foreach (var promoForRemoving in applyResult.Item1)
                        {
                            promoesForRecalculatingForFilters.TryRemove(promoForRemoving.Id, out promoDataFlowSimpleModel);
                        }
                    }
                });

                // --- ClientTreeBrandTech ---

                handlerLogger.Write(true, $"{nameof(ClientTreeBrandTechDataFlowFilter)}: Amount of promoes to check: { promoesForRecalculatingForFilters.Count() }. " +
                    $"Amount of {nameof(ClientTreeBrandTech)} models: { dataFlowFilterCollection.ClientTreeBrandTechDataFlowFilter.ChangedModels.Count() }", "Message");

                Parallel.ForEach(dataFlowFilterCollection.ClientTreeBrandTechDataFlowFilter.ChangedModels, 
                    new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism }, clientTreeBrandTech =>
                {
                    var applyResult = dataFlowFilterCollection.ClientTreeBrandTechDataFlowFilter.Apply(clientTreeBrandTech, promoesForRecalculatingForFilters.Values);
                    if (applyResult.Item1.Count() > 0)
                    {
                        lock (syncLock)
                        {
                            promoesForRecalculating.AddRange(applyResult.Item1);
                            handlerLogger.Write(true, $"Promo numbers {string.Join(", ", applyResult.Item1.Select(x => x.Number))} were filtered by {applyResult.Item2}", "Message");
                        }

                        var promoDataFlowSimpleModel = new PromoDataFlowModule.PromoDataFlowSimpleModel();
                        foreach (var promoForRemoving in applyResult.Item1)
                        {
                            promoesForRecalculatingForFilters.TryRemove(promoForRemoving.Id, out promoDataFlowSimpleModel);
                        }
                    }
                });

                // --- ProductTree ---

                handlerLogger.Write(true, $"{nameof(ProductTreeDataFlowFilter)}: Amount of promoes to check: { promoesForRecalculatingForFilters.Count() }. " +
                    $"Amount of {nameof(ProductTree)} models: { dataFlowFilterCollection.ProductTreeDataFlowFilter.ChangedModels.Count() }", "Message");

                Parallel.ForEach(dataFlowFilterCollection.ProductTreeDataFlowFilter.ChangedModels.Where(x => !x.EndDate.HasValue), 
                    new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism }, productTree =>
                {
                    var applyResult = dataFlowFilterCollection.ProductTreeDataFlowFilter.Apply(productTree, promoesForRecalculatingForFilters.Values);
                    if (applyResult.Item1.Count() > 0)
                    {
                        lock (syncLock)
                        {
                            promoesForRecalculating.AddRange(applyResult.Item1);
                            handlerLogger.Write(true, $"Promo numbers {string.Join(", ", applyResult.Item1.Select(x => x.Number))} were filtered by {applyResult.Item2}", "Message");
                        }

                        var promoDataFlowSimpleModel = new PromoDataFlowModule.PromoDataFlowSimpleModel();
                        foreach (var promoForRemoving in applyResult.Item1)
                        {
                            promoesForRecalculatingForFilters.TryRemove(promoForRemoving.Id, out promoDataFlowSimpleModel);
                        }
                    }
                });

                // --- IncrementalPromo ---

                handlerLogger.Write(true, $"{nameof(IncrementalPromoDataFlowFilter)}: Amount of promoes to check: { promoesForRecalculatingForFilters.Count() }. " +
                    $"Amount of {nameof(IncrementalPromo)} models: { dataFlowFilterCollection.IncrementalPromoDataFlowFilter.ChangedModels.Count() }", "Message");

                Parallel.ForEach(dataFlowFilterCollection.IncrementalPromoDataFlowFilter.ChangedModels.Where(x => !x.Disabled), 
                    new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism }, incrementalPromo =>
                {
                    var applyResult = dataFlowFilterCollection.IncrementalPromoDataFlowFilter.Apply(incrementalPromo, promoesForRecalculatingForFilters.Values);
                    if (applyResult.Item1.Count() > 0)
                    {
                        lock (syncLock)
                        {
                            promoesForRecalculating.AddRange(applyResult.Item1);
                            handlerLogger.Write(true, $"Promo numbers {string.Join(", ", applyResult.Item1.Select(x => x.Number))} were filtered by {applyResult.Item2}", "Message");
                        }

                        var promoDataFlowSimpleModel = new PromoDataFlowModule.PromoDataFlowSimpleModel();
                        foreach (var promoForRemoving in applyResult.Item1)
                        {
                            promoesForRecalculatingForFilters.TryRemove(promoForRemoving.Id, out promoDataFlowSimpleModel);
                        }
                    }
                });

                // --- AssortmentMatrix ---

                handlerLogger.Write(true, $"{nameof(AssortmentMatrixDataFlowFilter)}: Amount of promoes to check: { promoesForRecalculatingForFilters.Count() }. " +
                    $"Amount of {nameof(AssortmentMatrix)} models: { dataFlowFilterCollection.AssortmentMatrixDataFlowFilter.ChangedModels.Count() }", "Message");

                Parallel.ForEach(dataFlowFilterCollection.AssortmentMatrixDataFlowFilter.ChangedModels, 
                    new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism }, assortmentMatrix =>
                {
                    var applyResult = dataFlowFilterCollection.AssortmentMatrixDataFlowFilter.Apply(assortmentMatrix, promoesForRecalculatingForFilters.Values);
                    if (applyResult.Item1.Count() > 0)
                    {
                        lock (syncLock)
                        {
                            promoesForRecalculating.AddRange(applyResult.Item1);
                            handlerLogger.Write(true, $"Promo numbers {string.Join(", ", applyResult.Item1.Select(x => x.Number))} were filtered by {applyResult.Item2}", "Message");
                        }

                        var promoDataFlowSimpleModel = new PromoDataFlowModule.PromoDataFlowSimpleModel();
                        foreach (var promoForRemoving in applyResult.Item1)
                        {
                            promoesForRecalculatingForFilters.TryRemove(promoForRemoving.Id, out promoDataFlowSimpleModel);
                        }
                    }
                });

                // --- COGS ---

                handlerLogger.Write(true, $"{nameof(COGSDataFlowFilter)}: Amount of promoes to check: { promoesForRecalculatingForFilters.Count() }. " +
                    $"Amount of {nameof(COGS)} models: { dataFlowFilterCollection.COGSDataFlowFilter.ChangedModels.Count() }", "Message");

                Parallel.ForEach(dataFlowFilterCollection.COGSDataFlowFilter.ChangedModels, 
                    new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism }, cogs =>
                {
                    var applyResult = dataFlowFilterCollection.COGSDataFlowFilter.Apply(cogs, promoesForRecalculatingForFilters.Values);
                    if (applyResult.Item1.Count() > 0)
                    {
                        lock (syncLock)
                        {
                            promoesForRecalculating.AddRange(applyResult.Item1);
                            handlerLogger.Write(true, $"Promo numbers {string.Join(", ", applyResult.Item1.Select(x => x.Number))} were filtered by {applyResult.Item2}", "Message");
                        }

                        var promoDataFlowSimpleModel = new PromoDataFlowModule.PromoDataFlowSimpleModel();
                        foreach (var promoForRemoving in applyResult.Item1)
                        {
                            promoesForRecalculatingForFilters.TryRemove(promoForRemoving.Id, out promoDataFlowSimpleModel);
                        }
                    }
                });

                // --- TradeInvestment ---

                handlerLogger.Write(true, $"{nameof(TradeInvestmentDataFlowFilter)}: Amount of promoes to check: { promoesForRecalculatingForFilters.Count() }. " +
                    $"Amount of {nameof(TradeInvestment)} models: { dataFlowFilterCollection.TradeInvestmentDataFlowFilter.ChangedModels.Count() }", "Message");

                Parallel.ForEach(dataFlowFilterCollection.TradeInvestmentDataFlowFilter.ChangedModels, 
                    new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism }, ti =>
                {
                    var applyResult = dataFlowFilterCollection.TradeInvestmentDataFlowFilter.Apply(ti, promoesForRecalculatingForFilters.Values);
                    if (applyResult.Item1.Count() > 0)
                    {
                        lock (syncLock)
                        {
                            promoesForRecalculating.AddRange(applyResult.Item1);
                            handlerLogger.Write(true, $"Promo numbers {string.Join(", ", applyResult.Item1.Select(x => x.Number))} were filtered by {applyResult.Item2}", "Message");
                        }

                        var promoDataFlowSimpleModel = new PromoDataFlowModule.PromoDataFlowSimpleModel();
                        foreach (var promoForRemoving in applyResult.Item1)
                        {
                            promoesForRecalculatingForFilters.TryRemove(promoForRemoving.Id, out promoDataFlowSimpleModel);
                        }
                    }
                });

                // --- PromoProductsCorrection ---

                handlerLogger.Write(true, $"{nameof(PromoProductsCorrectionDataFlowFilter)}: Amount of promoes to check: { promoesForRecalculatingForFilters.Count() }. " +
                    $"Amount of {nameof(PromoProductsCorrection)} models: { dataFlowFilterCollection.PromoProductsCorrectionDataFlowFilter.ChangedModels.Count() }", "Message");

                Parallel.ForEach(dataFlowFilterCollection.PromoProductsCorrectionDataFlowFilter.ChangedModels,
                    new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism }, promoProductsCorrection =>
                    {
                        var applyResult = dataFlowFilterCollection.PromoProductsCorrectionDataFlowFilter.Apply(promoProductsCorrection, promoesForRecalculatingForFilters.Values);
                        if (applyResult.Item1.Count() > 0)
                        {
                            lock (syncLock)
                            {
                                promoesForRecalculating.AddRange(applyResult.Item1);
                                handlerLogger.Write(true, $"Promo numbers {string.Join(", ", applyResult.Item1.Select(x => x.Number))} were filtered by {applyResult.Item2}", "Message");
                            }

                            var promoDataFlowSimpleModel = new PromoDataFlowModule.PromoDataFlowSimpleModel();
                            foreach (var promoForRemoving in applyResult.Item1)
                            {
                                promoesForRecalculatingForFilters.TryRemove(promoForRemoving.Id, out promoDataFlowSimpleModel);
                            }
                        }
                    });

                // --- BaseLine ---

                handlerLogger.Write(true, $"{nameof(BaseLineDataFlowFilter)}: Amount of promoes to check: { promoesForRecalculatingForFilters.Count() }. " +
                    $"Amount of {nameof(BaseLine)} models: { dataFlowFilterCollection.BaseLineDataFlowFilter.ChangedModels.Count() }", "Message");

                Parallel.ForEach(dataFlowFilterCollection.BaseLineDataFlowFilter.ChangedModels, 
                    new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism }, baseLine =>
                {
                    var applyResult = dataFlowFilterCollection.BaseLineDataFlowFilter.Apply(baseLine, promoesForRecalculatingForFilters.Values);
                    if (applyResult.Item1.Count() > 0)
                    {
                        lock (syncLock)
                        {
                            promoesForRecalculating.AddRange(applyResult.Item1);
                            handlerLogger.Write(true, $"Promo numbers {string.Join(", ", applyResult.Item1.Select(x => x.Number))} were filtered by {applyResult.Item2}", "Message");
                        }

                        var promoDataFlowSimpleModel = new PromoDataFlowModule.PromoDataFlowSimpleModel();
                        foreach (var promoForRemoving in applyResult.Item1)
                        {
                            promoesForRecalculatingForFilters.TryRemove(promoForRemoving.Id, out promoDataFlowSimpleModel);
                        }
                    }
                });

                promoesForRecalculating = promoesForRecalculating.Distinct().ToList();

                // Список промо, набор продуктов в которых будет изменен.
                var changedProductsPromoes = Products.GetChangedProductsPromoes(context, promoesToCheck.Where(x => x.PromoStatusSystemName != "Started" && x.PromoStatusSystemName != "Finished").ToList(), handlerLogger);
                promoesForRecalculating = promoesForRecalculating.Union(changedProductsPromoes).Distinct().ToList();

                //список Id промо для пересчета параметров до Plan Promo LSV
                List<Guid> promoIdsForRecalculating = promoesForRecalculating.Select(x => x.Id).ToList();

                handlerLogger.Write(true, String.Format("The budgets filtering of promoes began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
                innerStopWatch.Restart();
                //список Id промо, для пересчета бюджетов
                List<Guid> promoIdsForBudgetRecalclating = new List<Guid>();
                List<Guid> promoIdsForBTLBudgetRecalclating = new List<Guid>();
                foreach (var promo in promoesForRecalculating)
                {
                    List<Guid> linkedPromoIds = BudgetsPromoCalculation.GetLinkedPromoId(promo.Id, context).ToList();
                    promoIdsForBudgetRecalclating = promoIdsForBudgetRecalclating.Union(linkedPromoIds).ToList();
                    string btlId = context.Set<BTLPromo>().Where(x => x.PromoId == promo.Id && !x.Disabled && x.DeletedDate == null).FirstOrDefault()?.BTLId.ToString();
                    if (!string.IsNullOrEmpty(btlId))
                    {
                        promoIdsForBTLBudgetRecalclating = promoIdsForBTLBudgetRecalclating.Union(BudgetsPromoCalculation.GetLinkedPromoId(btlId, context)).ToList();
                    }
                }
                handlerLogger.Write(true, $"The budgets filtering of promoes duration:{innerStopWatch.Elapsed.Hours} hours and {innerStopWatch.Elapsed.Minutes} minutes and {innerStopWatch.Elapsed.Seconds} seconds", "Timing");
                handlerLogger.Write(true, String.Format("The budgets filtering of promoes ended at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");

                //список Id промо для полномасштабного пересчета(второй этап)
                List<Guid> promoIdsForAllRecalculating = promoesToCheck.Select(x => x.Id).ToList();

                //список Id промо за предыдущий год для пересчета фактических параметров с учетом Actual COGS и TI
                var actualCOGSChangesIncidents = context.Set<ChangesIncident>().Where(x => x.DirectoryName == nameof(Promo) + nameof(ActualCOGS) && !x.ProcessDate.HasValue).ToList();
                var actualCOGSPromoIds = GetPromoIds(actualCOGSChangesIncidents);
                var actualTIChangesIncidents = context.Set<ChangesIncident>().Where(x => x.DirectoryName == nameof(Promo) + nameof(ActualTradeInvestment) && !x.ProcessDate.HasValue).ToList();
                var actualTIPromoIds = GetPromoIds(actualTIChangesIncidents);

                List<Guid> promoIdsForRecalculatingActualOnly = actualCOGSPromoIds.Union(actualTIPromoIds).ToList();

                //список Id промо для блокировки на время пересчета
                List<Guid> promoIdsForBlock = promoIdsForRecalculating.Union(promoIdsForBudgetRecalclating)
                                                                      .Union(promoIdsForBTLBudgetRecalclating)
                                                                      .Union(promoIdsForAllRecalculating)
                                                                      .Union(actualCOGSPromoIds)
                                                                      .Union(actualTIPromoIds)
                                                                      .Distinct().ToList();

                HandlerData handlerData = new HandlerData();
                HandlerDataHelper.SaveIncomingArgument("PromoIdsForRecalculating", promoIdsForRecalculating, handlerData, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("PromoIdsForBudgetRecalclating", promoIdsForBudgetRecalclating, handlerData, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("PromoIdsForBTLBudgetRecalclating", promoIdsForBTLBudgetRecalclating, handlerData, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("PromoIdsForAllRecalculating", promoIdsForAllRecalculating, handlerData, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ActualCOGSPromoIds", actualCOGSPromoIds, handlerData, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ActualTIPromoIds", actualTIPromoIds, handlerData, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("PromoIdsForRecalculatingActualOnly", promoIdsForRecalculatingActualOnly, handlerData, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("PromoIdsForBlock", promoIdsForBlock, handlerData, visible: false, throwIfNotExists: false);

                handlerLogger.Write(true, $"The selection of promoes duration:  {stopWatch.Elapsed.Hours} hours and {stopWatch.Elapsed.Minutes} minutes and {stopWatch.Elapsed.Seconds} seconds.", "Timing");
                handlerLogger.Write(true, String.Format("The selection of promoes ended at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
                handlerLogger.Write(true, "The task for recalculating of promoes will be created in a few seconds.", "Message");

                CalculationTaskManager.CreateCalculationTask(CalculationTaskManager.CalculationAction.DataFlow, handlerData, context);
                handlerLogger.Write(true, "The task for recalculating of promoes was created.", "Message");
            }
            catch (Exception e)
            {
                data.SetValue<bool>("HasErrors", true);
                logger.Error(e);

                handlerLogger.Write(true, String.Format("The selection of promoes ended with errors at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
                handlerLogger.Write(true, e.ToString(), "Error");
                throw;
            }
            finally
            {
                if (context != null)
                {
                    context.SaveChanges();
                    ((IDisposable)context).Dispose();
                }

                stopWatch.Stop();
                handlerLogger.Write(true, String.Format("The filtering of promoes ended at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
            }
        }

        List<Guid> GetPromoIds(IEnumerable<ChangesIncident> changesIncidents)
        {
            return changesIncidents.Select(x =>
            {
                Guid itemGuidId;
                bool successParse = Guid.TryParse(x.ItemId, out itemGuidId);
                return new { itemGuidId, successParse };
            }).Where(x => x.successParse).Select(x => x.itemGuidId).ToList();
        }
    }

    /// <summary>
    /// Класс для установки актуальных продуктов для промо.
    /// </summary>
    static class Products
    {
        /// <summary>
        /// Установить актуальный набор продуктов для регулярного промо.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="promoes"></param>
        public static List<PromoDataFlowModule.PromoDataFlowSimpleModel> GetChangedProductsPromoes(
            DatabaseContext context, List<PromoDataFlowModule.PromoDataFlowSimpleModel> promoes, ILogWriter handlerLogger)
        {
            handlerLogger.Write(true, String.Format("The setting of actual products for promoes began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
            var stopWatch = Stopwatch.StartNew();

            var promoesForRecalculation = new List<PromoDataFlowModule.PromoDataFlowSimpleModel>();
            // ПЕРЕДЕЛАТЬ НА ChangesIncident
            var productChangeIncidents = context.Set<ProductChangeIncident>().Where(x => x.RecalculationProcessDate == null);
            var productChangeIncidentsActual = productChangeIncidents.GroupBy(x => new { x.ProductId, x.IsCreate, x.IsDelete }).Select(x => x.FirstOrDefault());

            if (productChangeIncidentsActual.Count() > 0)
            {
                var productChangeIncidentsChanged = productChangeIncidentsActual.Where(x => !x.IsCreate && !x.IsDelete).Select(x => x.Product).ToList();
                var productChangeIncidentsCreated = productChangeIncidentsActual.Where(x => x.IsCreate).Select(x => x.Product).ToList();
                var productChangeIncidentsDeleted = productChangeIncidentsActual.Where(x => x.IsDelete).Select(x => x.Product).ToList();
                var productsFromAssortmentMatrix = context.Set<AssortmentMatrix>().Where(x => !x.Disabled).Select(x => x.Product).ToList();

                foreach (var promo in promoes)
                {
                    if (!promo.InOut.HasValue || !promo.InOut.Value)
                    {
                        var productsForAdding = new List<Product>();
                        var productsForDeleting = new List<Product>();

                        var promoProductTrees = context.Set<PromoProductTree>().Where(x => x.PromoId == promo.Id && !x.Disabled).ToList();
                        var productTrees = context.Set<ProductTree>().ToList().Where(x => x.EndDate == null && promoProductTrees.Any(y => y.ProductTreeObjectId == x.ObjectId));

                        var expressionsList = new List<Func<Product, bool>>();
                        try
                        {
                            expressionsList = GetExpressionList(productTrees);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            // На случай некорректного фильтра
                        }

                        if (expressionsList.Count > 0)
                        {
                            // Измененные / созданные  / удаленные продукты, подходящие под фильтр для текущего промо
                            var filteredProductListChanged = productChangeIncidentsChanged.Where(p => expressionsList.Any(e => e.Invoke(p))).ToList();
                            var filteredProductListCreated = productChangeIncidentsCreated.Where(p => expressionsList.Any(e => e.Invoke(p))).ToList();
                            var filteredProductListDeleted = productChangeIncidentsDeleted.Where(p => expressionsList.Any(e => e.Invoke(p))).ToList();

                            var promoProducts = context.Set<PromoProduct>().Where(x => x.PromoId == promo.Id).ToList();
                            productsForAdding = filteredProductListCreated.Intersect(productsFromAssortmentMatrix).Distinct().ToList();

                            productsForDeleting.AddRange(filteredProductListDeleted);
                            productsForDeleting = productsForDeleting.Intersect(productsFromAssortmentMatrix).Distinct().ToList();

                            // Продукты, которые после изменения больше не подходят под текущие промо
                            var changedPromoProducts = productChangeIncidentsChanged.Where(x => promoProducts.Select(y => y.ProductId).Contains(x.Id)).ToList();
                            if (changedPromoProducts.Any())
                            {
                                var sameProducts = changedPromoProducts.Where(p => expressionsList.Any(e => e.Invoke(p))).ToList();
                                var changed = productChangeIncidentsChanged.Where(p => !sameProducts.Contains(p)).ToList();
                                if (changed.Any())
                                {
                                    productsForDeleting.AddRange(changed);
                                }
                            }
                            // Продукты, котрорые после изменения подходят под новые промо
                            var changedNotPromoProducts = productChangeIncidentsChanged.Where(p => !promoProducts.Select(y => y.ProductId).Contains(p.Id)).ToList();
                            if (changedNotPromoProducts.Any())
                            {
                                var newProductsForPromo = changedNotPromoProducts.Where(p => expressionsList.Any(e => e.Invoke(p))).ToList();
                                if (newProductsForPromo.Any())
                                {
                                    productsForAdding.AddRange(newProductsForPromo);
                                }
                            }

                            if (productsForAdding.Count > 0 || productsForDeleting.Count > 0 || filteredProductListChanged.Count > 0)
                            {
                                if (productsForAdding.Count > 0)
                                {
                                    handlerLogger.Write(true, $"Products will be added to promo number { promo.Number } if not yet added (ZREPs: { string.Join(", ", productsForAdding.Select(x => x.ZREP)) })", "Message");
                                }
                                if (productsForDeleting.Count > 0)
                                {
                                    handlerLogger.Write(true, $"Products will be removed from promo number { promo.Number } (ZREPs: { string.Join(", ", productsForDeleting.Select(x => x.ZREP)) })", "Message");
                                }
                                if (filteredProductListChanged.Count > 0)
                                {
                                    handlerLogger.Write(true, $"Products have been changed for promo number { promo.Number } (ZREPs: { string.Join(", ", filteredProductListChanged.Select(x => x.ZREP)) })", "Message");
                                }

                                promoesForRecalculation.Add(promo);
                            }
                        }
                    }
                    else
                    {
                        var promoProductsForCurrentPromo = context.Set<PromoProduct>().Where(x => x.PromoId == promo.Id);
                        var resultChangedProducts = new List<string>();
                        var resultProductsForDeleting = new List<string>();

                        foreach (var promoproductForCurrentPromo in promoProductsForCurrentPromo)
                        {
                            // Если среди измененных продуктов есть продукт, входящий в InOut промо, то это промо нужно пересчитать.
                            var changedProducts = productChangeIncidentsChanged.Where(x => x.Id == promoproductForCurrentPromo.ProductId).Select(x => x.ZREP);
                            var productsForDeleting = productChangeIncidentsDeleted.Where(x => x.Id == promoproductForCurrentPromo.ProductId).Select(x => x.ZREP);

                            if (productsForDeleting.Count() > 0)
                            {
                                resultProductsForDeleting.AddRange(productsForDeleting);
                            }
                            if (changedProducts.Count() > 0)
                            {
                                resultChangedProducts.AddRange(changedProducts);
                            }
                        }

                        if (resultProductsForDeleting.Count > 0 || resultChangedProducts.Count > 0)
                        {
                            if (resultProductsForDeleting.Count > 0)
                            {
                                handlerLogger.Write(true, $"Products will be removed for In-Out promo number { promo.Number } (ZREPs: { string.Join(", ", resultProductsForDeleting) })", "Message");
                            }
                            if (resultChangedProducts.Count > 0)
                            {
                                handlerLogger.Write(true, $"Products have been changed for In-Out promo number { promo.Number } (ZREPs: { string.Join(", ", resultChangedProducts) })", "Message");
                            }

                            promoesForRecalculation.Add(promo);
                        }
                    }
                }
            }

            stopWatch.Stop();
            handlerLogger.Write(true, $"The setting of actual products for promoes duration:{stopWatch.Elapsed.Hours} hours and {stopWatch.Elapsed.Minutes} minutes and {stopWatch.Elapsed.Seconds} seconds.", "Timing");
            handlerLogger.Write(true, String.Format("The setting of actual products for promoes ended at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
            return promoesForRecalculation;
        }

        private static List<Func<Product, bool>> GetExpressionList(IEnumerable<ProductTree> productTreeNodes)
        {
            var expressionsList = new List<Func<Product, bool>>();
            foreach (ProductTree node in productTreeNodes)
            {
                if (node != null && !String.IsNullOrEmpty(node.Filter))
                {
                    string stringFilter = node.Filter;
                    // Преобразованиестроки фильтра в соответствующий класс
                    FilterNode filter = stringFilter.ConvertToNode();
                    // Создание функции фильтрации на основе построенного фильтра
                    var expr = filter.ToExpressionTree<Product>();
                    expressionsList.Add(expr.Compile());
                }
            }
            return expressionsList;
        }
    }
}
