using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.CalculatePromoParametersModule
{
    public class ActualPromoParametersCalculation
    {
        /// <summary>
        /// Расчитать фактические параметры для промо
        /// </summary>
        /// <param name="promo">Промо</param>
        /// <param name="context">Контекст БД</param>
        /// <returns>Null при успешном расчете, иначе строку с ошибками</returns>
        public static string CalculatePromoParameters(Promo promo, DatabaseContext context)
        {
            // подготовительная часть, проверяем все ли данные имеются
            string errors = "";

            if (!promo.PlanPromoBaselineLSV.HasValue)
                errors += "For promo №" + promo.Number + " is no Plan Promo Baseline LSV value;";

            if (!promo.ActualPromoLSV.HasValue)
                errors += "For promo №" + promo.Number + " is no Actual Promo LSV value;";

            if (!promo.ActualPromoXSites.HasValue || !promo.ActualPromoCatalogue.HasValue || !promo.ActualPromoBranding.HasValue
                || !promo.ActualPromoBTL.HasValue || !promo.ActualPromoCostProdXSites.HasValue || !promo.ActualPromoCostProdCatalogue.HasValue)
            {
                errors += "For promo №" + promo.Number + " is no Budget values;";
            }

            // ищем TI Base
            string message = null;
            double? TIBasePercent = PlanPromoParametersCalculation.GetTIBasePercent(promo, context, out message);
            if (message != null)
                errors += message + ";";

            // ищем TI COGS
            promo.PlanPromoIncrementalBaseTI = promo.PlanPromoIncrementalLSV * TIBasePercent / 100;
            double? COGSPercent = PlanPromoParametersCalculation.GetCOGSPercent(promo, context, out message);
            if (message != null)
                errors += message + ";";

            // обращение к БД в try-catch, всё не нужно и производительнее будет
            ClientTree clientTree = null;
            try
            {
                clientTree = context.Set<ClientTree>().Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue).FirstOrDefault();
                if (clientTree == null)
                    errors += "For promo №" + promo.Number + " client not found;";
            }
            catch (Exception e)
            {
                errors += e.Message + ";";
            }

            // если ошибок нет - считаем
            if (errors.Length == 0)
            {
                promo.ActualPromoBaselineLSV = promo.PlanPromoBaselineLSV;
                promo.ActualPromoIncrementalLSV = promo.ActualPromoLSV - promo.ActualPromoBaselineLSV;
                promo.ActualPromoUpliftPercent = (int)(promo.ActualPromoIncrementalLSV / promo.ActualPromoBaselineLSV) * 100;
                promo.ActualPromoTIShopper = promo.ActualPromoLSV * promo.MarsMechanicDiscount / 100;
                promo.ActualPromoCost = promo.ActualPromoTIShopper + promo.ActualPromoTIMarketing + promo.ActualPromoBranding + promo.ActualPromoBTL + promo.ActualPromoCostProduction;
                promo.ActualPromoIncrementalBaseTI = promo.ActualPromoIncrementalLSV * TIBasePercent / 100;
                promo.ActualPromoIncrementalCOGS = promo.ActualPromoIncrementalLSV * COGSPercent / 100;
                promo.ActualPromoTotalCost = promo.ActualPromoCost + promo.ActualPromoIncrementalBaseTI + promo.ActualPromoIncrementalCOGS;
                promo.FactPostPromoEffectW1 = promo.ActualPromoTotalCost * clientTree.PostPromoEffectW1 / 100;
                promo.FactPostPromoEffectW2 = promo.ActualPromoTotalCost * clientTree.PostPromoEffectW2 / 100;
                promo.ActualPromoNetIncrementalLSV = promo.ActualPromoIncrementalLSV - promo.FactPostPromoEffectW1 - promo.FactPostPromoEffectW2;
                promo.ActualPromoNetLSV = promo.ActualPromoBaselineLSV + promo.ActualPromoNetIncrementalLSV;
                promo.ActualPromoIncrementalNSV = promo.ActualPromoNetLSV - promo.ActualPromoTIShopper - promo.ActualPromoTIMarketing - promo.ActualPromoIncrementalBaseTI;
                promo.ActualPromoNetIncrementalNSV = promo.ActualPromoNetIncrementalLSV - promo.ActualPromoTIShopper - promo.ActualPromoTIMarketing - promo.ActualPromoIncrementalBaseTI;
                promo.ActualPromoIncrementalMAC = promo.ActualPromoIncrementalNSV - promo.ActualPromoIncrementalCOGS;
                promo.ActualPromoNetIncrementalMAC = promo.ActualPromoNetIncrementalNSV - promo.ActualPromoIncrementalCOGS;
                promo.ActualPromoIncrementalEarnings = promo.ActualPromoIncrementalMAC - promo.ActualPromoBranding - promo.ActualPromoBTL - promo.ActualPromoCostProduction;
                promo.ActualPromoNetIncrementalEarnings = promo.ActualPromoNetIncrementalMAC - promo.ActualPromoBranding - promo.ActualPromoBTL - promo.ActualPromoCostProduction;
                promo.ActualPromoROIPercent = (int)(promo.ActualPromoIncrementalEarnings / promo.ActualPromoTotalCost + 1) * 100;
                promo.ActualPromoNetROIPercent = (int)(promo.ActualPromoNetIncrementalEarnings / promo.ActualPromoTotalCost);
                promo.ActualPromoNetUpliftPercent = (int)(promo.ActualPromoNetIncrementalLSV / promo.ActualPromoBaselineLSV) * 100;

                try
                {
                    context.SaveChanges();
                }
                catch (Exception e)                
                {
                    errors += e.Message + ";";
                }
            }

            return errors.Length == 0 ? null : errors;
        }

        /// <summary>
        /// Сбросить значения фактических полей
        /// </summary>
        /// <param name="promo">Промо</param>
        /// <param name="context">Контекст БД</param>
        public static void ResetValues(Promo promo, DatabaseContext context)
        {
            promo.ActualPromoBaselineLSV = null;
            promo.ActualPromoIncrementalLSV = null;
            promo.ActualPromoUpliftPercent = null;
            promo.ActualPromoTIShopper = null;
            promo.ActualPromoCost = null;
            promo.ActualPromoIncrementalBaseTI = null;
            promo.ActualPromoIncrementalCOGS = null;
            promo.ActualPromoTotalCost = null;
            promo.FactPostPromoEffectW1 = null;
            promo.FactPostPromoEffectW2 = null;
            promo.ActualPromoNetIncrementalLSV = null;
            promo.ActualPromoNetLSV = null;
            promo.ActualPromoIncrementalNSV = null;
            promo.ActualPromoNetIncrementalNSV = null;
            promo.ActualPromoIncrementalMAC = null;
            promo.ActualPromoNetIncrementalMAC = null;
            promo.ActualPromoIncrementalEarnings = null;
            promo.ActualPromoNetIncrementalEarnings = null;
            promo.ActualPromoROIPercent = null;
            promo.ActualPromoNetROIPercent = null;
            promo.ActualPromoNetUpliftPercent = null;
            context.SaveChanges();
        }
    }
}
