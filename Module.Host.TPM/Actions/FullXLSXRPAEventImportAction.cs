using Core.Data;
using Core.Extensions;
using Core.History;
using Core.Settings;
using Interfaces.Implementation.Action;
using Interfaces.Implementation.Import.FullImport;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.CalculatePromoParametersModule;
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
using System.IO;
using System.Linq;
using Utility;
using Utility.FileWorker;
using Utility.Import;

namespace Module.Host.TPM.Actions
{
    class FullXLSXRPAEventImportAction : BaseAction
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


        private bool AllowPartialApply { get; set; }
        private readonly Logger logger;
        private string ResultStatus { get; set; }
        private bool HasErrors { get; set; }

        private ScriptGenerator Generator { get; set; }

        public FullXLSXRPAEventImportAction(FullImportSettings settings, Guid rpaId)
        {
            UserId = settings.UserId;
            RoleId = settings.RoleId;
            ImportFile = settings.ImportFile;
            ImportType = settings.ImportType;
            ModelType = settings.ModelType;
            Separator = settings.Separator;
            Quote = settings.Quote;
            HasHeader = settings.HasHeader;

            RPAId = rpaId;

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

                IList<IEntity<Guid>> sourceRecordWithoutDuplicates = new List<IEntity<Guid>>();
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


                CheckForDuplicates(context, sourceRecords, out sourceRecordWithoutDuplicates, out validationErrors);

                foreach (var item in sourceRecordWithoutDuplicates)
                {
                    IEntity<Guid> rec;
                    IList<string> warnings;

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

        private bool IsFilterSuitable(ref IEntity<Guid> rec, DatabaseContext context, out IList<string> errors, List<ClientTree> existingClientTreeIds)
        {
            errors = new List<string>();
            bool isSuitable = true;

            ImportRPAEvent typedRec = (ImportRPAEvent)rec;
            var promoEvent = context.Set<Event>().FirstOrDefault(x => !x.Disabled && x.Name == typedRec.EventName);
            if (promoEvent == null)
            {
                errors.Add("Event not found");
                isSuitable = false;
            }
            else
            {
                var promo = context.Set<Promo>().FirstOrDefault(x => !x.Disabled && x.Number == typedRec.PromoNumber);
                if (promo == null)
                {
                    errors.Add("Promo not found");
                    isSuitable = false;
                }
                if (!existingClientTreeIds.Any(x => x.ObjectId == promo.ClientTreeId))
                {
                    errors.Add("Client is not available");
                    isSuitable = false;
                }
                var allowStatuses = new List<String> { "Draft", "DraftPublished", "OnApproval", "Planned" };
                if (!allowStatuses.Contains(promo.PromoStatus.SystemName))
                {
                    errors.Add("Invalid Promo status");
                    isSuitable = false;
                }
                //var isSegmentSuitable = context.Set<PromoProduct>().Where(x => !x.Disabled && x.PromoId == promo.Id).All(x => x.Product.MarketSegment == promoEvent.MarketSegment);
                //if (isSegmentSuitable)
                //{
                //    errors.Add("Event is not suitable");
                //    isSuitable = false;
                //}
            }
            return isSuitable;
        }

        private void CheckForDuplicates(DatabaseContext context, IList<IEntity<Guid>> templateRecordIds, out IList<IEntity<Guid>> distinctRecordIds, out IList<string> errors)
        {
            distinctRecordIds = new List<IEntity<Guid>>();
            errors = new List<string>();

            var sourceTemplateRecords = templateRecordIds
                .Select(sr => (sr as ImportRPAEvent));

            bool isDuplicateRecords = sourceTemplateRecords
                .GroupBy(jps => new
                {
                    jps.PromoNumber,
                    jps.EventName
                })
                .Any(gr => gr.Count() > 1);

            if (isDuplicateRecords)
            {
                errors.Add("The Promo - Event pair occurs more than once");
            }

            distinctRecordIds = (IList<IEntity<Guid>>)sourceTemplateRecords
                .GroupBy(jps => new
                {
                    jps.PromoNumber,
                    jps.EventName
                })
                .Select(jps => jps.First())
                .ToList();

        }

        private int InsertDataToDatabase(IEnumerable<IEntity<Guid>> sourceRecords, DatabaseContext context)
        {
            foreach (ImportRPAEvent newRecord in sourceRecords)
            {
                var promo = context.Set<Promo>().FirstOrDefault(x => x.Number == newRecord.PromoNumber);
                var eventId = context.Set<Event>().FirstOrDefault(x => x.Name == newRecord.EventName)?.Id;
                //var btlPromo = context.Set<BTLPromo>().FirstOrDefault(x => x.PromoId == promo.Id);
                //if (btlPromo != null && promo.EventId != eventId)
                //{
                //    btlPromo.DeletedDate = System.DateTime.Now;
                //    btlPromo.Disabled = true;
                //    CalculateBTLBudgetsCreateTask(btlPromo.BTLId.ToString(), new List<Guid>() { promo.Id }, context);
                //}
                promo.EventId = eventId;
            }

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

        public void CalculateBTLBudgetsCreateTask(string btlId, List<Guid> unlinkedPromoIds, DatabaseContext context)
        {
            HandlerData data = new HandlerData();
            HandlerDataHelper.SaveIncomingArgument("BTLId", btlId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", UserId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", RoleId, data, visible: false, throwIfNotExists: false);
            if (unlinkedPromoIds != null)
            {
                HandlerDataHelper.SaveIncomingArgument("UnlinkedPromoIds", unlinkedPromoIds, data, visible: false, throwIfNotExists: false);
            }

            bool success = CalculationTaskManager.CreateCalculationTask(CalculationTaskManager.CalculationAction.BTL, data, context);

            if (!success)
                throw new Exception("Promo was blocked for calculation");
        }
    }
}
