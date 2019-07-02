using System;
using Persist;
using Core.Settings;
using Core.Dependency;
using System.IO;
using Module.Persist.TPM.Model.TPM;
using System.Linq;
using System.Collections.Generic;

namespace Module.Host.TPM.Actions.Notifications {
    /// <summary>
    /// Класс для формирования и рассылки уведомления по Promo Uplift Fail
    /// </summary>
    public class PromoUpliftFailNotificationAction : BaseNotificationAction {
        public override void Execute() {
            try {
                using (DatabaseContext context = new DatabaseContext()) {
                    ISettingsManager settingsManager = (ISettingsManager) IoC.Kernel.GetService(typeof(ISettingsManager));
                    string templateFileName = settingsManager.GetSetting<string>("PROMO_UPLIFT_FAIL_NOTIFICATION_TEMPLATE_FILE", "PromoUpliftFailTemplate.txt");
                    if (File.Exists(templateFileName)) {
                        string template = File.ReadAllText(templateFileName);
                        if (!String.IsNullOrEmpty(template)) {
                            var incidentsForNotify = context.Set<PromoUpliftFailIncident>().Where(x => x.ProcessDate == null).GroupBy(x => x.PromoId);
                            if (incidentsForNotify.Any()) {
                                CreateNotification(incidentsForNotify, "PROMO_UPLIFT_FAIL_NOTIFICATION", template, context);
                            }
                        } else {
                            Errors.Add(String.Format("Empty alert template: {0}", templateFileName));
                        }
                    } else {
                        Errors.Add(String.Format("Could not find alert template: {0}", templateFileName));
                    }
                }
            } catch (Exception e) {
                string msg = String.Format("An error occurred while sending a notification via Promo Uplift Fail Incident: {0}", e.ToString());
                logger.Error(msg);
                Errors.Add(msg);
            } finally {
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
        private void CreateNotification(IQueryable<IGrouping<Guid, PromoUpliftFailIncident>> incidentsForNotify, string notificationName, string template, DatabaseContext context) {
            List<string> allRows = new List<string>();
            foreach (IGrouping<Guid, PromoUpliftFailIncident> incidentGroup in incidentsForNotify) {
                List<string> allRowCells = GetRow(incidentGroup.FirstOrDefault().Promo, propertiesOrder);
                allRows.Add(String.Format(rowTemplate, string.Join("", allRowCells)));
                foreach (PromoUpliftFailIncident incident in incidentGroup) {
                    incident.ProcessDate = DateTimeOffset.Now;
                }
            }
            string notifyBody = String.Format(template, string.Join("", allRows));
            SendNotification(notifyBody, notificationName);
            context.SaveChanges();
        }

        private readonly string[] propertiesOrder = new string[] {
            "Number", "Name", "BrandTech.Name", "PromoStatus.Name", "StartDate", "EndDate" };
    }
}