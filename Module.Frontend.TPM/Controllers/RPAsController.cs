using AutoMapper;
using Core.Security;
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

		[ClaimsAuthorize]
		[HttpPost]
		public async Task<IHttpActionResult> SaveRPA()
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			this.constraints = this.user.Id.HasValue ? Context.Constraints
				.Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
				.ToList() : new List<Constraint>();
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

				if (!CheckFileCorrect(fileName))
				{
					throw new FileLoadException("The import file is corrupted.");
					
				}



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

				string LogURL = $"OutputLogFile_{ result.Id}.xlsx";

				string SchemaBD = AppSettingsManager.GetSetting("DefaultSchema", "");

				//Call Pipe
				string tenantID = AppSettingsManager.GetSetting("RPA_UPLOAD_TENANT_ID", "");
				string applicationId = AppSettingsManager.GetSetting("RPA_UPLOAD_APPLICATION_ID", "");
				string authenticationKey = AppSettingsManager.GetSetting("RPA_UPLOAD_AUTHENTICATION_KEY", "");
				string subscriptionId = AppSettingsManager.GetSetting("RPA_UPLOAD_SUBSCRIPTION_ID", "");
				string resourceGroup = AppSettingsManager.GetSetting("RPA_UPLOAD_RESOURCE_GROUP", "");
				string dataFactoryName = AppSettingsManager.GetSetting("RPA_UPLOAD_DATA_FACTORY_NAME", "");
				string pipelineName = "";
				string ServerName = AppSettingsManager.GetSetting("ServerName", ""); 
				string DatabaseName = AppSettingsManager.GetSetting("DatabaseName", ""); 
				string DBUserId = AppSettingsManager.GetSetting("DBUserId", ""); 
				string PasswordKV = AppSettingsManager.GetSetting("PasswordKV", ""); 
				string AKVScope = AppSettingsManager.GetSetting("AKVScope", "");
				string BlobStorageName = AppSettingsManager.GetSetting("BlobStorageName", "");
				Dictionary<string, object> parameters = null;
				switch (rpaType)
				{
					case "Actuals_EAN_PC":
						pipelineName = AppSettingsManager.GetSetting("RPA_UPLOAD_PIPELINE_ACTUALS_NAME", "");
						parameters = new Dictionary<string, object>
										{
											{ "FileName", Path.GetFileName(fileName) },
											{ "RPAId", result.Id },
											{ "UserRoleName", this.user.GetCurrentRole().SystemName },
											{ "UserId", this.user.Login },
											{ "ProductReference", "EAN_PC" },
											{ "LogFileURL", LogURL},
											{ "Constraints", constraintIds},
											{ "Schema", SchemaBD},
											{ "ServerName", ServerName},
											{ "DatabaseName", DatabaseName},
											{ "DBUserId", DBUserId},
											{ "TenantId", tenantID},
											{ "ClientId", applicationId},
											{ "BlobStorageName", BlobStorageName},
											{ "PasswordKV", PasswordKV}
											
										};
						await CreateCalculationTaskAsync(fileName, result.Id);
						CreatePipeForActuals(tenantID, applicationId, authenticationKey, subscriptionId, resourceGroup, dataFactoryName, pipelineName, parameters);
						break;
					case "Actuals_PLU":
						pipelineName = AppSettingsManager.GetSetting("RPA_UPLOAD_PIPELINE_ACTUALS_NAME", "");
						parameters = new Dictionary<string, object>
										{
											{ "FileName", Path.GetFileName(fileName) },
											{ "RPAId", result.Id },
											{ "UserRoleName", this.user.GetCurrentRole().SystemName },
											{ "UserId", this.user.Login },
											{ "ProductReference", "PLU" },
											{ "LogFileURL", LogURL},
											{ "Constraints", constraintIds},
											{ "Schema", SchemaBD},
											{ "ServerName", ServerName},
											{ "DatabaseName", DatabaseName},
											{ "DBUserId", DBUserId},
											{ "TenantId", tenantID},
											{ "ClientId", applicationId},
											{ "BlobStorageName", BlobStorageName},
											{ "PasswordKV", PasswordKV}
										};
						await CreateCalculationTaskAsync(fileName, result.Id);
						CreatePipeForActuals(tenantID, applicationId, authenticationKey, subscriptionId, resourceGroup, dataFactoryName, pipelineName, parameters);
						break;
					case "Events":
						pipelineName = AppSettingsManager.GetSetting("RPA_UPLOAD_PIPELINE_EVENT_NAME", "");
						parameters = new Dictionary<string, object>
									{
										{ "FileName", Path.GetFileName(fileName) },
										{ "RPAId", result.Id },
										{ "UserRoleName", this.user.GetCurrentRole().SystemName },
										{ "UserId", this.user.Login },
										{ "LogFileURL", LogURL},
										{ "Scheme", SchemaBD},
										{ "Constraints", constraintIds},
										{ "ServerName", ServerName},
										{ "DatabaseName", DatabaseName},
										{ "DBUserId", DBUserId},
										{ "TenantId", tenantID},
										{ "ClientId", applicationId},
										{ "BlobStorageName", BlobStorageName},
										{ "PasswordKV", PasswordKV},
										{ "AKVScope", AKVScope}
									};
						CreatePipeForEvents(tenantID, applicationId, authenticationKey, subscriptionId, resourceGroup, dataFactoryName, pipelineName, parameters);
						CreateRPAEventImportTask(fileName);
						break;
					case "NonPromoSupport":
						pipelineName = AppSettingsManager.GetSetting("RPA_UPLOAD_PIPELINE_SUPPORT_NAME", "");
						parameters = new Dictionary<string, object>
									{
										{ "FileName", Path.GetFileName(fileName) },
										{ "RPAId", result.Id },
										{ "UserRoleName", this.user.GetCurrentRole().SystemName },
										{ "UserId", this.user.Login },
										{ "LogFileURL", LogURL},
										{ "SupportType", "NonPromoSupport"},
										{ "Constraints", constraintTreeIds},
										{ "Schema", SchemaBD},
										{ "ServerName", ServerName},
										{ "DatabaseName", DatabaseName},
										{ "DBUserId", DBUserId},
										{ "TenantId", tenantID},
										{ "ClientId", applicationId},
										{ "BlobStorageName", BlobStorageName},
										{ "PasswordKV", PasswordKV},
										{ "AKVScope", AKVScope}
									};
						CreatePipeForEvents(tenantID, applicationId, authenticationKey, subscriptionId, resourceGroup, dataFactoryName, pipelineName, parameters);
						break;
					case "PromoSupport":
						pipelineName = AppSettingsManager.GetSetting("RPA_UPLOAD_PIPELINE_SUPPORT_NAME", "");
						parameters = new Dictionary<string, object>
									{
										{ "FileName", Path.GetFileName(fileName) },
										{ "RPAId", result.Id },
										{ "UserRoleName", this.user.GetCurrentRole().SystemName },
										{ "UserId", this.user.Login },
										{ "LogFileURL", LogURL},
										{ "SupportType", "PromoSupport"},
										{ "Constraints", constraintTreeIds},
										{ "Schema", SchemaBD},
										{ "ServerName", ServerName},
										{ "DatabaseName", DatabaseName},
										{ "DBUserId", DBUserId},
										{ "TenantId", tenantID},
										{ "ClientId", applicationId},
										{ "BlobStorageName", BlobStorageName},
										{ "PasswordKV", PasswordKV},
										{ "AKVScope", AKVScope}
									};
						await CreateCalculationPromoSupportTaskAsync(fileName, result.Id);
						CreatePipeForEvents(tenantID, applicationId, authenticationKey, subscriptionId, resourceGroup, dataFactoryName, pipelineName, parameters);
						break;
				}

			}
			catch (Exception e)
			{
				return GetErorrRequest(e);

			}

			return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, message = "RPA save and upload done." }));
		}

		private void CreateRPAEventImportTask(string fileName)
        {
			string importHandler = "FullXLSXRPAEventImportHandler";

            Core.Security.Models.UserInfo user = authorizationManager.GetCurrentUser();
			Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
			RoleInfo role = authorizationManager.GetCurrentRole();
			Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

			HandlerData data = new HandlerData();
			FileModel file = new FileModel()
			{
				LogicType = "Import",
				Name = Path.GetFileName(fileName),
				DisplayName = Path.GetFileName(fileName)
			};

			HandlerDataHelper.SaveIncomingArgument("File", file, data, visible: false, throwIfNotExists: false);
			HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
			HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
			HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportRPAEvent), data, visible: false, throwIfNotExists: false);
			HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportRPAEvent).Name, data, throwIfNotExists: false);
			HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(ImportRPAEvent), data, visible: false, throwIfNotExists: false);
			
			LoopHandler handler = new LoopHandler()
			{
				Id = Guid.NewGuid(),
				ConfigurationName = "PROCESSING",
				Description = "Загрузка импорта из файла " + typeof(ImportRPAEvent).Name,
				Name = "Module.Host.TPM.Handlers." + importHandler,
				ExecutionPeriod = null,
				RunGroup = typeof(ImportRPAEvent).Name,
				CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
				LastExecutionDate = null,
				NextExecutionDate = null,
				ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
				UserId = userId,
				RoleId = roleId
			};

			handler.SetParameterData(data);
			Context.LoopHandlers.Add(handler);
			Context.SaveChanges();
		}
		private IHttpActionResult CreatePipeForEvents(string tenantID, string applicationId, string authenticationKey, string subscriptionId, string resourceGroup, string dataFactoryName, string pipelineName, Dictionary<string, object> parameters)
		{
			try
			{
				var context = new AuthenticationContext("https://login.microsoftonline.com/" + tenantID);
				ClientCredential cc = new ClientCredential(applicationId, authenticationKey);
				AuthenticationResult authenticationResult = context.AcquireTokenAsync(
					"https://management.azure.com/", cc).Result;
				ServiceClientCredentials cred = new TokenCredentials(authenticationResult.AccessToken);
				var client = new DataFactoryManagementClient(cred)
				{
					SubscriptionId = subscriptionId
				};

				CreateRunResponse runResponse = client.Pipelines.CreateRunWithHttpMessagesAsync(
					resourceGroup, dataFactoryName, pipelineName, parameters: parameters
				).Result.Body;

			}
			catch (Exception ex)
			{

				return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, message = "Pipe start failure " + ex.Message }));

			}
			return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, message = "Pipe started successfull" }));
		}

		private IHttpActionResult CreatePipeForActuals(string tenantID, string applicationId, string authenticationKey, string subscriptionId, string resourceGroup, string dataFactoryName, string pipelineName, Dictionary<string, object> parameters)
		{
			try
			{
				var context = new AuthenticationContext("https://login.microsoftonline.com/" + tenantID);
				ClientCredential cc = new ClientCredential(applicationId, authenticationKey);
				AuthenticationResult authenticationResult = context.AcquireTokenAsync(
					"https://management.azure.com/", cc).Result;
				ServiceClientCredentials cred = new TokenCredentials(authenticationResult.AccessToken);
				var client = new DataFactoryManagementClient(cred)
				{
					SubscriptionId = subscriptionId
				};

				CreateRunResponse runResponse = client.Pipelines.CreateRunWithHttpMessagesAsync(
					resourceGroup, dataFactoryName, pipelineName, parameters: parameters
				).Result.Body;

			}
			catch (Exception ex)
			{
				return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, message = "Pipe start failure " + ex.Message }));
			}
			return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, message = "Pipe started successfull" }));
		}

		private bool CheckFileCorrect(string templateFileName)
        {
			bool res = true;
			SpreadsheetDocument book;
			var stringPath = Path.GetDirectoryName(templateFileName);
			var stringName = Path.GetFileName(templateFileName);
			byte[] resAzure = AzureBlobHelper.ReadExcelFromBlob(stringPath.Split('\\').Last(), stringName);
            try {
				if (resAzure.Length == 0)
				{
					book = SpreadsheetDocument.Open(templateFileName, false);
				}
				else
				{
					book = SpreadsheetDocument.Open(new MemoryStream(resAzure), false);
				}
				book.Close();
			}
			catch(Exception ex)
            {
				res = false;
				
            }
			
			return res;
        }

		private IEnumerable<Guid> ParseExcelTemplate(string template, string typePromo)
		{
			List<Guid> resultList = new List<Guid>();
			SpreadsheetDocument book;
			var stringPath = Path.GetDirectoryName(template);
			var stringName = Path.GetFileName(template);
            byte[] resAzure = AzureBlobHelper.ReadExcelFromBlob(stringPath.Split('\\').Last(), stringName);
            if (resAzure.Length == 0)
            {
                book = SpreadsheetDocument.Open(template, false);
            }
            else
            {
                book = SpreadsheetDocument.Open(new MemoryStream(resAzure), false);
            }
            WorkbookPart workbookPart = book.WorkbookPart;
			WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();

			OpenXmlReader reader = OpenXmlReader.Create(worksheetPart);
			SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
			int promoNumber;
			List<int> processedPromoes = new List<int>(); 
			foreach (Row r in sheetData.Elements<Row>())
			{
				if (r.RowIndex != 1)
				{
					Cell c = r.Elements<Cell>().ElementAt(0);
					if (Int32.TryParse(c.CellValue.Text, out promoNumber))
					{
						
						if (typePromo == "Actuals") { 
							var promo = Context.Set<Promo>().FirstOrDefault(p => p.Number == promoNumber && !p.Disabled);
							if (promo != null)
							{
								resultList.Add(promo.Id);
							}
						}
						else 
                        {
							if (!processedPromoes.Contains(promoNumber)) { 
								var promo = Context.Set<PromoSupport>().FirstOrDefault(p => p.Number == promoNumber && !p.Disabled);
								if (promo != null)
								{
									resultList.Add(promo.Id);
									processedPromoes.Add(promo.Number);
								}
							}
						}
						
					}
				}
			}
			return resultList;
		}

		private bool BlockPromo(Guid promoId, bool safe = false)
		{
			bool promoAvaible = false;
			bool promoFinished = false;

			try
			{
				lock (locker)
				{
					using (DatabaseContext context = new DatabaseContext())
					{
						promoAvaible = !context.Set<BlockedPromo>().Any(n => n.PromoId == promoId && !n.Disabled);
						promoFinished = context.Set<Promo>().Any(n => n.Id == promoId && n.PromoStatus.SystemName == "Finished");

						if (promoAvaible && promoFinished)
						{
							BlockedPromo bp = new BlockedPromo
							{
								Id = Guid.NewGuid(),
								PromoId = promoId,
								HandlerId = Guid.Empty,
								CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
								Disabled = false,
							};

							context.Set<BlockedPromo>().Add(bp);
							context.SaveChanges();
						}
					}
				}
			}
			catch
			{
				promoAvaible = false;
			}

			return promoAvaible && promoFinished;
		}

		/// <summary>
		/// Создание отложенной задачи, выполняющей расчет фактических параметров продуктов и промо
		/// </summary>
		/// <param name="promoId">ID промо</param>
		private async Task<Guid> CreateHandlerAsync(Guid promoId, Guid rpaId)
		{
			// к этому моменту промо уже заблокировано
			using (DatabaseContext context = new DatabaseContext())
			{
				HandlerData data = new HandlerData();
				HandlerDataHelper.SaveIncomingArgument("PromoId", promoId, data, visible: false, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("UserId", user.Id, data, visible: false, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("needRedistributeLSV", true, data, visible: false, throwIfNotExists: false);

				LoopHandler handler = new LoopHandler()
				{
					Id = Guid.NewGuid(),
					ConfigurationName = "PROCESSING",
					Status = "INPROGRESS",
					Description = "Calculate actual parameters",
					Name = "Module.Host.TPM.Handlers.CalculateActualParamatersHandler",
					ExecutionPeriod = null,
					CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
					LastExecutionDate = null,
					NextExecutionDate = null,
					ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
					UserId = user.Id,
					RoleId = roleId
				};

				BlockedPromo bp = context.Set<BlockedPromo>().First(n => n.PromoId == promoId && !n.Disabled);
				bp.HandlerId = handler.Id;
				
				handler.SetParameterData(data);
				context.LoopHandlers.Add(handler);

				await context.SaveChangesAsync();
				
				return handler.Id;
			}
		}

		

		private async Task CreateCalculationTaskAsync(string fileName, Guid rpaId)
		{
			string defaultSchema = AppSettingsManager.GetSetting("DefaultSchema", "");
			List<Guid> handlerIds = new List<Guid>();
			//Распарсить ексельку и вытащить id промо
			var listPromoId = ParseExcelTemplate(fileName,"Actuals");			

			//Вызвать блокировку promo и затем вызвать создание Task			
			foreach (Guid promoId in listPromoId)
			{
				if (BlockPromo(promoId))
				{
					var handlerId = await CreateHandlerAsync(promoId, rpaId);
					handlerIds.Add(handlerId);
				}
			}
			var tasks = "";
			if (handlerIds.Count() > 0)
				tasks = $"{String.Join(",", handlerIds.Select(el => $"''{el}''"))}";
			

			string insertScript = String.Format("INSERT INTO {0}.[RPAParameters] ([RPAId],[TasksToComplete]) VALUES ('{1}', '{2}')", defaultSchema, rpaId, tasks);

			await Context.Database.ExecuteSqlCommandAsync(insertScript);
		}

		private string FromListToString(List<Guid> list)
		{
			string result = "";

			if (list != null)
				foreach (Guid el in list.Distinct())
					result += el + ";";

			return result;
		}

		private async Task CreateCalculationPromoSupportTaskAsync(string fileName, Guid rpaId)
        {
			string defaultSchema = AppSettingsManager.GetSetting("DefaultSchema", "");
			List<Guid> handlerIds = new List<Guid>();
			//Распарсить ексельку и вытащить id промо
			var listPromoIds = ParseExcelTemplate(fileName,"PromoSupport").ToList();

			//Вызвать блокировку promo и затем вызвать создание Task
			HandlerData data = new HandlerData();
			HandlerDataHelper.SaveIncomingArgument("PromoSupportIds", FromListToString(listPromoIds), data, visible: false, throwIfNotExists: false);
			HandlerDataHelper.SaveIncomingArgument("UnlinkedPromoIds", "", data, visible: false, throwIfNotExists: false);
			HandlerDataHelper.SaveIncomingArgument("UserId", user.Id, data, visible: false, throwIfNotExists: false);
			HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
			var handlerId = RPAUploadCalculationTaskManager.CreateCalculationTask(data, Context);
			handlerIds.Add(handlerId);
			
			var tasks = "";
			if (handlerIds.Count() > 0)
				tasks = $"{String.Join(",", handlerIds.Select(el => $"''{el}''"))}";


			string insertScript = String.Format("INSERT INTO {0}.[RPAParameters] ([RPAId],[TasksToComplete]) VALUES ('{1}', '{2}')", defaultSchema, rpaId, tasks);

			await Context.Database.ExecuteSqlCommandAsync(insertScript);
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
