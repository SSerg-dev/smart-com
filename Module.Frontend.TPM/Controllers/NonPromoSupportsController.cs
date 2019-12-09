﻿using AutoMapper;
using Core.Security;
using Core.Security.Models;
using Core.Settings;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.DTO;
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
using System.Net.Http.Headers;
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
    public class NonPromoSupportsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public NonPromoSupportsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<NonPromoSupport> GetConstraintedQuery()
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();

			IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
			IQueryable<NonPromoSupport> query = Context.Set<NonPromoSupport>().Where(e => !e.Disabled);
			IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();

			query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

			return query;
		}

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<NonPromoSupport> GetNonPromoSupport([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<NonPromoSupport> GetNonPromoSupports()
        {
            return GetConstraintedQuery();
        }

        [ClaimsAuthorize]
        public IHttpActionResult Put([FromODataUri] System.Guid key, Delta<NonPromoSupport> patch)
        {
            var model = Context.Set<NonPromoSupport>().Find(key);
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
        public IHttpActionResult Post(NonPromoSupport model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // делаем UTC +3
            model.StartDate = ChangeTimeZoneUtil.ResetTimeZone(model.StartDate);
            model.EndDate = ChangeTimeZoneUtil.ResetTimeZone(model.EndDate);

            var proxy = Context.Set<NonPromoSupport>().Create<NonPromoSupport>();
            var result = (NonPromoSupport)Mapper.Map(model, proxy, typeof(NonPromoSupport), proxy.GetType(), opts => opts.CreateMissingTypeMaps = true);
            Context.Set<NonPromoSupport>().Add(result);

            try
            {
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                return GetErorrRequest(e);
            }

            return Created(result);
        }

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<NonPromoSupport> patch)
        {
            try
            {
                var model = Context.Set<NonPromoSupport>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                patch.Patch(model);
                // делаем UTC +3
                model.StartDate = ChangeTimeZoneUtil.ResetTimeZone(model.StartDate);
                model.EndDate = ChangeTimeZoneUtil.ResetTimeZone(model.EndDate);

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

        [ClaimsAuthorize]
        public IHttpActionResult Delete([FromODataUri] System.Guid key)
        {
            try
            {
                var model = Context.Set<NonPromoSupport>().Find(key);
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
            return Context.Set<NonPromoSupport>().Count(e => e.Id == key) > 0;
        }
        
 
        private IEnumerable<Column> GetExportSettingsTiCosts()
        {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 0, Field = "Number", Header = "ID", Quoting = false },
                new Column() { Order = 1, Field = "ClientTree.FullPathName", Header = "Client", Quoting = false },
                new Column() { Order = 2, Field = "BrandTech.Name", Header = "Brand Tech", Quoting = false },
                new Column() { Order = 3, Field = "NonPromoEquipment.EquipmentType", Header = "Equipment Type", Quoting = false },
                new Column() { Order = 4, Field = "PlanQuantity", Header = "Plan Quantity", Quoting = false },
                new Column() { Order = 5, Field = "ActualQuantity", Header = "Actual Quantity", Quoting = false },
                new Column() { Order = 6, Field = "PlanCostTE", Header = "Plan Cost TE Total", Quoting = false },
                new Column() { Order = 7, Field = "ActualCostTE", Header = "Actual Cost TE Total", Quoting = false },
                new Column() { Order = 8, Field = "StartDate", Header = "Start Date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 9, Field = "EndDate", Header = "End Date", Quoting = false, Format = "dd.MM.yyyy" },
            };
            return columns;
        }

        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<NonPromoSupport> options, string section)
        {
            try
            {
                IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));
				IEnumerable<Column> columns = GetExportSettingsTiCosts();
				XLSXExporter exporter = new XLSXExporter(columns);
                UserInfo user = authorizationManager.GetCurrentUser();
                string username = user == null ? "" : user.Login;
                string filePath = exporter.GetExportFileName("NonPromoSupport", username);
                exporter.Export(results, filePath);
                string filename = System.IO.Path.GetFileName(filePath);
                return Content<string>(HttpStatusCode.OK, filename);
            }
            catch (Exception e)
            {
                return Content<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

		[ClaimsAuthorize]
		public async Task<HttpResponseMessage> FullImportXLSX ()
		{
			try
			{
				if (!Request.Content.IsMimeMultipartContent())
				{
					throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
				}

				string importDir = Core.Settings.AppSettingsManager.GetSetting("IMPORT_DIRECTORY", "ImportFiles");
				string fileName = await FileUtility.UploadFile(Request, importDir);

				CreateImportTask(fileName, "FullXLSXUpdateAllHandler");

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

		private void CreateImportTask (string fileName, string importHandler)
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
				FileModel file = new FileModel() {
					LogicType = "Import",
					Name = System.IO.Path.GetFileName(fileName),
					DisplayName = System.IO.Path.GetFileName(fileName)
				};

				HandlerDataHelper.SaveIncomingArgument("File", file, data, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
				//HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportNonPromoSupport), data, visible: false, throwIfNotExists: false);
				//HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportNonPromoSupport).Name, data, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(NonPromoSupport), data, visible: false, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("UniqueFields", new List<String>() { "Name" }, data);

				LoopHandler handler = new LoopHandler() {
					Id = Guid.NewGuid(),
					ConfigurationName = "PROCESSING",
					Description = "Загрузка импорта из файла " + typeof(NonPromoSupport).Name,
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

		private ExceptionResult GetErorrRequest(Exception e)
        {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
            {
                return InternalServerError(new Exception("This NonPromoSupport has already existed"));
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
            catch (Exception e)
            {
                return Json(new { success = false });
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

                string directory = Core.Settings.AppSettingsManager.GetSetting("PROMO_SUPPORT_DIRECTORY", "NonPromoSupportFiles");
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
        [Route("odata/NonPromoSupports/DownloadFile")]
        public HttpResponseMessage DownloadFile(string fileName)
        { 
            try
            {
                string directory = Core.Settings.AppSettingsManager.GetSetting("PROMO_SUPPORT_DIRECTORY", "NonPromoSupportFiles");
                return FileUtility.DownloadFile(directory, fileName);
            }
            catch (Exception e)
            {
                return new HttpResponseMessage(HttpStatusCode.Accepted);
            }
        }
    }
}