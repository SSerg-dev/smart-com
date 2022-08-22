using Core.Data;
using Core.Extensions;
using Core.History;
using Core.Settings;
using Interfaces.Implementation.Action;
using Interfaces.Implementation.Import.FullImport;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Host.TPM.Util;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using NLog;
using Persist;
using Persist.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utility;
using Utility.FileWorker;
using Utility.Import;

namespace Module.Host.TPM.Actions
{
    class FullXLSXCompetitorPromoUpdateNewImportAction : BaseAction
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

        public FullXLSXCompetitorPromoUpdateNewImportAction(FullImportSettings settings)
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
            var fileDispatcher = new FileDispatcher();
            string importDir = AppSettingsManager.GetSetting<string>("IMPORT_DIRECTORY", "ImportFiles");
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
                var successList = new ConcurrentBag<IEntity<Guid>>();
                var errorRecords = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();
                var warningRecords = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();

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
                PromoTypes promoTypes = context.Set<PromoTypes>().FirstOrDefault(g => !g.Disabled && g.SystemName == "Regular");
                List<string> mechanics = context.Set<Mechanic>().Where(g => !g.Disabled && g.PromoTypesId == promoTypes.Id).Select(g => g.Name).ToList();
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
                    else if (!IsFilterSuitable(ref rec, context, out validationErrors, existingClientTreeIds, mechanics))
                    {
                        HasErrors = true;
                        errorRecords.Add(new Tuple<IEntity<Guid>, string>(item, string.Join(", ", validationErrors)));
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

        private bool IsFilterSuitable(ref IEntity<Guid> rec, DatabaseContext context, out IList<string> errors, List<ClientTree> existingClientTreeIds, List<string> mechanics)
        {
            errors = new List<string>();
            bool isSuitable = true;

            if (rec == null)
            {
                isSuitable = false;
                errors.Add("There is no such Competitor promo on base");
            }
            else
            {
                CompetitorPromo typedRec = (CompetitorPromo)rec;
                typedRec.ClientTree = context.Set<ClientTree>().First(x => x.ObjectId == typedRec.ClientTreeObjectId && x.EndDate == null);
                if (typedRec.CompetitorBrandTech == null)
                {
                    errors.Add("Competitor BrandTech not found");
                    isSuitable = false;
                }
                if (!context.Set<CompetitorBrandTech>().Any(x => !x.Disabled && x.CompetitorId == typedRec.CompetitorId && x.BrandTech == typedRec.CompetitorBrandTech.BrandTech))
                {
                    errors.Add("Competitor BrandTech not found");
                    isSuitable = false;
                }
                if (typedRec.ClientTreeObjectId == null)
                {
                    errors.Add("Client must have a value");
                    isSuitable = false;
                }
                if (!typedRec.ClientTree.IsBaseClient)
                {
                    errors.Add($"{typedRec.ClientTreeObjectId} is not a base client");
                    isSuitable = false;
                }
                if (!existingClientTreeIds.Where(x => x.EndDate == null).Any(x => x.ObjectId == typedRec.ClientTreeObjectId))
                {
                    errors.Add($"No access to import data for {typedRec.ClientTreeObjectId}");
                    isSuitable = false;
                }
                if (typedRec.StartDate == null)
                {
                    errors.Add("StartDate must have a value");
                    isSuitable = false;
                }
                if (typedRec.EndDate == null)
                {
                    errors.Add("EndDate must have a value");
                    isSuitable = false;
                }
                if (typedRec.StartDate > typedRec.EndDate)
                {
                    errors.Add("Invalid period");
                    isSuitable = false;
                }
                if (typedRec.CompetitorId == null)
                {
                    isSuitable = false;
                    errors.Add("Competitor must have value");
                }
                else if (typedRec.Competitor == null)
                {
                    errors.Add("Competitor not found");
                    isSuitable = false;
                }
                if (typedRec.Price == null || typedRec.Price < 0)
                {
                    errors.Add("Invalid price");
                    isSuitable = false;
                }
                if (String.IsNullOrEmpty(typedRec.MechanicType))
                {
                    errors.Add("Mechanic Type must have a value");
                    isSuitable = false;
                }
                else if (!mechanics.Contains(typedRec.MechanicType))
                {
                    errors.Add("Mechanic Type must be - " + String.Join(", ", mechanics.ToArray()));
                    isSuitable = false;
                }
                if (typedRec.Discount == null || (typedRec.Discount < 0 || typedRec.Discount > 100))
                {
                    errors.Add("Invalid discount");
                    isSuitable = false;
                }

                if (typedRec.Number != 0)
                {
                    CompetitorPromo recordFromDB = context.Set<CompetitorPromo>()
                    .FirstOrDefault(t
                        => (t.Number == typedRec.Number && !t.Disabled));
                    if (recordFromDB == null)
                    {
                        errors.Add($"Promo {typedRec.Number} not found");
                        isSuitable = false;
                    }
                }
            }

            return isSuitable;
        }

        private int InsertDataToDatabase(IEnumerable<IEntity<Guid>> sourceRecords, DatabaseContext context)
        {
            NoGuidGeneratingScriptGenerator generatorCreate = new NoGuidGeneratingScriptGenerator(typeof(CompetitorPromo), false);
            ScriptGenerator generatorUpdate = GetScriptGenerator();
            IList<CompetitorPromo> toCreate = new List<CompetitorPromo>();
            IList<CompetitorPromo> toUpdate = new List<CompetitorPromo>();
            IList<CompetitorPromo> competitorPromoes = context.Set<CompetitorPromo>().ToList();
            IList<CompetitorPromo> importItems = new List<CompetitorPromo>();

            List<Tuple<IEntity<Guid>, IEntity<Guid>>> toHisCreate = new List<Tuple<IEntity<Guid>, IEntity<Guid>>>();
            List<Tuple<IEntity<Guid>, IEntity<Guid>>> toHisUpdate = new List<Tuple<IEntity<Guid>, IEntity<Guid>>>();
            List<Tuple<IEntity<Guid>, IEntity<Guid>>> toHisDelete = new List<Tuple<IEntity<Guid>, IEntity<Guid>>>();

            foreach (CompetitorPromo newRecord in sourceRecords)
            {
                CompetitorPromo oldRecord = competitorPromoes
                    .FirstOrDefault(t
                        => (t.Number == newRecord.Number && !t.Disabled));

                string competitorBrandTech = newRecord.CompetitorBrandTech.BrandTech;
                string mechanicType = newRecord.MechanicType;
                double? discount = newRecord.Discount;
                dynamic handledDiscount = discount != 0 && discount != null ? discount + "%" : "";//к discount прибавляется знак процента
                newRecord.Name = competitorBrandTech + " " + mechanicType + " " + handledDiscount;

                if (oldRecord == null)
                {
                    newRecord.ClientTreeObjectId = context.Set<ClientTree>().First(x => x.ObjectId == newRecord.ClientTreeObjectId && x.EndDate == null).Id;
                    newRecord.Id = Guid.NewGuid();
                    toCreate.Add(newRecord);
                    toHisCreate.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(null, newRecord));
                }
                else
                {
                    oldRecord.Competitor = newRecord.Competitor;
                    oldRecord.ClientTreeObjectId = context.Set<ClientTree>().First(x => x.ObjectId == newRecord.ClientTreeObjectId && x.EndDate == null).Id;
                    oldRecord.CompetitorBrandTechId = newRecord.CompetitorBrandTechId;
                    oldRecord.Name = newRecord.Name;
                    oldRecord.Discount = newRecord.Discount;
                    oldRecord.Price = newRecord.Price;
                    oldRecord.StartDate = newRecord.StartDate;
                    oldRecord.EndDate = newRecord.EndDate;
                    oldRecord.MechanicType = newRecord.MechanicType;
                    toUpdate.Add(oldRecord);
                    toHisUpdate.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(oldRecord, newRecord));
                }
                importItems.Add(newRecord);
            }

            foreach (IEnumerable<IEntity<Guid>> items in toCreate.Partition(100))
            {
                string insertScript = generatorCreate.BuildInsertScript(items);
                context.ExecuteSqlCommand(insertScript);
            }

            foreach (IEnumerable<IEntity<Guid>> items in toUpdate.Partition(10000))
            {
                string updateScript = generatorUpdate.BuildUpdateScript(items);
                context.ExecuteSqlCommand(updateScript);
            }

            var importNumbers = importItems.Select(x => x.Number);
            var toDeletes = competitorPromoes.Where(x => !importNumbers.Contains(x.Number) && !x.Disabled);
            foreach (CompetitorPromo deleteCompetitorPromo in toDeletes)
            {
                deleteCompetitorPromo.Disabled = true;
                deleteCompetitorPromo.DeletedDate = System.DateTime.Now;
            }
            foreach (IEnumerable<IEntity<Guid>> items in toDeletes.Partition(10000))
            {
                string deleteScript = generatorUpdate.BuildDeleteScript(items);
                context.ExecuteSqlCommand(deleteScript);
            }
            foreach (var item in toDeletes)
            {
                toHisDelete.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(item, item));
            }
            context.HistoryWriter.Write(toHisCreate, context.AuthManager.GetCurrentUser(), context.AuthManager.GetCurrentRole(), OperationType.Created);
            context.HistoryWriter.Write(toHisUpdate, context.AuthManager.GetCurrentUser(), context.AuthManager.GetCurrentRole(), OperationType.Updated);
            context.HistoryWriter.Write(toHisDelete, context.AuthManager.GetCurrentUser(), context.AuthManager.GetCurrentRole(), OperationType.Deleted);

            context.SaveChanges();

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
