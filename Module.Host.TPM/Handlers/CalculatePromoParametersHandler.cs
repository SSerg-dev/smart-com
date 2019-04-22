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

namespace Module.Host.TPM.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public class CalculatePromoParametersHandler : BaseHandler
    {      
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                ILogWriter handlerLogger = null;
                Stopwatch sw = new Stopwatch();
                sw.Start();

                handlerLogger = new FileLogWriter(info.HandlerId.ToString());
                handlerLogger.Write(true, String.Format("The calculation of the parameters started at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now));
                handlerLogger.Write(true, "");

                Guid nullGuid = new Guid();
                Guid promoId = HandlerDataHelper.GetIncomingArgument<Guid>("PromoId", info.Data, false);
                bool needCalculatePlanMarketingTI = HandlerDataHelper.GetIncomingArgument<bool>("NeedCalculatePlanMarketingTI", info.Data, false);

                if (promoId != nullGuid)
                {
                    try
                    {
                        Promo promo = context.Set<Promo>().Where(x => x.Id == promoId).FirstOrDefault();

                        //Подбор исторических промо и расчет PlanPromoUpliftPercent
                        if (!promo.NeedRecountUplift.HasValue || promo.NeedRecountUplift.Value)
                        {
                            Stopwatch swUplift = new Stopwatch();
                            swUplift.Start();
                            handlerLogger.Write(true, String.Format("Pick uplift started at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now));

                            string upliftMessage;
                            double? planPromoUpliftPercent = PlanPromoUpliftCalculation.FindPlanPromoUplift(promoId, context, out upliftMessage);

                            if (planPromoUpliftPercent != -1)
                            {
                                handlerLogger.Write(true, String.Format("{0}: {1}", upliftMessage, planPromoUpliftPercent));
                            }
                            else
                            {
                                handlerLogger.Write(true, String.Format("{0}", upliftMessage));
                            }

                            swUplift.Stop();
                            handlerLogger.Write(true, String.Format("Pick uplift completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", DateTimeOffset.Now, swUplift.Elapsed.TotalSeconds));
                            handlerLogger.Write(true, "");
                        }

                        //Заполнение таблицы PromoProduct и расчет плановых параметров Product и Promo
                        Stopwatch swPromoProduct = new Stopwatch();
                        swPromoProduct.Start();
                        handlerLogger.Write(true, String.Format("Calculation of planned parameters began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now));

                        //TODO: УБРАТЬ ЗАДЕРЖКУ !!!
                        //Thread.Sleep(10000);

                        string setPromoProductError;
                        PlanProductParametersCalculation.SetPromoProduct(promoId, context, out setPromoProductError);
                        if (setPromoProductError != null)
                        {
                            handlerLogger.Write(true, String.Format("Error filling Product: {0}", setPromoProductError));
                        }

                        ////TODO: УБРАТЬ ЗАДЕРЖКУ !!!
                        //Thread.Sleep(15000);

                        string calculateError = PlanProductParametersCalculation.CalculatePromoProductParameters(promoId, context);
                        if (calculateError != null)
                        {
                            handlerLogger.Write(true, String.Format("Error when calculating the planned parameters of the Product: {0}", calculateError));
                        }

                        // если изменилась длительность промо, то необходимо перерасчитать Marketing TI
                        if (needCalculatePlanMarketingTI)
                        {
                            CalculatePlanMarketingTI(promo, handlerLogger, info, context);
                        }
                        else
                        {
                            calculateError = PlanPromoParametersCalculation.CalculatePromoParameters(promoId, context);
                            if (calculateError != null)
                            {
                                handlerLogger.Write(true, String.Format("Error when calculating the planned parameters Promo: {0}", calculateError));
                            }
                        }

                        swPromoProduct.Stop();
                        handlerLogger.Write(true, String.Format("Calculation of planned parameters was completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", DateTimeOffset.Now, swPromoProduct.Elapsed.TotalSeconds));
                        handlerLogger.Write(true, "");

                        Stopwatch swActual = new Stopwatch();
                        swActual.Start();
                        handlerLogger.Write(true, String.Format("The calculation of the actual parameters began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now));
                        handlerLogger.Write(true, "");

                        CalulateActual(promo, context, handlerLogger);

                        swActual.Stop();
                        handlerLogger.Write(true, "");
                        handlerLogger.Write(true, String.Format("The calculation of the actual parameters was completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", DateTimeOffset.Now, swActual.Elapsed.TotalSeconds));

                        //promo.Calculating = false;
                        context.SaveChanges();

                        CalculationTaskManager.UnLockPromo(promo.Id);
                    }
                    catch (Exception e)
                    {
                        handlerLogger.Write(true, e.Message);
                        CalculationTaskManager.UnLockPromo(promoId);
                    }
                }               

                sw.Stop();
                handlerLogger.Write(true, "");
                handlerLogger.Write(true, String.Format("Calculation of parameters completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", DateTimeOffset.Now, sw.Elapsed.TotalSeconds));
            }
        }

        /// <summary>
        /// Пересчитать распределенные значения для подстатьи Marketing и обновить Marketing TI для затронутых промо
        /// </summary>
        /// <param name="promo">Промо, послуживший причиной перерасчета</param>
        /// <param name="context">Контекст БД</param>
        private void CalculatePlanMarketingTI(Promo promo, ILogWriter handlerLogger, HandlerInfo info, DatabaseContext context)
        {
            handlerLogger.Write(true, "");
            handlerLogger.Write(true, String.Format("The calculation of the budgets started at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now));

            // находим все подстатьи Marketing к которым привязано промо
            PromoSupportPromo[] promoSupports = context.Set<PromoSupportPromo>().Where(n => n.PromoId == promo.Id && !n.Disabled
                && n.PromoSupport.BudgetSubItem.BudgetItem.Budget.Name.ToLower().IndexOf("marketing") >= 0).ToArray();

            // список промо, участвующих в расчете
            Promo[] promoes = CalculationTaskManager.GetBlockedPromo(info.HandlerId, context);

            string promoNumbers = "";
            foreach (Promo p in promoes)
                promoNumbers += p.Number.Value + ", ";

            context.SaveChanges();
            handlerLogger.Write(true, String.Format("At the time of calculation, the following promo are blocked for editing: {0}", promoNumbers));

            // пересчитываем распределенные значения
            string error = "";

            try
            {
                foreach (PromoSupportPromo psp in promoSupports)
                {
                    error = BudgetsPromoCalculation.CalculateBudgets(psp, true, false, false, false, context);

                    if (error != null)
                        throw new Exception();
                }
            }
            catch
            {
                handlerLogger.Write(true, String.Format("Error calculating planned Marketing TI: {0}", error));
            }

            handlerLogger.Write(true, String.Format("The calculation of the budgets completed"));

            foreach (Promo p in promoes)
            {
                string setPromoProductFieldsError = PlanPromoParametersCalculation.CalculatePromoParameters(p.Id, context);

                if (setPromoProductFieldsError != null)
                {
                    handlerLogger.Write(true, String.Format("Error when calculating the planned parameters of the PromoProduct table: {0}", setPromoProductFieldsError));
                }

                handlerLogger.Write(true, String.Format("Calculation of parameters for promo № {0} completed.", p.Number));

                CalculationTaskManager.UnLockPromo(p.Id);
                context.SaveChanges();
            }

            handlerLogger.Write(true, "");
        }

        /// <summary>
        /// Расчитать фактические показатели для Промо
        /// </summary>
        /// <param name="promo">Промо</param>
        /// <param name="context">Контекст БД</param>
        /// <param name="handlerLogger">Лог</param>
        private void CalulateActual(Promo promo, DatabaseContext context, ILogWriter handlerLogger)
        {
            // если есть ошибки, они перечисленны через ;
            string errorString = ActualProductParametersCalculation.CalculatePromoProductParameters(promo, context);

            ActualPromoParametersCalculation.ResetValues(promo, context);
            if (errorString == null)
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
    }
}