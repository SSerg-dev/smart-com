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
using Module.Persist.TPM.Model.SimpleModel;
using System.Web;
using Utility.FileWorker;

namespace Module.Frontend.TPM.Controllers
{
    public class RATIShoppersController : EFContextController {
        private readonly IAuthorizationManager authorizationManager;

        public RATIShoppersController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }


        protected IQueryable<RATIShopper> GetConstraintedQuery()
        {

            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();

            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<RATIShopper> query = Context.Set<RATIShopper>().Where(e => !e.Disabled);
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();

            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

            return query;
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<RATIShopper> GetRATIShopper([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<RATIShopper> GetRATIShoppers()
        {
            return GetConstraintedQuery();
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<RATIShopper> GetFilteredData(ODataQueryOptions<RATIShopper> options)
        {
            var query = GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<RATIShopper>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<RATIShopper>;
        }

        [ClaimsAuthorize]
        public IHttpActionResult Put([FromODataUri] System.Guid key, Delta<RATIShopper> patch)
        {
            var model = Context.Set<RATIShopper>().Find(key);
            if (model == null)
            {
                return NotFound();
            }
            patch.Put(model);
            try
            {
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(model);
        }

        [ClaimsAuthorize]
        public IHttpActionResult Post(RATIShopper model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var proxy = Context.Set<RATIShopper>().Create<RATIShopper>();
            var result = (RATIShopper)Mapper.Map(model, proxy, typeof(RATIShopper), proxy.GetType(), opts => opts.CreateMissingTypeMaps = true);

            //Проверка пересечения по времени на клиенте
            if (!DateCheck(result))
            {
                string msg = "There can not be two RA TI Shoppers of client in the same year";
                return InternalServerError(new Exception(msg));
            }


            Context.Set<RATIShopper>().Add(result);

            CreateRATIShopperChangeIncidents(result);

            try
            {
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                return GetErorrRequest(e);
            }

            return Created(model);
        }

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<RATIShopper> patch)
        {
            try
            {
                var model = Context.Set<RATIShopper>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                var oldModel = new RATIShopper
                {
                    Disabled = model.Disabled,
                    DeletedDate = model.DeletedDate,
                    RATIShopperPercent = model.RATIShopperPercent,
                    ClientTreeId = model.ClientTree.Id,
                    ClientTree = model.ClientTree,
                    Year = model.Year,
                    Id = model.Id
                };

                patch.Patch(model);

                var newModel = new RATIShopper
                {
                    Disabled = model.Disabled,
                    DeletedDate = model.DeletedDate,
                    RATIShopperPercent = model.RATIShopperPercent,
                    ClientTreeId = model.ClientTree.Id,
                    ClientTree = model.ClientTree,
                    Year = model.Year,
                    Id = model.Id
                };

                //Проверка пересечения по времени на клиенте
                if (!DateCheck(model))
                {
                    string msg = "There can not be two RA TI Shoppers of client in the same year";
                    return InternalServerError(new Exception(msg));
                }


                //???
                //Context.Set<ChangesIncident>().Add(new ChangesIncident
                //{
                //    Id = Guid.NewGuid(),
                //    DirectoryName = nameof(RATIShopper),
                //    ItemId = model.Id.ToString(),
                //    CreateDate = DateTimeOffset.Now,
                //    Disabled = false
                //});

                Context.SaveChanges();

                CreateRATIShopperChangeIncidents(oldModel);
                CreateRATIShopperChangeIncidents(newModel);

                return Updated(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception e)
            {
                return GetErorrRequest(e);
            }
        }

        [ClaimsAuthorize]
        public IHttpActionResult Delete([FromODataUri] System.Guid key)
        {
            try
            {
                var model = Context.Set<RATIShopper>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                CreateRATIShopperChangeIncidents(model);

                model.DeletedDate = System.DateTime.Now;
                model.Disabled = true;

                Context.SaveChanges();

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return InternalServerError(e.InnerException);
            }
        }

        private bool EntityExists(System.Guid key)
        {
            return Context.Set<RATIShopper>().Count(e => e.Id == key) > 0;
        }

        public static IEnumerable<Column> GetExportSettings()
        {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 1, Field = "ClientTree.FullPathName", Header = "Client", Quoting = false },
                new Column() { Order = 2, Field = "ClientTree.ObjectId", Header = "ClientId", Quoting = false },
                new Column() { Order = 3, Field = "Year", Header = "Year", Quoting = false },
                new Column() { Order = 4, Field = "RATIShopperPercent", Header = "RATIShopperPercent", Quoting = false }
            };
            return columns;
        }

        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<RATIShopper> options)
        {
            IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);
            using (DatabaseContext context = new DatabaseContext())
            {
                HandlerData data = new HandlerData();
                string handlerName = "ExportHandler";

                HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TModel", typeof(RATIShopper), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(RATIShoppersController), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(RATIShoppersController.GetExportSettings), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = $"Export {nameof(RATIShopper)} dictionary",
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
                context.LoopHandlers.Add(handler);
                context.SaveChanges();
            }

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

                NameValueCollection form = System.Web.HttpContext.Current.Request.Form;
                CreateImportTask(fileName, "FullXLSXRATIShopperUpdateImportHandler", form);

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

        private void CreateImportTask(string fileName, string importHandler, NameValueCollection paramForm)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            using (DatabaseContext context = new DatabaseContext())
            {
                ImportResultFilesModel resiltfile = new ImportResultFilesModel();
                ImportResultModel resultmodel = new ImportResultModel();

                HandlerData data = new HandlerData();
                FileModel file = new FileModel()
                {
                    LogicType = "Import",
                    Name = System.IO.Path.GetFileName(fileName),
                    DisplayName = System.IO.Path.GetFileName(fileName)
                };

                // параметры импорта
                HandlerDataHelper.SaveIncomingArgument("ImportDestination", "RATIShopper", data, throwIfNotExists: false);

                HandlerDataHelper.SaveIncomingArgument("File", file, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportRATIShopper), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportRATIShopper).Name, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(ImportRATIShopper), data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Загрузка импорта из файла " + typeof(ImportRATIShopper).Name,
                    Name = "Module.Host.TPM.Handlers." + importHandler,
                    ExecutionPeriod = null,
                    RunGroup = typeof(ImportRATIShopper).Name,
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
        public IHttpActionResult DownloadTemplateXLSX()
        {
            try
            {
                IEnumerable<Column> columns = GetExportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
                string filename = string.Format("{0}Template.xlsx", "RATIShopper");
                if (!Directory.Exists(exportDir))
                {
                    Directory.CreateDirectory(exportDir);
                }
                string filePath = Path.Combine(exportDir, filename);
                exporter.Export(Enumerable.Empty<RATIShopper>(), filePath);
                string file = Path.GetFileName(filePath);
                return Content(HttpStatusCode.OK, file);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }

        }


        private ExceptionResult GetErorrRequest(Exception e)
        {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
            {
                return InternalServerError(new Exception("This RA TI Shopper has already existed"));
            }
            else
            {
                return InternalServerError(e.InnerException);
            }
        }

        public bool DateCheck(RATIShopper toCheck)
        {
            return GetConstraintedQuery().Count(y => y.ClientTreeId == toCheck.ClientTreeId && y.Year == toCheck.Year && y.Id != toCheck.Id && !y.Disabled) == 0;
        }

        public string PromoDateCheck(RATIShopper model, RATIShopper newModel)
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

            //Получаем только те промо, которые могли быть привязаны к изменяемому RA TI Shopper
            var promoesToCheck = this.GetPromoesForCheck(Context, model, clientTreesIds);

            //Забираем RA TI Shopper только для соответствующего дерева клиентов
            var models = Context.Set<RATIShopper>().Where(x => x.Id != model.Id && !x.Disabled && (parentIds.Contains(x.ClientTreeId) || clientTreesIds.Contains(x.ClientTreeId))).ToList();

            if (newModel != null)
            {
                models.Add(newModel);
            }

            //Соединяем, для получения полного дерева
            clientTreesIds.AddRange(parentIds);

            var invalidPromoesByRATIShopper = this.GetInvalidPromoesByRATIShopper(models, promoesToCheck, clientTreesIds);
            if (invalidPromoesByRATIShopper.Any())
            {
                return $"Promo with numbers {string.Join(", ", invalidPromoesByRATIShopper)} will not have RATIShopper after that.";
            }

            return null;
        }

        private IEnumerable<SimplePromoRATIShopper> GetPromoesForCheck(DatabaseContext databaseContext, RATIShopper model, List<int> ClientTreeIds)
        {
            var settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
            var statusesSetting = settingsManager.GetSetting<string>("NOT_CHECK_PROMO_STATUS_LIST", "Draft,Cancelled,Deleted,Closed");
            var notCheckPromoStatuses = statusesSetting.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            var query = databaseContext.Set<Promo>()
                .Where(x => !x.Disabled && x.ClientTreeId.HasValue && ClientTreeIds.Contains((int)x.ClientTreeKeyId)
                && model.Year == x.BudgetYear
                && !notCheckPromoStatuses.Contains(x.PromoStatus.Name));

            List<SimplePromoRATIShopper> promos = new List<SimplePromoRATIShopper>();
            foreach (var item in query)
            {
                promos.Add(new SimplePromoRATIShopper(item));
            }

            return promos;
        }

        private IEnumerable<int?> GetInvalidPromoesByRATIShopper(
            IEnumerable<RATIShopper> models, IEnumerable<SimplePromoRATIShopper> promoes, List<int> clientTreeIds)
        {
            IEnumerable<SimplePromoRATIShopper> invalidPromoes = new List<SimplePromoRATIShopper>();

            //Группируем промо, что бы проверять только родительские ClientTree
            var promoGroups = promoes.GroupBy(x => x.ClientTreeId);
            //Получаем всё дерево клиентов
            List<ClientTree> clientTrees = Context.Set<ClientTree>().Where(x => clientTreeIds.Contains(x.Id)).ToList();
            ClientTree clientTree;
            List<SimplePromoRATIShopper> promoesToRemove;
            List<SimplePromoRATIShopper> promoesGroupList;
            List<int?> invalidPromoesNumbers = new List<int?>();

            foreach (var promoGroup in promoGroups)
            {
                promoesGroupList = promoGroup.ToList();
                promoesToRemove = new List<SimplePromoRATIShopper>();
                clientTree = clientTrees.Where(x => x.Id == promoGroup.Key).FirstOrDefault();
                if (clientTree != null)
                {
                    while (clientTree != null && promoesGroupList.Count > 0)
                    {
                        promoesGroupList = promoesGroupList.Except(promoesToRemove).ToList();
                        clientTree = clientTrees.Where(x => x.ObjectId == clientTree.parentId).FirstOrDefault();
                    }
                    if (promoesGroupList.Count > 0)
                    {
                        invalidPromoesNumbers.AddRange(promoesGroupList.Select(x => x.Number));
                    }
                }
            }
            return invalidPromoesNumbers;
        }


        public void CreateRATIShopperChangeIncidents(RATIShopper model)
        {
            var settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
            var statusesSetting = settingsManager.GetSetting<string>("NOT_CHECK_PROMO_STATUS_LIST", "Draft,Cancelled,Deleted,Closed");
            var checkPromoStatuses = statusesSetting.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            var promoes = Context.Set<Promo>()
                .Where(x => !x.Disabled && x.BudgetYear == model.Year && x.ClientTreeId == model.ClientTree.ObjectId)
                .Where(x => !checkPromoStatuses.Contains(x.PromoStatus.Name))
                .Select(x => x.Id).ToList();
            
            foreach (var item in promoes)
            {
                var changesIncident = new ChangesIncident
                {
                    Id = Guid.NewGuid(),
                    DirectoryName = nameof(Promo) + nameof(RATIShopper),
                    ItemId = item.ToString(),
                    CreateDate = DateTimeOffset.Now,
                    Disabled = false
                };

                Context.Set<ChangesIncident>().Add(changesIncident);
            }

            //foreach (var cogs in actualCOGSForPrevYear)
            //{
            //    cogs.IsCOGSIncidentCreated = true;
            //}

            Context.SaveChanges();
        }
    }
}
