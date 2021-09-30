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

namespace Module.Frontend.TPM.Controllers
{
	public class RPAsController : EFContextController
	{
		private readonly IAuthorizationManager authorizationManager;
		private Core.Security.Models.UserInfo user;
		private string role;
		private IList<Constraint> constraints;
		public RPAsController(IAuthorizationManager authorizationManager)
		{
			this.authorizationManager = authorizationManager;
			this.user = authorizationManager.GetCurrentUser();
			this.role = authorizationManager.GetCurrentRoleName();
			
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
		public IHttpActionResult SaveRPA()
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

					// Save RPA

					result.Constraint = String.Join(";", this.constraints.Select(x => x.Value).ToArray());
					result.CreateDate = DateTime.UtcNow;
					result.FileURL = $"<a href=https://russiapetcarejupiterdev2wa.azurewebsites.net//odata/RPA/DownloadFile?fileName={Path.GetFileName(fileName)} download>Download file</a>";                    
					var resultSaveChanges = Context.SaveChanges();


					//Call Pipe
					string tenantID = AppSettingsManager.GetSetting("RPA_UPLOAD_TENANT_ID", "");
					string applicationId = AppSettingsManager.GetSetting("RPA_UPLOAD_APPLICATION_ID", "");
					string authenticationKey = AppSettingsManager.GetSetting("RPA_UPLOAD_AUTHENTICATION_KEY", "");
					string subscriptionId = AppSettingsManager.GetSetting("RPA_UPLOAD_SUBSCRIPTION_ID", "");
					string resourceGroup = AppSettingsManager.GetSetting("RPA_UPLOAD_RESOURCE_GROUP", "");
					string dataFactoryName = AppSettingsManager.GetSetting("RPA_UPLOAD_DATA_FACTORY_NAME", "");
					string pipelineName = AppSettingsManager.GetSetting("RPA_UPLOAD_PIPELINE_NAME", "");
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
						Dictionary<string, object> parameters = new Dictionary<string, object>
							{
								{ "FileName", Path.GetFileName(fileName) },
								{ "RPAId", result.Id },
								{ "UserRoleName", this.user.GetCurrentRole().SystemName },
								{ "UserId", this.user.Id },
							};
						CreateRunResponse runResponse = client.Pipelines.CreateRunWithHttpMessagesAsync(
							resourceGroup, dataFactoryName, pipelineName, parameters: parameters
						).Result.Body;

					}
					catch (Exception ex)
					{
						result.Status = "Pipe not availabale";
						Context.SaveChanges();
						return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, message = "RPA save and upload failure " + ex.Message }));

					}

			}
			catch (Exception e)
			{
				return GetErorrRequest(e);
				
			}

			return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, message = "RPA save and upload done." }));
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
			// обработка при создании дублирующей записи
			SqlException exc = e.GetBaseException() as SqlException;

			if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
			{
				return InternalServerError(new Exception("This RPA has already existed"));
			}
			else
			{
				return InternalServerError(e.InnerException);
			}
		}

	}
}
