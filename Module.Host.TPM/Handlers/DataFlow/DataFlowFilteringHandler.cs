using Core.Dependency;
using Core.Settings;
using Looper.Core;
using Looper.Parameters;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Module.Persist.TPM.Utils.Filter;
using Persist;
using ProcessingHost.Handlers;
using System;
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
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            var stopWatch = Stopwatch.StartNew();
            ILogWriter handlerLogger = new FileLogWriter(info.HandlerId.ToString(), new Dictionary<string, string>() { ["Timing"] = "TIMING" });
            handlerLogger.Write(true, String.Format("The filtering of promoes began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
            var context = new DatabaseContext();
            bool successed = true;

            try
            {
                context.Database.CommandTimeout = 10000;

                var innerStopWatch = new Stopwatch();
                var settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                var statusesSetting = settingsManager.GetSetting<string>("NOT_CHECK_PROMO_STATUS_LIST", "Draft,Cancelled,Deleted,Closed");
                var statuses = statusesSetting.Split(',');
                var promoesToCheck = context.Set<Promo>().AsNoTracking()
                    .Where(x => !x.Disabled && x.StartDate.HasValue && !statuses.Contains(x.PromoStatus.SystemName)).ToList();

                // Список промо, набор продуктов в которых будет изменен.
                var changedProductsPromoes = Products.GetChangedProductsPromoes(context, promoesToCheck.Where(x => x.PromoStatus.SystemName != "Started" && x.PromoStatus.SystemName != "Finished").ToList(), handlerLogger);

                //список промо для пересчета параметров до Plan Promo LSV
                handlerLogger.Write(true, String.Format("The incident filtering of promoes began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
                innerStopWatch.Restart();
                FilterCollection.InitializeChangesIncidents(context, handlerLogger);

                var promoesForRecalculating = promoesToCheck.Where(x => FilterCollection.ApplyFilters(context, x, promoesToCheck.Count, handlerLogger));
                handlerLogger.Write(true, $"The incident filtering of promoes duration:{innerStopWatch.Elapsed.Hours} hours and {innerStopWatch.Elapsed.Minutes} minutes and {innerStopWatch.Elapsed.Seconds} seconds", "Timing");
                handlerLogger.Write(true, String.Format("The incident filtering of promoes ended at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");

                promoesForRecalculating = promoesForRecalculating.Union(changedProductsPromoes).Distinct().ToList();

                //список Id промо для пересчета параметров до Plan Promo LSV
                List<Guid> promoIdsForRecalculating = promoesForRecalculating.Select(n => n.Id).ToList();

                handlerLogger.Write(true, String.Format("The budgets filtering of promoes began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
                innerStopWatch.Restart();
                //список Id промо, для пересчета бюджетов
                List<Guid> promoIdsForBudgetRecalclating = new List<Guid>();
                foreach (Promo promo in promoesForRecalculating)
                {
                    List<Guid> linkedPromoIds = BudgetsPromoCalculation.GetLinkedPromoId(promo.Id, context).ToList();
                    promoIdsForBudgetRecalclating = promoIdsForBudgetRecalclating.Union(linkedPromoIds).ToList();
                }

                var promoIdsForBudgetRecalclatingbyIncident = FilterCollection.LinkedPromoIds;
                if (promoIdsForBudgetRecalclatingbyIncident.Count() > 0)
                {
                    promoIdsForBudgetRecalclating = promoIdsForBudgetRecalclating.Union(promoIdsForBudgetRecalclatingbyIncident).ToList();
                }

                handlerLogger.Write(true, $"The budgets filtering of promoes duration:{innerStopWatch.Elapsed.Hours} hours and {innerStopWatch.Elapsed.Minutes} minutes and {innerStopWatch.Elapsed.Seconds} seconds", "Timing");
                handlerLogger.Write(true, String.Format("The budgets filtering of promoes ended at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");

                //список Id промо для полномасштабного пересчета(второй этап)
                List<Guid> promoIdsForAllRecalculating = promoesToCheck.Select(x => x.Id).ToList();

                //список Id промо для блокировки на время пересчета
                List<Guid> promoIdsForBlock = promoIdsForRecalculating.Union(promoIdsForBudgetRecalclating)
                                                                        .Union(promoIdsForAllRecalculating).Distinct().ToList();

                HandlerData handlerData = new HandlerData();
                HandlerDataHelper.SaveIncomingArgument("PromoIdsForRecalculating", promoIdsForRecalculating, handlerData, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("PromoIdsForBudgetRecalclating", promoIdsForBudgetRecalclating, handlerData, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("PromoIdsForAllRecalculating", promoIdsForAllRecalculating, handlerData, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("PromoIdsForBlock", promoIdsForBlock, handlerData, visible: false, throwIfNotExists: false);

                handlerLogger.Write(true, $"The selection of promoes duration:  {stopWatch.Elapsed.Hours} hours and {stopWatch.Elapsed.Minutes} minutes and {stopWatch.Elapsed.Seconds} seconds.", "Timing");
                handlerLogger.Write(true, String.Format("The selection of promoes ended at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
                handlerLogger.Write(true, "The task for recalculating of promoes will be created in a few seconds.", "Message");

                CalculationTaskManager.CreateCalculationTask(CalculationTaskManager.CalculationAction.DataFlow, handlerData, context);
                handlerLogger.Write(true, "The task for recalculating of promoes was created.", "Message");
            }
            catch (Exception e)
            {
                successed = false;
                data.SetValue<bool>("HasErrors", true);
                logger.Error(e);

                handlerLogger.Write(true, String.Format("The selection of promoes ended with errors at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
                handlerLogger.Write(true, e.ToString(), "Error");

                throw;
            }
            finally
            {
                if (successed)
                {
                    /*
                    var assortmentMatrixChangesIds = context.Set<ChangesIncident>()
                        .Where(x => x.ProcessDate == null)
                        .GroupBy(x => new { x.DirectoryName, x.ItemId })
                        .Where(x => x.Key.DirectoryName == "AssortmentMatrix")
                        .Select(x => x.Key.ItemId);

                    foreach (var assortmentMatrixChangesId in assortmentMatrixChangesIds)
                    {
                        var assortmentMatrixChangesGuidId = Guid.Parse(assortmentMatrixChangesId);
                        var assortmentMatrix = context.Set<AssortmentMatrix>().FirstOrDefault(x => x.Id == assortmentMatrixChangesGuidId && x.Disabled);
                        if (assortmentMatrix != null)
                        {
                            context.Set<AssortmentMatrix>().Remove(assortmentMatrix);
                        }
                    }
                    */

                    var changesIncidents = context.Set<ChangesIncident>().Where(x => x.ProcessDate == null);
                    foreach (var changesIncident in changesIncidents)
                    {
                        changesIncident.ProcessDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                        changesIncident.Disabled = false;
                    }
                }

                if (context != null)
                {
                    context.SaveChanges();
                    ((IDisposable)context).Dispose();
                }
                stopWatch.Stop();
                handlerLogger.Write(true, String.Format("The filtering of promoes ended at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
            }
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
        public static List<Promo> GetChangedProductsPromoes(DatabaseContext context, List<Promo> promoes, ILogWriter handlerLogger)
        {
            handlerLogger.Write(true, String.Format("The setting of actual products for promoes began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
            var stopWatch = Stopwatch.StartNew();

            var promoesForRecalculation = new List<Promo>();
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

                foreach (var productChangeIncident in productChangeIncidents)
                {
                    productChangeIncident.RecalculationProcessDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
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

    /// <summary>
    /// Класс фильтрации промо для ночного пересчета.
    /// </summary>
    static class FilterCollection
    {
        private static int Counter { get; set; }

        private static IEnumerable<Guid> BaseLineChangesGuidIds;
        private static IEnumerable<Guid> AssortmentMatrixChangesGuidIds;
        private static IEnumerable<int> ClientTreeChangesIntIds;
        private static IEnumerable<int> ProductTreeChangesIntIds;
        private static IEnumerable<Guid> IncrementalPromoChangesGuidIds;
        private static IEnumerable<Guid> PromoSupportChangesGuidIds;

        public static List<Guid> LinkedPromoIds { get; set; }

        public static void InitializeChangesIncidents(DatabaseContext context, ILogWriter handlerLogger)
        {
            BaseLineChangesGuidIds = new List<Guid>();
            AssortmentMatrixChangesGuidIds = new List<Guid>();
            ClientTreeChangesIntIds = new List<int>();
            ProductTreeChangesIntIds = new List<int>();
            IncrementalPromoChangesGuidIds = new List<Guid>();
            PromoSupportChangesGuidIds = new List<Guid>();

            LinkedPromoIds = new List<Guid>();

            handlerLogger.Write(true, String.Format("The initialize of incidents began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
            Counter = 0;
            var changesIds = context.Set<ChangesIncident>().AsNoTracking().Where(x => x.ProcessDate == null);
            handlerLogger.Write(true, $"Amount of incidents: {changesIds.Count()}", "Message");

            var uniqueChangesIds = changesIds
                .GroupBy(x => new { x.DirectoryName, x.ItemId })
                .Select(x => x.Key);

            handlerLogger.Write(true, $"Amount of unique incidents: {uniqueChangesIds.Count()}", "Message");

            // Base Line
            handlerLogger.Write(true, String.Format("The searching of unique Base Line incidents began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
            var baseLineChanges = uniqueChangesIds.Where(x => x.DirectoryName == "BaseLine");
            handlerLogger.Write(true, $"Amount of unique Base Line incidents: {baseLineChanges.Count()}", "Message");
            handlerLogger.Write(true, String.Format("The searching of unique Base Line incidents end at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");

            // Assortment Matrix
            handlerLogger.Write(true, String.Format("The searching of unique Assortment Matrix incidents began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
            var assortmentMatrixChanges = uniqueChangesIds.Where(x => x.DirectoryName == "AssortmentMatrix");
            handlerLogger.Write(true, $"Amount of unique Assortment Matrix incidents: {assortmentMatrixChanges.Count()}", "Message");
            handlerLogger.Write(true, String.Format("The searching of unique Assortment Matrix incidents end at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");

            // Client Tree
            handlerLogger.Write(true, String.Format("The searching of unique Client Tree incidents began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
            var clientTreeChanges = uniqueChangesIds.Where(x => x.DirectoryName == "ClientTree");
            handlerLogger.Write(true, $"Amount of unique Client Tree incidents: {clientTreeChanges.Count()}", "Message");
            handlerLogger.Write(true, String.Format("The searching of unique Client Tree incidents end at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");

            // Product Tree
            handlerLogger.Write(true, String.Format("The searching of unique Product Tree incidents began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
            var productTreeChanges = uniqueChangesIds.Where(x => x.DirectoryName == "ProductTree");
            handlerLogger.Write(true, $"Amount of unique Product Tree incidents: {productTreeChanges.Count()}", "Message");
            handlerLogger.Write(true, String.Format("The searching of unique Product Tree incidents end at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");

            // Incremental Promo
            handlerLogger.Write(true, String.Format("The searching of unique Incremental Promo incidents began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
            var incrementalPromoChanges = uniqueChangesIds.Where(x => x.DirectoryName == "IncrementalPromo");
            handlerLogger.Write(true, $"Amount of unique Incremental Promo incidents: {incrementalPromoChanges.Count()}", "Message");
            handlerLogger.Write(true, String.Format("The searching of unique Incremental Promo incidents end at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");

            // Promo support
            handlerLogger.Write(true, String.Format("The searching of unique Promo support incidents began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
            var promoSupportChanges = uniqueChangesIds.Where(x => x.DirectoryName == "PromoSupport");
            handlerLogger.Write(true, $"Amount of unique Promo support incidents: {promoSupportChanges.Count()}", "Message");
            handlerLogger.Write(true, String.Format("The searching of unique Promo support incidents end at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");

            // Base Line
            if (baseLineChanges.Any())
            {
                handlerLogger.Write(true, String.Format("The parsing of unique Base Line incidents began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
                BaseLineChangesGuidIds = baseLineChanges.AsParallel().Select(x =>
                {
                    Guid itemGuidId;
                    bool success = Guid.TryParse(x.ItemId, out itemGuidId);
                    return new { itemGuidId, success };
                })
                .Where(x => x.success).Select(x => x.itemGuidId).ToList();
                handlerLogger.Write(true, $"The count of parsed unique Base Line incidents: {BaseLineChangesGuidIds.Count()}", "Message");
                handlerLogger.Write(true, String.Format("The parsing of unique Base Line incidents ended at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
            }

            // Assortment Matrix
            if (assortmentMatrixChanges.Any())
            {
                handlerLogger.Write(true, String.Format("The parsing of unique Assortment Matrix incidents began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
                AssortmentMatrixChangesGuidIds = assortmentMatrixChanges.AsParallel().Select(x =>
                {
                    Guid itemGuidId;
                    bool success = Guid.TryParse(x.ItemId, out itemGuidId);
                    return new { itemGuidId, success };
                })
                .Where(x => x.success).Select(x => x.itemGuidId).ToList();
                handlerLogger.Write(true, $"The count of parsed unique Assortment Matrix incidents: {AssortmentMatrixChangesGuidIds.Count()}", "Message");
                handlerLogger.Write(true, String.Format("The parsing of unique Assortment Matrix incidents ended at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
            }

            // Client Tree
            if (clientTreeChanges.Any())
            {
                handlerLogger.Write(true, String.Format("The parsing of unique Client Tree incidents began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
                ClientTreeChangesIntIds = clientTreeChanges.AsParallel().Select(x =>
                {
                    int itemIntId;
                    bool success = Int32.TryParse(x.ItemId, out itemIntId);
                    return new { itemIntId, success };
                })
                .Where(x => x.success).Select(x => x.itemIntId).ToList();
                handlerLogger.Write(true, $"The count of parsed unique Client Tree incidents: {ClientTreeChangesIntIds.Count()}", "Message");
                handlerLogger.Write(true, String.Format("The parsing of unique Client Tree incidents ended at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
            }

            // Product Tree
            if (productTreeChanges.Any())
            {
                handlerLogger.Write(true, String.Format("The parsing of unique Product Tree incidents began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
                ProductTreeChangesIntIds = productTreeChanges.AsParallel().Select(x =>
                {
                    int itemIntId;
                    bool success = Int32.TryParse(x.ItemId, out itemIntId);
                    return new { itemIntId, success };
                })
                .Where(x => x.success).Select(x => x.itemIntId).ToList();
                handlerLogger.Write(true, $"The count of parsed unique Product Tree  incidents: {ProductTreeChangesIntIds.Count()}", "Message");
                handlerLogger.Write(true, String.Format("The parsing of unique Product Tree incidents ended at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
            }

            // Incremental Promo
            if (incrementalPromoChanges.Any())
            {
                handlerLogger.Write(true, String.Format("The parsing of unique Incremental Promo incidents began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
                IncrementalPromoChangesGuidIds = incrementalPromoChanges.AsParallel().Select(x =>
                {
                    Guid itemGuidId;
                    bool success = Guid.TryParse(x.ItemId, out itemGuidId);
                    return new { itemGuidId, success };
                })
                .Where(x => x.success).Select(x => x.itemGuidId).ToList();
                handlerLogger.Write(true, $"The count of parsed unique Incremental Promo incidents: {IncrementalPromoChangesGuidIds.Count()}", "Message");
                handlerLogger.Write(true, String.Format("The parsing of unique Incremental Promo incidents ended at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
            }

            // Promo support
            if (promoSupportChanges.Any())
            {
                handlerLogger.Write(true, String.Format("The parsing of unique Promo support incidents began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
                PromoSupportChangesGuidIds = promoSupportChanges.AsParallel().Select(x =>
                {
                    Guid itemGuidId;
                    bool success = Guid.TryParse(x.ItemId, out itemGuidId);
                    return new { itemGuidId, success };
                })
                .Where(x => x.success).Select(x => x.itemGuidId).ToList();
                handlerLogger.Write(true, $"The count of parsed unique Promo support incidents: {PromoSupportChangesGuidIds.Count()}", "Message");
                handlerLogger.Write(true, String.Format("The parsing of unique Promo support incidents ended at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
            }

            LinkedPromoIds = BudgetsPromoCalculation.GetLinkedPromoId(PromoSupportChangesGuidIds.ToList(), context);

            handlerLogger.Write(true, String.Format("The initialize of incidents ended at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
        }

        public static bool ApplyFilters(DatabaseContext context, Promo promo, int promoesAmount, ILogWriter handlerLogger)
        {
            handlerLogger.Write(true, $"Filtering promo number { promo.Number } ({ ++Counter } / { promoesAmount })", "Message");

            return IncrementalPromo(context, promo, handlerLogger)
                   || AssortmentMatrix(context, promo, handlerLogger)
                   || ClientTree(context, promo, handlerLogger)
                   || ProductTree(context, promo, handlerLogger)
                   || BaseLine(context, promo, handlerLogger);
        }

        /// <summary>
        /// 1. StartDate в BaseLine лежит в промежутке дат промо (включительно). 
        /// 2. В промо входит продукт с таким же ZREP как и в Baseline.
        /// 3. Клиент в BaseLine cовпадает с клиентом в промо (смотрим по всей иерархии).
        /// При выполнении этих всех этих условий нужно пересчитать промо.
        /// </summary>
        /// <param name="promo">Модель промо.</param>
        /// <returns>Нужно ли пересчитывать данное промо?</returns>
        public static bool BaseLine(DatabaseContext context, Promo promo, ILogWriter handlerLogger)
        {
            foreach (var baseLineChangesGuidId in BaseLineChangesGuidIds)
            {
                var baseLine = context.Set<BaseLine>().FirstOrDefault(x => x.Id == baseLineChangesGuidId);
                if (baseLine != null)
                {
                    // Пункт номер 1 из комментария к методу. 
                    var baseLineStartDateBeetweenPromoDates = !(promo.StartDate > baseLine.StartDate.Value.AddDays(6) || promo.EndDate < baseLine.StartDate);
                    if (baseLineStartDateBeetweenPromoDates)
                    {
                        // Пункт номер 2 из комментария к методу. 
                        var promoProductContainsBaseLineProduct = context.Set<PromoProduct>()
                            .Any(x => x.PromoId == promo.Id && x.ProductId == baseLine.ProductId && !x.Disabled);

                        if (promoProductContainsBaseLineProduct)
                        {
                            // Пункт номер 3 из комментария к методу. 
                            var clientTree = context.Set<ClientTree>().FirstOrDefault(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue);
                            if (clientTree != null)
                            {
                                while (clientTree != null && clientTree.Type.ToLower() != "root")
                                {
                                    if (clientTree.DemandCode == baseLine.DemandCode)
                                    {
                                        break;
                                    }

                                    clientTree = context.Set<ClientTree>().FirstOrDefault(x => x.ObjectId == clientTree.parentId && !x.EndDate.HasValue);
                                }

                                var clientTreeHierarchyContainsBaseLineClient = clientTree.DemandCode == baseLine.DemandCode;
                                if (clientTreeHierarchyContainsBaseLineClient)
                                {
                                    handlerLogger.Write(true, $"Promo number { promo.Number } was filtered by Base Line.", "Message");
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 1. Клиент в промо такой же как и в ассортиментной матрице.
        /// 2. Даты промо находятся в промежутке дат из ассортиментной матрицы.
        /// При выполнении этих всех этих условий нужно пересчитать промо.
        /// </summary>
        /// <param name="promo">Модель промо.</param>
        /// <returns>Нужно ли пересчитывать данное промо?</returns>
        public static bool AssortmentMatrix(DatabaseContext context, Promo promo, ILogWriter handlerLogger)
        {
            foreach (var assortmentMatrixChangesGuidId in AssortmentMatrixChangesGuidIds)
            {
                var assortmentMatrix = context.Set<AssortmentMatrix>().FirstOrDefault(x => x.Id == assortmentMatrixChangesGuidId && !x.Disabled);
                // Если запись добавляется
                if (assortmentMatrix != null)
                {
                    // Пункт номер 1 из комментария к методу. 
                    var matrixClientEqualsPromoClient = assortmentMatrix.ClientTreeId == promo.ClientTreeKeyId;
                    if (matrixClientEqualsPromoClient)
                    {
                        // Пункт номер 2 из комментария к методу. 
                        var promoDatesBeetweenMatrixDates = promo.DispatchesStart >= assortmentMatrix.StartDate &&
                            promo.DispatchesEnd <= assortmentMatrix.EndDate;

                        if (promoDatesBeetweenMatrixDates)
                        {
                            handlerLogger.Write(true, $"Promo number { promo.Number } was filtered by Assortment Matrix.", "Message");
                            return true;
                        }
                    }
                }
                // Если запись удаляется
                else if (context.Set<AssortmentMatrix>().Where(x => x.Id == assortmentMatrixChangesGuidId && x.Disabled).Count() > 0)
                {
                    handlerLogger.Write(true, $"Promo number { promo.Number } was filtered by Assortment Matrix.", "Message");
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 1. Если изменилась доля у любого выбранного узла.
        /// При выполнении этих всех этих условий нужно пересчитать промо.
        /// </summary>
        /// <param name="promo">Модель промо.</param>
        /// <returns>Нужно ли пересчитывать данное промо?</returns>
        public static bool ClientTree(DatabaseContext context, Promo promo, ILogWriter handlerLogger)
        {
            foreach (var clientTreeChangesIntId in ClientTreeChangesIntIds)
            {
                var clientTree = context.Set<ClientTree>().FirstOrDefault(x => x.Id == clientTreeChangesIntId && !x.EndDate.HasValue);
                if (clientTree != null)
                {
                    var clientTreeShareChanged = context.Set<ClientTree>()
                        .Any(x => x.Id == promo.ClientTreeKeyId && x.Id == clientTree.Id && !x.EndDate.HasValue);

                    if (clientTreeShareChanged)
                    {
                        handlerLogger.Write(true, $"Promo number { promo.Number } was filtered by Client Tree.", "Message");
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 1. Если изменился фильтр у любого выбранного узла.
        /// 2. Промо не In-Out 
        /// При выполнении этих всех этих условий нужно пересчитать промо.
        /// </summary>
        /// <param name="promo">Модель промо.</param>
        /// <returns>Нужно ли пересчитывать данное промо?</returns>
        public static bool ProductTree(DatabaseContext context, Promo promo, ILogWriter handlerLogger)
        {
            if (!promo.InOut.HasValue || !promo.InOut.Value)
            {
                foreach (var productTreeChangesIntId in ProductTreeChangesIntIds)
                {
                    var productTree = context.Set<ProductTree>().FirstOrDefault(x => x.Id == productTreeChangesIntId && !x.EndDate.HasValue);
                    if (productTree != null)
                    {
                        var productTreeFilterChanged = context.Set<PromoProductTree>()
                            .Any(x => x.PromoId == promo.Id && x.ProductTreeObjectId == productTree.ObjectId && !x.Disabled);

                        if (productTreeFilterChanged)
                        {
                            handlerLogger.Write(true, $"Promo number { promo.Number } was filtered by Product Tree.", "Message");
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 1. Если обновилась запись в таблице IncrementalPromo, прикрепленная к какому-то промо, то это промо нужно пересчитать.
        /// </summary>
        /// <param name="promo">Модель промо</param>
        /// <returns>Нужно ли пересчитывать данное промо?</returns>
        public static bool IncrementalPromo(DatabaseContext context, Promo promo, ILogWriter handlerLogger)
        {

            if (promo.InOut.HasValue && promo.InOut.Value)
            {
                foreach (var incrementalPromoChangeGuidId in IncrementalPromoChangesGuidIds)
                {
                    var incrementalPromo = context.Set<IncrementalPromo>().FirstOrDefault(x => x.Id == incrementalPromoChangeGuidId && !x.Disabled);
                    if (incrementalPromo != null)
                    {
                        // Пункт номер 1 из комментария к методу. 
                        if (promo.Id == incrementalPromo.PromoId)
                        {
                            handlerLogger.Write(true, $"Promo number { promo.Number } was filtered by Incremental Promo.", "Message");
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
