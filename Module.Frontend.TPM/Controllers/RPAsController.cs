using AutoMapper;
using Core.Security;
using UserInfoCore = Core.Security.Models.UserInfo;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions;
using Module.Persist.TPM.Model.TPM;
using Persist.Model;
using Microsoft.Azure.Management.DataFactory;
using Microsoft.Azure.Management.DataFactory.Models;
using System;
using System.Collections.Generic;
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
using Core.Settings;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using Newtonsoft.Json;
using Module.Persist.TPM.Utils;
using Frontend.Core.Extensions.Export;
using Module.Frontend.TPM.Model;
using Persist;
using Looper.Core;
using Looper.Parameters;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Utility.Azure;
using Column = Frontend.Core.Extensions.Export.Column;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Utility;
using Module.Persist.TPM.Model.DTO;
using Core.Security.Models;
using Module.Persist.TPM.Model.Import;

namespace Module.Frontend.TPM.Controllers
{
	public class RPAsController : EFContextController
	{
		private readonly IAuthorizationManager authorizationManager;
		private Core.Security.Models.UserInfo user;
		private string role;
		private Guid roleId;
		private IList<Constraint> constraints;
		private static object locker = new object();
		public RPAsController(IAuthorizationManager authorizationManager)
		{
			this.authorizationManager = authorizationManager;
			this.user = authorizationManager.GetCurrentUser();
			this.role = authorizationManager.GetCurrentRoleName();
			this.roleId = this.user.Roles.ToList().Find(role => role.SystemName == this.role).Id.Value;
		}

		protected IQueryable<RPA> GetConstraintedQuery()
		{

			this.constraints = this.user.Id.HasValue ? Context.Constraints
				.Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
				.ToList() : new List<Constraint>();
			IQueryable<RPA> query = Context.Set<RPA>();

			return query;
		}

		[ClaimsAuthorize]
		[EnableQuery(MaxNodeCount = int.MaxValue)]
		public SingleResult<RPA> GetRPA([FromODataUri] System.Guid key)
		{
			return SingleResult.Create<RPA>(GetConstraintedQuery());
		}

		[ClaimsAuthorize]
		[EnableQuery(MaxNodeCount = int.MaxValue)]
		public IQueryable<RPA> GetRPAs()
		{
			return GetConstraintedQuery();
		}

		[ClaimsAuthorize]
		[HttpPost]
		public IQueryable<RPA> GetFilteredData(ODataQueryOptions<RPA> options)
		{
			var query = GetConstraintedQuery();

			var querySettings = new ODataQuerySettings
			{
				EnsureStableOrdering = false,
				HandleNullPropagation = HandleNullPropagationOption.False
			};

			var optionsPost = new ODataQueryOptionsPost<RPA>(options.Context, Request, HttpContext.Current.Request);
			return optionsPost.ApplyTo(query, querySettings) as IQueryable<RPA>;
		}
				
		/// <summary>
		/// Метод создания RPA (новый функционал)
		/// </summary>
		/// <returns></returns>
		[ClaimsAuthorize]
		[HttpPost]
		public IHttpActionResult SaveRPA()
        {
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var currentRequest = HttpContext.Current.Request;
			var rpaModel = JsonConvert.DeserializeObject<RPA>(currentRequest.Params.Get("Model"));
			var rpaType = currentRequest.Params.Get("RPAType");
			var proxy = Context.Set<RPA>().Create<RPA>();
			var result = (RPA)Mapper.Map(rpaModel, proxy, typeof(RPA), proxy.GetType(), opts => opts.CreateMissingTypeMaps = true);
			Context.Set<RPA>().Add(result);
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

				//Save file
				string directory = Core.Settings.AppSettingsManager.GetSetting("RPA_DIRECTORY", "RPAFiles");

				string fileName = Task<string>.Run(async () => await FileUtility.UploadFile(Request, directory)).Result;

				IList<Constraint> constraints = Context.Constraints
														.Where(x => x.UserRole.UserId == user.Id && x.UserRole.Role.Id == roleId)
														.ToList();
				IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
				//здесь должны быть все записи, а не только неудаленные!
				IQueryable<ClientTree> query = Context.Set<ClientTree>().AsNoTracking();
				IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
				query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);
				List<ClientTree> existingClientTreeIds = query.Where(x => x.EndDate == null && x.IsBaseClient == true).ToList();
				var constraintIds = String.Join(",", existingClientTreeIds.Select(x => x.ObjectId.ToString()));
				var constraintTreeIds = String.Join(",", existingClientTreeIds.Select(x => x.Id.ToString()));
				result.Constraint = String.Join(",", constraints.Where(c => c.Prefix == "CLIENT_ID").Select(x => x.Value));
				result.CreateDate = DateTime.UtcNow;
				result.FileURL = Path.GetFileName(fileName);

				// Save RPA
				var resultSaveChanges = Context.SaveChanges();

				switch(rpaType)
                {
					case "PromoSupport":
						CreateRPAPromoSupportTask(fileName);
						break;
					case "NonPromoSupport":
						CreateRPANonPromoSupportTask(fileName);
						break;
					case "Actuals_EAN_PC":
						CreateRPAActualEanPcTask(fileName);
						break;
				}
			}
			catch(Exception ex)
            {
				return GetErorrRequest(ex);
            }			
			return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, message = "RPA save and upload done." }));
		}

		private void CreateRPAPromoSupportTask(string fileName)
        {
			var handlerName = "FullXLSXRPAPromoSupportImportHandler";
			UserInfoCore user = authorizationManager.GetCurrentUser();
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
				HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportRPAPromoSupport), data, visible: false, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportRPAPromoSupport).Name, data, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(ImportRPAPromoSupport), data, visible: false, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("UniqueFields", new List<String>() { "Name" }, data);

				LoopHandler handler = new LoopHandler()
				{
					Id = Guid.NewGuid(),
					ConfigurationName = "PROCESSING",
					Description = "Загрузка шаблона из файла " + typeof(RPA).Name,
					Name = "Module.Host.TPM.Handlers." + handlerName,
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

		private void CreateRPANonPromoSupportTask(string fileName)
		{
			var handlerName = "FullXLSXRPANonPromoSupportImportHandler";
			UserInfoCore user = authorizationManager.GetCurrentUser();
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
				HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportRPAPromoSupport), data, visible: false, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportRPAPromoSupport).Name, data, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(ImportRPAPromoSupport), data, visible: false, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("UniqueFields", new List<String>() { "Name" }, data);

				LoopHandler handler = new LoopHandler()
				{
					Id = Guid.NewGuid(),
					ConfigurationName = "PROCESSING",
					Description = "Загрузка шаблона из файла " + typeof(RPA).Name,
					Name = "Module.Host.TPM.Handlers." + handlerName,
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

		private void CreateRPAActualEanPcTask(string fileName)
		{
			var handlerName = "FullXLSXRPAActualEANPCImportHandler";
			UserInfoCore user = authorizationManager.GetCurrentUser();
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
				HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportRpaActualEanPc), data, visible: false, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportRpaActualEanPc).Name, data, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(ImportRpaActualEanPc), data, visible: false, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("UniqueFields", new List<String>() { "Name" }, data);

				LoopHandler handler = new LoopHandler()
				{
					Id = Guid.NewGuid(),
					ConfigurationName = "PROCESSING",
					Description = "Загрузка шаблона из файла " + typeof(RPA).Name,
					Name = "Module.Host.TPM.Handlers." + handlerName,
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

		[ClaimsAuthorize]
		public IHttpActionResult DownloadTemplateXLSX()
		{
			try
			{
				var currentRequest = HttpContext.Current.Request;
				var handlerId = JsonConvert.DeserializeObject<string>(currentRequest.Params.Get("handlerId"));
				Guid testId = Guid.Parse(handlerId);
				RPASetting setting = Context.Set<RPASetting>()
					.First(s => s.Id == testId);
				var columnHeaders = JsonConvert.DeserializeObject<RPAEventJsonField>(setting.Json).templateColumns;
				var columns = columnHeaders.Select(c => JsonConvert.DeserializeObject<Column>(c.ToString()));
				XLSXExporter exporter = new XLSXExporter(columns);
				string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
				string filename = string.Format("{0}Template_{1}.xlsx", "RPA", DateTime.UtcNow.ToString("yyyyddMMHHmmss"));
				if (!Directory.Exists(exportDir))
				{
					Directory.CreateDirectory(exportDir);
				}
				string filePath = Path.Combine(exportDir, filename);
				exporter.Export(Enumerable.Empty<RPA>(), filePath);
				string file = Path.GetFileName(filePath);
				return Content(HttpStatusCode.OK, file);
			}
			catch (Exception e)
			{
				return Content(HttpStatusCode.InternalServerError, e.Message);
			}

		}

		[ClaimsAuthorize]
		[HttpGet]
		[Route("odata/RPAs/DownloadFile")]
		public HttpResponseMessage DownloadFile(string fileName)
		{
			try
			{
				string directory = Core.Settings.AppSettingsManager.GetSetting("RPA_DIRECTORY", "RPAFiles");
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
			catch (Exception e)
			{
				return new HttpResponseMessage(HttpStatusCode.Accepted);
			}
		}

		private ExceptionResult GetErorrRequest(Exception e)
		{
			return InternalServerError(e);
		}

	}
}
