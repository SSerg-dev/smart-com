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
using Module.Persist.TPM;
using Module.Persist.TPM.Utils;

namespace Module.Host.TPM.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public class CalculateBudgetsHandler : BaseHandler
    {
        public override void Action(HandlerInfo info, ExecuteData data)
        {

            LogWriter handlerLogger = null;
            string logLine = "";
            Stopwatch sw = new Stopwatch();
            sw.Start();

            handlerLogger = new LogWriter(info.HandlerId.ToString());
            handlerLogger.Write(true, "");
            logLine = String.Format("The calculation of the budgets started at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow));
            handlerLogger.Write(true, logLine, "Message");

            // список ID подстатей
            string promoSupportIdsString = HandlerDataHelper.GetIncomingArgument<string>("PromoSupportIds", info.Data, false);
            // список ID отвязанных промо
            string unlinkedPromoIdsString = HandlerDataHelper.GetIncomingArgument<string>("UnlinkedPromoIds", info.Data, false);

            // распарсенный список ID подстатей
            List<Guid> promoSupportIds = new List<Guid>();
            // распарсенный список ID отвязанных промо
            List<Guid> unlinkedPromoIds = new List<Guid>();

            // список промо, участвующих в расчете
            Promo[] promoes = null;

            try
            {
                using (DatabaseContext context = new DatabaseContext())
                {
                    promoes = CalculationTaskManager.GetBlockedPromo(info.HandlerId, context);

                    // копируем старые значения, чтобы определить нужно ли пересчитывать остальные параметры
                    double?[] oldPlanMarketingTI = promoes.Select(n => n.PlanPromoTIMarketing).ToArray();
                    double?[] oldActualMarketingTI = promoes.Select(n => n.ActualPromoTIMarketing).ToArray();
                    double?[] oldPlanCostProd = promoes.Select(n => n.PlanPromoCostProduction).ToArray();
                    double?[] oldActualCostProd = promoes.Select(n => n.ActualPromoCostProduction).ToArray();

                    if (promoes.Length > 0)
                    {
                        promoSupportIds = promoSupportIdsString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(n => Guid.Parse(n)).ToList();
                        unlinkedPromoIds = unlinkedPromoIdsString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(n => Guid.Parse(n)).ToList();

                        string promoNumbers = "";
                        foreach (Promo promo in promoes)
                            promoNumbers += promo.Number.Value + ", ";

                        context.SaveChanges();
                        logLine = String.Format("At the time of calculation, the following promo are blocked for editing: {0}", promoNumbers);
                        handlerLogger.Write(true, logLine, "Message");

                        // пересчитываем бюджеты
                        foreach (Guid promoSupportId in promoSupportIds)
                            BudgetsPromoCalculation.CalculateBudgets(promoSupportId, true, true, context);

                        // обновляем общие суммы по бюджетам для отвязанных промо
                        foreach (Guid promoId in unlinkedPromoIds)
                            BudgetsPromoCalculation.RecalculateSummBudgets(promoId, context);


                        context.SaveChanges();

                        for (int i = 0; i < promoes.Length; i++)
                        {
                            Promo promo = promoes[i];

                            handlerLogger.Write(true, "");
                            logLine = String.Format("Calculation of parameters for promo № {0} started at {1:yyyy-MM-dd HH:mm:ss}", promo.Number, DateTimeOffset.Now);
                            handlerLogger.Write(true, logLine, "Message");

                            if (oldPlanMarketingTI[i] != promo.PlanPromoTIMarketing || oldPlanCostProd[i] != promo.PlanPromoCostProduction)
                            {
                                string setPromoProductFieldsError = PlanPromoParametersCalculation.CalculatePromoParameters(promo.Id, context);

                                if (setPromoProductFieldsError != null)
                                {
                                    logLine = String.Format("Error when calculating the planned parameters of the PromoProduct table: {0}", setPromoProductFieldsError);
                                    handlerLogger.Write(true, logLine, "Error");
                                }
                            }

                            if (oldActualMarketingTI[i] != promo.ActualPromoTIMarketing || oldActualCostProd[i] != promo.ActualPromoCostProduction)
                            {
                                CalulateActual(promo, context, handlerLogger.CurrentLogWriter);
                            }

                            logLine = String.Format("Calculation of parameters for promo № {0} completed.", promo.Number);
                            handlerLogger.Write(true, logLine, "Message");

                            CalculationTaskManager.UnLockPromo(promo.Id);
                            context.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                handlerLogger.Write(true, e.ToString(), "Error");

                if (promoes != null)
                {
                    foreach (Promo p in promoes)
                        CalculationTaskManager.UnLockPromo(p.Id);
                }
            }

            sw.Stop();
            handlerLogger.Write(true, "");
            logLine = String.Format("The calculation of the budgets ended at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), sw.Elapsed.TotalSeconds);
            handlerLogger.Write(true, logLine, "Message");
            handlerLogger.UploadToBlob();
        }

        private void CalulateActual(Promo promo, DatabaseContext context, ILogWriter handlerLogger)
        {
            // Параметры промо считаем только, если промо из TLC или если были загружены Actuals
            var promoProductList = context.Set<PromoProduct>().Where(x => x.PromoId == promo.Id && !x.Disabled).ToList();
            if (promo.LoadFromTLC || promoProductList.Any(x => x.ActualProductPCQty.HasValue))
            {
                string errorString = ActualPromoParametersCalculation.CalculatePromoParameters(promo, context);

                // записываем ошибки если они есть
                if (errorString != null)
                    WriteErrorsInLog(handlerLogger, errorString);
            }
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