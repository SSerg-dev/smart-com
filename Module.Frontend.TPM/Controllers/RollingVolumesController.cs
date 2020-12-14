using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.Results;
using Core.Dependency;
using Core.Security;
using Core.Security.Models;
using Core.Settings;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Newtonsoft.Json;
using Persist;
using Persist.Model;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers
{
    public class RollingVolumesController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public RollingVolumesController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<RollingVolume> GetConstraintedQuery()
        {
            IQueryable<RollingVolume> query = Context.Set<RollingVolume>();

            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<RollingVolume> GetCategory([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<RollingVolume> GetRollingVolumes()
        {
            return GetConstraintedQuery();
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<RollingVolume> GetFilteredData(ODataQueryOptions<RollingVolume> options)
        {
            var query = GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<RollingVolume>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<RollingVolume>;
        }

        [ClaimsAuthorize]
        public IHttpActionResult Put([FromODataUri] System.Guid key, Delta<RollingVolume> patch)
        {
            var model = Context.Set<RollingVolume>().Find(key);
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
        public async Task<HttpResponseMessage> FullImportXLSX()
        {
            var currentDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
            var availableDay = DayOfWeek();
            if (currentDate.DayOfWeek != ((DayOfWeek)availableDay) && availableDay < 8)
            {
                string msg;
                if (availableDay == -1)
                {
                    msg = "Import is not available now";
                }
                else
                {
                    msg = String.Format("Import is only available on {0}", (DayOfWeek)availableDay);
                }

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.BadRequest);
                result.Content = new StringContent(msg);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                return result;
            }
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                string importDir = Core.Settings.AppSettingsManager.GetSetting("IMPORT_DIRECTORY", "ImportFiles");
                string fileName = await FileUtility.UploadFile(Request, importDir);

                CreateImportTask(fileName, "FullXLSXUpdateImportRollingVolumesHandler");

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
        [ClaimsAuthorize]
        public IHttpActionResult IsRollingVolumeDay()
        {
            var settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
            int dayOfWeek = settingsManager.GetSetting<int>("DAY_OF_WEEK_Rolling_Volume", 99);
            dayOfWeek = dayOfWeek == 7 ? 0 : dayOfWeek;
            bool isAvailable;
            if (dayOfWeek > 7)
            {
                isAvailable = true;
            }
            else
            {
             isAvailable = DateTimeOffset.UtcNow.DayOfWeek == (DayOfWeek)dayOfWeek;
            }
            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new
            {
                success = true,
                isDayOfWeek = isAvailable
            }));
        }
        private void CreateImportTask(string fileName, string importHandler)
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
                HandlerDataHelper.SaveIncomingArgument("File", file, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportRollingVolume), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportRollingVolume).Name, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(RollingVolume), data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Загрузка импорта из файла " + typeof(ImportRollingVolume).Name,
                    Name = "Module.Host.TPM.Handlers." + importHandler,
                    ExecutionPeriod = null,
                    RunGroup = typeof(ImportRollingVolume).Name,
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
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<RollingVolume> patch)
        {
            try
            {
                var model = Context.Set<RollingVolume>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                patch.Patch(model);
                Context.SaveChanges();

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
        public static IEnumerable<Column> GetExportSettings()
        {
            var order = 0;
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = order++, Field = "Product.ZREP", Header = "ZREP", Quoting = false },
                new Column() { Order = order++, Field = "Product.ProductEN", Header = "SKU", Quoting = false },
                new Column() { Order = order++, Field = "Product.BrandsegTechsub", Header = "BrandTech", Quoting = false },
                new Column() { Order = order++, Field = "DemandGroup", Header = "DemandGroup", Quoting = false }, 
                new Column() { Order = order++, Field = "Week", Header = "Week", Quoting = false }, 
                new Column() { Order = order++, Field = "PlanProductIncrementalQTY", Header = "PlanProductIncrementalQTY", Quoting = false,Format = "0.00" }, 
                new Column() { Order = order++, Field = "Actuals", Header = "Actuals", Quoting = false,Format = "0.00" }, 
                new Column() { Order = order++, Field = "OpenOrders", Header = "OpenOrders", Quoting = false,Format = "0.00" }, 
                new Column() { Order = order++, Field = "ActualOO", Header = "ACTL+OO", Quoting = false,Format = "0.00" }, 
                new Column() { Order = order++, Field = "Baseline", Header = "Baseline", Quoting = false ,Format = "0.00"}, 
                new Column() { Order = order++, Field = "ActualIncremental", Header = "ActualIncremental", Quoting = false,Format = "0.00" }, 
                new Column() { Order = order++, Field = "PreliminaryRollingVolumes", Header = "PreliminaryRollingVolumes", Quoting = false,Format = "0.00" }, 
                new Column() { Order = order++, Field = "PreviousRollingVolumes", Header = "PreviousRollingVolumes", Quoting = false,Format = "0.00" }, 
                new Column() { Order = order++, Field = "PromoDifference", Header = "PromoDifference", Quoting = false,Format = "0.00" }, 
                new Column() { Order = order++, Field = "RollingVolumesCorrection", Header = "RollingVolumesCorrection", Quoting = false ,Format = "0.00"},
                new Column() { Order = order++, Field = "RollingVolumesTotal", Header = "RollingVolumesTotal", Quoting = false ,Format = "0.00"},
                new Column() { Order = order++, Field = "ManualRollingTotalVolumes", Header = "ManualRollingTotalVolumes", Quoting = false ,Format = "0.00"},
                new Column() { Order = order++, Field = "FullWeekDiff", Header = "FullWeekDiff", Quoting = false ,Format = "0.00"}
            };
            return columns;
        }
        private bool EntityExists(System.Guid key)
        {
            return Context.Set<RollingVolume>().Count(e => e.Id == key) > 0;
        }

        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<RollingVolume> options)
        {
            IQueryable results = options.ApplyTo(GetConstraintedQuery());
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
                HandlerDataHelper.SaveIncomingArgument("TModel", typeof(RollingVolume), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(RollingVolumesController), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(RollingVolumesController.GetExportSettings), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = $"Export {nameof(RollingVolume)} dictionary",
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

        private int DayOfWeek()
        {
            var settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
            int dayOfWeek = settingsManager.GetSetting<int>("DAY_OF_WEEK_Rolling_Volume", 99);
            dayOfWeek = dayOfWeek < 0 ? -1 : dayOfWeek;
            dayOfWeek = dayOfWeek == 7 ? 0 : dayOfWeek;
            return dayOfWeek;
        }

        private ExceptionResult GetErorrRequest(Exception e)
        {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
            {
                return InternalServerError(new Exception("This Rolling Volume has already existed"));
            }
            else
            {
                return InternalServerError(e.InnerException);
            }
        }
    }
}
