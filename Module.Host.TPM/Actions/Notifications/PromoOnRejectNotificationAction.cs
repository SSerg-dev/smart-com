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
	/// Класс для формирования и рассылки уведомления по действию Reject промо
	/// </summary>
	public class PromoOnRejectNotificationAction : BaseNotificationAction {
		public override void Execute () {
			try {
				using (DatabaseContext context = new DatabaseContext()) {
					ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
					string templateFileName = settingsManager.GetSetting<string>("PROMO_ON_REJECT_NOTIFICATION_TEMPLATE_FILE", "PromoOnRejectTemplate.txt");
					if (File.Exists(templateFileName)) {
						string template = File.ReadAllText(templateFileName);
						if (!String.IsNullOrEmpty(template)) {
							var notifyIncidents = context.Set<PromoOnRejectIncident>().Where(x => x.ProcessDate == null && !x.Promo.Disabled);
							var notifyIncidentsByCreator = notifyIncidents.GroupBy(y => y.Promo.CreatorId);

							if (notifyIncidentsByCreator.Any()) {
								CreateNotification(notifyIncidentsByCreator, notifyIncidents, "PROMO_ON_REJECT_NOTIFICATION", template, context);
							} else {
								Warnings.Add(String.Format("There are no incidents to send notifications."));
							}
						} else {
							Errors.Add(String.Format("Empty alert template: {0}", templateFileName));
						}
					} else {
						Errors.Add(String.Format("Could not find alert template: {0}", templateFileName));
					}
				}
			}
			catch (Exception e) {
				string msg = String.Format("An error occurred while sending a notification via Promo On Reject Incident: {0}", e.ToString());
				logger.Error(msg);
				Errors.Add(msg);
			}
			finally {
				logger.Trace("Finish");
			}
		}
		/// <summary>
		/// Формирование и отправка письма
		/// </summary>
		/// <param name="incidentsForNotifyByCreator"></param>
		/// <param name="notificationName"></param>
		/// <param name="template"></param>
		/// <param name="context"></param>
		private void CreateNotification (IQueryable<IGrouping<Guid?, PromoOnRejectIncident>> incidentsForNotifyByCreator, IQueryable<PromoOnRejectIncident> incidentsForNotify, string notificationName, string template, DatabaseContext context) {
			var creatorsEmails = new List<string>();
			var promoNumbers = new List<string>();

			Results.Add(String.Format("Sending notifications to promo creators."), null);
			foreach (IGrouping<Guid?, PromoOnRejectIncident> incidentsGroup in incidentsForNotifyByCreator) {
				Guid? creatorId = incidentsGroup.Key != null ? incidentsGroup.Key : Guid.Empty;
				if (creatorId.Equals(Guid.Empty)) {
					foreach (PromoOnRejectIncident incident in incidentsGroup) {
						promoNumbers.Add(incident.Promo.Number.ToString());
					}
					Warnings.Add(String.Format("Promo creator not specified or not found. Promo numbers: {0}.", String.Join(", ", promoNumbers.Distinct())));
				}

				string creatorEmail = context.Users.Where(x => x.Id == creatorId && !x.Disabled).Select(y => y.Email).FirstOrDefault();
				if (String.IsNullOrEmpty(creatorEmail) && !creatorId.Equals(Guid.Empty)) {
					promoNumbers = new List<string>();
					foreach (PromoOnRejectIncident incident in incidentsGroup) {
						promoNumbers.Add(incident.Promo.Number.ToString());
					}
					Warnings.Add(String.Format("Promo creator's email not found. Promo numbers: {0}.", String.Join(", ", promoNumbers.Distinct())));
				} else {
					creatorsEmails.Add(creatorEmail);
				}

				var allRows = new List<string>();
				promoNumbers = new List<string>();
				foreach (PromoOnRejectIncident incident in incidentsGroup) {
					List<string> allRowCells = GetRow(incident.Promo, propertiesOrder);
					allRowCells.Add(String.Format(cellTemplate, incident.UserLogin));
					allRows.Add(String.Format(rowTemplate, string.Join("", allRowCells)));
					incident.ProcessDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
					promoNumbers.Add(incident.Promo.Number.ToString());
				}

				string notifyBody = String.Format(template, string.Join("", allRows));

				string[] emailArray = new string[] { creatorEmail };
				if (!String.IsNullOrEmpty(creatorEmail)) {
					SendNotificationByEmails(notifyBody, notificationName, emailArray);
					Results.Add(String.Format("Notification about reject of promoes with numbers: {0} were sent to promo creator {1}.",
						String.Join(", ", promoNumbers.Distinct()), String.Join(", ", emailArray)), null);
				} else {
					Warnings.Add(String.Format("Promo creator's email not found. Promo numbers: {0}.", String.Join(", ", promoNumbers.Distinct())));
				}
			}

			// Отправка нотификаций для Recipients и Settings без проверок
			Results.Add(String.Format("Sending notifications to users from recipients of {0}.", notificationName), null);
			if (incidentsForNotify.Any()) {
				List<Recipient> recipients = NotificationsHelper.GetRecipientsByNotifyName(notificationName, context);
				IList<string> userErrors;
				List<Guid> userIds = NotificationsHelper.GetUserIdsByRecipients(notificationName, recipients, context, out userErrors);

				if (userErrors.Any()) {
					foreach (string error in userErrors) {
						Warnings.Add(error);
					}
				}

				List<string> defaultRecipients = NotificationsHelper.GetUsersEmail(userIds, context);
				List<string> allRows = new List<string>();
				promoNumbers = new List<string>();
				foreach (PromoOnRejectIncident incident in incidentsForNotify) {
					List<string> allRowCells = GetRow(incident.Promo, propertiesOrder);
					allRowCells.Add(String.Format(cellTemplate, incident.UserLogin));
					allRows.Add(String.Format(rowTemplate, string.Join("", allRowCells)));
					incident.ProcessDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
					promoNumbers.Add(incident.Promo.Number.ToString());
				}

				string notifyBody = String.Format(template, string.Join("", allRows));

				string[] emailArray = defaultRecipients.Distinct().Except(creatorsEmails).ToArray();
				if (emailArray.Length > 0) {
					SendNotificationByEmails(notifyBody, notificationName, emailArray);
					Results.Add(String.Format("Notification about reject of promoes with numbers: {0} were sent to {1}.",
						String.Join(", ", promoNumbers.Distinct()), String.Join(", ", emailArray)), null);
				}
			}
			
			context.SaveChanges();
		}

		private readonly string[] propertiesOrder = new string[] {
			"ClientHierarchy", "Number", "Name", "PromoStatus.Name", "StartDate", "EndDate", "DispatchesStart", "DispatchesEnd"  };
	}
}
