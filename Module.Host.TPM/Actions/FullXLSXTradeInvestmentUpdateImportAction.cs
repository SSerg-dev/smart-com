using Core.Data;
using Core.Dependency;
using Core.Extensions;
using Core.Settings;
using DocumentFormat.OpenXml;
using Interfaces.Implementation.Action;
using Interfaces.Implementation.Import.FullImport;
using Looper.Parameters;
using Module.Host.TPM.Util;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using NLog;
using Persist;
using Persist.Model;
using Persist.Model.Import;
using Persist.ScriptGenerator;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utility;
using Utility.FileWorker;
using Utility.Import;
using Utility.Import.Cache;
using Utility.Import.ImportModelBuilder;
using Utility.Import.ModelBuilder;

namespace Module.Host.TPM.Actions
{

    /// <summary>
    /// Переопределение Action из ядра приложения
    /// </summary>
    public class FullXLSXTradeInvestmentUpdateImportAction : BaseAction
    {

        public FullXLSXTradeInvestmentUpdateImportAction(FullImportSettings settings, int year, string importDestination)
        {
            UserId = settings.UserId;
            RoleId = settings.RoleId;
            ImportFile = settings.ImportFile;
            ImportType = settings.ImportType;
            ModelType = settings.ModelType;
            Separator = settings.Separator;
            Quote = settings.Quote;
            HasHeader = settings.HasHeader;
            Year = year;
            ImportDestination = importDestination;

            AllowPartialApply = false;
        }

        protected readonly Guid UserId;
        protected readonly Guid RoleId;
        protected readonly FileModel ImportFile;
        protected readonly Type ImportType;
        protected readonly Type ModelType;
        protected readonly string Separator;
        protected readonly string Quote;
        protected readonly bool HasHeader;
        protected readonly int Year;
        protected readonly string ImportDestination;

        protected bool AllowPartialApply { get; set; }

        protected string ResultStatus { get; set; }
        protected bool HasErrors { get; set; }

        protected readonly static Logger logger = LogManager.GetCurrentClassLogger();



        /// <summary>
        /// Выполнить разбор source-данных в импорт-модели и сохранить в БД
        /// </summary>
        public override void Execute()
        {
            logger.Trace("Begin");
            try
            {
                ResultStatus = null;
                HasErrors = false;

                IList<IEntity<Guid>> sourceRecords = ParseImportFile();

                int successCount;
                int warningCount;
                int errorCount;
                ImportResultFilesModel resultFilesModel = ApplyImport(sourceRecords, out successCount, out warningCount, out errorCount);

                if (HasErrors)
                {
                    Fail();
                }
                else
                {
                    Success();
                }

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
                string msg = String.Format("FullImportAction failed: {0}", e.ToString());
                logger.Error(msg);
                string message;
                if (e.IsUniqueConstraintException())
                {
                    message = "This entry already exists in the database";
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
                Complete();
            }
        }

        /// <summary>
        /// Выполнить разбор файла импорта
        /// </summary>
        /// <returns></returns>
        private IList<IEntity<Guid>> ParseImportFile()
        {
            var fileDispatcher = new FileDispatcher();
            string importDir = AppSettingsManager.GetSetting<string>("IMPORT_DIRECTORY", "ImportFiles");
            string importFilePath = Path.Combine(importDir, ImportFile.Name);
            if (!fileDispatcher.IsExists(importDir, ImportFile.Name))
            {
                throw new Exception("Import File not found");
            }

            IImportModelBuilder<string[]> builder = ImportModelFactory.GetCSVImportModelBuilder(ImportType);
            IImportValidator validator = ImportModelFactory.GetImportValidator(ImportType);
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
                throw new ImportException("An error occurred while retrieving the import file");
            }

            return records;
        }

        /// <summary>
        /// Загрузить импортируемые записи в БД
        /// </summary>
        /// <param name="sourceRecords"></param>
        /// <param name="successCount"></param>
        /// <param name="warningCount"></param>
        /// <param name="errorCount"></param>
        /// <returns></returns>
        private ImportResultFilesModel ApplyImport(IList<IEntity<Guid>> sourceRecords, out int successCount, out int warningCount, out int errorCount)
        {

            // Логика переноса данных из временной таблицы в постоянную
            // Получить записи текущего импорта
            using (DatabaseContext context = new DatabaseContext())
            {

                ConcurrentBag<IEntity<Guid>> records = new ConcurrentBag<IEntity<Guid>>();
                ConcurrentBag<IEntity<Guid>> successList = new ConcurrentBag<IEntity<Guid>>();
                ConcurrentBag<Tuple<IEntity<Guid>, string>> errorRecords = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();
                ConcurrentBag<Tuple<IEntity<Guid>, string>> warningRecords = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();

                // Получить функцию Validate
                IImportValidator validator = ImportModelFactory.GetImportValidator(ImportType);
                // Получить функцию SetProperty
                IModelBuilder builder = ImportModelFactory.GetModelBuilder(ImportType, ModelType);
                IImportCacheBuilder cacheBuilder = ImportModelFactory.GetImportCacheBuilder(ImportType);
                IImportCache cache = cacheBuilder.Build(sourceRecords, context);

                DateTime dtNow = DateTime.Now;

                //Ограничение пользователя  
                IList<Constraint> constraints = context.Constraints
                    .Where(x => x.UserRole.UserId.Equals(UserId) && x.UserRole.Role.Id.Equals(RoleId))
                    .ToList();
                IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
                IQueryable<ClientTree> ctQuery = context.Set<ClientTree>().AsNoTracking().Where(x => DateTime.Compare(x.StartDate, dtNow) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dtNow) > 0));
                IQueryable<ClientTreeHierarchyView> hierarchy = context.Set<ClientTreeHierarchyView>().AsNoTracking();
                ctQuery = ModuleApplyFilterHelper.ApplyFilter(ctQuery, hierarchy, filters);

                //Запрос действующих ObjectId
                IList<ClientTree> existedClientTrees = ctQuery.Where(y => y.EndDate == null || y.EndDate > dtNow).ToList();
                IList<Tuple<int, int>> existedClientTreesTuples = existedClientTrees.Select(y => new Tuple<int, int>(y.Id, y.ObjectId)).ToList();
                IList<int> existedexistedClientTreesIds = existedClientTreesTuples.Select(y => y.Item2).ToList();

                //Действующие BrandTech
                IList<BrandTech> brandTeches = context.Set<BrandTech>().Where(z => !z.Disabled).ToList();
                IList<Tuple<String, Guid?>> brandTechesTuples = brandTeches.Select(y => new Tuple<String, Guid?>(y.BrandsegTechsub, y.Id)).ToList();

                //Присваивание ID
                Parallel.ForEach(sourceRecords, importObj =>
                {
                    ImportTradeInvestment typedItem = (ImportTradeInvestment)importObj;
                    int objId = typedItem.ClientTreeObjectId;
                    if (existedexistedClientTreesIds.Contains(objId))
                    {
                        var finden = existedClientTreesTuples.FirstOrDefault(y => y.Item2 == objId);
                        if (finden != null)
                        {
                            typedItem.ClientTreeId = finden.Item1;
                        }
                    }

                    var bt = brandTechesTuples.FirstOrDefault(y => y.Item1 == typedItem.BrandsegTechsub);
                    ((ImportTradeInvestment)importObj).BrandTechId = bt == null ? null : bt.Item2;

                    typedItem.MarcCalcBudgetsBool = typedItem.MarcCalcBudgets.ToUpper() == "YES";
                    typedItem.MarcCalcROIBool = typedItem.MarcCalcROI.ToUpper() == "YES";

                });

                //Проверка по пересечению времени
                IList<Tuple<int, Guid?, String, String>> badTimesIds = new List<Tuple<int, Guid?, String, String>>();

                IList<Tuple<int, String, String, String, DateTimeOffset?, DateTimeOffset?>> existedTradeInvestmentsTimes =
                    this.GetQuery(context).Where(x => !x.Disabled).Select(y => new Tuple<int, String, String, String, DateTimeOffset?, DateTimeOffset?>(y.ClientTreeId, y.BrandTech != null ? y.BrandTech.BrandsegTechsub : null, y.TIType, y.TISubType, y.StartDate, y.EndDate)).ToList();

                IList<Tuple<int, String, String, String, DateTimeOffset?, DateTimeOffset?>> importedTradeInvestmentsTimes =
                    sourceRecords.Select(y => new Tuple<int, String, String, String, DateTimeOffset?, DateTimeOffset?>(((ImportTradeInvestment)y).ClientTreeId, ((ImportTradeInvestment)y).BrandsegTechsub, ((ImportTradeInvestment)y).TIType, ((ImportTradeInvestment)y).TISubType, ((ImportTradeInvestment)y).StartDate, ((ImportTradeInvestment)y).EndDate)).ToList();

                var importTradeInvestments = sourceRecords.Cast<ImportTradeInvestment>().Where(x => x.StartDate.HasValue && x.EndDate.HasValue);

                //Стандартные проверки
                Parallel.ForEach(sourceRecords, importObj =>
                {
                    IEntity<Guid> rec;
                    IList<string> warnings;
                    IList<string> validationErrors;

                    if (!validator.Validate(importObj, out validationErrors))
                    {
                        HasErrors = true;
                        errorRecords.Add(new Tuple<IEntity<Guid>, string>(importObj, String.Join(", ", validationErrors)));
                    }
                    else if (!builder.Build(importObj, cache, context, out rec, out warnings, out validationErrors))
                    {
                        HasErrors = true;
                        errorRecords.Add(new Tuple<IEntity<Guid>, string>(importObj, String.Join(", ", validationErrors)));
                        if (warnings.Any())
                        {
                            warningRecords.Add(new Tuple<IEntity<Guid>, string>(importObj, String.Join(", ", warnings)));
                        }
                    }
                    else if (!IsFilterSuitable(rec, importTradeInvestments, existedexistedClientTreesIds, brandTechesTuples, out validationErrors))
                    {
                        HasErrors = true;
                        errorRecords.Add(new Tuple<IEntity<Guid>, string>(importObj, String.Join(", ", validationErrors)));
                    }
                    else
                    {
                        records.Add(rec);
                        if (warnings.Any())
                        {
                            warningRecords.Add(new Tuple<IEntity<Guid>, string>(importObj, String.Join(", ", warnings)));
                        }
                    }
                });

                logger.Trace("Persist models built");

                int resultRecordCount = 0;

                ResultStatus = GetImportStatus();
                Import importModel = ImportUtility.BuildActiveImport(UserId, RoleId, ImportType);
                importModel.Status = ResultStatus;
                context.Imports.Add(importModel);

                bool hasSuccessList = AllowPartialApply || !HasErrors;
                if (hasSuccessList)
                {
                    // Закончить импорт
                    IEnumerable<IEntity<Guid>> importObjs = BeforeInsert(records, context).ToList();
                    if (this.ImportDestination == "TI")
                    {
                        resultRecordCount = InsertTIDataToDatabase(importObjs, context);
                    }
                    else if (this.ImportDestination == "ActualTI")
                    {
                        resultRecordCount = InsertActualTIDataToDatabase(importObjs, context);
                    }
                }
                if (!HasErrors)
                {
                    foreach(var importObj in sourceRecords)
                    {
                        successList.Add(importObj);
                    }

                    logger.Trace("Persist models inserted");
                    context.SaveChanges();
                    logger.Trace("Data saved");
                }
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

        protected ScriptGenerator _generator { get; set; }

        //Кастомная проверка
        protected virtual bool IsFilterSuitable(IEntity<Guid> rec, IEnumerable<ImportTradeInvestment> importTradeInvestments, IList<int> existedObjIds, IList<Tuple<String, Guid?>> brandTechesTuples, out IList<string> errors)
        {
            errors = new List<string>();
            bool isError = false;

            ImportTradeInvestment importObj = (ImportTradeInvestment)rec;

            //Проверка по существующим активным ClientTree для пользователя
            if (!existedObjIds.Contains(importObj.ClientTreeObjectId))
            {
                isError = true;
                errors.Add(importObj.ClientTreeObjectId.ToString() + " not in user's active ClientTree list");
            } 

            Tuple<String, Guid?> btech = brandTechesTuples.FirstOrDefault(x => x.Item1 == importObj.BrandsegTechsub);
            if (importObj.BrandsegTechsub != "All" && !String.IsNullOrEmpty(importObj.BrandsegTechsub) && btech == null)
            {
                isError = true;
                errors.Add(" There is no such BrandTech");
            }

            //Проверка StartDate, EndDate
            if (importObj.StartDate == null || importObj.EndDate == null)
            {
                isError = true;
                errors.Add(" StartDate and EndDate must be fullfilled");
            }
            else
            {

                if (importObj.StartDate > importObj.EndDate)
                {
                    isError = true;
                    errors.Add(" StartDate must be before EndDate");
                }

            }

            if (importObj.StartDate.HasValue && importObj.StartDate.Value.Year != this.Year)
            {
                isError = true;
                errors.Add($"({importObj.ClientTreeObjectId}, {importObj.BrandsegTechsub}) Start Date year must be equal {this.Year}.");
            }

            if (importObj.EndDate.HasValue && importObj.EndDate.Value.Year != this.Year)
            {
                isError = true;
                errors.Add($"({importObj.ClientTreeObjectId}, {importObj.BrandsegTechsub}) End Date year must be equal {this.Year}.");
            }

            var intersectDatesTradeInvestments = importTradeInvestments.Where(x => 
                importObj.ClientTreeObjectId == x.ClientTreeObjectId && importObj.BrandsegTechsub == x.BrandsegTechsub && x.TIType == importObj.TIType && x.TISubType == importObj.TISubType && importObj.StartDate >= x.StartDate && importObj.StartDate <= x.EndDate || 
                importObj.ClientTreeObjectId == x.ClientTreeObjectId && importObj.BrandsegTechsub == x.BrandsegTechsub && x.TIType == importObj.TIType && x.TISubType == importObj.TISubType && importObj.EndDate >= x.StartDate && importObj.EndDate <= x.EndDate);

            if (intersectDatesTradeInvestments.Count() > 1)
            {
                isError = true;
                errors.Add($"({importObj.ClientTreeObjectId}, {importObj.BrandsegTechsub}, {importObj.TIType}, {importObj.TISubType}) there can not be two TI of Client, BrandTech, Type, SubType in some Time.");
            }

            // SizePercent не больше 100 процентов
            if (importObj.SizePercent > 100 || importObj.SizePercent < 0)
            {
                isError = true;
                errors.Add("SizePercent must be in percentage 0 up to 100");
            }

            return !isError;
        }

        protected virtual void Fail()
        {

        }

        protected virtual void Success()
        {

        }

        protected virtual void Complete()
        {

        }

        /// <summary>
        /// Запись в базу аналогично изменению TradeInvestment из интерфейса через контекст
        /// </summary>
        /// <param name="sourceRecords"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected int InsertTIDataToDatabase(IEnumerable<IEntity<Guid>> sourceRecords, DatabaseContext context)
        {
            IList<TradeInvestment> toCreate = new List<TradeInvestment>();
            var query = GetQuery(context).ToList();

            var tiChangeIncidents = new List<TradeInvestment>();

            var settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
            var statusesSetting = settingsManager.GetSetting<string>("NOT_CHECK_PROMO_STATUS_LIST", "Draft,Cancelled,Deleted,Closed");
            var notCheckPromoStatuses = statusesSetting.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            var promoes = context.Set<Promo>().Where(x => !x.Disabled && x.StartDate.HasValue && x.StartDate.Value.Year == this.Year)
                .Select(x => new PromoSimpleTI
                {
                    PromoStatusName = x.PromoStatus.Name,
                    Number = x.Number,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    ClientTreeId = x.ClientTree.Id,
                    ClientTreeObjectId = x.ClientTreeId,
                    BrandTechId = x.BrandTechId
                })
                .ToList()
                .Where(x => !notCheckPromoStatuses.Contains(x.PromoStatusName));

            var importTIes = sourceRecords.Cast<ImportTradeInvestment>().Where(x => x.StartDate.HasValue && x.EndDate.HasValue);
            var allTIForCurrentYear = context.Set<TradeInvestment>().Where(x => !x.Disabled && x.Year == this.Year);
            var clientTrees = context.Set<ClientTree>().ToList();
            var brandTeches = context.Set<BrandTech>().ToList();

            foreach (var promo in promoes)
            {
                if (!importTIes.Any(x => x.ClientTreeId == promo.ClientTreeId && (x.BrandTechId == null || x.BrandTechId == promo.BrandTechId) && x.StartDate <= promo.StartDate && x.EndDate >= promo.StartDate))
                {
                    bool existTradeInvestment = false;

                    var clientNode = clientTrees.Where(x => x.ObjectId == promo.ClientTreeObjectId && !x.EndDate.HasValue).FirstOrDefault();
                    while (!existTradeInvestment && clientNode != null && clientNode.Type != "root")
                    {
                        //промо может быть привязно к удаленному брендтеху(в более общем случае - к бредтеху с другим Id), поэтому сравнение приходится производить по Name, а не по Id
                        var promoBrandTechName = brandTeches.Where(bt => bt.Id == promo.BrandTechId).Select(x => x.BrandsegTechsub).FirstOrDefault();
                        var validBrandTeches = context.Set<BrandTech>().Where(x => x.BrandsegTechsub == promoBrandTechName);

                        existTradeInvestment = importTIes.Any(x => x.ClientTreeId == clientNode.Id 
                                && (x.BrandTechId == null || validBrandTeches.Where(bt => bt.Id == x.BrandTechId).Any()) 
                                && x.StartDate <= promo.StartDate 
                                && x.EndDate >= promo.StartDate);
                        clientNode = clientTrees.Where(x => x.ObjectId == clientNode.parentId && !x.EndDate.HasValue).FirstOrDefault();
                    }

                    if (!existTradeInvestment)
                    {
                        HasErrors = true;
                        Errors.Add($"Not found interval for promo number {promo.Number}.");
                    }
                }
            }

            if (!HasErrors)
            {
                foreach (var importObj in allTIForCurrentYear)
                {
                    importObj.Disabled = true;
                    importObj.DeletedDate = DateTimeOffset.Now;
                }

                foreach (ImportTradeInvestment newRecord in sourceRecords)
                {
                    TradeInvestment oldRecord = query.FirstOrDefault(x => x.ClientTree?.ObjectId == newRecord.ClientTreeObjectId && !x.Disabled);
                    BrandTech bt = context.Set<BrandTech>().FirstOrDefault(x => x.BrandsegTechsub == newRecord.BrandsegTechsub && !x.Disabled);
                    TradeInvestment toSave = new TradeInvestment()
                    {
                        StartDate = newRecord.StartDate,
                        EndDate = newRecord.EndDate,
                        SizePercent = (float)Math.Round((decimal)newRecord.SizePercent, 2, MidpointRounding.AwayFromZero),
                        TISubType = newRecord.TISubType,
                        TIType = newRecord.TIType,
                        ClientTreeId = newRecord.ClientTreeId,
                        MarcCalcROI = newRecord.MarcCalcROIBool,
                        MarcCalcBudgets = newRecord.MarcCalcBudgetsBool,
                        BrandTechId = bt != null ? (Guid?)bt.Id : null,
                        Year = newRecord.StartDate.Value.Year
                    };
                    toCreate.Add(toSave);
                    tiChangeIncidents.Add(toSave);
                }

                foreach (IEnumerable<TradeInvestment> importObjs in toCreate.Partition(100))
                {
                    context.Set<TradeInvestment>().AddRange(importObjs);
                }

                // Необходимо выполнить перед созданием инцидентов.
                context.SaveChanges();

                foreach (var tradeInvestment in tiChangeIncidents)
                {
                    var currentTI = allTIForCurrentYear.FirstOrDefault(x => x.ClientTreeId == tradeInvestment.ClientTreeId && x.BrandTechId == tradeInvestment.BrandTechId && x.TIType == tradeInvestment.TIType && x.TISubType == tradeInvestment.TISubType && x.StartDate == tradeInvestment.StartDate && x.EndDate == tradeInvestment.EndDate && !x.Disabled);
                    if (currentTI != null)
                    {
                        context.Set<ChangesIncident>().Add(new ChangesIncident
                        {
                            Id = Guid.NewGuid(),
                            DirectoryName = nameof(TradeInvestment),
                            ItemId = currentTI.Id.ToString(),
                            CreateDate = DateTimeOffset.Now,
                            Disabled = false
                        });
                    }
                }
            }

            context.SaveChanges();
            return sourceRecords.Count();
        }

        /// <summary>
        /// Запись в базу аналогично изменению TradeInvestment из интерфейса через контекст
        /// </summary>
        /// <param name="sourceRecords"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected int InsertActualTIDataToDatabase(IEnumerable<IEntity<Guid>> sourceRecords, DatabaseContext context)
        {
            IList<ActualTradeInvestment> toCreate = new List<ActualTradeInvestment>();

            var tiChangeIncidents = new List<ActualTradeInvestment>();

            var settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
            var statusesSetting = settingsManager.GetSetting<string>("ACTUAL_COGSTI_CHECK_PROMO_STATUS_LIST", "Finished, Closed");
            var checkPromoStatuses = statusesSetting.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            var promoes = context.Set<Promo>().Where(x => !x.Disabled && x.StartDate.HasValue && x.StartDate.Value.Year == this.Year)
                .Select(x => new PromoSimpleTI
                {
                    PromoStatusName = x.PromoStatus.Name,
                    Number = x.Number,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    ClientTreeId = x.ClientTree.Id,
                    ClientTreeObjectId = x.ClientTreeId,
                    BrandTechId = x.BrandTechId
                })
                .ToList()
                .Where(x => checkPromoStatuses.Contains(x.PromoStatusName));

            var importTIes = sourceRecords.Cast<ImportTradeInvestment>().Where(x => x.StartDate.HasValue && x.EndDate.HasValue);
            var allActualTIForYear = context.Set<ActualTradeInvestment>().Where(x => !x.Disabled && x.Year == this.Year);
            var clientTrees = context.Set<ClientTree>().ToList();
            var brandTeches = context.Set<BrandTech>().ToList();

            foreach (var promo in promoes)
            {
                if (!importTIes.Any(x => x.ClientTreeId == promo.ClientTreeId && (x.BrandTechId == null || x.BrandTechId == promo.BrandTechId) && x.StartDate <= promo.StartDate && x.EndDate >= promo.StartDate))
                {
                    bool existTradeInvestment = false;

                    var clientNode = clientTrees.Where(x => x.ObjectId == promo.ClientTreeObjectId && !x.EndDate.HasValue).FirstOrDefault();
                    while (!existTradeInvestment && clientNode != null && clientNode.Type != "root")
                    {
                        //промо может быть привязно к удаленному брендтеху(в более общем случае - к бредтеху с другим Id), поэтому сравнение приходится производить по Name, а не по Id
                        var promoBrandTechName = brandTeches.Where(bt => bt.Id == promo.BrandTechId).Select(x => x.BrandsegTechsub).FirstOrDefault();
                        var validBrandTeches = context.Set<BrandTech>().Where(x => x.BrandsegTechsub == promoBrandTechName);

                        existTradeInvestment = importTIes.Any(x => x.ClientTreeId == clientNode.Id
                                && (x.BrandTechId == null || validBrandTeches.Where(bt => bt.Id == x.BrandTechId).Any())
                                && x.StartDate <= promo.StartDate
                                && x.EndDate >= promo.StartDate);
                        clientNode = clientTrees.Where(x => x.ObjectId == clientNode.parentId && !x.EndDate.HasValue).FirstOrDefault();
                    }

                    if (!existTradeInvestment)
                    {
                        HasErrors = true;
                        Errors.Add($"Not found interval for promo number {promo.Number}.");
                    }
                }
            }

            if (!HasErrors)
            {
                foreach (var importObj in allActualTIForYear)
                {
                    importObj.Disabled = true;
                    importObj.DeletedDate = DateTimeOffset.Now;
                }

                foreach (ImportTradeInvestment newRecord in sourceRecords)
                {
                    BrandTech bt = context.Set<BrandTech>().FirstOrDefault(x => x.BrandsegTechsub == newRecord.BrandsegTechsub && !x.Disabled);
                    ActualTradeInvestment toSave = new ActualTradeInvestment()
                    {
                        StartDate = newRecord.StartDate,
                        EndDate = newRecord.EndDate,
                        SizePercent = (float)Math.Round((decimal)newRecord.SizePercent, 2, MidpointRounding.AwayFromZero),
                        TISubType = newRecord.TISubType,
                        TIType = newRecord.TIType,
                        ClientTreeId = newRecord.ClientTreeId,
                        MarcCalcROI = newRecord.MarcCalcROIBool,
                        MarcCalcBudgets = newRecord.MarcCalcBudgetsBool,
                        BrandTechId = bt != null ? (Guid?)bt.Id : null,
                        Year = newRecord.StartDate.Value.Year,
                        IsTIIncidentCreated = false
                    };
                    toCreate.Add(toSave);
                    tiChangeIncidents.Add(toSave);
                }

                foreach (IEnumerable<ActualTradeInvestment> importObjs in toCreate.Partition(100))
                {
                    context.Set<ActualTradeInvestment>().AddRange(importObjs);
                }

                //инциденты при импорте ActualTI не создаются
            }

            context.SaveChanges();
            return sourceRecords.Count();
        }

        protected virtual IEnumerable<IEntity<Guid>> BeforeInsert(IEnumerable<IEntity<Guid>> records, DatabaseContext context)
        {
            return records;
        }

        private IEnumerable<TradeInvestment> GetQuery(DatabaseContext context)
        {
            IQueryable<TradeInvestment> query = context.Set<TradeInvestment>().AsNoTracking();
            return query.ToList();
        }
    }


    class PromoSimpleTI
    {
        public int? Number { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public int? ClientTreeId { get; set; }
        public int? ClientTreeObjectId { get; set; }
        public Guid? BrandTechId { get; set; }
        public string PromoStatusName { get; set; }
    }
}