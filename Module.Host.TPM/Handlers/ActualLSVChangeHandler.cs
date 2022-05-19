using Looper.Core;
using Looper.Parameters;
using Module.Persist.TPM;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
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
            LogWriter handlerLogger = new LogWriter(info.HandlerId.ToString());
            Stopwatch sw = new Stopwatch();
            sw.Start();

            logLine = String.Format("The calculation of the actual parameters began at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow));
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

                            string errorString = null;
                            // Продуктовые параметры считаем только, если были загружены Actuals
                            var promoProductList = context.Set<PromoProduct>().Where(x => x.PromoId == promo.Id && !x.Disabled).ToList();
                            if (!promo.LoadFromTLC && promoProductList.Any(x => x.ActualProductPCQty.HasValue))
                            {
                                // если есть ошибки, они перечисленны через ;
                                errorString = ActualProductParametersCalculation.CalculatePromoProductParameters(promo, context, true);
                                // записываем ошибки если они есть
                                if (errorString != null)
                                    WriteErrorsInLog(handlerLogger.CurrentLogWriter, errorString);
                            }

                            // Параметры промо считаем только, если промо из TLC или если были загружены Actuals
                            if (promo.LoadFromTLC || promoProductList.Any(x => x.ActualProductPCQty.HasValue))
                            {
                                errorString = ActualPromoParametersCalculation.CalculatePromoParameters(promo, context, true);
                            }

                            // записываем ошибки если они есть
                            if (errorString != null)
                                WriteErrorsInLog(handlerLogger.CurrentLogWriter, errorString);

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
            logLine = String.Format("The calculation of the actual parameters was completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), sw.Elapsed.TotalSeconds);
            handlerLogger.Write(true, logLine, "Message");
            handlerLogger.UploadToBlob();
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
        public static void CalculateAllActualLSV(Promo promo, DatabaseContext context)
        {
            // распределять введенное значение по ActualProductLSV надо только для Off-Invoice промо
            // для On-Invoice промо значения ActualProductLSV рассчитаются после введения конпенсаций
            if (!promo.IsOnInvoice)
            {
                promo.ActualPromoLSV = promo.ActualPromoLSVSO;
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
                                promoProduct.ActualProductQtySO = promoProduct.ActualProductLSV / (promoProduct.Price / promoProduct.Product.UOM_PC2Case);
                            }
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
            if (!promo.InOut.HasValue || !promo.InOut.Value)
            {
                ClientTree clientTree = null;
                clientTree = context.Set<ClientTree>().Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue).FirstOrDefault();

                // Получаем все записи из таблицы PromoProduct для текущего промо.
                var promoProducts = context.Set<PromoProduct>().Where(x => x.PromoId == promo.Id && !x.Disabled);

                promo.ActualPromoPostPromoEffectLSV = promo.IsOnInvoice ? (promo.ActualPromoLSVSO ?? 0) - (promo.ActualPromoLSVSI ?? 0) : promo.ActualPromoPostPromoEffectLSVW1 + promo.ActualPromoPostPromoEffectLSVW2;
                //Volume
                promo.ActualPromoPostPromoEffectVolume = (promoProducts.Sum(g => g.ActualProductPostPromoEffectLSV)) / promoProducts.Sum(g => g.Price / g.Product.UOM_PC2Case) * promoProducts.Sum(g => g.Product.PCVolume);

                if (promo.IsOnInvoice)
                {
                    promo.ActualPromoPostPromoEffectLSVW1 = promo.ActualPromoPostPromoEffectLSV * (clientTree.PostPromoEffectW1 / (clientTree.PostPromoEffectW1 + clientTree.PostPromoEffectW2));
                    promo.ActualPromoPostPromoEffectLSVW2 = promo.ActualPromoPostPromoEffectLSV * (clientTree.PostPromoEffectW2 / (clientTree.PostPromoEffectW1 + clientTree.PostPromoEffectW2));
                }

                // Если есть от чего считать долю.
                if (promo.PlanPromoBaselineLSV.HasValue && promo.PlanPromoBaselineLSV.Value != 0)
                {
                    // Перебираем все найденные для текущего промо записи из таблицы PromoProduct.
                    foreach (var promoProduct in promoProducts)
                    {
                        // Если PlanProductPostPromoEffectLSV нет, то мы не сможем посчитать долю
                        if (promoProduct.PlanProductPostPromoEffectLSV.HasValue)
                        {
                            double currentPlanProductPostPromoEffectLSVPercent = 0;
                            //double currentPlanProductBaselineLSVPercent = 0;

                            // Если показатель PlanProductPostPromoEffectLSV == 0, то он составляет 0 процентов от показателя PlanPromoPostPromoEffectLSV.
                            if (promoProduct.PlanProductPostPromoEffectLSV.Value != 0)
                            {
                                // Считаем долю PlanProductPostPromoEffectLSV от PlanPromoPostPromoEffectLSV.
                                currentPlanProductPostPromoEffectLSVPercent = promoProduct.PlanProductPostPromoEffectLSV.Value / promo.PlanPromoPostPromoEffectLSV.Value;

                                // Считаем долю PlanProductBaselineLSV от PlanPromoBaselineLSV.
                                //currentPlanProductBaselineLSVPercent = promoProduct.PlanProductBaselineLSV.Value / promo.PlanPromoBaselineLSV.Value;
                            }

                            // Устанавливаем ActualProductPostPromoEffectLSV в запись таблицы PromoProduct.
                            promoProduct.ActualProductPostPromoEffectLSV = promo.ActualPromoPostPromoEffectLSV * currentPlanProductPostPromoEffectLSVPercent;
                        }
                    }
                }
            }
        }
    }
}
