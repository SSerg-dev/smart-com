using Core.Data;
using Core.Extensions;
using Interfaces.Implementation.Action;
using Interfaces.Implementation.Import.FullImport;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Host.TPM.Util;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using NLog;
using Persist;
using Persist.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using Utility;
using Utility.FileWorker;
using Utility.Import;

namespace Module.Host.TPM.Actions
{
    public class FullXLSXRPAActualEanPcImportAction : BaseAction
    {
        private readonly Guid UserId;
        private readonly Guid RoleId;
        private readonly Guid RPAId;
        private readonly FileModel ImportFile;
        private readonly Type ImportType;
        private readonly Type ModelType;
        private readonly string Separator;
        private readonly string Quote;
        private readonly bool HasHeader;

        private ConcurrentBag<IEntity<Guid>> successList = new ConcurrentBag<IEntity<Guid>>();
        private ConcurrentBag<Tuple<IEntity<Guid>, string>> errorRecords = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();
        private ConcurrentBag<Tuple<IEntity<Guid>, string>> warningRecords = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();

        private bool AllowPartialApply { get; set; }
        private readonly Logger logger;
        private string ResultStatus { get; set; }
        private bool HasErrors { get; set; }

        private ScriptGenerator Generator { get; set; }

        public FullXLSXRPAActualEanPcImportAction(FullImportSettings settings, Guid RPAId)
        {
            UserId = settings.UserId;
            RoleId = settings.RoleId;
            ImportFile = settings.ImportFile;
            ImportType = settings.ImportType;
            ModelType = settings.ModelType;
            Separator = settings.Separator;
            Quote = settings.Quote;
            HasHeader = settings.HasHeader;
            this.RPAId = RPAId;

            AllowPartialApply = true;
            logger = LogManager.GetCurrentClassLogger();
        }

        public override void Execute()
        {
            logger.Trace("Begin");
            try
            {
                ResultStatus = null;
                HasErrors = false;

                var rpaStatus = "In progress";
                using (var context = new DatabaseContext())
                {
                    var rpa = context.Set<RPA>().FirstOrDefault(x => x.Id == RPAId);
                    rpa.Status = rpaStatus;
                    context.SaveChanges();
                }

                var sourceRecords = ParseImportFile();

                int successCount;
                int warningCount;
                int errorCount;

                var resultFilesModel = ApplyImport(sourceRecords, out successCount, out warningCount, out errorCount);

                // Сохранить выходные параметры
                Results["ImportSourceRecordCount"] = sourceRecords.Count();
                Results["ImportResultRecordCount"] = successCount;
                Results["ErrorCount"] = errorCount;
                Results["WarningCount"] = warningCount;
                Results["ImportResultFilesModel"] = resultFilesModel;

            }
            catch (Exception e)
            {
                HasErrors = true;
                string message = String.Format("FullImportAction failed: {0}", e.ToString());
                logger.Error(message);

                if (e.IsUniqueConstraintException())
                {
                    message = "This entry already exists in the database.";
                }
                else
                {
                    message = e.ToString();
                }

                Errors.Add(message);
                ResultStatus = ImportUtility.StatusName.ERROR;
            }
            finally
            {
                // информация о том, какой долен быть статус у задачи
                Results["ImportResultStatus"] = ResultStatus;
                logger.Debug("Finish");
            }
        }

        private IList<IEntity<Guid>> ParseImportFile()
        {
            var fileDispatcher = new FileDispatcher();
            string importDir = Core.Settings.AppSettingsManager.GetSetting("RPA_DIRECTORY", "RPAFiles");

            string importFilePath = Path.Combine(importDir, ImportFile.Name);
            if (!fileDispatcher.IsExists(importDir, ImportFile.Name))
            {
                throw new Exception("Import File not found");
            }

            var builder = ImportModelFactory.GetCSVImportModelBuilder(ImportType);
            var validator = ImportModelFactory.GetImportValidator(ImportType);
            int sourceRecordCount;
            List<string> errors;
            IList<Tuple<string, string>> buildErrors;
            IList<Tuple<IEntity<Guid>, string>> validateErrors;
            logger.Trace("before parse file");
            IList<IEntity<Guid>> records = ImportUtilityTPM.ParseXLSXFile(importFilePath, null, builder, validator, Separator, Quote, HasHeader, out sourceRecordCount, out errors, out buildErrors, out validateErrors);
            logger.Trace("after parse file");

            // Обработать ошибки
            foreach (string err in errors)
            {
                Errors.Add(err);
            }
            if (errors.Any())
            {
                HasErrors = true;
                throw new ImportException("An error occurred while parsing the import file.");
            }

            return records;
        }

        private ImportResultFilesModel ApplyImport(IList<IEntity<Guid>> sourceRecords, out int successCount, out int warningCount, out int errorCount)
        {

            // Логика переноса данных из временной таблицы в постоянную
            // Получить записи текущего импорта
            using (DatabaseContext context = new DatabaseContext())
            {

                var records = new ConcurrentBag<IEntity<Guid>>();
                IList<IEntity<Guid>> sourceRecordWithoutDuplicates = new List<IEntity<Guid>>();
                IList<string> warnings = new List<string>();
                IList<string> validationErrors = new List<string>();

                // Получить функцию Validate
                var validator = ImportModelFactory.GetImportValidator(ImportType);
                // Получить функцию SetProperty
                var builder = ImportModelFactory.GetModelBuilder(ImportType, ModelType);


                var cacheBuilder = ImportModelFactory.GetImportCacheBuilder(ImportType);
                var cache = cacheBuilder.Build(sourceRecords, context);

                //Ограничение пользователя  
                IList<Constraint> constraints = context.Constraints
                    .Where(x => x.UserRole.UserId.Equals(UserId) && x.UserRole.Role.Id.Equals(RoleId))
                    .ToList();
                IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
                //здесь должны быть все записи, а не только неудаленные!
                IQueryable<ClientTree> query = context.Set<ClientTree>().AsNoTracking();
                IQueryable<ClientTreeHierarchyView> hierarchy = context.Set<ClientTreeHierarchyView>().AsNoTracking();
                query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);
                List<ClientTree> existingClientTreeIds = query.ToList();

                // Проверка на дубликаты и остальное........
                CheckForDuplicates(context, sourceRecords, out sourceRecordWithoutDuplicates, out validationErrors);

                //Стандартные проверки
                foreach (var item in sourceRecordWithoutDuplicates)
                {

                    IEntity<Guid> rec;

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
                    else if (!IsFilterSuitable(ref rec, context, out validationErrors, existingClientTreeIds))
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
                };

                logger.Trace("Persist models built");

                int resultRecordCount = 0;

                ResultStatus = GetImportStatus();
                var rpaStatus = ResultStatus;
                var rpa = context.Set<RPA>().FirstOrDefault(x => x.Id == RPAId);
                rpa.Status = rpaStatus;
                var importModel = ImportUtility.BuildActiveImport(UserId, RoleId, ImportType);
                importModel.Status = ResultStatus;
                context.Imports.Add(importModel);

                bool hasSuccessList = AllowPartialApply || !HasErrors;
                if (hasSuccessList)
                {
                    // Закончить импорт
                    resultRecordCount = InsertDataToDatabase(records, context);
                    //CalculateBudgetsCreateTask(context, promoesForBudgetCalculation);
                }
                logger.Trace("Persist models inserted");
                context.SaveChanges();
                logger.Trace("Data saved");

                errorCount = errorRecords.Count;
                warningCount = warningRecords.Count;
                successCount = successList.Count;
                ImportResultFilesModel resultFilesModel = SaveProcessResultHelper.SaveResultToFile(
                    importModel.Id,
                    hasSuccessList ? successList : null,
                    null,
                    errorRecords,
                    warningRecords);

                if (errorCount > 0 || warningCount > 0)
                {
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
                }

                return resultFilesModel;
            }
        }

        private void CheckForDuplicates(DatabaseContext context, IList<IEntity<Guid>> templateRecordIds, out IList<IEntity<Guid>> distinctRecordIds, out IList<string> errors)
        {
            distinctRecordIds = new List<IEntity<Guid>>();
            errors = new List<string>();


            var sourceTemplateRecords = templateRecordIds
                .Select(sr => (sr as ImportRpaActualEanPc));

            var shortPromoes = context.Set<Promo>()
                .Select(ps => new
                {
                    ps.Id,
                    ps.Disabled,
                    ps.ClientTreeId,
                    ps.Number,
                    ps.PromoStatus.SystemName
                });

            var joinPromoSupports = sourceTemplateRecords
                .Join(shortPromoes,
                        str => str.PromoNumberImport,
                        ssp => ssp.Number,
                        (str, ssp) => new
                        {
                            PromoNumberImport = str.PromoNumberImport,
                            EanPcImport = str.EanPcImport,
                            ActualProductPcQuantityImport = str.ActualProductPcQuantityImport,
                            PromoId = ssp.Id,
                            Disabled = ssp.Disabled,
                            ClietTreeId = ssp.ClientTreeId,
                            StatusName = ssp.SystemName
                        });

            bool isDuplicateRecords = joinPromoSupports
                .GroupBy(jps => new
                {
                    jps.PromoNumberImport,
                    jps.EanPcImport
                })
                .Any(gr => gr.Count() > 1);

            if (isDuplicateRecords)
            {
                errors.Add("The record with this PromoId - EAN_PC pair occurs more than once");
            }

            var promoWithoutDuplicates = joinPromoSupports
                .GroupBy(jps => new
                {
                    jps.PromoNumberImport,
                    jps.EanPcImport
                })
                .Select(jps => jps.First())
                .ToList();

            distinctRecordIds = promoWithoutDuplicates
                .Select(p => new ImportRpaActualEanPc
                {
                    PromoNumberImport = p.PromoNumberImport,
                    EanPcImport = p.EanPcImport,
                    ActualProductPcQuantityImport = p.ActualProductPcQuantityImport,
                    PromoId = p.PromoId,
                    StatusName = p.StatusName
                } as IEntity<Guid>)
                .ToList();

        }

        private bool IsFilterSuitable(ref IEntity<Guid> rec, DatabaseContext context, out IList<string> errors, List<ClientTree> existingClientTreeIds)
        {
            errors = new List<string>();
            warnings = new List<string>();
            bool isSuitable = true;

            ImportRpaActualEanPc typedRec = (ImportRpaActualEanPc)rec;
            var promo = context.Set<Promo>().FirstOrDefault(x => x.Number == typedRec.PromoNumberImport && !x.Disabled);
            if (promo == null)
            {
                errors.Add("Promo not found or deleted");
                isSuitable = false;
                return isSuitable;
            }
            if (!existingClientTreeIds.Any(x => x.ObjectId == promo.ClientTree.ObjectId))
            {
                errors.Add("No access to the client");
                isSuitable = false;
            }
            var promoProduct = context.Set<PromoProduct>().FirstOrDefault(x => x.PromoId == promo.Id && !x.Disabled);
            if (promoProduct == null)
            {
                errors.Add("PromoProduct not found");
                isSuitable = false;
            }
            var promoStatus = context.Set<PromoStatus>().FirstOrDefault(x => x.SystemName == promo.PromoStatus.SystemName && !x.Disabled);
            if (promoStatus.SystemName != "Finished")
            {
                errors.Add("Invalid promo status");
                isSuitable = false;
            }
            return isSuitable;
        }

        private int InsertDataToDatabase(IEnumerable<IEntity<Guid>> sourceRecords, DatabaseContext context)
        {

            var sourcePromo = sourceRecords
                 .Select(sr => sr as ImportRpaActualEanPc)
                 .ToList();

            var sourcePromoIds = sourcePromo
                .Distinct()
                .Select(ps => ps.PromoId)
                .ToList();


            var promoes = context.Set<Promo>()
                .Where(x => sourcePromoIds.Contains(x.Id) && !x.Disabled)
                .ToList();


            ScriptGenerator generator = GetScriptGenerator();
            IList<PromoProduct> query = GetQuery(context).ToList();

            List<PromoProduct> toUpdate = new List<PromoProduct>();
            List<Tuple<IEntity<Guid>, IEntity<Guid>>> toHisUpdate = new List<Tuple<IEntity<Guid>, IEntity<Guid>>>();
            foreach (Promo promo in promoes)
            {
                if (promo != null)
                {
                    if (!promo.InOut.HasValue || !promo.InOut.Value)
                    {
                        foreach (ImportRpaActualEanPc itemRecord in sourceRecords)
                        {
                            PromoProduct promoProduct = context.Set<PromoProduct>()
                                .FirstOrDefault(pp => pp.PromoId == itemRecord.PromoId);
                            // выбор продуктов с ненулевым BaseLine (проверка Baseline ниже)
                            var productsWithRealBaseline = query.Where(x => x.EAN_PC == promoProduct.EAN_PC && x.PromoId == promo.Id && !x.Disabled).ToList();

                            if (productsWithRealBaseline != null && productsWithRealBaseline.Count() > 0)
                            {
                                //распределение импортируемого количества пропорционально PlanProductBaselineLSV
                                var sumBaseline = productsWithRealBaseline.Sum(x => x.PlanProductBaselineLSV);
                                List<Tuple<IEntity<Guid>, IEntity<Guid>>> promoProductsToUpdateHis = new List<Tuple<IEntity<Guid>, IEntity<Guid>>>();
                                List<PromoProduct> promoProductsToUpdate = new List<PromoProduct>();
                                bool isRealBaselineExist = false;
                                foreach (var p in productsWithRealBaseline)
                                {
                                    var newRecordClone = ClonePromoProduct(promoProduct);
                                    p.ActualProductUOM = "PC";
                                    // проверка Baseline (исправляет ActualProductPCQty)
                                    if (p.PlanProductBaselineLSV != null && p.PlanProductBaselineLSV != 0)
                                    {
                                        if (p.PlanProductIncrementalLSV != 0 && sumBaseline != 0)
                                        {
                                            p.ActualProductPCQty = (int?)(promoProduct.ActualProductPCQty * ((decimal?)sumBaseline / (decimal?)p.PlanProductBaselineLSV));
                                        }
                                        else
                                        {
                                            p.ActualProductPCQty = promoProduct.ActualProductPCQty;
                                        }
                                        isRealBaselineExist = true;
                                    }
                                    else
                                    {
                                        p.ActualProductPCQty = null;
                                        newRecordClone.ActualProductPCQty = null;
                                    }

                                    promoProductsToUpdateHis.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(p, newRecordClone));
                                    promoProductsToUpdate.Add(p);
                                }

                                if (isRealBaselineExist)
                                {
                                    var differenceActualProductPCQty = promoProduct.ActualProductPCQty - promoProductsToUpdate.Sum(x => x.ActualProductPCQty);
                                    if (differenceActualProductPCQty.HasValue && differenceActualProductPCQty != 0)
                                    {
                                        var firstRealBaselineItem = promoProductsToUpdate.FirstOrDefault(x => x.ActualProductPCQty != null);
                                        firstRealBaselineItem.ActualProductPCQty += differenceActualProductPCQty;
                                    }
                                }
                                else
                                {
                                    //TODO: вывод предупреждения
                                    //если не найдено продуктов с ненулевым basline, просто записываем импортируемое количество в первый попавшийся продукт, чтобы сохранилось
                                    warningRecords.Add(new Tuple<IEntity<Guid>, string>(promo, "Plan Product Base Line LSV is 0"));
                                    PromoProduct oldRecord = query.FirstOrDefault(x => x.EAN_PC == promoProduct.EAN_PC && x.PromoId == promo.Id && !x.Disabled);
                                    if (oldRecord != null)
                                    {
                                        oldRecord.ActualProductUOM = "PC";
                                        oldRecord.ActualProductPCQty = promoProduct.ActualProductPCQty;
                                        toUpdate.Add(oldRecord);
                                        toHisUpdate.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(oldRecord, promoProduct));
                                    }
                                }

                                toUpdate.AddRange(promoProductsToUpdate);
                                toHisUpdate.AddRange(promoProductsToUpdateHis);
                            }
                        }
                    }
                    else
                    {
                        foreach (ImportRpaActualEanPc itemRecord in sourceRecords)
                        {
                            PromoProduct promoProduct = context.Set<PromoProduct>()
                                .FirstOrDefault(pp => pp.PromoId == itemRecord.PromoId);
                            //в случае inout промо выбираем продукты с ненулевой ценой PlanProductPCPrice, которая подбирается из справочника IncrementalPromo
                            var productsWithRealPCPrice = query.Where(x => x.EAN_PC == promoProduct.EAN_PC && x.PromoId == promo.Id && !x.Disabled).ToList();

                            if (productsWithRealPCPrice != null && productsWithRealPCPrice.Count() > 0)
                            {
                                //распределение импортируемого количества пропорционально PlanProductIncrementalLSV
                                var sumIncremental = productsWithRealPCPrice.Sum(x => x.PlanProductIncrementalLSV);
                                List<PromoProduct> promoProductsToUpdate = new List<PromoProduct>();
                                List<Tuple<IEntity<Guid>, IEntity<Guid>>> promoProductsToUpdateHis = new List<Tuple<IEntity<Guid>, IEntity<Guid>>>();
                                bool isRealPCPriceExist = false;
                                foreach (var p in productsWithRealPCPrice)
                                {
                                    var newRecordClone = ClonePromoProduct(promoProduct);
                                    p.ActualProductUOM = "PC";
                                    // проверка Price (исправляет ActualProductPCQty)
                                    if (p.PlanProductPCPrice != null && p.PlanProductPCPrice != 0)
                                    {
                                        if (p.PlanProductIncrementalLSV != 0 && sumIncremental != 0)
                                        {
                                            p.ActualProductPCQty = (int?)(promoProduct.ActualProductPCQty * ((decimal?)sumIncremental / (decimal?)p.PlanProductIncrementalLSV));
                                        }
                                        else
                                        {
                                            p.ActualProductPCQty = promoProduct.ActualProductPCQty;
                                        }
                                        isRealPCPriceExist = true;
                                    }
                                    else
                                    {
                                        p.ActualProductPCQty = null;
                                        newRecordClone.ActualProductPCQty = null;
                                    }

                                    promoProductsToUpdateHis.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(p, newRecordClone));
                                    promoProductsToUpdate.Add(p);
                                }

                                if (isRealPCPriceExist)
                                {
                                    var differenceActualProductPCQty = promoProduct.ActualProductPCQty - promoProductsToUpdate.Sum(x => x.ActualProductPCQty);
                                    if (differenceActualProductPCQty.HasValue && differenceActualProductPCQty != 0)
                                    {
                                        var firstRealPriceItem = promoProductsToUpdate.FirstOrDefault(x => x.ActualProductPCQty != null);
                                        firstRealPriceItem.ActualProductPCQty += differenceActualProductPCQty;
                                    }
                                }
                                else
                                {
                                    //TODO: вывод предупреждения
                                    //если не найдено продуктов с ненулевым basline, просто записываем импортируемое количество в первый попавшийся продукт, чтобы сохранилось
                                    warningRecords.Add(new Tuple<IEntity<Guid>, string>(promo, "Plan Product Base Line LSV is 0"));
                                    PromoProduct oldRecord = query.FirstOrDefault(x => x.EAN_PC == promoProduct.EAN_PC && x.PromoId == promo.Id && !x.Disabled);
                                    if (oldRecord != null)
                                    {
                                        oldRecord.ActualProductUOM = "PC";
                                        oldRecord.ActualProductPCQty = promoProduct.ActualProductPCQty;
                                        toUpdate.Add(oldRecord);
                                    }
                                }

                                toUpdate.AddRange(promoProductsToUpdate);
                            }
                        }
                    }
                }

                foreach (PromoProduct item in toUpdate)
                {
                    context.Set<PromoProduct>().AddOrUpdate(item);
                }
                context.SaveChanges();
                // Обновляем фактические значения
                CreateTaskCalculateActual(promo.Id);
            }

            return sourceRecords.Count();
        }

        private ScriptGenerator GetScriptGenerator()
        {
            if (Generator == null)
            {
                Generator = new ScriptGenerator(ModelType);
            }
            return Generator;
        }

        private string GetImportStatus()
        {
            if (HasErrors)
            {
                if (AllowPartialApply)
                {
                    return ImportUtility.StatusName.PARTIAL_COMPLETE;
                }
                else
                {

                    return ImportUtility.StatusName.ERROR;
                }
            }
            else
            {
                return ImportUtility.StatusName.COMPLETE;
            }
        }

        /// <summary>
        /// Создание клона продукта
        /// </summary>
        /// <returns></returns>
        private PromoProduct ClonePromoProduct(PromoProduct promoProduct)
        {
            var promoProductClone = new PromoProduct();
            promoProductClone.ActualProductPCQty = promoProduct.ActualProductPCQty;
            promoProductClone.EAN_PC = promoProduct.EAN_PC;
            promoProductClone.ActualProductUOM = promoProduct.ActualProductUOM;
            return promoProductClone;
        }

        /// <summary>
        /// Создание отложенной задачи, выполняющей расчет фактических параметров продуктов и промо
        /// </summary>
        /// <param name="promoId">ID промо</param>
        private void CreateTaskCalculateActual(Guid promoId)
        {
            // к этому моменту промо уже заблокировано

            using (DatabaseContext context = new DatabaseContext())
            {
                HandlerData data = new HandlerData();
                HandlerDataHelper.SaveIncomingArgument("PromoId", promoId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UserId", UserId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", RoleId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("needRedistributeLSV", true, data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Calculate actual parameters",
                    Name = "Module.Host.TPM.Handlers.CalculateActualParamatersHandler",
                    ExecutionPeriod = null,
                    CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                    LastExecutionDate = null,
                    NextExecutionDate = null,
                    ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                    UserId = UserId,
                    RoleId = RoleId
                };

                BlockedPromo bp = context.Set<BlockedPromo>().First(n => n.PromoId == promoId && !n.Disabled);
                bp.HandlerId = handler.Id;

                handler.SetParameterData(data);
                context.LoopHandlers.Add(handler);
                context.SaveChanges();
            }
        }

        private IEnumerable<PromoProduct> GetQuery(DatabaseContext context)
        {
            IQueryable<PromoProduct> query = context.Set<PromoProduct>().AsNoTracking();
            return query.ToList();
        }


    }
}
