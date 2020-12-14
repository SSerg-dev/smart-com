using ProcessingHost.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Looper.Core;
using Utility.LogWriter;
using System.Diagnostics;
using Interfaces.Core.Common;
using Module.Host.TPM.Actions;
using Persist;
using Persist.Model;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.CalculatePromoParametersModule;
using System.Threading;
using Module.Persist.TPM.Utils;

namespace Module.Host.TPM.Handlers
{
    /// <summary>
    /// Класс для пересчета параметров при изменении BaseLine
    /// </summary>
    public class BaseLineUpgradeHandler : BaseHandler
    {
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            LogWriter handlerLogger = null;
            Stopwatch sw = new Stopwatch();
            List<Promo> promoQuery = new List<Promo>();

            sw.Start();
            try
            {
                handlerLogger = new LogWriter(info.HandlerId.ToString());
                handlerLogger.Write(true, String.Format("Calculation of parameters started at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)), "Message");
                //Console.WriteLine("Start: {0}", DateTimeOffset.Now);

                using (DatabaseContext context = new DatabaseContext())
                {
                    LoopHandler handler = context.Set<LoopHandler>().Find(info.HandlerId);
                    DateTimeOffset prevExecutionDate;

                    // если по расписанию то дата последнего запуска рассчитывается через вычитание периода запуска
                    // иначе - это разовая задача и нужно брать фактическое значение и обновить даты у истинного обработчика
                    if (handler.ExecutionMode.ToLower() == "schedule")
                    {
                        prevExecutionDate = handler.LastExecutionDate.Value.AddMilliseconds(-(double)handler.ExecutionPeriod);
                    }
                    else
                    {
                        LoopHandler handlerSchedule = context.Set<LoopHandler>().Where(n => n.ExecutionMode.ToLower() == "schedule" && n.Name == handler.Name).First();
                        prevExecutionDate = handlerSchedule.LastExecutionDate ?? DateTimeOffset.Now;
                        handlerSchedule.LastExecutionDate = DateTimeOffset.Now;
                        handlerSchedule.Status = "WAITING";

                        // если дата следущего запуска была, отодвигаем её на период запуск (или несколько, если обработчик давно завис)
                        // иначе определям от текущего момента + период
                        if (handlerSchedule.NextExecutionDate.HasValue)
                        {
                            while (DateTimeOffset.Compare(handlerSchedule.NextExecutionDate.Value, handlerSchedule.LastExecutionDate.Value) <= 0)
                                handlerSchedule.NextExecutionDate = handlerSchedule.NextExecutionDate.Value.AddMilliseconds((double)handlerSchedule.ExecutionPeriod);
                        }
                        else
                        {
                            handlerSchedule.NextExecutionDate = handlerSchedule.LastExecutionDate.Value.AddMilliseconds((double)handlerSchedule.ExecutionPeriod);
                        }
                    }

                    List<BaseLine> baseLineList = context.Set<BaseLine>().Where(x => x.LastModifiedDate.HasValue && DateTimeOffset.Compare(prevExecutionDate, x.LastModifiedDate.Value) < 0 && !x.Disabled).ToList();

                    promoQuery = GetPromo(baseLineList, context);
                    string promoNumbers = "";

                    //TODO: It is not a bad practice, it just means that you did not think your code through.
                    while (true)
                    {
                        if (CalculationTaskManager.BlockPromoRange(promoQuery, info.HandlerId))
                        {
                            foreach (Promo promo in promoQuery)
                                promoNumbers += promo.Number.Value + ", ";

                            break;
                        }
                        else
                        {
                            // Если блокировка не удалась, ждем 5 мин и проверяем
                            // Надо бы через перезапуск задачи
                            Thread.Sleep(1000 * 60 * 5);
                        }
                    }

                    handlerLogger.Write(true, String.Format("At the time of calculation, the following promo are blocked for editing: {0}", promoNumbers), "Message");

                    foreach (Promo promo in promoQuery)
                    {
                        string calculateError = null;
                        if (!promo.LoadFromTLC)
                        {
                            string calculateBaselineError = null;
                            calculateError = PlanProductParametersCalculation.CalculateBaseline(context, promo.Id);
                            calculateError = PlanProductParametersCalculation.CalculatePromoProductParameters(promo.Id, context);

                            if (calculateBaselineError != null && calculateError != null)
                            {
                                calculateError += calculateBaselineError;
                            }
                            else if (calculateBaselineError != null && calculateError == null)
                            {
                                calculateError = calculateBaselineError;
                            }

                            if (calculateError != null)
                            {
                                handlerLogger.Write(true, String.Format("Error when calculating the planned parameters of the Product: {0}", calculateError), "Error");
                            }
                        }

                        // пересчет плановых бюджетов (из-за LSV)
                        BudgetsPromoCalculation.CalculateBudgets(promo, true, false, handlerLogger.CurrentLogWriter, info.HandlerId, context);

                        BTL btl = context.Set<BTLPromo>().Where(x => x.PromoId == promo.Id && !x.Disabled && x.DeletedDate == null).FirstOrDefault()?.BTL;
                        if (btl != null)
                        {
                            BudgetsPromoCalculation.CalculateBTLBudgets(btl, true, false, handlerLogger.CurrentLogWriter, context);
                        }

                        calculateError = PlanPromoParametersCalculation.CalculatePromoParameters(promo.Id, context);
                        if (calculateError != null)
                        {
                            handlerLogger.Write(true, String.Format("Error when calculating the planned parameters Promo: {0}", calculateError), "Error");
                        }

                        CalulateActual(promo, context, handlerLogger.CurrentLogWriter, info.HandlerId);
                        context.SaveChanges();
                    }

                }
            }
            catch (Exception e)
            {
                if (handlerLogger != null)
                {
                    handlerLogger.Write(true, e.ToString(), "Error");
                }
            }
            finally
            {
                foreach (Promo promo in promoQuery)
                    CalculationTaskManager.UnLockPromo(promo.Id);

                sw.Stop();
                if (handlerLogger != null)
                {
                    handlerLogger.Write(true, String.Format("Calculation of parameters completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), sw.Elapsed.TotalSeconds), "Message");
                    //Console.WriteLine("Fin. Duration: {1} s", DateTimeOffset.Now, sw.Elapsed.TotalSeconds);
                    handlerLogger.UploadToBlob();
                }
            }
        }


        /// <summary>
        /// Получить промо, которое необходимо пересчитать
        /// </summary>
        /// <param name="baseLineList">Список BaseLines</param>
        /// <param name="context">Контекст БД</param>
        /// <returns></returns>
        private List<Promo> GetPromo(List<BaseLine> baseLineList, DatabaseContext context)
        {
            List<Promo> promo = new List<Promo>();

            foreach (BaseLine baseLine in baseLineList)
            {
                List<PromoProduct> promoProduct = context.Set<PromoProduct>().Where(x => x.ProductId == baseLine.ProductId && !x.Disabled).ToList();
                promo.AddRange(promoProduct.Select(x => x.Promo).Where(n => n.PromoStatus.SystemName != "Draft" && n.PromoStatus.SystemName != "Closed" && !n.Disabled).Distinct());
            }

            return promo;
        }

        /// <summary>
        /// Расчитать фактические показатели для Промо
        /// </summary>
        /// <param name="promo">Промо</param>
        /// <param name="context">Контекст БД</param>
        /// <param name="handlerLogger">Лог</param>
        private void CalulateActual(Promo promo, DatabaseContext context, ILogWriter handlerLogger, Guid handlerId)
        {
            string errorString = null;
            // Продуктовые параметры считаем только, если были загружены Actuals
            var promoProductList = context.Set<PromoProduct>().Where(x => x.PromoId == promo.Id && !x.Disabled).ToList();
            if (!promo.LoadFromTLC && promoProductList.Any(x => x.ActualProductPCQty.HasValue))
            {
                // если есть ошибки, они перечисленны через ;
                errorString = ActualProductParametersCalculation.CalculatePromoProductParameters(promo, context);
                // записываем ошибки если они есть
                if (errorString != null)
                    WriteErrorsInLog(handlerLogger, errorString);
            }

            // пересчет актуальных[ бюджетов (из-за LSV)
            BudgetsPromoCalculation.CalculateBudgets(promo, false, true, handlerLogger, handlerId, context);

            BTL btl = context.Set<BTLPromo>().Where(x => x.PromoId == promo.Id && !x.Disabled && x.DeletedDate == null).FirstOrDefault()?.BTL;
            if (btl != null)
            {
                BudgetsPromoCalculation.CalculateBTLBudgets(btl, false, true, handlerLogger, context);
            }

            // Параметры промо считаем только, если промо из TLC или если были загружены Actuals
            if (promo.LoadFromTLC || promoProductList.Any(x => x.ActualProductPCQty.HasValue))
            {
                errorString = ActualPromoParametersCalculation.CalculatePromoParameters(promo, context);
            }

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
