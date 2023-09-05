using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl;
using Module.Persist.TPM.Utils;
using Persist;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

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
                var promo = context.Set<Promo>()
                    .Include(g => g.PromoPriceIncrease.PromoProductPriceIncreases.Select(y => y.ProductCorrectionPriceIncreases))
                    .Where(x => x.Id == promoId && !x.Disabled).FirstOrDefault();
                var changeProductIncidents = context.Set<ProductChangeIncident>().Where(x => x.NotificationProcessDate == null).ToList();
                var changedProducts = changeProductIncidents.Select(x => x.Product.ZREP).Distinct();
                var createdProducts = changeProductIncidents.Where(p => p.IsCreate && !p.IsChecked).Select(i => i.Product.ZREP);
                bool createIncidents = statusesForIncidents.Any(s => s.ToLower() == promo.PromoStatus.SystemName.ToLower());

                var productTreeArray = context.Set<ProductTree>().Where(x => context.Set<PromoProductTree>().Where(p => p.PromoId == promoId && !p.Disabled).Any(p => p.ProductTreeObjectId == x.ObjectId && !x.EndDate.HasValue)).ToArray();
                // добавление пустого PromoPriceIncrease к Promo если его нет
                if (promo.PromoPriceIncrease is null)
                {
                    promo.PromoPriceIncrease = new PromoPriceIncrease();
                    if (promo.PlanPromoUpliftPercentPI != null)
                    {
                        promo.PromoPriceIncrease.PlanPromoUpliftPercent = promo.PlanPromoUpliftPercentPI;
                    }
                }
                else
                {
                    promo.PromoPriceIncrease.PlanPromoUpliftPercent = promo.PlanPromoUpliftPercentPI;
                }
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

                var promoProducts = context.Set<PromoProduct>()
                    .Include(g => g.PromoProductsCorrections)
                    .Where(x => x.PromoId == promoId)
                    .ToList();
                var incrementalPromoes = context.Set<IncrementalPromo>().Where(x => x.PromoId == promoId).ToList();
                var promoProductsNotDisabled = promoProducts.Where(x => !x.Disabled).ToList();

                if (promoProductsNotDisabled.Count > 0)
                {
                    if (promo.PromoPriceIncrease.PromoProductPriceIncreases == null || promo.PromoPriceIncrease.PromoProductPriceIncreases.Count == 0)
                    {
                        FillPriceIncreaseProdusts(promo, promoProductsNotDisabled.ToList());
                    }
                    else
                    {
                        CopyPriceIncreaseProdusts(promo, promoProductsNotDisabled.ToList(), context);
                    }
                }

                foreach (PromoProduct promoProduct in promoProductsNotDisabled)
                {
                    if (!resultProductList.Any(x => x.ZREP == promoProduct.ZREP))
                    {
                        if (changedProducts.Contains(promoProduct.ZREP) && createIncidents)
                        {
                            deletedZREPs.Add(promoProduct.ZREP);
                        }
                        // удаляем старый promoProduct если есть из базы
                        if (promoProducts.Where(g => g.Disabled).Any(g => g.ZREP == promoProduct.ZREP))
                        {
                            PromoProduct promoProductDelete = promoProducts.FirstOrDefault(g => g.Disabled && g.ZREP == promoProduct.ZREP);
                            context.Set<PromoProduct>().Remove(promoProductDelete);
                        }
                        promoProduct.Disabled = true;
                        promoProduct.DeletedDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                        needReturnToOnApprovalStatus = true;
                    }
                    // PriceIncrease
                    if (promo.PromoPriceIncrease.PromoProductPriceIncreases.Where(x => !x.Disabled) != null)
                    {
                        if (!resultProductList.Any(x => x.ZREP == promoProduct.ZREP))
                        {
                            PromoProductPriceIncrease promoProductPriceIncrease = promo.PromoPriceIncrease.PromoProductPriceIncreases.FirstOrDefault(g => g.ZREP == promoProduct.ZREP && !g.Disabled);
                            // удаляем старый promoProductPriceIncrease если есть из базы
                            if (promo.PromoPriceIncrease.PromoProductPriceIncreases.Where(g => g.Disabled).Any(g => g.ZREP == promoProductPriceIncrease.ZREP))
                            {
                                PromoProductPriceIncrease promoProductPIDelete = promo.PromoPriceIncrease.PromoProductPriceIncreases.FirstOrDefault(g => g.Disabled && g.ZREP == promoProductPriceIncrease.ZREP);
                                context.Set<PromoProductPriceIncrease>().Remove(promoProductPIDelete);
                            }
                            promoProductPriceIncrease.Disabled = true;
                            promoProductPriceIncrease.DeletedDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                        }
                    }
                }

                var draftStatus = context.Set<PromoStatus>().FirstOrDefault(x => x.SystemName == PromoStates.Draft.ToString() && !x.Disabled);
                if (promo.PromoStatus.Id != draftStatus.Id)
                {
                    foreach (Product product in resultProductList)
                    {
                        if (changedProducts.Contains(product.ZREP) && createIncidents)
                        {
                            addedZREPs.Add(product.ZREP);
                        }
                        PromoProduct promoProduct = promoProducts.FirstOrDefault(x => x.ZREP == product.ZREP);
                        if (promoProduct != null && promoProduct.Disabled)
                        {
                            if (!promoProducts.Where(g => !g.Disabled).Any(g => g.ZREP == promoProduct.ZREP))
                            {
                                promoProduct.Disabled = false;
                                promoProduct.DeletedDate = null;
                                needReturnToOnApprovalStatus = true;
                                // PriceIncrease
                                PromoProductPriceIncrease promoProductPriceIncrease = promo.PromoPriceIncrease.PromoProductPriceIncreases.FirstOrDefault(g => g.PromoProductId == promoProduct.Id);
                                promoProductPriceIncrease.Disabled = false;
                                promoProductPriceIncrease.DeletedDate = null;
                            }                            
                        }
                        else if (promoProduct == null)
                        {
                            var newPromoProduct = new PromoProduct
                            {
                                PromoId = promoId,
                                ZREP = product.ZREP,
                                EAN_Case = product.EAN_Case,
                                EAN_PC = product.EAN_PC,
                                ProductEN = product.ProductEN,
                                TPMmode = promo.TPMmode
                            };
                            product.PromoProducts.Add(newPromoProduct);
                            needReturnToOnApprovalStatus = true;
                            // PriceIncrease
                            if (promo.PromoPriceIncrease.PromoProductPriceIncreases == null)
                            {
                                promo.PromoPriceIncrease.PromoProductPriceIncreases = new List<PromoProductPriceIncrease>();
                            }
                            promo.PromoPriceIncrease.PromoProductPriceIncreases.Add(new PromoProductPriceIncrease
                            {
                                PromoProduct = newPromoProduct,
                                ZREP = product.ZREP,
                                EAN_Case = product.EAN_Case,
                                EAN_PC = product.EAN_PC,
                                ProductEN = product.ProductEN
                            });
                        }

                        if (createdProducts.Any(x => x == product.ZREP) && !addedZREPs.Any(x => x == product.ZREP) && createIncidents)
                        {
                            addedZREPs.Add(product.ZREP);
                        }

                        if (promo.InOut.HasValue && promo.InOut.Value)
                        {
                            var incrementalPromo = incrementalPromoes.FirstOrDefault(x => x.Product.ZREP == product.ZREP);
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
                                product.IncrementalPromoes.Add(
                                    new IncrementalPromo
                                    {
                                        PromoId = promoId,
                                        ProductId = product.Id,
                                        TPMmode = promo.TPMmode
                                    }
                                    );
                                needReturnToOnApprovalStatus = true;
                            }
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
                            AddedProductIds = addedZREPs.Any() ? string.Join(";", addedZREPs) : null,
                            ExcludedProductIds = deletedZREPs.Any() ? string.Join(";", deletedZREPs) : null,
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
                    PlanPostPromoEffectSelection.SelectPPEforPromoProduct(context.Set<PromoProduct>().Where(g=>g.PromoId == promo.Id).ToList(), promo, context);
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
            // PriceIncrease
            if (promo.PromoPriceIncrease.PromoProductPriceIncreases != null)
            {
                foreach (PromoProductPriceIncrease promoProductPriceIncrease in promo.PromoPriceIncrease.PromoProductPriceIncreases.Where(g => !newZreps.Contains(g.ZREP) && !g.Disabled))
                {
                    if (promoProductPriceIncrease.ProductCorrectionPriceIncreases.FirstOrDefault() != null)
                    {
                        promoProductPriceIncrease.ProductCorrectionPriceIncreases.FirstOrDefault().Disabled = true;
                        promoProductPriceIncrease.ProductCorrectionPriceIncreases.FirstOrDefault().DeletedDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                    }
                }
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
                var promo = context.Set<Promo>()
                    .Include(g => g.PromoPriceIncrease)
                    .Where(x => x.Id == promoId && !x.Disabled)
                    .FirstOrDefault();
                string message = null;
                Promo promoCopy = AutomapperProfiles.PromoCopy(promo);

                if (promo.StartDate.HasValue && promo.EndDate.HasValue)
                {

                    List<PromoProduct> promoProducts = context.Set<PromoProduct>()
                        .Include(x => x.PromoProductPriceIncreases.Select(g => g.ProductCorrectionPriceIncreases))
                        .Where(x => x.PromoId == promo.Id && !x.Disabled)
                        .ToList();

                    // вначале сбрасываем значения                    
                    ResetProductParams(promoProducts, context);

                    // если стоит флаг inout, расчет производися по другим формулам, подбирать baseline не требуется
                    if (!promo.InOut.HasValue || !promo.InOut.Value)
                    {
                        var promoProductCorrections = context.Set<PromoProductsCorrection>().Where(x => !x.Disabled && x.PromoProduct.PromoId == promo.Id && x.TempId == null);

                        if (!promo.PlanPromoUpliftPercent.HasValue)
                        {
                            message = string.Format("For promo №{0} is no Plan Promo Uplift value. Plan parameters will not be calculated.", promo.Number);
                        }

                        foreach (PromoProduct promoProduct in promoProducts)
                        {
                            PromoProductsCorrection promoProductCorrection = promoProductCorrections.FirstOrDefault(x => x.PromoProductId == promoProduct.Id && !x.Disabled);
                            double? promoProductUplift = promoProductCorrection?.PlanProductUpliftPercentCorrected ?? promoProduct.PlanProductUpliftPercent;
                            promoProduct.PlanProductIncrementalLSV = promoProduct.PlanProductBaselineLSV * promoProductUplift / 100;
                            promoProduct.PlanProductLSV = promoProduct.PlanProductBaselineLSV + promoProduct.PlanProductIncrementalLSV;

                            //Расчет плановых значений PromoProduct
                            promoProduct.PlanProductPCPrice = promoProduct.Product.UOM_PC2Case != 0 ? promoProduct.Price / promoProduct.Product.UOM_PC2Case : null;
                            promoProduct.PlanProductIncrementalCaseQty = promoProduct.PlanProductBaselineCaseQty * promoProductUplift / 100;
                            promoProduct.PlanProductCaseQty = promoProduct.PlanProductBaselineCaseQty + promoProduct.PlanProductIncrementalCaseQty;
                            promoProduct.PlanProductPCQty = promoProduct.Product.UOM_PC2Case != 0 ? (int?)promoProduct.PlanProductCaseQty * promoProduct.Product.UOM_PC2Case : null;
                            promoProduct.PlanProductCaseLSV = promoProduct.PlanProductBaselineCaseQty * promoProduct.Price;
                            promoProduct.PlanProductPCLSV = promoProduct.Product.UOM_PC2Case != 0 ? (int?)promoProduct.PlanProductCaseLSV / promoProduct.Product.UOM_PC2Case : null;


                            //TODO: Уточнить насчет деления на 100
                            promoProduct.PlanProductPostPromoEffectQtyW1 = promoProduct.PlanProductBaselineCaseQty * promoProduct.PlanProductPostPromoEffectW1 / 100 ?? 0;
                            promoProduct.PlanProductPostPromoEffectQtyW2 = promoProduct.PlanProductBaselineCaseQty * promoProduct.PlanProductPostPromoEffectW2 / 100 ?? 0;
                            promoProduct.PlanProductPostPromoEffectQty = promoProduct.PlanProductPostPromoEffectQtyW1 + promoProduct.PlanProductPostPromoEffectQtyW2;

                            promoProduct.PlanProductPostPromoEffectLSVW1 = promoProduct.PlanProductBaselineLSV * promoProduct.PlanProductPostPromoEffectW1 / 100 ?? 0;
                            promoProduct.PlanProductPostPromoEffectLSVW2 = promoProduct.PlanProductBaselineLSV * promoProduct.PlanProductPostPromoEffectW2 / 100 ?? 0;
                            promoProduct.PlanProductPostPromoEffectLSV = promoProduct.PlanProductPostPromoEffectLSVW1 + promoProduct.PlanProductPostPromoEffectLSVW2;

                            promoProduct.PlanProductPostPromoEffectVolumeW1 = promoProduct.PlanProductBaselineVolume * promoProduct.PlanProductPostPromoEffectW1 / 100 ?? 0;
                            promoProduct.PlanProductPostPromoEffectVolumeW2 = promoProduct.PlanProductBaselineVolume * promoProduct.PlanProductPostPromoEffectW2 / 100 ?? 0;
                            promoProduct.PlanProductPostPromoEffectVolume = promoProduct.PlanProductPostPromoEffectVolumeW1 + promoProduct.PlanProductPostPromoEffectVolumeW2;
                        }

                        double? sumPlanProductBaseLineLSV = promoProducts.Sum(x => x.PlanProductBaselineLSV);
                        double? sumPlanProductIncrementalLSV = promoProducts.Sum(x => x.PlanProductIncrementalLSV);
                        if (promo.NeedRecountUplift.Value)
                            promo.PlanPromoUpliftPercent = sumPlanProductBaseLineLSV != 0 ? sumPlanProductIncrementalLSV / sumPlanProductBaseLineLSV * 100 : null;

                        promo.PlanPromoIncrementalLSV = sumPlanProductIncrementalLSV;
                        promo.PlanPromoBaselineLSV = sumPlanProductBaseLineLSV;
                        promo.PlanPromoLSV = promo.PlanPromoBaselineLSV + promo.PlanPromoIncrementalLSV;
                        // PriceIncrease
                        foreach (PromoProductPriceIncrease promoProductPriceIncrease in promoProducts.SelectMany(g => g.PromoProductPriceIncreases))
                        {
                            var promoProductCorrectionPI = promoProductPriceIncrease.ProductCorrectionPriceIncreases.FirstOrDefault(x => !x.Disabled);
                            var promoProductUpliftPI = promoProductCorrectionPI?.PlanProductUpliftPercentCorrected ?? promoProductPriceIncrease.PlanProductUpliftPercent;
                            promoProductPriceIncrease.PlanProductIncrementalLSV = promoProductPriceIncrease.PlanProductBaselineLSV * promoProductUpliftPI / 100;
                            promoProductPriceIncrease.PlanProductLSV = promoProductPriceIncrease.PlanProductBaselineLSV + promoProductPriceIncrease.PlanProductIncrementalLSV;

                            //Расчет плановых значений PromoProduct
                            promoProductPriceIncrease.PlanProductPCPrice = promoProductPriceIncrease.PromoProduct.Product.UOM_PC2Case != 0 ? promoProductPriceIncrease.Price / promoProductPriceIncrease.PromoProduct.Product.UOM_PC2Case : null;
                            promoProductPriceIncrease.PlanProductIncrementalCaseQty = promoProductPriceIncrease.PlanProductBaselineCaseQty * promoProductUpliftPI / 100;
                            promoProductPriceIncrease.PlanProductCaseQty = promoProductPriceIncrease.PlanProductBaselineCaseQty + promoProductPriceIncrease.PlanProductIncrementalCaseQty;
                            promoProductPriceIncrease.PlanProductPCQty = promoProductPriceIncrease.PromoProduct.Product.UOM_PC2Case != 0 ? (int?)promoProductPriceIncrease.PlanProductCaseQty * promoProductPriceIncrease.PromoProduct.Product.UOM_PC2Case : null;
                            promoProductPriceIncrease.PlanProductCaseLSV = promoProductPriceIncrease.PlanProductBaselineCaseQty * promoProductPriceIncrease.Price;
                            promoProductPriceIncrease.PlanProductPCLSV = promoProductPriceIncrease.PromoProduct.Product.UOM_PC2Case != 0 ? (int?)promoProductPriceIncrease.PlanProductCaseLSV / promoProductPriceIncrease.PromoProduct.Product.UOM_PC2Case : null;


                            //TODO: Уточнить насчет деления на 100
                            promoProductPriceIncrease.PlanProductPostPromoEffectQtyW1 = promoProductPriceIncrease.PlanProductBaselineCaseQty * promoProductPriceIncrease.PromoProduct.PlanProductPostPromoEffectW1 / 100 ?? 0;
                            promoProductPriceIncrease.PlanProductPostPromoEffectQtyW2 = promoProductPriceIncrease.PlanProductBaselineCaseQty * promoProductPriceIncrease.PromoProduct.PlanProductPostPromoEffectW2 / 100 ?? 0;
                            promoProductPriceIncrease.PlanProductPostPromoEffectQty = promoProductPriceIncrease.PlanProductPostPromoEffectQtyW1 + promoProductPriceIncrease.PlanProductPostPromoEffectQtyW2;

                            promoProductPriceIncrease.PlanProductPostPromoEffectLSVW1 = promoProductPriceIncrease.PlanProductBaselineLSV * promoProductPriceIncrease.PromoProduct.PlanProductPostPromoEffectW1 / 100 ?? 0;
                            promoProductPriceIncrease.PlanProductPostPromoEffectLSVW2 = promoProductPriceIncrease.PlanProductBaselineLSV * promoProductPriceIncrease.PromoProduct.PlanProductPostPromoEffectW2 / 100 ?? 0;
                            promoProductPriceIncrease.PlanProductPostPromoEffectLSV = promoProductPriceIncrease.PlanProductPostPromoEffectLSVW1 + promoProductPriceIncrease.PlanProductPostPromoEffectLSVW2;

                            promoProductPriceIncrease.PlanProductPostPromoEffectVolumeW1 = promoProductPriceIncrease.PlanProductBaselineVolume * promoProductPriceIncrease.PromoProduct.PlanProductPostPromoEffectW1 / 100 ?? 0;
                            promoProductPriceIncrease.PlanProductPostPromoEffectVolumeW2 = promoProductPriceIncrease.PlanProductBaselineVolume * promoProductPriceIncrease.PromoProduct.PlanProductPostPromoEffectW2 / 100 ?? 0;
                            promoProductPriceIncrease.PlanProductPostPromoEffectVolume = promoProductPriceIncrease.PlanProductPostPromoEffectVolumeW1 + promoProductPriceIncrease.PlanProductPostPromoEffectVolumeW2;

                        }

                        double? sumPlanProductBaseLineLSVPI = promoProducts.SelectMany(g => g.PromoProductPriceIncreases).Sum(x => x.PlanProductBaselineLSV);
                        double? sumPlanProductIncrementalLSVPI = promoProducts.SelectMany(g => g.PromoProductPriceIncreases).Sum(x => x.PlanProductIncrementalLSV);
                        if (!promo.NeedRecountUpliftPI)
                        {
                            promo.PromoPriceIncrease.PlanPromoUpliftPercent = sumPlanProductBaseLineLSVPI != 0 ? sumPlanProductIncrementalLSVPI / sumPlanProductBaseLineLSVPI * 100 : null;
                            promo.PlanPromoUpliftPercentPI = sumPlanProductBaseLineLSVPI != 0 ? sumPlanProductIncrementalLSVPI / sumPlanProductBaseLineLSVPI * 100 : null;
                        }


                        promo.PromoPriceIncrease.PlanPromoIncrementalLSV = sumPlanProductIncrementalLSVPI;
                        promo.PromoPriceIncrease.PlanPromoBaselineLSV = sumPlanProductBaseLineLSVPI;
                        promo.PromoPriceIncrease.PlanPromoLSV = promo.PromoPriceIncrease.PlanPromoBaselineLSV + promo.PromoPriceIncrease.PlanPromoIncrementalLSV;
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
                                message = string.Format("Incremental promo was not found for product with ZREP: {0}", promoProduct.Product.ZREP);
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
                        // PriceIncrease
                        foreach (PromoProductPriceIncrease promoProductPriceIncrease in promoProducts.SelectMany(g => g.PromoProductPriceIncreases))
                        {
                            IncrementalPromo incrementalPromo = context.Set<IncrementalPromo>()
                                .Where(x => x.PromoId == promo.Id && x.ProductId == promoProductPriceIncrease.PromoProduct.ProductId && !x.Disabled).FirstOrDefault();

                            if (incrementalPromo != null)
                            {
                                //Расчет плановых значений PromoProduct
                                promoProductPriceIncrease.PlanProductPCPrice = promoProductPriceIncrease.PromoProduct.Product.UOM_PC2Case != 0 ? promoProductPriceIncrease.Price / promoProductPriceIncrease.PromoProduct.Product.UOM_PC2Case : null;
                                promoProductPriceIncrease.PlanProductIncrementalCaseQty = incrementalPromo.PlanPromoIncrementalCases;
                                promoProductPriceIncrease.PlanProductCaseQty = promoProductPriceIncrease.PlanProductIncrementalCaseQty;
                                promoProductPriceIncrease.PlanProductPCQty = promoProductPriceIncrease.PromoProduct.Product.UOM_PC2Case != 0 ? (int?)promoProductPriceIncrease.PlanProductCaseQty * promoProductPriceIncrease.PromoProduct.Product.UOM_PC2Case : null;
                                promoProductPriceIncrease.PlanProductCaseLSV = promoProductPriceIncrease.PlanProductCaseQty * promoProductPriceIncrease.Price;
                                incrementalPromo.PlanPromoIncrementalLSV = (promoProductPriceIncrease.Price ?? 0) * (incrementalPromo.PlanPromoIncrementalCases ?? 0);
                                promoProductPriceIncrease.PlanProductIncrementalLSV = incrementalPromo.PlanPromoIncrementalLSV;
                                promoProductPriceIncrease.PlanProductLSV = promoProductPriceIncrease.PlanProductIncrementalLSV;

                                // TODO: удаляем?
                                //promoProduct.PlanProductPCLSV = promoProduct.Product.UOM_PC2Case != 0 ? (int?)promoProduct.PlanProductCaseLSV / promoProduct.Product.UOM_PC2Case : null;
                            }
                            else
                            {
                                message = string.Format("Incremental promo was not found for product with ZREP: {0}", promoProductPriceIncrease.PromoProduct.Product.ZREP);
                            }

                            //promoProduct.PlanProductUpliftPercent = promo.PlanPromoUpliftPercent;

                            promoProductPriceIncrease.PlanProductPostPromoEffectQtyW1 = 0;
                            promoProductPriceIncrease.PlanProductPostPromoEffectQtyW2 = 0;
                            promoProductPriceIncrease.PlanProductPostPromoEffectQty = 0;
                            promoProductPriceIncrease.PlanProductPostPromoEffectLSVW1 = 0;
                            promoProductPriceIncrease.PlanProductPostPromoEffectLSVW2 = 0;
                            promoProductPriceIncrease.PlanProductPostPromoEffectLSV = 0;
                            promoProductPriceIncrease.PlanProductPostPromoEffectVolumeW1 = 0;
                            promoProductPriceIncrease.PlanProductPostPromoEffectVolumeW2 = 0;
                            promoProductPriceIncrease.PlanProductPostPromoEffectVolume = 0;
                        }

                        promo.PlanPromoBaselineLSV = null;

                        double? sumPlanProductIncrementalLSV = promoProducts.Sum(x => x.PlanProductIncrementalLSV);
                        // LSV = Qty ?
                        promo.PlanPromoIncrementalLSV = sumPlanProductIncrementalLSV;
                        promo.PlanPromoLSV = promo.PlanPromoIncrementalLSV;
                        // PriceIncrease
                        promo.PromoPriceIncrease.PlanPromoBaselineLSV = null;

                        double? sumPlanProductIncrementalLSVPI = promoProducts.SelectMany(g => g.PromoProductPriceIncreases).Sum(x => x.PlanProductIncrementalLSV);
                        // LSV = Qty ?
                        promo.PromoPriceIncrease.PlanPromoIncrementalLSV = sumPlanProductIncrementalLSVPI;
                        promo.PromoPriceIncrease.PlanPromoLSV = promo.PromoPriceIncrease.PlanPromoIncrementalLSV;
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
                    message = string.Format("Promo has not start date or end date");
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
            Promo promo = context.Set<Promo>()
                .Include(g => g.PromoPriceIncrease.PromoProductPriceIncreases.Select(y => y.ProductCorrectionPriceIncreases))
                .Include(g => g.PromoPriceIncrease.PromoProductPriceIncreases.Select(y => y.PromoProduct))
                .Include(g => g.IncrementalPromoes)
                .Where(x => x.Id == promoId && !x.Disabled).FirstOrDefault();

            if (promo != null)
            {
                BaselineAndPriceCalculation.SetPriceForPromoProducts(context, promo);

                bool isOnInvoice = promo.IsOnInvoice;
                //Подбор baseline производится по датам ПРОВЕДЕНИЯ промо независимо от типа промо (on/off-invoice)
                message = BaselineAndPriceCalculation.CalculateBaselineQtyAndLSV(promo, context, promo.StartDate, promo.EndDate, isOnInvoice);
            }
            else
            {
                message = string.Format("Promo with Id = {0} was not found", promoId);
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
            List<Product> products = null;
            List<Product> filteredProductList = new List<Product>();
            error = null;

            try
            {
                Promo promo = context.Set<Promo>().Where(x => x.Id == promoId && !x.Disabled).FirstOrDefault();

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

                //products = context.Set<Product>()
                //    .Include(f => f.PromoProducts)
                //    .Include(f => f.IncrementalPromoes)
                //    .Where(x => !x.Disabled)
                //    .ToList();
                //productsCopy = products.ToList();
                //foreach (ProductTree productTree in productTreeArray)
                //{
                //    var stringFilter = productTree.Filter;
                //    // можно и на 0 проверить, но вдруг будет пустой фильтр вида "{}"
                //    if (stringFilter.Length < 2)
                //        throw new Exception("Filter for product " + productTree.FullPathName + " is empty");

                //    // Преобразование строки фильтра в соответствующий класс
                //    FilterNode filter = stringFilter.ConvertToNode();

                //    // Создание функции фильтрации на основе построенного фильтра
                //    var expr = filter.ToExpressionTree<Product>();

                //    // Список продуктов, подходящих по параметрам фильтрации
                //    products = products.Where(expr.Compile()).ToList();

                //    filteredProductList = filteredProductList.Union(products).ToList();
                //    products = productsCopy;
                //}
                foreach (ProductTree productTree in productTreeArray)
                {
                    products = context.Set<Product>().SqlQuery(productTree.FilterQuery).ToList();
                    filteredProductList = filteredProductList.Union(products).ToList();
                }

                if (filteredProductList.Count == 0)
                {
                    throw new Exception("No suitable products were found for the current PROMO");
                }
                foreach (Product product in filteredProductList)
                {
                    context.Entry(product).Collection(p => p.PromoProducts).Load();
                    context.Entry(product).Collection(p => p.IncrementalPromoes).Load();
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
            // Отфильтрованные записи из таблицы AssortimentMatrix.
            List<string> eanPCList = context.Set<AssortmentMatrix>()
                .Where(x => !x.Disabled && x.ClientTreeId == promo.ClientTreeKeyId && promo.DispatchesStart >= x.StartDate && promo.DispatchesStart <= x.EndDate)
                .Select(x => x.Product.EAN_PC)
                .ToList();

            return eanPCList;
        }

        public static List<string> GetProductListFromAssortmentMatrix(DatabaseContext context, int clientTreeKeyId, DateTimeOffset dispatchesStart)
        {
            // Отфильтрованные записи из таблицы AssortimentMatrix.
            List<string> eanPCList = context.Set<AssortmentMatrix>()
                .Where(x => !x.Disabled && x.ClientTreeId == clientTreeKeyId && dispatchesStart >= x.StartDate && dispatchesStart <= x.EndDate)
                .Select(x => x.Product.EAN_PC)
                .ToList();

            return eanPCList;
        }

        /// <summary>
        /// Сбросить значения для продуктов
        /// </summary>
        /// <param name="promoProducts">Список продуктов</param>
        /// <param name="context">Контекст БД</param>
        private static void ResetProductParams(List<PromoProduct> promoProducts, DatabaseContext context)
        {
            foreach (PromoProduct promoProduct in promoProducts)
            {
                //promoProduct.PlanProductBaselineLSV = null;
                //promoProduct.PlanProductBaselineCaseQty = null;
                //promoProduct.ProductBaselinePrice = null;
                promoProduct.PlanProductPCPrice = null;
                promoProduct.PlanProductIncrementalCaseQty = null;
                promoProduct.PlanProductCaseQty = null;
                promoProduct.PlanProductPCQty = null;
                promoProduct.PlanProductCaseLSV = null;
                promoProduct.PlanProductPCLSV = null;
                //promoProduct.PlanProductUpliftPercent = null;
                promoProduct.PlanProductPostPromoEffectQtyW1 = null;
                promoProduct.PlanProductPostPromoEffectQtyW2 = null;
                promoProduct.PlanProductPostPromoEffectQty = null;
                promoProduct.PlanProductPostPromoEffectLSVW1 = null;
                promoProduct.PlanProductPostPromoEffectLSVW2 = null;
                promoProduct.PlanProductPostPromoEffectLSV = null;
                promoProduct.PlanProductPostPromoEffectVolumeW1 = null;
                promoProduct.PlanProductPostPromoEffectVolumeW2 = null;
                promoProduct.PlanProductPostPromoEffectVolume = null;

                if (promoProduct.PromoProductPriceIncreases.Any())
                {
                    //promoProduct.PlanProductBaselineLSV = null;
                    //promoProduct.PlanProductBaselineCaseQty = null;
                    //promoProduct.ProductBaselinePrice = null;
                    promoProduct.PromoProductPriceIncreases.FirstOrDefault().PlanProductPCPrice = null;
                    promoProduct.PromoProductPriceIncreases.FirstOrDefault().PlanProductIncrementalCaseQty = null;
                    promoProduct.PromoProductPriceIncreases.FirstOrDefault().PlanProductCaseQty = null;
                    promoProduct.PromoProductPriceIncreases.FirstOrDefault().PlanProductPCQty = null;
                    promoProduct.PromoProductPriceIncreases.FirstOrDefault().PlanProductCaseLSV = null;
                    promoProduct.PromoProductPriceIncreases.FirstOrDefault().PlanProductPCLSV = null;
                    //promoProduct.PlanProductUpliftPercent = null;
                    promoProduct.PromoProductPriceIncreases.FirstOrDefault().PlanProductPostPromoEffectQtyW1 = null;
                    promoProduct.PromoProductPriceIncreases.FirstOrDefault().PlanProductPostPromoEffectQtyW2 = null;
                    promoProduct.PromoProductPriceIncreases.FirstOrDefault().PlanProductPostPromoEffectQty = null;
                    promoProduct.PromoProductPriceIncreases.FirstOrDefault().PlanProductPostPromoEffectLSVW1 = null;
                    promoProduct.PromoProductPriceIncreases.FirstOrDefault().PlanProductPostPromoEffectLSVW2 = null;
                    promoProduct.PromoProductPriceIncreases.FirstOrDefault().PlanProductPostPromoEffectLSV = null;
                    promoProduct.PromoProductPriceIncreases.FirstOrDefault().PlanProductPostPromoEffectVolumeW1 = null;
                    promoProduct.PromoProductPriceIncreases.FirstOrDefault().PlanProductPostPromoEffectVolumeW2 = null;
                    promoProduct.PromoProductPriceIncreases.FirstOrDefault().PlanProductPostPromoEffectVolume = null;
                }
            }
        }

        public static List<Product> GetCheckedProducts(DatabaseContext context, Promo promo, IQueryable<Product> productQuery = null)
        {
            List<Product> products = new List<Product>();
            if (productQuery == null)
            {
                productQuery = context.Set<Product>().Where(x => !x.Disabled);
            }

            if (!string.IsNullOrEmpty(promo.InOutProductIds))
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
        public static void FillPriceIncreaseProdusts(Promo promo, List<PromoProduct> promoProducts)
        {
            promo.PromoPriceIncrease.PromoProductPriceIncreases = new List<PromoProductPriceIncrease>();
            foreach (PromoProduct promoProduct in promoProducts)
            {
                PromoProductPriceIncrease promoProductPriceIncrease = new PromoProductPriceIncrease
                {
                    PromoProduct = promoProduct,
                    ZREP = promoProduct.ZREP,
                    EAN_Case = promoProduct.EAN_Case,
                    EAN_PC = promoProduct.EAN_PC,
                    ProductEN = promoProduct.ProductEN
                };
                if (promoProduct.PromoProductsCorrections != null)
                {
                    promoProductPriceIncrease.ProductCorrectionPriceIncreases = new List<PromoProductCorrectionPriceIncrease>();
                    if (promoProduct.PromoProductsCorrections.Any(f => !f.Disabled))
                    {
                        PromoProductCorrectionPriceIncrease promoProductCorrectionPriceIncrease = new PromoProductCorrectionPriceIncrease
                        {
                            PlanProductUpliftPercentCorrected = promoProduct.PromoProductsCorrections.FirstOrDefault(f => !f.Disabled).PlanProductUpliftPercentCorrected,
                            TempId = promoProduct.PromoProductsCorrections.FirstOrDefault().TempId,
                            UserId = promoProduct.PromoProductsCorrections.FirstOrDefault().UserId,
                            UserName = promoProduct.PromoProductsCorrections.FirstOrDefault().UserName,
                            CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)
                        };
                        promoProductPriceIncrease.ProductCorrectionPriceIncreases.Add(promoProductCorrectionPriceIncrease);
                    }
                }
                promo.PromoPriceIncrease.PromoProductPriceIncreases.Add(promoProductPriceIncrease);
            }
        }
        public static void CopyPriceIncreaseProdusts(Promo promo, List<PromoProduct> promoProducts, DatabaseContext Context)
        {
            foreach (PromoProductPriceIncrease pppi in promo.PromoPriceIncrease.PromoProductPriceIncreases.ToList())
            {
                promo.PromoPriceIncrease.PromoProductPriceIncreases.Remove(pppi);
                Context.Set<PromoProductPriceIncrease>().Remove(pppi);
            }
            //promo.PromoPriceIncrease.PromoProductPriceIncreases = new List<PromoProductPriceIncrease>();
            foreach (PromoProduct promoProduct in promoProducts)
            {
                PromoProductPriceIncrease promoProductPriceIncrease = new PromoProductPriceIncrease
                {
                    PromoProduct = promoProduct,
                    ZREP = promoProduct.ZREP,
                    EAN_Case = promoProduct.EAN_Case,
                    EAN_PC = promoProduct.EAN_PC,
                    ProductEN = promoProduct.ProductEN
                };
                if (promoProduct.PromoProductsCorrections != null)
                {
                    promoProductPriceIncrease.ProductCorrectionPriceIncreases = new List<PromoProductCorrectionPriceIncrease>();
                    if (promoProduct.PromoProductsCorrections.Any(f => !f.Disabled))
                    {
                        PromoProductCorrectionPriceIncrease promoProductCorrectionPriceIncrease = new PromoProductCorrectionPriceIncrease
                        {
                            PlanProductUpliftPercentCorrected = promoProduct.PromoProductsCorrections.FirstOrDefault(f => !f.Disabled).PlanProductUpliftPercentCorrected,
                            TempId = promoProduct.PromoProductsCorrections.FirstOrDefault().TempId,
                            UserId = promoProduct.PromoProductsCorrections.FirstOrDefault().UserId,
                            UserName = promoProduct.PromoProductsCorrections.FirstOrDefault().UserName,
                            CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)
                        };
                        promoProductPriceIncrease.ProductCorrectionPriceIncreases.Add(promoProductCorrectionPriceIncrease);
                    }
                }
                promo.PromoPriceIncrease.PromoProductPriceIncreases.Add(promoProductPriceIncrease);
            }
        }
    }
}