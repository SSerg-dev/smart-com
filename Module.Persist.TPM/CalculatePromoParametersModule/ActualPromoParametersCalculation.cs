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
        /// <param name="lockedActualLSV">Блокировка значений, введенных Demand'ом</param>
        /// <returns>Null при успешном расчете, иначе строку с ошибками</returns>
        public static string CalculatePromoParameters(Promo promo, DatabaseContext context, bool lockedActualLSV = false)
        {
            bool isActualPromoBaseLineLSVChangedByDemand = promo.ActualPromoBaselineLSV != promo.PlanPromoBaselineLSV;
            bool isActualPromoLSVChangedByDemand = promo.ActualPromoLSV != 0;
            bool isActualPromoProstPromoEffectLSVChangedByDemand = promo.ActualPromoPostPromoEffectLSV != promo.PlanPromoPostPromoEffectLSV;

            ResetValues(promo, context, !isActualPromoBaseLineLSVChangedByDemand, !isActualPromoProstPromoEffectLSVChangedByDemand);
            // подготовительная часть, проверяем все ли данные имеются
            string errors = "";

            if (!promo.PlanPromoBaselineLSV.HasValue)
                errors += Log.GenerateLogLine(Log.MessageType["Error"], "For promo №") + promo.Number + " is no Plan Promo Baseline LSV value. Actual parameters will not be calculated.";

            if (!promo.ActualPromoLSVByCompensation.HasValue)
                errors += Log.GenerateLogLine(Log.MessageType["Error"], "For promo №") + promo.Number + " is no Actual Promo LSV by Compensation value. Actual parameters will not be calculated.";

            // ищем TI Base
            string message = null;
            bool error;
            double? TIBasePercent = PlanPromoParametersCalculation.GetTIBasePercent(promo, context, out message, out error);
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
                    errors += Log.GenerateLogLine(Log.MessageType["Error"], "For promo №") + promo.Number + " client not found. Actual parameters will not be calculated.";
            }
            catch (Exception e)
            {
                errors += e.Message + ";";
            }

            // если ошибок нет - считаем
            if (errors.Length == 0)
            {
                // если значения введены вручную через грид ActualLSV, то ненужно обновлять
                if (!isActualPromoLSVChangedByDemand)
                {
                    promo.ActualPromoLSV = 0;
                }

                promo.ActualPromoTIShopper = promo.ActualPromoLSVByCompensation * promo.MarsMechanicDiscount;
                promo.ActualPromoCost = (promo.ActualPromoTIShopper ?? 0) + (promo.ActualPromoTIMarketing ?? 0) + (promo.ActualPromoBranding ?? 0) + (promo.ActualPromoBTL ?? 0) + (promo.ActualPromoCostProduction ?? 0);

                promo.ActualPromoBaseTI = promo.ActualPromoLSVByCompensation * TIBasePercent / 100;
                promo.ActualPromoTotalCost = (promo.ActualPromoCost ?? 0) + (promo.ActualPromoBaseTI ?? 0);

                if (!promo.InOut.HasValue || !promo.InOut.Value)
                {
                    // если значения введены вручную через грид ActualLSV, то ненужно обновлять
                    if (!isActualPromoProstPromoEffectLSVChangedByDemand)
                    {
                        promo.ActualPromoPostPromoEffectLSVW1 = promo.PlanPromoPostPromoEffectLSVW1;
                        promo.ActualPromoPostPromoEffectLSVW2 = promo.PlanPromoPostPromoEffectLSVW2;
                        promo.ActualPromoPostPromoEffectLSV = promo.PlanPromoPostPromoEffectLSV;
                    }
                    
                    if (!isActualPromoBaseLineLSVChangedByDemand)
                    {
                        promo.ActualPromoBaselineLSV = promo.PlanPromoBaselineLSV;
                    }

                    promo.ActualPromoIncrementalLSV = (promo.ActualPromoLSVByCompensation ?? 0) - (promo.ActualPromoBaselineLSV ?? 0);
                    promo.ActualPromoNetIncrementalLSV = (promo.ActualPromoIncrementalLSV ?? 0) - (promo.ActualPromoPostPromoEffectLSVW1 ?? 0) - (promo.ActualPromoPostPromoEffectLSVW2 ?? 0);

                    promo.ActualPromoUpliftPercent = promo.ActualPromoIncrementalLSV / promo.ActualPromoBaselineLSV * 100;
                    promo.ActualPromoNetUpliftPercent = promo.ActualPromoBaselineLSV == 0 ? 0 : promo.ActualPromoNetIncrementalLSV / promo.ActualPromoBaselineLSV * 100;
                }
                else
                {
                    // если значения введены вручную через грид ActualLSV, то ненужно обновлять
                    if (!isActualPromoProstPromoEffectLSVChangedByDemand)
                    {
                        promo.ActualPromoPostPromoEffectLSVW1 = 0;
                        promo.ActualPromoPostPromoEffectLSVW2 = 0;
                        promo.ActualPromoPostPromoEffectLSV = 0;
                    }

                    if (!isActualPromoBaseLineLSVChangedByDemand)
                    {
                        promo.ActualPromoBaselineLSV = 1;
                    }

                    promo.ActualPromoIncrementalLSV = (promo.ActualPromoLSVByCompensation ?? 0) - (promo.ActualPromoBaselineLSV ?? 0);
                    promo.ActualPromoNetIncrementalLSV = (promo.ActualPromoIncrementalLSV ?? 0) - (promo.ActualPromoPostPromoEffectLSVW1 ?? 0) - (promo.ActualPromoPostPromoEffectLSVW2 ?? 0);

                    promo.ActualPromoUpliftPercent = null;
                    promo.ActualPromoNetUpliftPercent = null;
                }
                
                promo.ActualPromoIncrementalBaseTI = promo.ActualPromoIncrementalLSV * TIBasePercent / 100;
                promo.ActualPromoNetIncrementalBaseTI = promo.ActualPromoNetIncrementalLSV * TIBasePercent / 100;

                promo.ActualPromoIncrementalCOGS = promo.ActualPromoIncrementalLSV * COGSPercent / 100;
                promo.ActualPromoNetIncrementalCOGS = promo.ActualPromoNetIncrementalLSV * COGSPercent / 100;

                promo.ActualPromoNetLSV = (promo.ActualPromoBaselineLSV ?? 0) + (promo.ActualPromoNetIncrementalLSV ?? 0);

                promo.ActualPromoIncrementalNSV = (promo.ActualPromoIncrementalLSV ?? 0) - (promo.ActualPromoTIShopper ?? 0) - (promo.ActualPromoTIMarketing ?? 0) - (promo.ActualPromoIncrementalBaseTI ?? 0);
                promo.ActualPromoNetIncrementalNSV = (promo.ActualPromoNetIncrementalLSV ?? 0) - (promo.ActualPromoTIShopper ?? 0) - (promo.ActualPromoTIMarketing ?? 0) - (promo.ActualPromoIncrementalBaseTI ?? 0);

                promo.ActualPromoBaselineBaseTI = promo.ActualPromoBaselineLSV * TIBasePercent / 100;
                promo.ActualPromoNetBaseTI = promo.ActualPromoNetLSV * TIBasePercent / 100;

                promo.ActualPromoNSV = (promo.ActualPromoLSVByCompensation ?? 0) - (promo.ActualPromoTIShopper ?? 0) - (promo.ActualPromoTIMarketing ?? 0) - (promo.ActualPromoBaselineBaseTI ?? 0);
                promo.ActualPromoNetNSV = (promo.ActualPromoNetLSV ?? 0) - (promo.ActualPromoTIShopper ?? 0) - (promo.ActualPromoTIMarketing ?? 0) - (promo.ActualPromoNetBaseTI ?? 0);

                promo.ActualPromoIncrementalMAC = (promo.ActualPromoIncrementalNSV ?? 0) - (promo.ActualPromoIncrementalCOGS ?? 0);
                promo.ActualPromoNetIncrementalMAC = (promo.ActualPromoNetIncrementalNSV ?? 0) - (promo.ActualPromoNetIncrementalCOGS ?? 0);

                promo.ActualPromoIncrementalEarnings = (promo.ActualPromoIncrementalMAC ?? 0) - (promo.ActualPromoBranding ?? 0) - (promo.ActualPromoBTL ?? 0) - (promo.ActualPromoCostProduction ?? 0);
                promo.ActualPromoNetIncrementalEarnings = (promo.ActualPromoNetIncrementalMAC ?? 0) - (promo.ActualPromoBranding ?? 0) - (promo.ActualPromoBTL ?? 0) - (promo.ActualPromoCostProduction ?? 0);

                // +1 / -1 ?
                promo.ActualPromoROIPercent = promo.ActualPromoTotalCost == 0 ? 0 : (promo.ActualPromoIncrementalEarnings / promo.ActualPromoTotalCost + 1) * 100;
                promo.ActualPromoNetROIPercent = promo.ActualPromoTotalCost == 0 ? 0 : (promo.ActualPromoNetIncrementalEarnings / promo.ActualPromoTotalCost + 1) * 100;

                try
                {
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    errors += e.Message + ";";
                }
            }

            if (!promo.ActualPromoXSites.HasValue || !promo.ActualPromoCatalogue.HasValue || !promo.ActualPromoBranding.HasValue
                || !promo.ActualPromoBTL.HasValue || !promo.ActualPromoCostProdXSites.HasValue || !promo.ActualPromoCostProdCatalogue.HasValue)
            {
                errors += Log.GenerateLogLine(Log.MessageType["Warning"], "For promo №") + promo.Number + " is no Budget values.";
            }

            return errors.Length == 0 ? null : errors;
        }

        /// <summary>
        /// Сбросить значения фактических полей
        /// </summary>
        /// <param name="promo">Промо</param>
        /// <param name="context">Контекст БД</param>
        private static void ResetValues(Promo promo, DatabaseContext context, bool resetActualPromoBaselineLSV, bool resetActualPromoPostPromoEffectLSV)
        {
            // если значения введены вручную через грид ActualLSV, то ненужно обновлять
            if (resetActualPromoBaselineLSV)
            {
                promo.ActualPromoBaselineLSV = null;
            }

            if (resetActualPromoPostPromoEffectLSV)
            {
                promo.ActualPromoPostPromoEffectLSVW1 = null;
                promo.ActualPromoPostPromoEffectLSVW2 = null;
                promo.ActualPromoNetIncrementalLSV = null;
            }

            promo.ActualPromoIncrementalLSV = null;
            promo.ActualPromoUpliftPercent = null;
            promo.ActualPromoNetBaseTI = null;
            promo.ActualPromoNSV = null;
            promo.ActualPromoTIShopper = null;
            promo.ActualPromoCost = null;
            promo.ActualPromoIncrementalBaseTI = null;
            promo.ActualPromoNetIncrementalBaseTI = null;
            promo.ActualPromoIncrementalCOGS = null;
            promo.ActualPromoNetIncrementalCOGS = null;
            promo.ActualPromoTotalCost = null;
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

        /// <summary>
        /// Устанавливает ActualproductBaselineLSV для записей из таблицы PromoProduct.
        /// Рассчитывает ActualPromoBaselineLSV (сумма всех ActualProductBaselineLSV).
        /// </summary>
        /// <param name="context"></param>
        /// <param name="promo"></param>
        /// <returns></returns>
        private static double CalculateActualPromoBaselineLSV(DatabaseContext context, Promo promo)
        {
            // Получаем все записи из таблицы PromoProduct для текущего промо.
            var promoProductsForCurrentPromo = context.Set<PromoProduct>()
                .Where(x => x.PromoId == promo.Id);

            // Сумма всех ActualProductBaselineLSV.
            double actualPromoBaselineLSV = 0;

            // Если есть от чего считать долю.
            if (promo.PlanPromoBaselineLSV.HasValue)
            {
                // Перебираем все найденные для текущего промо записи из таблицы PromoProduct.
                foreach (var promoProduct in promoProductsForCurrentPromo)
                {
                    // Если PlanProductBaselineLSV нет, то мы не сможем посчитать долю
                    if (promoProduct.PlanProductBaselineLSV.HasValue)
                    {
                        double currentProductBaselineLSVPercent = 0;
                        // Если показатель PlanProductBaselineLSV == 0, то он составляет 0 процентов от показателя PlanPromoBaselineLSV.
                        if (promoProduct.PlanProductBaselineLSV.Value != 0)
                        {
                            // Считаем долю PlanProductBaselineLSV от PlanPromoBaselineLSV.
                            currentProductBaselineLSVPercent = promoProduct.PlanProductBaselineLSV.Value / promo.PlanPromoBaselineLSV.Value;
                        }
                        // Устанавливаем ActualProductBaselineLSV в запись таблицы PromoProduct.
                        promoProduct.ActualProductBaselineLSV = promo.PlanPromoBaselineLSV.Value * currentProductBaselineLSVPercent;

                        // Суммираем все ActualProductBaselineLSV для получения ActualPromoBaselineLSV.
                        actualPromoBaselineLSV += promoProduct.ActualProductBaselineLSV.Value;
                    }
                }
                context.SaveChangesAsync();
            }

            return actualPromoBaselineLSV;
        }
    }
}
