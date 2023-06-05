using AutoMapper;
using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist.Model;
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
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers
{
    public class PromoSalesesController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public PromoSalesesController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<PromoSales> GetConstraintedQuery()
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();

            IQueryable<PromoSales> query = Context.Set<PromoSales>().Where(e => !e.Disabled);

            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public SingleResult<PromoSales> GetPromoSale([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<PromoSales> GetPromoSaleses()
        {
            return GetConstraintedQuery();
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<PromoSales> GetFilteredData(ODataQueryOptions<PromoSales> options)
        {
            var query = GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<PromoSales>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<PromoSales>;
        }

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<PromoSales> patch)
        {
            var model = Context.Set<PromoSales>().Find(key);

            if (model == null)
            {
                return NotFound();
            }

            patch.Put(model);

            try
            {
                await Context.SaveChangesAsync();
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
        public IHttpActionResult Post(PromoSales model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var proxy = Context.Set<PromoSales>().Create<PromoSales>();
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<PromoSales, PromoSales>().ReverseMap());
            var mapper = configuration.CreateMapper();
            var result = mapper.Map(model, proxy);

            //Context.Set<PromoSales>().Add(result);
            //Context.SaveChanges();
            try
            {
                string startDate = (result.StartDate == null) ? "NULL" : String.Format("'{0}'", result.StartDate.Value);
                string endDate = (result.EndDate == null) ? "NULL" : String.Format("'{0}'", result.EndDate.Value);
                string dispatchesStart = (result.DispatchesStart == null) ? "NULL" : String.Format("'{0}'", result.DispatchesStart.Value);
                string dispatchesEnd = (result.DispatchesEnd == null) ? "NULL" : String.Format("'{0}'", result.DispatchesEnd.Value);

                string insertScript = String.Format("INSERT INTO [DefaultSchemaSetting].[PromoSales] ([Id],[Name],[ClientId],[BrandId],[BrandTechId],[PromoStatusId],[MechanicId],[StartDate],[EndDate],[DispatchesStart],[DispatchesEnd],[BudgetItemId],[Plan],[Fact]) VALUES (NEWID(), '{0}', '{1}', NULL, NULL, '{4}', NULL, {6}, {7}, {8}, {9}, '{10}', {11}, {12})",
                    result.Name, result.ClientId, result.BrandId, result.BrandTechId, result.PromoStatusId, result.MechanicId, startDate, endDate, dispatchesStart, dispatchesEnd, result.BudgetItemId, result.Plan, result.Fact);

                Context.ExecuteSqlCommand(insertScript);
            }
            catch (Exception e)
            {
                return GetErorrRequest(e);
            }

            return Created(result);
        }

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<PromoSales> patch)
        {
            try
            {
                var model = Context.Set<PromoSales>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                patch.Patch(model);
                await Context.SaveChangesAsync();

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
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            try
            {
                var model = Context.Set<PromoSales>().Find(key);

                if (model == null)
                {
                    return NotFound();
                }

                Promo promo = Context.Set<Promo>().Where(x => x.Name == model.Name).FirstOrDefault();
                IQueryable<Sale> query = Context.Set<Sale>().Where(x => x.PromoId == promo.Id);

                promo.DeletedDate = DateTime.Now;
                promo.Disabled = true;

                foreach (var sale in query)
                {
                    sale.DeletedDate = DateTime.Now;
                    sale.Disabled = true;
                }

                await Context.SaveChangesAsync();

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }

        private bool EntityExists(Guid key)
        {
            return Context.Set<PromoSales>().Count(e => e.Id == key) > 0;
        }

        public static IEnumerable<Column> GetExportSettings()
        {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 0, Field = "Number", Header = "Promo ID", Quoting = false },
                new Column() { Order = 1, Field = "Name", Header = "Promo name", Quoting = false },
                new Column() { Order = 2, Field = "Client.CommercialSubnet.CommercialNet.Name", Header = "Customer", Quoting = false },
                new Column() { Order = 3, Field = "Brand.Name", Header = "Brand", Quoting = false },
                new Column() { Order = 4, Field = "BrandTech.BrandsegTechsub", Header = "BrandTech", Quoting = false },
                new Column() { Order = 5, Field = "PromoStatus.Name", Header = "PromoStatus", Quoting = false },
                new Column() { Order = 6, Field = "Mechanic.MechanicName", Header = "Mechanic", Quoting = false },
                new Column() { Order = 7, Field = "Mechanic.Discount", Header = "Mechanic discount, %", Quoting = false },
                new Column() { Order = 8, Field = "Mechanic.Comment", Header = "Mechanic comment", Quoting = false },
                new Column() { Order = 9, Field = "StartDate", Header = "StartDate", Quoting = false, Format = "dd.MM.yyyy HH:mm:ss" },
                new Column() { Order = 10, Field = "EndDate", Header = "EndDate", Quoting = false, Format = "dd.MM.yyyy HH:mm:ss" },
                new Column() { Order = 11, Field = "DispatchesStart", Header = "DispatchesStart", Quoting = false, Format = "dd.MM.yyyy HH:mm:ss" },
                new Column() { Order = 12, Field = "DispatchesEnd", Header = "DispatchesEnd", Quoting = false, Format = "dd.MM.yyyy HH:mm:ss" },
                new Column() { Order = 13, Field = "BudgetItem.Budget.Name", Header = "Бюджет", Quoting = false },
                new Column() { Order = 14, Field = "BudgetItem.Name", Header = "Бюджетная статья", Quoting = false },
                new Column() { Order = 15, Field = "Plan", Header = "План", Quoting = false },
                new Column() { Order = 16, Field = "Fact", Header = "Факт", Quoting = false },
            };
            return columns;
        }

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> ExportXLSX(ODataQueryOptions<PromoSales> options)
        {
            IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            HandlerData data = new HandlerData();
            string handlerName = "ExportHandler";

            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TModel", typeof(PromoSales), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(PromoSalesesController), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(PromoSalesesController.GetExportSettings), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = $"Export {nameof(PromoSales)} dictionary",
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

                await CreateImportTask(fileName, "FullXLSXUpdateImportPromoSalesHandler");

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
            HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportPromoSales), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportPromoSales).Name, data, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(PromoSales), data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = "Загрузка импорта из файла " + typeof(ImportPromoSales).Name,
                Name = "Module.Host.TPM.Handlers." + importHandler,
                ExecutionPeriod = null,
                CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                RunGroup = typeof(ImportPromoSales).Name,
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

        private ExceptionResult GetErorrRequest(Exception e)
        {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
            {
                return InternalServerError(new Exception("This Promo Sale has already existed"));
            }
            else
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }
    }
}