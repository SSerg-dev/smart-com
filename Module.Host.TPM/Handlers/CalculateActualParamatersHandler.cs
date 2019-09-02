using Looper.Core;
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
    public class CalculateActualParamatersHandler : BaseHandler
    {
        public string logLine = "";
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            ILogWriter handlerLogger = null;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            handlerLogger = new FileLogWriter(info.HandlerId.ToString());
            logLine = String.Format("The calculation of the actual parameters began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now);
            handlerLogger.Write(true, logLine, "Message");
            handlerLogger.Write(true, "");

            Guid promoId = HandlerDataHelper.GetIncomingArgument<Guid>("PromoId", info.Data, false);

            try
            {
                using (DatabaseContext context = new DatabaseContext())
                {
                    Promo promo = context.Set<Promo>().FirstOrDefault(n => n.Id == promoId && !n.Disabled);

                    if (promo != null && promo.PromoStatus.SystemName == "Finished")
                    {
                        string errorString = null;
                        if (!promo.LoadFromTLC)
                        {
                            // если есть ошибки, они перечисленны через ;
                            errorString = ActualProductParametersCalculation.CalculatePromoProductParameters(promo, context);
                            // записываем ошибки если они есть
                            if (errorString != null)
                                WriteErrorsInLog(handlerLogger, errorString);
                        }

                        // пересчет фактических бюджетов (из-за LSV)
                        BudgetsPromoCalculation.CalculateBudgets(promo, false, true, handlerLogger, info.HandlerId, context);

                        errorString = ActualPromoParametersCalculation.CalculatePromoParameters(promo, context);
                        // записываем ошибки если они есть
                        if (errorString != null)
                            WriteErrorsInLog(handlerLogger, errorString);
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
    }
}
