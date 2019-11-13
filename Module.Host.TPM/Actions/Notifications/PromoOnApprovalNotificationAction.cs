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

namespace Module.Host.TPM.Actions.Notifications
{
    /// <summary>
    /// Класс для формирования и рассылки уведомления по On Approval промо
    /// </summary>
    public class PromoOnApprovalNotificationAction : BaseNotificationAction
    {
        public override void Execute()
        {
            try
            {
                using (DatabaseContext context = new DatabaseContext())
                {
                    ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                    string templateFileName = settingsManager.GetSetting<string>("PROMO_ON_APPROVAL_NOTIFICATION_TEMPLATE_FILE", "PromoOnApprovalTemplate.txt");
                    if (File.Exists(templateFileName))
                    {
                        string template = File.ReadAllText(templateFileName);
                        if (!String.IsNullOrEmpty(template))
                        {
 							var notifyIncidents = context.Set<PromoOnApprovalIncident>().Where(x => x.ProcessDate == null);
							// Проверяем, что промо в статусе OnApproval 
							notifyIncidents = notifyIncidents.Where(x => x.Promo.PromoStatus.SystemName == "OnApproval");

							// Проверяем, что промо ещё не подтвреждено указанной ролью
							List<PromoOnApprovalIncident> actualNotifyIncidents = new List<PromoOnApprovalIncident>();
							foreach (PromoOnApprovalIncident incident in notifyIncidents)
							{
								switch (incident.ApprovingRole)
								{
									case "CMManager":
										if (!incident.Promo.IsCMManagerApproved.HasValue || incident.Promo.IsCMManagerApproved == false)
										{
											actualNotifyIncidents.Add(incident);
										}
										break;
									case "DemandPlanning":
										if (!incident.Promo.IsDemandPlanningApproved.HasValue || incident.Promo.IsDemandPlanningApproved == false)
										{
											actualNotifyIncidents.Add(incident);
										}
										break;
									case "DemandFinance":
										if (!incident.Promo.IsDemandFinanceApproved.HasValue || incident.Promo.IsDemandFinanceApproved == false)
										{
											actualNotifyIncidents.Add(incident);
										}
										break;
								}
							}

							var notifyIncidentsForCMM = actualNotifyIncidents.Where(x => x.ApprovingRole == "CMManager");
							var notifyIncidentsForDP = actualNotifyIncidents.Where(x => x.ApprovingRole == "DemandPlanning");
							var notifyIncidentsForDF = actualNotifyIncidents.Where(x => x.ApprovingRole == "DemandFinance");

							if (notifyIncidentsForCMM.Any())
                            {
                                CreateNotification(notifyIncidentsForCMM, "PROMO_ON_APPROVAL_NOTIFICATION", template, context, "CMManager");
							}
							if (notifyIncidentsForDP.Any())
							{
								CreateNotification(notifyIncidentsForDP, "PROMO_ON_APPROVAL_NOTIFICATION", template, context, "DemandPlanning");
							}
							if (notifyIncidentsForDF.Any())
							{
								CreateNotification(notifyIncidentsForDF, "PROMO_ON_APPROVAL_NOTIFICATION", template, context, "DemandFinance");
							}
							else if (!actualNotifyIncidents.Where(x => x.ApprovingRole == "CMManager" || x.ApprovingRole == "DemandPlanning" || x.ApprovingRole == "DemandFinance").Any())
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
                string msg = String.Format("An error occurred while sending a notification via Promo On Approval Incident: {0}", e.ToString());
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
        private void CreateNotification(IEnumerable<PromoOnApprovalIncident> incidentsForNotify, string notificationName, string template, DatabaseContext context, string approvingRole)
        {
			string[] recipientsRole = { approvingRole };
			List<Recipient> recipients = ConstraintsHelper.GetRecipientsByNotifyName(notificationName, context);

			if (!recipients.Any())
			{
				if (Errors.Count == 0)
				{
					Errors.Add(String.Format("There are no recipinets for notification: {0}.", notificationName));
				}
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
			if (!userIds.Any())
			{
				foreach (PromoOnApprovalIncident incident in incidentsForNotify)
				{
					incident.ProcessDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
				}
				Warnings.Add(String.Format("There are no appropriate recipinets for notification: {0} for role {1}.", notificationName, approvingRole));
				context.SaveChanges();
				return;
			}

			foreach (Guid userId in userIds)
			{
				string userEmail = context.Users.Where(x => x.Id == userId).Select(y => y.Email).FirstOrDefault();
				List<Constraint> constraints = ConstraintsHelper.GetConstraitnsByUserId(userId, context);
				IEnumerable<PromoOnApprovalIncident> constraintNotifies = incidentsForNotify;

				// Применение ограничений
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
					IList<string> promoNumbers = new List<string>();
					List<string> allRows = new List<string>();
					foreach (PromoOnApprovalIncident incident in constraintNotifies)
					{
						List<string> allRowCells = GetRow(incident.Promo, propertiesOrder);
						allRows.Add(String.Format(rowTemplate, string.Join("", allRowCells)));
						promoNumbers.Add(incident.Promo.Number.ToString());
					}
					string notifyBody = String.Format(template, string.Join("", allRows));

					string[] emailArray = new[] { userEmail };
					if (!String.IsNullOrEmpty(userEmail))
					{
						SendNotificationByEmails(notifyBody, notificationName, emailArray);
						Results.Add(String.Format("Notification about necessity of approving promoes with numbers {0} by {1} role were sent to {2}", String.Join(", ", promoNumbers.Distinct().ToArray()), approvingRole, String.Join(", ", emailArray)), null);
					}
					else
					{
						string userLogin = context.Users.Where(x => x.Id == userId).Select(x => x.Name).FirstOrDefault();
						Warnings.Add(String.Format("Email not found for user: {0}", userLogin));
					}
				}
			}

			foreach (PromoOnApprovalIncident incident in incidentsForNotify)
			{
				incident.ProcessDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
			}
			context.SaveChanges();
        }

        private readonly string[] propertiesOrder = new string[] {
			"ClientHierarchy", "Number", "Name", "PromoStatus.Name", "StartDate", "EndDate", "DispatchesStart", "DispatchesEnd"  };
    }
}
