using Interfaces.Implementation.Import.FullImport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Data;
using Persist;
using Persist.ScriptGenerator;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using Core.Extensions;
using Looper.Parameters;
using Interfaces.Implementation.Action;
using Utility.Import;
using System.Collections.Concurrent;
using Core.History;
using Module.Persist.TPM.Utils;
using Persist.ScriptGenerator.Filter;
using Core.Settings;
using Utility.LogWriter;
using System.Diagnostics;
using NLog;

namespace Module.Host.TPM.Actions
{
    class FullXLSXImportBaseLineAction : FullXLSXImportAction
    {

        private readonly bool NeedClearData;
        private readonly DateTimeOffset StartDate;
        private readonly DateTimeOffset FinishDate;
        private readonly IDictionary<string, IEnumerable<string>> Filters;
        ILogWriter handlerLogger = null;
        Stopwatch stopwatch = new Stopwatch();
        protected readonly static Logger traceLogger = LogManager.GetCurrentClassLogger();

        public FullXLSXImportBaseLineAction(FullImportSettings settings, DateTimeOffset startDate, DateTimeOffset finishDate, IDictionary<string, IEnumerable<string>> filters, bool needClearData, Guid handlerId) : base(settings)
        {
            NeedClearData = needClearData;
            StartDate = startDate;
            FinishDate = finishDate;
            Filters = filters;
            UniqueErrorMessage = "This entry already exists in the database";
            ErrorMessageScaffold = "FullImportAction failed: {0}";
            FileParseErrorMessage = "An error occurred while parsing the import file";

            //добавляем новый тип тега
            handlerLogger = new FileLogWriter(handlerId.ToString(),
                                              new Dictionary<string, string>() { ["Timing"] = "TIMING" });
        }

        protected override ImportResultFilesModel ApplyImport(IList<IEntity<Guid>> sourceRecords, out int successCount, out int warningCount, out int errorCount)
        {
            // Логика переноса данных из временной таблицы в постоянную
            // Получить записи текущего импорта
            //handlerLogger.Write(true, String.Format("ApplyImport started at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Timing");
            stopwatch.Restart();
            traceLogger.Trace("Start get context");
            using (DatabaseContext context = new DatabaseContext())
            {
                traceLogger.Trace("Apply Start");
                var records = new ConcurrentBag<IEntity<Guid>>();
                var successList = new ConcurrentBag<IEntity<Guid>>();
                var errorRecords = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();
                var warningRecords = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();

                // Получить функцию Validate
                var validator = ImportModelFactory.GetImportValidator(ImportType);
                // Получить функцию SetProperty
                var builder = ImportModelFactory.GetModelBuilder(ImportType, ImportType);
                traceLogger.Trace("Start source records filtering");
                //stopwatch.Stop();
                //handlerLogger.Write(true, String.Format("Start source records filtering records list stopped at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", DateTimeOffset.Now, stopwatch.Elapsed.TotalSeconds), "Timing");
                //stopwatch.Restart();

                //Отфильтровать все записи с типом не 1
                sourceRecords = sourceRecords.Where(y => ((ImportBaseLine)y).Type == 1).ToList();
                traceLogger.Trace("Start source records grouping");
                //stopwatch.Stop();
                //handlerLogger.Write(true, String.Format("Start source records grouping records list stopped at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", DateTimeOffset.Now, stopwatch.Elapsed.TotalSeconds), "Timing");
                //stopwatch.Restart();

                var groupedRecds = sourceRecords.Select(y => (ImportBaseLine)y).GroupBy(bl => new { bl.ProductZREP, bl.ClientTreeDemandCode, bl.StartDate });
                foreach (var singleGroup in groupedRecds)
                {
                    if (singleGroup.Count() > 1)
                    {
                        HasErrors = true;
                        errorRecords.Add(new Tuple<IEntity<Guid>, string>(singleGroup.First(), "There is dublicate"));
                    }
                }
                traceLogger.Trace("Finish get grouped Source records list");
                //stopwatch.Stop();
                //handlerLogger.Write(true, String.Format("Finish get grouped Source records list stopped at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", DateTimeOffset.Now, stopwatch.Elapsed.TotalSeconds), "Timing");
                //stopwatch.Restart();

                var cacheBuilder = ImportModelFactory.GetImportCacheBuilder(ImportType);
                var cache = cacheBuilder.Build(sourceRecords, context);
                traceLogger.Trace("finish cache build");
                //stopwatch.Stop();
                //handlerLogger.Write(true, String.Format("finish cache build stopped at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", DateTimeOffset.Now, stopwatch.Elapsed.TotalSeconds), "Timing");
                //stopwatch.Restart();


                IList<ClientTree> clientTreeList = context.Set<ClientTree>().AsNoTracking().Where(x => x.DemandCode != null && !x.EndDate.HasValue).ToList();
                IList<Tuple<int, string>> clientTreeTupleList = clientTreeList.Select(x => new Tuple<int, string>(x.Id, x.DemandCode)).ToList();
                IList<string> clientTreeDemandCodeList = clientTreeTupleList.Select(x => x.Item2).ToList();
                traceLogger.Trace("finish get client tree");
                //stopwatch.Stop();
                //handlerLogger.Write(true, String.Format("finish get client tree stopped at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", DateTimeOffset.Now, stopwatch.Elapsed.TotalSeconds), "Timing");
                //stopwatch.Restart();


                traceLogger.Trace("start zrep cut");
                //Обрезка ZREP
                List<string> zrepList = new List<string>();
                foreach (var item in sourceRecords)
                {
                    String zrep = ((ImportBaseLine)item).ProductZREP;
                    var splited = zrep.Split('_');
                    if (splited.Length > 1)
                    {
                        zrep = String.Join("_", splited.Take(splited.Length - 1));
                    }
                    ((ImportBaseLine)item).ProductZREP = zrep;
                    zrepList.Add(zrep);
                }
                traceLogger.Trace("finish zrep cut");
                //stopwatch.Stop();
                //handlerLogger.Write(true, String.Format("finish zrep cut stopped at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", DateTimeOffset.Now, stopwatch.Elapsed.TotalSeconds), "Timing");
                //stopwatch.Restart();


                List<string> badZrepList = zrepList.Where(z => !context.Set<Product>().Select(p => p.ZREP).Contains(z)).ToList();
                traceLogger.Trace("finish get badZrepList");
                //stopwatch.Stop();
                //handlerLogger.Write(true, String.Format("finish get badZrepList stopped at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", DateTimeOffset.Now, stopwatch.Elapsed.TotalSeconds), "Timing");
                //stopwatch.Restart();


                //Проверка по наличию клиента
                List<String> badDemandCodes = sourceRecords
                    .Where(y => !clientTreeDemandCodeList.Contains(((ImportBaseLine)y).ClientTreeDemandCode))
                    .Select(z => ((ImportBaseLine)z).ClientTreeDemandCode).ToList();
                traceLogger.Trace("finish get badDemandCodes");
                //stopwatch.Stop();
                //handlerLogger.Write(true, String.Format("finish get blUniqueIdent stopped at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", DateTimeOffset.Now, stopwatch.Elapsed.TotalSeconds), "Timing");
                //stopwatch.Restart();
                //Проверка уникальности
                List<Tuple<string, string, DateTimeOffset?>> blUniqueIdent = sourceRecords
                    .Select(z => (ImportBaseLine)z)
                    .Select(u => new Tuple<string, string, DateTimeOffset?>(u.ProductZREP, u.ClientTreeDemandCode, u.StartDate)).ToList();
                traceLogger.Trace("finish get blUniqueIdent");
                //stopwatch.Stop();
                //handlerLogger.Write(true, String.Format("finish get blUniqueIdent stopped at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", DateTimeOffset.Now, stopwatch.Elapsed.TotalSeconds), "Timing");
                //stopwatch.Restart();

                //Базы
                List<BaseLine> baseLines = context.Set<BaseLine>().Where(x => !x.Disabled).ToList();
                List<Tuple<string, string, DateTimeOffset?>> blBadBaseUniqueIdent = blUniqueIdent
                    .Where(y => baseLines
                    .Any(z => z.Product.ZREP == y.Item1 && z.ClientTree.DemandCode == y.Item2 && z.StartDate == y.Item3))
                    .ToList();
                traceLogger.Trace("finish get blBadBaseUniqueIdent");
                //stopwatch.Stop();
                //handlerLogger.Write(true, String.Format("finish get blBadBaseUniqueIdent records list stopped at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", DateTimeOffset.Now, stopwatch.Elapsed.TotalSeconds), "Timing");
                //stopwatch.Restart();

                traceLogger.Trace("first parallel start");
                Parallel.ForEach(sourceRecords, item => {
                    string demandCode = ((ImportBaseLine)item).ClientTreeDemandCode;
                    if (clientTreeDemandCodeList.Contains(demandCode))
                    {
                        Tuple<int, string> clientTreeTuple = clientTreeTupleList.FirstOrDefault(x => x.Item2 == demandCode);
                        if (clientTreeTuple != null)
                        {
                            ((ImportBaseLine)item).ClientTreeId = clientTreeTuple.Item1;
                        }
                    }
                    IEntity<Guid> rec;
                    IList<string> warnings;
                    IList<string> validationErrors;

                    if (!validator.Validate(item, out validationErrors))
                    {
                        HasErrors = true;
                        errorRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", validationErrors)));
                    }
                    else if (!builder.Build(item, cache, context, out rec, out warnings, out validationErrors))
                    {
                        HasErrors = true;
                        errorRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", validationErrors)));
                        if (warnings.Any())
                        {
                            warningRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", warnings)));
                        }
                    }
                    else if (!IsFilterSuitable(rec, badDemandCodes, blBadBaseUniqueIdent, badZrepList, out validationErrors))
                    {
                        HasErrors = true;
                        errorRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", validationErrors)));
                    }
                    else
                    {
                        records.Add(rec);
                        successList.Add(item);
                        if (warnings.Any())
                        {
                            warningRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", warnings)));
                        }
                    }
                });
                traceLogger.Trace("second parallel finish (build)");

                //stopwatch.Stop();
                //handlerLogger.Write(true, String.Format("parallel  stopped at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", DateTimeOffset.Now, stopwatch.Elapsed.TotalSeconds), "Timing");
                //stopwatch.Restart();

                int resultRecordCount = 0;

                ResultStatus = GetImportStatus();
                var importModel = ImportUtility.BuildActiveImport(UserId, RoleId, ImportType);
                importModel.Status = ResultStatus;
                context.Imports.Add(importModel);

                //stopwatch.Stop();
                //handlerLogger.Write(true, String.Format("ApplyImport  stopped at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", DateTimeOffset.Now, stopwatch.Elapsed.TotalSeconds), "Timing");

                bool hasSuccessList = AllowPartialApply || !HasErrors;
                if (hasSuccessList)
                {
                    // Закончить импорт
                    resultRecordCount = InsertDataToDatabase(records, context);
                }

                //handlerLogger.Write(true, String.Format("ApplyImport continued at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Timing");
                //stopwatch.Restart();

                traceLogger.Trace("Persist models inserted");
                context.SaveChanges();
                traceLogger.Trace("Data saved");
                //stopwatch.Stop();
                //handlerLogger.Write(true, String.Format("Data saved  stopped at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", DateTimeOffset.Now, stopwatch.Elapsed.TotalSeconds), "Timing");
                //stopwatch.Restart();

                errorCount = errorRecords.Count;
                warningCount = warningRecords.Count;
                successCount = resultRecordCount;

                ImportResultFilesModel resultFilesModel = SaveProcessResultHelper.SaveResultToFile(
                    importModel.Id,
                    hasSuccessList ? successList : null,
                    null,
                    errorRecords,
                    warningRecords);

                if (errorCount > 0 || warningCount > 0)
                {
                    traceLogger.Trace(String.Format("Start write errors({0}) and warnings({1})", errorCount, warningCount));
                    string errorsPath = "/api/File/ImportResultErrorDownload?filename=";
                    string warningsPath = "/api/File/ImportResultWarningDownload?filename=";

                    if (errorCount > 0)
                    {
                        foreach (var record in errorRecords)
                        {
                            Errors.Add($"{ record.Item2 } <a href=\"{ errorsPath + resultFilesModel.TaskId }\">Download</a>");
                        }
                    }
                    if (warningCount > 0)
                    {
                        foreach (var record in warningRecords)
                        {
                            Warnings.Add($"{ record.Item2 } <a href=\"{ warningsPath + resultFilesModel.TaskId }\">Download</a>");
                        }
                    }
                    traceLogger.Trace("Writing finished");
                }

                //stopwatch.Stop();
                //handlerLogger.Write(true, String.Format("ApplyImport ended at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", DateTimeOffset.Now, stopwatch.Elapsed.TotalSeconds), "Timing");

                return resultFilesModel;
            }
        }

        private bool IsFilterSuitable(IEntity<Guid> rec, List<String> badDemandCodes, List<Tuple<string, string, DateTimeOffset?>> blBadBaseUniqueIdent, List<string> badZrepList, out IList<string> errors)
        {
            errors = new List<string>();
            bool isSuitable = true;
            ImportBaseLine typedRec = (ImportBaseLine)rec;

            if (badDemandCodes
                .Any(z => z == typedRec.ClientTreeDemandCode))
            {
                isSuitable = false;
                errors.Add("There is no Client with such DemandCode");
            }
            //if (blBadBaseUniqueIdent
            //    .Any(z => z.Item1 == typedRec.ProductZREP && z.Item2 == typedRec.ClientTreeDemandCode && z.Item3 == typedRec.StartDate)) {
            //    isSuitable = false;
            //    errors.Add("There is such BaseLine in database");
            //}

            if (badZrepList.Any(z => z == typedRec.ProductZREP))
            {
                isSuitable = false;
                errors.Add("(not found)Запись типа 'Product' не найдена");
            }

            return isSuitable;
        }
        protected override int InsertDataToDatabase(IEnumerable<IEntity<Guid>> sourceRecords, DatabaseContext context)
        {
            //traceLogger.Trace("Start Insert Data To Database");
            //stopwatch.Restart();
            //handlerLogger.Write(true, String.Format("Insert to database started at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Timing");
            context.Database.CommandTimeout = 3000;
            ScriptGenerator generator = GetScriptGenerator();
            IList<BaseLine> toCreate = new List<BaseLine>();
            IList<BaseLine> toUpdate = new List<BaseLine>();
            List<Tuple<IEntity<Guid>, IEntity<Guid>>> toHisCreate = new List<Tuple<IEntity<Guid>, IEntity<Guid>>>();
            List<Tuple<IEntity<Guid>, IEntity<Guid>>> toHisUpdate = new List<Tuple<IEntity<Guid>, IEntity<Guid>>>();
            //handlerLogger.Write(true, String.Format("Filtered records started at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Timing");
            traceLogger.Trace("Start filter records");
            IQueryable<ImportBaseLine> filteredRecords = AddFilter(sourceRecords.Cast<ImportBaseLine>().AsQueryable());
            traceLogger.Trace("Finish filter records");
            if (NeedClearData)
            {
                traceLogger.Trace("Start build filter for delete");
                PersistFilter filter = ModuleApplyFilterHelper.BuildBaseLineFilter(StartDate, FinishDate, Filters);
                traceLogger.Trace("Filter builded");
                string deleteScript = generator.BuildDeleteScript(filter);
                traceLogger.Trace("Script generated");
                try
                {
                    traceLogger.Trace("Start delete script executing");
                    context.Database.ExecuteSqlCommand(deleteScript);
                    traceLogger.Trace("Delete script executing finished");
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            traceLogger.Trace("Start grouping");
            // Забор по уникальным полям
            var newRecordList = filteredRecords.Select(bli => (ImportBaseLine)bli);
            traceLogger.Trace("Finish grouping");
            //stopwatch.Stop();
            //handlerLogger.Write(true, String.Format("Filtered records ended at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", DateTimeOffset.Now, stopwatch.Elapsed.TotalSeconds), "Timing");
            //handlerLogger.Write(true, String.Format("Type 1 record started at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Timing");
            //stopwatch.Restart();
            traceLogger.Trace("Start group iterate");

            List<Product> products = context.Set<Product>().Where(x => !x.Disabled).ToList();
            List<ClientTree> clientTrees = context.Set<ClientTree>().ToList();
            List<BaseLine> baseLines = context.Set<BaseLine>().Where(x => !x.Disabled).ToList();
            //stopwatch.Stop();
            //handlerLogger.Write(true, String.Format("GEt lists at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", DateTimeOffset.Now, stopwatch.Elapsed.TotalSeconds), "Timing");
            //stopwatch.Restart();
            foreach (var newRecord in newRecordList)
            {
                if (newRecord != null)
                {
                    Product product = products.FirstOrDefault(x => x.ZREP == newRecord.ProductZREP && !x.Disabled);
                    if (product != null)
                    {
                        BaseLine oldRecord = baseLines
                            .FirstOrDefault(x => x.ProductId == product.Id && x.ClientTreeId == newRecord.ClientTreeId && x.StartDate == newRecord.StartDate);
                        var oldRecordCopy = oldRecord;
                        if (oldRecord != null)
                        {
                            oldRecord.QTY = newRecord.QTY;
                            oldRecord.Price = newRecord.Price;
                            oldRecord.BaselineLSV = newRecord.BaselineLSV;
                            oldRecord.Type = newRecord.Type;
                            oldRecord.LastModifiedDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                            toHisUpdate.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(oldRecordCopy, oldRecord));
                            toUpdate.Add(oldRecord);
                        }
                        else
                        {
                            BaseLine toSave = new BaseLine()
                            {
                                Id = Guid.NewGuid(),
                                ProductId = product.Id,
                                ClientTreeId = newRecord.ClientTreeId,
                                StartDate = newRecord.StartDate,
                                QTY = newRecord.QTY,
                                Price = newRecord.Price,
                                BaselineLSV = newRecord.BaselineLSV,
                                Type = newRecord.Type,
                                LastModifiedDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)
                            };
                            toCreate.Add(toSave);
                            toHisCreate.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(null, toSave));
                        }
                    }
                }
            }
            traceLogger.Trace("Finish group iterate");
            //stopwatch.Stop();
            //handlerLogger.Write(true, String.Format("Type 1 record ended at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", DateTimeOffset.Now, stopwatch.Elapsed.TotalSeconds), "Timing");
            //handlerLogger.Write(true, String.Format("Fill ToCreate started at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Timing");
            //stopwatch.Restart();
            traceLogger.Trace("Start records creating");
            foreach (IEnumerable<BaseLine> items in toCreate.Partition(10000))
            {
                string insertScript = String.Join("", items.Select(y => String.Format("INSERT INTO BaseLine (QTY, Price, BaselineLSV, Type,LastModifiedDate, ProductId ,StartDate, ClientTreeId, [Disabled], [Id]) VALUES ({0}, {1}, {2}, {3}, '{4:yyyy-MM-dd HH:mm:ss +03:00}', '{5}', '{6:yyyy-MM-dd HH:mm:ss +03:00}', {7}, '{8}', '{9}');",
                    y.QTY, y.Price, y.BaselineLSV, y.Type, y.LastModifiedDate, y.ProductId.ToString(), y.StartDate, y.ClientTreeId, y.Disabled, y.Id)));
                context.Database.ExecuteSqlCommand(insertScript);
            }
            traceLogger.Trace("Finish records creating");
            //stopwatch.Stop();
            //handlerLogger.Write(true, String.Format("Fill ToCreate ended at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", DateTimeOffset.Now, stopwatch.Elapsed.TotalSeconds), "Timing");
            //handlerLogger.Write(true, String.Format("Fill ToUpdate started at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Timing");
            //stopwatch.Restart();
            traceLogger.Trace("Start records updating");
            foreach (IEnumerable<BaseLine> items in toUpdate.Partition(1000))
            {
                string insertScript = String.Join("", items.Select(y => String.Format("UPDATE BaseLine SET QTY = {0}, Price = {1}, BaselineLSV = {2}, Type = {3},LastModifiedDate = '{4:yyyy-MM-dd HH:mm:ss +03:00}'  WHERE Id = '{5}';", y.QTY, y.Price, y.BaselineLSV, y.Type, y.LastModifiedDate, y.Id)));
                context.Database.ExecuteSqlCommand(insertScript);
            }
            traceLogger.Trace("Finish records updating");
            //stopwatch.Stop();
            //handlerLogger.Write(true, String.Format("Fill ToUpdate ended at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", DateTimeOffset.Now, stopwatch.Elapsed.TotalSeconds), "Timing");
            //handlerLogger.Write(true, String.Format("Insert in history started at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Timing");
            //stopwatch.Restart();
            //Добавление изменений в историю
            //traceLogger.Trace("Start history writing");
            context.HistoryWriter.Write(toHisCreate, context.AuthManager.GetCurrentUser(), context.AuthManager.GetCurrentRole(), OperationType.Created);
            context.HistoryWriter.Write(toHisUpdate, context.AuthManager.GetCurrentUser(), context.AuthManager.GetCurrentRole(), OperationType.Updated);
            //traceLogger.Trace("finish history writing");
            //stopwatch.Stop();
            //handlerLogger.Write(true, String.Format("Insert in history ended at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", DateTimeOffset.Now, stopwatch.Elapsed.TotalSeconds), "Timing");
            //handlerLogger.Write(true, String.Format("Save changes started at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Timing");
            //stopwatch.Restart();
            //traceLogger.Trace("start save changes");
            context.SaveChanges();
            //traceLogger.Trace("finish save changes");
            //stopwatch.Stop();
            //handlerLogger.Write(true, String.Format("Save changes ended at {0:yyyy-MM-dd HH:mm:ss}; Duration: {1} seconds", DateTimeOffset.Now, stopwatch.Elapsed.TotalSeconds), "Timing");
            //handlerLogger.Write(true, String.Format("Insert to database ended at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Timing");
            return filteredRecords.Count();
        }

        private IQueryable<ImportBaseLine> AddFilter(IQueryable<ImportBaseLine> source)
        {
            return ModuleApplyFilterHelper.ApplyFilter(source, StartDate, FinishDate, Filters);
        }
    }
}