using Core.Security;
using Core.Security.Models;
using Core.Settings;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;
using Module.Frontend.TPM.Util;
using Frontend.Core.Extensions;
using System.Net.Http;
using System.Net.Http.Headers;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.History;
using Ninject;
using Core.History;

namespace Module.Frontend.TPM.Controllers
{
	public class PLUDictionariesController : EFContextController
	{
		private readonly IAuthorizationManager authorizationManager;

		[Inject]
		public IHistoryReader HistoryReader { get; set; }

		public PLUDictionariesController(IAuthorizationManager authorizationManager)
		{
			this.authorizationManager = authorizationManager;
		}

		protected IQueryable<PLUDictionary> GetQuery()
		{
			Context.Database.Log = x => Debug.WriteLine(x);
			UserInfo user = authorizationManager.GetCurrentUser();
			//user.Id = new Guid("7A0BE6A0-F19B-E911-A842-000D3A46085B");
			string role =  authorizationManager.GetCurrentRoleName(); //"KeyAccountManager"
			IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
				.Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
				.ToList() : new List<Constraint>();

			IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
			IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();

			IQueryable<PLUDictionary> query = Context.Set<PLUDictionary>();
			query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);
			return query;
		}

		[ClaimsAuthorize]
		[EnableQuery(MaxNodeCount = int.MaxValue)]
		public IQueryable<PLUDictionary> GetPLUDictionaries()
		{

			//var sss = HistoryReader.GetAll<HistoryPLUDictionary>();

			var query = GetQuery();
			return query;
		}

		[ClaimsAuthorize]
		public IHttpActionResult DownloadTemplateXLSX()
		{
			try
			{
				var columns = GetExportSettings();
				XLSXExporter exporter = new XLSXExporter(columns);
				string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
				string filename = string.Format("{0}Template.xlsx", "AssortmentMatrix");
				if (!Directory.Exists(exportDir))
				{
					Directory.CreateDirectory(exportDir);
				}
				string filePath = Path.Combine(exportDir, filename);
				exporter.Export(Enumerable.Empty<AssortmentMatrix>(), filePath);
				string file = Path.GetFileName(filePath);
				return Content(HttpStatusCode.OK, file);
			}
			catch (Exception e)
			{
				return Content(HttpStatusCode.InternalServerError, e.Message);
			}

		}

		public static IEnumerable<Column> GetExportSettings()
		{
			IEnumerable<Column> columns = new List<Column>() {
				new Column() { Order = 0, Field = "ClientTreeId", Header = "Client id", Quoting = false },
				new Column() { Order = 1, Field = "PluCode", Header = "PLU", Quoting = false },
				new Column() { Order = 2, Field = "EAN_PC", Header = "EAN PC", Quoting = false },
				new Column() { Order = 3, Field = "ClientTreeName", Header = "Client", Quoting = false },
				new Column() { Order = 4, Field = "ProductEN", Header = "ProductEN", Quoting = false },
				};
			return columns;
		}

		public IHttpActionResult ExportXLSX(ODataQueryOptions<PLUDictionary> options)
		{
			IQueryable results = options.ApplyTo(GetPLUDictionaries());
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
				HandlerDataHelper.SaveIncomingArgument("TModel", typeof(PLUDictionary), data, visible: false, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(PLUDictionariesController), data, visible: false, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(PLUDictionariesController.GetExportSettings), data, visible: false, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);

				LoopHandler handler = new LoopHandler()
				{
					Id = Guid.NewGuid(),
					ConfigurationName = "PROCESSING",
					Description = $"Export {nameof(PLUDictionary)} dictionary",
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

				CreateImportTask(fileName, "FullXLSXPLUDictionaryImportHandler");

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
				HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportPLUDictionary), data, visible: false, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportPLUDictionary).Name, data, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(ImportPLUDictionary), data, visible: false, throwIfNotExists: false);

				LoopHandler handler = new LoopHandler()
				{
					Id = Guid.NewGuid(),
					ConfigurationName = "PROCESSING",
					Description = "Загрузка импорта из файла " + typeof(ImportPLUDictionary).Name,
					Name = "Module.Host.TPM.Handlers." + importHandler,
					ExecutionPeriod = null,
					CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
					RunGroup = typeof(ImportPLUDictionary).Name,
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
	}
}
