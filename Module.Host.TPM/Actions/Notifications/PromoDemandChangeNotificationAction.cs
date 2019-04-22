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
    /// Класс для формирования и рассылки уведомления по Promo Demand Change
    /// </summary>
    public class PromoDemandChangeNotificationAction : BaseNotificationAction {
        public override void Execute() {
            try {
                using (DatabaseContext context = new DatabaseContext()) {
                    ISettingsManager settingsManager = (ISettingsManager) IoC.Kernel.GetService(typeof(ISettingsManager));
                    string templateFileName = settingsManager.GetSetting<string>("PROMO_DEMAND_CHANGE_NOTIFICATION_TEMPLATE_FILE", "PromoDemandChangeTemplate.txt");
                    if (File.Exists(templateFileName)) {
                        string template = File.ReadAllText(templateFileName);
                        if (!String.IsNullOrEmpty(template)) {
                            IQueryable<PromoDemandChangeIncident> incidentsForNotify = context.Set<PromoDemandChangeIncident>().Where(x => x.ProcessDate == null).OrderBy(x => x.PromoIntId);
                            if (incidentsForNotify.Any()) {
                                CreateNotification(incidentsForNotify, "PROMO_DEMAND_CHANGE_NOTIFICATION", template, context);
                            }
                        } else {
                            Errors.Add(String.Format("Empty alert template: {0}", templateFileName));
                        }
                    } else {
                        Errors.Add(String.Format("Could not find alert template: {0}", templateFileName));
                    }
                }
            } catch (Exception e) {
                string msg = String.Format("An error occurred while sending the notification via PromoDemandChangeIncident: {0}", e.ToString());
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
        private void CreateNotification(IQueryable<PromoDemandChangeIncident> incidentsForNotify, string notificationName, string template, DatabaseContext context) {
            List<string> allRows = new List<string>();
            foreach (PromoDemandChangeIncident incident in incidentsForNotify) {
                IDictionary<string, object> record = GetDictionary(incident);
                List<string> allRowCells = GetRow(incident, propertiesOrder);
                allRows.Add(String.Format(rowTemplate, string.Join("", allRowCells)));
                incident.ProcessDate = DateTimeOffset.Now;
            }
            string notifyBody = String.Format(template, string.Join("", allRows));
            SendNotification(notifyBody, notificationName);
            context.SaveChanges();
        }

        private readonly string[] propertiesOrder = new string[] { // TODO: вынести в настройки. БД - нужно изменение длины поля (ядро), файл, конфиг?
            "PromoIntId", "Name", "ClientHierarchy", "BrandTech", "OldPromoStatus", "NewPromoStatus", "OldMarsMechanic", "NewMarsMechanic", "OldPlanInstoreMechanic",
            "NewPlanInstoreMechanic", "OldMarsMechanicDiscount", "NewMarsMechanicDiscount", "OldPlanInstoreMechanicDiscount", "NewPlanInstoreMechanicDiscount",
            "OldStartDate", "NewStartDate", "OldEndDate", "NewEndDate", "OldDispatchesStart", "NewDispatchesStart", "OldDispatchesEnd", "NewDispatchesEnd",
            "OldPlanPromoUpliftPercent", "NewPlanPromoUpliftPercent", "OldPlanPromoIncrementalLSV", "NewPlanPromoIncrementalLSV", "OldPlanSteel", "NewPlanSteel", "OldOutletCount",
            "NewOutletCount", "OldXSite", "NEWXSite", "OldCatalogue", "NEWCatalogue", "IsProductListChange" };
    }
}