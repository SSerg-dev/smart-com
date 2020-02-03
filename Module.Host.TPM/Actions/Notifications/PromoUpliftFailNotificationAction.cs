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
using Persist.Model;
using Module.Persist.TPM.Model.DTO;
using Utility;

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
								.Where(x => x.ProcessDate == null && x.Promo.PromoStatus.SystemName != "Draft" && !x.Promo.Disabled);

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
		private void CreateNotification(IQueryable<PromoUpliftFailIncident> incidentsForNotify, string notificationName, string template, DatabaseContext context)
		{
			var notifyBody = String.Empty;
			var allRows = new List<string>();
			var logPromoNums = new List<string>();
			var logPromoEmails = new List<string>();
			var emailArray = new string[] { };

			List<Recipient> recipients = NotificationsHelper.GetRecipientsByNotifyName(notificationName, context);

			IList<string> userErrors;
			IList<string> guaranteedEmails;
			List<Guid> userIdsWithConstraints = NotificationsHelper.GetUserIdsByRecipients(notificationName, recipients, context, out userErrors, out guaranteedEmails).ToList();
			if (userErrors.Any())
			{
				foreach (string error in userErrors)
				{
					Warnings.Add(error);
				}
			}
			var emailsWithoutConstraints = new List<string>(guaranteedEmails);

			// Отправка оповещений для recipients указанных по Role и User. С ограничениями по клиенту.
			foreach (Guid userId in userIdsWithConstraints)
			{
				string userEmail = context.Users.Where(x => x.Id == userId).Select(y => y.Email).FirstOrDefault();
				if (emailsWithoutConstraints.Contains(userEmail)) { continue; }
				List<Constraint> constraints = NotificationsHelper.GetConstraitnsByUserId(userId, context);
				IEnumerable<PromoUpliftFailIncident> constraintNotifies = incidentsForNotify;

				if (constraints.Any())
				{
					IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
					IQueryable<ClientTreeHierarchyView> hierarchy = context.Set<ClientTreeHierarchyView>().AsNoTracking();
					IEnumerable<string> clientFilter = FilterHelper.GetFilter(filters, ModuleFilterName.Client);

					hierarchy = hierarchy.Where(h => clientFilter.Contains(h.Id.ToString()) || clientFilter.Any(c => h.Hierarchy.Contains(c)));
					constraintNotifies = incidentsForNotify.Where(x =>
						hierarchy.Any(h => h.Id == x.Promo.ClientTreeId || h.Hierarchy.Contains(x.Promo.ClientTreeId.Value.ToString())));
				}

				if (constraintNotifies.Any())
				{
					logPromoNums = new List<string>();
					allRows = new List<string>();
					foreach (PromoUpliftFailIncident incident in constraintNotifies)
					{
						List<string> allRowCells = GetRow(incident.Promo, propertiesOrder);
						allRows.Add(String.Format(rowTemplate, string.Join("", allRowCells)));
						logPromoNums.Add(incident.Promo.Number.ToString());
					}
					notifyBody = String.Format(template, string.Join("", allRows));

					emailArray = new[] { userEmail };
					if (!String.IsNullOrEmpty(userEmail))
					{
						SendNotificationByEmails(notifyBody, notificationName, emailArray);
						Results.Add(String.Format("Notifications about fail of uplift culculation for promoes {0} were sent to {1}.", String.Join(", ", logPromoNums.Distinct()), String.Join(", ", emailArray)), null);
					}
					else
					{
						string userLogin = context.Users.Where(x => x.Id == userId).Select(x => x.Name).FirstOrDefault();
						Warnings.Add(String.Format("Email not found for user: {0}.", userLogin));
					}
				}
			}

			// Отправка оповещений для "To" из настроек нотификации и recipients указанных по email. Без ограничений по клиенту.
			logPromoNums = new List<string>();
			allRows = new List<string>();
			foreach (PromoUpliftFailIncident incident in incidentsForNotify)
			{
				List<string> allRowCells = GetRow(incident.Promo, propertiesOrder);
				allRows.Add(String.Format(rowTemplate, string.Join("", allRowCells)));
				logPromoNums.Add(incident.Promo.Number.ToString());
			}
			notifyBody = String.Format(template, string.Join("", allRows));

			emailArray = emailsWithoutConstraints.ToArray();
			SendNotificationByEmails(notifyBody, notificationName, emailArray);
			Results.Add(String.Format("Notifications about fail of uplift culculation for promoes {0} were sent to {1}.", String.Join(", ", logPromoNums.Distinct()), String.Join(", ", emailArray)), null);
			foreach (var incident in incidentsForNotify)
			{
				incident.ProcessDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
			}
			context.SaveChanges();
		}

        private readonly string[] propertiesOrder = new string[] {
			"ClientHierarchy", "Number", "Name", "BrandTech.Name", "PromoStatus.Name", "StartDate", "EndDate" };
    }
}