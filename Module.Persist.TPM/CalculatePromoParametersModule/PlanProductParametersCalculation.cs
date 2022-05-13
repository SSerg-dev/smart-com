using Core.Extensions;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl;
using Module.Persist.TPM.Utils;
using Module.Persist.TPM.Utils.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using Persist;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace Module.Persist.TPM.CalculatePromoParametersModule
{
    public class PlanProductParametersCalculation
    {
        /// <summary>
        /// Метод для создания записей в таблице PromoProduct.
        /// Производится подбор списка продуктов по фильтрам выбранных узлов в иерархии продуктов
        /// и запись списка пар PromoId-ProductId в таблицу PromoProduct.
        /// </summary>
        /// <param name="promoId">Id создаваемого/редактируемого промо</param>
        /// <param name="context">Текущий контекст</param>
        public static bool SetPromoProduct(Guid promoId, DatabaseContext context, out string error, bool? duringTheSave = false, List<PromoProductTree> promoProductTrees = null)
        {
            try
            {
                string[] statusesForIncidents = { "OnApproval", "Approved", "Planned" };
                var addedZREPs = new List<string>();
                var deletedZREPs = new List<string>();
                bool needReturnToOnApprovalStatus = false;
                var promo = context.Set<Promo>().Where(x => x.Id == promoId && !x.Disabled).FirstOrDefault();
                var changeProductIncidents = context.Set<ProductChangeIncident>().Where(x => x.NotificationProcessDate == null);
                var changedProducts = changeProductIncidents.Select(x => x.Product.ZREP).Distinct();
                var createdProducts = changeProductIncidents.Where(p => p.IsCreate && !p.IsChecked).Select(i => i.Product.ZREP);
                bool createIncidents = statusesForIncidents.Any(s => s.ToLower() == promo.PromoStatus.SystemName.ToLower());

                var productTreeArray = context.Set<ProductTree>().Where(x => context.Set<PromoProductTree>().Where(p => p.PromoId == promoId && !p.Disabled).Any(p => p.ProductTreeObjectId == x.ObjectId && !x.EndDate.HasValue)).ToArray();

                // добавление записей в таблицу PromoProduct может производиться и при сохранении промо (статус Draft) и при расчете промо (статус !Draft)
                List<Product> filteredProducts = (duringTheSave.HasValue && duringTheSave.Value) ? GetProductFiltered(promoId, context, out error, promoProductTrees) : GetProductFiltered(promoId, context, out error);
                List<string> eanPCs = GetProductListFromAssortmentMatrix(promo, context);
                List<Product> resultProductList = null;

                if (promo.InOut.HasValue && promo.InOut.Value)
                {
                    resultProductList = GetResultProducts(filteredProducts, promo, context);
                    DisableOldIncrementalPromo(context, promo, resultProductList);
                }
                else
                {
                    resultProductList = GetResultProducts(filteredProducts, eanPCs, promo, context);
                    DisablePromoProductCorrections(context, promo, resultProductList);
                }

                var promoProducts = context.Set<PromoProduct>().Where(x => x.PromoId == promoId);
                var incrementalPromoes = context.Set<IncrementalPromo>().Where(x => x.PromoId == promoId);
                var promoProductsNotDisabled = promoProducts.Where(x => !x.Disabled);

                foreach (var promoProduct in promoProductsNotDisabled)
                {
                    if (!resultProductList.Any(x => x.ZREP == promoProduct.ZREP))
                    {
                        if (changedProducts.Contains(promoProduct.ZREP) && createIncidents)
                        {
                            deletedZREPs.Add(promoProduct.ZREP);
                        }
                        promoProduct.Disabled = true;
                        promoProduct.DeletedDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                        needReturnToOnApprovalStatus = true;
                    }
                }

                var draftStatus = context.Set<PromoStatus>().FirstOrDefault(x => x.SystemName == PromoStates.Draft.ToString() && !x.Disabled);
                if (promo.PromoStatus.Id != draftStatus.Id)
                {
                    // Делаем для ускорения вставки записей, через Mapping всё очень долго                    
                    String formatStrPromoProduct = "INSERT INTO [DefaultSchemaSetting].[PromoProduct] ([Id], [Disabled], [DeletedDate], [PromoId], [ProductId], [ZREP], [EAN_Case], [EAN_PC], [ProductEN]) VALUES ('{0}', 0, NULL, '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')";
                    String formatStrIncremental = "INSERT INTO [DefaultSchemaSetting].[IncrementalPromo] ([Id], [Disabled], [DeletedDate], [PromoId], [ProductId]) VALUES ('{0}', 0, NULL, '{1}', '{2}')";
                    foreach (IEnumerable<Product> items in resultProductList.Partition(100))
                    {
                        string insertScript = String.Empty;

                        foreach (Product p in items)
                        {
                            var promoProduct = promoProducts.FirstOrDefault(x => x.ZREP == p.ZREP);
                            if (promoProduct != null && promoProduct.Disabled)
                            {
                                if (changedProducts.Contains(p.ZREP) && createIncidents)
                                {
                                    addedZREPs.Add(p.ZREP);
                                }

                                promoProduct.Disabled = false;
                                promoProduct.DeletedDate = null;
                                needReturnToOnApprovalStatus = true;
                            }
                            else if (promoProduct == null)
                            {
                                if (changedProducts.Contains(p.ZREP) && createIncidents)
                                {
                                    addedZREPs.Add(p.ZREP);
                                }

                                insertScript += String.Format(formatStrPromoProduct, Guid.NewGuid(), promoId, p.Id, p.ZREP, p.EAN_Case, p.EAN_PC, p.ProductEN);
                                needReturnToOnApprovalStatus = true;
                            }

                            if (createdProducts.Any(x => x == p.ZREP) && !addedZREPs.Any(x => x == p.ZREP) && createIncidents)
                            {
                                addedZREPs.Add(p.ZREP);
                            }

                            if (promo.InOut.HasValue && promo.InOut.Value)
                            {
                                var incrementalPromo = incrementalPromoes.FirstOrDefault(x => x.Product.ZREP == p.ZREP);
                                if (incrementalPromo != null && incrementalPromo.Disabled)
                                {
                                    incrementalPromo.Disabled = false;
                                    incrementalPromo.DeletedDate = null;

                                    incrementalPromo.LastModifiedDate = null;
                                    incrementalPromo.PlanPromoIncrementalCases = null;
                                    incrementalPromo.PlanPromoIncrementalLSV = null;

                                    needReturnToOnApprovalStatus = true;
                                }
                                else if (incrementalPromo == null)
                                {
                                    insertScript += String.Format(formatStrIncremental, Guid.NewGuid(), promoId, p.Id);
                                    needReturnToOnApprovalStatus = true;
                                }
                            }
                        }

                        if (!String.IsNullOrEmpty(insertScript))
                        {
                            context.ExecuteSqlCommand(insertScript);
                        }
                    }
                }

                if (addedZREPs.Any() || deletedZREPs.Any())
                {
                    Product p = changeProductIncidents.Select(x => x.Product).FirstOrDefault();
                    if (p != null)
                    {
                        ProductChangeIncident pci = new ProductChangeIncident()
                        {
                            Product = p,
                            ProductId = p.Id,
                            IsRecalculated = true,
                            RecalculatedPromoId = promoId,
                            AddedProductIds = addedZREPs.Any() ? String.Join(";", addedZREPs) : null,
                            ExcludedProductIds = deletedZREPs.Any() ? String.Join(";", deletedZREPs) : null,
                            CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).Value,
                            IsCreate = false,
                            IsChecked = false
                        };
                        context.Set<ProductChangeIncident>().Add(pci);
                    }
                }

                // если добавление записей происходит при сохранении промо (статус Draft), то контекст сохранится в контроллере промо,
                // а если добавление записей происходит при расчете промо (статус !Draft), то сохраняем контекст тут
                if (duringTheSave.HasValue && !duringTheSave.Value)
                {
                    context.SaveChanges();
                }

                return needReturnToOnApprovalStatus;
            }
            catch (Exception e)
            {
                error = e.Message.ToString();
                return false;
            }
        }

        /// <summary>
        /// Отключение старой коррекции продуктов
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="promo"></param>
        /// <param name="resultProductList"></param>
        private static void DisablePromoProductCorrections(DatabaseContext Context, Promo promo, List<Product> resultProductList)
        {
            var newZreps = resultProductList.Select(p => p.ZREP).ToList();
            var promoProductCorrectionToDeleteList = Context.Set<PromoProductsCorrection>()
                .Where(x => !newZreps.Contains(x.PromoProduct.ZREP) &&
                x.PromoProduct.PromoId == promo.Id && x.Disabled != true).ToList();
            foreach (var productCorrection in promoProductCorrectionToDeleteList)
            {
                productCorrection.Disabled = true;
                productCorrection.DeletedDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
            }
            Context.SaveChanges();
        }

        /// <summary>
        /// Отключение старых инкременталов промо
        /// </summary>
        /// <param name="context"></param>
        /// <param name="promo"></param>
        /// <param name="resultProductList"></param>
        public static void DisableOldIncrementalPromo(DatabaseContext context, Promo promo, List<Product> resultProductList)
        {
            var newZreps = resultProductList.Select(p => p.ZREP).ToList();
            var incrementalPromoesToDelete = context.Set<IncrementalPromo>()
                .Where(x => x.PromoId == promo.Id && !newZreps.Contains(x.Product.ZREP) && !x.Disabled)
                .ToList();
            foreach (var incrementalPromo in incrementalPromoesToDelete)
            {
                incrementalPromo.Disabled = true;
                incrementalPromo.DeletedDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
            }
            context.SaveChanges();
        }

        /// <summary>
        /// Метод для формирования списка продуктов для промо.
        /// Возвращает список продуктов для текущего промо с учетом фильтров, ассортиментной матрицы и baseline.
        /// </summary>
        /// <param name="filteredProducts">Список продуктов, подходящих по фильтрам</param>
        /// <param name="eanPCs">Список EAN PC из ассортиментной матрицы</param>
        /// <param name="promo">Промо, для которого подбираются продукты</param>
        /// <param name="context">Текущий контекст</param>
        public static List<Product> GetResultProducts(List<Product> filteredProducts, List<string> eanPCs, Promo promo, DatabaseContext context, IQueryable<Product> productQuery = null)
        {
            List<Product> resultProductList = new List<Product>();

            if (filteredProducts != null)
            {
                resultProductList = filteredProducts.Where(x => eanPCs.Any(y => y == x.EAN_PC)).ToList();
                resultProductList = resultProductList.Intersect(GetCheckedProducts(context, promo, productQuery)).ToList();
            }

            return resultProductList;
        }

        /// <summary>
        /// Метод для формирования списка продуктов для промо.
        /// Возвращает список продуктов для текущего промо с учетом фильтров, ассортиментной матрицы и baseline.
        /// </summary>
        /// <param name="filteredProducts">Список продуктов, подходящих по фильтрам</param>
        /// <param name="promo">Промо, для которого подбираются продукты</param>
        /// <param name="context">Текущий контекст</param>
        public static List<Product> GetResultProducts(List<Product> filteredProducts, Promo promo, DatabaseContext context, IQueryable<Product> productQuery = null)
        {
            List<Product> resultProductList = new List<Product>();

            if (filteredProducts != null)
            {
                var productIds = promo.InOutProductIds.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(x => Guid.Parse(x)).ToList();
                resultProductList = filteredProducts.Where(x => productIds.Any(y => y == x.Id)).ToList();
                resultProductList = resultProductList.Intersect(GetCheckedProducts(context, promo, productQuery)).ToList();
            }

            return resultProductList;
        }

        /// <summary>
        /// Метод для расчета параметров PromoProduct.
        /// Производится подбор подходящих BaseLine и их распределение по всей длительности промо.
        /// </summary>
        /// <param name="promoId">Id создаваемого/редактируемого промо</param>
        /// <param name="context">Текущий контекст</param>
        public static string CalculatePromoProductParameters(Guid promoId, DatabaseContext context)
        {
            try
            {
                var promo = context.Set<Promo>().Where(x => x.Id == promoId && !x.Disabled).FirstOrDefault();
                string message = null;
                Promo promoCopy = new Promo(promo);

                if (promo.StartDate.HasValue && promo.EndDate.HasValue)
                {
                    ClientTree clientNode = context.Set<ClientTree>().Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue).FirstOrDefault();
                    if (clientNode != null)
                    {
                        List<PromoProduct> promoProducts = context.Set<PromoProduct>().Where(x => x.PromoId == promo.Id && !x.Disabled).ToList();
                        double? clientPostPromoEffectW1 = clientNode.PostPromoEffectW1;
                        double? clientPostPromoEffectW2 = clientNode.PostPromoEffectW2;

                        // вначале сбрасываем значения                    
                        ResetProductParams(promoProducts, context);

                        // если стоит флаг inout, расчет производися по другим формулам, подбирать baseline не требуется
                        if (!promo.InOut.HasValue || !promo.InOut.Value)
                        {
                            var promoProductCorrections = context.Set<PromoProductsCorrection>().Where(x => !x.Disabled && x.PromoProduct.PromoId == promo.Id && x.TempId == null);

                            if (!promo.PlanPromoUpliftPercent.HasValue)
                            {
                                message = String.Format("For promo №{0} is no Plan Promo Uplift value. Plan parameters will not be calculated.", promo.Number);
                            }

                            foreach (var promoProduct in promoProducts)
                            {
                                var promoProductCorrection = promoProductCorrections.FirstOrDefault(x => x.PromoProductId == promoProduct.Id && !x.Disabled);
                                var promoProductUplift = promoProductCorrection?.PlanProductUpliftPercentCorrected ?? promoProduct.PlanProductUpliftPercent;
                                promoProduct.PlanProductIncrementalLSV = promoProduct.PlanProductBaselineLSV * promoProductUplift / 100;
                                promoProduct.PlanProductLSV = promoProduct.PlanProductBaselineLSV + promoProduct.PlanProductIncrementalLSV;

                                //Расчет плановых значений PromoProduct
                                promoProduct.PlanProductPCPrice = promoProduct.Product.UOM_PC2Case != 0 ? promoProduct.Price / promoProduct.Product.UOM_PC2Case : null;
                                promoProduct.PlanProductIncrementalCaseQty = promoProduct.PlanProductBaselineCaseQty * promoProductUplift / 100;
                                promoProduct.PlanProductCaseQty = promoProduct.PlanProductBaselineCaseQty + promoProduct.PlanProductIncrementalCaseQty;
                                promoProduct.PlanProductPCQty = promoProduct.Product.UOM_PC2Case != 0 ? (int?)promoProduct.PlanProductCaseQty * promoProduct.Product.UOM_PC2Case : null;
                                promoProduct.PlanProductCaseLSV = promoProduct.PlanProductBaselineCaseQty * promoProduct.Price;
                                promoProduct.PlanProductPCLSV = promoProduct.Product.UOM_PC2Case != 0 ? (int?)promoProduct.PlanProductCaseLSV / promoProduct.Product.UOM_PC2Case : null;

                                if (clientNode != null)
                                {
                                    //TODO: Уточнить насчет деления на 100
                                    promoProduct.PlanProductPostPromoEffectQtyW1 = promoProduct.PlanProductBaselineCaseQty * clientPostPromoEffectW1 / 100 ?? 0;
                                    promoProduct.PlanProductPostPromoEffectQtyW2 = promoProduct.PlanProductBaselineCaseQty * clientPostPromoEffectW2 / 100 ?? 0;
                                    promoProduct.PlanProductPostPromoEffectQty = promoProduct.PlanProductPostPromoEffectQtyW1 + promoProduct.PlanProductPostPromoEffectQtyW2;

                                    promoProduct.PlanProductPostPromoEffectLSVW1 = promoProduct.PlanProductBaselineLSV * clientPostPromoEffectW1 / 100 ?? 0;
                                    promoProduct.PlanProductPostPromoEffectLSVW2 = promoProduct.PlanProductBaselineLSV * clientPostPromoEffectW2 / 100 ?? 0;
                                    promoProduct.PlanProductPostPromoEffectLSV = promoProduct.PlanProductPostPromoEffectLSVW1 + promoProduct.PlanProductPostPromoEffectLSVW2;

                                    promoProduct.PlanProductPostPromoEffectVolumeW1 = promoProduct.PlanProductBaselineVolume * clientPostPromoEffectW1 / 100 ?? 0;
                                    promoProduct.PlanProductPostPromoEffectVolumeW2 = promoProduct.PlanProductBaselineVolume * clientPostPromoEffectW2 / 100 ?? 0;
                                    promoProduct.PlanProductPostPromoEffectVolume = promoProduct.PlanProductPostPromoEffectVolumeW1 + promoProduct.PlanProductPostPromoEffectVolumeW2;
                                }
                            }

                            double? sumPlanProductBaseLineLSV = promoProducts.Sum(x => x.PlanProductBaselineLSV);
                            double? sumPlanProductIncrementalLSV = promoProducts.Sum(x => x.PlanProductIncrementalLSV);
                            if (promo.NeedRecountUplift.Value)
                                promo.PlanPromoUpliftPercent = sumPlanProductBaseLineLSV != 0 ? sumPlanProductIncrementalLSV / sumPlanProductBaseLineLSV * 100 : null;

                            promo.PlanPromoIncrementalLSV = sumPlanProductIncrementalLSV;
                            promo.PlanPromoBaselineLSV = sumPlanProductBaseLineLSV;
                            promo.PlanPromoLSV = promo.PlanPromoBaselineLSV + promo.PlanPromoIncrementalLSV;
                        }
                        else
                        {
                            foreach (var promoProduct in promoProducts)
                            {
                                IncrementalPromo incrementalPromo = context.Set<IncrementalPromo>().Where(x => x.PromoId == promo.Id && x.ProductId == promoProduct.ProductId && !x.Disabled).FirstOrDefault();

                                if (incrementalPromo != null)
                                {
                                    //Расчет плановых значений PromoProduct
                                    promoProduct.PlanProductPCPrice = promoProduct.Product.UOM_PC2Case != 0 ? promoProduct.Price / promoProduct.Product.UOM_PC2Case : null;
                                    promoProduct.PlanProductIncrementalCaseQty = incrementalPromo.PlanPromoIncrementalCases;
                                    promoProduct.PlanProductCaseQty = promoProduct.PlanProductIncrementalCaseQty;
                                    promoProduct.PlanProductPCQty = promoProduct.Product.UOM_PC2Case != 0 ? (int?)promoProduct.PlanProductCaseQty * promoProduct.Product.UOM_PC2Case : null;
                                    promoProduct.PlanProductCaseLSV = promoProduct.PlanProductCaseQty * promoProduct.Price;
                                    incrementalPromo.PlanPromoIncrementalLSV = (promoProduct.Price ?? 0) * (incrementalPromo.PlanPromoIncrementalCases ?? 0);
                                    promoProduct.PlanProductIncrementalLSV = incrementalPromo.PlanPromoIncrementalLSV;
                                    promoProduct.PlanProductLSV = promoProduct.PlanProductIncrementalLSV;

                                    // TODO: удаляем?
                                    //promoProduct.PlanProductPCLSV = promoProduct.Product.UOM_PC2Case != 0 ? (int?)promoProduct.PlanProductCaseLSV / promoProduct.Product.UOM_PC2Case : null;
                                }
                                else
                                {
                                    message = String.Format("Incremental promo was not found for product with ZREP: {0}", promoProduct.Product.ZREP);
                                }

                                //promoProduct.PlanProductUpliftPercent = promo.PlanPromoUpliftPercent;

                                promoProduct.PlanProductPostPromoEffectQtyW1 = 0;
                                promoProduct.PlanProductPostPromoEffectQtyW2 = 0;
                                promoProduct.PlanProductPostPromoEffectQty = 0;
                                promoProduct.PlanProductPostPromoEffectLSVW1 = 0;
                                promoProduct.PlanProductPostPromoEffectLSVW2 = 0;
                                promoProduct.PlanProductPostPromoEffectLSV = 0;
                                promoProduct.PlanProductPostPromoEffectVolumeW1 = 0;
                                promoProduct.PlanProductPostPromoEffectVolumeW2 = 0;
                                promoProduct.PlanProductPostPromoEffectVolume = 0;
                            }

                            promo.PlanPromoBaselineLSV = null;

                            double? sumPlanProductIncrementalLSV = promoProducts.Sum(x => x.PlanProductIncrementalLSV);
                            // LSV = Qty ?
                            promo.PlanPromoIncrementalLSV = sumPlanProductIncrementalLSV;
                            promo.PlanPromoLSV = promo.PlanPromoIncrementalLSV;
                        }

                        if (PromoUtils.HasChanges(context.ChangeTracker, promo.Id))
                        {
                            promo.LastChangedDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                            if (IsDemandChanged(promo, promoCopy))
                            {
                                promo.LastChangedDateDemand = promo.LastChangedDate;
                                promo.LastChangedDateFinance = promo.LastChangedDate;
                            }
                        }

                        context.SaveChanges();
                    }
                    else
                    {
                        message = String.Format("Plan parameters can not be recalculated, because client was not found for this promo.");
                    }
                }
                else
                {
                    message = String.Format("Promo has not start date or end date");
                }
                return message;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);

                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public static string CalculateBaseline(DatabaseContext context, Guid promoId)
        {
            string message = null;
            var promo = context.Set<Promo>().Where(x => x.Id == promoId && !x.Disabled).FirstOrDefault();

            if (promo != null)
            {
                BaselineAndPriceCalculation.SetPriceForPromoProducts(context, promo);

                bool isOnInvoice = promo.IsOnInvoice;
                //Подбор baseline производится по датам ПРОВЕДЕНИЯ промо независимо от типа промо (on/off-invoice)
                message = BaselineAndPriceCalculation.CalculateBaselineQtyAndLSV(promo, context, promo.StartDate, promo.EndDate, isOnInvoice);
            }
            else
            {
                message = String.Format("Promo with Id = {0} was not found", promoId);
            }
            return message;
        }

        /// <summary>
        /// Получить продукты, подходящие под фильтр
        /// </summary>
        /// <param name="promoId">ID промо</param>
        /// <param name="context">Контекст БД</param>
        /// <param name="error">Сообщения об ошибках</param>
        /// <returns></returns>
        public static List<Product> GetProductFiltered(Guid promoId, DatabaseContext context, out string error, List<PromoProductTree> promoProductTrees = null)
        {
            // также используется в промо для проверки, если нет продуктов, то сохранение/редактирование отменяется
            // отдельно т.к. заполнение может оказаться очень долгой операцией
            List<Product> product = null;
            List<Product> filteredProductList = new List<Product>();
            error = null;

            try
            {
                var promo = context.Set<Promo>().Where(x => x.Id == promoId && !x.Disabled).FirstOrDefault();

                // из-за отказа от множества SaveChanges приходиться возить с собой список узлов в прод. дереве
                ProductTree[] productTreeArray;
                if (promoProductTrees == null)
                {
                    productTreeArray = context.Set<ProductTree>().Where(x => context.Set<PromoProductTree>().Where(p => p.PromoId == promoId && !p.Disabled).Any(p => p.ProductTreeObjectId == x.ObjectId && !x.EndDate.HasValue)).ToArray();
                }
                else
                {
                    productTreeArray = context.Set<ProductTree>().Where(x => !x.EndDate.HasValue).ToArray().Where(n => promoProductTrees.Any(p => p.ProductTreeObjectId == n.ObjectId)).ToArray();
                }

                product = context.Set<Product>().Where(x => !x.Disabled).ToList();

                foreach (var productTree in productTreeArray)
                {
                    var stringFilter = productTree.Filter;
                    // можно и на 0 проверить, но вдруг будет пустой фильтр вида "{}"
                    if (stringFilter.Length < 2)
                        throw new Exception("Filter for product " + productTree.FullPathName + " is empty");

                    // Преобразование строки фильтра в соответствующий класс
                    FilterNode filter = stringFilter.ConvertToNode();

                    // Создание функции фильтрации на основе построенного фильтра
                    var expr = filter.ToExpressionTree<Product>();

                    // Список продуктов, подходящих по параметрам фильтрации
                    product = product.Where(expr.Compile()).ToList();

                    filteredProductList = filteredProductList.Union(product).ToList();
                    product = context.Set<Product>().Where(x => !x.Disabled).ToList();
                }

                if (filteredProductList.Count == 0)
                {
                    throw new Exception("No suitable products were found for the current PROMO");
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                filteredProductList = null;
            }

            return filteredProductList;
        }
        public static bool IsProductListEmpty(Promo promo, DatabaseContext context, out string error, List<PromoProductTree> promoProductTrees = null)
        {
            List<Product> filteredProducts = GetProductFiltered(promo.Id, context, out error, promoProductTrees);
            List<string> eanPCs = GetProductListFromAssortmentMatrix(promo, context);
            List<Product> resultProductList = GetResultProducts(filteredProducts, eanPCs, promo, context);
            bool isProductListEmpty = !(resultProductList.Count() > 0);

            return isProductListEmpty;
        }

        /// <summary>
        /// Формирует список продуктов из записей таблицы AssortimentMatrix, которые предварительно фильтруются по клиенту и дате.
        /// Promo.Dispatches.Start должна быть между AssortimentMatrix.Start и AssortimentMatrix.End включительно.
        /// </summary>
        /// <param name="promo">Модель Promo</param>
        /// <param name="context">Контекст базы данных</param>
        /// <returns>Возвращает список продуктов, сформированный из записей таблицы AssortimentMatrix.</returns>
        public static List<string> GetProductListFromAssortmentMatrix(Promo promo, DatabaseContext context)
        {
            // Список продуктов из ассортиментной матрицы, который будет возвращен.
            var productListFromAssortimentMatrix = new List<Product>();

            // Отфильтрованные записи из таблицы AssortimentMatrix.
            var assortimentMatrixFilteredRecords = context.Set<AssortmentMatrix>().Where(x => !x.Disabled);
            assortimentMatrixFilteredRecords = assortimentMatrixFilteredRecords.Where(x => x.ClientTreeId == promo.ClientTreeKeyId);
            assortimentMatrixFilteredRecords = assortimentMatrixFilteredRecords.Where(x => promo.DispatchesStart >= x.StartDate && promo.DispatchesStart <= x.EndDate);
            List<string> eanPCList = assortimentMatrixFilteredRecords.Select(x => x.Product.EAN_PC).ToList();

            return eanPCList;
        }

        public static List<string> GetProductListFromAssortmentMatrix(DatabaseContext context, int clientTreeKeyId, DateTimeOffset dispatchesStart, DateTimeOffset dispatchesEnd)
        {
            // Список продуктов из ассортиментной матрицы, который будет возвращен.
            var productListFromAssortimentMatrix = new List<Product>();

            // Отфильтрованные записи из таблицы AssortimentMatrix.
            var assortimentMatrixFilteredRecords = context.Set<AssortmentMatrix>().Where(x => !x.Disabled);
            assortimentMatrixFilteredRecords = assortimentMatrixFilteredRecords.Where(x => x.ClientTreeId == clientTreeKeyId);
            assortimentMatrixFilteredRecords = assortimentMatrixFilteredRecords.Where(x => dispatchesStart >= x.StartDate && dispatchesStart <= x.EndDate);
            List<string> eanPCList = assortimentMatrixFilteredRecords.Select(x => x.Product.EAN_PC).ToList();

            return eanPCList;
        }

        /// <summary>
        /// Сбросить значения для продуктов
        /// </summary>
        /// <param name="promoProducts">Список продуктов</param>
        /// <param name="context">Контекст БД</param>
        private static void ResetProductParams(List<PromoProduct> promoProducts, DatabaseContext context)
        {
            foreach (PromoProduct product in promoProducts)
            {
                //product.PlanProductBaselineLSV = null;
                //product.PlanProductBaselineCaseQty = null;
                //product.ProductBaselinePrice = null;
                product.PlanProductPCPrice = null;
                product.PlanProductIncrementalCaseQty = null;
                product.PlanProductCaseQty = null;
                product.PlanProductPCQty = null;
                product.PlanProductCaseLSV = null;
                product.PlanProductPCLSV = null;
                //product.PlanProductUpliftPercent = null;
                product.PlanProductPostPromoEffectQtyW1 = null;
                product.PlanProductPostPromoEffectQtyW2 = null;
                product.PlanProductPostPromoEffectQty = null;
                product.PlanProductPostPromoEffectLSVW1 = null;
                product.PlanProductPostPromoEffectLSVW2 = null;
                product.PlanProductPostPromoEffectLSV = null;
            }
        }

        public static List<Product> GetCheckedProducts(DatabaseContext context, Promo promo, IQueryable<Product> productQuery = null)
        {
            List<Product> products = new List<Product>();
            if (productQuery == null)
            {
                productQuery = context.Set<Product>().Where(x => !x.Disabled);
            }

            string time, time2;
            Stopwatch s = new Stopwatch();
            if (!String.IsNullOrEmpty(promo.InOutProductIds))
            {
                var productIds = promo.InOutProductIds.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(x => Guid.Parse(x)).ToList();
                products = productQuery.Where(x => productIds.Contains(x.Id)).ToList();
            }

            return products;
        }

        public static bool IsDemandChanged(Promo oldPromo, Promo newPromo)
        {
            if (oldPromo.PlanPromoLSV != newPromo.PlanPromoLSV
                || oldPromo.PlanPromoIncrementalLSV != newPromo.PlanPromoIncrementalLSV)
                return true;
            else return false;
        }
    }
}