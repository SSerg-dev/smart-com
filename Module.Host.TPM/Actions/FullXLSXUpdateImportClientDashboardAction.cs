using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Threading.Tasks;
using Core.Data;
using Core.Extensions;
using Core.History;
using Core.Settings;
using Interfaces.Implementation.Action;
using Interfaces.Implementation.Import.FullImport;
using Looper.Parameters;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model;
using Persist.ScriptGenerator;
using Utility;
using Utility.Import;
using Utility.Import.Cache;
using Utility.Import.ImportModelBuilder;
using Utility.Import.ModelBuilder;
using Utility.LogWriter;

namespace Moule.Host.TPM.Actions
{
    public class FullXLSXUpdateImportClientDashboardAction : FullXLSXImportAction
    {
        private readonly IEnumerable<PropertyInfo> _modelProperties;
        private readonly IEnumerable<string> _createModeAllowedFields;
        private readonly IDictionary<string, IEnumerable<string>> _roleFieldsConstraints;
        private readonly Guid _roleId;
        private readonly Guid _userId;

        public FullXLSXUpdateImportClientDashboardAction(FullImportSettings settings) 
            : base(settings)
        {
            _roleId = settings.RoleId;
            _userId = settings.UserId;

            _modelProperties = typeof(ClientDashboard).GetProperties().Where(x => x.Name != nameof(ClientDashboard.Id));

            _createModeAllowedFields = new List<string>
            {
                nameof(ClientDashboard.ClientTreeId),
                nameof(ClientDashboard.ClientHierarchy),
                nameof(ClientDashboard.BrandTechId),
                nameof(ClientDashboard.BrandsegTechsubName),
                nameof(ClientDashboard.Year),
            };

            _roleFieldsConstraints = new Dictionary<string, IEnumerable<string>>
            {
                [UserRoles.Administrator.ToString()] = new List<string>(_modelProperties.Select(x => x.Name)),
                [UserRoles.SupportAdministrator.ToString()] = new List<string>(_modelProperties.Select(x => x.Name)),
                [UserRoles.DemandFinance.ToString()] = new List<string>
                {
                    nameof(ClientDashboard.ShopperTiPlanPercent),
                    nameof(ClientDashboard.MarketingTiPlanPercent),
                    nameof(ClientDashboard.ROIPlanPercent),
                    nameof(ClientDashboard.IncrementalNSVPlan),
                    nameof(ClientDashboard.PromoNSVPlan),
                    nameof(ClientDashboard.NonPromoTiCostPlanPercent),
                    nameof(ClientDashboard.PromoTiCostPlanPercent),
                },
                [UserRoles.KeyAccountManager.ToString()] = new List<string>
                {
                    nameof(ClientDashboard.ProductionPlan),
                    nameof(ClientDashboard.BrandingPlan),
                    nameof(ClientDashboard.BTLPlan),
                },
                [UserRoles.DemandPlanning.ToString()] = new List<string>
                {
                    nameof(ClientDashboard.PlanLSV),
                },
            };
        }

        protected override ImportResultFilesModel ApplyImport(IList<IEntity<Guid>> sourceRecords, out int successCount, out int warningCount, out int errorCount)
        {
            DatabaseContext context = new DatabaseContext();
            try
            {
                ConcurrentBag<IEntity<Guid>> records = new ConcurrentBag<IEntity<Guid>>();
                ConcurrentBag<IEntity<Guid>> successList = new ConcurrentBag<IEntity<Guid>>();
                ConcurrentBag<Tuple<IEntity<Guid>, string>> errorRecords = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();
                ConcurrentBag<Tuple<IEntity<Guid>, string>> warningRecords = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();
                IImportValidator validator = ImportModelFactory.GetImportValidator(ImportType);
                IModelBuilder builder = ImportModelFactory.GetModelBuilder(ImportType, ModelType);
                IImportCacheBuilder importCacheBuilder = ImportModelFactory.GetImportCacheBuilder(ImportType);
                IImportCache cache = importCacheBuilder.Build(sourceRecords, context);

                DateTime dtNow = DateTime.Now;

                //Ограничение пользователя  
                IList<Constraint> constraints = context.Constraints
                    .Where(x => x.UserRole.UserId.Equals(_userId) && x.UserRole.Role.Id.Equals(_roleId))
                    .ToList();
                IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
                IQueryable<ClientTree> ctQuery = context.Set<ClientTree>().AsNoTracking().Where(x => DateTime.Compare(x.StartDate, dtNow) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dtNow) > 0));
                IQueryable<ClientTreeHierarchyView> hierarchy = context.Set<ClientTreeHierarchyView>().AsNoTracking();
                ctQuery = ModuleApplyFilterHelper.ApplyFilter(ctQuery, hierarchy, filters);

                //Запрос действующих ObjectId
                IList<ClientTree> existedClientTrees = ctQuery.Where(y => y.EndDate == null || y.EndDate > dtNow).ToList();
                IList<Tuple<int, int>> existedClientTreesTuples = existedClientTrees.Select(y => new Tuple<int, int>(y.Id, y.ObjectId)).ToList();
                IList<int> existedexistedClientTreesIds = existedClientTreesTuples.Select(y => y.Item2).ToList();

                //Присваивание ID
                Parallel.ForEach(sourceRecords, importObj =>
                {
                    ImportClientDashboard typedItem = (ImportClientDashboard)importObj;
                    int objId = typedItem.ClientTreeId;
                    if (existedexistedClientTreesIds.Contains(objId))
                    {
                        var finden = existedClientTreesTuples.FirstOrDefault(y => y.Item2 == objId);
                        if (finden != null)
                        {
                            typedItem.ClientTreeObjectId = finden.Item1;
                        }
                    }  
                });


                var importClientDashboardRecords = new ConcurrentBag<ImportClientDashboard>(sourceRecords.Cast<ImportClientDashboard>());
                var clientTrees = new ConcurrentBag<ClientTree>(context.Set<ClientTree>().Where(x => !x.EndDate.HasValue));
                var brandTeches = new ConcurrentBag<BrandTech>(context.Set<BrandTech>().Where(x => !x.Disabled));
                var clientDashboardViews = context.Set<ClientDashboardView>().ToList();

                Parallel.ForEach(sourceRecords, delegate (IEntity<Guid> item)
                {
                    IEntity<Guid> model;
                    IList<string> warnings;
                    IList<string> errors;

                    if (!validator.Validate(item, out errors))
                    {
                        HasErrors = true;
                        errorRecords.Add(new Tuple<IEntity<Guid>, string>(item, string.Join(", ", errors)));
                    }
                    else if (!builder.Build(item, cache, context, out model, out warnings, out errors))
                    {
                        HasErrors = true;
                        errorRecords.Add(new Tuple<IEntity<Guid>, string>(item, string.Join(", ", errors)));
                        if (warnings.Any())
                        {
                            warningRecords.Add(new Tuple<IEntity<Guid>, string>(item, string.Join(", ", warnings)));
                        }
                    }
                    else if (!IsFilterSuitable(model, importClientDashboardRecords, clientDashboardViews, clientTrees, brandTeches, existedexistedClientTreesIds,  out errors))
                    {
                        HasErrors = true;
                        errorRecords.Add(new Tuple<IEntity<Guid>, string>(item, string.Join(", ", errors)));
                    }
                    else
                    {
                        records.Add(model);
                        successList.Add(item);
                        if (warnings.Any())
                        {
                            warningRecords.Add(new Tuple<IEntity<Guid>, string>(item, string.Join(", ", warnings)));
                        }
                    }
                });
                logger.Trace("Persist models built");

                int resultRecordCount = 0;

                ResultStatus = GetImportStatus();
                var importModel = ImportUtility.BuildActiveImport(UserId, RoleId, ImportType);
                importModel.Status = ResultStatus;
                context.Imports.Add(importModel);

                bool hasSuccessList = AllowPartialApply || !HasErrors;
                if (hasSuccessList) {
                    // Закончить импорт
                    var items = BeforeInsert(records, context).ToList();
                    resultRecordCount = InsertDataToDatabase(items, context);
                }
                logger.Trace("Persist models inserted");
               // context.SaveChanges();
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
            finally
            {
                if (context != null)
                {
                    ((IDisposable)context).Dispose();
                }
            }
        }

        protected new string GetImportStatus()
        {
            if (HasErrors)
            {
                if (AllowPartialApply)
                {
                    return "Partial Complete";
                }
                return "Error";
            }
            else if (Warnings.Any())
            {
                return "Warning";
            }
            else
            {
                return "Complete";
            }
        }

        protected bool IsFilterSuitable(IEntity<Guid> rec, ConcurrentBag<ImportClientDashboard> importClientDashboardRecords, IEnumerable<ClientDashboardView> clientDashboardViews, 
            ConcurrentBag<ClientTree> clientTrees, ConcurrentBag<BrandTech> brandTeches, IList<int> existedObjIds, out IList<string> errors)
        {
            errors = new List<string>();
            var clientDashboardRecord = (ClientDashboard)rec;

            var clientTree = clientTrees.FirstOrDefault(x => !x.EndDate.HasValue && x.ObjectId == clientDashboardRecord.ClientTreeId);
            var brandTech = brandTeches.FirstOrDefault(x => !x.Disabled && x.BrandsegTechsub == clientDashboardRecord.BrandsegTechsubName);

            //Проверка по существующим активным ClientTree для пользователя
            if (!existedObjIds.Contains(clientDashboardRecord.ClientTreeId.Value))
            { 
                errors.Add(clientDashboardRecord.ClientTreeId.ToString() + " not in user's active ClientTree list");
            }
            if (importClientDashboardRecords.GroupBy(x => new { x.ClientTreeId, x.BrandsegTechsubName, x.Year })
                .FirstOrDefault(x => x.Key.ClientTreeId == clientDashboardRecord.ClientTreeId && x.Key.BrandsegTechsubName == clientDashboardRecord.BrandsegTechsubName && x.Key.Year.ToString() == clientDashboardRecord.Year)?.Count() > 1)
            {
                errors.Add($"You can't create more than one entry per {nameof(ImportClientDashboard.ClientTreeId)}, {nameof(ImportClientDashboard.BrandsegTechsubName)}, {nameof(ImportClientDashboard.Year)} " +
                    $"({clientDashboardRecord.ClientTreeId}, {clientDashboardRecord.BrandsegTechsubName}, {clientDashboardRecord.Year})");
            }

            if (clientTree == null)
            {
                errors.Add($"Client with id {clientDashboardRecord.ClientTreeId} not found.");
            }

            if (brandTech == null)
            {
                errors.Add($"Brandtech with BrandsegTechsub {clientDashboardRecord.BrandsegTechsubName} not found.");
            }

            if (clientTree != null && brandTech != null)
            {
                var clientDashboardViewsContainsImportedRecord = clientDashboardViews.Any(x => x.ObjectId == clientTree.ObjectId && x.BrandsegTechsubName == brandTech.BrandsegTechsub && x.Year.ToString() == clientDashboardRecord.Year);
                if (!clientDashboardViewsContainsImportedRecord)
                {
                    var message = $"Record with Client ID = {clientDashboardRecord.ClientTreeId}, Brand Tech = {clientDashboardRecord.BrandsegTechsubName}, Year = {clientDashboardRecord.Year} doesn't exist.";
                    errors.Add(message);
                }
            }

            return errors.Count() == 0;
        }

        protected override int InsertDataToDatabase(IEnumerable<IEntity<Guid>> records, DatabaseContext context)
        {
            List<Tuple<IEntity<Guid>, IEntity<Guid>>> toHisCreate = new List<Tuple<IEntity<Guid>, IEntity<Guid>>>();
            List<Tuple<IEntity<Guid>, IEntity<Guid>>> toHisUpdate = new List<Tuple<IEntity<Guid>, IEntity<Guid>>>(); 
            List<ClientDashboardView> toHisUpdateTemp = new List<ClientDashboardView>();
            List<ClientDashboardView> toHisCreateTemp = new List<ClientDashboardView>();

            var clientDashboardView = context.Set<ClientDashboardView>().ToList();
            var clientDashboard = context.Set<ClientDashboard>();
            var clientTrees = context.Set<ClientTree>();
            var brandTechs = context.Set<BrandTech>();
            IList<ClientDashboard> toCreate = new List<ClientDashboard>();
            IList<ClientDashboard> toUpdate = new List<ClientDashboard>();

            ClientDashboardView hisModel = null;
            ClientDashboardView oldHisModel = null;

            var currentRole = context.Set<Role>().FirstOrDefault(x => !x.Disabled && x.Id == _roleId);
            if (currentRole != null)
            {
                var clientDashboardRecords = records.Cast<ClientDashboard>();
                foreach (var clientDashboardRecord in clientDashboardRecords)
                {

                    var notMaterializedClientDashboardRecord = clientDashboard
                        .FirstOrDefault(x => x.ClientTreeId == clientDashboardRecord.ClientTreeId && x.BrandsegTechsubName == clientDashboardRecord.BrandsegTechsubName && x.Year == clientDashboardRecord.Year);
                    oldHisModel = clientDashboardView.Where(x => x.ObjectId == clientDashboardRecord.ClientTreeId && x.BrandsegTechsubName == clientDashboardRecord.BrandsegTechsubName && x.Year.ToString() == clientDashboardRecord.Year).FirstOrDefault();
                  
                    if (notMaterializedClientDashboardRecord != null)
                    {
                        SetValues(clientDashboardRecord, notMaterializedClientDashboardRecord, _modelProperties, currentRole, false);

                        oldHisModel.Id = notMaterializedClientDashboardRecord.Id;
                        toHisUpdateTemp.Add(oldHisModel);
                        toUpdate.Add(notMaterializedClientDashboardRecord);
                    }
                    else
                    {
                        var clientTree = clientTrees.FirstOrDefault(x => !x.EndDate.HasValue && x.ObjectId == clientDashboardRecord.ClientTreeId);
                        var brandTech = brandTechs.FirstOrDefault(x => !x.Disabled && x.BrandsegTechsub == clientDashboardRecord.BrandsegTechsubName);

                        if (clientTree != null && brandTech != null)
                        {
                            var newClientDashboardRecord = new ClientDashboard
                            {
                                Id = Guid.NewGuid(),
                                ClientHierarchy = clientTree?.FullPathName,
                                BrandTechId = brandTech?.Id,
                                BrandsegTechsubName = brandTech?.BrandsegTechsub
                            };

                            SetValues(clientDashboardRecord, newClientDashboardRecord, _modelProperties, currentRole, true);

                            oldHisModel.Id = newClientDashboardRecord.Id;
                            toHisCreateTemp.Add(oldHisModel);
                            toCreate.Add(newClientDashboardRecord);
                        }
                    }
                }

                logger.Trace("Persist models inserted");
                SaveToDatabase(toCreate, OperationType.Created, context);
                SaveToDatabase(toUpdate, OperationType.Updated, context);

                foreach (var oldModel in toHisUpdateTemp)
                {
                    hisModel = clientDashboardView.Where(x => x.ObjectId == oldModel.ObjectId && x.BrandTechId == oldModel.BrandTechId && x.Year == oldModel.Year).FirstOrDefault();
                    hisModel.Id = oldModel.Id;
                    toHisUpdate.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(oldModel, hisModel));
                }
                foreach (var oldModel in toHisCreateTemp)
                {
                    hisModel = clientDashboardView.Where(x => x.ObjectId == oldModel.ObjectId && x.BrandTechId == oldModel.BrandTechId && x.Year == oldModel.Year).FirstOrDefault();
                    hisModel.Id = oldModel.Id;
                    toHisCreate.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(null, hisModel));
                }

                context.HistoryWriter.Write(toHisCreate, context.AuthManager.GetCurrentUser(), context.AuthManager.GetCurrentRole(), OperationType.Created);
                context.HistoryWriter.Write(toHisUpdate, context.AuthManager.GetCurrentUser(), context.AuthManager.GetCurrentRole(), OperationType.Updated);
                logger.Trace("Data saved");
            }
            else
            {
                Errors.Add($"Role with id {_roleId} not found.");
            }

            return records.Count();
        }

        private void SetValues(ClientDashboard fromClientDashboardRecord, ClientDashboard toClientDashboardRecord, IEnumerable<PropertyInfo> properties, Role currentRole, bool createMode)
        {
            foreach (var property in properties)
            {
                var fromClientDashboardRecordValue = property.GetValue(fromClientDashboardRecord);

                if (createMode && _roleFieldsConstraints.ContainsKey(currentRole.SystemName) && 
                    (_createModeAllowedFields.Any(x => x == property.Name) || _roleFieldsConstraints[currentRole.SystemName].Any(x => x == property.Name)))
                {
                    var value = property.GetValue(fromClientDashboardRecord);
                    if (value != null)
                    {
                        property.SetValue(toClientDashboardRecord, fromClientDashboardRecordValue);
                    }
                }
                else if (!createMode && _roleFieldsConstraints.ContainsKey(currentRole.SystemName) 
                    && !_createModeAllowedFields.Any(x => x == property.Name) && _roleFieldsConstraints[currentRole.SystemName].Any(x => x == property.Name))
                {
                    property.SetValue(toClientDashboardRecord, fromClientDashboardRecordValue);
                }
            }
        }

        private void ToView(ClientDashboardView clientDashboardView, ClientDashboard model)
        {
            if (clientDashboardView != null && model != null)
            {
                clientDashboardView.Id = model.Id;
                clientDashboardView.ShopperTiPlanPercent = model.ShopperTiPlanPercent;
                clientDashboardView.MarketingTiPlanPercent = model.MarketingTiPlanPercent;
                clientDashboardView.ProductionPlan = model.ProductionPlan;
                clientDashboardView.BrandingPlan = model.BrandingPlan;
                clientDashboardView.BTLPlan = model.BTLPlan;
                clientDashboardView.ROIPlanPercent = model.ROIPlanPercent;
                clientDashboardView.IncrementalNSVPlan = model.IncrementalNSVPlan;
                clientDashboardView.PromoNSVPlan = model.PromoNSVPlan;
                clientDashboardView.LSVPlan = model.PlanLSV;
                clientDashboardView.NonPromoTiCostPlanPercent = model.NonPromoTiCostPlanPercent;
                clientDashboardView.PromoTiCostPlanPercent = model.PromoTiCostPlanPercent;
            }
        }

        private void SaveToDatabase(IList<ClientDashboard> clientDashboardViews, OperationType operation, DatabaseContext context)
        {
            if (operation == OperationType.Created)
            {
                foreach (IEnumerable<ClientDashboard> items in clientDashboardViews.Partition(1000))
                {
                    string updateScript = String.Join("", items.Select(y =>
                    String.Format("INSERT INTO [DefaultSchemaSetting].ClientDashboard (ShopperTiPlanPercent, MarketingTiPlanPercent, ProductionPlan, BrandingPlan, BTLPlan, " +
                    "ROIPlanPercent ,IncrementalNSVPlan, PromoNSVPlan, ClientTreeId, BrandsegTechsubName, Year, ClientHierarchy, BrandTechId, " +
                    "PromoTiCostPlanPercent, NonPromoTiCostPlanPercent, PlanLSV, [Id])" +
                    " VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}');",
                    y.ShopperTiPlanPercent, y.MarketingTiPlanPercent, y.ProductionPlan.ToString(), y.BrandingPlan, y.BTLPlan,
                    y.ROIPlanPercent, y.IncrementalNSVPlan, y.PromoNSVPlan, y.ClientTreeId, y.BrandsegTechsubName,
                    y.Year, y.ClientHierarchy, y.BrandTechId, y.PromoTiCostPlanPercent, y.NonPromoTiCostPlanPercent, y.PlanLSV, y.Id)));
                    context.ExecuteSqlCommand(updateScript);
                }
            }
            else if (operation == OperationType.Updated)
            {
                foreach (IEnumerable<ClientDashboard> items in clientDashboardViews.Partition(1000))
                {
                    string updateScript = String.Join("", items.Select(y =>
                    String.Format("UPDATE [DefaultSchemaSetting].ClientDashboard SET ShopperTiPlanPercent = '{0}', MarketingTiPlanPercent = '{1}', ProductionPlan = '{2}', BrandingPlan = '{3}'," +
                            "BTLPlan = '{4}', ROIPlanPercent = '{5}', IncrementalNSVPlan = '{6}', PromoNSVPlan = '{7}', " +
                            "PromoTiCostPlanPercent = {8}, NonPromoTiCostPlanPercent = {9}, PlanLSV = {10}" +
                            "WHERE [Id] = '{11}';",
                            y.ShopperTiPlanPercent, y.MarketingTiPlanPercent, y.ProductionPlan, y.BrandingPlan, y.BTLPlan,
                            y.ROIPlanPercent, y.IncrementalNSVPlan, y.PromoNSVPlan, 
                            y.PromoTiCostPlanPercent, y.NonPromoTiCostPlanPercent, y.PlanLSV, y.Id)));
                    context.ExecuteSqlCommand(updateScript);
                }
            }
        }
    }
}
