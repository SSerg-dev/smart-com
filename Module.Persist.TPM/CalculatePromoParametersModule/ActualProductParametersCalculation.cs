using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.CalculatePromoParametersModule {
    public class ActualProductParametersCalculation {
        /// <summary>
        /// Расчитать фактические параметры для продуктов, связанных с промо
        /// </summary>
        /// <param name="promo">Промо</param>
        /// <param name="context">Контекст БД</param>
        /// <param name="lockedActualLSV">Блокировка значений, введенных Demand'ом</param>
        /// <returns>Null при успешном расчете, иначе строку с ошибками</returns>
        public static string CalculatePromoProductParameters(Promo promo, DatabaseContext context, bool lockedActualLSV = false) {
            PromoProduct[] products = context.Set<PromoProduct>().Where(n => n.PromoId == promo.Id && !n.Disabled).ToArray();
            ClientTree clientTree = context.Set<ClientTree>().Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue).FirstOrDefault();

            ResetProductParameters(products, lockedActualLSV);

            double? ActualPromoLSVByCompensation = 0;
            string errors = ""; // общий список ошибок
            foreach (PromoProduct product in products) {
                string errorsForProduct = ""; // ошибки для конкретного ZREP

                if (!product.Product.UOM_PC2Case.HasValue)
                    errorsForProduct += Log.GenerateLogLine(Log.MessageType["Warning"], "For product with EAN Case:") + product.EAN_Case + " is no UOM_PC2Case value;";

                if (!product.PlanProductPCQty.HasValue)
                    errorsForProduct += Log.GenerateLogLine(Log.MessageType["Warning"], "For product with EAN Case:") + product.EAN_Case + " is no Plan Product PC Qty value;";

                if (!product.ActualProductPCQty.HasValue)
                    errorsForProduct += Log.GenerateLogLine(Log.MessageType["Warning"], "For product with EAN Case:") + product.EAN_Case + " is no Actual ProductPC Qty value;";
                
                if (errorsForProduct.Length == 0) {

                    // если значения введены вручную через грид ActualLSV, то ненужно обновлять
                    if (!lockedActualLSV)
                    {
                        product.ActualProductBaselineLSV = product.PlanProductBaselineLSV;
                        product.ActualProductLSV = 0;// product.Product.UOM_PC2Case != 0 ? product.ActualProductPCLSV / product.Product.UOM_PC2Case : 0;
                    }

                    product.ActualProductCaseQty = product.ActualProductPCQty / product.Product.UOM_PC2Case;
                    product.ActualProductSellInPrice = product.PlanProductPCPrice;

                    //удалять? 17/06/19
                    //все -таки не надо удалять 20/06/19
                    product.ActualProductPCLSV = product.ActualProductPCQty * product.ActualProductSellInPrice;// * (product.ActualProductShelfDiscount / 100);
                        

                    // Plan Product Baseline PC Qty = Plan Product Baseline Case Qty * UOM_PC2Case
                    double? planProductBaselinePCQty = product.PlanProductBaselineCaseQty * product.Product.UOM_PC2Case;
                    product.ActualProductIncrementalPCQty = product.ActualProductPCQty - planProductBaselinePCQty;

                    // что значит "показатель недействителен по формуле" в спеке?
                    double? planProductBaselinePCLSV = product.Product.UOM_PC2Case != 0 ? (product.PlanProductBaselineLSV / product.Product.UOM_PC2Case) : 0;
                    product.ActualProductIncrementalPCLSV = product.ActualProductPCLSV - planProductBaselinePCLSV;

                    product.ActualProductUplift = planProductBaselinePCQty != 0 ? (product.ActualProductIncrementalPCQty / planProductBaselinePCQty) * 100 : 0;

                    // если стоит флаг inout, ActualPromoPostPromoEffect = 0
                    if (!promo.InOut.HasValue || !promo.InOut.Value)
                    {
                        if (clientTree != null)
                        {
                            //TODO: Уточнить насчет деления на 100
                            product.ActualProductPostPromoEffectQtyW1 = product.PlanProductBaselineCaseQty * clientTree.PostPromoEffectW1 / 100;
                            product.ActualProductPostPromoEffectQtyW2 = product.PlanProductBaselineCaseQty * clientTree.PostPromoEffectW2 / 100;
                            product.ActualProductPostPromoEffectQty = product.PlanProductPostPromoEffectQtyW1 + product.PlanProductPostPromoEffectQtyW2;

                            product.ActualProductPostPromoEffectLSVW1 = product.PlanProductBaselineLSV * clientTree.PostPromoEffectW1 / 100;
                            product.ActualProductPostPromoEffectLSVW2 = product.PlanProductBaselineLSV * clientTree.PostPromoEffectW2 / 100;
                            product.ActualProductPostPromoEffectLSV = product.PlanProductPostPromoEffectLSVW1 + product.PlanProductPostPromoEffectLSVW2;

                            product.ActualProductLSVByCompensation = product.ActualProductPCQty * product.PlanProductPCPrice;
                            product.ActualProductIncrementalLSV = product.ActualProductLSVByCompensation - product.PlanProductBaselineLSV;
                        }
                    }
                    else
                    {
                        product.ActualProductPostPromoEffectQtyW1 = 0;
                        product.ActualProductPostPromoEffectQtyW2 = 0;
                        product.ActualProductPostPromoEffectQty = 0;

                        product.ActualProductPostPromoEffectLSVW1 = 0;
                        product.ActualProductPostPromoEffectLSVW2 = 0;
                        product.ActualProductPostPromoEffectLSV = 0;

                        product.ActualProductLSVByCompensation = product.ActualProductPCQty * product.PlanProductPCPrice;
                        product.ActualProductIncrementalLSV = product.ActualProductLSVByCompensation - product.PlanProductBaselineLSV;
                    }

                    ActualPromoLSVByCompensation += (product.ActualProductPCQty * product.PlanProductPCPrice);
                } else {
                    errors += errorsForProduct;
                }
            }

            promo.ActualPromoLSVByCompensation = ActualPromoLSVByCompensation != 0 ? ActualPromoLSVByCompensation : new double?();

            try {
                context.SaveChanges();
            } catch (Exception e) {
                errors += e.Message + ";";
            }

            return errors.Length == 0 ? null : errors;
        }

        /// <summary>
        /// Сбросить фактические значения для продуктов
        /// </summary>
        /// <param name="products">Список продуктов</param>
        private static void ResetProductParameters(PromoProduct[] products, bool lockedActualLSV) {
            foreach (PromoProduct product in products) {
                if(!lockedActualLSV)
                    product.ActualProductLSV = null;

                product.ActualProductIncrementalPCQty = null;
                product.ActualProductIncrementalPCLSV = null;
                product.ActualProductIncrementalLSV = null;
                product.ActualProductUplift = null;
                product.ActualProductLSVByCompensation = null;
            }
        }
    }
}
