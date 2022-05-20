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
                List<PromoProduct> products = context.Set<PromoProduct>().Where(n => n.PromoId == promo.Id && !n.Disabled).ToList();
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

                ResetProductParameters(products, context, !isActualPromoProstPromoEffectLSVChangedByDemand);

                double? ActualPromoLSVByCompensation = 0;
                string errors = ""; // общий список ошибок
                foreach (PromoProduct product in products)
                {
                    string errorsForProduct = ""; // ошибки для конкретного ZREP

                    if (!product.Product.UOM_PC2Case.HasValue)
                        errorsForProduct += Log.GenerateLogLine(Log.MessageType["Warning"], "For product with EAN Case:") + product.EAN_Case + " is no UOM_PC2Case value;";

                    if (!isActualPromoBaseLineLSVChangedByDemand && (!promo.InOut.HasValue || !promo.InOut.Value))
                    {
                        product.ActualProductBaselineLSV = product.PlanProductBaselineLSV;
                    }

                    if (errorsForProduct.Length == 0)
                    {
                        double? actualProductPCPrice = 0;

                        if (!product.Price.HasValue || (product.Price.HasValue && product.Price.Value == 0))
                        {
                            var priceLists = context.Set<PriceList>().Where(x => !x.Disabled && x.StartDate <= promo.DispatchesStart && x.EndDate >= promo.DispatchesStart && x.ClientTreeId == promo.ClientTreeKeyId);
                            var priceList = priceLists.Where(x => x.ProductId == product.ProductId).OrderByDescending(x => x.StartDate).FirstOrDefault();
                            var incrementalPromo = context.Set<IncrementalPromo>().Where(x => !x.Disabled && x.PromoId == promo.Id
                                                                                  && x.ProductId == product.ProductId).FirstOrDefault();
                            if (priceList != null)
                            {
                                product.Price = priceList.Price;
                                if (incrementalPromo != null) incrementalPromo.CasePrice = priceList.Price;
                            }
                            else
                            {
                                product.Price = null;
                                if (incrementalPromo != null) incrementalPromo.CasePrice = null;
                            }
                        }

                        if (!promo.InOut.HasValue || !promo.InOut.Value)
                        {
                            if (product.Product.UOM_PC2Case != 0)
                            {
                                var price = product.Price;
                                actualProductPCPrice = product.Price / product.Product.UOM_PC2Case;
                            }
                        }
                        else
                        {
                            var incrementalPromo = context.Set<IncrementalPromo>().Where(x => x.PromoId == promo.Id && x.ProductId == product.ProductId && !x.Disabled).FirstOrDefault();
                            if (incrementalPromo != null && product.Product.UOM_PC2Case != 0)
                            {
                                var price = product.Price;
                                actualProductPCPrice = price / product.Product.UOM_PC2Case;
                            }
                        }

                        product.ActualProductCaseQty = product.Product.UOM_PC2Case != 0 ? (product.ActualProductPCQty ?? 0) / product.Product.UOM_PC2Case : 0;
                        product.ActualProductSellInPrice = actualProductPCPrice;
                        product.ActualProductBaselineCaseQty = (product.Price != 0 && product.Price != null) ?
                                                                product.ActualProductBaselineLSV / product.Price : 0;

                        //удалять? 17/06/19
                        //все -таки не надо удалять 20/06/19
                        product.ActualProductPCLSV = (product.ActualProductPCQty * product.ActualProductSellInPrice) ?? 0;// * (product.ActualProductShelfDiscount / 100);

                        if (promo.IsOnInvoice)
                        {
                            product.ActualProductLSV = product.ActualProductPCLSV;
                        }
                        else if (!isActualPromoLSVChangedByDemand)
                        {
                            product.ActualProductLSV = 0;
                        }

                        // Plan Product Baseline PC Qty = Plan Product Baseline Case Qty * UOM_PC2Case
                        double? planProductBaselinePCQty = product.PlanProductBaselineCaseQty * product.Product.UOM_PC2Case;

                        // что значит "показатель недействителен по формуле" в спеке?
                        double? planProductBaselinePCLSV = product.Product.UOM_PC2Case != 0 ? (product.PlanProductBaselineLSV / product.Product.UOM_PC2Case) : 0;
                        product.ActualProductIncrementalLSV = (product.ActualProductLSV ?? 0) - (product.ActualProductBaselineLSV ?? 0);

                        // если стоит флаг inout, ActualPromoPostPromoEffect = 0
                        if (!promo.InOut.HasValue || !promo.InOut.Value)
                        {
                            product.ActualProductUpliftPercent = product.ActualProductBaselineLSV != 0 ? (product.ActualProductIncrementalLSV / product.ActualProductBaselineLSV) * 100 : 0;
                            if (clientTree != null)
                            {
                                product.ActualProductPostPromoEffectQtyW1 = product.PlanProductBaselineCaseQty * clientTree.PostPromoEffectW1 / 100;
                                product.ActualProductPostPromoEffectQtyW2 = product.PlanProductBaselineCaseQty * clientTree.PostPromoEffectW2 / 100;
                                product.ActualProductPostPromoEffectQty = product.PlanProductPostPromoEffectQtyW1 + product.PlanProductPostPromoEffectQtyW2;
                            }

                            product.ActualProductLSVByCompensation = (product.ActualProductPCQty * actualProductPCPrice) ?? 0;
                        }
                        else
                        {
                            product.ActualProductUpliftPercent = null;

                            product.ActualProductPostPromoEffectQtyW1 = 0;
                            product.ActualProductPostPromoEffectQtyW2 = 0;
                            product.ActualProductPostPromoEffectQty = 0;

                            if (!isActualPromoProstPromoEffectLSVChangedByDemand)
                            {
                                product.ActualProductPostPromoEffectLSV = 0;
                            }

                            product.ActualProductLSVByCompensation = (product.ActualProductPCQty * actualProductPCPrice) ?? 0;
                        }

                        ActualPromoLSVByCompensation += (product.ActualProductPCQty * actualProductPCPrice) ?? 0;
                        product.ActualProductIncrementalPCQty = product.ActualProductSellInPrice != 0 ? product.ActualProductIncrementalLSV / product.ActualProductSellInPrice : 0;
                        product.ActualProductIncrementalPCLSV = product.Product.UOM_PC2Case != 0 ? product.ActualProductIncrementalLSV / product.Product.UOM_PC2Case : 0;
                        product.ActualProductQtySO = product.ActualProductLSV * product.Price / product.Product.UOM_PC2Case;
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
                        promo.ActualPromoVolumeByCompensation = products.Sum(g => g.ActualProductLSVByCompensation);
                        promo.ActualPromoVolumeSI = 0;
                    }
                    else
                    {
                        promo.ActualPromoVolumeByCompensation = products.Sum(g => g.ActualProductPCQty) * products.Sum(g => g.Product.PCVolume);
                        promo.ActualPromoVolumeSI = promo.ActualPromoVolumeByCompensation;                        
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
        /// <param name="products">Список продуктов</param>
        private static void ResetProductParameters(List<PromoProduct> products, DatabaseContext context, bool resetActualProductPostPromoEffectLSV)
        {
            foreach (PromoProduct product in products)
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
