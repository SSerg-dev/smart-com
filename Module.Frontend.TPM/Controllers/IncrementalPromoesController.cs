using AutoMapper;
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
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.Interfaces;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
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
using System.Web.Http.Results;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;
using System.Data.Entity;

namespace Module.Frontend.TPM.Controllers
{
    public class IncrementalPromoesController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public IncrementalPromoesController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<IncrementalPromo> GetConstraintedQuery(TPMmode TPMmode = TPMmode.Current)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();

            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();

            IQueryable<IncrementalPromo> query = Context.Set<IncrementalPromo>().Where(e => !e.Disabled);

            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, TPMmode, filters);

            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<IncrementalPromo> GetIncrementalPromo([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<IncrementalPromo> GetIncrementalPromoes(TPMmode TPMmode)
        {
            return GetConstraintedQuery(TPMmode);
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<IncrementalPromo> GetFilteredData(ODataQueryOptions<IncrementalPromo> options)
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

            var optionsPost = new ODataQueryOptionsPost<IncrementalPromo>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<IncrementalPromo>;
        }

        [ClaimsAuthorize]
        public IHttpActionResult Put([FromODataUri] System.Guid key, Delta<IncrementalPromo> patch)
        {
            var model = Context.Set<IncrementalPromo>().Find(key);
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
        public IHttpActionResult Post(IncrementalPromo model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var proxy = Context.Set<IncrementalPromo>().Create<IncrementalPromo>();
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<IncrementalPromo, IncrementalPromo>().ReverseMap());
            var mapper = configuration.CreateMapper();
            var result = mapper.Map(model, proxy);
            Context.Set<IncrementalPromo>().Add(result);
            result.LastModifiedDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);

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
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<IncrementalPromo> patch)
        {
            try
            {
                IncrementalPromo ChangeIncrementalPromo = new IncrementalPromo();
                patch.CopyChangedValues(ChangeIncrementalPromo);

                var model = Context.Set<IncrementalPromo>()
                    .Include(x => x.Promo)
                    .FirstOrDefault(g => g.Id == key);
                if (model == null)
                {
                    return NotFound();
                }
                List<IncrementalPromo> ipromoes = Context.Set<Promo>()
                    .Include(x => x.IncrementalPromoes)
                    .Where(g => g.Number == model.Promo.Number)
                    .SelectMany(f => f.IncrementalPromoes)
                    .ToList();


                if (ChangeIncrementalPromo.TPMmode == TPMmode.RS)
                {
                    if (model.TPMmode == TPMmode.Current)
                    {
                        //нужно взять все причастные записи IncrementalPromo по promo
                        List<IncrementalPromo> incrementalPromos = Context.Set<IncrementalPromo>()
                            .Include(x => x.Promo.PromoProducts.Select(y => y.PromoProductsCorrections))
                            .Include(x => x.Promo.BTLPromoes)
                            .Include(x => x.Promo.PromoProductTrees)
                            .Include(x => x.Promo.PromoSupportPromoes)
                            .Where(g => g.Promo.Number == model.Promo.Number && !g.Disabled)
                            .ToList();
                        List<IncrementalPromo> resultIncrementalPromos = RSmodeHelper.EditToIncrementalPromoRS(Context, incrementalPromos);
                        model = resultIncrementalPromos.FirstOrDefault(g => g.Promo.Number == model.Promo.Number && g.Product.ZREP == model.Product.ZREP && !g.Disabled);
                    }
                }

                patch.Patch(model);
                model.LastModifiedDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);

                var casePrice = BaselineAndPriceCalculation.CalculateCasePrice(model, Context);
                model.CasePrice = casePrice;
                model.PlanPromoIncrementalLSV = model.CasePrice * model.PlanPromoIncrementalCases;

                Context.SaveChanges();

                if (ChangeIncrementalPromo.TPMmode == TPMmode.Current)
                {
                    if (ipromoes.Count > 1)
                    {
                        IncrementalPromo modelRS = ipromoes
                            .FirstOrDefault(g => g.Promo.Number == model.Promo.Number && g.TPMmode == TPMmode.RS);
                        if (modelRS != null)
                        {
                            Context.Set<Promo>().Remove(modelRS.Promo);
                            Context.SaveChanges();
                            //нужно взять все причастные записи IncrementalPromo по promo
                            List<IncrementalPromo> incrementalPromos = Context.Set<IncrementalPromo>()
                                .Include(x => x.Promo.PromoProducts.Select(y => y.PromoProductsCorrections))
                                .Include(x => x.Promo.BTLPromoes)
                                .Include(x => x.Promo.PromoProductTrees)
                                .Include(x => x.Promo.PromoSupportPromoes)
                                .Where(g => g.Promo.Number == model.Promo.Number && !g.Disabled)
                                .ToList();
                            List<IncrementalPromo> resultIncrementalPromos = RSmodeHelper.EditToIncrementalPromoRS(Context, incrementalPromos);
                        }
                    }
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
                var model = Context.Set<IncrementalPromo>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

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
            return Context.Set<IncrementalPromo>().Count(e => e.Id == key) > 0;
        }

        public static IEnumerable<Column> GetExportSettings()
        {
            int orderNumber = 1;
            IEnumerable<Column> columns = new List<Column>()
            {
                new Column { Order = orderNumber++, Field = "Product.ZREP", Header = "ZREP", Quoting = false },
                new Column { Order = orderNumber++, Field = "Product.ProductEN", Header = "Product Name", Quoting = false },
                new Column { Order = orderNumber++, Field = "Promo.ClientHierarchy", Header = "Client", Quoting = false },
                new Column { Order = orderNumber++, Field = "Promo.Number", Header = "Promo ID", Quoting = false, },
                new Column { Order = orderNumber++, Field = "Promo.Name", Header = "Promo Name", Quoting = false, },
                new Column { Order = orderNumber++, Field = "PlanPromoIncrementalCases", Header = "Plan Promo Incremental Cases", },
                new Column { Order = orderNumber++, Field = "CasePrice", Header = "Case Price", },
                new Column { Order = orderNumber++, Field = "PlanPromoIncrementalLSV", Header = "Plan Promo Incremental LSV", Quoting = false },
            };

            return columns;
        }

        public static IEnumerable<Column> GetExportSettingsRS()
        {
            int orderNumber = 1;
            IEnumerable<Column> columns = new List<Column>()
            {
                new Column { Order = orderNumber++, Field = "TPMmode", Header = "Indicator", Quoting = false },
                new Column { Order = orderNumber++, Field = "Product.ZREP", Header = "ZREP", Quoting = false },
                new Column { Order = orderNumber++, Field = "Product.ProductEN", Header = "Product Name", Quoting = false },
                new Column { Order = orderNumber++, Field = "Promo.ClientHierarchy", Header = "Client", Quoting = false },
                new Column { Order = orderNumber++, Field = "Promo.Number", Header = "Promo ID", Quoting = false, },
                new Column { Order = orderNumber++, Field = "Promo.Name", Header = "Promo Name", Quoting = false, },
                new Column { Order = orderNumber++, Field = "PlanPromoIncrementalCases", Header = "Plan Promo Incremental Cases", },
                new Column { Order = orderNumber++, Field = "CasePrice", Header = "Case Price", },
                new Column { Order = orderNumber++, Field = "PlanPromoIncrementalLSV", Header = "Plan Promo Incremental LSV", Quoting = false },
            };

            return columns;
        }

        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<IncrementalPromo> options, [FromUri] TPMmode tPMmode)
        {
            IQueryable results = options.ApplyTo(GetConstraintedQuery(tPMmode).Where(x => !x.Disabled));
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
                HandlerDataHelper.SaveIncomingArgument("TModel", typeof(IncrementalPromo), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(IncrementalPromoesController), data, visible: false, throwIfNotExists: false);
                if (tPMmode == TPMmode.Current)
                {
                    HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(IncrementalPromoesController.GetExportSettings), data, visible: false, throwIfNotExists: false);
                }
                if (tPMmode == TPMmode.RS)
                {
                    HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(IncrementalPromoesController.GetExportSettingsRS), data, visible: false, throwIfNotExists: false);
                }

                HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = $"Export {nameof(IncrementalPromo)} dictionary",
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
        public IHttpActionResult ExportIncrementalPromoPriceListXLSX(ODataQueryOptions<IncrementalPromo> options)
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
                HandlerDataHelper.SaveIncomingArgument("TModel", typeof(IncrementalPromo), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(IncrementalPromoesController), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(IncrementalPromoesController.GetExportSettings), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = $"Export {nameof(IncrementalPromo)} dictionary",
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
        public async Task<HttpResponseMessage> FullImportXLSX([FromUri] TPMmode tPMmode)
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
                CreateImportTask(fileName, "FullXLSXUpdateImportIncrementalPromoHandler", form, tPMmode);

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

        private void CreateImportTask(string fileName, string importHandler, NameValueCollection paramForm, TPMmode tPMmode)
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
                // Параметры импорта
                HandlerDataHelper.SaveIncomingArgument("CrossParam.ClientFilter", new TextListModel(paramForm.GetStringValue("clientFilter")), data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TPMmode", tPMmode, data, throwIfNotExists: false);

                HandlerDataHelper.SaveIncomingArgument("File", file, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportIncrementalPromo), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportIncrementalPromo).Name, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(IncrementalPromo), data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Загрузка импорта из файла " + typeof(ImportIncrementalPromo).Name,
                    Name = "Module.Host.TPM.Handlers." + importHandler,
                    ExecutionPeriod = null,
                    RunGroup = typeof(ImportIncrementalPromo).Name,
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
        public IHttpActionResult DownloadTemplateXLSX([FromUri] TPMmode tPMmode)
        {
            try
            {
                IEnumerable<Column> columns = new List<Column>();
                columns = GetExportSettings().Where(col => col.Field != "PlanPromoIncrementalLSV");
                //if (tPMmode == TPMmode.Current)
                //{
                //    columns = GetExportSettings().Where(col => col.Field != "PlanPromoIncrementalLSV");
                //}
                //if (tPMmode == TPMmode.RS)
                //{
                //    columns = GetExportSettingsRS().Where(col => col.Field != "PlanPromoIncrementalLSV");
                //}

                XLSXExporter exporter = new XLSXExporter(columns);
                string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
                string filename = string.Format("{0}Template.xlsx", "IncrementalPromo");
                if (!Directory.Exists(exportDir))
                {
                    Directory.CreateDirectory(exportDir);
                }
                string filePath = Path.Combine(exportDir, filename);
                exporter.Export(Enumerable.Empty<IncrementalPromo>(), filePath);
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
                return InternalServerError(new Exception("This Incremental Promo has already existed"));
            }
            else
            {
                return InternalServerError(e.InnerException);
            }
        }
    }
}