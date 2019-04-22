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

namespace Module.Host.TPM.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public class CalculateBudgetsHandler : BaseHandler
    {
        public override void Action(HandlerInfo info, ExecuteData data)
        {

            ILogWriter handlerLogger = null;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            handlerLogger = new FileLogWriter(info.HandlerId.ToString());
            handlerLogger.Write(true, "");
            handlerLogger.Write(true, String.Format("The calculation of the budgets started at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now));

            // переменные определяющие какие бюджеты необходимо пересчитать
            bool calculatePlanCostTE = HandlerDataHelper.GetIncomingArgument<bool>("CalculatePlanCostTE", info.Data, false);
            bool calculateFactCostTE = HandlerDataHelper.GetIncomingArgument<bool>("CalculateFactCostTE", info.Data, false);
            bool calculatePlanCostProd = HandlerDataHelper.GetIncomingArgument<bool>("CalculatePlanCostProd", info.Data, false);
            bool calculateFactCostProd = HandlerDataHelper.GetIncomingArgument<bool>("CalculateFactCostProd", info.Data, false);

            // список ID подстатей/промо
            string promoSupportPromoIds = HandlerDataHelper.GetIncomingArgument<string>("PromoSupportPromoIds", info.Data, false);
            // распарсенный список ID подстатей/промо
            List<Guid> subItemsIdsList = new List<Guid>();
            // список промо, участвующих в расчете
            Promo[] promoes = null;

            try
            {
                using (DatabaseContext context = new DatabaseContext())
                {                    
                    promoes = CalculationTaskManager.GetBlockedPromo(info.HandlerId, context);

                    if (promoes.Length > 0)
                    {
                        subItemsIdsList = promoSupportPromoIds.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(n => Guid.Parse(n)).ToList();

                        // список подстатей/промо
                        PromoSupportPromo[] psps = context.Set<PromoSupportPromo>().Where(n => subItemsIdsList.Any(m => m == n.Id)).ToArray();

                        string promoNumbers = "";
                        foreach (Promo promo in promoes)
                            promoNumbers += promo.Number.Value + ", ";

                        context.SaveChanges();
                        handlerLogger.Write(true, String.Format("At the time of calculation, the following promo are blocked for editing: {0}", promoNumbers));

                        foreach (PromoSupportPromo psp in psps)
                            BudgetsPromoCalculation.CalculateBudgets(psp, calculatePlanCostTE, calculateFactCostTE, calculatePlanCostProd, calculateFactCostProd, context);

                        foreach (Promo promo in promoes)
                        {
                            handlerLogger.Write(true, "");
                            handlerLogger.Write(true, String.Format("Calculation of parameters for promo №{0} started at {1:yyyy-MM-dd HH:mm:ss}", promo.Number, DateTimeOffset.Now));

                            if (calculatePlanCostTE || calculatePlanCostProd)
                            {
                                string setPromoProductFieldsError = PlanPromoParametersCalculation.CalculatePromoParameters(promo.Id, context);

                                if (setPromoProductFieldsError != null)
                                {
                                    handlerLogger.Write(true, String.Format("Error when calculating the planned parameters of the PromoProduct table: {0}", setPromoProductFieldsError));
                                }

                                CalulateActual(promo, context, handlerLogger);
                            }

                            if (calculateFactCostTE || calculateFactCostProd)
                            {
                                CalulateActual(promo, context, handlerLogger);
                            }

                            handlerLogger.Write(true, String.Format("Calculation of parameters for promo № {0} completed.", promo.Number));
                            CalculationTaskManager.UnLockPromo(promo.Id);
                            context.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                handlerLogger.Write(true, e.ToString());

                if (promoes != null)
                {
                    foreach (Promo p in promoes)
                        CalculationTaskManager.UnLockPromo(p.Id);
                }
            }

            sw.Stop();
            handlerLogger.Write(true, "");
            handlerLogger.Write(true, String.Format("The calculation of the budgets ended at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", DateTimeOffset.Now, sw.Elapsed.TotalSeconds));
        }

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