using ProcessingHost.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Looper.Core;
using Module.Persist.TPM.Model.TPM;
using Looper.Parameters;
using Persist;
using System.Data.Entity;
using Utility.LogWriter;
using System.Diagnostics;
using System.Data;
using Module.Persist.TPM.CalculatePromoParametersModule;
using System.Threading;
using Core.Security.Models;
using Persist.Model;
using Module.Persist.TPM;

namespace Module.Host.TPM.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public class CalculatePromoParametersHandler : BaseHandler
    {
        public string logLine = "";
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                ILogWriter handlerLogger = null;
                Stopwatch sw = new Stopwatch();
                sw.Start();

                handlerLogger = new FileLogWriter(info.HandlerId.ToString());
                logLine = String.Format("The calculation of the parameters started at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now);
                handlerLogger.Write(true, logLine, "Message");
                handlerLogger.Write(true, "");

                Guid nullGuid = new Guid();
                Guid promoId = HandlerDataHelper.GetIncomingArgument<Guid>("PromoId", info.Data, false);
                bool needCalculatePlanMarketingTI = HandlerDataHelper.GetIncomingArgument<bool>("NeedCalculatePlanMarketingTI", info.Data, false);

                try
                {
                    if (promoId != nullGuid)
                    {
                        Promo promo = context.Set<Promo>().Where(x => x.Id == promoId).FirstOrDefault();

                        //добавление номера рассчитываемого промо в лог
                        handlerLogger.Write(true, String.Format("Calculating promo: №{0}", promo.Number), "Message");
                        handlerLogger.Write(true, "");
                        
                        //Подбор исторических промо и расчет PlanPromoUpliftPercent
                        if (NeedUpliftFinding(promo))
                        {
                            Stopwatch swUplift = new Stopwatch();
                            swUplift.Start();
                            logLine = String.Format("Pick uplift started at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now);
                            handlerLogger.Write(true, logLine, "Message");

                            string upliftMessage;
                            double? planPromoUpliftPercent = PlanPromoUpliftCalculation.FindPlanPromoUplift(promoId, context, out upliftMessage);

                            if (planPromoUpliftPercent != -1)
                            {
                                logLine = String.Format("{0}: {1}", upliftMessage, planPromoUpliftPercent);
                                handlerLogger.Write(true, logLine, "Message");
                            }
                            else
                            {
                                logLine = String.Format("{0}", upliftMessage);
                                handlerLogger.Write(true, logLine, "Message");
                            }

                            swUplift.Stop();
                            logLine = String.Format("Pick uplift completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", DateTimeOffset.Now, swUplift.Elapsed.TotalSeconds);
                            handlerLogger.Write(true, logLine, "Message");
                            handlerLogger.Write(true, "");
                        }

                        //Заполнение таблицы PromoProduct и расчет плановых параметров Product и Promo
                        Stopwatch swPromoProduct = new Stopwatch();
                        swPromoProduct.Start();
                        logLine = String.Format("Calculation of planned parameters began at {0:yyyy-MM-dd HH:mm:ss}. It may take some time.", DateTimeOffset.Now);
                        handlerLogger.Write(true, logLine, "Message");

                        //TODO: УБРАТЬ ЗАДЕРЖКУ !!!
                        //Thread.Sleep(100000000);

                        string setPromoProductError;
                        PlanProductParametersCalculation.SetPromoProduct(promoId, context, out setPromoProductError);
                        if (setPromoProductError != null)
                        {
                            logLine = String.Format("Error filling Product: {0}", setPromoProductError);
                            handlerLogger.Write(true, logLine, "Error");
                        }

                        ////TODO: УБРАТЬ ЗАДЕРЖКУ !!!
                        //Thread.Sleep(100000000);

                        string calculateError = PlanProductParametersCalculation.CalculatePromoProductParameters(promoId, context);
                        if (calculateError != null)
                        {
                            logLine = String.Format("Error when calculating the planned parameters of the Product: {0}", calculateError);
                            handlerLogger.Write(true, logLine, "Error");
                        }


                        // пересчет плановых бюджетов (из-за LSV)
                        BudgetsPromoCalculation.CalculateBudgets(promo, true, false, handlerLogger, info.HandlerId, context);

                        calculateError = PlanPromoParametersCalculation.CalculatePromoParameters(promoId, context);
                        if (calculateError != null)
                        {
                            logLine = String.Format("Error when calculating the planned parameters Promo: {0}", calculateError);
                            handlerLogger.Write(true, logLine, "Error");
                        }


                        swPromoProduct.Stop();
                        logLine = String.Format("Calculation of planned parameters was completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", DateTimeOffset.Now, swPromoProduct.Elapsed.TotalSeconds);
                        handlerLogger.Write(true, logLine, "Message");
                        handlerLogger.Write(true, "");

                        Stopwatch swActual = new Stopwatch();
                        swActual.Start();
                        logLine = String.Format("The calculation of the actual parameters began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now);
                        handlerLogger.Write(true, logLine, "Message");
                        handlerLogger.Write(true, "");

                        CalulateActual(promo, context, handlerLogger, info.HandlerId);

                        swActual.Stop();
                        handlerLogger.Write(true, "");
                        logLine = String.Format("The calculation of the actual parameters was completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", DateTimeOffset.Now, swActual.Elapsed.TotalSeconds);
                        handlerLogger.Write(true, logLine, "Message");

                        //promo.Calculating = false;
                        context.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    handlerLogger.Write(true, e.Message, "Error");
                }
                finally
                {
                    sw.Stop();
                    handlerLogger.Write(true, "");
                    logLine = String.Format("Calculation of parameters completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", DateTimeOffset.Now, sw.Elapsed.TotalSeconds);
                    handlerLogger.Write(true, logLine, "Message");

                    //разблокировку промо необходимо производить после всего, иначе не успевает прочитаться последнее сообщение и (самое главное) окончательный статус хендлера
                    //для перестраховки добавлена небольшая задержка
                    if (promoId != nullGuid)
                    {
                        Promo promo = context.Set<Promo>().Where(x => x.Id == promoId).FirstOrDefault();
                        //TODO: УБРАТЬ ЗАДЕРЖКУ !!!
                        Thread.Sleep(5000);

                        CalculationTaskManager.UnLockPromoForHandler(info.HandlerId);
                        //CalculationTaskManager.UnLockPromo(promo.Id);
                    }
                }
            }
        }

        /// <summary>
        /// Расчитать фактические показатели для Промо
        /// </summary>
        /// <param name="promo">Промо</param>
        /// <param name="context">Контекст БД</param>
        /// <param name="handlerLogger">Лог</param>
        private void CalulateActual(Promo promo, DatabaseContext context, ILogWriter handlerLogger, Guid handlerId)
        {
            // если есть ошибки, они перечисленны через ;
            string errorString = ActualProductParametersCalculation.CalculatePromoProductParameters(promo, context);
            // записываем ошибки если они есть
            if (errorString != null)
                WriteErrorsInLog(handlerLogger, errorString);

            // пересчет фактических бюджетов (из-за LSV)
            BudgetsPromoCalculation.CalculateBudgets(promo, false, true, handlerLogger, handlerId, context);

            errorString = ActualPromoParametersCalculation.CalculatePromoParameters(promo, context);

            // записываем ошибки если они есть
            if (errorString != null)
                WriteErrorsInLog(handlerLogger, errorString);
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

        private bool NeedUpliftFinding(Promo promo)
        {
            // если стоит флаг inout, то подбирать uplift не требуется
            return (!promo.NeedRecountUplift.HasValue || promo.NeedRecountUplift.Value) && (!promo.InOut.HasValue || !promo.InOut.Value);
        }
    }
}