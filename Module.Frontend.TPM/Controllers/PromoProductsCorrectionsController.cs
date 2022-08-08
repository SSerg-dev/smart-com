using AutoMapper;
using Core.Dependency;
using Core.Security;
using Core.Security.Models;
using Core.Settings;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.FunctionalHelpers.RSmode;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.Interfaces;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model;
using Persist.ScriptGenerator.Filter;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
    public class PromoProductsCorrectionsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public PromoProductsCorrectionsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }
        protected IQueryable<PromoProductsCorrection> GetConstraintedQuery(TPMmode TPMmode = TPMmode.Current)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            IQueryable<PromoProductsCorrection> query = Context.Set<PromoProductsCorrection>().Where(e => !e.Disabled && e.TempId == null);

            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, TPMmode, filters);

            return query;
        }
        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public SingleResult<PromoProductsCorrection> GetPromoProductCorrection([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<PromoProductsCorrection> GetPromoProductsCorrections(TPMmode TPMmode)
        {
            return GetConstraintedQuery(TPMmode);
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<PromoProductsCorrection> GetFilteredData(ODataQueryOptions<PromoProductsCorrection> options)
        {
            var bodyText = HttpContext.Current.Request.GetRequestBody();
            var query = JsonHelper.IsValueExists(bodyText, "TPMmode")
                 ? GetConstraintedQuery(JsonHelper.GetValueIfExists<TPMmode>(bodyText, "TPMmode"))
                 : GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<PromoProductsCorrection>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<PromoProductsCorrection>;
        }

        [ClaimsAuthorize]
        public IHttpActionResult Put([FromODataUri] System.Guid key, Delta<PromoProductsCorrection> patch)
        {
            var model = Context.Set<PromoProductsCorrection>().Find(key);
            if (model == null)
            {
                return NotFound();
            }

            patch.Put(model);

            try
            {
                var saveChangesResult = Context.SaveChanges();
                if (saveChangesResult > 0)
                {
                    CreateChangesIncident(Context.Set<ChangesIncident>(), model);
                    Context.SaveChanges();
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

            return Updated(model);
        }

        [ClaimsAuthorize]
        public IHttpActionResult Post(PromoProductsCorrection model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (model.TempId == "")
            {
                model.TempId = null;
            }
            UserInfo user = authorizationManager.GetCurrentUser();

            // если существует коррекция на данный PromoProduct, то не создаем новый объект
            var item = Context.Set<PromoProductsCorrection>()
                .FirstOrDefault(x => x.PromoProductId == model.PromoProductId && x.TempId == model.TempId && !x.Disabled);

            if (item != null)
            {
                if (item.PromoProduct.Promo.NeedRecountUplift == false && String.IsNullOrEmpty(item.TempId))
                {
                    return InternalServerError(new Exception("Promo Locked Update"));
                }
                item.PlanProductUpliftPercentCorrected = model.PlanProductUpliftPercentCorrected;
                item.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                item.UserId = user.Id;
                item.UserName = user.Login;

                try
                {
                    var saveChangesResult = Context.SaveChanges();
                    if (saveChangesResult > 0)
                    {
                        CreateChangesIncident(Context.Set<ChangesIncident>(), model);
                        Context.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    return GetErorrRequest(e);
                }

                return Created(model);
            }
            else
            {
                var proxy = Context.Set<PromoProductsCorrection>().Create<PromoProductsCorrection>();
                var configuration = new MapperConfiguration(cfg =>
                    cfg.CreateMap<PromoProductsCorrection, PromoProductsCorrection>().ReverseMap());
                var mapper = configuration.CreateMapper();
                var result = mapper.Map(model, proxy);
                var promoProduct = Context.Set<PromoProduct>()
                    .FirstOrDefault(x => x.Id == result.PromoProductId && !x.Disabled);

                if (promoProduct.Promo.NeedRecountUplift == false && String.IsNullOrEmpty(result.TempId))
                {
                    return InternalServerError(new Exception("Promo Locked Update"));
                }
                result.CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                result.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                result.UserId = user.Id;
                result.UserName = user.Login;

                Context.Set<PromoProductsCorrection>().Add(result);

                try
                {
                    var saveChangesResult = Context.SaveChanges();
                    if (saveChangesResult > 0)
                    {
                        CreateChangesIncident(Context.Set<ChangesIncident>(), result);
                        Context.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    return GetErorrRequest(e);
                }

                return Created(result);
            }
        }

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<PromoProductsCorrection> patch)
        {
            try
            {
                UserInfo user = authorizationManager.GetCurrentUser();
                var model = Context.Set<PromoProductsCorrection>().Find(key);
                string role = authorizationManager.GetCurrentRoleName();

                if (model == null)
                {
                    return NotFound();
                }
                var promoStatus = model.PromoProduct.Promo.PromoStatus.SystemName;

                patch.TryGetPropertyValue("TPMmode", out object mode);

                if ((int)model.TPMmode != (int)mode)
                {
                    model = RSmodeHelper.EditToPromoProductsCorrectionRS(Context, model);
                }

                patch.Patch(model);
                model.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                model.UserId = user.Id;
                model.UserName = user.Login;

                if (model.TempId == "")
                {
                    model.TempId = null;
                }
              

                ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                string promoStatuses = settingsManager.GetSetting<string>("PROMO_PRODUCT_CORRECTION_PROMO_STATUS_LIST", "Draft,Deleted,Cancelled,Started,Finished,Closed");
                string[] status = promoStatuses.Split(',');
                if (status.Any(x => x == promoStatus) && !role.Equals("SupportAdministrator"))
                    return InternalServerError(new Exception("Cannot be update correction where status promo = " + promoStatus));
                if (model.PromoProduct.Promo.NeedRecountUplift == false)
                {
                    return InternalServerError(new Exception("Promo Locked Update"));
                }

                var saveChangesResult = Context.SaveChanges();
                if (saveChangesResult > 0)
                {
                    CreateChangesIncident(Context.Set<ChangesIncident>(), model);
                    Context.SaveChanges();
                }

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
                var model = Context.Set<PromoProductsCorrection>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }
                if (model.PromoProduct.Promo.NeedRecountUplift == false)
                {
                    return InternalServerError(new Exception("Promo Locked Update"));
                }
                model.DeletedDate = System.DateTime.Now;
                model.Disabled = true;

                var saveChangesResult = Context.SaveChanges();
                if (saveChangesResult > 0)
                {
                    CreateChangesIncident(Context.Set<ChangesIncident>(), model);
                    Context.SaveChanges();
                }

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return InternalServerError(e.InnerException);
            }
        }
        private bool EntityExists(System.Guid key)
        {
            return Context.Set<PromoProductsCorrection>().Count(e => e.Id == key) > 0;
        }
        private ExceptionResult GetErorrRequest(Exception e)
        {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
            {
                return InternalServerError(new Exception("This PromoProductCorrection has already existed"));
            }
            else
            {
                return InternalServerError(e);
            }
        }
        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<PromoProductsCorrection> options)
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
                HandlerDataHelper.SaveIncomingArgument("TModel", typeof(PromoProductsCorrection), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(PromoProductsCorrectionsController), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(PromoProductsCorrectionsController.GetPromoProductCorrectionExportSettings), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = $"Export {nameof(PromoProductsCorrection)} dictionary",
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

        public static IEnumerable<Column> GetPromoProductCorrectionExportSettings()
        {
            int orderNumber = 1;
            IEnumerable<Column> columns = new List<Column>
            {
                 new Column { Order = orderNumber++, Field = "PromoProduct.Promo.Number", Header = "Promo ID", Quoting = false,  Format = "0" },
                 new Column { Order = orderNumber++, Field = "PromoProduct.ZREP", Header = "ZREP", Quoting = false,  Format = "0" },
                 new Column { Order = orderNumber++, Field = "PlanProductUpliftPercentCorrected", Header = "Plan Product Uplift Percent Corrected", Quoting = false,  Format = "0.00"  },
                 new Column { Order = orderNumber++, Field = "PromoProduct.Promo.ClientHierarchy", Header = "Client", Quoting = false },
                 new Column { Order = orderNumber++, Field = "PromoProduct.Promo.BrandTech.Name", Header = "BrandTech", Quoting = false },
                 new Column { Order = orderNumber++, Field = "PromoProduct.Promo.ProductSubrangesList", Header = "Subranges", Quoting = false },
                 new Column { Order = orderNumber++, Field = "PromoProduct.Promo.Mechanic", Header = "Mars Mechanic", Quoting = false },
                 new Column { Order = orderNumber++, Field = "PromoProduct.Promo.Event.Name", Header = "Event", Quoting = false },
                 new Column { Order = orderNumber++, Field = "PromoProduct.Promo.PromoStatus.SystemName", Header = "Promo Status", Quoting = false },
                 new Column { Order = orderNumber++, Field = "PromoProduct.Promo.MarsStartDate", Header = "Mars Start Date", Quoting = false },
                 new Column { Order = orderNumber++, Field = "PromoProduct.Promo.MarsEndDate", Header = "Mars End Date", Quoting = false },
                 new Column { Order = orderNumber++, Field = "PromoProduct.PlanProductBaselineLSV", Header = "Plan Product Baseline LSV", Quoting = false,  Format = "0.00" },
                 new Column { Order = orderNumber++, Field = "PromoProduct.PlanProductIncrementalLSV", Header = "Plan Product Incremental LSV", Quoting = false,  Format = "0.00" },
                 new Column { Order = orderNumber++, Field = "PromoProduct.PlanProductLSV", Header = "Plan Product LSV", Quoting = false,  Format = "0.00" },
                 new Column { Order = orderNumber++, Field = "CreateDate", Header = "CreateDate", Quoting = false,Format = "dd.MM.yyyy"},
                 new Column { Order = orderNumber++, Field = "ChangeDate", Header = "ChangeDate", Quoting = false,Format = "dd.MM.yyyy"},
                 new Column { Order = orderNumber++, Field = "UserName", Header = "UserName", Quoting = false }

            };

            return columns;
        }
        [ClaimsAuthorize]
        public IHttpActionResult ExportCorrectionXLSX(ODataQueryOptions<PromoProductsCorrection> options)
        {
            List<string> stasuses = new List<string> { "DraftPublished", "OnApproval", "Approved", "Planned" };
            IQueryable<PromoProduct> results = Context.Set<PromoProduct>()
                .Where(g => !g.Disabled && stasuses.Contains(g.Promo.PromoStatus.SystemName) && !(bool)g.Promo.InOut && (bool)g.Promo.NeedRecountUplift)
                .OrderBy(g => g.Promo.Number).ThenBy(d => d.ZREP);
            //IQueryable results = options.ApplyTo(GetConstraintedQuery());
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
                HandlerDataHelper.SaveIncomingArgument("TModel", typeof(PromoProduct), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(PromoProductsCorrectionsController), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(PromoProductsCorrectionsController.GetExportCorrectionSettings), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = $"Export {nameof(PromoProductsCorrection)} dictionary",
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

        public static IEnumerable<Column> GetExportCorrectionSettings()
        {
            int orderNumber = 1;
            IEnumerable<Column> columns = new List<Column>()
            {
                new Column { Order = orderNumber++, Field = "Promo.Number", Header = "Promo ID", Quoting = false,  Format = "0" },
                new Column { Order = orderNumber++, Field = "ZREP", Header = "ZREP", Quoting = false,  Format = "0" },
                new Column { Order = orderNumber++, Field = "PlanProductUpliftPercent??????", Header = "Plan Product Uplift, %", Quoting = false,  Format = "0.00"},
            };
            return columns;
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

                var importDir = AppSettingsManager.GetSetting("IMPORT_DIRECTORY", "ImportFiles");
                var fileName = await FileUtility.UploadFile(Request, importDir);

                CreateImportTask(fileName, "FullXLSXUpdateImportPromoProductsCorrectionHandler");

                var result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StringContent("success = true");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

                return result;
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private void CreateImportTask(string fileName, string importHandler)
        {
            var userInfo = authorizationManager.GetCurrentUser();
            var userId = userInfo == null ? Guid.Empty : (userInfo.Id.HasValue ? userInfo.Id.Value : Guid.Empty);
            var role = authorizationManager.GetCurrentRole();
            var roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            using (var databaseContext = new DatabaseContext())
            {
                var resiltfile = new ImportResultFilesModel();
                var resultmodel = new ImportResultModel();

                var handlerData = new HandlerData();
                var fileModel = new FileModel()
                {
                    LogicType = "Import",
                    Name = Path.GetFileName(fileName),
                    DisplayName = Path.GetFileName(fileName)
                };

                HandlerDataHelper.SaveIncomingArgument("File", fileModel, handlerData, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UserId", userId, handlerData, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, handlerData, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportPromoProductsCorrection), handlerData, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportPromoProductsCorrection).Name, handlerData, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(PromoProductsCorrection), handlerData, visible: false, throwIfNotExists: false);
                //HandlerDataHelper.SaveIncomingArgument("UniqueFields", new List<String>() {"Name"}, handlerData);

                var loopHandler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Загрузка импорта из файла " + typeof(PromoProductsCorrection).Name,
                    Name = "Module.Host.TPM.Handlers." + importHandler,
                    ExecutionPeriod = null,
                    RunGroup = typeof(PromoProductsCorrection).Name,
                    CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                    LastExecutionDate = null,
                    NextExecutionDate = null,
                    ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                    UserId = userId,
                    RoleId = roleId
                };

                loopHandler.SetParameterData(handlerData);
                databaseContext.LoopHandlers.Add(loopHandler);
                databaseContext.SaveChanges();
            }
        }

        [ClaimsAuthorize]
        public IHttpActionResult DownloadTemplateXLSX()
        {
            try
            {
                IEnumerable<Column> columns = GetImportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
                string filename = string.Format("{0}Template.xlsx", "PromoProductsUplift");
                if (!Directory.Exists(exportDir))
                {
                    Directory.CreateDirectory(exportDir);
                }
                string filePath = Path.Combine(exportDir, filename);
                exporter.Export(Enumerable.Empty<PromoProductsCorrection>(), filePath);
                string file = Path.GetFileName(filePath);
                return Content(HttpStatusCode.OK, file);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }

        }

        private IEnumerable<Column> GetImportSettings()
        {
            int orderNumber = 1;
            IEnumerable<Column> columns = new List<Column>()
            {
                new Column { Order = orderNumber++, Field = "PromoNumber", Header = "Promo ID", Quoting = false,  Format = "0"  },
                new Column { Order = orderNumber++, Field = "ProductZREP", Header = "ZREP", Quoting = false,  Format = "0"  },
                new Column { Order = orderNumber++, Field = "PlanProductUpliftPercent", Header = "Plan Product Uplift, %", Quoting = false,  Format = "0.00"},
            };
            return columns;
        }

        public static void CreateChangesIncident(DbSet<ChangesIncident> changesIncidents, PromoProductsCorrection promoProductCorrection)
        {
            changesIncidents.Add(new ChangesIncident
            {
                Id = Guid.NewGuid(),
                DirectoryName = nameof(PromoProductsCorrection),
                ItemId = promoProductCorrection.Id.ToString(),
                CreateDate = DateTimeOffset.Now,
                ProcessDate = null,
                DeletedDate = null,
                Disabled = false
            });
        }
    }
}
