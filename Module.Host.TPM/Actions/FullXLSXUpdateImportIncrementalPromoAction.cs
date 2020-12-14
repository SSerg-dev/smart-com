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
using Utility;
using Core.Settings;
using Module.Persist.TPM.CalculatePromoParametersModule;

namespace Module.Host.TPM.Actions {
    class FullXLSXUpdateImportIncrementalPromoAction : FullXLSXImportAction {

        private readonly IDictionary<string, IEnumerable<string>> Filters;

        public FullXLSXUpdateImportIncrementalPromoAction(FullImportSettings settings, IDictionary<string, IEnumerable<string>> filters) : base(settings) {
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

				// Получить фильтр по ClientId
				IEnumerable<string> clientFilter = FilterHelper.GetFilter(Filters, ModuleFilterName.Client);

				// Получить функцию Validate
				var validator = ImportModelFactory.GetImportValidator(ImportType);
                // Получить функцию SetProperty
                var builder = ImportModelFactory.GetModelBuilder(ImportType, ModelType);

				sourceRecords = sourceRecords.Select(iip => (ImportIncrementalPromo)iip).GroupBy(ip => new { ip.ProductZREP, ip.PromoNumber })
				.Select(y => (IEntity<Guid>)y.FirstOrDefault()).ToList();

				var cacheBuilder = ImportModelFactory.GetImportCacheBuilder(ImportType);
                var cache = cacheBuilder.Build(sourceRecords, context);

				// Проверка наличия продуктов с импортируемыми ZREP
				List<string> zrepList = new List<string>();
                foreach (var item in sourceRecords) {
                    String zrep = ((ImportIncrementalPromo) item).ProductZREP;
                    var splited = zrep.Split('_');
                    if (splited.Length > 1) {
                        zrep = String.Join("_", splited.Take(splited.Length - 1));
                    }
                    ((ImportIncrementalPromo) item).ProductZREP = zrep;
                    zrepList.Add(zrep);
                }
				List<string> badZrepList = zrepList.Where(z => !context.Set<Product>().Select(p => p.ZREP).Contains(z)).ToList();

				// Проверка наличия промо с импортиремым PromoNumber
				List<int?> ipWithBadNumberList = sourceRecords
					.Select(z => (ImportIncrementalPromo)z)
					.Where(n => !context.Set<Promo>()
					.Any(p => p.Number == n.PromoNumber))
					.Select(j => j.PromoNumber).ToList();

				// Проверка наличия подходящих для обновления IncrementalPromo в базе
				List<Tuple<string, int?>> ipUniqueIdent = sourceRecords
                    .Select(z => (ImportIncrementalPromo) z)
                    .Select(u => new Tuple<string, int?>(u.ProductZREP, u.PromoNumber)).ToList();

                List<Tuple<string, int?>> ipBadBaseUniqueIdent = ipUniqueIdent
                    .Where(y => !context.Set<IncrementalPromo>()
                    .Any(z => (z.Product.ZREP == y.Item1 || z.Promo.Number == y.Item2) && !z.Disabled))
                    .ToList();

				Parallel.ForEach(sourceRecords, item => {
                    IEntity<Guid> rec;
                    IList<string> warnings;
                    IList<string> validationErrors;

                    if (!validator.Validate(item, out validationErrors)) {
                        HasErrors = true;
                        errorRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", validationErrors)));
                    }
					else if (!builder.Build(item, cache, context, out rec, out warnings, out validationErrors))
					{
                        HasErrors = true;
                        errorRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", validationErrors)));
                        if (warnings.Any()) {
                            warningRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", warnings)));
                        }
					}
					else if (!IsFilterSuitable(rec, ipBadBaseUniqueIdent, badZrepList, ipWithBadNumberList, out validationErrors, context, clientFilter))
					{
						HasErrors = true;
						errorRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", validationErrors)));
					}
					else {
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

				bool hasSuccessList = !successList.IsEmpty;

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

        private bool IsFilterSuitable(IEntity<Guid> rec, List<Tuple<string, int?>> ipBadBaseUniqueIdent, List<string> badZrepList, List<int?> ipWithBadNumberList, out IList<string> errors, DatabaseContext context, IEnumerable<string> clientFilter) {
            errors = new List<string>();
            bool isSuitable = true;
			IncrementalPromo ipRec = (IncrementalPromo)rec;
			ImportIncrementalPromo iipRec = new ImportIncrementalPromo()
			{
				ProductZREP = ipRec.Product.ZREP,
				PromoNumber = ipRec.Promo.Number,
				PlanPromoIncrementalCases = ipRec.PlanPromoIncrementalCases
			};

			if (ipBadBaseUniqueIdent
				.Any(z => z.Item1 == iipRec.ProductZREP && z.Item2 == iipRec.PromoNumber))
			{
				isSuitable = false;
				errors.Add("There is no IncrementalPromo with such PromoNumber and ZREP in database");
			}
			else if (badZrepList.Any(z => z == iipRec.ProductZREP))
			{
				isSuitable = false;
				errors.Add("There is no entry of type 'Product' with such ZREP in database");
			}
			else if (ipWithBadNumberList.Contains(iipRec.PromoNumber))
			{
				isSuitable = false;
				errors.Add("There is no Promo with such PromoNumber in database");
			}
			else if (!clientFilter.Contains(ipRec.Promo.ClientTreeId.ToString()))
			{
				isSuitable = false;
				errors.Add("The entry has an inappropriate ClientId");
			}
			else if (ipRec.PlanPromoIncrementalCases < 0)
			{
				isSuitable = false;
				errors.Add("The PlanPromoIncrementalCases can't have negative value");
			}

			return isSuitable;
        }
        protected override int InsertDataToDatabase(IEnumerable<IEntity<Guid>> sourceRecords, DatabaseContext context)
		{
            ScriptGenerator generator = GetScriptGenerator();
            IList<IncrementalPromo> toUpdate = new List<IncrementalPromo>();
			IList<ImportIncrementalPromo> filteredRecords = new List<ImportIncrementalPromo>();
            List<Tuple<IEntity<Guid>, IEntity<Guid>>> toHisUpdate = new List<Tuple<IEntity<Guid>, IEntity<Guid>>>();

            foreach (IncrementalPromo incrementalPromo in sourceRecords)
			{
                ImportIncrementalPromo newRecord = new ImportIncrementalPromo()
                {
					PlanPromoIncrementalCases = incrementalPromo.PlanPromoIncrementalCases,
					PromoNumber = incrementalPromo.Promo.Number,
					ProductZREP = incrementalPromo.Product.ZREP
				};
				filteredRecords.Add(newRecord);
			}

			filteredRecords = ModuleApplyFilterHelper.ApplyFilter(filteredRecords.AsQueryable(), context);

			// Забор по уникальным полям
			var groups = filteredRecords.Select(iip => (ImportIncrementalPromo)iip).GroupBy(ip => new { ip.ProductZREP, ip.PromoNumber});
            foreach (var group in groups) {
                ImportIncrementalPromo newRecord = group.FirstOrDefault();

				if (newRecord != null)
				{
					Product product = context.Set<Product>().FirstOrDefault(x => x.ZREP == newRecord.ProductZREP && !x.Disabled);
					if (product != null)
					{
						IncrementalPromo oldRecord = context.Set<IncrementalPromo>()
							.FirstOrDefault(x => x.Product.ZREP == newRecord.ProductZREP && x.Promo.Number == newRecord.PromoNumber && !x.Disabled);
                        var oldRecordCopy = oldRecord;
                        if (oldRecord != null)
                        {
                            newRecord.Id = oldRecord.Id;
							oldRecord.PlanPromoIncrementalCases = newRecord.PlanPromoIncrementalCases;
                            var casePrice = BaselineAndPriceCalculation.CalculateCasePrice(oldRecord, context);
                            oldRecord.CasePrice = casePrice;
                            oldRecord.PlanPromoIncrementalLSV = oldRecord.CasePrice * oldRecord.PlanPromoIncrementalCases;
                            oldRecord.LastModifiedDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                            toHisUpdate.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(oldRecordCopy, oldRecord));
                            toUpdate.Add(oldRecord);
						}
					}
				}
			}

            foreach (IEnumerable<IncrementalPromo> items in toUpdate.Partition(1000))
            {
                string insertScript = String.Join("", items.Select(y => String.Format("UPDATE [DefaultSchemaSetting].IncrementalPromo SET PlanPromoIncrementalCases = {0}, " +
                                                                                    "CasePrice = {1}, " +
                                                                                    "PlanPromoIncrementalLSV = {2}, " +
                                                                                    "LastModifiedDate = '{3:yyyy-MM-dd HH:mm:ss +03:00}'  " +
                                                                                    "WHERE Id = '{4}';", 
                                                                                    y.PlanPromoIncrementalCases, y.CasePrice, y.PlanPromoIncrementalLSV, y.LastModifiedDate, y.Id)));
                context.ExecuteSqlCommand(insertScript);
            }
            //Добавление изменений в историю
            context.HistoryWriter.Write(toHisUpdate, context.AuthManager.GetCurrentUser(), context.AuthManager.GetCurrentRole(), OperationType.Updated);

            context.SaveChanges();

            return filteredRecords.Count();
        }
	}
}