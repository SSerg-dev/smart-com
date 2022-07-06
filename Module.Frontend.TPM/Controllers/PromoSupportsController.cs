using AutoMapper;
using Core.Extensions;
using Core.Security;
using Core.Security.Models;
using Core.Settings;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Newtonsoft.Json;
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
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.Results;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;

namespace Module.Frontend.TPM.Controllers
{
    public class PromoSupportsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public PromoSupportsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<PromoSupport> GetConstraintedQuery()
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();

            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<PromoSupport> query = Context.Set<PromoSupport>().Where(e => !e.Disabled);
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();

            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<PromoSupport> GetPromoSupport([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<PromoSupport> GetPromoSupports()
        {
            return GetConstraintedQuery();
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<PromoSupport> GetFilteredData(ODataQueryOptions<PromoSupport> options)
        {
            var query = GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<PromoSupport>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<PromoSupport>;
        }

        [ClaimsAuthorize]
        public IHttpActionResult Put([FromODataUri] System.Guid key, Delta<PromoSupport> patch)
        {
            var model = Context.Set<PromoSupport>().Find(key);
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
        public IHttpActionResult Post(PromoSupport model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // делаем UTC +3
            model.StartDate = ChangeTimeZoneUtil.ResetTimeZone(model.StartDate);
            model.EndDate = ChangeTimeZoneUtil.ResetTimeZone(model.EndDate);

            var proxy = Context.Set<PromoSupport>().Create<PromoSupport>();
            var result = (PromoSupport)Mapper.Map(model, proxy, typeof(PromoSupport), proxy.GetType(), opts => opts.CreateMissingTypeMaps = true);
            Context.Set<PromoSupport>().Add(result);

            try
            {
                Context.SaveChanges();
                if (!String.IsNullOrEmpty(result.AttachFileName))
                {
                    CreateImportDMPTask(result);
                }
            }
            catch (Exception e)
            {
                return GetErorrRequest(e);
            }

            return Created(result);
        }

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<PromoSupport> patch)
        {
            try
            {
                var model = Context.Set<PromoSupport>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                patch.Patch(model);
                // делаем UTC +3
                model.StartDate = ChangeTimeZoneUtil.ResetTimeZone(model.StartDate);
                model.EndDate = ChangeTimeZoneUtil.ResetTimeZone(model.EndDate);

                var promoSupportPromoes = Context.Set<PromoSupportPromo>().Where(x => !x.Disabled && x.PromoSupportId == model.Id).Select(x => x.PromoId).ToList();
                var blockedPromoSupportPromoes = Context.Set<BlockedPromo>().Where(x => !x.Disabled && promoSupportPromoes.Contains(x.PromoId));

                if (blockedPromoSupportPromoes.Count() == 0)
                {
                    Context.SaveChanges();

                    if (patch.GetChangedPropertyNames().Contains("AttachFileName"))
                    {
                        if (!String.IsNullOrEmpty(model.AttachFileName))
                        {
                            CreateImportDMPTask(model);
                        }
                        else
                        {
                            RemoveOldDMPRecords(model.Id);
                        }
                    }
                    return Updated(model);
                }
                else
                {
                    return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, message = "Unable to save PromoSupport. Some of PromoSupport Promoes are blocked." }));
                }
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
                var model = Context.Set<PromoSupport>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                model.DeletedDate = System.DateTime.Now;
                model.Disabled = true;
                Context.SaveChanges();

                List<Guid> promoIdsToRecalculate = new List<Guid>();

                IQueryable<PromoSupportPromo> pspQuery = Context.Set<PromoSupportPromo>().Where(x => x.PromoSupportId == model.Id && !x.Disabled);
                foreach (var psp in pspQuery)
                {
                    psp.DeletedDate = System.DateTime.Now;
                    psp.Disabled = true;

                    promoIdsToRecalculate.Add(psp.PromoId);
                }

                IQueryable<Promo> promos = Context.Set<Promo>().Where(x => promoIdsToRecalculate.Any(y => x.Id == y));
                if (promos.Any(x => x.PromoStatus.Name == "Closed"))
                    return InternalServerError(new Exception("Cannot be deleted due to closed promo"));

                CalculateBudgetsCreateTask(new List<Guid>() { key }, promoIdsToRecalculate);

                Context.SaveChanges();

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return InternalServerError(e.InnerException);
            }
        }

        private void RemoveOldDMPRecords(Guid promoSupportId)
        {
            var toRemove = Context.Set<PromoSupportDMP>().Where(x => x.PromoSupportId == promoSupportId);
            foreach (IEnumerable<PromoSupportDMP> items in toRemove.Partition(100))
            {
                Context.Set<PromoSupportDMP>().RemoveRange(items);
            }
            Context.SaveChanges();
        }

        private bool EntityExists(System.Guid key)
        {
            return Context.Set<PromoSupport>().Count(e => e.Id == key) > 0;
        }

        private void CalculateBudgetsCreateTask(List<Guid> promoSupportIds, List<Guid> unlinkedPromoIds = null)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            string promoSupportIdsString = FromListToString(promoSupportIds);
            string unlinkedPromoIdsString = FromListToString(unlinkedPromoIds);

            HandlerData data = new HandlerData();
            HandlerDataHelper.SaveIncomingArgument("PromoSupportIds", promoSupportIdsString, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UnlinkedPromoIds", unlinkedPromoIdsString, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);

            bool success = CalculationTaskManager.CreateCalculationTask(CalculationTaskManager.CalculationAction.Budgets, data, Context);

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

        public static IEnumerable<Column> GetExportSettingsTiCosts()
        {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 0, Field = "Number", Header = "ID", Quoting = false },
                new Column() { Order = 1, Field = "ClientTree.FullPathName", Header = "Client", Quoting = false },
                new Column() { Order = 2, Field = "BudgetSubItem.BudgetItem.Name", Header = "Support", Quoting = false },
                new Column() { Order = 3, Field = "BudgetSubItem.Name", Header = "Equipment Type", Quoting = false },
                new Column() { Order = 4, Field = "PlanQuantity", Header = "Plan Quantity", Quoting = false },
                new Column() { Order = 5, Field = "ActualQuantity", Header = "Actual Quantity", Quoting = false },
                new Column() { Order = 6, Field = "PlanCostTE", Header = "Plan Cost TE Total", Quoting = false },
                new Column() { Order = 7, Field = "ActualCostTE", Header = "Actual Cost TE Total", Quoting = false },
                new Column() { Order = 8, Field = "StartDate", Header = "Start Date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 9, Field = "EndDate", Header = "End Date", Quoting = false, Format = "dd.MM.yyyy" },
            };
            return columns;
        }

        public static IEnumerable<Column> GetExportSettingsCostProduction()
        {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 0, Field = "Number", Header = "ID", Quoting = false },
                new Column() { Order = 1, Field = "ClientTree.FullPathName", Header = "Client", Quoting = false },
                new Column() { Order = 2, Field = "BudgetSubItem.BudgetItem.Name", Header = "Support", Quoting = false },
                new Column() { Order = 3, Field = "BudgetSubItem.Name", Header = "Equipment Type", Quoting = false },
                new Column() { Order = 4, Field = "PlanQuantity", Header = "Plan Quantity", Quoting = false },
                new Column() { Order = 5, Field = "ActualQuantity", Header = "Actual Quantity", Quoting = false },
                new Column() { Order = 6, Field = "StartDate", Header = "Start Date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 7, Field = "EndDate", Header = "End Date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 8, Field = "PlanProdCostPer1Item", Header = "Plan Prod Cost Per 1 Item", Quoting = false },
                new Column() { Order = 9, Field = "ActualProdCostPer1Item", Header = "Actual Prod Cost Per 1 Item", Quoting = false },
                new Column() { Order = 10, Field = "PlanProdCost", Header = "Plan Prod Cost", Quoting = false },
                new Column() { Order = 11, Field = "ActualProdCost", Header = "Actual Prod Cost", Quoting = false },
            };
            return columns;
        }

        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<PromoSupport> options, string section)
        {
            IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));
            string getColumnMethod = section == "ticosts"
                                        ? nameof(PromoSupportsController.GetExportSettingsTiCosts)
                                        : nameof(PromoSupportsController.GetExportSettingsCostProduction);
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
                HandlerDataHelper.SaveIncomingArgument("TModel", typeof(PromoSupport), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(PromoSupportsController), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", getColumnMethod, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = $"Export {nameof(PromoSupport)} dictionary",
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

        private void CreateImportDMPTask(PromoSupport model)
        {
            var importHandler = "FullXLSXImportPromoDMPHandler";
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
                    Name = System.IO.Path.GetFileName(model.AttachFileName),
                    DisplayName = System.IO.Path.GetFileName(model.AttachFileName)
                };

                // параметры импорта
                HandlerDataHelper.SaveIncomingArgument("PlanQuantity", model.PlanQuantity.ToString(), data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("PromoSupportId", model.Id.ToString(), data, throwIfNotExists: false);

                HandlerDataHelper.SaveIncomingArgument("File", file, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportPromoDMP), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportPromoDMP).Name, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(ImportPromoDMP), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UniqueFields", new List<String>() { "Name" }, data);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Загрузка импорта из файла " + typeof(PromoSupport).Name,
                    Name = "Module.Host.TPM.Handlers." + importHandler,
                    ExecutionPeriod = null,
                    RunGroup = typeof(PromoSupport).Name,
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

        private ExceptionResult GetErorrRequest(Exception e)
        {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
            {
                return InternalServerError(new Exception("This PromoSupport has already existed"));
            }
            else
            {
                return InternalServerError(e.InnerException);
            }
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult GetUserTimestamp()
        {
            try
            {
                string timeFormat = String.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now);
                UserInfo user = authorizationManager.GetCurrentUser();
                string userTimestamp = (user.Login.Split('\\').Last() + timeFormat).Replace(" ", "");
                return Json(new { success = true, userTimestamp });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult GetPromoSupportGroup([FromODataUri] Guid promoSupportId)
        {
            try
            {
                PromoSupport ps = Context.Set<PromoSupport>().FirstOrDefault(x => x.Id == promoSupportId && !x.Disabled);
                IQueryable<PromoSupport> promoSupports = Context.Set<PromoSupport>().Where(x => x.UserTimestamp == ps.UserTimestamp && x.UserTimestamp != null && !x.Disabled);
                List<PromoSupportGroup> promoSupportGroupList = new List<PromoSupportGroup>();

                foreach (PromoSupport promoSupport in promoSupports)
                {
                    List<PromoSupportPromo> promoSupportPromoes = Context.Set<PromoSupportPromo>().Where(x => x.PromoSupportId == promoSupport.Id && !x.Disabled).ToList();
                    promoSupportGroupList.Add(new PromoSupportGroup(promoSupport, promoSupportPromoes));
                }

                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, models = promoSupportGroupList }));
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false }));
            }
        }

        [ClaimsAuthorize]
        [HttpPost]
        public async Task<IHttpActionResult> UploadFile()
        {
            try
            {
                int maxFileByteLength = 25000000;

                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                if (Request.Content.Headers.ContentLength > maxFileByteLength)
                {
                    throw new FileLoadException("The file size must be less than 25mb.");
                }

                string directory = Core.Settings.AppSettingsManager.GetSetting("PROMO_SUPPORT_DIRECTORY", "PromoSupportFiles");
                string fileName = await FileUtility.UploadFile(Request, directory);

                return Json(new { success = true, fileName = fileName.Split('\\').Last() });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        [ClaimsAuthorize]
        [HttpGet]
        [Route("odata/PromoSupports/DownloadFile")]
        public HttpResponseMessage DownloadFile(string fileName)
        {
            try
            {
                string directory = Core.Settings.AppSettingsManager.GetSetting("PROMO_SUPPORT_DIRECTORY", "PromoSupportFiles");
                string type = Core.Settings.AppSettingsManager.GetSetting("HANDLER_LOG_TYPE", "File");
                HttpResponseMessage result;
                switch (type)
                {
                    case "File":
                        {
                            result = FileUtility.DownloadFile(directory, fileName);
                            break;
                        }
                    case "Azure":
                        {
                            result = FileUtility.DownloadFileAzure(directory, fileName);
                            break;
                        }
                    default:
                        {
                            result = FileUtility.DownloadFile(directory, fileName);
                            break;
                        }
                }
                return result;
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.Accepted);
            }
        }
    }

    public class PromoSupportGroup
    {
        public Guid PromoSupportId { get; set; }
        //public List<Guid> PromoLinkedIds { get; set; }
        public List<PromoSupportPromo> PromoSupportPromoes { get; set; }

        public PromoSupportGroup(PromoSupport promoSupport, List<PromoSupportPromo> promoSupportPromoes)
        {
            PromoSupportId = promoSupport.Id;
            PromoSupportPromoes = promoSupportPromoes;
        }
    }
}