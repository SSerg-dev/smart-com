﻿using Looper.Core;
using Looper.Parameters;
using Module.Persist.TPM;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.TPM;
using Persist;
using ProcessingHost.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.LogWriter;

namespace Module.Host.TPM.Handlers
{
    public class ActualLSVChangeHandler : BaseHandler
    {
        public string logLine = "";
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            ILogWriter handlerLogger = new FileLogWriter(info.HandlerId.ToString());
            Stopwatch sw = new Stopwatch();
            sw.Start();

            logLine = String.Format("The calculation of the actual parameters began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now);
            handlerLogger.Write(true, logLine, "Message");
            handlerLogger.Write(true, "");

            Guid[] promoIds = HandlerDataHelper.GetIncomingArgument<Guid[]>("PromoIds", info.Data, false);

            foreach (Guid promoId in promoIds)
            {
                try
                {
                    using (DatabaseContext context = new DatabaseContext())
                    {
                        Promo promo = context.Set<Promo>().FirstOrDefault(n => n.Id == promoId && !n.Disabled);

                        if (promo != null)
                        {
                            logLine = String.Format("The calculation of the actual parameters for Promo № {0} parameters began at {1:yyyy-MM-dd HH:mm:ss}", promo.Number, DateTimeOffset.Now);
                            handlerLogger.Write(true, logLine, "Message");

                            // перераспределяем веденные занчения
                            CalculateAllActualProductBaselineLSV(promo, context);
                            CalculateAllActualLSV(promo, context);
                            CalculateAllActualProductPostPromoEffect(promo, context);
                            context.SaveChanges();

                            // если есть ошибки, они перечисленны через ;
                            string errorString = ActualProductParametersCalculation.CalculatePromoProductParameters(promo, context, true);
                            // записываем ошибки если они есть
                            if (errorString != null)
                                WriteErrorsInLog(handlerLogger, errorString);

                            ActualPromoParametersCalculation.ResetValues(promo, context, true);
                            errorString = ActualPromoParametersCalculation.CalculatePromoParameters(promo, context, true);
                            // записываем ошибки если они есть
                            if (errorString != null)
                                WriteErrorsInLog(handlerLogger, errorString);

                            logLine = String.Format("The calculation of the actual parameters for Promo № {0} parameters was completed at {1:yyyy-MM-dd HH:mm:ss}", promo.Number, DateTimeOffset.Now);
                            handlerLogger.Write(true, logLine, "Message");
                        }

                        //CalculationTaskManager.UnLockPromo(promo.Id);
                        CalculationTaskManager.UnLockPromoForHandler(info.HandlerId);
                        context.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    handlerLogger.Write(true, e.ToString(), "Error");
                    CalculationTaskManager.UnLockPromoForHandler(info.HandlerId);
                }
            }

            sw.Stop();
            handlerLogger.Write(true, "");
            logLine = String.Format("The calculation of the actual parameters was completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", DateTimeOffset.Now, sw.Elapsed.TotalSeconds);
            handlerLogger.Write(true, logLine, "Message");
        }

        /// <summary>
        /// Записать ошибки в лог
        /// </summary>
        /// <param name="handlerLogger">Лог</param>
        /// <param name="errorString">Список ошибок, записанных через ';'</param>
        private void WriteErrorsInLog(ILogWriter handlerLogger, string errorString)
        {
            string[] errors = errorString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            string message = "";
            foreach (string e in errors)
                message += e + "\n";

            handlerLogger.Write(true, message);
        }

        /// <summary>
        /// Рассчитывает ActualProductBaselineLSV для каждой записи из таблицы PromoProduct.
        /// </summary>
        private void CalculateAllActualProductBaselineLSV(Promo promo, DatabaseContext context)
        {
            // Если Demand ввел корректное число.
            if (promo.ActualPromoBaselineLSV.HasValue && promo.ActualPromoBaselineLSV.Value > 0)
            {
                // Если есть от чего считать долю.
                if (promo.PlanPromoBaselineLSV.HasValue && promo.PlanPromoBaselineLSV.Value != 0)
                {
                    // Получаем все записи из таблицы PromoProduct для текущего промо.
                    var promoProductsForCurrentPromo = context.Set<PromoProduct>().Where(x => x.PromoId == promo.Id && !x.Disabled);

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
                            promoProduct.ActualProductBaselineLSV = promo.ActualPromoBaselineLSV.Value * currentProductBaselineLSVPercent;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Рассчитывает ActualProductLSV для каждой записи из таблицы PromoProduct.
        /// </summary>
        private void CalculateAllActualLSV(Promo promo, DatabaseContext context)
        {
            // Если Demand ввел корректное число.
            if (promo.ActualPromoLSV.HasValue && promo.ActualPromoLSV.Value > 0)
            {
                // Если есть от чего считать долю.
                if (promo.ActualPromoLSVByCompensation.HasValue && promo.ActualPromoLSVByCompensation.Value != 0)
                {
                    // Получаем все записи из таблицы PromoProduct для текущего промо.
                    var promoProductsForCurrentPromo = context.Set<PromoProduct>().Where(x => x.PromoId == promo.Id && !x.Disabled);

                    // Перебираем все найденные для текущего промо записи из таблицы PromoProduct.
                    foreach (var promoProduct in promoProductsForCurrentPromo)
                    {
                        // Если ActualProductLSVByCompensation нет, то мы не сможем посчитать долю
                        if (promoProduct.ActualProductLSVByCompensation.HasValue)
                        {
                            double currentActualProductLSVByCompensation = 0;
                            // Если показатель ActualProductLSVByCompensation == 0, то он составляет 0 процентов от показателя ActualPromoLSVByCompensation.
                            if (promoProduct.ActualProductLSVByCompensation.Value != 0)
                            {
                                // Считаем долю ActualProductLSVByCompensation от ActualPromoLSVByCompensation.
                                currentActualProductLSVByCompensation = promoProduct.ActualProductLSVByCompensation.Value / promo.ActualPromoLSVByCompensation.Value;
                            }
                            // Устанавливаем ActualProductLSV в запись таблицы PromoProduct.
                            promoProduct.ActualProductLSV = promo.ActualPromoLSV.Value * currentActualProductLSVByCompensation;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Рассчитывает ActualProductPostPromoEffectLSV для каждой записи из таблицы PromoProduct.
        /// </summary>
        private void CalculateAllActualProductPostPromoEffect(Promo promo, DatabaseContext context)
        {
            var actualPromoPostPromoEffectLSVTotal = promo.ActualPromoPostPromoEffectLSVW1 + promo.ActualPromoPostPromoEffectLSVW2;

            // Если Demand ввел корректное число.
            if (actualPromoPostPromoEffectLSVTotal.HasValue && actualPromoPostPromoEffectLSVTotal.Value > 0)
            {
                // Если есть от чего считать долю.
                if (promo.PlanPromoBaselineLSV.HasValue && promo.PlanPromoBaselineLSV.Value != 0)
                {
                    // Получаем все записи из таблицы PromoProduct для текущего промо.
                    var promoProductsForCurrentPromo = context.Set<PromoProduct>().Where(x => x.PromoId == promo.Id && !x.Disabled);

                    // Перебираем все найденные для текущего промо записи из таблицы PromoProduct.
                    foreach (var promoProduct in promoProductsForCurrentPromo)
                    {
                        // Если ActualProductPostPromoEffectLSV нет, то мы не сможем посчитать долю
                        if (promoProduct.ActualProductPostPromoEffectLSV.HasValue)
                        {
                            double currentActualProductPostPromoEffectLSVPercent = 0;
                            // Если показатель ActualProductPostPromoEffectLSV == 0, то он составляет 0 процентов от показателя PlanPromoBaselineLSV.
                            if (promoProduct.ActualProductPostPromoEffectLSV.Value != 0)
                            {
                                // Считаем долю ActualProductPostPromoEffectLSV от PlanPromoBaselineLSV.
                                currentActualProductPostPromoEffectLSVPercent = promoProduct.ActualProductPostPromoEffectLSV.Value / promo.PlanPromoBaselineLSV.Value;
                            }
                            // Устанавливаем ActualProductBaselineLSV в запись таблицы PromoProduct.
                            promoProduct.ActualProductPostPromoEffectLSV = actualPromoPostPromoEffectLSVTotal * currentActualProductPostPromoEffectLSVPercent;
                        }
                    }
                }
            }
        }
    }
}
