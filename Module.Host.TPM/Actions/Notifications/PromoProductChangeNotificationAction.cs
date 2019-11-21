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
							IQueryable<ProductChangeIncident> changeProductIncidents = context.Set<ProductChangeIncident>().Where(x => x.NotificationProcessDate == null).OrderBy(x => x.CreateDate);
							IQueryable<Product> createdProducts = changeProductIncidents.Where(p => p.IsCreate && !p.IsDeleteInMatrix && !p.IsCreateInMatrix).Select(i => i.Product);
							IQueryable<Product> deletedProducts = changeProductIncidents.Where(p => p.IsDelete && !p.IsDeleteInMatrix && !p.IsCreateInMatrix).Select(i => i.Product);
							IQueryable<Guid> addedFromMatrixProducts = changeProductIncidents.Where(p => p.IsCreateInMatrix).Select(i => i.ProductId);
							IQueryable<Product> deletedFromMatrixProducts = changeProductIncidents.Where(p => p.IsDeleteInMatrix && !p.IsCreateInMatrix).Select(i => i.Product);
							IQueryable<Guid> changedInMatrixProducts = changeProductIncidents.Where(p => p.IsDeleteInMatrix && p.IsCreateInMatrix).Select(i => i.ProductId);
							List<Product> changedProducts = changeProductIncidents.Where(p => !p.IsCreate && !p.IsDelete && !p.IsDeleteInMatrix && !p.IsCreateInMatrix).Select(i => i.Product).ToList();
							IList<Product> allProducts = context.Set<Product>().Where(x => !x.Disabled).ToList();

							DateTimeOffset today = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
							DateTimeOffset after12Weeks = today.AddDays(daysToCheckSetting);
							IQueryable<Promo> promoesToCheck = context.Set<Promo>().Where(x => !x.Disabled && x.StartDate.HasValue && x.StartDate.Value <= after12Weeks && statuses.Contains(x.PromoStatus.SystemName));
							IQueryable<ProductTree> allNodes = context.Set<ProductTree>();
							IQueryable<PromoProductTree> allProductTreeLinks = context.Set<PromoProductTree>().Where(p => !p.Disabled);

							IQueryable<PromoProduct> promoProducts = context.Set<PromoProduct>().Where(p => !p.Disabled);

							if ((createdProducts.Any() || deletedProducts.Any() || changedProducts.Any() || deletedFromMatrixProducts.Any() || changedInMatrixProducts.Any() || addedFromMatrixProducts.Any()) && promoesToCheck.Any())
							{
								// Список промо со списком подходящих новых продутов для каждого
								List<Tuple<Promo, IEnumerable<Product>>> promoToNewProductsList = new List<Tuple<Promo, IEnumerable<Product>>>();
								// Список промо со списком продуктов больше не подходящих под условия
								List<Tuple<Promo, IEnumerable<Product>>> promoToDeletedProductsList = new List<Tuple<Promo, IEnumerable<Product>>>();

								foreach (Promo promo in promoesToCheck)
								{
									// Узлы иерархии продуктов для данного промо
									IQueryable<PromoProductTree> promoProductTrees = allProductTreeLinks.Where(p => p.PromoId == promo.Id);

									// Фильтруем иерархию по текущей дате и ObjectId дерева продуктов промо
									IQueryable<ProductTree> nodes = allNodes.Where(n => !n.EndDate.HasValue && promoProductTrees.Any(p => p.ProductTreeObjectId == n.ObjectId));

									List<Func<Product, bool>> expressionsList = GetExpressionList(nodes);
									List<string> eanPCsFromMatix = PlanProductParametersCalculation.GetProductListFromAssortmentMatrix(promo, context);
									if (expressionsList.Any())
									{
										// Список новых продуктов, подходящих по параметрам фильтрации узлов продуктов, выбранных для данного промо
										List<Product> matchesProducts = createdProducts.ToList().Where(p => expressionsList.Any(e => e.Invoke(p))).ToList();
										if (!promo.InOut.HasValue || !promo.InOut.Value)
										{
											matchesProducts = PlanProductParametersCalculation.GetResultProducts(matchesProducts, eanPCsFromMatix, promo, context);
										}

										IQueryable<Guid> currentPromoProducts = promoProducts.Where(p => p.PromoId == promo.Id).Select(x => x.ProductId);
										if (currentPromoProducts.Any())
										{
											// Продукты, котрорые после изменения подходят под новые промо (с учётом ассортиментной матрицы)
											List<Product> changedNotPromoProducts = changedProducts.Where(p => !currentPromoProducts.Contains(p.Id)).ToList();
											if (promo.InOut.HasValue && promo.InOut.Value)
											{
												changedNotPromoProducts = changedNotPromoProducts.Where(x => changedInMatrixProducts.Contains(x.Id)).ToList();
											}
											changedNotPromoProducts = PlanProductParametersCalculation.GetResultProducts(changedNotPromoProducts, eanPCsFromMatix, promo, context);

											if (changedNotPromoProducts.Any())
											{
												List<Product> newProductsForPromo = changedNotPromoProducts.Where(p => expressionsList.Any(e => e.Invoke(p))).ToList();
												if (newProductsForPromo.Any())
												{
													matchesProducts.AddRange(newProductsForPromo);
												}
											}
										}
										// Продкуты, которые подходят из-за изменений в АМ
										IList<Guid> selectedProductsList = new List<Guid>();
										if (promo.InOutProductIds != null)
										{
											var productIdList = promo.InOutProductIds.Split(';');
											foreach (var productId in productIdList)
											{
												var productGuid = Guid.Empty;
												if (Guid.TryParse(productId, out productGuid))
												{
													if (productGuid != null && !productGuid.Equals(Guid.Empty)) selectedProductsList.Add(productGuid);
												}
											}

											List<Product> productsToAdd = new List<Product>();
											// Добавленные в АМ
											foreach (var createdInMatrix in addedFromMatrixProducts)
											{
												if (selectedProductsList.Contains(createdInMatrix) && !currentPromoProducts.Contains(createdInMatrix))
												{
													Product product = allProducts.Where(x => x.Id == createdInMatrix).FirstOrDefault();
													productsToAdd.Add(product);
												}
											}
											// Измененные в АМ
											if (changedInMatrixProducts.Any(x => selectedProductsList.Contains(x)))
											{
												foreach (var changedProduct in changedInMatrixProducts)
												{
													if (selectedProductsList.Contains(changedProduct) && !currentPromoProducts.Contains(changedProduct))
													{
														Product product = allProducts.Where(x => x.Id == changedProduct).FirstOrDefault();
														productsToAdd.Add(product);
													}
												}
											}
											productsToAdd = PlanProductParametersCalculation.GetResultProducts(productsToAdd.Distinct().ToList(), eanPCsFromMatix, promo, context);
											matchesProducts.AddRange(productsToAdd);
										}
										
										if (matchesProducts.Any())
										{
											promoToNewProductsList.Add(new Tuple<Promo, IEnumerable<Product>>(promo, matchesProducts));
										}

										// Удалённые продукты
										List<Product> deletedPromoProducts = deletedProducts.Where(p => currentPromoProducts.Contains(p.Id)).ToList();

										// Удалённые из ассортиментной матрицы продукты
										List<Product> deletedFromMatrixPromoProducts = deletedFromMatrixProducts.Where(p => currentPromoProducts.Contains(p.Id)).ToList();

										// Продукты, которые после изменения больше не подходят под текущие промо
										List<Product> changedPromoProducts = changedProducts.Where(p => currentPromoProducts.Contains(p.Id)).ToList();
										if (changedPromoProducts.Any())
										{
											List<Product> sameProducts = changedPromoProducts.Where(p => expressionsList.Any(e => e.Invoke(p))).ToList();
											List<Product> changed = changedProducts.Where(p => !sameProducts.Contains(p)).ToList();

											if (changed.Any())
											{
												deletedPromoProducts.AddRange(changed);
											}
										}

										// Продукты, которые после изменения больше не подходят под текущие промо из-за изменения в AM
										List<Product> productsToDel = new List<Product>();
										foreach (var changedProduct in changedInMatrixProducts)
										{
											if (currentPromoProducts.Contains(changedProduct))
											{
												Product product = allProducts.Where(x => x.Id == changedProduct).FirstOrDefault();
												productsToDel.Add(product);
											}
										}
										deletedPromoProducts.AddRange(PlanProductParametersCalculation.GetResultProducts(productsToDel, eanPCsFromMatix, promo, context));

										if (deletedPromoProducts.Any())
										{
											promoToDeletedProductsList.Add(new Tuple<Promo, IEnumerable<Product>>(promo, deletedPromoProducts));
										}
										if (deletedFromMatrixPromoProducts.Any())
										{
											promoToDeletedProductsList.Add(new Tuple<Promo, IEnumerable<Product>>(promo, deletedFromMatrixPromoProducts));
										}
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
			IList<string> userErrors;
			List<Recipient> recipients = NotificationsHelper.GetRecipientsByNotifyName(notificationName, context);
			List<Guid> userIds = NotificationsHelper.GetUserIdsByRecipients(notificationName, recipients, context, out userErrors);

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
				context.SaveChanges();
				return;
			}

			string[] userEmails = context.Users.Where(x => userIds.Contains(x.Id) && !String.IsNullOrEmpty(x.Email)).Select(x => x.Email).ToArray();
			Results.Add(String.Format("Notification {0} for promoes {1} were sent to {2}.", notificationName, String.Join(", ", promoNumbers.Distinct().ToArray()), String.Join(", ", userEmails)), null);
		}

		private readonly string[] propertiesOrder = new string[] {
			"Number", "Name", "ClientHierarchy", "StartDate", "EndDate" };
	}
}