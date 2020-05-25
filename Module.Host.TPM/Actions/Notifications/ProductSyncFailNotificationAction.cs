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

namespace Module.Host.TPM.Actions.Notifications
{
    /// <summary>
    /// Класс для формирования и рассылки уведомления об ошибках при синхронизации продуктов с таблицей Materials
    /// </summary>
    public class ProductSyncFailNotificationAction : BaseNotificationAction
    {
        Dictionary<string, string> errorsToNotify = new Dictionary<string, string>();

        public ProductSyncFailNotificationAction(Dictionary<string, string> errorsToNotify)
        {
            this.errorsToNotify = errorsToNotify;
        }

        public override void Execute()
        {
            try
            {
                using (DatabaseContext context = new DatabaseContext())
                {
                    ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                    string templateFileName = settingsManager.GetSetting<string>("PRODUCT_SYNC_FAIL_NOTIFICATION_TEMPLATE_FILE", "ProductSyncFailTemplate.txt");
                    if (File.Exists(templateFileName))
                    {
                        string template = File.ReadAllText(templateFileName);
                        if (!String.IsNullOrEmpty(template))
                        {
							if (errorsToNotify.Any())
							{
								CreateNotification(errorsToNotify, "PRODUCT_SYNC_FAIL_NOTIFICATION", template, context);
							}
							else
							{
								Warnings.Add(String.Format("There are no errors to send in notifications."));
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
                string msg = String.Format("An error occurred while sending a notification via ProductSyncFail Notification: {0}", e.ToString());
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
        /// <param name="errorsToNotify"></param>
        /// <param name="notificationName"></param>
        /// <param name="template"></param>
        /// <param name="context"></param>
        private void CreateNotification(Dictionary<string, string> errorsToNotify, string notificationName, string template, DatabaseContext context)
        {
            List<string> allRows = new List<string>();
			IList<string> productZREPs = new List<string>();
			foreach (KeyValuePair<string, string> ZREPError in errorsToNotify)
            {
                List<string> allRowCells = new List<string>();
                allRowCells.Add(String.Format(cellTemplate, ZREPError.Key));
                allRowCells.Add(String.Format(cellTemplate, ZREPError.Value));
                allRows.Add(String.Format(rowTemplate, string.Join("", allRowCells)));
                productZREPs.Add(ZREPError.Key);
            }
            string notifyBody = String.Format(template, string.Join("", allRows));
            SendNotification(notifyBody, notificationName);

			//Получаем получателей для лога
            List<Recipient> recipients = NotificationsHelper.GetRecipientsByNotifyName(notificationName, context);
            IList<string> userErrors;
            IList<string> guaranteedEmails;
            List<Guid> userIds = NotificationsHelper.GetUserIdsByRecipients(notificationName, recipients, context, out userErrors, out guaranteedEmails);

			if (userErrors.Any())
			{
				foreach (string error in userErrors)
				{
					Warnings.Add(error);
				}
			}
			else if (!userIds.Any() && !guaranteedEmails.Any())
			{
				Warnings.Add(String.Format("There are no appropriate recipinets for notification: {0}.", notificationName));
				context.SaveChanges();
				return;
			}

            var emails = context.Users.Where(x => userIds.Contains(x.Id) && !String.IsNullOrEmpty(x.Email)).Select(x => x.Email).ToList();
            emails.AddRange(guaranteedEmails);
			Results.Add(String.Format("Notifications about products sync fail (ZREPs: {0}) were sent to {1}.", String.Join(", ", productZREPs.Distinct()), String.Join(", ", emails.ToArray())), null);

			context.SaveChanges();
        }
    }
}
