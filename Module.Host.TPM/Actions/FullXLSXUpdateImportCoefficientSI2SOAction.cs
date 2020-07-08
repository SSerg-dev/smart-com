using Core.Data;
using Core.Extensions;
using Core.History;
using Core.MarsCalendar;
using Core.Settings;
using Interfaces.Implementation.Action;
using Interfaces.Implementation.Import.FullImport;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using NLog;
using Persist;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utility.Import;
using Utility.LogWriter;

namespace Module.Host.TPM.Actions
{
    class FullXLSXUpdateImportCoefficientSI2SOAction : BaseAction
    {
        private readonly Guid UserId;
        private readonly Guid RoleId;
        private readonly FileModel ImportFile;
        private readonly Type ImportType;
        private readonly Type ModelType;
        private readonly string Separator;
        private readonly string Quote;
        private readonly bool HasHeader;


        private bool AllowPartialApply { get; set; }
        private readonly Logger logger;
        private string ResultStatus { get; set; }
        private bool HasErrors { get; set; }

        private ScriptGenerator Generator { get; set; }

        public FullXLSXUpdateImportCoefficientSI2SOAction(FullImportSettings settings, Guid handlerId)
        {
            UserId = settings.UserId;
            RoleId = settings.RoleId;
            ImportFile = settings.ImportFile;
            ImportType = settings.ImportType;
            ModelType = settings.ModelType;
            Separator = settings.Separator;
            Quote = settings.Quote;
            HasHeader = settings.HasHeader;
           
            AllowPartialApply = false;
            logger = LogManager.GetCurrentClassLogger();
        }

        public override void Execute()
        {
            logger.Trace("Begin");
            try
            {
                ResultStatus = null;
                HasErrors = false;

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
            var importDir = AppSettingsManager.GetSetting<string>("IMPORT_DIRECTORY", "ImportFiles");
            var importFilePath = Path.Combine(importDir, ImportFile.Name);

            if (!File.Exists(importFilePath))
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
            
            IList<IEntity<Guid>> records = ImportUtility.ParseXLSXFile(importFilePath, null, builder, validator, Separator, Quote, HasHeader, out sourceRecordCount, out errors, out buildErrors, out validateErrors);

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
                var successList = new ConcurrentBag<IEntity<Guid>>();
                var errorRecords = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();
                var warningRecords = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();

                // Получить функцию Validate
                var validator = ImportModelFactory.GetImportValidator(ImportType);
                // Получить функцию SetProperty
                var builder = ImportModelFactory.GetModelBuilder(ImportType, ModelType);


                var cacheBuilder = ImportModelFactory.GetImportCacheBuilder(ImportType);
                var cache = cacheBuilder.Build(sourceRecords, context);

                foreach (var item in sourceRecords)
                {
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
                    else if (!IsFilterSuitable(ref rec, out validationErrors))
                    {
                        HasErrors = true;
                        validationErrors
                            .ToList()
                            .ForEach(i 
                                => errorRecords.Add(new Tuple<IEntity<Guid>, string>(item, i)));
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
                }

                logger.Trace("Persist models built");

        
                int resultRecordCount = 0;

                ResultStatus = GetImportStatus();
                var importModel = ImportUtility.BuildActiveImport(UserId, RoleId, ImportType);
                importModel.Status = ResultStatus;
                context.Imports.Add(importModel);

                bool hasSuccessList = AllowPartialApply || !HasErrors;
                if (hasSuccessList)
                {
                    // Закончить импорт
                    resultRecordCount = InsertDataToDatabase(records, context);
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

        private bool IsFilterSuitable(ref IEntity<Guid> rec, out IList<string> errors)
        {
            errors = new List<string>();
            bool isSuitable = true;

            if (rec == null)
            {
                isSuitable = false;
                errors.Add("There is no such CoefficientSI2SO on base");
            }
            else
            {
                var coef = (CoefficientSI2SO)rec;

                if (coef.CoefficientValue <= 0)
                {
                    errors.Add("CoefficientValue must have a positive value");
                    isSuitable = false;
                }
                if (coef.DemandCode == string.Empty)
                {
                    errors.Add("DemandCode must have a value");
                    isSuitable = false;
                }
                if (coef.BrandTechId == Guid.Empty)
                {
                    errors.Add("BrandtechCode and BrandtechName must have a value");
                    isSuitable = false;
                }

                coef.CoefficientValue = Math.Round(coef.CoefficientValue.Value, 2, MidpointRounding.AwayFromZero);

                rec = coef;
            }

            return isSuitable;
        }

        private int InsertDataToDatabase(IEnumerable<IEntity<Guid>> sourceRecords, DatabaseContext context)
        {
            NoGuidGeneratingScriptGenerator generatorCreate = new NoGuidGeneratingScriptGenerator(typeof(CoefficientSI2SO), false);
            ScriptGenerator generatorUpdate = GetScriptGenerator();
            IList<CoefficientSI2SO> toCreate = new List<CoefficientSI2SO>();
            IList<CoefficientSI2SO> toUpdate = new List<CoefficientSI2SO>();
            IList<CoefficientSI2SO> coefficientSI2SOs = context.Set<CoefficientSI2SO>().ToList();

            List<Tuple<IEntity<Guid>, IEntity<Guid>>> toHisCreate = new List<Tuple<IEntity<Guid>, IEntity<Guid>>>();
            List<Tuple<IEntity<Guid>, IEntity<Guid>>> toHisUpdate = new List<Tuple<IEntity<Guid>, IEntity<Guid>>>();

            foreach (CoefficientSI2SO newRecord in sourceRecords)
            {
                CoefficientSI2SO oldRecord = coefficientSI2SOs
                    .FirstOrDefault(c 
                        =>  c.DemandCode == newRecord.DemandCode
                            && c.BrandTechId == newRecord.BrandTechId
                            && !c.Disabled);

                if (oldRecord == null)
                {
                    newRecord.NeedProcessing = true;
                    newRecord.Id = Guid.NewGuid();
                    newRecord.Lock = true;

                    toCreate.Add(newRecord);
                    toHisCreate.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(null, newRecord));
                }
                else if (oldRecord.CoefficientValue != newRecord.CoefficientValue)
                {
                    oldRecord.NeedProcessing = true;
                    oldRecord.CoefficientValue = newRecord.CoefficientValue;

                    toUpdate.Add(oldRecord);
                    toHisUpdate.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(oldRecord, newRecord));
                }
            }

            foreach (IEnumerable<IEntity<Guid>> items in toCreate.Partition(100))
            {
                string insertScript = generatorCreate.BuildInsertScript(items);
                context.Database.ExecuteSqlCommand(insertScript);
            }

            foreach (IEnumerable<IEntity<Guid>> items in toUpdate.Partition(10000))
            {
                string updateScript = generatorUpdate.BuildUpdateScript(items);
                context.Database.ExecuteSqlCommand(updateScript);
            }

            context.HistoryWriter.Write(toHisCreate, context.AuthManager.GetCurrentUser(), context.AuthManager.GetCurrentRole(), OperationType.Created);
            context.HistoryWriter.Write(toHisUpdate, context.AuthManager.GetCurrentUser(), context.AuthManager.GetCurrentRole(), OperationType.Updated);

            return sourceRecords.Count();
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

        private ScriptGenerator GetScriptGenerator()
        {
            if (Generator == null)
            {
                Generator = new ScriptGenerator(ModelType);
            }
            return Generator;
        }
    }
}
