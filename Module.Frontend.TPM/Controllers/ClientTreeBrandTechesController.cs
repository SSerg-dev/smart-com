﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;

using Core.Security;
using Core.Security.Models;
using Core.Settings;

using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions;
using Frontend.Core.Extensions.Export;

using Looper.Core;
using Looper.Parameters;

using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;

using Persist;
using Persist.Model;

using Thinktecture.IdentityModel.Authorization.WebApi;

using Utility;

namespace Module.Frontend.TPM.Controllers
{

    public class ClientTreeBrandTechesController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public ClientTreeBrandTechesController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<ClientTreeBrandTech> GetConstraintedQuery()
        {

            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);

            var query = Context.Set<ClientTreeBrandTech>().Where(x => !x.Disabled).AsQueryable();
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

            return query;
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<ClientTreeBrandTech> GetFilteredData(ODataQueryOptions<ClientTreeBrandTech> options)
        {
            var query = GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<ClientTreeBrandTech>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<ClientTreeBrandTech>;
        }

        public static void ResetClientTreeBrandTechDemandGroup(String newDemandCode, List<ClientTree> clientTrees, ClientTree oldClientTree, DatabaseContext databaseContext)
        {
            var clientTreeBrandTeches = databaseContext.Set<ClientTreeBrandTech>().Where(d => d.ParentClientTreeDemandCode.Equals(oldClientTree.DemandCode) && !d.Disabled);
            foreach (var item in clientTrees)
            {
                if (item.IsBaseClient)
                {
                    string demandCode;
                    if (oldClientTree.Equals(clientTrees.FirstOrDefault()))
                        demandCode = newDemandCode;
                    else
                        demandCode = String.IsNullOrEmpty(item.DemandCode) ? newDemandCode : item.DemandCode;
                    var clients = databaseContext.Set<ClientTreeBrandTech>().Where(e => e.ClientTreeId == item.Id && !e.Disabled);
                    foreach (var client in clients)
                    {
                        if (client.ParentClientTreeDemandCode.Equals(oldClientTree.DemandCode) && !string.IsNullOrEmpty(demandCode))
                        {
                            client.ParentClientTreeDemandCode = demandCode;
                        }
                        else if (client.ParentClientTreeDemandCode.Equals(oldClientTree.DemandCode) && string.IsNullOrEmpty(demandCode))
                        {
                            client.Disabled = true;
                            client.DeletedDate = DateTime.Now;
                        }
                    }
                }
            }
        }

        public static async Task<int> FillClientTreeBrandTechTableAsync(DatabaseContext context)
        {
            var clients = context.Set<ClientTree>().Where(ct => ct.IsBaseClient && ct.EndDate == null).ToList();
            var brandTeches = context.Set<BrandTech>().AsNoTracking().Where(bt => !bt.Disabled).ToList();

            clients.ForEach(c =>
            {
                string demandCode = GetDemandCode(c, context);
                brandTeches.ForEach(bt =>
                {
                    int shareRecordCounnt = context.Set<ClientTreeBrandTech>()
                                .Where(s => s.ClientTreeId == c.Id
                                            && s.BrandTechId == bt.Id
                                            && s.ParentClientTreeDemandCode == demandCode
                                            && !s.Disabled).Count();
                    if (shareRecordCounnt == 0 && !String.IsNullOrEmpty(demandCode))
                    {
                        context.Set<ClientTreeBrandTech>().Add(new ClientTreeBrandTech()
                        {
                            Id = Guid.NewGuid(),
                            ClientTreeId = c.Id,
                            BrandTechId = bt.Id,
                            ParentClientTreeDemandCode = demandCode,
                            Share = 0,
                            CurrentBrandTechName = bt.BrandsegTechsub,
                            DeletedDate = null,
                            Disabled = false
                        });
                    }
                });
            });

            return await context.SaveChangesAsync();
        }

        public static async Task DeleteInvalidClientBrandTech(int key, DatabaseContext databaseContext)
        {
            var invalidBrandTeches = databaseContext.Set<ClientTreeBrandTech>().Where(x => x.ClientTreeId == key && x.Disabled == false && x.DeletedDate == null);

            if (invalidBrandTeches != null)
            {

                foreach (var invalidBrandTech in invalidBrandTeches)
                {
                    invalidBrandTech.Disabled = true;
                    invalidBrandTech.DeletedDate = DateTime.Now;
                }
                await databaseContext.SaveChangesAsync();
            }
        }

        private static string GetDemandCode(ClientTree clientTree, DatabaseContext context)
        {
            if (clientTree == null)
                return "";
            if (!string.IsNullOrEmpty(clientTree.DemandCode))
                return clientTree.DemandCode.Trim();

            return GetDemandCode(GetParent(clientTree, context), context);
        }

        private static ClientTree GetParent(ClientTree clientTree, DatabaseContext context)
        {
            return context.Set<ClientTree>()
                .Where(ct => ct.EndDate == null && ct.ObjectId == clientTree.parentId && ct.Type != "root")
                .FirstOrDefault();
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<ClientTreeBrandTech> GetClientTreeBrandTech([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetClientTreeBrandTeches());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<ClientTreeBrandTech> GetClientTreeBrandTeches(ODataQueryOptions<ClientTreeBrandTech> queryOptions = null)
        {
            var query = GetConstraintedQuery();
            return query;
        }

        /// <summary>
        /// Создает записи в таблице ClientTreeBrandTech аналогичные тем, что были для переданного в параметрах объекта ClientTree.
        /// </summary>
        public static async Task CreateActualCloneWithZeroShareByClientTree(DatabaseContext databaseContext, ClientTree clientTree)
        {
            var newClientTreeBrandTeches = new List<ClientTreeBrandTech>();
            var actualClientTreeBrandTeches = GetActualQuery(databaseContext);
            var actualClientTreeBrandTechesForCurrentClient = actualClientTreeBrandTeches.Where(x => x.ClientTreeId == clientTree.Id);
            var brandTeches = databaseContext.Set<BrandTech>().Where(x => !x.Disabled);

            foreach (var actualClientTreeBrandTechForCurrentClient in actualClientTreeBrandTechesForCurrentClient)
            {
                actualClientTreeBrandTechForCurrentClient.DeletedDate = DateTimeOffset.Now;
                actualClientTreeBrandTechForCurrentClient.Disabled = true;
            }

            foreach (var brandTech in brandTeches)
            {
                var parentDemandCode = actualClientTreeBrandTechesForCurrentClient.FirstOrDefault(x => x.BrandTechId == brandTech.Id)?.ParentClientTreeDemandCode;
                if (!String.IsNullOrEmpty(parentDemandCode))
                {
                    var newClientTreeBrandTech = new ClientTreeBrandTech
                    {
                        Id = Guid.NewGuid(),
                        ClientTreeId = clientTree.Id,
                        BrandTechId = brandTech.Id,
                        Share = 0,
                        ParentClientTreeDemandCode = parentDemandCode,
                        CurrentBrandTechName = brandTech.BrandsegTechsub
                    };

                    newClientTreeBrandTeches.Add(newClientTreeBrandTech);
                }
            }

            databaseContext.Set<ClientTreeBrandTech>().AddRange(newClientTreeBrandTeches);
            await databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Создает записи в таблице ClientTreeBrandTech аналогичные тем, что были для переданного в параметрах объекта BrandTech.
        /// </summary>
        public static async Task CreateActualCloneWithZeroShareByBrandTechAsync(DatabaseContext databaseContext, BrandTech brandTech)
        {
            var newClientTreeBrandTeches = new List<ClientTreeBrandTech>();
            var actualClientTreeBrandTeches = GetActualQuery(databaseContext);
            var actualClientTreeBrandTechesForCurrentBrandTech = actualClientTreeBrandTeches.Where(x => x.BrandTechId == brandTech.Id);
            var clientTrees = databaseContext.Set<ClientTree>().Where(x => x.EndDate == null);

            foreach (var actualClientTreeBrandTechForCurrentBrandTech in actualClientTreeBrandTechesForCurrentBrandTech)
            {
                actualClientTreeBrandTechForCurrentBrandTech.DeletedDate = DateTimeOffset.Now;
                actualClientTreeBrandTechForCurrentBrandTech.Disabled = true;
            }

            foreach (var clientTree in clientTrees)
            {
                var parentDemandCode = actualClientTreeBrandTechesForCurrentBrandTech.FirstOrDefault(x => x.ClientTreeId == clientTree.Id)?.ParentClientTreeDemandCode;
                if (!String.IsNullOrEmpty(parentDemandCode))
                {
                    var newClientTreeBrandTech = new ClientTreeBrandTech
                    {
                        Id = Guid.NewGuid(),
                        ClientTreeId = clientTree.Id,
                        BrandTechId = brandTech.Id,
                        Share = 0,
                        ParentClientTreeDemandCode = parentDemandCode,
                        CurrentBrandTechName = brandTech.BrandsegTechsub
                    };

                    newClientTreeBrandTeches.Add(newClientTreeBrandTech);
                }
            }

            databaseContext.Set<ClientTreeBrandTech>().AddRange(newClientTreeBrandTeches);
            await databaseContext.SaveChangesAsync();
        }

        public static async Task DisableNotActualClientTreeBrandTech(DatabaseContext databaseContext)
        {
            var clientTreeBrandTeches = databaseContext.Set<ClientTreeBrandTech>().Where(x => !x.Disabled).ToList();
            var actualClientTreeBrandTeches = GetActualQuery(databaseContext);
            var notActualClientTreeBrandTeches = clientTreeBrandTeches.Except(actualClientTreeBrandTeches);

            foreach (var notActualClientTreeBrandTech in notActualClientTreeBrandTeches)
            {
                if (!notActualClientTreeBrandTech.Disabled)
                {
                    notActualClientTreeBrandTech.Disabled = true;
                    notActualClientTreeBrandTech.DeletedDate = DateTimeOffset.Now;
                }
            }

            await databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Возвращает только актуальные записи с учетом фильтров
        /// </summary>
        /// <returns></returns>
        private IQueryable<ClientTreeBrandTech> GetFilteredActualQuery()
        {
            var actualClientTreeBrandTeches = GetActualQuery(Context);
            return actualClientTreeBrandTeches.AsQueryable();
        }

        /// <summary>
        /// Возвращает только актуальные записи
        /// </summary>
        public static List<ClientTreeBrandTech> GetActualQuery(DatabaseContext databaseContext)
        {
            var clientTreeBrandTeches = databaseContext.Set<ClientTreeBrandTech>().Include("ClientTree").Include("BrandTech").Where(x => !x.Disabled).ToList();

            return clientTreeBrandTeches;
        }

        public static IEnumerable<Column> GetExportSettings()
        {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 0, Field = "ParentClientTreeDemandCode", Header = "Demand code", Quoting = false },
                new Column() { Order = 1, Field = "ClientTree.ObjectId", Header = "Client hierarchy code", Quoting = false },
                new Column() { Order = 2, Field = "ClientTree.Name", Header = "Base client", Quoting = false },
                new Column() { Order = 3, Field = "BrandTech.BrandsegTechsub", Header = "Brand tech", Quoting = false },
                new Column() { Order = 4, Field = "Share", Header = "Share", Quoting = false }
            };
            return columns;
        }

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> ExportXLSX(ODataQueryOptions<ClientTreeBrandTech> options)
        {
            var ids = GetFilteredActualQuery().Select(q => q.Id).AsEnumerable();
            var query = Context.Set<ClientTreeBrandTech>().Where(q => ids.Contains(q.Id));
            IQueryable results = options.ApplyTo(query);
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            HandlerData data = new HandlerData();
            string handlerName = "ExportHandler";

            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TModel", typeof(ClientTreeBrandTech), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(ClientTreeBrandTechesController), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(ClientTreeBrandTechesController.GetExportSettings), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = $"Export {nameof(ClientTreeBrandTech)} dictionary",
                Name = "Module.Host.TPM.Handlers." + handlerName,
                ExecutionPeriod = null,
                CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                LastExecutionDate = null,
                NextExecutionDate = null,
                ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                UserId = userId,
                RoleId = roleId
            };
            handler.SetParameterData(data);
            Context.LoopHandlers.Add(handler);
            await Context.SaveChangesAsync();

            return Content(HttpStatusCode.OK, "success");
        }

        [ClaimsAuthorize]
        public async Task<HttpResponseMessage> FullImportXLSX()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string importDir = Core.Settings.AppSettingsManager.GetSetting("IMPORT_DIRECTORY", "ImportFiles");
                string fileName = await FileUtility.UploadFile(Request, importDir);

                await CreateImportTask(fileName, "FullXLSXUpdateImportClientShareHandler");

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StringContent("success = true");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

                return result;
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private async Task CreateImportTask(string fileName, string importHandler)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            ImportResultFilesModel resiltfile = new ImportResultFilesModel();
            ImportResultModel resultmodel = new ImportResultModel();

            HandlerData data = new HandlerData();
            FileModel file = new FileModel()
            {
                LogicType = "Import",
                Name = System.IO.Path.GetFileName(fileName),
                DisplayName = System.IO.Path.GetFileName(fileName)
            };

            HandlerDataHelper.SaveIncomingArgument("File", file, data, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportClientsShare), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportClientsShare).Name, data, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(ImportClientsShare), data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = "Загрузка импорта из файла " + typeof(ClientTreeBrandTech).Name,
                Name = "Module.Host.TPM.Handlers." + importHandler,
                ExecutionPeriod = null,
                RunGroup = typeof(ClientTreeBrandTech).Name,
                CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                LastExecutionDate = null,
                NextExecutionDate = null,
                ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                UserId = userId,
                RoleId = roleId
            };
            handler.SetParameterData(data);
            Context.LoopHandlers.Add(handler);
            await Context.SaveChangesAsync();
        }

        [ClaimsAuthorize]
        public IHttpActionResult DownloadTemplateXLSX()
        {
            try
            {
                IEnumerable<Column> columns = GetExportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
                string filename = string.Format("{0}Template.xlsx", "ClientsShare");
                if (!Directory.Exists(exportDir))
                {
                    Directory.CreateDirectory(exportDir);
                }
                string filePath = Path.Combine(exportDir, filename);
                exporter.Export(Enumerable.Empty<ClientTreeBrandTech>(), filePath);
                string file = Path.GetFileName(filePath);
                return Content(HttpStatusCode.OK, file);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }

        }
    }
}