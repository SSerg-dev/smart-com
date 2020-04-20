using System;
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

    public class ClientTreeBrandTechesController : EFContextController {
        private readonly IAuthorizationManager authorizationManager;

        public ClientTreeBrandTechesController(IAuthorizationManager authorizationManager) {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<ClientTreeBrandTech> GetConstraintedQuery() {

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
             var clientTreeBrandTeches = databaseContext.Set<ClientTreeBrandTech>().Where(d=> d.ParentClientTreeDemandCode.Equals(oldClientTree.DemandCode) && !d.Disabled);
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
                        if(client.ParentClientTreeDemandCode.Equals(oldClientTree.DemandCode))
                            client.ParentClientTreeDemandCode = demandCode;
                    }
                }
            }
        }
        public static int FillClientTreeBrandTechTable(DatabaseContext databaseContext)
        {
            var deleteScript = GetDeleteScript();
            var insertScript = GetInsertScript();

            var insertScriptExists = !String.IsNullOrEmpty(insertScript) && !String.IsNullOrWhiteSpace(insertScript); 
            if (insertScriptExists)
            {
                databaseContext.Database.ExecuteSqlCommand(insertScript);
            }

            return databaseContext.SaveChanges();
        }

        public static string GetDeleteScript()
        {
            var deleteScript = $@"";
            return deleteScript;
        }

        public static string GetInsertScript()
        {
            var insertScript = 
            $@"
                DECLARE @ClientTreeCount INT = (SELECT COUNT(*) FROM {nameof(ClientTree)} WHERE {nameof(ClientTree.EndDate)} IS NULL AND {nameof(ClientTree.IsBaseClient)} = 1);
                DECLARE @ClientTreeCounter INT = 0;

                WHILE (@ClientTreeCounter < @ClientTreeCount)
                BEGIN
	                DECLARE @ClientTreeId INT;
	                DECLARE @ObjectId VARCHAR(MAX);
	                SELECT @ClientTreeId = {nameof(ClientTree.Id)}, @ObjectId = {nameof(ClientTree.ObjectId)} FROM {nameof(ClientTree)} WHERE {nameof(ClientTree.EndDate)} IS NULL AND {nameof(ClientTree.IsBaseClient)} = 1 ORDER BY {nameof(ClientTree.Id)} OFFSET @ClientTreeCounter ROWS FETCH NEXT 1 ROWS ONLY;
	
	                IF @ObjectId IS NOT NULL 
	                BEGIN
		                DECLARE @ParentDemandCode VARCHAR(MAX) = NULL;
		                DECLARE @ParentClientTreeType VARCHAR(MAX) = NULL;

		                WHILE ((@ParentClientTreeType IS NULL OR @ParentClientTreeType != 'root') AND (@ParentDemandCode IS NULL OR LEN(@ParentDemandCode) = 0))
		                BEGIN
			                SELECT @ObjectId = {nameof(ClientTree.parentId)}, @ParentClientTreeType = {nameof(ClientTree.Type)}, @ParentDemandCode = {nameof(ClientTree.DemandCode)} FROM {nameof(ClientTree)} WHERE {nameof(ClientTree.EndDate)} IS NULL AND {nameof(ClientTree.ObjectId)} = @ObjectId;
		                END

		                IF @ParentDemandCode IS NOT NULL AND LEN(@ParentDemandCode) > 0
		                BEGIN
			                DECLARE @BrandTechCounter INT = 0;
			                DECLARE @BrandTechCount INT = (SELECT COUNT(*) FROM {nameof(BrandTech)} WHERE {nameof(BrandTech.Disabled)} = 0)

			                WHILE (@BrandTechCounter < @BrandTechCount)
			                BEGIN
                                DECLARE @BrandTechId UNIQUEIDENTIFIER;
                                DECLARE @CurrentBrandTechName VARCHAR(MAX);

				                SELECT @BrandTechId = {nameof(BrandTech.Id)}, @CurrentBrandTechName = {nameof(BrandTech.Name)} FROM {nameof(BrandTech)} WHERE {nameof(BrandTech.Disabled)} = 0 ORDER BY {nameof(BrandTech.Id)} OFFSET @BrandTechCounter ROWS FETCH NEXT 1 ROWS ONLY;

				                IF (SELECT COUNT(*) FROM {nameof(ClientTreeBrandTech)} WHERE {nameof(ClientTreeBrandTech.ClientTreeId)} = @ClientTreeId AND {nameof(ClientTreeBrandTech.BrandTechId)} = @BrandTechId AND {nameof(ClientTreeBrandTech.Disabled)} = 0) = 0
				                
                                BEGIN
					                INSERT INTO {nameof(ClientTreeBrandTech)} ([{nameof(ClientTreeBrandTech.Id)}], [{nameof(ClientTreeBrandTech.ClientTreeId)}], [{nameof(ClientTreeBrandTech.BrandTechId)}], [{nameof(ClientTreeBrandTech.ParentClientTreeDemandCode)}], [{nameof(ClientTreeBrandTech.Share)}], [{nameof(ClientTreeBrandTech.CurrentBrandTechName)}], [{nameof(ClientTreeBrandTech.DeletedDate)}], [{nameof(ClientTreeBrandTech.Disabled)}]) VALUES (NEWID(), @ClientTreeId, @BrandTechId, @ParentDemandCode, 0, @CurrentBrandTechName, null, 0);
				                END
				                SET @BrandTechCounter = @BrandTechCounter + 1;
			                END
		                END
	                END
	                SET @ClientTreeCounter = @ClientTreeCounter + 1;
                END
            ";

            return insertScript;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<ClientTreeBrandTech> GetClientTreeBrandTech([FromODataUri] System.Guid key) {
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
        public static void CreateActualCloneWithZeroShareByClientTree(DatabaseContext databaseContext, ClientTree clientTree)
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
                        CurrentBrandTechName = brandTech.Name
                    };

                    newClientTreeBrandTeches.Add(newClientTreeBrandTech);
                }
            }

            databaseContext.Set<ClientTreeBrandTech>().AddRange(newClientTreeBrandTeches);
            databaseContext.SaveChanges();
        }

        /// <summary>
        /// Создает записи в таблице ClientTreeBrandTech аналогичные тем, что были для переданного в параметрах объекта BrandTech.
        /// </summary>
        public static void CreateActualCloneWithZeroShareByBrandTech(DatabaseContext databaseContext, BrandTech brandTech)
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
                        CurrentBrandTechName = brandTech.Name
                    };

                    newClientTreeBrandTeches.Add(newClientTreeBrandTech);
                }
            }

            databaseContext.Set<ClientTreeBrandTech>().AddRange(newClientTreeBrandTeches);
            databaseContext.SaveChanges();
        }

        public static void DisableNotActualClientTreeBrandTech(DatabaseContext databaseContext)
        {
            var clientTreeBrandTeches = databaseContext.Set<ClientTreeBrandTech>().ToList();
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

            databaseContext.SaveChanges();
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
            var actualClientTreeBrandTeches = new List<ClientTreeBrandTech>();

            var clientTreeBrandTeches = databaseContext.Set<ClientTreeBrandTech>().Where(x => !x.Disabled).ToList();
            var actualBrandTeches = databaseContext.Set<BrandTech>().Where(x => !x.Disabled).ToList();
            var actualClientTrees = databaseContext.Set<ClientTree>().Where(x => x.EndDate == null && !x.IsBaseClient).ToList();

            foreach (var clientTreeBrandTech in clientTreeBrandTeches)
            {
                if (clientTreeBrandTech.ClientTree.EndDate == null && clientTreeBrandTech.ClientTree.IsBaseClient)
                {
                    if (actualBrandTeches.Any(x => x.Name == clientTreeBrandTech.CurrentBrandTechName /*&& !x.Brand.Disabled && !x.Technology.Disabled*/))
                    {
                        var currentClientTree = clientTreeBrandTech.ClientTree;
                        while (currentClientTree != null && currentClientTree.Type != "root" && String.IsNullOrEmpty(currentClientTree.DemandCode))
                        {
                            currentClientTree = actualClientTrees.FirstOrDefault(x => x.ObjectId == currentClientTree.parentId);
                        }

                        // Если родитель узла существует и его DemandCode равен DemandCode из ClientTreeBrandTech.
                        if (currentClientTree != null &&
                            !String.IsNullOrEmpty(currentClientTree.DemandCode)
                            && currentClientTree.DemandCode == clientTreeBrandTech.ParentClientTreeDemandCode)
                        {
                            actualClientTreeBrandTeches.Add(clientTreeBrandTech);
                        }
                    }
                }
            }

            return actualClientTreeBrandTeches;
        }

        private IEnumerable<Column> GetExportSettings() {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 0, Field = "ParentClientTreeDemandCode", Header = "Demand code", Quoting = false },
                new Column() { Order = 1, Field = "ClientTree.ObjectId", Header = "Client hierarchy code", Quoting = false },
                new Column() { Order = 2, Field = "ClientTree.Name", Header = "Base client", Quoting = false },
                new Column() { Order = 3, Field = "CurrentBrandTechName", Header = "Brand tech", Quoting = false },
                new Column() { Order = 4, Field = "Share", Header = "Share", Quoting = false }
            };
            return columns;
        }
        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<ClientTreeBrandTech> options) {
            try {
                IQueryable results = options.ApplyTo(GetFilteredActualQuery());
                IEnumerable<Column> columns = GetExportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                UserInfo user = authorizationManager.GetCurrentUser();
                string username = user == null ? "" : user.Login;
                string filePath = exporter.GetExportFileName("ClientTreeBrandTech", username);
                exporter.Export(results, filePath);
                string filename = System.IO.Path.GetFileName(filePath);
                return Content<string>(HttpStatusCode.OK, filename);
            } catch (Exception e) {
                return Content<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [ClaimsAuthorize]
        public async Task<HttpResponseMessage> FullImportXLSX() {
            try {
                if (!Request.Content.IsMimeMultipartContent()) {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string importDir = Core.Settings.AppSettingsManager.GetSetting("IMPORT_DIRECTORY", "ImportFiles");
                string fileName = await FileUtility.UploadFile(Request, importDir);

                CreateImportTask(fileName, "FullXLSXUpdateImportClientShareHandler");

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StringContent("success = true");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

                return result;
            } catch (Exception e) {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private void CreateImportTask(string fileName, string importHandler) {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            using (DatabaseContext context = new DatabaseContext()) {
                ImportResultFilesModel resiltfile = new ImportResultFilesModel();
                ImportResultModel resultmodel = new ImportResultModel();

                HandlerData data = new HandlerData();
                FileModel file = new FileModel() {
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

                LoopHandler handler = new LoopHandler() {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Загрузка импорта из файла " + typeof(ClientTreeBrandTech).Name,
                    Name = "Module.Host.TPM.Handlers." + importHandler,
                    ExecutionPeriod = null,
                    CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                    LastExecutionDate = null,
                    NextExecutionDate = null,
                    ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                    UserId = userId,
                    RoleId = roleId
                };
                handler.SetParameterData(data);
                context.LoopHandlers.Add(handler);
                context.SaveChanges();
            }
        }

        [ClaimsAuthorize]
        public IHttpActionResult DownloadTemplateXLSX() {
            try {
                IEnumerable<Column> columns = GetExportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
                string filename = string.Format("{0}Template.xlsx", "ClientsShare");
                if (!Directory.Exists(exportDir)) {
                    Directory.CreateDirectory(exportDir);
                }
                string filePath = Path.Combine(exportDir, filename);
                exporter.Export(Enumerable.Empty<ClientTreeBrandTech>(), filePath);
                string file = Path.GetFileName(filePath);
                return Content(HttpStatusCode.OK, file);
            } catch (Exception e) {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }

        }
    }
}