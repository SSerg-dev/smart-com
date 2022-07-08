using Core.Data;
using Core.Extensions;
using Interfaces.Implementation.Action;
using Interfaces.Implementation.Import.FullImport;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Host.TPM.Util;
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
using System.Threading.Tasks;
using Utility;
using Utility.FileWorker;
using Utility.Import;

namespace Module.Host.TPM.Actions
{
    public class FullXLSXRPANonPromoSupportImportAction : BaseAction
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

        private ConcurrentBag<IEntity<Guid>>  successList = new ConcurrentBag<IEntity<Guid>>();
        private ConcurrentBag<Tuple<IEntity<Guid>, string>> errorRecords = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();
        private ConcurrentBag<Tuple<IEntity<Guid>, string>> warningRecords = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();

        private bool AllowPartialApply { get; set; }
        private readonly Logger logger;
        private string ResultStatus { get; set; }
        private bool HasErrors { get; set; }

        private ScriptGenerator Generator { get; set; }

        public FullXLSXRPANonPromoSupportImportAction(FullImportSettings settings, Guid RPAId)
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

            string targetAttachFileDir = Core.Settings.AppSettingsManager.GetSetting("PROMO_SUPPORT_DIRECTORY", "PromoSupportFiles");
            string targetAttachFilePath = Path.Combine(targetAttachFileDir, ImportFile.Name);
            File.Copy(importFilePath, targetAttachFilePath);
            fileDispatcher.UploadToBlob(ImportFile.Name, targetAttachFilePath, targetAttachFilePath.Split('\\').Last());

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
                foreach(var item in sourceRecordWithoutDuplicates)
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
                    List<Guid> promoesForBudgetCalculation = new List<Guid>();
                    // Закончить импорт
                    resultRecordCount = InsertDataToDatabase(records, context, out promoesForBudgetCalculation);
                    CalculateBudgetsCreateTask(context, promoesForBudgetCalculation);
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
                .Select(sr=> (sr as ImportRPANonPromoSupport));            
            
            var shortSupports = context.Set<NonPromoSupport>()
                .Select(ps => new
                {
                    ps.Id,
                    ps.Disabled,
                    ps.ClientTreeId,
                    ps.Number,
                    ps.ActualQuantity
                });

            var joinPromoSupports = sourceTemplateRecords
                .Join(shortSupports,
                        str => str.NonPromoSupportNumber,
                        ssp => ssp.Number,
                        (str, ssp) => new
                        {
                            NonPromoSupportNumber = str.NonPromoSupportNumber,
                            ExternalCode = str.ExternalCode,
                            Quantity = str.Quantity,
                            NonPromoSupportId = ssp.Id,
                            Disabled = ssp.Disabled,
                            ClietTreeId = ssp.ClientTreeId,
                            ActualQuantuty = ssp.ActualQuantity
                        });
            bool isDuplicateRecords = joinPromoSupports
                .GroupBy(jps => new
                {
                    jps.NonPromoSupportNumber,
                    jps.ExternalCode
                })
                .Any(gr => gr.Count() > 1);

            if(isDuplicateRecords)
            {
                errors.Add("The ExternalCode pair occurs more than once");
            }

            var promoSupportWithoutDuplicates = joinPromoSupports
                .GroupBy(jps => new
                {
                    jps.NonPromoSupportNumber,
                    jps.ExternalCode,
                    jps.NonPromoSupportId
                })
                .Select(jps => jps.First())
                .ToList();

            distinctRecordIds = promoSupportWithoutDuplicates
                .Select(p => new ImportRPANonPromoSupport
                {
                    NonPromoSupportNumber = p.NonPromoSupportNumber,
                    ExternalCode = p.ExternalCode,
                    Quantity = p.Quantity,
                    NonPromoSupportId = p.NonPromoSupportId
                } as IEntity<Guid>)
                .ToList();
            
            foreach(ImportRPAPromoSupport item in distinctRecordIds)
            {
                if (item.Quantity <= 0)
                {
                    errors.Add(String.Format("Quantity in promo {0} should be a positive integer", item.PromoSupportId));
                    distinctRecordIds.Remove(item);
                }
                if (String.IsNullOrEmpty(item.ExternalCode))
                {
                    errors.Add(String.Format("External code {0} should not be empty in promo {1}", item.ExternalCode, item.PromoSupportId));
                    distinctRecordIds.Remove(item);
                }
            }

        }

        private bool IsFilterSuitable(ref IEntity<Guid> rec, DatabaseContext context, out IList<string> errors, List<ClientTree> existingClientTreeIds)
        {
            errors = new List<string>();
            bool isSuitable = true;

            ImportRPAPromoSupport typedRec = (ImportRPAPromoSupport)rec;            

            var promoSupport = context.Set<PromoSupport>().FirstOrDefault(x => x.Number == typedRec.PromoSupportNumber && !x.Disabled);
            if (promoSupport == null)
            {
                errors.Add("PromoSupport not found or deleted");
                isSuitable = false;
                return isSuitable;
            }
            if (!existingClientTreeIds.Any(x => x.ObjectId == promoSupport.ClientTree.ObjectId))
            {
                errors.Add("No access to the client");
                isSuitable = false;
            }
            return isSuitable;
        }

        private int InsertDataToDatabase(IEnumerable<IEntity<Guid>> sourceRecords, DatabaseContext context, out List<Guid> promoesForBudgetCalculation)
        {


            var sourcePromoSupport = sourceRecords
                 .Select(sr => sr as ImportRPANonPromoSupport)
                 .ToList();

            var sourcePromoSupportIds = sourcePromoSupport
                .Distinct()
                .Select(ps => ps.NonPromoSupportId)
                .ToList();

            promoesForBudgetCalculation = sourcePromoSupportIds;

            var toRemove = context.Set<NonPromoSupportDMP>()
                .Where(x => sourcePromoSupportIds.Contains(x.NonPromoSupportId.Value));

            IList<NonPromoSupportDMP> toCreate = new List<NonPromoSupportDMP>();
                                   
            foreach (ImportRPANonPromoSupport newRecord in sourceRecords)
            {
                NonPromoSupportDMP toSave = new NonPromoSupportDMP()
                {
                    NonPromoSupportId = newRecord.NonPromoSupportId,
                    ExternalCode = newRecord.ExternalCode,
                    Quantity = newRecord.Quantity
                };
                toCreate.Add(toSave);
            }

            foreach (IEnumerable<NonPromoSupportDMP> items in toRemove.Partition(100))
            {
                context.Set<NonPromoSupportDMP>().RemoveRange(items);
            }           
            
            foreach (IEnumerable<NonPromoSupportDMP> items in toCreate.Partition(100))
            {
                context.Set<NonPromoSupportDMP>().AddRange(items);
            }

            var aggQuantitySumRecords = sourcePromoSupport
                .GroupBy(sr => sr.NonPromoSupportNumber)
                .Select(pr => new ImportRPANonPromoSupport
                {
                            NonPromoSupportNumber = pr.Key,
                            Quantity = pr.Sum(x => x.Quantity)
                            
                        } as IEntity<Guid>
                 );

            foreach(ImportRPANonPromoSupport newRecord in aggQuantitySumRecords)
            {
                var promoSupport = context.Set<NonPromoSupport>().FirstOrDefault(x =>x.Number == newRecord.NonPromoSupportNumber);

                promoSupport.ActualQuantity = newRecord.Quantity;                
                promoSupport.AttachFileName = ImportFile.Name;

            }            
            context.SaveChanges();            

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

        private void CalculateBudgetsCreateTask(DatabaseContext context, List<Guid> nonPromoSupportIds, List<Guid> unlinkedPromoIds = null)
        {
            string nonPromoSupportIdsString = FromListToString(nonPromoSupportIds);
            string unlinkedPromoIdsString = FromListToString(unlinkedPromoIds);

            HandlerData data = new HandlerData();
            HandlerDataHelper.SaveIncomingArgument("PromoSupportIds", nonPromoSupportIdsString, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UnlinkedPromoIds", unlinkedPromoIdsString, data, visible: false, throwIfNotExists: false);

            bool success = CalculationTaskManager.CreateCalculationTask(CalculationTaskManager.CalculationAction.Budgets, data, context);

            if (!success)
                throw new Exception("Promo was blocked for calculation");
        }

        private string FromListToString(List<Guid> list)
        {
            string result = "";

            if (list != null)
                foreach (Guid el in list.Distinct())
                    result += el + ";";

            return result;
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
    }
}
