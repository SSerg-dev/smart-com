using Core.Settings;
using Interfaces.Implementation.Action;
using LinqToQuerystring;
using Microsoft.Azure.Management.DataFactory;
using Microsoft.Azure.Management.DataFactory.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using NLog;
using Persist;
using Persist.Model;
using Persist.ScriptGenerator.Filter;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text.RegularExpressions;
using Utility;

namespace Module.Host.TPM.Actions.Notifications
{
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
                    //из библиотеки LinqToQuerystring нашей версии убрали datetimeoffset заменяем
                    var row = RawFilters.Replace("$orderby=Id", "").Replace("datetimeoffset", "datetime").Replace(".000Z", "").Replace(".00Z", "");
                    //из библиотеки LinqToQuerystring нашей версии нет данных в вииде 12d что есть в нашей версии odata заменяем
                    row = Regex.Replace(row, @"(\d+)[d]", @"$1");
                    promoes = promoes.LinqToQuerystring(row);

                    DateTime startDate = DateTime.Now;
                    DateTime endDate = DateTime.Now;
                    bool yearExport = Year != 0;

                    var sql = promoes.ToString();

                    if (yearExport) {
                        startDate = new DateTime(Year, 1, 1);
                        endDate = new DateTime(Year, 12, 31);
                        promoes = promoes.Where(p => (p.EndDate > startDate && p.EndDate < endDate) || (p.StartDate > startDate && p.StartDate < endDate));

                        sql = promoes.ToString().Replace("@p__linq__0", GetDateParam(startDate)).Replace("@p__linq__1", GetDateParam(endDate)).Replace("@p__linq__2", GetDateParam(startDate)).Replace("@p__linq__3", GetDateParam(endDate));
                    }

                    string userName = context.Users.FirstOrDefault(u => u.Id == UserId).Name;

                    ExportQuery exportQuery = new ExportQuery();

                    exportQuery.Parameters = "";
                    exportQuery.Text = sql;
                    exportQuery.Type = "";
                    exportQuery.Disabled = false;

                    context.Set<ExportQuery>().Add(exportQuery);

                    context.SaveChanges();

                    string schemaDB = AppSettingsManager.GetSetting("DefaultSchema", "");

                    //Call Pipe
                    string tenantID = AppSettingsManager.GetSetting("EXPORT_TENANT_ID", "");
                    string applicationId = AppSettingsManager.GetSetting("EXPORT_APPLICATION_ID", "");
                    string authenticationKey = AppSettingsManager.GetSetting("EXPORT_AUTHENTICATION_KEY", "");
                    string subscriptionId = AppSettingsManager.GetSetting("EXPORT_SUBSCRIPTION_ID", "");
                    string resourceGroup = AppSettingsManager.GetSetting("EXPORT_RESOURCE_GROUP", "");
                    string dataFactoryName = AppSettingsManager.GetSetting("EXPORT_DATA_FACTORY_NAME", "");
                    string pipelineName = AppSettingsManager.GetSetting("EXPORT_PIPELINE_NAME", "");

                    //tenantID = "2fc13e34-f03f-498b-982a-7cb446e25bc6";
                    //applicationId = "6f231dc9-7560-4d58-8655-a9fec42cac17";
                    //authenticationKey = "kKOHIvUL0nKvtQ4e2Fn53RvYLba5Ne7/pKphScyEg8c=";
                    //subscriptionId = "066fc627-241c-4166-b66f-70f51b9b4b95";
                    //resourceGroup = "RUSSIA-PETCARE-JUPITER-DEV-RG";
                    //dataFactoryName = "russiapetcarejupiterdevadf";
                    //pipelineName = "JUPITER_EXPORT_SCHEDULER_DISPATCHER_PIPE";

                    Dictionary<string, object> parameters = null;

                        
                    parameters = new Dictionary<string, object>
                                    {
                                        { "QueryId", exportQuery.Id.ToString() },
                                        { "Schema", schemaDB}
                                    };

                    var aucontext = new AuthenticationContext("https://login.microsoftonline.com/" + tenantID);
                    ClientCredential cc = new ClientCredential(applicationId, authenticationKey);
                    AuthenticationResult authenticationResult = aucontext.AcquireTokenAsync(
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
            } catch (Exception e) {
                string msg = String.Format("Error exporting calendar: {0}", e.ToString());
                logger.Error(msg);
                Errors.Add(msg);
            } finally {
                logger.Trace("Finish");
                performanceLogger.Stop();
            }
        }

        private String GetDateParam(DateTime dt)
        {
            return String.Concat("'", dt.ToString("yyyy-MM-dd"), "'");
        }

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