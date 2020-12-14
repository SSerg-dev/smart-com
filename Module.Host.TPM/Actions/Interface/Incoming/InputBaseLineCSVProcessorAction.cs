using Core.Data;
using Core.Extensions;
using Core.History;
using Core.Security.Models;
using Core.Settings;
using Interfaces.Implementation.Action;
using Interfaces.Implementation.Inbound.Processor;
using Looper.Core;
using Looper.Parameters;
using Microsoft.Ajax.Utilities;
using Module.Frontend.TPM.Util;
using Module.Host.TPM.Handlers.MainNightProcessing;
using Module.Host.TPM.Util.DispatcherLogger;
using Module.Host.TPM.Util.Interface;
using Module.Host.TPM.Util.Model;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using NLog;
using Persist;
using Persist.Model;
using Persist.Model.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utility.LogWriter;

namespace Module.Host.TPM.Actions.Interface.Incoming
{
    class InputBaseLineCSVProcessorAction : CSVProcessorAction
    {
        private Logger actionLogger;
        Stopwatch stopwatch = new Stopwatch();

        protected HandlerInfo HandlerInfo;

        double validateDur = 0, convertDur = 0, dmdDur = 0, SalesORGDur = 0, ProductDur = 0, SalesDISTDur = 0, clientDur = 0, qtyDur = 0, sundayDur = 0, priceDur = 0, zeroQtyDur = 0;
        double createBLDur = 0, needRecDur = 0;

        public InputBaseLineCSVProcessorAction(HandlerInfo info, Guid interfaceId, FileBufferModel fileBufferModel)
            : base(interfaceId, fileBufferModel, typeof(InputBaseline), typeof(InputBaseline), null)
        {
            HandlerInfo = info;
            FileBufferTakeCount = 30;
            actionLogger = LogManager.GetCurrentClassLogger();
            actionLogger.Info("File processing begin: {0}", DateTimeOffset.Now.ToString());
            AllowPartialApply = true;
        }

        public override void Execute()
        {
            logger.Trace("Begin");
            try
            {
                // Получить запись или список записей
                IEnumerable<FileBuffer> fileBufferList = GetFileBufferList();
                fileBufferList = fileBufferList.OrderBy(x => x.FileName);
                foreach (FileBuffer fileBuffer in fileBufferList)
                {
                    Results.Add("Starting processing file " + fileBuffer.FileName + " at " + DateTimeOffset.Now, null);
                    ProcessFileBuffer(fileBuffer);
                }
                Success();
                HandlerDataHelper.SaveOutcomingArgument<int>("fileCount", fileBufferList.Count(), HandlerInfo.Data);
            }
            catch (Exception e)
            {
                logger.Error(e, "Error during Processing files interface '{0}'", InterfaceId);
                Errors.Add(String.Format("Error during Processing files interface '{0}': {1}", InterfaceId, e.ToString()));
                Fail();
            }
            finally
            {
                logger.Trace("Finish");
                string mainNightProcessingStepPrefix = AppSettingsManager.GetSetting<string>("MAIN_NIGHT_PROCESSING_STEP_PREFIX", "MainNightProcessingStep");
                using (DatabaseContext context = new DatabaseContext())
                {
                    MainNightProcessingHelper.SetProcessingFlagDown(context, mainNightProcessingStepPrefix);
                }
            }
        }

        protected override void ProcessFileBuffer(FileBuffer fileBuffer)
        {
            stopwatch.Restart();
            logger.Trace("Begin processing FileBuffer '{0}'", fileBuffer.Id);
            hasErrors = false;
            hasWarnings = false;
            try
            {
                IList<IEntity<Guid>> modelList;
                List<BaseLine> createBaselineList;
                List<BaseLine> updateBaselineList;
                List<Guid> baselineIdsToRecalculateList;

                List<Tuple<IEntity<Guid>, IEntity<Guid>>> toHisCreate;
                List<Tuple<IEntity<Guid>, IEntity<Guid>>> toHisUpdate;

                IList<Tuple<IEntity<Guid>, string>> dtoErrorList;
                IList<Tuple<string, string>> buildErrorList;
                IList<IEntity<Guid>> successList;
                IList<IEntity<Guid>> successRecordsList;
                IList<Tuple<IEntity<Guid>, string>> errorList;
                List<Tuple<IEntity<Guid>, string>> errorFilterList = new List<Tuple<IEntity<Guid>, string>>();
                IList<Tuple<IEntity<Guid>, string>> warningList;

                ConcurrentBag<Tuple<IEntity<Guid>, string>> errorRecords = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();
                ConcurrentBag<Tuple<IEntity<Guid>, string>> warningRecords = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();
                ConcurrentBag<Tuple<IEntity<Guid>, string>> errorRecordsForFile = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();
                ConcurrentBag<Tuple<IEntity<Guid>, string>> warningRecordsForFile = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();
                BaseDispatcherLogger dispatcherLogger = new BaseDispatcherLogger();

                ConcurrentBag<IEntity<Guid>> successRecords = new ConcurrentBag<IEntity<Guid>>();
                ConcurrentBag<BaseLine> modelsToCreate = new ConcurrentBag<BaseLine>();
                ConcurrentBag<BaseLine> modelsToUpdate = new ConcurrentBag<BaseLine>();
                ConcurrentBag<Guid> needRecalculateBaselineIds = new ConcurrentBag<Guid>();

                ConcurrentBag<BaseLine> createBaselineListAsync = new ConcurrentBag<BaseLine>();
                ConcurrentBag<BaseLine> updateBaselineListAsync = new ConcurrentBag<BaseLine>();
                var toHisCreateAsync = new ConcurrentBag<Tuple<IEntity<Guid>, IEntity<Guid>>>();
                var toHisUpdateAsync = new ConcurrentBag<Tuple<IEntity<Guid>, IEntity<Guid>>>();

                bool hasHeader = AppSettingsManager.GetSetting<bool>("InputBaselineHeader", false);

                fileLogger = new LogWriter(fileBuffer.Id.ToString());
                // Построить список DTO
                int sourceRecordCount;
                stopwatch.Stop();
                fileLogger.Write(true, String.Format("GetDTOList ended at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), stopwatch.Elapsed.TotalSeconds), "Message");
                stopwatch.Restart();
                IList<IEntity<Guid>> dtoList = GetDTOList(fileBuffer, out dtoErrorList, out buildErrorList, out sourceRecordCount, hasHeader);
                stopwatch.Stop();
                fileLogger.Write(true, String.Format("GetDTOList ended at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), stopwatch.Elapsed.TotalSeconds), "Message");
                stopwatch.Restart();
                // fileLogger.Write(true, "Количество исходных записей: " + sourceRecordCount + Environment.NewLine, "Message");
                Results["ImportSourceRecordCount"] = sourceRecordCount;
                BeforeModelBuild(dtoList);
                using (DatabaseContext context = new DatabaseContext())
                {
                    stopwatch.Stop();
                    fileLogger.Write(true, String.Format("BeforeModelBuild ended at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), stopwatch.Elapsed.TotalSeconds), "Message");
                    stopwatch.Restart();
                    modelList = GetModelList(context, dtoList, out successList, out warningList, out errorList);
                    fileLogger.Write(true, "Количество построенных записей: " + modelList.Count + Environment.NewLine, "Message");
                    var typedModelList = modelList.Cast<InputBaseline>();
                    foreach (InputBaseline item in typedModelList)
                    {
                        //Избавляемся от нулей
                        item.REP = item.REP.TrimStart('0');
                        item.DMDGroup = item.DMDGroup.TrimStart('0');
                        //Убираем время
                        item.STARTDATE = ChangeTimeZoneUtil.ResetTimeZone(item.STARTDATE.Date);
                    }
                    stopwatch.Stop();
                    fileLogger.Write(true, String.Format("GetModelList ended at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), stopwatch.Elapsed.TotalSeconds), "Message");
                    stopwatch.Restart();
                    var promoesStatuses = new string[3] { "Started", "Planned", "Approved" };
                    var products = context.Set<Product>().Where(x => !x.Disabled).Distinct().Select(x => new { x.Id, x.ZREP }).ToList();
                    var clientTrees = context.Set<ClientTree>().Where(x => !x.EndDate.HasValue).Distinct().ToList();
                    var hierarchy = context.Set<ClientTreeHierarchyView>().ToList();
                    var baseLines = context.Set<BaseLine>().Where(x => !x.Disabled).Distinct().ToList();
                    var prices = context.Set<PriceList>().Where(x => !x.Disabled).Distinct().ToList().Select(x => new { x.StartDate, x.EndDate, x.Product.ZREP, x.ClientTreeId }).ToList();
                    var onInvoicePromoes = context.Set<Promo>().Where(x => !x.Disabled && x.IsOnInvoice && promoesStatuses.Contains(x.PromoStatus.Name)).Select(x => new { x.Number, x.DispatchesStart, x.DispatchesEnd, x.Id, x.ClientTreeId });
                    var offInvoicePromoes = context.Set<Promo>().Where(x => !x.Disabled && !x.IsOnInvoice && promoesStatuses.Contains(x.PromoStatus.Name)).Select(x => new { x.Number, x.StartDate, x.EndDate, x.Id, x.ClientTreeId });
                    var promoProducts = context.Set<PromoProduct>().Where(x => !x.Disabled).Select(x => new { x.ZREP, x.PromoId });
                    List<SimpleProduct> productList = new List<SimpleProduct>();
                    foreach (var item in products)
                    {
                        SimpleProduct sp = new SimpleProduct(item.Id, item.ZREP);
                        productList.Add(sp);
                    }

                    var baseClientTrees = GetDMDGroupForBaseClients(clientTrees);

                    List<SimplePriseList> priceLists = new List<SimplePriseList>();
                    foreach (var item in prices)
                    {
                        SimplePriseList spl = new SimplePriseList(item.StartDate, item.EndDate, item.ZREP, item.ClientTreeId);
                        priceLists.Add(spl);
                    }

                    List<SimplePromoProduct> promoProductLists = new List<SimplePromoProduct>();
                    foreach (var item in promoProducts)
                    {
                        SimplePromoProduct spl = new SimplePromoProduct(item.PromoId, item.ZREP);
                        promoProductLists.Add(spl);
                    }

                    List<SimplePromo> onInvoicePromoLists = new List<SimplePromo>();
                    foreach (var item in onInvoicePromoes)
                    {
                        SimplePromo spl = new SimplePromo(startDate: item.DispatchesStart, endDate: item.DispatchesEnd, number: item.Number, id: item.Id, clienttreeid: item.ClientTreeId);
                        onInvoicePromoLists.Add(spl);
                    }

                    List<SimplePromo> promoLists = new List<SimplePromo>();
                    foreach (var item in offInvoicePromoes)
                    {
                        SimplePromo spl = new SimplePromo(item.StartDate, item.EndDate, item.Number, item.Id, item.ClientTreeId);
                        promoLists.Add(spl);
                    }

                    promoLists.AddRange(onInvoicePromoLists);

                    ////Проверка по каким полям есть дубликаты (для оптимального нахождения - перенесите из лога в эксель)
                    ///расскоменчивать только в крайнем случае
                    //List<string> message2 = new List<string>();
                    //message2.Add("ZREP, Start date, DMD, MOE, QTY, BUS_SEG, DEL_DATE, DEL_Flag, DurInMin, Integration_Stamp, LOC, MKT_SEG, Sales_Dist_Channel, Sales_Division, Sales_Org" + Environment.NewLine);
                    //var groupedBaselineList2 = typedModelList.GroupBy(bl => new { bl.DMDGroup, bl.REP, bl.STARTDATE });
                    //foreach (var singleGroup in groupedBaselineList2)
                    //{
                    //    int i;
                    //    if (singleGroup.Count() > 1)
                    //    {
                    //        i = 0;
                    //        string[] s = new string[15];
                    //        s[i++] = singleGroup.Select(x => x.REP).First();
                    //        s[i++] = singleGroup.Select(x => x.STARTDATE).First().ToString();
                    //        s[i++] = singleGroup.Select(x => x.DMDGroup).First();

                    //        s[i++] = (singleGroup.Select(x => x.MOE).Distinct().Count() <= 1) ? "   " : "MOE";
                    //        s[i++] = (singleGroup.Select(x => x.QTY).Distinct().Count() <= 1) ? "   " : "QTY";
                    //        s[i++] = (singleGroup.Select(x => x.BUS_SEG).Distinct().Count() <= 1) ? "       " : "BUS_SEG";
                    //        s[i++] = (singleGroup.Select(x => x.DELETION_DATE).Distinct().Count() <= 1) ? "   " : "DELETION_DATE";
                    //        s[i++] = (singleGroup.Select(x => x.DELETION_FLAG).Distinct().Count() <= 1) ? "   " : "DELETION_FLAG";
                    //        s[i++] = (singleGroup.Select(x => x.DurInMinutes).Distinct().Count() <= 1) ? "   " : "DurInMinutes";
                    //        s[i++] = (singleGroup.Select(x => x.INTEGRATION_STAMP).Distinct().Count() <= 1) ? "   " : "INTEGRATION_STAMP";
                    //        s[i++] = (singleGroup.Select(x => x.LOC).Distinct().Count() <= 1) ? "   " : "LOC";
                    //        s[i++] = (singleGroup.Select(x => x.MKT_SEG).Distinct().Count() <= 1) ? "       " : "MKT_SEG";
                    //        s[i++] = (singleGroup.Select(x => x.SALES_DIST_CHANNEL).Distinct().Count() <= 1) ? "   " : "SALES_DIST_CHANNEL";
                    //        s[i++] = (singleGroup.Select(x => x.SALES_DIVISON).Distinct().Count() <= 1) ? "   " : "SALES_DIVISON";
                    //        s[i++] = (singleGroup.Select(x => x.SALES_ORG).Distinct().Count() <= 1) ? "   " : "SALES_ORG";
                    //        message2.Add(String.Join(", ", s) + Environment.NewLine);
                    //    }
                    //}
                    //fileLogger.Write(true, String.Join("", message2));
                    //throw new Exception();

                    stopwatch.Stop();
                    fileLogger.Write(true, String.Format("Filtering ended at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), stopwatch.Elapsed.TotalSeconds), "Message");
                    stopwatch.Restart();
                    Parallel.ForEach(typedModelList, new ParallelOptions { MaxDegreeOfParallelism = 4 }, item =>
                    {
                        Stopwatch parallelStopwatch = new Stopwatch();

                        BaseLine modelToCreate, modelToUpdate;
                        Tuple<IEntity<Guid>, IEntity<Guid>> modelToHisCreate, modelToHisUpdate;
                        IList<string> warnings;
                        IList<string> validationErrors;
                        IList<Guid> idsToRecalculate;
                        if (!IsModelSuitable(item, productList, clientTrees, hierarchy, priceLists, promoProductLists, promoLists, dispatcherLogger,out validationErrors, out warnings, baseClientTrees, parallelStopwatch))
                        {
                            //errorRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", dispatcherLogger.GetError())));
                        }
                        else if (!IsConvertToBaselineSuccess(item, baseLines, productList, clientTrees, out idsToRecalculate, out modelToCreate, out modelToUpdate, out modelToHisCreate, out modelToHisUpdate,dispatcherLogger, out validationErrors, parallelStopwatch))
                        {
                            //errorRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", dispatcherLogger.GetError())));
                        }
                        else
                        {
                            if (modelToCreate != null)
                            {
                                modelsToCreate.Add(modelToCreate);
                                toHisCreateAsync.Add(modelToHisCreate);
                            }

                            if (modelToUpdate != null)
                            {
                                modelsToUpdate.Add(modelToUpdate);
                                toHisUpdateAsync.Add(modelToHisUpdate);
                            }

                            if (idsToRecalculate.Any())
                            {
                                foreach (var i in idsToRecalculate)
                                {
                                    needRecalculateBaselineIds.Add(i);
                                }
                            }


                            successRecords.Add(item);
                        }
                    });
                    stopwatch.Stop();
                    fileLogger.Write(true, String.Format("Parallel processing ended at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), stopwatch.Elapsed.TotalSeconds), "Message");

                    fileLogger.Write(true, String.Format("Validation duration: {0} seconds", validateDur), "Message");
                    fileLogger.Write(true, String.Format("Convert duration: {0} seconds", convertDur), "Message");
                    fileLogger.Write(true, String.Format("DMD duration: {0} seconds", dmdDur), "Message");
                    fileLogger.Write(true, String.Format("SalesORG duration: {0} seconds", SalesORGDur), "Message");
                    fileLogger.Write(true, String.Format("Product duration: {0} seconds", ProductDur), "Message");
                    fileLogger.Write(true, String.Format("SalesDist duration: {0} seconds", SalesDISTDur), "Message");
                    fileLogger.Write(true, String.Format("Client duration: {0} seconds", clientDur), "Message");
                    fileLogger.Write(true, String.Format("Qty duration: {0} seconds", qtyDur), "Message");
                    fileLogger.Write(true, String.Format("Sunday duration: {0} seconds", sundayDur), "Message");
                    fileLogger.Write(true, String.Format("Price List duration: {0} seconds", priceDur), "Message");
                    fileLogger.Write(true, String.Format("Zero QTY duration: {0} seconds", zeroQtyDur), "Message");
                    fileLogger.Write(true, String.Format("Need recalculation duration: {0} seconds", needRecDur), "Message");
                    fileLogger.Write(true, String.Format("Create Baseline duration: {0} seconds", createBLDur), "Message");
                    stopwatch.Restart();
                    createBaselineList = modelsToCreate.ToList();
                    updateBaselineList = modelsToUpdate.ToList();
                    successRecordsList = successRecords.Distinct().ToList();

                    toHisCreate = toHisCreateAsync.Distinct().ToList();
                    toHisUpdate = toHisUpdateAsync.Distinct().ToList();

                    List<BaseLine> uniqueBaseLinesShow = modelsToCreate.ToList();
                    uniqueBaseLinesShow.AddRange(modelsToUpdate.ToList());
                    //Убираем дубликаты
                    var groupedBaselineList = uniqueBaseLinesShow.Select(y => (BaseLine)y).GroupBy(bl => new { bl.DemandCode, bl.ProductId, bl.StartDate });
                    //Если в файле будут присутствовать две записи с одинаковыми значениями STARTDATE - ZREP - DMD Group, то импорта не будет – пишем запись в лог ошибок.
                    foreach (var singleGroup in groupedBaselineList)
                    {
                        if (singleGroup.Count() > 1)
                        {
                            if (singleGroup.Select(x => x.InputBaselineQTY).Distinct().Count() > 1)
                            {
                                var dublicateIds = singleGroup.Select(x => x.Id);
                                dispatcherLogger.Add<ErrorLogInfo>(singleGroup.First(),"There is dublicate", "StartDate,ZREP,DemandCode", String.Join(",", singleGroup.First().StartDate, singleGroup.First().Product.ZREP, singleGroup.First().DemandCode));
                                //errorRecords.Add(new Tuple<IEntity<Guid>, string>(singleGroup.First(), String.Join(",",dispatcherLogger.GetError()));
                                createBaselineList = createBaselineList.Where(c => !c.DemandCode.Equals(singleGroup.Key.DemandCode) || !c.ProductId.Equals(singleGroup.Key.ProductId) || !c.StartDate.Equals(singleGroup.Key.StartDate)).ToList();
                                updateBaselineList = updateBaselineList.Where(c => !c.DemandCode.Equals(singleGroup.Key.DemandCode) || !c.ProductId.Equals(singleGroup.Key.ProductId) || !c.StartDate.Equals(singleGroup.Key.StartDate)).ToList();
                                toHisCreate = toHisCreate.Where(x => !dublicateIds.Contains(x.Item2.Id)).ToList();
                                toHisUpdate = toHisUpdate.Where(x => !dublicateIds.Contains(x.Item2.Id)).ToList();
                            }
                            else
                            {
                                var i = 0;
                                foreach (var item in singleGroup)
                                {
                                    if (i++ != 0)
                                    {
                                        createBaselineList.Remove(item);
                                        updateBaselineList.Remove(item);
                                        toHisCreate = toHisCreate.Where(x => x.Item2.Id != item.Id).ToList();
                                    };
                                }
                            }
                        }
                    }

                    toHisUpdate = toHisUpdate.Distinct().ToList();
                    //чтобы не обновлять одни и те же записи, создавая, тем самым, дубликаты инцидентов
                    baselineIdsToRecalculateList = needRecalculateBaselineIds.Distinct().ToList();
                    baselineIdsToRecalculateList = baselineIdsToRecalculateList.Except(createBaselineList.Select(x => x.Id))
                                                                               .Except(updateBaselineList.Select(x => x.Id))
                                                                               .ToList();

                    var nowTime = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                    try
                    {
                        stopwatch.Stop();
                        fileLogger.Write(true, String.Format("Dublicate deletion ended at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), stopwatch.Elapsed.TotalSeconds), "Message");
                        stopwatch.Restart();
                        // чтобы не создавать лишних инцидентов, отключаем триггер
                        string disableBaselineTriggerScript = "DISABLE TRIGGER [BaseLine_ChangesIncident_Insert_Update_Trigger] ON [DefaultSchemaSetting].[BaseLine];";
                        context.ExecuteSqlCommand(disableBaselineTriggerScript);

                        // отключем индекс
                        string disableBaselineIndexScript = "ALTER INDEX IX_BaseLine_NonClustered ON [DefaultSchemaSetting].[BaseLine] DISABLE;";
                        context.ExecuteSqlCommand(disableBaselineIndexScript);

                        stopwatch.Stop();
                        fileLogger.Write(true, String.Format("Disable trigger ended at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), stopwatch.Elapsed.TotalSeconds), "Message");
                        stopwatch.Restart();
                        foreach (IEnumerable<BaseLine> items in createBaselineList.Partition(1000))
                        {
                            string insertScript = String.Join("", items.Select(y => String.Format("INSERT INTO [DefaultSchemaSetting].BaseLine ([InputBaselineQTY], [LastModifiedDate], [ProductId], [StartDate], [DemandCode], [Type], [Disabled], [NeedProcessing], [Id]) VALUES ({0}, '{1:yyyy-MM-dd HH:mm:ss +03:00}', '{2}', '{3:yyyy-MM-dd HH:mm:ss +03:00}', '{4}', 1, '{5}', 1, '{6}');",
                                y.InputBaselineQTY, nowTime, y.ProductId.ToString(), y.StartDate, y.DemandCode, false, y.Id)));
                            context.ExecuteSqlCommand(insertScript);
                        }
                        stopwatch.Stop();
                        fileLogger.Write(true, String.Format("Create baselines ended at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), stopwatch.Elapsed.TotalSeconds), "Message");
                        stopwatch.Restart();

                        foreach (IEnumerable<BaseLine> items in updateBaselineList.Partition(10000))
                        {
                            string updateScript = String.Join("", items.Select(y => String.Format("UPDATE [DefaultSchemaSetting].BaseLine SET [NeedProcessing] = 1, InputBaselineQTY = {0}, LastModifiedDate = '{1:yyyy-MM-dd HH:mm:ss +03:00}'  WHERE Id = '{2}';",
                                y.InputBaselineQTY, nowTime, y.Id)));
                            context.ExecuteSqlCommand(updateScript);
                        }
                        stopwatch.Stop();
                        fileLogger.Write(true, String.Format("Update baselines ended at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), stopwatch.Elapsed.TotalSeconds), "Message");
                        stopwatch.Restart();

                        string rebuildBaselineIndexScript = "ALTER INDEX IX_BaseLine_NonClustered ON [DefaultSchemaSetting].[BaseLine] REBUILD;";
                        context.ExecuteSqlCommand(rebuildBaselineIndexScript);
                        baseLines = context.Set<BaseLine>().Where(x => !x.Disabled).ToList();
                        var newBaselines = createBaselineList.Union(updateBaselineList);
                        //long i = 0;
                        foreach (var baseline in newBaselines)
                        {
                            //if (i++ % 1000 == 0)
                            //{
                            //    stopwatch.Stop();
                            //    fileLogger.Write(true, String.Format("Build baseline list at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds; Iteration: {2}", DateTimeOffset.Now, stopwatch.Elapsed.TotalSeconds, i), "Message");
                            //    stopwatch.Restart();
                            //}
                            var baselines = baseLines.Where(x => x.DemandCode == baseline.DemandCode && x.ProductId == baseline.ProductId)
                                                                .ToList();
                            // первые и последние 5 записей надо пересчитать(здесь возможна несущественная избыточность)
                            var orderedBaseline = baselines.OrderBy(x => x.StartDate).ToList();
                            if (orderedBaseline.Count() > 5)
                            {
                                var firstItems = orderedBaseline.GetRange(0, 5);
                                var lastItems = orderedBaseline.GetRange(orderedBaseline.Count() - 5, 5);
                                baselineIdsToRecalculateList.AddRange(firstItems.Select(x => x.Id));
                                baselineIdsToRecalculateList.AddRange(lastItems.Select(x => x.Id));
                            }
                            else
                            {
                                baselineIdsToRecalculateList.AddRange(orderedBaseline.Select(x => x.Id));
                            }
                        }
                        // отключем индекс
                        disableBaselineIndexScript = "ALTER INDEX IX_BaseLine_NonClustered ON [DefaultSchemaSetting].[BaseLine] DISABLE;";
                        context.ExecuteSqlCommand(disableBaselineIndexScript);
                        baselineIdsToRecalculateList = baselineIdsToRecalculateList.Distinct().ToList();


                        foreach (IEnumerable<Guid> ids in baselineIdsToRecalculateList.Partition(10000))
                        {
                            string updateFlagScript = String.Join("", ids.Select(y => String.Format("UPDATE [DefaultSchemaSetting].BaseLine SET [NeedProcessing] = 1  WHERE Id = '{0}';",
                                y.ToString())));
                            context.ExecuteSqlCommand(updateFlagScript);
                        }
                        stopwatch.Stop();
                        fileLogger.Write(true, String.Format("Update baselines recalculation ended at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), stopwatch.Elapsed.TotalSeconds), "Message");
                        stopwatch.Restart();

                        stopwatch.Stop();
                        fileLogger.Write(true, String.Format("Created history started at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), stopwatch.Elapsed.TotalSeconds), "Message");
                        stopwatch.Restart();
                        context.HistoryWriter.Write(toHisCreate, context.AuthManager.GetCurrentUser(), context.AuthManager.GetCurrentRole(), OperationType.Created);
                        stopwatch.Stop();
                        fileLogger.Write(true, String.Format("Created history ended at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), stopwatch.Elapsed.TotalSeconds), "Message");
                        stopwatch.Restart();
                        context.HistoryWriter.Write(toHisUpdate, context.AuthManager.GetCurrentUser(), context.AuthManager.GetCurrentRole(), OperationType.Updated);
                        stopwatch.Stop();
                        fileLogger.Write(true, String.Format("Updated history ended at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), stopwatch.Elapsed.TotalSeconds), "Message");
                        stopwatch.Restart();
                    }
                    catch (Exception e)
                    {
                        logger.Error(e, "Error during Processing files interface '{0}'", InterfaceId);
                        Errors.Add(String.Format("Error during Processing files interface '{0}': {1}", InterfaceId, e.ToString()));
                        Fail();
                    }
                    finally
                    {
                        // включаем триггер
                        string enableBaselineTriggerScript = "ENABLE TRIGGER [BaseLine_ChangesIncident_Insert_Update_Trigger] ON [DefaultSchemaSetting].[BaseLine];";
                        context.ExecuteSqlCommand(enableBaselineTriggerScript);

                        // включаем индекс
                        string rebuildBaselineIndexScript = "ALTER INDEX IX_BaseLine_NonClustered ON [DefaultSchemaSetting].[BaseLine] REBUILD;";
                        context.ExecuteSqlCommand(rebuildBaselineIndexScript);
                    }

                    stopwatch.Stop();
                    fileLogger.Write(true, String.Format("Enable triggers ended at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), stopwatch.Elapsed.TotalSeconds), "Message");
                    stopwatch.Restart();

                    int processedCount = createBaselineList.Count + updateBaselineList.Count;
                    fileLogger.Write(true, "Количество обработанных записей: " + processedCount + Environment.NewLine, "Message");
                    warningRecords = dispatcherLogger.GetRecords<WarningLogInfo>();
                    errorRecords = dispatcherLogger.GetRecords<ErrorLogInfo>();                 
                    Results["ImportResultRecordCount"] = processedCount;
                    Results["ErrorCount"] = errorRecords.Count;
                    Results["WarningCount"] = warningRecords.Count;
                    hasWarnings = false;
                    hasErrors = false;

                    foreach (var item in dispatcherLogger.GetLog<WarningLogInfo>())
                    {
                        hasWarnings = true;
                        Warnings.Add(item);
                    }
                    foreach (var item in dispatcherLogger.GetLog<ErrorLogInfo>())
                    {
                        hasErrors = true;
                        Errors.Add(item);
                    } 
                    stopwatch.Stop();
                    fileLogger.Write(true, String.Format("Save arguments ended at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), stopwatch.Elapsed.TotalSeconds), "Message");
                    stopwatch.Restart();
                }

                HasChanges = true;

                bool hasSuccessList = AllowPartialApply || !hasErrors;
                ImportResultFilesModel resultFilesModel = SaveProcessResultHelper.SaveResultToFile(
                    fileBuffer.Id,
                    hasSuccessList ? successRecordsList : null,
                    buildErrorList,
                    errorRecords,
                    warningRecords);
                Results["ImportResultFilesModel"] = resultFilesModel;

                WriteErrorsToFileLog();

                string fileStatus = Interfaces.Core.Model.Consts.ProcessResult.Complete;
                if (hasErrors)
                {
                    fileStatus = Interfaces.Core.Model.Consts.ProcessResult.Error;
                }
                else if (hasWarnings)
                {
                    fileStatus = Interfaces.Core.Model.Consts.ProcessResult.Warning;
                }
                SaveFileBufferStatus(fileBuffer, fileStatus, resultFilesModel);
            }
            catch (ApplicationException e)
            {
                string msg = String.Format("Error during Processing files interface '{0}': {1}", InterfaceId, e.Message);
                Errors.Add(msg);
                WriteErrorsToFileLog();
                logger.Error(e, "Error during Processing files interface '{0}'", InterfaceId);
                SaveFileBufferStatus(fileBuffer, Interfaces.Core.Model.Consts.ProcessResult.Error, null);
            }
            catch (Exception e)
            {
                string msg = String.Format("Error during Processing files interface '{0}': {1}", InterfaceId, e.ToString());
                Errors.Add(msg);
                WriteErrorsToFileLog();
                logger.Error(e, "Error during Processing files interface '{0}'", InterfaceId);
                SaveFileBufferStatus(fileBuffer, Interfaces.Core.Model.Consts.ProcessResult.Error, null);
            }
            finally
            {
                logger.Trace("Finish processing FileBuffer '{0}'", fileBuffer.Id);
                fileLogger.UploadToBlob();
                fileLogger = null;
            }
        }

        private void WriteErrorsToFileLog()
        {
            if (fileLogger != null)
            {
                if (Errors.Any())
                {
                    //Работает в тысячи раз быстрее чем обычный цикл
                    string s = "[ERROR]: " + String.Join(Environment.NewLine + "[ERROR]: ", Errors.ToArray()) + Environment.NewLine;
                    fileLogger.Write(false, s);
                }
                if (Warnings.Any())
                {
                    //Работает в тысячи раз быстрее чем обычный цикл
                    string s = "[Warning]: " + String.Join(Environment.NewLine + "[Warning]: ", Warnings.ToArray()) + Environment.NewLine;
                    fileLogger.Write(false, s);
                }
            }
        }

        private void SaveFileBufferStatus(FileBuffer fileBuffer, string status, ImportResultFilesModel resultFilesModel)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.FileBuffers.Attach(fileBuffer);
                fileBuffer.Status = status;
                fileBuffer.ProcessDate = DateTimeOffset.Now;
                context.SaveChanges();
                if (fileLogger != null)
                {
                    fileLogger.Write(true, "Результат обработки файла: " + status + Environment.NewLine, "Message");
                }
            }
        }

        protected virtual bool IsModelSuitable(InputBaseline rec, List<SimpleProduct> products, List<ClientTree> clientTrees, IEnumerable<ClientTreeHierarchyView> hierarchy, List<SimplePriseList> priceLists, List<SimplePromoProduct> promoProducts, List<SimplePromo> promoes, BaseDispatcherLogger dispatcherLogger ,out IList<string> errors, out IList<string> warnings, List<SimpleClientTreeDMDGroup> baseClientTrees, Stopwatch parallelStopwatch)
        {
            Stopwatch validateStopwatch = new Stopwatch();
            validateStopwatch.Restart();
            errors = new List<string>();
            warnings = new List<string>();
            bool isError = false;
            string message;

            string REP = rec.REP;
            string DMDGroup = rec.DMDGroup;

            parallelStopwatch.Restart();
            if (!Regex.IsMatch(rec.DMDGroup, @"^0*[\d]{8}") || rec.DurInMinutes != 10080 || !Regex.IsMatch(rec.REP, @"^0*[\d]{6}"))
            {
                isError = true;
                dispatcherLogger.Add<ErrorLogInfo>(rec,"DMDGroup should be 8 digits long, DurInMinutes should be equal to 10080, REP should be 6 digits long.","","");
            }
            parallelStopwatch.Stop();
            dmdDur += parallelStopwatch.Elapsed.TotalSeconds;
            parallelStopwatch.Restart();
            if (rec.SALES_ORG != 261 || rec.SALES_DIVISON != 51 || rec.BUS_SEG != 5 || rec.MOE != 125)
            {
                isError = true;
                dispatcherLogger.Add<ErrorLogInfo>(rec, "DMDGrThere are baselines with wrong SALES_ORG, SALES_DIVISON, BUS_SEG or MOE.", "", "");
            }
            parallelStopwatch.Stop();
            SalesORGDur += parallelStopwatch.Elapsed.TotalSeconds;
            parallelStopwatch.Restart();

            if (!products.Select(x => x.ZREP).Contains(rec.REP))
            {
                isError = true;
                dispatcherLogger.Add<ErrorLogInfo>(rec, "Product was not found", "ZREP", rec.REP);
            }
            parallelStopwatch.Stop();
            ProductDur += parallelStopwatch.Elapsed.TotalSeconds;
            parallelStopwatch.Restart();
            if (rec.SALES_DIST_CHANNEL != 11 && rec.SALES_DIST_CHANNEL != 22)
            {
                isError = true;
                dispatcherLogger.Add<ErrorLogInfo>(rec, "Sales dist channel not equals 11 or 22.", "ZREP,DMDGroup", string.Join(",", rec.REP, rec.DMDGroup));
            }
            parallelStopwatch.Stop();
            SalesDISTDur += parallelStopwatch.Elapsed.TotalSeconds;
            parallelStopwatch.Restart();
            if (!clientTrees.Select(x => x.DMDGroup).Contains(rec.DMDGroup))
            {
                isError = true;
                dispatcherLogger.Add<ErrorLogInfo>(rec, "Client was not found", "DmdGroup", rec.DMDGroup);
            }
            parallelStopwatch.Stop();
            clientDur += parallelStopwatch.Elapsed.TotalSeconds;
            parallelStopwatch.Restart(); 
          
            if (rec.STARTDATE.DayOfWeek != DayOfWeek.Sunday)
            {
                isError = true;
                dispatcherLogger.Add<ErrorLogInfo>(rec, "Start date must be on Sunday", "ZREP, Dmd Group, Start Date", String.Join(",",rec.REP,rec.DMDGroup,rec.STARTDATE));
            }
            parallelStopwatch.Stop();
            sundayDur += parallelStopwatch.Elapsed.TotalSeconds;
            parallelStopwatch.Restart();

            var ClientWithDMD = baseClientTrees.Where(x => x.DMDGroup == rec.DMDGroup);
            foreach (var client in ClientWithDMD)
            {
                if (!priceLists.Any(x => x.StartDate <= rec.STARTDATE && x.EndDate >= rec.STARTDATE && x.ZREP == rec.REP && client.Id == x.ClientTreeId))
                {
                    dispatcherLogger.Add<WarningLogInfo>(rec, "Price List for Client Not Found","ZREP ,Start Date , DMD Group",String.Join(",",rec.REP,rec.STARTDATE,rec.DMDGroup));
                }
            }
            parallelStopwatch.Stop();
            priceDur += parallelStopwatch.Elapsed.TotalSeconds;
            parallelStopwatch.Restart();

            if (rec.QTY < 0)
            {
                rec.QTY = 0;
                dispatcherLogger.Add<WarningLogInfo>(rec, "QTY cannot be less than 0, QTY will be equal to 0. ","ZREP,DMDGroup",String.Join(",",rec.REP,rec.DMDGroup));
            }
            parallelStopwatch.Stop();
            qtyDur += parallelStopwatch.Elapsed.TotalSeconds;
            parallelStopwatch.Restart();

            //Проверяем, существуют ли промо в статусах Planned, Approved, Started, ZREP по которым в файле на дату начала отгрузки qty = 0.
            if (rec.QTY == 0 && rec.STARTDATE.Year != 9999)
            {
                var client = clientTrees.Where(x => x.DMDGroup != null && x.DMDGroup.EndsWith(rec.DMDGroup)).FirstOrDefault();
                if (client != null)
                {
                    var clients = Frontend.TPM.Util.Helper.getClientTreeAllChildrenWithoutContext(client.ObjectId, clientTrees).Select(x => x.ObjectId).ToList();
                    clients.Add(client.ObjectId);
                    var recPromoProducts = promoProducts.Where(x => x.ZREP == rec.REP).Select(x => x.PromoId).ToList();

                    var startDatePlus6 = rec.STARTDATE.AddDays(6);
                    var affectedPromoes = promoes.Where(promo => !(promo.StartDate > startDatePlus6 || promo.EndDate < rec.STARTDATE)
                        && clients.Contains((int)promo.ClientTreeId) && recPromoProducts.Contains(promo.Id)).Select(x => x.Number).ToList();

                    if (affectedPromoes.Any())
                    {
                        dispatcherLogger.Add<WarningLogInfo>(rec, "QTY = 0 downloaded for promo in Started, Planned or Approved status: ", "Id", String.Join(", ", affectedPromoes.Distinct()));
                    }
                }
            }
            parallelStopwatch.Stop();
            zeroQtyDur += parallelStopwatch.Elapsed.TotalSeconds;

            validateStopwatch.Stop();
            validateDur += validateStopwatch.Elapsed.TotalSeconds;
            return !isError;
        }

        private bool IsConvertToBaselineSuccess(InputBaseline rec, List<BaseLine> baseLines, List<SimpleProduct> products, List<ClientTree> clientTrees, out IList<Guid> needRecalculateBaselineIds, out BaseLine modelToCreate, out BaseLine modelToUpdate, out Tuple<IEntity<Guid>, IEntity<Guid>> modelToHisCreate, out Tuple<IEntity<Guid>, IEntity<Guid>> modelToHisUpdate,BaseDispatcherLogger dispatcherLogger, out IList<string> errors, Stopwatch parallelStopwatch)
        {
            Stopwatch convertStopwatch = new Stopwatch();
            convertStopwatch.Restart();

            needRecalculateBaselineIds = new List<Guid>();
            modelToCreate = null;
            modelToUpdate = null;
            modelToHisCreate = null;
            modelToHisUpdate = null;
            errors = new List<string>();
            BaseLine baseLine;

            bool isError = false;
            string message;

            try
            {
                parallelStopwatch.Restart();
                var recordStartDate = ChangeTimeZoneUtil.ResetTimeZone(rec.STARTDATE);
                string demandCode = clientTrees.Where(x => x.DMDGroup == rec.DMDGroup).Select(x => x.DemandCode).FirstOrDefault();
                Guid productId = products.Where(x => x.ZREP == rec.REP).Select(x => x.ProductId).FirstOrDefault();
                baseLine = baseLines.Where(x => x.StartDate == recordStartDate && x.ProductId == productId && x.DemandCode == demandCode)
                                    .FirstOrDefault();

                if (recordStartDate.Year != 9999)
                {
                    var plus2weeks = recordStartDate.AddDays(14);
                    var minus2weeks = recordStartDate.AddDays(-14);
                    if (baseLines.Any())
                    {
                        if (baseLine != null)
                        {
                            needRecalculateBaselineIds = baseLines.Where(x => x.StartDate <= plus2weeks && x.StartDate >= minus2weeks && x.Id != baseLine.Id
                                                        && productId == x.ProductId && x.DemandCode == demandCode).Select(x => x.Id).ToList();

                        }
                        else
                        {
                            needRecalculateBaselineIds = baseLines.Where(x => x.StartDate <= plus2weeks && x.StartDate >= minus2weeks
                                                        && productId == x.ProductId && x.DemandCode == demandCode).Select(x => x.Id).ToList();
                        }
                    }
                }
                else
                {
                    dispatcherLogger.Add<ErrorLogInfo>(rec, "Wrong start date in record", "ZREP,DMD Group, Start Date", string.Join(",", rec.REP, rec.DMDGroup, rec.STARTDATE)); ;
                    convertStopwatch.Stop();
                    convertDur = convertStopwatch.Elapsed.TotalSeconds;
                    return false;
                }
                parallelStopwatch.Stop();
                needRecDur += parallelStopwatch.Elapsed.TotalSeconds;
                parallelStopwatch.Restart();

                if (baseLine != null)
                {
                    BaseLine oldBaseline = (BaseLine)baseLine.CloneWithoutDB();
                    modelToUpdate = new BaseLine()
                    {
                        Id = baseLine.Id,
                        DemandCode = baseLine.DemandCode,
                        StartDate = baseLine.StartDate,
                        ProductId = baseLine.ProductId,
                        Product = new Product()
                        {
                            ZREP = products.Where(p => p.ProductId == baseLine.ProductId).FirstOrDefault().ZREP,

                        }

                    };
                    modelToUpdate.InputBaselineQTY = rec.QTY;
                    modelToUpdate.NeedProcessing = true;
                    modelToUpdate.Type = 1;

                    modelToHisUpdate = new Tuple<IEntity<Guid>, IEntity<Guid>>(modelToUpdate, oldBaseline);
                }
                else
                {
                    modelToCreate = new BaseLine()
                    {
                        Id = Guid.NewGuid(),
                        InputBaselineQTY = rec.QTY,
                        DemandCode = demandCode,
                        StartDate = recordStartDate,
                        NeedProcessing = true,
                        ProductId = productId,
                        Product = new Product()
                        {
                            ZREP = rec.REP
                        },
                        Type = 1
                    };

                    modelToHisCreate = new Tuple<IEntity<Guid>, IEntity<Guid>>(null, modelToCreate);
                }
                parallelStopwatch.Stop();
                createBLDur += parallelStopwatch.Elapsed.TotalSeconds;
                parallelStopwatch.Restart();
            }
            catch (Exception e)
            {
                isError = true;
                message = e.ToString();
                dispatcherLogger.Add<ErrorLogInfo>(rec, message,"",""); 

                errors.Add(message);
            }

            convertStopwatch.Stop();
            convertDur = convertStopwatch.Elapsed.TotalSeconds;
            return !isError;
        }

        private List<SimpleClientTreeDMDGroup> GetDMDGroupForBaseClients(List<ClientTree> clientTrees)
        {
            List<SimpleClientTreeDMDGroup> result = new List<SimpleClientTreeDMDGroup>();
            List<ClientTree> children;
            foreach (var client in clientTrees.Where(x => !x.DMDGroup.IsNullOrWhiteSpace() && x.EndDate == null))
            {
                children = Helper.getClientTreeAllChildrenWithoutContext(client.ObjectId, clientTrees, true);
                if (client.IsBaseClient == true)
                {
                    result.Add(new SimpleClientTreeDMDGroup(client.Id, client.DMDGroup));
                }
                foreach (var res in children)
                {
                    if (res.IsBaseClient == true)
                    {
                        result.Add(new SimpleClientTreeDMDGroup(res.Id, client.DMDGroup));
                    }
                }
            }
            return result;
        }
    }

    public class SimplePriseList
    {
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string ZREP { get; set; }
        public int ClientTreeId { get; set; }

        public SimplePriseList(DateTimeOffset startDate, DateTimeOffset endDate, string zrep, int clientTreeId)
        {
            StartDate = startDate;
            EndDate = endDate;
            ZREP = zrep;
            ClientTreeId = clientTreeId;
        }
    }

    public class SimpleProduct
    {
        public Guid ProductId { get; set; }
        public string ZREP { get; set; }

        public SimpleProduct(Guid productid, string zrep)
        {
            ProductId = productid;
            ZREP = zrep;
        }
    }

    public class SimplePromoProduct
    {
        public Guid PromoId { get; set; }
        public string ZREP { get; set; }

        public SimplePromoProduct(Guid promoId, string zrep)
        {
            PromoId = promoId;
            ZREP = zrep;
        }
    }

    public class SimplePromo
    {
        public Guid Id { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public int? Number { get; set; }
        public int? ClientTreeId { get; set; }

        public SimplePromo(DateTimeOffset? startDate, DateTimeOffset? endDate, int? number, Guid id, int? clienttreeid)
        {
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            Number = number;
            ClientTreeId = clienttreeid;
        }
    }

    public class SimpleClientTreeDMDGroup
    {
        public int Id { get; set; }
        public string DMDGroup { get; set; }
        public SimpleClientTreeDMDGroup(int id, string dmdgroup)
        {
            Id = id;
            DMDGroup = dmdgroup;
        }
    }
}