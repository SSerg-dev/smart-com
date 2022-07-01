using Core.Data;
using Core.Extensions;
using Core.History;
using Core.Settings;
using Interfaces.Implementation.Action;
using Interfaces.Implementation.Import.FullImport;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Host.TPM.Util;
//using Module.Persist.TPM.Migrations;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using NLog;
using Persist;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Utility.FileWorker;
using Utility.Import;
using Utility.Import.ImportModelBuilder;

namespace Module.Host.TPM.Actions
{
    public class FullXLSXPLUDictionaryImportAction : BaseAction
    {
        public FullXLSXPLUDictionaryImportAction(FullImportSettings settings)
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
                throw new ImportException("An error occurred while loading the import file.");
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
            successCount = 0;
            warningCount = 0;
            errorCount = 0;
            var updateCount = 0;
            var createCount = 0;

            var importModel = ImportUtility.BuildActiveImport(UserId, RoleId, ImportType);
            
            ScriptGenerator generatorUpdate = new ScriptGenerator(typeof(Plu));
            
            var successList = new List<IEntity<Guid>>();
            var pluUpdateList = new List<Plu>();

            var pluCreateListHistory = new List<Tuple<IEntity<Guid>, IEntity<Guid>>>();
            var pluUpdateListHistory = new List<Tuple<IEntity<Guid>, IEntity<Guid>>>();
            var errorRecords = new List<Tuple<IEntity<Guid>, string>>();
            var warningRecords = new List<Tuple<IEntity<Guid>, string>>();


            var imports = sourceRecords.Select(x => new { Import = x as ImportPLUDictionary, Original = x }).Where(x => string.IsNullOrEmpty( x.Import.PLU) != true) .ToList();
            var objectIds = imports.GroupBy(x => x.Import.ObjectId).ToList().Select(x=>x.Key).ToList();
            
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Database.Log = x => Debug.WriteLine(x);
               var clientProducts = context.Set<PLUDictionary>().AsNoTracking().Where(x => objectIds.Contains(x.ObjectId)).ToList();
                foreach (var importItem in imports)
                {
                    var item = importItem.Import;
                    if (string.IsNullOrEmpty(item.EAN_PC))
                    {
                        Errors.Add($"EAN_PC is empty for PLU - {item.PLU}");
                        errorRecords.Add (new Tuple<IEntity<Guid>, string>( importItem.Original, "EAN_PC is empty"));
                    }
                    else
                    {
                        var clientProduct = clientProducts.FirstOrDefault(x => x.EAN_PC == item.EAN_PC && x.ObjectId == item.ObjectId);
                        if (clientProduct == null)
                        {
                            Errors.Add($"EAN_PC '{item.EAN_PC}' not found");
                            errorRecords.Add(new Tuple<IEntity<Guid>, string>(importItem.Original, "Not found"));
                        }
                        else
                        {
                            successCount++;
                            if(clientProduct.PluCode == null)
							{
								createCount++;
								var plu = new Plu() { ClientTreeId = clientProduct.ClientTreeId,  PluCode = item.PLU, EAN_PC = item.EAN_PC, Id = Guid.NewGuid() };
                                importItem.Original.Id = plu.Id;
								var exists = context.Set<Plu>().Local.FirstOrDefault(x => x.ClientTreeId == plu.ClientTreeId && x.EAN_PC == item.EAN_PC);
								if (exists != null && exists.PluCode != plu.PluCode)
								{
									Warnings.Add($"Double PluCode for one EAN_PC. PluCode -  {exists.PluCode},{plu.PluCode}");
									Warnings.Add($"Was created only  {clientProduct.EAN_PC}, {exists.PluCode}");
									warningRecords.Add(new Tuple<IEntity<Guid>, string>(importItem.Original, $"Double PluCode for one EAN_PC. PluCode -  {exists.PluCode},{plu.PluCode}"));
									warningRecords.Add(new Tuple<IEntity<Guid>, string>(importItem.Original, $"Was created only  {clientProduct.EAN_PC}, {exists.PluCode}"));
								}
								else
								{
									context.Set<Plu>().Local.Add(plu);
									successList.Add(importItem.Original);
									pluCreateListHistory.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(null, importItem.Original));
								}
							}
                            else if(item.PLU != clientProduct.PluCode)
							{
                                updateCount++;
                                var newPlu = new PLUDictionary()
                                {
                                    ClientTreeId = clientProduct.ClientTreeId,
                                    ClientTreeName = clientProduct.ClientTreeName,
                                    EAN_PC = clientProduct.EAN_PC,
                                    ObjectId = clientProduct.ObjectId,
                                    PluCode = item.PLU,
                                    Id = clientProduct.Id
                                };
                                pluUpdateListHistory.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(clientProduct, newPlu));
                                successList.Add(importItem.Original);
                                pluUpdateList.Add(new Plu() { ClientTreeId = clientProduct.ClientTreeId, PluCode = item.PLU, EAN_PC = item.EAN_PC, Id=newPlu.Id });
                            }
                        }
                    }
                }

                foreach (IEnumerable<Plu> items in pluUpdateList.Partition(10000))
                {
                    var insertScript = generatorUpdate.BuildUpdateScript(items);
                    context.ExecuteSqlCommand(insertScript);
                }


                importModel.Status = GetImportStatus();
                context.Imports.Add(importModel);

                //Добавление записей в историю
                context.HistoryWriter.Write(pluCreateListHistory, context.AuthManager.GetCurrentUser(), context.AuthManager.GetCurrentRole(), OperationType.Created);
                context.HistoryWriter.Write(pluUpdateListHistory, context.AuthManager.GetCurrentUser(), context.AuthManager.GetCurrentRole(), OperationType.Updated);

                context.SaveChanges();
            }



            ResultStatus = GetImportStatus();
            var resultFilesModel = SaveProcessResultHelper.SaveResultToFile(importModel.Id, successList, null, errorRecords, warningRecords);
            return resultFilesModel;
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

        protected virtual void Fail()
        {

        }

        protected virtual void Success()
        {

        }

        protected virtual void Complete()
        {

        }
    }
}
