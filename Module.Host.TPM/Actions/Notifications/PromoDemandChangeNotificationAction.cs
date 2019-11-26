using Core.Dependency;
using Core.Settings;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model;
using Persist.Model.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utility;

namespace Module.Host.TPM.Actions.Notifications
{
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
							List<PromoDemandChangeIncident> incidents = context.Set<PromoDemandChangeIncident>().Where(x => x.ProcessDate == null).OrderBy(x => x.PromoIntId).ToList();
							//Проверка на 12 недель
							List<PromoDemandChangeIncident> incidentsForNotify = new List<PromoDemandChangeIncident>();
							List<Promo> incidentsPromo = context.Set<Promo>().Where(x => !x.Disabled).ToList();
							foreach (var incident in incidents)
							{
								DateTimeOffset today = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
								DateTimeOffset after12Weeks = today.AddDays(84); // 12 недель
								Promo incidentPromo = incidentsPromo.Where(x => x.Number == incident.PromoIntId).FirstOrDefault();
								if (incidentPromo != null)
								{
									if (incidentPromo.StartDate.Value <= after12Weeks)
									{
										incidentsForNotify.Add(incident);
									}
									else
									{
										incident.ProcessDate = today;
									}
								}
							}

							if (incidentsForNotify.Any()) 
							{
                                CreateNotification(incidentsForNotify, "PROMO_DEMAND_CHANGE_NOTIFICATION", template, incidentsPromo, context);
                            }
							else
							{
								Warnings.Add(String.Format("There are no incidents to send notifications."));
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
        private void CreateNotification(List<PromoDemandChangeIncident> incidentsForNotify, string notificationName, string template, List<Promo> promoes, DatabaseContext context) {

			var notifyBody = String.Empty;
			var allRows = new List<string>();
			var logPromoNums = new List<string>();
			var emailArray = new string[] { };
			var toEmails = new List<string>();

			List<Recipient> recipients = NotificationsHelper.GetRecipientsByNotifyName(notificationName, context, false);

			Guid mailNotificationSettingsId = context.MailNotificationSettings
				.Where(y => y.Name == notificationName && !y.Disabled)
				.Select(x => x.Id).FirstOrDefault();
			
			string toEmail = context.MailNotificationSettings
				 .Where(y => y.Id == mailNotificationSettingsId)
				 .Select(x => x.To).FirstOrDefault();
			if (!String.IsNullOrEmpty(toEmail))
			{
				toEmails.Add(toEmail);
			}
			
			if (!recipients.Any() && toEmails.Count == 0)
			{
				Errors.Add(String.Format("There are no recipinets for notification: {0}", notificationName));
				return;
			}

			IList<string> userErrors;
			List<Guid> userIds = NotificationsHelper.GetUserIdsByRecipients(notificationName, recipients, context, out userErrors);

			if (userErrors.Any())
			{
				foreach (string error in userErrors)
				{
					Warnings.Add(error);
				}
			}
			else if (!userIds.Any() && String.IsNullOrWhiteSpace(toEmail))
			{
				foreach (PromoDemandChangeIncident incident in incidentsForNotify)
				{
					incident.ProcessDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
				}
				Warnings.Add(String.Format("There are no appropriate recipinets for notification: {0}.", notificationName));
				context.SaveChanges();
				return;
			}

			incidentsForNotify = incidentsForNotify.ToList();
			promoes = promoes.Where(x => incidentsForNotify.Any(i => i.PromoIntId == x.Number)).ToList();
			foreach (Guid userId in userIds)
			{
				string userEmail = context.Users.Where(x => x.Id == userId).Select(y => y.Email).FirstOrDefault();
				List<Constraint> constraints = NotificationsHelper.GetConstraitnsByUserId(userId, context);
				//IQueryable<PromoDemandChangeIncident> constraintIncidents = incidentsForNotify;
				var constraintIncidents = new List<PromoDemandChangeIncident>();

				// Применение ограничений
				if (constraints.Any())
				{
					IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
					List<ClientTreeHierarchyView> hierarchy = context.Set<ClientTreeHierarchyView>().AsNoTracking().ToList();
					IEnumerable<string> clientFilter = FilterHelper.GetFilter(filters, ModuleFilterName.Client);
					hierarchy = hierarchy.Where(h => clientFilter.Contains(h.Id.ToString()) || clientFilter.Any(c => h.Hierarchy.Contains(c))).ToList();

					constraintIncidents = incidentsForNotify.Where(x =>
						hierarchy.Any(h => promoes.Any(p => h.Id == p.ClientTreeId && x.PromoIntId == p.Number)) ||
						hierarchy.Any(h => h.Hierarchy.Contains(promoes
						.Where(p2 => p2.Number == x.PromoIntId).Select(p2 => p2.ClientTreeId).FirstOrDefault().Value.ToString()))).ToList();
				}

				if (constraintIncidents.Any())
				{
					logPromoNums = new List<string>();
					allRows = new List<string>();
					foreach (PromoDemandChangeIncident incident in constraintIncidents)
					{
						IDictionary<string, object> record = GetDictionary(incident);
						List<string> allRowCells = GetRow(incident, propertiesOrder);
						allRows.Add(String.Format(rowTemplate, string.Join("", allRowCells)));
						logPromoNums.Add(incident.PromoIntId.ToString());
					}
					notifyBody = String.Format(template, string.Join("", allRows));

					emailArray = new[] { userEmail };
					if (!String.IsNullOrEmpty(userEmail))
					{
						SendNotificationByEmails(notifyBody, notificationName, emailArray);
						Results.Add(String.Format("Notifications about changes in {0} promoes were sent to {1}", String.Join(", ", logPromoNums.Distinct()), String.Join(", ", emailArray.Distinct())), null);
					}
					else
					{
						string userLogin = context.Users.Where(x => x.Id == userId).Select(x => x.Name).FirstOrDefault();
						Warnings.Add(String.Format("Email not found for user: {0}", userLogin));
					}
				}
				else
				{
					// Если нет ограничений, то отправляем для всех промо.
					toEmails.Add(userEmail);
				}

				foreach (PromoDemandChangeIncident incident in incidentsForNotify)
				{
					incident.ProcessDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
				}
			}

			// Отправляем получателю из настроек без огранечений
			logPromoNums = new List<string>();
			allRows = new List<string>();
			foreach (PromoDemandChangeIncident incident in incidentsForNotify)
			{
				IDictionary<string, object> record = GetDictionary(incident);
				List<string> allRowCells = GetRow(incident, propertiesOrder);
				allRows.Add(String.Format(rowTemplate, string.Join("", allRowCells)));
				logPromoNums.Add(incident.PromoIntId.ToString());
			}
			notifyBody = String.Format(template, string.Join("", allRows));

			emailArray = toEmails.ToArray();
			if (emailArray.Length != 0)
			{
				SendNotificationByEmails(notifyBody, notificationName, emailArray);
				Results.Add(String.Format("Notifications about changes in {0} promoes were sent to {1}", String.Join(", ", logPromoNums.Distinct()), String.Join(", ", emailArray.Distinct())), null);
			}
			
			if (String.IsNullOrWhiteSpace(toEmail))
			{
				Warnings.Add(String.Format("Can't send notification to recipient from MailNotificationSetting ('to' field)"));
			}

			context.SaveChanges();
        }

        private readonly string[] propertiesOrder = new string[] { // TODO: вынести в настройки. БД - нужно изменение длины поля (ядро), файл, конфиг?
            "PromoIntId", "Name", "ClientHierarchy", "BrandTech", "PromoStatus", "OldMarsMechanic", "NewMarsMechanic", "OldMarsMechanicDiscount", "NewMarsMechanicDiscount", "OldDispatchesStart", "NewDispatchesStart", "OldPlanPromoUpliftPercent", "NewPlanPromoUpliftPercent", "OldPlanPromoIncrementalLSV", "NewPlanPromoIncrementalLSV", "OldPlanSteel", "NewPlanSteel" };
    }
}