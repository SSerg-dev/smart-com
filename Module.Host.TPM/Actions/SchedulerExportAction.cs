using Core.Settings;
using Interfaces.Implementation.Action;
using LinqToQuerystring;
using Looper.Parameters;
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

        private readonly Guid HandlerId;

        private readonly string RawFilters;

        protected readonly static Logger logger = LogManager.GetCurrentClassLogger();

        public SchedulerExportAction(IEnumerable<int> clients, int year, Guid userId, Guid roleId, string rawFilters, Guid handlerId) {
            Clients = clients;
            Year = year;
            UserId = userId;
            RoleId = roleId;
            HandlerId = handlerId;
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

                    if (sql.Contains("p__linq"))
                    {
                        sql.Replace("@p__linq__0", RoleId.ToString());
                    }

                    if (promoes.Count() == 0)
                    {
                        Errors.Add("No promoes to export");
                    }
                    else
                    {
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


                        string ServerName = AppSettingsManager.GetSetting("ServerName", "");
                        string DatabaseName = AppSettingsManager.GetSetting("DatabaseName", "");
                        string DBUserId = AppSettingsManager.GetSetting("DBUserId", "");
                        string PasswordKV = AppSettingsManager.GetSetting("PasswordKV", "");
                        string AKVScope = AppSettingsManager.GetSetting("AKVScope", "");
                        string BlobStorageName = AppSettingsManager.GetSetting("BlobStorageName", "");

                        string fileName = GetExportFileName(userName);

                        string clients = String.Join(",", Clients);

                        FileModel file = new FileModel()
                        {
                            LogicType = "Export",
                            Name = System.IO.Path.GetFileName(fileName),
                            DisplayName = System.IO.Path.GetFileName(fileName)
                        };
                        Results.Add("ExportFile", file);

                        Dictionary<string, object> parameters = null;


                        parameters = new Dictionary<string, object>
                                    {
                                        { "QueryId", exportQuery.Id.ToString() },
                                        { "Schema", schemaDB},
                                        { "TaskId", HandlerId.ToString() },
                                        { "FileName", fileName },
                                        { "ServerName", ServerName },
                                        { "DatabaseName", DatabaseName },
                                        { "DBUserId", DBUserId },
                                        { "PasswordKV", PasswordKV },
                                        { "AKVScope", AKVScope },
                                        { "BlobStorageName", BlobStorageName },
                                        { "Clients", clients},
                                        { "Year", Year},
                                        { "YearExport", yearExport}
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

        private string GetExportFileName(string userName)
        {
            string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
            string userShortName = GetUserName(userName);
            string filename = String.Format("{0}_{1}_{2:yyyyMMddHHmmss}.xlsx", "Schedule", userShortName, DateTime.Now);
            /*
            if (!Directory.Exists(exportDir))
            {
                Directory.CreateDirectory(exportDir);
            }
            return Path.Combine(exportDir, filename);
            */
            return filename;
        }

        private string GetUserName(string userName)
        {
            string[] userParts = userName.Split(new char[] { '/', '\\' });
            return userParts[userParts.Length - 1];
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