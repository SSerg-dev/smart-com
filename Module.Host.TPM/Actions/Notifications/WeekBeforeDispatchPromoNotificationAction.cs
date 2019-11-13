using System;
using Persist;
using Core.Settings;
using Core.Dependency;
using System.IO;
using Module.Persist.TPM.Model.TPM;
using System.Linq;
using System.Collections.Generic;
using Module.Persist.TPM.Model.DTO;
using Utility;
using Module.Persist.TPM.Utils;
using Persist.Model;
using Persist.Model.Settings;

namespace Module.Host.TPM.Actions.Notifications
{
    /// <summary>
    /// Класс для формирования и рассылки уведомления по промо, которым осталась неделя до Dispatch start
    /// </summary>
    public class WeekBeforeDispatchPromoNotificationAction : BaseNotificationAction
    {
        public override void Execute()
        {
            try
            {
                using (DatabaseContext context = new DatabaseContext())
                {
					string[] statusesToCheck = { "DraftPublished", "OnApproval", "Approved" };
					List<Promo> promoes = context.Set<Promo>()
						.Where(x => statusesToCheck.Contains(x.PromoStatus.SystemName) 
						&& !x.Disabled).ToList();

					List<Promo> promoesForNotify = new List<Promo>();
					foreach (Promo promo in promoes)
					{
						DateTime dt = DateTime.Now.Date;
						DateTimeOffset dto = new DateTimeOffset(dt.Year, dt.Month, dt.Day, 0, 0, 0, TimeSpan.Zero);
						DateTimeOffset promoDTO = new DateTimeOffset(promo.DispatchesStart.Value.Date, TimeSpan.Zero);

						if (promoDTO.Subtract(dto).TotalDays == 7)
						{
							promoesForNotify.Add(promo);
						}
					}

					if (promoesForNotify.Count().Equals(0))
					{
						Warnings.Add("There are no promo with week before dispatch start");
						return;
					}

					ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                    string templateFileName = settingsManager.GetSetting<string>("WEEK_BEFORE_DISPATCH_PROMO_NOTIFICATION_TEMPLATE", "WeekBeforeDispatchPromoTemplate.txt");
                    if (File.Exists(templateFileName))
                    {
                        string template = File.ReadAllText(templateFileName);
                        if (!String.IsNullOrEmpty(template))
                        {
							CreateNotification(promoesForNotify.AsQueryable(), "WEEK_BEFORE_DISPATCH_PROMO_NOTIFICATION", template, context);
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
                string msg = String.Format("An error occurred while sending a notification via Week Before Dispatch Start Promoes: {0}", e.ToString());
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
		/// <param name="promoesForNotify"></param>
		/// <param name="notificationName"></param>
		/// <param name="template"></param>
		/// <param name="context"></param>
		private void CreateNotification(IQueryable<Promo> promoesForNotify, string notificationName, string template, DatabaseContext context)
        {
			string[] recipientsRole = { "KeyAccountManager" };
			List<Recipient> recipients = ConstraintsHelper.GetRecipientsByNotifyName(notificationName, context);

			if (!recipients.Any())
			{
				Errors.Add(String.Format("There are no recipinets for notification: {0}", notificationName));
				return;
			}

			IList<string> userErrors;
			List<Guid> userIds = ConstraintsHelper.GetUserIdsByRecipients(notificationName, recipients, context, out userErrors, recipientsRole);

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

			foreach (Guid userId in userIds)
			{
				string userEmail = context.Users.Where(x => x.Id == userId).Select(y => y.Email).FirstOrDefault();
				List<Constraint> constraints = ConstraintsHelper.GetConstraitnsByUserId(userId, context);
				IQueryable<Promo> constraintPromoes = promoesForNotify;

				// Применение ограничений
				if (constraints.Any())
				{
					IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
					IQueryable<ClientTreeHierarchyView> hierarchy = context.Set<ClientTreeHierarchyView>().AsNoTracking();
					IEnumerable<string> clientFilter = FilterHelper.GetFilter(filters, ModuleFilterName.Client);

					hierarchy = hierarchy.Where(h => clientFilter.Contains(h.Id.ToString()) || clientFilter.Any(c => h.Hierarchy.Contains(c)));
					constraintPromoes = promoesForNotify.Where(x =>
						hierarchy.Any(h => h.Id == x.ClientTreeId || h.Hierarchy.Contains(x.ClientTreeId.Value.ToString())));
				}

				if (constraintPromoes.Any())
				{
					IList<string> promoNumbers = new List<string>();
					List<string> allRows = new List<string>();
					foreach (Promo promo in constraintPromoes)
					{
						List<string> allRowCells = GetRow(promo, propertiesOrder);
						allRows.Add(String.Format(rowTemplate, string.Join("", allRowCells)));
						promoNumbers.Add(promo.Number.ToString());
					}
					string notifyBody = String.Format(template, string.Join("", allRows));

					string[] emailArray = new[] { userEmail };
					if (!String.IsNullOrEmpty(userEmail))
					{
						SendNotificationByEmails(notifyBody, notificationName, emailArray);
						Results.Add(String.Format("Notification of promos (numbers: {0}) with less than a week left before dispatch were sent to {1}", String.Join(", ", promoNumbers.Distinct().ToArray()), String.Join(", ", emailArray)), null);
					}
					else
					{
						string userLogin = context.Users.Where(x => x.Id == userId).Select(x => x.Name).FirstOrDefault();
						Warnings.Add(String.Format("Email not found for user: {0}", userLogin));
					}
				}
			}
            
            context.SaveChanges();
        }

        private readonly string[] propertiesOrder = new string[] {
			"ClientHierarchy", "Number", "Name", "PromoStatus.Name", "StartDate", "EndDate", "DispatchesStart", "DispatchesEnd" };
    }
}
