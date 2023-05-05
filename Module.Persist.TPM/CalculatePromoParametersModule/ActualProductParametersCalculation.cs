using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.CalculatePromoParametersModule
{
    public class ActualProductParametersCalculation
    {
        /// <summary>
        /// Расчитать фактические параметры для продуктов, связанных с промо
        /// </summary>
        /// <param name="promo">Промо</param>
        /// <param name="context">Контекст БД</param>
        /// <param name="lockedActualLSV">Блокировка значений, введенных Demand'ом</param>
        /// <returns>Null при успешном расчете, иначе строку с ошибками</returns>
        public static string CalculatePromoProductParameters(Promo promo, DatabaseContext context, bool lockedActualLSV = false, bool isSupportAdmin = false, bool needToSaveChanges = true)
        {
            if (promo != null && (promo.PromoStatus.SystemName == "Finished" || (isSupportAdmin && promo.PromoStatus.SystemName == "Closed")))
            {
                List<PromoProduct> promoProducts = context.Set<PromoProduct>().Where(n => n.PromoId == promo.Id && !n.Disabled).ToList();
                ClientTree clientTree = context.Set<ClientTree>().Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue).FirstOrDefault();

                PromoStatus finishedStatus = context.Set<PromoStatus>().Where(x => x.SystemName.ToLower() == "finished" && !x.Disabled).FirstOrDefault();
                bool isActualPromoBaseLineLSVChangedByDemand = promo.PromoStatusId == finishedStatus.Id
                                                            && promo.ActualPromoBaselineLSV != null
                                                            && promo.ActualPromoBaselineLSV != promo.PlanPromoBaselineLSV;
                bool isActualPromoLSVChangedByDemand = promo.PromoStatusId == finishedStatus.Id
                                                    && promo.ActualPromoLSVSO != null
                                                    && promo.ActualPromoLSVSO != 0;
                bool isActualPromoProstPromoEffectLSVChangedByDemand = promo.PromoStatusId == finishedStatus.Id
                                                                    && promo.ActualPromoPostPromoEffectLSV != null
                                                                    && promo.ActualPromoPostPromoEffectLSV != 0;

                ResetProductParameters(promoProducts, context, !isActualPromoProstPromoEffectLSVChangedByDemand);

                double? ActualPromoLSVByCompensation = 0;
                string errors = ""; // общий список ошибок
                foreach (PromoProduct promoProduct in promoProducts)
                {
                    string errorsForProduct = ""; // ошибки для конкретного ZREP

                    if (!promoProduct.Product.UOM_PC2Case.HasValue)
                        errorsForProduct += Log.GenerateLogLine(Log.MessageType["Warning"], "For product with EAN Case:") + promoProduct.EAN_Case + " is no UOM_PC2Case value;";

                    if (!isActualPromoBaseLineLSVChangedByDemand && (!promo.InOut.HasValue || !promo.InOut.Value))
                    {
                        promoProduct.ActualProductBaselineLSV = promoProduct.PlanProductBaselineLSV;
                    }

                    if (errorsForProduct.Length == 0)
                    {
                        double? actualProductPCPrice = 0;

                        if (!promoProduct.Price.HasValue || (promoProduct.Price.HasValue && promoProduct.Price.Value == 0))
                        {
                            var priceLists = context.Set<PriceList>().Where(x => !x.Disabled && x.StartDate <= promo.DispatchesStart && x.EndDate >= promo.DispatchesStart && x.ClientTreeId == promo.ClientTreeKeyId);
                            var priceList = priceLists.Where(x => x.ProductId == promoProduct.ProductId).OrderByDescending(x => x.StartDate).FirstOrDefault();
                            var incrementalPromo = context.Set<IncrementalPromo>().Where(x => !x.Disabled && x.PromoId == promo.Id
                                                                                  && x.ProductId == promoProduct.ProductId).FirstOrDefault();
                            if (priceList != null)
                            {
                                promoProduct.Price = priceList.Price;
                                if (incrementalPromo != null) incrementalPromo.CasePrice = priceList.Price;
                            }
                            else
                            {
                                promoProduct.Price = null;
                                if (incrementalPromo != null) incrementalPromo.CasePrice = null;
                            }
                        }

                        if (!promo.InOut.HasValue || !promo.InOut.Value)
                        {
                            if (promoProduct.Product.UOM_PC2Case != 0)
                            {
                                var price = promoProduct.Price;
                                actualProductPCPrice = promoProduct.Price / promoProduct.Product.UOM_PC2Case;
                            }
                        }
                        else
                        {
                            var incrementalPromo = context.Set<IncrementalPromo>().Where(x => x.PromoId == promo.Id && x.ProductId == promoProduct.ProductId && !x.Disabled).FirstOrDefault();
                            if (incrementalPromo != null && promoProduct.Product.UOM_PC2Case != 0)
                            {
                                var price = promoProduct.Price;
                                actualProductPCPrice = price / promoProduct.Product.UOM_PC2Case;
                            }
                        }

                        promoProduct.ActualProductCaseQty = promoProduct.Product.UOM_PC2Case != 0 ? (promoProduct.ActualProductPCQty ?? 0) / promoProduct.Product.UOM_PC2Case : 0;
                        promoProduct.ActualProductSellInPrice = actualProductPCPrice;
                        promoProduct.ActualProductBaselineCaseQty = (promoProduct.Price != 0 && promoProduct.Price != null) ?
                                                                promoProduct.ActualProductBaselineLSV / promoProduct.Price : 0;

                        //удалять? 17/06/19
                        //все -таки не надо удалять 20/06/19
                        promoProduct.ActualProductPCLSV = (promoProduct.ActualProductPCQty * promoProduct.ActualProductSellInPrice) ?? 0;// * (product.ActualProductShelfDiscount / 100);

                        if (promo.IsOnInvoice)
                        {
                            promoProduct.ActualProductLSV = promoProduct.ActualProductPCLSV;
                        }
                        else if (!isActualPromoLSVChangedByDemand)
                        {
                            promoProduct.ActualProductLSV = 0;
                        }

                        // Plan Product Baseline PC Qty = Plan Product Baseline Case Qty * UOM_PC2Case
                        double? planProductBaselinePCQty = promoProduct.PlanProductBaselineCaseQty * promoProduct.Product.UOM_PC2Case;

                        // что значит "показатель недействителен по формуле" в спеке?
                        double? planProductBaselinePCLSV = promoProduct.Product.UOM_PC2Case != 0 ? (promoProduct.PlanProductBaselineLSV / promoProduct.Product.UOM_PC2Case) : 0;
                        promoProduct.ActualProductIncrementalLSV = (promoProduct.ActualProductLSV ?? 0) - (promoProduct.ActualProductBaselineLSV ?? 0);

                        // если стоит флаг inout, ActualPromoPostPromoEffect = 0
                        if (!promo.InOut.HasValue || !promo.InOut.Value)
                        {
                            promoProduct.ActualProductUpliftPercent = promoProduct.ActualProductBaselineLSV != 0 ? (promoProduct.ActualProductIncrementalLSV / promoProduct.ActualProductBaselineLSV) * 100 : 0;
                            if (clientTree != null)
                            {
                                promoProduct.ActualProductPostPromoEffectQtyW1 = promoProduct.PlanProductBaselineCaseQty * clientTree.PostPromoEffectW1 / 100;
                                promoProduct.ActualProductPostPromoEffectQtyW2 = promoProduct.PlanProductBaselineCaseQty * clientTree.PostPromoEffectW2 / 100;
                                promoProduct.ActualProductPostPromoEffectQty = promoProduct.PlanProductPostPromoEffectQtyW1 + promoProduct.PlanProductPostPromoEffectQtyW2;
                            }

                            promoProduct.ActualProductLSVByCompensation = (promoProduct.ActualProductPCQty * actualProductPCPrice) ?? 0;
                        }
                        else
                        {
                            promoProduct.ActualProductUpliftPercent = null;

                            promoProduct.ActualProductPostPromoEffectQtyW1 = 0;
                            promoProduct.ActualProductPostPromoEffectQtyW2 = 0;
                            promoProduct.ActualProductPostPromoEffectQty = 0;

                            if (!isActualPromoProstPromoEffectLSVChangedByDemand)
                            {
                                promoProduct.ActualProductPostPromoEffectLSV = 0;
                            }

                            promoProduct.ActualProductLSVByCompensation = (promoProduct.ActualProductPCQty * actualProductPCPrice) ?? 0;
                        }

                        ActualPromoLSVByCompensation += (promoProduct.ActualProductPCQty * actualProductPCPrice) ?? 0;
                        promoProduct.ActualProductIncrementalPCQty = promoProduct.ActualProductSellInPrice != 0 ? promoProduct.ActualProductIncrementalLSV / promoProduct.ActualProductSellInPrice : 0;
                        promoProduct.ActualProductIncrementalPCLSV = promoProduct.Product.UOM_PC2Case != 0 ? promoProduct.ActualProductIncrementalLSV / promoProduct.Product.UOM_PC2Case : 0;
                        promoProduct.ActualProductQtySO = (promoProduct.ActualProductLSV  / actualProductPCPrice) ?? 0;
                    }
                    else
                    {
                        errors += errorsForProduct;
                    }
                }

                if (ActualPromoLSVByCompensation == 0)
                {
                    promo.ActualPromoLSVByCompensation = null;
                    promo.ActualPromoLSVSI = null;
                    promo.ActualPromoVolumeByCompensation = null;
                }
                else
                {
                    promo.ActualPromoLSVByCompensation = ActualPromoLSVByCompensation;
                    promo.ActualPromoLSVSI = ActualPromoLSVByCompensation;
                    //volume
                    if (!promo.InOut.HasValue || !promo.InOut.Value)
                    {
                        promo.ActualPromoVolumeByCompensation = promoProducts.Sum(g => g.ActualProductPCQty * g.Product.PCVolume);
                        promo.ActualPromoVolumeSI = promo.ActualPromoVolumeByCompensation;
                    }
                    else
                    {
                        promo.ActualPromoVolumeByCompensation = promoProducts.Sum(g => g.ActualProductLSVByCompensation);
                        promo.ActualPromoVolumeSI = 0;
                                               
                    }
                }

                try
                {
                    if (needToSaveChanges)
                    {
                        context.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    errors += e.Message + ";";
                }

                return errors.Length == 0 ? null : errors;
            }

            return null;
        }

        /// <summary>
        /// Сбросить фактические значения для продуктов
        /// </summary>
        /// <param name="promoProducts">Список продуктов</param>
        private static void ResetProductParameters(List<PromoProduct> promoProducts, DatabaseContext context, bool resetActualProductPostPromoEffectLSV)
        {
            foreach (PromoProduct product in promoProducts)
            {
                if (resetActualProductPostPromoEffectLSV)
                {
                    product.ActualProductPostPromoEffectLSV = null;
                }

                product.ActualProductIncrementalPCQty = null;
                product.ActualProductIncrementalPCLSV = null;
                product.ActualProductIncrementalLSV = null;
                product.ActualProductUpliftPercent = null;
                product.ActualProductLSVByCompensation = null;
            }
        }
    }
}
