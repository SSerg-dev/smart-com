using System;
using Persist;
using Core.Settings;
using Core.Dependency;
using System.IO;
using Module.Persist.TPM.Model.TPM;
using System.Linq;
using System.Collections.Generic;
using Module.Persist.TPM.Utils;
using Persist.Model.Settings;

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
                            var incidentsForNotify = context.Set<PromoUpliftFailIncident>()
								.Where(x => x.ProcessDate == null && x.Promo.PromoStatus.SystemName != "Draft" && !x.Promo.Disabled).GroupBy(x => x.Promo.Number);

                            if (incidentsForNotify.Any()) 
							{
                                CreateNotification(incidentsForNotify, "PROMO_UPLIFT_FAIL_NOTIFICATION", template, context);
                            }
							else
							{
								Warnings.Add("There are no incidents to send notifications.");
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
        private void CreateNotification(IQueryable<IGrouping<int?, PromoUpliftFailIncident>> incidentsForNotify, string notificationName, string template, DatabaseContext context) {
            List<string> allRows = new List<string>();
			IList<string> promoNumbers = new List<string>();
			foreach (IGrouping<int?, PromoUpliftFailIncident> incidentGroup in incidentsForNotify) {
				PromoUpliftFailIncident incident = incidentGroup.FirstOrDefault();
				if (incident != null)
				{
					List<string> allRowCells = GetRow(incidentGroup.FirstOrDefault().Promo, propertiesOrder);
					allRows.Add(String.Format(rowTemplate, string.Join("", allRowCells)));
					
					promoNumbers.Add(incidentGroup.Key.ToString());
				}
				foreach (var item in incidentGroup)
				{
					item.ProcessDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
				}
			}
            string notifyBody = String.Format(template, string.Join("", allRows));
            SendNotification(notifyBody, notificationName);
			context.SaveChanges();

			//Получаем получателей для лога
			IList<string> userErrors;
			List<Recipient> recipients = ConstraintsHelper.GetRecipientsByNotifyName(notificationName, context);
			List<Guid> userIds = ConstraintsHelper.GetUserIdsByRecipients(notificationName, recipients, context, out userErrors);

			if (userErrors.Any())
			{
				foreach (string error in userErrors)
				{
					Warnings.Add(error);
				}
			}
			else if (!userIds.Any())
			{
				Warnings.Add(String.Format("There are no appropriate recipinets for notification: {0}.", notificationName));
				return;
			}

			string[] userEmails = context.Users.Where(x => userIds.Contains(x.Id) && !String.IsNullOrEmpty(x.Email)).Select(x => x.Email).ToArray();
			Results.Add(String.Format("Notifications about fail of uplift culculation for promoes {0} were sent to {1}.", String.Join(", ", promoNumbers.Distinct().ToArray()), String.Join(", ", userEmails)), null);
		}

        private readonly string[] propertiesOrder = new string[] {
            "Number", "Name", "BrandTech.Name", "PromoStatus.Name", "StartDate", "EndDate" };
    }
}