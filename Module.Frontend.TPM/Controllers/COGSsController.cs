using AutoMapper;
using Core.Security;
using Core.Security.Models;
using Core.Settings;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using Persist;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.Results;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;
using Module.Persist.TPM.Utils;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Module.Persist.TPM.Model.DTO;
using System.Collections.Specialized;
using Core.Dependency;
using Module.Frontend.TPM.Util;

namespace Module.Frontend.TPM.Controllers {

    public class COGSsController : EFContextController {
        private readonly IAuthorizationManager authorizationManager;

        public COGSsController(IAuthorizationManager authorizationManager) {
            this.authorizationManager = authorizationManager;
        }


        protected IQueryable<COGS> GetConstraintedQuery() {

            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();

			IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
			IQueryable<COGS> query = Context.Set<COGS>().Where(e => !e.Disabled);
			IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();

			query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

			return query;
		}


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<COGS> GetCOGS([FromODataUri] System.Guid key) {
            return SingleResult.Create(GetConstraintedQuery());
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<COGS> GetCOGSs() {
            return GetConstraintedQuery();
        }


        [ClaimsAuthorize]
        public IHttpActionResult Put([FromODataUri] System.Guid key, Delta<COGS> patch) {
            var model = Context.Set<COGS>().Find(key);
            if (model == null) {
                return NotFound();
            }
            patch.Put(model);
            try {
                Context.SaveChanges();
            } catch (DbUpdateConcurrencyException) {
                if (!EntityExists(key)) {
                    return NotFound();
                } else {
                    throw;
                }
            }
            return Updated(model);
        }

        [ClaimsAuthorize]
        public IHttpActionResult Post(COGS model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            // делаем UTC +3
            model.StartDate = ChangeTimeZoneUtil.ResetTimeZone(model.StartDate);
            model.EndDate = ChangeTimeZoneUtil.ResetTimeZone(model.EndDate);
            model.Year = model.StartDate.Value.Year;

            var proxy = Context.Set<COGS>().Create<COGS>();
            var result = (COGS) Mapper.Map(model, proxy, typeof(COGS), proxy.GetType(), opts => opts.CreateMissingTypeMaps = true);

            //Проверка пересечения по времени на клиенте
            if (!DateCheck(result)) {
                string msg = "There can not be two COGS of client and Brand Tech  in some Time";
                return InternalServerError(new Exception(msg)); //Json(new { success = false, message = msg });
            }
            if (result.StartDate.Value.Year != result.EndDate.Value.Year)
            {
                string msg = "Start and End date must be in same year";
                return InternalServerError(new Exception(msg));
            }

            Context.Set<COGS>().Add(result);

            try {
                Context.SaveChanges();
            } catch (Exception e) {
                return GetErorrRequest(e);
            }

            return Created(model);
        }

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<COGS> patch) {
            try {
                var model = Context.Set<COGS>().Find(key);
                if (model == null) {
                    return NotFound();
                }

                var oldModel = new COGS
                {
                    Disabled = model.Disabled,
                    DeletedDate = model.DeletedDate,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    LVSpercent = model.LVSpercent,
                    ClientTreeId = model.ClientTree.Id,
                    BrandTechId = model.BrandTechId,
                    Year = model.Year
                };

                patch.Patch(model);
                // делаем UTC +3
                model.StartDate = ChangeTimeZoneUtil.ResetTimeZone(model.StartDate);
                model.EndDate = ChangeTimeZoneUtil.ResetTimeZone(model.EndDate);
                model.Year = model.StartDate.Value.Year;

                string promoPatchDateCheckMsg = PromoDateCheck(oldModel); 
                if (promoPatchDateCheckMsg != null)
                {
                    return InternalServerError(new Exception(promoPatchDateCheckMsg));
                }

                //Проверка пересечения по времени на клиенте
                if (!DateCheck(model)) {
                    string msg = "There can not be two COGS of client and Brand Tech in some Time";
                    return InternalServerError(new Exception(msg)); //Json(new { success = false, message = msg });
                }
                if (model.StartDate.Value.Year != model.EndDate.Value.Year)
                {
                    string msg = "Start and End date must be in same year";
                    return InternalServerError(new Exception(msg));
                }

                Context.Set<ChangesIncident>().Add(new ChangesIncident
                {
                    Id = Guid.NewGuid(),
                    DirectoryName = nameof(COGS),
                    ItemId = model.Id.ToString(),
                    CreateDate = DateTimeOffset.Now,
                    Disabled = false
                });

                Context.SaveChanges();

                return Updated(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityExists(key))
                {
                    return NotFound();
                }
                else {
                    throw;
                }
            }
            catch (Exception e) {
                return GetErorrRequest(e);
            }
        }

        [ClaimsAuthorize]
        public IHttpActionResult Delete([FromODataUri] System.Guid key) {
            try {
                var model = Context.Set<COGS>().Find(key);
                if (model == null) {
                    return NotFound();
                }

                model.DeletedDate = System.DateTime.Now;
                model.Disabled = true;

                string promoDeleteDateCheckMsg = PromoDateCheck(model); 
                if (promoDeleteDateCheckMsg != null)
                {
                    return InternalServerError(new Exception(promoDeleteDateCheckMsg));
                }

                Context.SaveChanges();

                return StatusCode(HttpStatusCode.NoContent);
            } catch (Exception e) {
                return InternalServerError(e.InnerException);
            }
        }

        private bool EntityExists(System.Guid key) {
            return Context.Set<COGS>().Count(e => e.Id == key) > 0;
        }

        private IEnumerable<Column> GetExportSettings() {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 0, Field = "StartDate", Header = "StartDate", Quoting = false, Format = "dd.MM.yyyy"  },
                new Column() { Order = 1, Field = "EndDate", Header = "EndDate", Quoting = false, Format = "dd.MM.yyyy"  },
                new Column() { Order = 2, Field = "ClientTree.FullPathName", Header = "Client", Quoting = false },
                new Column() { Order = 3, Field = "ClientTree.ObjectId", Header = "ClientId", Quoting = false },
                new Column() { Order = 4, Field = "BrandTech.Name", Header = "BrandTech", Quoting = false },
                new Column() { Order = 5, Field = "LVSpercent", Header = "LSVpercent", Quoting = false }
            };
            return columns;
        }
        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<COGS> options) {
            try {
                IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));
                IEnumerable<Column> columns = GetExportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                UserInfo user = authorizationManager.GetCurrentUser();
                string username = user == null ? "" : user.Login;
                string filePath = exporter.GetExportFileName("COGS", username);
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

                NameValueCollection form = System.Web.HttpContext.Current.Request.Form;
                CreateImportTask(fileName, "FullXLSXCOGSUpdateImporHandler", form);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StringContent("success = true");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

                return result;
            } catch (Exception e) {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private void CreateImportTask(string fileName, string importHandler, NameValueCollection paramForm)
        {
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

                // параметры импорта
                HandlerDataHelper.SaveIncomingArgument("CrossParam.Year", paramForm.GetStringValue("year"), data, throwIfNotExists: false);

                HandlerDataHelper.SaveIncomingArgument("File", file, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportCOGS), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportCOGS).Name, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(ImportCOGS), data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler() {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Загрузка импорта из файла " + typeof(ImportCOGS).Name,
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

                string templateDir = AppSettingsManager.GetSetting("TEMPLATE_DIRECTORY", "Templates");
                string templateFilePath = Path.Combine(templateDir, "COGSPreTemplate.xlsx");
                using (FileStream templateStream = new FileStream(templateFilePath, FileMode.Open, FileAccess.Read)) {
                    IWorkbook twb = new XSSFWorkbook(templateStream);

                    string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
                    string filename = string.Format("{0}Template.xlsx", "COGS");
                    if (!Directory.Exists(exportDir)) {
                        Directory.CreateDirectory(exportDir);
                    }
                    string filePath = Path.Combine(exportDir, filename);
                    string file = Path.GetFileName(filePath);

                    DateTime dt = DateTime.Now;
                    List<ClientTree> clientsList = Context.Set<ClientTree>().Where(x => x.Type == "root"
                    || (DateTime.Compare(x.StartDate, dt) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dt) > 0))).ToList();

                    List<BrandTech> brandtechs = Context.Set<BrandTech>().Where(x => !x.Disabled).ToList();

                    using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write)) {
                        ISheet sheet2 = twb.GetSheet("Лист2");
                        ICreationHelper cH = twb.GetCreationHelper();

                        int i = 0;
                        foreach (ClientTree ct in clientsList) {
                            IRow clientRow = sheet2.CreateRow(i);
                            ICell hcell = clientRow.CreateCell(0);
                            hcell.SetCellValue(ct.FullPathName);

                            ICell idCell = clientRow.CreateCell(1);
                            idCell.SetCellValue(ct.ObjectId);
                            i++;
                        }

                        ISheet sheet3 = twb.GetSheet("Лист3");
                        i = 1;
                        foreach (BrandTech bt in brandtechs) {
                            IRow clientRow = sheet3.CreateRow(i);
                            ICell hcell = clientRow.CreateCell(0);
                            hcell.SetCellValue(bt.Name);
                            i++;
                        }
                        sheet2.AutoSizeColumn(0);
                        sheet2.AutoSizeColumn(1);
                        sheet3.AutoSizeColumn(0);

                        twb.Write(stream);
                        stream.Close();
                    }
                    return Content(HttpStatusCode.OK, file);
                }
            } catch (Exception e) {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }

        }


        private ExceptionResult GetErorrRequest(Exception e) {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601)) {
                return InternalServerError(new Exception("This COGS has already existed"));
            } else {
                return InternalServerError(e.InnerException);
            }
        }

        public bool DateCheck(COGS toCheck) {
            List<COGS> clientCOGSs = GetConstraintedQuery().Where(y => y.ClientTreeId == toCheck.ClientTreeId && y.BrandTechId == toCheck.BrandTechId && y.Id != toCheck.Id && !y.Disabled).ToList();
            foreach (COGS item in clientCOGSs) {
                if ((item.StartDate <= toCheck.StartDate && item.EndDate >= toCheck.StartDate) ||
                    (item.StartDate <= toCheck.EndDate && item.EndDate >= toCheck.EndDate) ||
                    (item.StartDate >= toCheck.StartDate && item.EndDate <= toCheck.EndDate)) {
                    return false;
                }
            }
            return true;
        }

        public string PromoDateCheck(COGS model)
        {
            //Получаем все дочерние ClientTreeId 
            var currClientTree = Context.Set<ClientTree>().Where(x => !x.EndDate.HasValue && x.Id == model.ClientTreeId).FirstOrDefault();
            var clientTreesIds = Helper.getClientTreeAllChildren(currClientTree.ObjectId, Context).Select(x => x.Id).ToList();
            clientTreesIds.Add(currClientTree.Id);

            List<int> parentIds = new List<int>();
            while (currClientTree.Type != "root" && currClientTree.parentId != 5000000)
            {
                currClientTree = Context.Set<ClientTree>().Where(x => !x.EndDate.HasValue && x.ObjectId == currClientTree.parentId).FirstOrDefault();
                parentIds.Add(currClientTree.Id);
            };

            //Получаем только те промо, которые могли быть привязаны к изменяемому COGS
            var promoesToCheck = this.GetPromoesForCheck(Context, model, clientTreesIds);

            //Забираем COGS только для соответствующего дерева клиентов
            var models = Context.Set<COGS>().Where(x => x.Id != model.Id && !x.Disabled && (parentIds.Contains(x.ClientTreeId) || clientTreesIds.Contains(x.ClientTreeId)));

            //Соединяем, для получения полного дерева
            clientTreesIds.AddRange(parentIds);

            var invalidPromoesByCOGS = this.GetInvalidPromoesByCOGS(models, promoesToCheck, clientTreesIds);
            if (invalidPromoesByCOGS.Any())
            {
                return $"Promo with numbers {string.Join(", ", invalidPromoesByCOGS)} will not have COGS after that.";
            }

            return null;
        }

        private IEnumerable<PromoSimpleCOGS> GetPromoesForCheck(DatabaseContext databaseContext, COGS model, List<int> ClientTreeIds)
        {
            var settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
            var statusesSetting = settingsManager.GetSetting<string>("NOT_CHECK_PROMO_STATUS_LIST", "Draft,Cancelled,Deleted,Closed");
            var notCheckPromoStatuses = statusesSetting.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            //Получаем промо, подходящие под этот COGS
            var promoes = databaseContext.Set<Promo>()
            .Where(x => !x.Disabled && x.ClientTreeId.HasValue && ClientTreeIds.Contains((int)x.ClientTreeKeyId)
                && model.StartDate.HasValue && model.EndDate.HasValue && x.DispatchesStart.HasValue
                && DateTimeOffset.Compare(model.StartDate.Value, x.DispatchesStart.Value) <= 0
                && DateTimeOffset.Compare(model.EndDate.Value, x.DispatchesStart.Value) >= 0
                && !notCheckPromoStatuses.Contains(x.PromoStatus.Name))
            .Select(x => new PromoSimpleCOGS
            {
                PromoStatusName = x.PromoStatus.Name,
                DispatchesStart = x.DispatchesStart,
                DispatchesEnd = x.DispatchesEnd,
                ClientTreeId = x.ClientTree.Id,
                BrandTechId = x.BrandTechId,
                Number = x.Number
            })
            .ToList();

            return promoes;
        }

        private IEnumerable<int?> GetInvalidPromoesByCOGS(
            IEnumerable<COGS> models, IEnumerable<PromoSimpleCOGS> promoes, List<int> clientTreeIds)
        {
            IEnumerable<PromoSimpleCOGS> invalidPromoes = new List<PromoSimpleCOGS>();

            //Группируем промо, что бы проверять только родительские ClientTree
            var promoGroups = promoes.GroupBy(x=>x.ClientTreeId);
            //Получаем всё дерево клиентов
            List<ClientTree> clientTrees = Context.Set<ClientTree>().Where(x => clientTreeIds.Contains(x.Id)).ToList();
            ClientTree clientTree;
            List<PromoSimpleCOGS> promoesToRemove;
            List<PromoSimpleCOGS> promoesGroupList;
            List<COGS> ClientCOGS;
            IEnumerable<COGS> FittedCOGS;
            List<int?> invalidPromoesNumbers = new List<int?>();

            foreach (var promoGroup in promoGroups)
            {
                promoesGroupList = promoGroup.ToList();
                promoesToRemove = new List<PromoSimpleCOGS>();
                clientTree = clientTrees.Where(x => x.Id == promoGroup.Key).FirstOrDefault();
                if (clientTree != null)
                {
                    while (clientTree != null && promoesGroupList.Count > 0)
                    {
                        ClientCOGS = models.Where(x => x.ClientTreeId == clientTree.Id).ToList();
                        foreach (PromoSimpleCOGS promo in promoesGroupList)
                        {
                            if (ClientCOGS != null)
                            {
                                FittedCOGS = ClientCOGS.Where(x => DateTimeOffset.Compare(x.StartDate.Value, promo.DispatchesStart.Value) <= 0
                                       && DateTimeOffset.Compare(x.EndDate.Value, promo.DispatchesStart.Value) >= 0
                                       && (promo.BrandTechId == x.BrandTechId || x.BrandTechId == null));
                                if (FittedCOGS.Any())
                                {
                                    promoesToRemove.Add(promo);
                                }
                            }
                        }
                        promoesGroupList = promoesGroupList.Except(promoesToRemove).ToList();
                        clientTree = clientTrees.Where(x => x.ObjectId == clientTree.parentId).FirstOrDefault();
                    }
                    if (promoesGroupList.Count > 0)
                    {
                        invalidPromoesNumbers.AddRange(promoesGroupList.Select(x=>x.Number));
                    }
                }
            }
            return invalidPromoesNumbers;
        }

        public class PromoSimpleCOGS
        {
            public int? Number { get; set; }
            public DateTimeOffset? DispatchesStart { get; set; }
            public DateTimeOffset? DispatchesEnd { get; set; }
            public int? ClientTreeId { get; set; }
            public int? ClientTreeObjectId { get; set; }
            public Guid? BrandTechId { get; set; }
            public string BrandTechName { get; set; }
            public string PromoStatusName { get; set; }
        }
    }
}