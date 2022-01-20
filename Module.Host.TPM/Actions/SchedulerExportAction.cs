using System;
using Persist;
using Module.Persist.TPM.Model.TPM;
using System.Linq;
using System.Collections.Generic;
using Persist.Model;
using Utility;
using Module.Persist.TPM.Model.DTO;
using Persist.ScriptGenerator.Filter;
using Module.Persist.TPM.Utils;
using Looper.Parameters;
using Interfaces.Implementation.Action;
using NLog;
using System.Web.Http.OData.Query;
using System.Linq.Expressions;
using LinqToQuerystring;
using System.Text.RegularExpressions;
using System.Data.Entity.Core.Objects;
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

namespace Module.Host.TPM.Actions.Notifications {
    /// <summary>
    /// Класс для экспорта календаря в EXCEL
    /// </summary>
    public class SchedulerExportAction : BaseAction {
        private readonly IEnumerable<int> Clients;

        private readonly int Year;

        private readonly Guid UserId;

        private readonly Guid RoleId;

        private readonly string RawFilters;

        protected readonly static Logger logger = LogManager.GetCurrentClassLogger();

        public SchedulerExportAction(IEnumerable<int> clients, int year, Guid userId, Guid roleId, string rawFilters) {
            Clients = clients;
            Year = year;
            UserId = userId;
            RoleId = roleId;
            RawFilters = rawFilters;
        }
        public override void Execute() {
            PerformanceLogger performanceLogger = new PerformanceLogger();
            try {
                performanceLogger.Start();
                using (DatabaseContext context = new DatabaseContext()) {

                    IQueryable<PromoView> query = (GetConstraintedQuery(context));

                    
                    IQueryable<PromoView> promoes = query.Cast<PromoView>();
                    var objQuery = (ObjectQuery<PromoView>)promoes;
                    //из библиотеки LinqToQuerystring нашей версии убрали datetimeoffset заменяем
                    var row = RawFilters.Replace("datetimeoffset", "datetime").Replace(".000Z", "").Replace(".00Z", "");
                    //из библиотеки LinqToQuerystring нашей версии нет данных в вииде 12d что есть в нашей версии odata заменяем
                    row = Regex.Replace(row, @"(\d+)[d]", @"$1");
                    promoes = promoes.LinqToQuerystring(row);

                    objQuery = (ObjectQuery<PromoView>)promoes;

                    DateTime startDate = DateTime.Now;
                    DateTime endDate = DateTime.Now;
                    bool yearExport = Year != 0;

                    if (yearExport) {
                        startDate = new DateTime(Year, 1, 1);
                        endDate = new DateTime(Year, 12, 31);
                        promoes = promoes.Where(p => (p.EndDate > startDate && p.EndDate < endDate) || (p.StartDate > startDate && p.StartDate < endDate));
                    }
                    if (promoes.Count() == 0) {
                        Errors.Add("No promoes to export");
                    } else {
                        string userName = context.Users.FirstOrDefault(u => u.Id == UserId).Name;

                        ExportQuery exportQuery = new ExportQuery();

                        objQuery = (ObjectQuery<PromoView>)promoes;
                        var sql = objQuery.ToTraceString();

                        exportQuery.Parameters = "";
                        exportQuery.Text = promoes.ToString();
                        exportQuery.Type = "";
                        exportQuery.Disabled = false;

                        context.Set<ExportQuery>().Add(exportQuery);

                        context.SaveChanges();
                        //SchedulerExporter exporter = yearExport ? new SchedulerExporter(startDate, endDate) : new SchedulerExporter();
                        //string filePath = exporter.GetExportFileName(userName);
                        //exporter.Export(promoes.ToList(), Clients, filePath, context);
                        //string fileName = System.IO.Path.GetFileName(filePath);

                        /*
                        FileModel file = new FileModel() {
                            LogicType = "Export",
                            Name = System.IO.Path.GetFileName(fileName),
                            DisplayName = System.IO.Path.GetFileName(fileName)
                        };
                        Results.Add("ExportFile", file);
                        */


                    }
                }
            } catch (Exception e) {
                string msg = String.Format("Error exporting calendar: {0}", e.ToString());
                logger.Error(msg);
                Errors.Add(msg);
            } finally {
                logger.Trace("Finish");
                performanceLogger.Stop();
            }
        }

        /*
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
        */

        /// <summary>
        /// Получение списка промо с учётом ограничений
        /// </summary>
        /// <returns></returns>
        private IQueryable<PromoView> GetConstraintedQuery(DatabaseContext context) {
            PerformanceLogger performanceLogger = new PerformanceLogger();
            performanceLogger.Start();
            string role = context.Roles.FirstOrDefault(r => r.Id == RoleId).SystemName;
            IList<Constraint> constraints = context.Constraints
                .Where(x => x.UserRole.UserId.Equals(UserId) && x.UserRole.Role.SystemName.Equals(role))
                .ToList();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<PromoView> query = context.Set<PromoView>();
            IQueryable<ClientTreeHierarchyView> hierarchy = context.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters, FilterQueryModes.Active, String.Empty);
            // Не администраторы не смотрят чужие черновики
            if (role != "Administrator" && role != "SupportAdministrator")
            {
                query = query.Where(e => e.PromoStatusSystemName != "Draft" || e.CreatorId == UserId);
            }
            performanceLogger.Stop();
            return query;
        }
    }
}