using System;
using Persist;
using NLog;
using Core.Settings;
using Core.Dependency;
using Module.Persist.TPM.Model.TPM;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Data.Entity;

namespace Module.Host.TPM.Actions.Notifications {
    /// <summary>
    /// Класс для формирования и рассылки уведомления по новым продуктам подходящим для текущих промо
    /// </summary>
    public class PromoProductChangeNotificationAction : BaseNotificationAction {
        public override void Execute() {
            try {
                using (DatabaseContext context = new DatabaseContext()) {
                    ISettingsManager settingsManager = (ISettingsManager) IoC.Kernel.GetService(typeof(ISettingsManager));
                    string statusesSetting = settingsManager.GetSetting<string>("CHECK_PRODUCT_CHANGE_PROMO_STATUS_LIST", "OnApproval,Approved,Planned,Started");
                    string templateFileName = settingsManager.GetSetting<string>("PROMO_PRODUCT_CHANGE_NOTIFICATION_TEMPLATE_FILE", "PromoProductChangeTemplate.txt");
                    int daysToCheckSetting = settingsManager.GetSetting<int>("PRODUCT_CHANGE_PERIOD_DAYS", 84);
                    if (File.Exists(templateFileName)) {
                        string template = File.ReadAllText(templateFileName);
                        if (!String.IsNullOrEmpty(template)) {
                            string[] statuses = statusesSetting.Split(',');
                            IQueryable<ProductChangeIncident> changeProductIncidents = context.Set<ProductChangeIncident>().Where(x => x.ProcessDate == null).OrderBy(x => x.CreateDate);
                            IQueryable<Product> createdProducts = changeProductIncidents.Where(p => p.IsCreate).Select(i => i.Product);
                            IQueryable<Product> deletedProducts = changeProductIncidents.Where(p => p.IsDelete).Select(i => i.Product);
                            List<Product> changedProducts = changeProductIncidents.Where(p => !p.IsCreate && !p.IsDelete).Select(i => i.Product).ToList();

                            DateTimeOffset today = DateTimeOffset.Now;
                            IQueryable<Promo> promoesToCheck = context.Set<Promo>().Where(x => !x.Disabled && x.StartDate.HasValue && (DbFunctions.DiffDays(x.StartDate, today).Value <= daysToCheckSetting) && statuses.Contains(x.PromoStatus.SystemName));
                            IQueryable<ProductTree> allNodes = context.Set<ProductTree>();
                            IQueryable<PromoProductTree> allProductTreeLinks = context.Set<PromoProductTree>().Where(p => !p.Disabled);

                            IQueryable<PromoProduct> promoProducts = context.Set<PromoProduct>().Where(p => !p.Disabled);

                            if ((createdProducts.Any() || deletedProducts.Any() || changedProducts.Any()) && promoesToCheck.Any()) {
                                // Список промо со списком подходящих новых продутов для каждого
                                List<Tuple<Promo, IEnumerable<Product>>> promoToNewProductsList = new List<Tuple<Promo, IEnumerable<Product>>>();
                                // Список промо со списком продуктов больше не подходящих под условия
                                List<Tuple<Promo, IEnumerable<Product>>> promoToDeletedProductsList = new List<Tuple<Promo, IEnumerable<Product>>>();

                                foreach (Promo promo in promoesToCheck) {
                                    DateTime approveDate = promo.LastApprovedDate.HasValue ? promo.LastApprovedDate.Value.DateTime : DateTime.Now; // TODO: offset...
                                                                                                                                                   // Фильтруем иерархию по дате подтверждения промо ??
                                    IQueryable<ProductTree> nodes = allNodes.Where(n => DateTime.Compare(n.StartDate, approveDate) <= 0 && (!n.EndDate.HasValue || DateTime.Compare(n.EndDate.Value, approveDate) > 0));
                                    // Узлы иерархии продуктов для данного промо
                                    IQueryable<PromoProductTree> promoProductTrees = allProductTreeLinks.Where(p => p.PromoId == promo.Id);
                                    // Продукстов в иерархии может быть несколько
                                    IQueryable<ProductTree> productTreeNodes = nodes.Where(n => promoProductTrees.Any(p => p.ProductTreeObjectId == n.ObjectId));

                                    List<Func<Product, bool>> expressionsList = GetExpressionList(productTreeNodes);
                                    if (expressionsList.Any()) {
                                        // Список новых продуктов, подходящих по параметрам фильтрации узлов продуктов, выбранных для данного промо
                                        List<Product> matchesProducts = createdProducts.ToList().Where(p => expressionsList.Any(e => e.Invoke(p))).ToList();
                                        if (matchesProducts.Any()) {
                                            promoToNewProductsList.Add(new Tuple<Promo, IEnumerable<Product>>(promo, matchesProducts));
                                        }
                                    }
                                    IQueryable<Guid> currentPromoProducts = promoProducts.Where(p => p.PromoId == promo.Id).Select(x => x.ProductId);
                                    if (currentPromoProducts.Any()) {
                                        // Удалённые продукты
                                        IQueryable<Product> deletedPromoProducts = deletedProducts.Where(p => currentPromoProducts.Contains(p.Id));
                                        if (deletedPromoProducts.Any()) {
                                            promoToDeletedProductsList.Add(new Tuple<Promo, IEnumerable<Product>>(promo, deletedPromoProducts));
                                        }
                                        // Продукты, которые после изменения больше не подходят под текущие промо
                                        List<Product> changedPromoProducts = changedProducts.Where(p => currentPromoProducts.Contains(p.Id)).ToList();
                                        if (changedPromoProducts.Any()) {
                                            List<Product> sameProducts = changedPromoProducts.Where(p => expressionsList.Any(e => e.Invoke(p))).ToList();
                                            List<Product> changed = changedProducts.Where(p => !sameProducts.Contains(p)).ToList();
                                            if (changed.Any()) {
                                                promoToDeletedProductsList.Add(new Tuple<Promo, IEnumerable<Product>>(promo, changed));
                                            }
                                        }
                                        // Продукты, котрорые после изменения подходят под новые промо
                                        List<Product> changedNotPromoProducts = changedProducts.Where(p => !currentPromoProducts.Contains(p.Id)).ToList();
                                        if (changedNotPromoProducts.Any()) {
                                            List<Product> newProductsForPromo = changedNotPromoProducts.Where(p => expressionsList.Any(e => e.Invoke(p))).ToList();
                                            if (newProductsForPromo.Any()) {
                                                promoToNewProductsList.Add(new Tuple<Promo, IEnumerable<Product>>(promo, newProductsForPromo));
                                            }
                                        }
                                    }
                                }
                                CreateNotification(promoToNewProductsList, "PROMO_PRODUCT_CREATE_NOTIFICATION", template);
                                CreateNotification(promoToDeletedProductsList, "PROMO_PRODUCT_DELETE_NOTIFICATION", template);
                                foreach (ProductChangeIncident incident in changeProductIncidents) {
                                    incident.ProcessDate = DateTimeOffset.Now;
                                }
                                context.SaveChanges();
                            }
                        } else {
                            Errors.Add(String.Format("Empty alert template: {0}", templateFileName));
                        }
                    } else {
                        Errors.Add(String.Format("Could not find alert template: {0}", templateFileName));
                    }
                }
            } catch (Exception e) {
                string msg = String.Format("An error occurred while sending a notification for Product Create Incident: {0}", e.ToString());
                logger.Error(msg);
                Errors.Add(msg);
            } finally {
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
        private void CreateNotification(List<Tuple<Promo, IEnumerable<Product>>> records, string notificationName, string template) {
            List<string> allRows = new List<string>();
            foreach (Tuple<Promo, IEnumerable<Product>> promoToProduct in records) {
                List<string> allRowCells = GetRow(promoToProduct.Item1, propertiesOrder);
                allRowCells.Add(String.Format(cellTemplate, String.Join(";", promoToProduct.Item2.Select(p => p.ZREP))));
                allRows.Add(String.Format(rowTemplate, string.Join("", allRowCells)));
            }            
            string notifyBody = String.Format(template, string.Join("", allRows));

            SendNotification(notifyBody, notificationName);
        }

        private readonly string[] propertiesOrder = new string[] {
            "Number", "Name", "ClientHierarchy", "PromoStatus.Name", "StartDate", "EndDate" };
    }
}