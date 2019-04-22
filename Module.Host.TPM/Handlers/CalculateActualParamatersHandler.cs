using Looper.Core;
using Looper.Parameters;
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
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            ILogWriter handlerLogger = null;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            handlerLogger = new FileLogWriter(info.HandlerId.ToString());
            handlerLogger.Write(true, String.Format("The calculation of the actual parameters began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now));
            handlerLogger.Write(true, "");

            Guid promoId = HandlerDataHelper.GetIncomingArgument<Guid>("PromoId", info.Data, false);

            try
            {
                using (DatabaseContext context = new DatabaseContext())
                {                    
                    Promo promo = context.Set<Promo>().FirstOrDefault(n => n.Id == promoId && !n.Disabled);

                    if (promo != null)
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

                    CalculationTaskManager.UnLockPromo(promo.Id);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                handlerLogger.Write(true, e.ToString());
                CalculationTaskManager.UnLockPromo(promoId);
            }

            sw.Stop();
            handlerLogger.Write(true, "");
            handlerLogger.Write(true, String.Format("The calculation of the actual parameters was completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", DateTimeOffset.Now, sw.Elapsed.TotalSeconds));
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
