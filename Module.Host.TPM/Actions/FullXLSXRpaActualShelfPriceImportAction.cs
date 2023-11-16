using Core.Data;
using Core.Extensions;
using Interfaces.Implementation.Action;
using Interfaces.Implementation.Import.FullImport;
using Looper.Parameters;
using Module.Host.TPM.Util;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.Interfaces;
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
using System.Text;
using System.Text.RegularExpressions;
using Utility;
using Utility.FileWorker;
using Utility.Import;

namespace Module.Host.TPM.Actions
{
    public class FullXLSXRpaActualShelfPriceImportAction : BaseAction
    {
        private readonly Guid UserId;
        private readonly Guid RoleId;
        private readonly Guid RPAId;
        private readonly Guid HandlerId;
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

        public FullXLSXRpaActualShelfPriceImportAction(FullImportSettings settings, Guid rPAId, Guid handlerId)
        {
            UserId = settings.UserId;
            RoleId = settings.RoleId;
            ImportFile = settings.ImportFile;
            ImportType = settings.ImportType;
            ModelType = settings.ModelType;
            Separator = settings.Separator;
            Quote = settings.Quote;
            HasHeader = settings.HasHeader;
            RPAId = rPAId;
            HandlerId = handlerId;

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

                string rpaStatus = "In progress";
                using (DatabaseContext context = new DatabaseContext())
                {
                    RPA rpa = context.Set<RPA>().FirstOrDefault(x => x.Id == RPAId);
                    rpa.Status = rpaStatus;
                    context.SaveChanges();
                }

                IList<IEntity<Guid>> sourceRecords = ParseImportFile();

                int successCount;
                int warningCount;
                int errorCount;

                ImportResultFilesModel resultFilesModel = ApplyImport(sourceRecords, out successCount, out warningCount, out errorCount);

                // Сохранить выходные параметры
                Results["ImportSourceRecordCount"] = sourceRecords.Count();
                Results["ImportResultRecordCount"] = successCount;
                Results["ErrorCount"] = errorCount;
                Results["WarningCount"] = warningCount;
                Results["ImportResultFilesModel"] = resultFilesModel;

            }
            catch (Exception e)
            {
                var rpaStatus = "Error";
                using (var context = new DatabaseContext())
                {
                    var rpa = context.Set<RPA>().FirstOrDefault(x => x.Id == RPAId);
                    rpa.Status = rpaStatus;
                    context.SaveChanges();
                }

                HasErrors = true;
                string message = string.Format("FullImportAction failed: {0}", e.ToString());
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
            FileDispatcher fileDispatcher = new FileDispatcher();
            string importDir = Core.Settings.AppSettingsManager.GetSetting("RPA_DIRECTORY", "RPAFiles");

            string importFilePath = Path.Combine(importDir, ImportFile.Name);
            if (!fileDispatcher.IsExists(importDir, ImportFile.Name))
            {
                throw new Exception("Import File not found");
            }

            var builder = ImportModelFactory.GetCSVImportModelBuilder(ImportType);
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
                // здесь должны быть все записи, а не только неудаленные!
                IQueryable<ClientTree> query = context.Set<ClientTree>().AsNoTracking();
                IQueryable<ClientTreeHierarchyView> hierarchy = context.Set<ClientTreeHierarchyView>().AsNoTracking();
                query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

                var existingClientTreeIds = query
                    .Where(x => x.EndDate == null)
                    .Select(x => x.ObjectId)
                    .ToHashSet();

                string username = context.Users.FirstOrDefault(g => g.Id == UserId).Name;
                // проверка на дубликаты из файла
                var existingDouble = CheckForDuplicatesImport(context, sourceRecords, out validationErrors);
                var existingLetters = CheckForlettersImport(existingDouble, out validationErrors);
                // Изменение промо
                List<Promo> promos = UpdatePromoesFromActualShelfPriceDiscount(context, existingLetters, existingClientTreeIds, out validationErrors);

                logger.Trace("Persist models built");

                ResultStatus = GetImportStatus();
                string rpaStatus = ResultStatus;
                RPA rpa = context.Set<RPA>().FirstOrDefault(x => x.Id == RPAId);
                rpa.Status = rpaStatus;
                var importModel = ImportUtility.BuildActiveImport(UserId, RoleId, ImportType);
                importModel.Status = ResultStatus;
                context.Imports.Add(importModel);

                bool hasSuccessList = AllowPartialApply || !HasErrors;

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

        private List<ImportActualShelfPriceDiscount> CheckForDuplicatesImport(DatabaseContext context, IList<IEntity<Guid>> sourceRecords, out IList<string> errors)
        {
            errors = new List<string>();

            List<ImportActualShelfPriceDiscount> sourceTemplateRecords = sourceRecords.Select(sr => (sr as ImportActualShelfPriceDiscount)).ToList();
            var duplicate = sourceTemplateRecords.GroupBy(x => x.PromoId).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
            sourceTemplateRecords = sourceTemplateRecords.GroupBy(x => x.PromoId).Select(y => y.First()).ToList();

            if (duplicate.Count > 0)
            {
                HasErrors = true;
                foreach (var duplicateRecord in duplicate)
                {
                    errorRecords.Add(new Tuple<IEntity<Guid>, string>(sourceTemplateRecords.FirstOrDefault(g => g.PromoId == duplicateRecord.Value), string.Join(", ", "Duplicate present in promoId:" + string.Join(",", duplicateRecord.Value))));
                }
            }

            return sourceTemplateRecords;
        }
        //Проверка на корректность заполнения InvoiceNumber и DocumentNumber
        private List<ImportActualShelfPriceDiscount> CheckForlettersImport(List<ImportActualShelfPriceDiscount> sourceRecords, out IList<string> errors)
        {
            string checkInvoiceNumber;
            string checkDocumentNumber;

            errors = new List<string>();
            var invalidNumbers = new List<string>();
            foreach (var sourceRecord in sourceRecords)
            {
                checkInvoiceNumber = (CheckExistAsnc(sourceRecord.InvoiceNumber.ToString()));
                if (checkInvoiceNumber != null && checkInvoiceNumber != "")
                {
                    HasErrors = true;
                    errors.Add("Incorrect filling Invoice Number:" + string.Join(",", checkInvoiceNumber + " Promo number :" + sourceRecord.PromoId.ToString()));
                    errorRecords.Add(new Tuple<IEntity<Guid>, string>(sourceRecords.FirstOrDefault(g => g.PromoId == sourceRecord.PromoId), string.Join(", ", errors)));
                    invalidNumbers.Add(sourceRecord.PromoId.ToString());
                }
                checkDocumentNumber = (CheckExistAsnc(sourceRecord.DocumentNumber.ToString()));
                if (checkDocumentNumber != null && checkDocumentNumber != "")
                {
                    HasErrors = true;
                    errors.Add("Incorrect Document Number:" + string.Join(",", checkDocumentNumber + " Promo number :" + sourceRecord.PromoId.ToString()));
                    errorRecords.Add(new Tuple<IEntity<Guid>, string>(sourceRecords.FirstOrDefault(g => g.PromoId == sourceRecord.PromoId), string.Join(", ", errors)));
                    invalidNumbers.Add(sourceRecord.PromoId.ToString());
                }
                if (sourceRecord.Mechanic != "VP" && (sourceRecord.Discount == null || sourceRecord.Discount > 100 || sourceRecord.Discount < 0))
                {
                    HasErrors = true;
                    errors.Add("Incorrect Discount:" + string.Join(",", sourceRecord.Discount + " Promo number :" + sourceRecord.PromoId.ToString()));
                    errorRecords.Add(new Tuple<IEntity<Guid>, string>(sourceRecords.FirstOrDefault(g => g.PromoId == sourceRecord.PromoId), string.Join(", ", errors)));
                    invalidNumbers.Add(sourceRecord.PromoId.ToString());
                }
                if (sourceRecord.Mechanic == "VP" && String.IsNullOrEmpty(sourceRecord.MechanicType))
                {
                    HasErrors = true;
                    errors.Add("Empty Mechanic type:" + string.Join(",", sourceRecord.Discount + " Promo number :" + sourceRecord.PromoId.ToString()));
                    errorRecords.Add(new Tuple<IEntity<Guid>, string>(sourceRecords.FirstOrDefault(g => g.PromoId == sourceRecord.PromoId), string.Join(", ", errors)));
                    invalidNumbers.Add(sourceRecord.PromoId.ToString());
                }
                if (sourceRecord.ShelfPrice == null || sourceRecord.ShelfPrice < 0)
                {
                    HasErrors = true;
                    errors.Add("Incorrect ActualInStoreShelfPrice:" + string.Join(",", sourceRecord.ShelfPrice + " Promo number :" + sourceRecord.PromoId.ToString()));
                    errorRecords.Add(new Tuple<IEntity<Guid>, string>(sourceRecords.FirstOrDefault(g => g.PromoId == sourceRecord.PromoId), string.Join(", ", errors)));
                    invalidNumbers.Add(sourceRecord.PromoId.ToString());
                }
            }
            if (invalidNumbers.Count > 0)
            {
                sourceRecords = sourceRecords.Where(x => !invalidNumbers.Contains(x.PromoId.ToString())).ToList();
            }

            string CheckExistAsnc(string existingText)
            {
                StringBuilder sb = new StringBuilder();
                if (existingText.Length < 4)
                    sb.Append("Minimum text lenght should be 4");
                if (!Regex.IsMatch(existingText, "^[0-9]*$"))
                    sb.Append("Invalid characters [a - zA - Z]");
                return sb.ToString();
            }
            return sourceRecords;
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

        private List<Promo> UpdatePromoesFromActualShelfPriceDiscount(DatabaseContext context, List<ImportActualShelfPriceDiscount> sourceRecords, HashSet<int> existingClientTreeIds, out IList<string> errors)
        {
            errors = new List<string>();

            var existingRecords = sourceRecords
                .GroupBy(x => x.PromoId)
                .Select(y => y.First())
                .Select(z => new { z.PromoId, z.Mechanic, z.MechanicType, z.Discount, z.ShelfPrice, z.InvoiceNumber, z.DocumentNumber })
                .ToList();

            var promoNumbers = existingRecords.Select(y => y.PromoId).Distinct().ToList();

            // RPAs available only in Current mode
            var preparedPromos = context.Set<Promo>()
                .Where(x => !x.Disabled 
                    && x.TPMmode == TPMmode.Current
                    && promoNumbers.Contains(x.Number))
                .ToList();

            List<Mechanic> mechanics = context.Set<Mechanic>().Where(g => !g.Disabled).ToList();
            List<MechanicType> mechanicTypes = context.Set<MechanicType>().Where(g => !g.Disabled).ToList();

            foreach (var preparedPromo in preparedPromos)
            {
                var sourcePromo = existingRecords.FirstOrDefault(x => x.PromoId == preparedPromo.Number);

                if (sourcePromo == null)
                    continue;

                if (preparedPromo.ClientTreeId.HasValue && !existingClientTreeIds.Contains(preparedPromo.ClientTreeId.Value))
                {
                    HasErrors = true;
                    errors.Add($"No access to import data for {preparedPromo.ClientTreeId}");
                    errorRecords.Add(new Tuple<IEntity<Guid>, string>(sourceRecords.FirstOrDefault(g => g.PromoId == sourcePromo.PromoId), string.Join(", ", errors)));
                }

                var existingMechanic = mechanics.FirstOrDefault(x => x.Name == sourcePromo.Mechanic);
                if (existingMechanic != null)
                {
                    preparedPromo.ActualInStoreMechanicId = existingMechanic.Id;
                }
                else
                {
                    HasErrors = true;
                    errors.Add("Incorrect Mechanic:" + string.Join(",", sourcePromo.Mechanic + " Promo number :" + sourcePromo.PromoId.ToString()));
                    errorRecords.Add(new Tuple<IEntity<Guid>, string>(sourceRecords.FirstOrDefault(g => g.PromoId == sourcePromo.PromoId), string.Join(", ", errors)));
                }

                var existingMechanicType = mechanicTypes.FirstOrDefault(x => x.Name == sourcePromo.MechanicType);
                if (existingMechanicType != null)
                {
                    preparedPromo.ActualInStoreMechanicTypeId = existingMechanicType.Id;
                    preparedPromo.ActualInStoreDiscount = existingMechanicType.Discount;
                }
                else if (!String.IsNullOrEmpty(sourcePromo.MechanicType))
                {
                    HasErrors = true;
                    errors.Add("Incorrect Mechanic Type:" + string.Join(",", sourcePromo.MechanicType + " Promo number :" + sourcePromo.PromoId.ToString()));
                    errorRecords.Add(new Tuple<IEntity<Guid>, string>(sourceRecords.FirstOrDefault(g => g.PromoId == sourcePromo.PromoId), string.Join(", ", errors)));
                }
                
                if (sourcePromo.Mechanic != "VP")
                    preparedPromo.ActualInStoreDiscount = sourcePromo.Discount;

                preparedPromo.ActualInStoreShelfPrice = sourcePromo.ShelfPrice;
                preparedPromo.InvoiceNumber = sourcePromo.InvoiceNumber;
                preparedPromo.DocumentNumber = sourcePromo.DocumentNumber;
            }

            if (!HasErrors)
                context.SaveChanges();

            return preparedPromos;
        }

    }
}
