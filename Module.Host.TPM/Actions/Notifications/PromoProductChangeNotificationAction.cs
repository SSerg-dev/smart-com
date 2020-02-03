using System;
using Persist;
using Core.Settings;
using Core.Dependency;
using Module.Persist.TPM.Model.TPM;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Data.Entity;
using Module.Persist.TPM.Utils;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Persist.Model.Settings;

namespace Module.Host.TPM.Actions.Notifications
{
	/// <summary>
	/// Класс для формирования и рассылки уведомления по новым продуктам подходящим для текущих промо
	/// </summary>
	public class PromoProductChangeNotificationAction : BaseNotificationAction
	{
		public override void Execute()
		{
			try
			{
				using (DatabaseContext context = new DatabaseContext())
				{
					ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
					string statusesSetting = settingsManager.GetSetting<string>("CHECK_PRODUCT_CHANGE_PROMO_STATUS_LIST", "OnApproval,Approved,Planned");
					string templateFileName = settingsManager.GetSetting<string>("PROMO_PRODUCT_CHANGE_NOTIFICATION_TEMPLATE_FILE", "PromoProductChangeTemplate.txt");
					int daysToCheckSetting = settingsManager.GetSetting<int>("PRODUCT_CHANGE_PERIOD_DAYS", 84);
					if (File.Exists(templateFileName))
					{
						string template = File.ReadAllText(templateFileName);
						if (!String.IsNullOrEmpty(template))
						{
							string[] statuses = statusesSetting.Split(',');
							var changeProductIncidents = context.Set<ProductChangeIncident>().Where(x => x.NotificationProcessDate == null).OrderBy(x => x.CreateDate).AsEnumerable();
							var changedProducts = changeProductIncidents.Select(x => x.Product).Distinct().ToList();
							var createdProducts = changeProductIncidents.Where(p => p.IsCreate && !p.IsChecked).Select(i => i.Product).AsEnumerable();
							var allProducts = context.Set<Product>();

							DateTimeOffset today = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
							DateTimeOffset after12Weeks = today.AddDays(daysToCheckSetting);
							var promoesToCheck = context.Set<Promo>().Where(x => !x.Disabled && x.StartDate.HasValue && x.StartDate.Value <= after12Weeks && statuses.Contains(x.PromoStatus.SystemName));
							var allPromoProducts = context.Set<PromoProduct>();

							if (promoesToCheck.Any() && changeProductIncidents.Any())
							{
								List<Tuple<Promo, IEnumerable<Product>>> promoToNewProductsList = new List<Tuple<Promo, IEnumerable<Product>>>();
								List<Tuple<Promo, IEnumerable<Product>>> promoToDeletedProductsList = new List<Tuple<Promo, IEnumerable<Product>>>();

								foreach (Promo promo in promoesToCheck)
								{
									var promoId = promo.Id;
									var recalculatedIncidents = changeProductIncidents
										.Where(x => x.RecalculatedPromoId == promoId && x.IsRecalculated && !x.IsChecked);

									var addedProducts = new List<Product>();
									var excludedProducts = new List<Product>();
									if (recalculatedIncidents.Any())
									{
										foreach (var incident in recalculatedIncidents)
										{
											if (!String.IsNullOrWhiteSpace(incident.AddedProductIds))
											{
												IEnumerable<string> tempProductIds = incident.AddedProductIds.Split(';').Distinct();
												List<Product> tempProducts = allProducts.Where(p => tempProductIds.Contains(p.ZREP)).ToList();
												addedProducts.AddRange(tempProducts);

												var intersectProducts = excludedProducts.Intersect(tempProducts);
												excludedProducts.Except(intersectProducts);
												addedProducts.Except(intersectProducts);
											}
											if (!String.IsNullOrWhiteSpace(incident.ExcludedProductIds))
											{
												IEnumerable<string> tempProductIds = incident.ExcludedProductIds.Split(';');
												List<Product> tempProducts = allProducts.Where(p => tempProductIds.Contains(p.ZREP)).ToList();
												excludedProducts.AddRange(tempProducts);

												var intersectProducts = addedProducts.Intersect(tempProducts);
												excludedProducts.Except(intersectProducts);
												addedProducts.Except(intersectProducts);
											}
										}
									}

									var checkedIncidents = changeProductIncidents
										.Where(x => x.RecalculatedPromoId == promoId && !x.IsRecalculated && x.IsChecked);
									if (checkedIncidents.Any())
									{
										foreach (var incident in checkedIncidents)
										{
											if (!String.IsNullOrWhiteSpace(incident.AddedProductIds))
											{
												IEnumerable<string> tempProductIds = incident.AddedProductIds.Split(';').Distinct();
												List<Product> tempProducts = allProducts.Where(p => tempProductIds.Contains(p.ZREP)).ToList();
												addedProducts.AddRange(tempProducts);
											}
											if (!String.IsNullOrWhiteSpace(incident.ExcludedProductIds))
											{
												IEnumerable<string> tempProductIds = incident.ExcludedProductIds.Split(';');
												List<Product> tempProducts = allProducts.Where(p => tempProductIds.Contains(p.ZREP)).ToList();
												excludedProducts.AddRange(tempProducts);
											}
										}
									}

									string error = null;
									List<Product> filteredProducts = PlanProductParametersCalculation.GetProductFiltered(promoId, context, out error);
									List<string> eanPCs = PlanProductParametersCalculation.GetProductListFromAssortmentMatrix(promo, context);
									List<Product> resultProductList = null;

									if (promo.InOut.HasValue && promo.InOut.Value)
									{
										resultProductList = PlanProductParametersCalculation.GetCheckedProducts(context, promo);
									}
									else
									{
										resultProductList = PlanProductParametersCalculation.GetResultProducts(filteredProducts, eanPCs, promo, context);
									}

									var promoProducts = allPromoProducts.Where(x => x.PromoId == promoId);
									var promoProductsNotDisabled = promoProducts.Where(x => !x.Disabled);

									foreach (PromoProduct promoProduct in promoProductsNotDisabled)
									{
										if (!resultProductList.Any(x => x.ZREP == promoProduct.ZREP) && changedProducts.Any(cp => cp.ZREP == promoProduct.ZREP))
										{
											excludedProducts.Add(promoProduct.Product);

											if (addedProducts.Contains(promoProduct.Product))
											{
												excludedProducts.Remove(promoProduct.Product);
												addedProducts.Remove(promoProduct.Product);
											}

										}
									}

									foreach (Product product in resultProductList)
									{
										var promoProduct = promoProducts.FirstOrDefault(x => x.ZREP == product.ZREP);
										if ((promoProduct == null || promoProduct.Disabled) && changedProducts.Any(cp => cp.ZREP == product.ZREP))
										{
											addedProducts.Add(product);

											if (excludedProducts.Contains(product))
											{
												excludedProducts.Remove(product);
												addedProducts.Remove(product);
											}
										}
										if (createdProducts.Any(p => p.ZREP == product.ZREP) && resultProductList.Any(p => p.ZREP == product.ZREP) && !addedProducts.Any(p => p.ZREP == product.ZREP))
										{
											addedProducts.Add(product);

											if (excludedProducts.Contains(product))
											{
												excludedProducts.Remove(product);
												addedProducts.Remove(product);
											}
										}
									}

									if (addedProducts.Any())
									{
										promoToNewProductsList.Add(new Tuple<Promo, IEnumerable<Product>>(promo, addedProducts.AsEnumerable()));
									}
									if (excludedProducts.Any())
									{
										promoToDeletedProductsList.Add(new Tuple<Promo, IEnumerable<Product>>(promo, excludedProducts.AsEnumerable()));
									}
								}
								if (promoToNewProductsList.Count > 0)
								{
									CreateNotification(promoToNewProductsList, "PROMO_PRODUCT_CREATE_NOTIFICATION", template, context);
								}
								else
								{
									Warnings.Add(String.Format("There are no new products relevant for the promotions"));
								}
								if (promoToDeletedProductsList.Count > 0)
								{
									CreateNotification(promoToDeletedProductsList, "PROMO_PRODUCT_DELETE_NOTIFICATION", template, context);
								}
								else
								{
									Warnings.Add(String.Format("There are no products used in the promotions that no longer match the conditions"));
								}
								foreach (ProductChangeIncident incident in changeProductIncidents)
								{
									incident.NotificationProcessDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
								}
								context.SaveChanges();
							}
							else
							{
								Warnings.Add(String.Format("There are no incidents to send notifications."));
								context.SaveChanges();
							}
						}
						else
						{
							Errors.Add(String.Format("Empty notification template: {0}", templateFileName));
						}
					}
					else
					{
						Errors.Add(String.Format("Could not find notification template: {0}", templateFileName));
					}
				}
			}
			catch (Exception e)
			{
				string msg = String.Format("An error occurred while sending a notification for Product Create Incident: {0}", e.ToString());
				logger.Error(msg);
				Errors.Add(msg);
			}
			finally
			{
				logger.Trace("Finish");
			}
		}
		/// <summary>
		/// Формирование и отправка уведомления
		/// </summary>
		/// <param name="records"></param>
		/// <param name="notificationName"></param>
		/// <param name="template"></param>
		/// <param name="notifier"></param>
		private void CreateNotification(List<Tuple<Promo, IEnumerable<Product>>> records, string notificationName, string template, DatabaseContext context)
		{
			List<string> allRows = new List<string>();
			IList<string> promoNumbers = new List<string>();
			foreach (Tuple<Promo, IEnumerable<Product>> promoToProduct in records)
			{
				List<string> allRowCells = GetRow(promoToProduct.Item1, propertiesOrder);
				allRowCells.Add(String.Format(cellTemplate, String.Join(";", promoToProduct.Item2.Distinct().Select(p => p.ZREP))));
				allRows.Add(String.Format(rowTemplate, string.Join("", allRowCells)));
				promoNumbers.Add(promoToProduct.Item1.Number.ToString());

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
			Results.Add(String.Format("Notification {0} for promoes {1} were sent to {2}.", notificationName, String.Join(", ", promoNumbers.Distinct()), String.Join(", ", emails.Distinct())), null);
		}

		private readonly string[] propertiesOrder = new string[] {
			"Number", "Name", "ClientHierarchy", "StartDate", "EndDate" };
	}
}