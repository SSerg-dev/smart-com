﻿using Interfaces.Implementation.Import.FullImport;
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

namespace Module.Host.TPM.Actions {
    class FullXLSXImportBaseLineAction : FullXLSXImportAction {

        private readonly bool NeedClearData;
        private readonly DateTimeOffset StartDate;
        private readonly DateTimeOffset FinishDate;
        private readonly IDictionary<string, IEnumerable<string>> Filters;

        public FullXLSXImportBaseLineAction(FullImportSettings settings, DateTimeOffset startDate, DateTimeOffset finishDate, IDictionary<string, IEnumerable<string>> filters, bool needClearData) : base(settings) {
            NeedClearData = needClearData;
            StartDate = startDate;
            FinishDate = finishDate;
            Filters = filters;
            UniqueErrorMessage = "This entry already exists in the database";
            ErrorMessageScaffold = "FullImportAction failed: {0}";
            FileParseErrorMessage = "An error occurred while parsing the import file";
        }

        protected override ImportResultFilesModel ApplyImport(IList<IEntity<Guid>> sourceRecords, out int successCount, out int warningCount, out int errorCount) {

            // Логика переноса данных из временной таблицы в постоянную
            // Получить записи текущего импорта
            using (DatabaseContext context = new DatabaseContext()) {

                var records = new ConcurrentBag<IEntity<Guid>>();
                var successList = new ConcurrentBag<IEntity<Guid>>();
                var errorRecords = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();
                var warningRecords = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();

                // Получить функцию Validate
                var validator = ImportModelFactory.GetImportValidator(ImportType);
                // Получить функцию SetProperty
                var builder = ImportModelFactory.GetModelBuilder(ImportType, ImportType);

                //Отфильтровать все записи с типом не 1
                sourceRecords = sourceRecords.Where(y => ((ImportBaseLine) y).Type == 1).ToList();
                sourceRecords = sourceRecords.Select(y => (ImportBaseLine) y).GroupBy(bl => new { bl.ProductZREP, bl.ClientTreeDemandCode, bl.StartDate })
                .Select(y => (IEntity<Guid>) y.FirstOrDefault()).ToList();

                var cacheBuilder = ImportModelFactory.GetImportCacheBuilder(ImportType);
                var cache = cacheBuilder.Build(sourceRecords, context);

                IList<ClientTree> clientTreeList = context.Set<ClientTree>().AsNoTracking().Where(x => x.DemandCode != null && !x.EndDate.HasValue).ToList();
                IList<Tuple<int, string>> clientTreeTupleList = clientTreeList.Select(x => new Tuple<int, string>(x.Id, x.DemandCode)).ToList();
                IList<string> clientTreeDemandCodeList = clientTreeTupleList.Select(x => x.Item2).ToList();


                //Обрезка ZREP
                List<string> zrepList = new List<string>();
                foreach (var item in sourceRecords) {
                    String zrep = ((ImportBaseLine) item).ProductZREP;
                    var splited = zrep.Split('_');
                    if (splited.Length > 1) {
                        zrep = String.Join("_", splited.Take(splited.Length - 1));
                    }
                    ((ImportBaseLine) item).ProductZREP = zrep;
                    zrepList.Add(zrep);
                }

                List<string> badZrepList = zrepList.Where(z => !context.Set<Product>().Select(p => p.ZREP).Contains(z)).ToList();


                //Проверка по наличию клиента
                List<String> badDemandCodes = sourceRecords
                    .Where(y => !clientTreeDemandCodeList.Contains(((ImportBaseLine) y).ClientTreeDemandCode))
                    .Select(z => ((ImportBaseLine) z).ClientTreeDemandCode).ToList();

                //Проверка уникальности
                List<Tuple<string, string, DateTimeOffset?>> blUniqueIdent = sourceRecords
                    .Select(z => (ImportBaseLine) z)
                    .Select(u => new Tuple<string, string, DateTimeOffset?>(u.ProductZREP, u.ClientTreeDemandCode, u.StartDate)).ToList();
                //Базы
                List<Tuple<string, string, DateTimeOffset?>> blBadBaseUniqueIdent = blUniqueIdent
                    .Where(y => context.Set<BaseLine>()
                    .Any(z => z.Product.ZREP == y.Item1 && z.ClientTree.DemandCode == y.Item2 && z.StartDate == y.Item3 && !z.Disabled))
                    .ToList();

                Parallel.ForEach(sourceRecords, item => {
                    string demandCode = ((ImportBaseLine) item).ClientTreeDemandCode;
                    if (clientTreeDemandCodeList.Contains(demandCode)) {
                        Tuple<int, string> clientTreeTuple = clientTreeTupleList.FirstOrDefault(x => x.Item2 == demandCode);
                        if (clientTreeTuple != null) {
                            ((ImportBaseLine) item).ClientTreeId = clientTreeTuple.Item1;
                        }
                    }
                });

                Parallel.ForEach(sourceRecords, item => {
                    IEntity<Guid> rec;
                    IList<string> warnings;
                    IList<string> validationErrors;

                    if (!validator.Validate(item, out validationErrors)) {
                        HasErrors = true;
                        errorRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", validationErrors)));
                    } else if (!builder.Build(item, cache, context, out rec, out warnings, out validationErrors)) {
                        HasErrors = true;
                        errorRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", validationErrors)));
                        if (warnings.Any()) {
                            warningRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", warnings)));
                        }
                    } else if (!IsFilterSuitable(rec, badDemandCodes, blBadBaseUniqueIdent, badZrepList, out validationErrors)) {
                        HasErrors = true;
                        errorRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", validationErrors)));
                    } else {
                        records.Add(rec);
                        successList.Add(item);
                        if (warnings.Any()) {
                            warningRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", warnings)));
                        }
                    }
                });

                logger.Trace("Persist models built");

                int resultRecordCount = 0;

                ResultStatus = GetImportStatus();
                var importModel = ImportUtility.BuildActiveImport(UserId, RoleId, ImportType);
                importModel.Status = ResultStatus;
                context.Imports.Add(importModel);

                bool hasSuccessList = AllowPartialApply || !HasErrors;
                if (hasSuccessList) {
                    // Закончить импорт
                    resultRecordCount = InsertDataToDatabase(records, context);
                }
                logger.Trace("Persist models inserted");
                context.SaveChanges();
                logger.Trace("Data saved");

                errorCount = errorRecords.Count;
                warningCount = warningRecords.Count;
                successCount = resultRecordCount;
                ImportResultFilesModel resultFilesModel = SaveProcessResultHelper.SaveResultToFile(
                    importModel.Id,
                    hasSuccessList ? successList : null,
                    null,
                    errorRecords,
                    warningRecords);

                return resultFilesModel;
            }
        }

        private bool IsFilterSuitable(IEntity<Guid> rec, List<String> badDemandCodes, List<Tuple<string, string, DateTimeOffset?>> blBadBaseUniqueIdent, List<string> badZrepList, out IList<string> errors) {
            errors = new List<string>();
            bool isSuitable = true;
            ImportBaseLine typedRec = (ImportBaseLine) rec;

            if (badDemandCodes
                .Any(z => z == typedRec.ClientTreeDemandCode)) {
                isSuitable = false;
                errors.Add("There is no Client with such DemandCode");
            }

            //if (blBadBaseUniqueIdent
            //    .Any(z => z.Item1 == typedRec.ProductZREP && z.Item2 == typedRec.ClientTreeDemandCode && z.Item3 == typedRec.StartDate)) {
            //    isSuitable = false;
            //    errors.Add("There is such BaseLine in database");
            //}

            if (badZrepList.Any(z => z == typedRec.ProductZREP)) {
                isSuitable = false;
                errors.Add("(not found)Запись типа 'Product' не найдена");
            }

            return isSuitable;
        }
        protected override int InsertDataToDatabase(IEnumerable<IEntity<Guid>> sourceRecords, DatabaseContext context) {
            ScriptGenerator generator = GetScriptGenerator();
            IList<BaseLine> toCreate = new List<BaseLine>();
            IList<BaseLine> toUpdate = new List<BaseLine>();
            IQueryable<ImportBaseLine> filteredRecords = AddFilter(sourceRecords.Cast<ImportBaseLine>().AsQueryable());
            if (NeedClearData) {
                PersistFilter filter = ModuleApplyFilterHelper.BuildBaseLineFilter(StartDate, FinishDate, Filters);
                string deleteScript = generator.BuildDeleteScript(filter);
                context.Database.ExecuteSqlCommand(deleteScript);
            }
            // Забор по уникальным полям
            var groups = filteredRecords.Select(bli => (ImportBaseLine) bli).GroupBy(bl => new { bl.ProductZREP, bl.ClientTreeDemandCode, bl.StartDate });
            foreach (var group in groups) {
                //Выбор записи с Type 1
                ImportBaseLine newRecord = group.FirstOrDefault(y => y.Type == 1);

                if (newRecord != null) {
                    Product product = context.Set<Product>().FirstOrDefault(x => x.ZREP == newRecord.ProductZREP && !x.Disabled);
                    if (product != null) {
                        BaseLine oldRecord = context.Set<BaseLine>()
                            .FirstOrDefault(x => x.ProductId == product.Id && x.ClientTreeId == newRecord.ClientTreeId && x.StartDate == newRecord.StartDate && !x.Disabled);
                        if (oldRecord != null) {
                            oldRecord.QTY = newRecord.QTY;
                            oldRecord.Price = newRecord.Price;
                            oldRecord.BaselineLSV = newRecord.BaselineLSV;
                            oldRecord.Type = newRecord.Type;
                            oldRecord.LastModifiedDate = DateTime.Now;
                            toUpdate.Add(oldRecord);
                        } else {
                            BaseLine toSave = new BaseLine() {
                                ProductId = product.Id,
                                ClientTreeId = newRecord.ClientTreeId,
                                StartDate = newRecord.StartDate,
                                QTY = newRecord.QTY,
                                Price = newRecord.Price,
                                BaselineLSV = newRecord.BaselineLSV,
                                Type = newRecord.Type,
                                LastModifiedDate = DateTime.Now
                            };
                            toCreate.Add(toSave);
                        }
                    }
                }
            }

            foreach (IEnumerable<BaseLine> items in toCreate.Partition(10000)) {
                context.Set<BaseLine>().AddRange(items);
            }

            foreach (IEnumerable<BaseLine> items in toUpdate.Partition(1000)) {
                string insertScript = String.Join("", items.Select(y => String.Format("UPDATE BaseLine SET QTY = {0}, Price = {1}, BaselineLSV = {2}, Type = {3},LastModifiedDate = '{4:yyyy-MM-dd HH:mm:ss +03:00}'  WHERE Id = '{5}';", y.QTY, y.Price, y.BaselineLSV, y.Type, y.LastModifiedDate, y.Id)));
                context.Database.ExecuteSqlCommand(insertScript);
            }

            //Добавление изменений в историю
            List<Core.History.OperationDescriptor<Guid>> toHis = new List<Core.History.OperationDescriptor<Guid>>();
            foreach (var item in toUpdate) {
                toHis.Add(new Core.History.OperationDescriptor<Guid>() { Operation = OperationType.Updated, Entity = item });
            }
            context.HistoryWriter.Write(toHis.ToArray(), context.AuthManager.GetCurrentUser(), context.AuthManager.GetCurrentRole());

            context.SaveChanges();

            return filteredRecords.Count();
        }

        private IQueryable<ImportBaseLine> AddFilter(IQueryable<ImportBaseLine> source) {
            return ModuleApplyFilterHelper.ApplyFilter(source, StartDate, FinishDate, Filters);
        }
    }
}