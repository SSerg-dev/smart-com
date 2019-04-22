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
        /// <returns>Null при успешном расчете, иначе строку с ошибками</returns>
        public static string CalculatePromoProductParameters(Promo promo, DatabaseContext context)
        {
            PromoProduct[] products = context.Set<PromoProduct>().Where(n => n.PromoId == promo.Id && !n.Disabled).ToArray();
            ResetProductParameters(products);

            double actualPromoLSV = 0;
            string errors = "";
            foreach (PromoProduct produt in products)
            {
                if (!produt.Product.UOM_PC2Case.HasValue)
                    errors += "For product with EAN:" + produt.EAN + " is no UOM_PC2Case value;";

                if (!produt.PlanProductQty.HasValue)
                    errors += "For product with EAN:" + produt.EAN + " is no Plan Product Qty value;";

                if (!produt.ActualProductPCQty.HasValue)
                    errors += "For product with EAN:" + produt.EAN + " is no Actual ProductPC Qty value;";

                if (!produt.ActualProductPCLSV.HasValue)
                    errors += "For product with EAN:" + produt.EAN + " is no Actual Product PC LSV value;";

                if (errors.Length == 0)
                {
                    produt.ActualProductLSV = produt.ActualProductPCLSV / produt.Product.UOM_PC2Case;
                    produt.ActualProductIncrementalPCQty = produt.ActualProductPCQty - produt.PlanProductPCQty;
                    produt.ActualProductIncrementalPCLSV = produt.ActualProductPCLSV - produt.PlanProductPCLSV;
                    produt.ActualProductIncrementalLSV = produt.ActualProductLSV - produt.PlanProductLSV;
                    produt.ActualProductUplift = (produt.ActualProductIncrementalPCQty / produt.PlanProductPCQty) * 100;

                    actualPromoLSV += produt.ActualProductLSV.Value;
                }
            }

            // если без ошибок устанавливаем суммы продаж для промо иначе сбрасываем
            promo.ActualPromoLSV = errors.Length == 0 ? actualPromoLSV : new double?();

            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                errors += e.Message + ";";
            }

            return errors.Length == 0 ? null : errors;
        }

        /// <summary>
        /// Сбросить фактические значения для продуктов
        /// </summary>
        /// <param name="products">Список продуктов</param>
        private static void ResetProductParameters(PromoProduct[] products)
        {
            foreach (PromoProduct product in products)
            {
                product.ActualProductLSV = null;
                product.ActualProductIncrementalPCQty = null;
                product.ActualProductIncrementalPCLSV = null;
                product.ActualProductIncrementalLSV = null;
                product.ActualProductUplift = null;
            }
        }
    }
}
