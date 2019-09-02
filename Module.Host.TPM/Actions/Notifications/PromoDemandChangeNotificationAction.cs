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
                            IQueryable<PromoDemandChangeIncident> incidentsForNotify = context.Set<PromoDemandChangeIncident>().Where(x => x.ProcessDate == null).OrderBy(x => x.PromoIntId);
                            if (incidentsForNotify.Any()) {
                                CreateNotification(incidentsForNotify, "PROMO_DEMAND_CHANGE_NOTIFICATION", template, context);
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
        private void CreateNotification(IQueryable<PromoDemandChangeIncident> incidentsForNotify, string notificationName, string template, DatabaseContext context) {
			List<Recipient> recipients = ConstraintsHelper.GetRecipientsByNotifyName(notificationName, context);

			if (!recipients.Any())
			{
				Errors.Add(String.Format("There are no recipinets for notification: {0}", notificationName));
				return;
			}

			IList<string> userErrors;
			List<Guid> userIds = ConstraintsHelper.GetUserIdsByRecipients(recipients, context, out userErrors);

			if (userErrors.Any())
			{
				foreach (string error in userErrors)
				{
					Errors.Add(error);
				}
			}
			else if (!userIds.Any())
			{
				return;
			}

			foreach (Guid userId in userIds)
			{
				string[] userEmail = context.Users.Where(x => x.Id == userId).Select(y => y.Email).ToArray();
				List<Constraint> constraints = ConstraintsHelper.GetConstraitnsByUserId(userId, context);
				IQueryable<PromoDemandChangeIncident> constraintIncidents = incidentsForNotify;

				// Применение ограничений
				if (constraints.Any())
				{
					IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
					IQueryable<ClientTreeHierarchyView> hierarchy = context.Set<ClientTreeHierarchyView>().AsNoTracking();
					IEnumerable<string> clientFilter = FilterHelper.GetFilter(filters, ModuleFilterName.Client);

					IQueryable<Promo> promoes = context.Set<Promo>().Where(x => !x.Disabled && incidentsForNotify.Any(i => i.PromoIntId == x.Number));
					hierarchy = hierarchy.Where(h => clientFilter.Contains(h.Id.ToString()) || clientFilter.Any(c => h.Hierarchy.Contains(c)));
					constraintIncidents = incidentsForNotify.Where(x =>
						hierarchy.Any(h => promoes.Any(p => h.Id == p.ClientTreeId && x.PromoIntId == p.Number)) ||
						hierarchy.Any(h => (h.Hierarchy.Contains(promoes.Where(p2 => p2.Number == x.PromoIntId).Select(p2 => p2.ClientTreeId).FirstOrDefault().Value.ToString()))));
				}

				if (constraintIncidents.Any())
				{
					List<string> allRows = new List<string>();
					foreach (PromoDemandChangeIncident incident in incidentsForNotify)
					{
						IDictionary<string, object> record = GetDictionary(incident);
						List<string> allRowCells = GetRow(incident, propertiesOrder);
						allRows.Add(String.Format(rowTemplate, string.Join("", allRowCells)));
					}

					string notifyBody = String.Format(template, string.Join("", allRows));
					SendNotificationByEmails(notifyBody, notificationName, userEmail);
				}

				foreach (PromoDemandChangeIncident incident in incidentsForNotify)
				{
					incident.ProcessDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
				}
			}

			context.SaveChanges();
        }

        private readonly string[] propertiesOrder = new string[] { // TODO: вынести в настройки. БД - нужно изменение длины поля (ядро), файл, конфиг?
            "PromoIntId", "Name", "ClientHierarchy", "BrandTech", "OldPromoStatus", "NewPromoStatus", "OldMarsMechanic", "NewMarsMechanic", "OldPlanInstoreMechanic",
            "NewPlanInstoreMechanic", "OldMarsMechanicDiscount", "NewMarsMechanicDiscount", "OldPlanInstoreMechanicDiscount", "NewPlanInstoreMechanicDiscount",
            "OldStartDate", "NewStartDate", "OldEndDate", "NewEndDate", "OldDispatchesStart", "NewDispatchesStart", "OldDispatchesEnd", "NewDispatchesEnd",
            "OldPlanPromoUpliftPercent", "NewPlanPromoUpliftPercent", "OldPlanPromoIncrementalLSV", "NewPlanPromoIncrementalLSV", "OldPlanSteel", "NewPlanSteel", "OldOutletCount",
            "NewOutletCount", "OldXSite", "NEWXSite", "OldCatalogue", "NEWCatalogue", "IsProductListChange" };
    }
}