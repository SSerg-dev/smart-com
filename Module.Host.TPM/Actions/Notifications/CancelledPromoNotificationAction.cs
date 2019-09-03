﻿using System;
using Persist;
using Core.Settings;
using Core.Dependency;
using System.IO;
using Module.Persist.TPM.Model.TPM;
using System.Linq;
using System.Collections.Generic;
using Module.Persist.TPM.Utils;

namespace Module.Host.TPM.Actions.Notifications
{
    /// <summary>
    /// Класс для формирования и рассылки уведомления по Cancelled promo
    /// </summary>
    public class CancelledPromoNotificationAction : BaseNotificationAction
    {
        public override void Execute()
        {
            try
            {
                using (DatabaseContext context = new DatabaseContext())
                {
                    ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                    string templateFileName = settingsManager.GetSetting<string>("PROMO_CANCELLED_NOTIFICATION_TEMPLATE_FILE", "PromoCancelledTemplate.txt");
                    if (File.Exists(templateFileName))
                    {
                        string template = File.ReadAllText(templateFileName);
                        if (!String.IsNullOrEmpty(template))
                        {
                            var incidentsForNotify = context.Set<PromoCancelledIncident>().Where(x => x.ProcessDate == null).GroupBy(x => x.PromoId);
                            if (incidentsForNotify.Any())
                            {
                                CreateNotification(incidentsForNotify, "CANCELLED_PROMO_NOTIFICATION", template, context);
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
                string msg = String.Format("An error occurred while sending a notification via Cancelled Promo Incident: {0}", e.ToString());
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
        private void CreateNotification(IQueryable<IGrouping<Guid, PromoCancelledIncident>> incidentsForNotify, string notificationName, string template, DatabaseContext context)
        {
            List<string> allRows = new List<string>();
            foreach (IGrouping<Guid, PromoCancelledIncident> incidentGroup in incidentsForNotify)
            {
				foreach (PromoCancelledIncident incident in incidentGroup)
				{
					List<string> allRowCells = GetRow(incident.Promo, propertiesOrder);
					allRowCells.Add(String.Format(cellTemplate, incident.CreateDate.ToString("dd.MM.yyyy HH:mm:ss")));
					allRows.Add(String.Format(rowTemplate, string.Join("", allRowCells)));
					incident.ProcessDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
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