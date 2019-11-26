﻿using System;
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
	/// Класс для формирования и рассылки уведомления при смене статуса промо на Approved 
	/// </summary>
	public class PromoApprovedNotificationAction : BaseNotificationAction {
		public override void Execute () {
			try {
				using (DatabaseContext context = new DatabaseContext()) {
					ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
					string templateFileName = settingsManager.GetSetting<string>("PROMO_APPROVED_NOTIFICATION_TEMPLATE_FILE", "PromoApprovedTemplate.txt");
					if (File.Exists(templateFileName)) {
						string template = File.ReadAllText(templateFileName);
						if (!String.IsNullOrEmpty(template)) {
							var notifyIncidents = context.Set<PromoApprovedIncident>().Where(x => x.ProcessDate == null && x.Promo.PromoStatus.SystemName == "Approved" && !x.Promo.Disabled);
							var notifyIncidentsByCreator = notifyIncidents.GroupBy(y => y.Promo.CreatorId);

							if (notifyIncidentsByCreator.Any()) {
								CreateNotification(notifyIncidentsByCreator, notifyIncidents, "PROMO_APPROVED_NOTIFICATION", template, context);
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
				string msg = String.Format("An error occurred while sending a notification via Promo Approved Incident: {0}", e.ToString());
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
		private void CreateNotification (IQueryable<IGrouping<Guid?, PromoApprovedIncident>> incidentsForNotifyByCreator, IQueryable<PromoApprovedIncident> incidentsForNotify, string notificationName, string template, DatabaseContext context) {
			var creatorsEmails = new List<string>();
			var promoNumbers = new List<string>();

			Results.Add(String.Format("Sending notifications to promo creators."), null);
			foreach (IGrouping<Guid?, PromoApprovedIncident> incidentsGroup in incidentsForNotifyByCreator) {
				Guid? creatorId = incidentsGroup.Key != null ? incidentsGroup.Key : Guid.Empty;
				if (creatorId.Equals(Guid.Empty)) {
					foreach (PromoApprovedIncident incident in incidentsGroup) {
						promoNumbers.Add(incident.Promo.Number.ToString());
					}
					Warnings.Add(String.Format("Promo creator not specified or not found. Promo numbers: {0}.", String.Join(", ", promoNumbers.Distinct())));
				}

				string creatorEmail = context.Users.Where(x => x.Id == creatorId && !x.Disabled).Select(y => y.Email).FirstOrDefault();
				if (String.IsNullOrEmpty(creatorEmail) && !creatorId.Equals(Guid.Empty)) {
					promoNumbers = new List<string>();
					foreach (PromoApprovedIncident incident in incidentsGroup) {
						promoNumbers.Add(incident.Promo.Number.ToString());
					}
					Warnings.Add(String.Format("Promo creator's email not found. Promo numbers: {0}.", String.Join(", ", promoNumbers.Distinct())));
				} else {
					creatorsEmails.Add(creatorEmail);
				}

				List<string> allRows = new List<string>();
				promoNumbers = new List<string>();
				foreach (PromoApprovedIncident incident in incidentsGroup) {
					List<string> allRowCells = GetRow(incident.Promo, propertiesOrder);
					allRows.Add(String.Format(rowTemplate, string.Join("", allRowCells)));
					promoNumbers.Add(incident.Promo.Number.ToString());
					incident.ProcessDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
				}
				string notifyBody = String.Format(template, string.Join("", allRows));

				string[] emailArray = new string[] { creatorEmail };
				if (emailArray.Length > 0) {
					SendNotificationByEmails(notifyBody, notificationName, emailArray);
					Results.Add(String.Format("Notifications about transition in approved status of promoes with numbers {0} were sent to promo creator: {1}.", String.Join(", ", promoNumbers.Distinct()), String.Join(", ", emailArray)), null);
				} else {
					Warnings.Add(String.Format("There is no recipient to send notification."));
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
				foreach (PromoApprovedIncident incident in incidentsForNotify) {
					List<string> allRowCells = GetRow(incident.Promo, propertiesOrder);
					allRows.Add(String.Format(rowTemplate, string.Join("", allRowCells)));
					promoNumbers.Add(incident.Promo.Number.ToString());
					incident.ProcessDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
				}

				string notifyBody = String.Format(template, string.Join("", allRows));

				string[] emailArray = defaultRecipients.Distinct().Except(creatorsEmails).ToArray();
				if (emailArray.Length > 0) {
					SendNotificationByEmails(notifyBody, notificationName, emailArray);
					Results.Add(String.Format("Notifications about transition in approved status of promoes with numbers {0} were sent to {1}.", String.Join(", ", promoNumbers.Distinct()), String.Join(", ", emailArray)), null);
				}
			}

			context.SaveChanges();
		}

		private readonly string[] propertiesOrder = new string[] {
			"ClientHierarchy", "Number", "Name", "PromoStatus.Name", "StartDate", "EndDate", "DispatchesStart", "DispatchesEnd"  };
	}
}
