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
using Persist.Model;
using Module.Persist.TPM.Model.DTO;
using Utility;

namespace Module.Host.TPM.Actions.Notifications
{
    /// <summary>
    /// Класс для формирования и рассылки уведомления по действию Reject промо
    /// </summary>
    public class PromoOnRejectNotificationAction : BaseNotificationAction
    {
        public override void Execute()
        {
            try
            {
                using (DatabaseContext context = new DatabaseContext())
                {
                    ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                    string templateFileName = settingsManager.GetSetting<string>("PROMO_ON_REJECT_NOTIFICATION_TEMPLATE_FILE", "PromoOnRejectTemplate.txt");
                    if (File.Exists(templateFileName))
                    {
                        string template = File.ReadAllText(templateFileName);
                        if (!String.IsNullOrEmpty(template))
                        {
							var notifyIncidents = context.Set<PromoOnRejectIncident>().Where(x => x.ProcessDate == null).GroupBy(y => y.Promo.CreatorId);

							if (notifyIncidents.Any())
							{
								CreateNotification(notifyIncidents, "PROMO_ON_REJECT_NOTIFICATION", template, context);
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
                string msg = String.Format("An error occurred while sending a notification via Promo On Reject Incident: {0}", e.ToString());
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
		private void CreateNotification(IQueryable<IGrouping<Guid?, PromoOnRejectIncident>> incidentsForNotify, string notificationName, string template, DatabaseContext context)
		{
			string[] requiredRoles = { "KeyAccountManager" };
			foreach (IGrouping<Guid?, PromoOnRejectIncident> incidentsGroup in incidentsForNotify)
			{
				IList<string> promoNumbers = new List<string>();
				Guid? creatorId = incidentsGroup.Key != null ? incidentsGroup.Key : Guid.Empty;
				if (creatorId.Equals(Guid.Empty))
				{
					foreach (PromoOnRejectIncident incident in incidentsGroup)
					{
						incident.ProcessDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
						promoNumbers.Add(incident.Promo.Number.ToString());
					}
					Warnings.Add(String.Format("Promo creator not specified or not found. Promo numbers: {0}", String.Join(", ", promoNumbers.Distinct().ToArray())));
					continue;
				}

				string creatorEmail = context.Users.Where(x => x.Id == creatorId && !x.Disabled).Select(y => y.Email).FirstOrDefault();
				if (String.IsNullOrEmpty(creatorEmail))
				{
					promoNumbers = new List<string>();
					foreach (PromoOnRejectIncident incident in incidentsGroup)
					{
						incident.ProcessDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
						promoNumbers.Add(incident.Promo.Number.ToString());
					}
					Warnings.Add(String.Format("Promo creator's email not found. Promo numbers: {0}", String.Join(", ", promoNumbers.Distinct().ToArray())));
					continue;
				}

				List<string> allRows = new List<string>();
				promoNumbers = new List<string>();
				foreach (PromoOnRejectIncident incident in incidentsGroup)
				{
					List<string> allRowCells = GetRow(incident.Promo, propertiesOrder);
					allRowCells.Add(String.Format(cellTemplate, incident.UserLogin));
					allRows.Add(String.Format(rowTemplate, string.Join("", allRowCells)));
					incident.ProcessDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
					promoNumbers.Add(incident.Promo.Number.ToString());
				}

				string notifyBody = String.Format(template, string.Join("", allRows));
				string defaultRecipient = context.MailNotificationSettings.Where(x => x.Name == notificationName && !x.Disabled && !x.IsDisabled).Select(x => x.To).FirstOrDefault();

				string[] emailArray = new[] { creatorEmail };
				if (!String.IsNullOrEmpty(defaultRecipient))
				{
					emailArray = new[] { creatorEmail, defaultRecipient }; 
				}

				if (!String.IsNullOrEmpty(creatorEmail))
				{
					SendNotificationByEmails(notifyBody, notificationName, emailArray);
					Results.Add(String.Format("Notification about reject of promoes with numbers: {0} were sent to {1}", String.Join(", ", promoNumbers.Distinct().ToArray()), String.Join(", ", emailArray)), null);
				}
			}

			context.SaveChanges();
		}

		private readonly string[] propertiesOrder = new string[] {
			"ClientHierarchy", "Number", "Name", "PromoStatus.Name", "StartDate", "EndDate", "DispatchesStart", "DispatchesEnd"  };
    }
}
