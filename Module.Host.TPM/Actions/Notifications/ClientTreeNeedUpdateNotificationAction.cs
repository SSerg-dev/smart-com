using System;
using Persist;
using Core.Settings;
using Core.Dependency;
using System.IO;
using Module.Persist.TPM.Model.TPM;
using System.Linq;
using System.Collections.Generic;
using Module.Persist.TPM.Utils;
using Module.Persist.TPM.Model.SimpleModel;
using Microsoft.Ajax.Utilities;

namespace Module.Host.TPM.Actions.Notifications
{
    /// <summary>
    /// Класс для формирования и рассылки уведомления о необходимости создания нового клиента
    /// </summary>
    public class ClientTreeNeedUpdateNotificationAction : BaseNotificationAction
    {
        public override void Execute()
        {
            try
            {
                using (DatabaseContext context = new DatabaseContext())
                {
                    ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                    string templateFileName = settingsManager.GetSetting<string>("CLIENTTREE_NEED_UPDATE_NOTIFICATION_TEMPLATE_FILE", "ClientTreeNeedUpdateTemplate.txt");
                    if (File.Exists(templateFileName))
                    {
                        string template = File.ReadAllText(templateFileName);
                        if (!String.IsNullOrEmpty(template))
                        {
                            var incidentsForNotify = context.Set<ClientTreeNeedUpdateIncident>().Where(x => x.ProcessDate == null && x.PropertyName == "ZCUSTHG04").GroupBy(x => x.PropertyName);
							if (incidentsForNotify.Any())
							{
								CreateNotification(incidentsForNotify, "CLIENTTREE_NEED_UPDATE_NOTIFICATION", template, context);
							}
							else
							{
								Warnings.Add(String.Format("There are no incidents to send notifications."));
							}
                        }
                        else
                        {
                            Errors.Add(String.Format("Empty alert template: {0}", templateFileName));
                        }
                    }
                    else
                    {
                        Errors.Add(String.Format("Could not find alert template: {0}", templateFileName));
                    }
                }
            }
            catch (Exception e)
            {
                string msg = String.Format("An error occurred while sending a notification via ClientTreeNeedUpdate Incident: {0}", e.ToString());
                logger.Error(msg);
                Errors.Add(msg);
            }
            finally
            {
                logger.Trace("Finish");
            }
        }
        /// <summary>
        /// Формирование и отправка письма
        /// </summary>
        /// <param name="incidentsForNotify"></param>
        /// <param name="notificationName"></param>
        /// <param name="template"></param>
        /// <param name="context"></param>
        private void CreateNotification(IQueryable<IGrouping<string, ClientTreeNeedUpdateIncident>> incidentsForNotify, string notificationName, string template, DatabaseContext context)
        {
            List<string> allRows = new List<string>();
            foreach (IGrouping<string, ClientTreeNeedUpdateIncident> incidentGroup in incidentsForNotify)
            {
                foreach (ClientTreeNeedUpdateIncident incident in incidentGroup)
                {
                    allRows.AddRange(GetRowsWithNewClientTree(incident, context));
                    incident.ProcessDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                }
            }
            string notifyBody = String.Format(template, string.Join("", allRows));
            SendNotification(notifyBody, notificationName);
            Results.Add(String.Format("Notifications about new client trees were sent"), null);

            context.SaveChanges();
        }

        private List<string> GetRowsWithNewClientTree(ClientTreeNeedUpdateIncident incident, DatabaseContext context)
        {
            List<string> rowsForIncident = new List<string>();
            
            // добавляется три узла
            if (incident.PropertyName == propertyNames[2])
            {
                var selectScript = String.Format(@"
                        SELECT ZCUSTHG02, ZCUSTHG02___T, ZCUSTHG03, ZCUSTHG03___T, ZCUSTHG04, ZCUSTHG04___T
                        FROM [DefaultSchemaSetting].MARS_UNIVERSAL_PETCARE_CUSTOMERS
                        WHERE ZCUSTHG04 = '{0}' AND Active_Till IS NULL
                        ", incident.PropertyValue);
                var marsCustomersItem = context.SqlQuery<MarsUniversalPetcareCustomers>(selectScript).DistinctBy(x => x.ZCUSTHG04).ToList();//.AsEnumerable();

                foreach (var item in marsCustomersItem)
                {
                    List<string>  allRowCells = new List<string>();
                    string fullName = $"{item.ZCUSTHG02___T} > {item.ZCUSTHG03___T} > {item.ZCUSTHG04___T}";
                    allRowCells.Add(String.Format(cellTemplate, fullName));
                    allRowCells.Add(String.Format(cellTemplate, item.ZCUSTHG04));
                    allRowCells.Add(String.Format(cellTemplate, "Chain"));
                    allRowCells.Add(String.Format(cellTemplate, incident.CreateDate.ToString("dd.MM.yyyy HH:mm:ss")));

                    rowsForIncident.Add(String.Format(rowTemplate, string.Join("", allRowCells)));
                }
            }
            return rowsForIncident;
        }

        private readonly string[] propertyNames = new string[] {
            "ZCUSTHG02", "ZCUSTHG03", "ZCUSTHG04" };
    }
}
