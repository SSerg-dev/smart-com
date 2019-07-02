using Core.Data;
using Core.Extensions;
using Core.Settings;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Interfaces.Implementation.Action;
using Interfaces.Implementation.Import.FullImport;
using Looper.Parameters;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Import;
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
using Utility.Import;
using Utility.Import.Cache;
using Utility.Import.ImportModelBuilder;
using Utility.Import.ModelBuilder;

namespace Module.Host.TPM.Actions {

    /// <summary>
    /// Переопределение Action из ядра приложения
    /// </summary>
    public class FullXLSXTradeInvestmentUpdateImportAction : BaseAction {

        public FullXLSXTradeInvestmentUpdateImportAction(FullImportSettings settings) {
            UserId = settings.UserId;
            RoleId = settings.RoleId;
            ImportFile = settings.ImportFile;
            ImportType = settings.ImportType;
            ModelType = settings.ModelType;
            Separator = settings.Separator;
            Quote = settings.Quote;
            HasHeader = settings.HasHeader;

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

        protected bool AllowPartialApply { get; set; }

        protected string ResultStatus { get; set; }
        protected bool HasErrors { get; set; }

        protected readonly static Logger logger = LogManager.GetCurrentClassLogger();



        /// <summary>
        /// Выполнить разбор source-данных в импорт-модели и сохранить в БД
        /// </summary>
        public override void Execute() {
            logger.Trace("Begin");
            try {
                ResultStatus = null;
                HasErrors = false;

                IList<IEntity<Guid>> sourceRecords = ParseImportFile();

                int successCount;
                int warningCount;
                int errorCount;
                ImportResultFilesModel resultFilesModel = ApplyImport(sourceRecords, out successCount, out warningCount, out errorCount);

                if (HasErrors) {
                    Fail();
                } else {
                    Success();
                }

                // Сохранить выходные параметры
                Results["ImportSourceRecordCount"] = sourceRecords.Count();
                Results["ImportResultRecordCount"] = successCount;
                Results["ErrorCount"] = errorCount;
                Results["WarningCount"] = warningCount;
                Results["ImportResultFilesModel"] = resultFilesModel;

            } catch (Exception e) {
                HasErrors = true;
                string msg = String.Format("FullImportAction failed: {0}", e.ToString());
                logger.Error(msg);
                string message;
                if (e.IsUniqueConstraintException()) {
                    message = "This entry already exists in the database";
                } else {
                    message = e.ToString();
                }
                Errors.Add(message);
                ResultStatus = ImportUtility.StatusName.ERROR;
            } finally {
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
        private IList<IEntity<Guid>> ParseImportFile() {
            string importDir = AppSettingsManager.GetSetting<string>("IMPORT_DIRECTORY", "ImportFiles");
            string importFilePath = Path.Combine(importDir, ImportFile.Name);
            if (!File.Exists(importFilePath)) {
                throw new Exception("Import File not found");
            }

            IImportModelBuilder<string[]> builder = ImportModelFactory.GetCSVImportModelBuilder(ImportType);
            IImportValidator validator = ImportModelFactory.GetImportValidator(ImportType);
            int sourceRecordCount;
            List<string> errors;
            IList<Tuple<string, string>> buildErrors;
            IList<Tuple<IEntity<Guid>, string>> validateErrors;
            logger.Trace("before parse file");
            IList<IEntity<Guid>> records = ParseXLSXFile(importFilePath, null, builder, validator, Separator, Quote, HasHeader, out sourceRecordCount, out errors, out buildErrors, out validateErrors);
            logger.Trace("after parse file");

            // Обработать ошибки
            foreach (string err in errors) {
                Errors.Add(err);
            }
            if (errors.Any()) {
                HasErrors = true;
                throw new ApplicationException("An error occurred while retrieving the import file");
            }

            return records;
        }

        /// <summary>
        /// Разбор XLSX-файла импорта
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="importId"></param>
        /// <param name="builder"></param>
        /// <param name="separator"></param>
        /// <param name="sourceFileRecordCount"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static IList<IEntity<Guid>> ParseXLSXFile(string filePath, Guid? importId, IImportModelBuilder<string[]> builder, IImportValidator validator, string separator, string quote, bool hasHeader, out int sourceFileRecordCount, out List<string> errors, out IList<Tuple<string, string>> notBuildedRecords, out IList<Tuple<IEntity<Guid>, string>> notValidRecords) {
            sourceFileRecordCount = 0;
            IList<IEntity<Guid>> result = new List<IEntity<Guid>>();
            notBuildedRecords = new List<Tuple<string, string>>();
            notValidRecords = new List<Tuple<IEntity<Guid>, string>>();
            errors = new List<string>();
            SpreadsheetDocument book = SpreadsheetDocument.Open(filePath, false);
            //Брать только первый лист
            WorksheetPart wSheet = book.WorkbookPart.WorksheetParts.FirstOrDefault(y => y.Uri.OriginalString.Contains("sheet1"));
            using (OpenXmlReader reader = OpenXmlReader.Create(wSheet)) {
                DocumentFormat.OpenXml.Spreadsheet.Row row;
                int columnsCount = 0;
                while (reader.Read()) {
                    if (reader.ElementType == typeof(DocumentFormat.OpenXml.Spreadsheet.Row)) {
                        row = (DocumentFormat.OpenXml.Spreadsheet.Row) reader.LoadCurrentElement();
                        if (row.RowIndex == 1 && hasHeader) {
                            foreach (Cell cell in row.Elements<Cell>()) {
                                if (cell.CellValue != null) {
                                    columnsCount++;
                                }
                            }
                            continue;
                        }
                        if (!row.HasChildren) {
                            continue;
                        }
                        string text;
                        List<string> strRow = new List<string>();
                        int lastCellIndex = 0;
                        foreach (Cell cell in row.Elements<Cell>()) {
                            int cureCellIndex = ColumnIndex(cell.CellReference);
                            int emptyCellsCount = cureCellIndex - lastCellIndex;
                            if (emptyCellsCount != 1) {
                                for (int i = 1; i < emptyCellsCount; i++) {
                                    strRow.Add(String.Empty);
                                }
                            }
                            lastCellIndex = cureCellIndex;
                            if (cell.CellValue != null) {
                                text = String.Empty;
                            } else {
                                strRow.Add(String.Empty);
                                continue;
                            }
                            if (cell.DataType != null) {
                                if (cell.DataType == CellValues.SharedString) {
                                    int id = -1;
                                    if (Int32.TryParse(cell.InnerText, out id)) {
                                        SharedStringItem item = book.WorkbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                                        if (item.Text != null) {
                                            text = item.Text.Text;
                                        } else if (item.InnerText != null) {
                                            text = item.InnerText;
                                        } else if (item.InnerXml != null) {
                                            text = item.InnerXml;
                                        }

                                    }
                                } else if (cell.DataType == CellValues.Number) {
                                    text = cell.InnerText;
                                }
                            } else if (cell.InnerText != null) {

                                // В случае формулы, брать значение
                                if (cell.CellFormula != null && !String.IsNullOrEmpty(cell.CellFormula.InnerText)) {
                                    text = cell.CellValue.InnerText;
                                } else {
                                    text = cell.InnerText;
                                }

                            }
                            strRow.Add(text);
                        }
                        while (strRow.Count < columnsCount) {
                            strRow.Add(String.Empty);
                        }
                        if (strRow.All(x => String.IsNullOrEmpty(x) || String.IsNullOrWhiteSpace(x))) {
                            continue;
                        }
                        if (ParseRow(null, importId, builder, validator, ';', null, hasHeader, result, ref errors, out notBuildedRecords, out notValidRecords, splitedRow: strRow.ToArray())) {
                            sourceFileRecordCount++;
                        }

                    }
                }
            }
            return result;
        }

        private static bool ParseRow(string row, Guid? importId, IImportModelBuilder<string[]> builder, IImportValidator validator, char separator, char? quote, bool hasHeader, IList<IEntity<Guid>> records, ref List<string> errors, out IList<Tuple<string, string>> notBuildedRecords, out IList<Tuple<IEntity<Guid>, string>> notValidRecords, string[] splitedRow = null) {
            bool isEmpty = true;
            notBuildedRecords = new List<Tuple<string, string>>();
            notValidRecords = new List<Tuple<IEntity<Guid>, string>>();
            if (!String.IsNullOrEmpty(row) || splitedRow != null) {
                isEmpty = false;
                if (splitedRow == null) {
                    splitedRow = ParseCSVRow(row, separator, quote);
                }
                IEntity<Guid> item;
                IList<string> buildErrors;
                if (builder.Build(splitedRow, importId, out item, out buildErrors)) {
                    IList<string> validateErrors;
                    if (validator.Validate(item, out validateErrors)) {
                        records.Add(item);
                    } else {
                        errors.AddRange(validateErrors);
                        notValidRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", validateErrors)));
                    }
                } else {
                    errors.AddRange(buildErrors);
                    notBuildedRecords.Add(new Tuple<string, string>(row, String.Join(", ", buildErrors)));
                }
            }
            return !isEmpty;
        }

        /// <summary>
        /// Распарсить csv-строку по колонкам
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static string[] ParseCSVRow(string source, char separator, char? quote) {
            if (quote.HasValue) {
                List<string> tokens = new List<string>();
                int last = -1;
                int current = 0;
                bool inText = false;
                while (current < source.Length) {
                    char ch = source[current];
                    if (ch == quote.Value) {
                        inText = !inText;
                    } else if (ch == separator) {
                        if (!inText) {
                            tokens.Add(source.Substring(last + 1, (current - last - 1)));
                            last = current;
                        }
                    }
                    current++;
                }
                if (last != source.Length - 1) {
                    tokens.Add(source.Substring(last + 1));
                }
                return tokens.ToArray();
            } else {
                return source.Split(new char[] { separator }, StringSplitOptions.None);
            }
        }

        private static int ColumnIndex(string reference) {
            int ci = 0;
            reference = reference.ToUpper();
            for (int ix = 0; ix < reference.Length && reference[ix] >= 'A'; ix++)
                ci = (ci * 26) + ((int) reference[ix] - 64);
            return ci;
        }


        /// <summary>
        /// Загрузить импортируемые записи в БД
        /// </summary>
        /// <param name="sourceRecords"></param>
        /// <param name="successCount"></param>
        /// <param name="warningCount"></param>
        /// <param name="errorCount"></param>
        /// <returns></returns>
        private ImportResultFilesModel ApplyImport(IList<IEntity<Guid>> sourceRecords, out int successCount, out int warningCount, out int errorCount) {

            // Логика переноса данных из временной таблицы в постоянную
            // Получить записи текущего импорта
            using (DatabaseContext context = new DatabaseContext()) {

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
                IList<Tuple<String, Guid?>> brandTechesTuples = brandTeches.Select(y => new Tuple<String, Guid?>(y.Name, y.Id)).ToList();

                //Присваивание ID
                Parallel.ForEach(sourceRecords, item => {
                    ImportTradeInvestment typedItem = (ImportTradeInvestment) item;
                    int objId = typedItem.ClientTreeObjectId;
                    if (existedexistedClientTreesIds.Contains(objId)) {
                        var finden = existedClientTreesTuples.FirstOrDefault(y => y.Item2 == objId);
                        if (finden != null) {
                            typedItem.ClientTreeId = finden.Item1;
                        }
                    }

                    var bt = brandTechesTuples.FirstOrDefault(y => y.Item1 == typedItem.BrandTechName);
                    ((ImportTradeInvestment) item).BrandTechId = bt == null ? null : bt.Item2;

                    typedItem.MarcCalcBudgetsBool = typedItem.MarcCalcBudgets.ToUpper() == "YES";
                    typedItem.MarcCalcROIBool = typedItem.MarcCalcROI.ToUpper() == "YES";

                });

                //Проверка по пересечению времени
                IList<Tuple<int, Guid?, String, String>> badTimesIds = new List<Tuple<int, Guid?, String, String>>();

                IList<Tuple<int, String, String, String, DateTimeOffset?, DateTimeOffset?>> existedTradeInvestmentsTimes =
                    this.GetQuery(context).Where(x => !x.Disabled).Select(y => new Tuple<int, String, String, String, DateTimeOffset?, DateTimeOffset?>(y.ClientTreeId, y.BrandTech != null ? y.BrandTech.Name : null, y.TIType, y.TISubType, y.StartDate, y.EndDate)).ToList();

                IList<Tuple<int, String, String, String, DateTimeOffset?, DateTimeOffset?>> importedTradeInvestmentsTimes =
                    sourceRecords.Select(y => new Tuple<int, String, String, String, DateTimeOffset?, DateTimeOffset?>(((ImportTradeInvestment) y).ClientTreeId, ((ImportTradeInvestment) y).BrandTechName, ((ImportTradeInvestment) y).TIType, ((ImportTradeInvestment) y).TISubType, ((ImportTradeInvestment) y).StartDate, ((ImportTradeInvestment) y).EndDate)).ToList();


                Parallel.ForEach(sourceRecords, item => {
                    ImportTradeInvestment typedObj = (ImportTradeInvestment) item;
                    if (!DateCheck(typedObj, existedTradeInvestmentsTimes, importedTradeInvestmentsTimes)) {
                        badTimesIds.Add(new Tuple<int, Guid?, String, String>(typedObj.ClientTreeObjectId, typedObj.BrandTechId, typedObj.TIType, typedObj.TISubType));
                    }
                });

                //Стандартные проверки
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
                    } else if (!IsFilterSuitable(rec, existedexistedClientTreesIds, badTimesIds, brandTechesTuples, out validationErrors)) {
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
                Import importModel = ImportUtility.BuildActiveImport(UserId, RoleId, ImportType);
                importModel.Status = ResultStatus;
                context.Imports.Add(importModel);

                bool hasSuccessList = AllowPartialApply || !HasErrors;
                if (hasSuccessList) {
                    // Закончить импорт
                    IEnumerable<IEntity<Guid>> items = BeforeInsert(records, context).ToList();
                    resultRecordCount = InsertDataToDatabase(items, context);
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

                return resultFilesModel;
            }
        }

        private string GetImportStatus() {
            if (HasErrors) {
                if (AllowPartialApply) {
                    return ImportUtility.StatusName.PARTIAL_COMPLETE;
                } else {
                    return ImportUtility.StatusName.ERROR;
                }
            } else {
                return ImportUtility.StatusName.COMPLETE;
            }
        }

        protected ScriptGenerator _generator { get; set; }

        //Кастомная проверка
        protected virtual bool IsFilterSuitable(IEntity<Guid> rec, IList<int> existedObjIds, IList<Tuple<int, Guid?, String, String>> badTimesIds, IList<Tuple<String, Guid?>> brandTechesTuples, out IList<string> errors) {
            errors = new List<string>();
            bool isError = false;

            ImportTradeInvestment importObj = (ImportTradeInvestment) rec;

            //Проверка по существующим активным ClientTree для пользователя
            if (!existedObjIds.Contains(importObj.ClientTreeObjectId)) {
                isError = true;
                errors.Add(importObj.ClientTreeObjectId.ToString() + " not in user's active ClientTree list");
            } else {

                //Проверка пересечения по времени на клиенте
                if (badTimesIds.Any(y => y.Item1 == importObj.ClientTreeObjectId && y.Item2 == importObj.BrandTechId && y.Item3 == importObj.TIType && y.Item4 == importObj.TISubType)) {
                    isError = true;
                    errors.Add(" there can not be two TradeInvestment of such client, brandTech, Type and SubType in some Time");
                }

            }

            Tuple<String, Guid?> btech = brandTechesTuples.FirstOrDefault(x => x.Item1 == importObj.BrandTechName);
            if (importObj.BrandTechName != "All" && !String.IsNullOrEmpty(importObj.BrandTechName) && btech == null) {
                isError = true;
                errors.Add(" There is no such BrandTech");
            }

            //Проверка StartDate, EndDate
            if (importObj.StartDate == null || importObj.EndDate == null) {
                isError = true;
                errors.Add(" StartDate and EndDate must be fullfilled");
            } else {

                if (importObj.StartDate > importObj.EndDate) {
                    isError = true;
                    errors.Add(" StartDate must be before EndDate");
                }

            }

            ////Проверка двоичных значений
            //if (importObj.MarcCalcROI.ToUpper() != "YES" && importObj.MarcCalcROI.ToUpper() != "NO") {
            //    isError = true;
            //    errors.Add(" Marc Calc ROI Must be YES or NO");
            //}
            //if (importObj.MarcCalcBudgets.ToUpper() != "YES" && importObj.MarcCalcBudgets.ToUpper() != "NO") {
            //    isError = true;
            //    errors.Add(" Marc Calc Budgets Must be YES or NO");
            //}


            // SizePercent не больше 100 процентов
            if (importObj.SizePercent > 100 || importObj.SizePercent < 0) {
                isError = true;
                errors.Add("SizePercent must be in percentage 0 up to 100");
            }

            return !isError;
        }

        protected virtual void Fail() {

        }

        protected virtual void Success() {

        }

        protected virtual void Complete() {

        }

        /// <summary>
        /// Запись в базу аналогично изменению TradeInvestment из интерфейса через контекст
        /// </summary>
        /// <param name="sourceRecords"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected int InsertDataToDatabase(IEnumerable<IEntity<Guid>> sourceRecords, DatabaseContext context) {
            IList<TradeInvestment> toCreate = new List<TradeInvestment>();
            var query = GetQuery(context).ToList();

            DateTime dtNow = DateTime.Now;
            foreach (ImportTradeInvestment newRecord in sourceRecords) {
                TradeInvestment oldRecord = query.FirstOrDefault(x => x.ClientTree.ObjectId == newRecord.ClientTreeObjectId && !x.Disabled);
                BrandTech bt = context.Set<BrandTech>().FirstOrDefault(x => x.Name == newRecord.BrandTechName && !x.Disabled);
                float sizepercent = newRecord.SizePercent < 1 ? (float)Math.Round(newRecord.SizePercent * 100, 2) : (float)Math.Round(newRecord.SizePercent, 2);
                TradeInvestment toSave = new TradeInvestment() {
                    StartDate = newRecord.StartDate,
                    EndDate = newRecord.EndDate,
                    SizePercent = sizepercent,
                    TISubType = newRecord.TISubType,
                    TIType = newRecord.TIType,
                    ClientTreeId = newRecord.ClientTreeId,
                    MarcCalcROI = newRecord.MarcCalcROIBool,
                    MarcCalcBudgets = newRecord.MarcCalcBudgetsBool,
                    BrandTechId = bt != null ? (Guid?) bt.Id : null
                };

                toCreate.Add(toSave);
            }

            foreach (IEnumerable<TradeInvestment> items in toCreate.Partition(100)) {
                context.Set<TradeInvestment>().AddRange(items);
            }
            context.SaveChanges();

            return sourceRecords.Count();
        }

        protected virtual IEnumerable<IEntity<Guid>> BeforeInsert(IEnumerable<IEntity<Guid>> records, DatabaseContext context) {
            return records;
        }

        private IEnumerable<TradeInvestment> GetQuery(DatabaseContext context) {
            IQueryable<TradeInvestment> query = context.Set<TradeInvestment>().AsNoTracking();
            return query.ToList();
        }


        // Логика проверки пересечения времени
        public bool DateCheck(ImportTradeInvestment toCheck,
            IList<Tuple<int, String, String, String, DateTimeOffset?, DateTimeOffset?>> existedTradeInvestmentsTimes,
            IList<Tuple<int, String, String, String, DateTimeOffset?, DateTimeOffset?>> importedTradeInvestmentsTimes) {

            int clientTreeId = toCheck.ClientTreeId;
            String brandTech = (String.IsNullOrWhiteSpace(toCheck.BrandTechName) || toCheck.BrandTechName == "All") ? null : toCheck.BrandTechName; ;

            foreach (Tuple<int, String, String, String, DateTimeOffset?, DateTimeOffset?> item in existedTradeInvestmentsTimes.Where(y => y.Item1 == clientTreeId && y.Item2 == brandTech && y.Item3 == toCheck.TIType && y.Item4 == toCheck.TISubType)) {
                if ((item.Item5 <= toCheck.StartDate && item.Item6 >= toCheck.StartDate) ||
                    (item.Item5 <= toCheck.EndDate && item.Item6 >= toCheck.EndDate)) {
                    return false;
                }
            }

            var ctCogs = importedTradeInvestmentsTimes.Where(y => y.Item1 == clientTreeId && y.Item2 == brandTech && y.Item3 == toCheck.TIType && y.Item4 == toCheck.TISubType);
            if (ctCogs.Count() > 1) {
                var thisTI = new Tuple<int, String, String, String, DateTimeOffset?, DateTimeOffset?>(toCheck.ClientTreeId, toCheck.BrandTechName, toCheck.TIType, toCheck.TISubType, toCheck.StartDate, toCheck.EndDate);
                if (ctCogs.Where(y => y.Item1 == thisTI.Item1 && y.Item2 == thisTI.Item2 && y.Item3 == thisTI.Item3 && y.Item4 == thisTI.Item4 && y.Item5 == thisTI.Item5 && y.Item6 == thisTI.Item6).Count() > 1) {
                    return false;
                }
                foreach (Tuple<int, String, String, String, DateTimeOffset?, DateTimeOffset?> item in ctCogs
                    .Where(y => !(y.Item1 == thisTI.Item1 && y.Item2 == thisTI.Item2 && y.Item3 == thisTI.Item3 && y.Item4 == thisTI.Item4 && y.Item5 == thisTI.Item5 && y.Item6 == thisTI.Item6))) {
                    if ((item.Item5 <= toCheck.StartDate && item.Item6 >= toCheck.StartDate) ||
                        (item.Item5 <= toCheck.EndDate && item.Item6 >= toCheck.EndDate) ||
                        (item.Item5 >= toCheck.StartDate && item.Item6 <= toCheck.EndDate)) {
                        return false;
                    }
                }
            }
            return true;
        }

    }
}
