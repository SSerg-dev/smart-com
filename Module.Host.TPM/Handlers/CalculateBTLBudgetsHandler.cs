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
using Core.Settings;
using Core.Dependency;
using Module.Persist.TPM.Utils;

namespace Module.Host.TPM.Handlers
{
    /// <summary>
    /// Производит перерасчёт значений BTL для всех промо, связанных с статьёй BTL
    /// </summary>
    public class CalculateBTLBudgetsHandler : BaseHandler
    {
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            // Статусы, в которых промо необходимо отвязать от BTL
            var settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
            string notAllowedBTLStatusesList = settingsManager.GetSetting<string>("NOT_ALLOWED_BTL_STATUS_LIST", "Draft,Cancelled,Deleted");

            LogWriter handlerLogger = null;
            string logLine = "";
            Stopwatch sw = new Stopwatch();
            sw.Start();

            handlerLogger = new LogWriter(info.HandlerId.ToString());
            handlerLogger.Write(true, "");
            logLine = String.Format("The calculation of the BTL budgets started at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow));
            handlerLogger.Write(true, logLine, "Message");

            string btlId = HandlerDataHelper.GetIncomingArgument<string>("BTLId", info.Data, false);
            var unlinkedPromoIdsList = HandlerDataHelper.GetIncomingArgument<List<Guid>>("UnlinkedPromoIds", info.Data, false);

            List<Guid> promoIds = new List<Guid>();
            var guidBTLId = Guid.Empty;
            Guid.TryParse(btlId, out guidBTLId);

            List<Promo> promoes = new List<Promo>();
            List<Promo> unlinkedPromoes = new List<Promo>();
            List<Promo> promoesToUnlink = new List<Promo>();

            try
            {
                if (guidBTLId == Guid.Empty)
                {
                    throw new Exception("Error when parsing BTL Id. Id has invalid format.");
                }
                using (DatabaseContext context = new DatabaseContext())
                {
                    var BTL = context.Set<BTL>().Find(guidBTLId);

                    promoes = CalculationTaskManager.GetBlockedPromo(info.HandlerId, context).ToList();
                    promoesToUnlink = promoes.Where(x => notAllowedBTLStatusesList.Contains(x.PromoStatus.SystemName)).ToList();
                    promoes = promoes.Except(promoesToUnlink).ToList();
                    if (unlinkedPromoIdsList != null)
                    {
                        var allBTLPromoes = context.Set<BTLPromo>().Where(x => x.BTLId == guidBTLId).Select(x => x.Promo);
                        unlinkedPromoes.AddRange(allBTLPromoes.Where(x => unlinkedPromoIdsList.Contains(x.Id)));
                        promoes = promoes.Except(unlinkedPromoes).ToList();
                    }
                    var closedPromoes = promoes.Where(x => x.PromoStatus.SystemName == "Closed").ToList();
                    promoes = promoes.Except(closedPromoes).ToList();

                    if (promoes.Any() || unlinkedPromoes.Any() || closedPromoes.Any() || promoesToUnlink.Any())
                    {
                        string promoNumbers = string.Join(",", promoes.Select(x => x.Number));

                        context.SaveChanges();
                        logLine = String.Format("At the time of calculation, the following promo are blocked for editing: {0}", promoNumbers);
                        handlerLogger.Write(true, logLine, "Message");
                        
                        double? promoSummPlanLSV = promoes.Any() ? promoes.Where(n => n.PlanPromoLSV.HasValue).Sum(n => n.PlanPromoLSV.Value) : 0;
                        double? closedBudgetBTL = closedPromoes.Any() ? closedPromoes.Where(n => n.ActualPromoBTL.HasValue).Sum(n => n.ActualPromoBTL.Value) : 0;
                        if (promoSummPlanLSV == null) promoSummPlanLSV = 0;
                        if (closedBudgetBTL == null) closedBudgetBTL = 0;

                        foreach (var promo in promoes)
                        {
                            handlerLogger.Write(true, "");
                            logLine = String.Format("Calculation of BTL params for promo № {0} started at {1:yyyy-MM-dd HH:mm:ss}", promo.Number, DateTimeOffset.Now);
                            handlerLogger.Write(true, logLine, "Message");

                            double kPlan = promo.PlanPromoLSV.HasValue ? promo.PlanPromoLSV.Value / promoSummPlanLSV.Value : 0;

                            if (double.IsNaN(kPlan))
                                kPlan = 0;

                            promo.PlanPromoBTL = Math.Round((BTL.PlanBTLTotal.Value - closedBudgetBTL.Value) * kPlan, 2, MidpointRounding.AwayFromZero);
                            promo.ActualPromoBTL = Math.Round((BTL.ActualBTLTotal.Value - closedBudgetBTL.Value) * kPlan, 2, MidpointRounding.AwayFromZero);
                            string calculateError = PlanPromoParametersCalculation.CalculatePromoParameters(promo.Id, context);

                            if (calculateError != null)
                            {
                                logLine = string.Format("Error when calculating the plan parameters Promo: {0}", calculateError);
                                handlerLogger.Write(true, logLine, "Error");
                            }
                            calculateError = ActualPromoParametersCalculation.CalculatePromoParameters(promo, context);
                            if (calculateError != null)
                            {
                                logLine = string.Format("Error when calculating the actual parameters Promo: {0}", calculateError);
                                handlerLogger.Write(true, logLine, "Error");
                            }
                            logLine = String.Format("Calculation of BTL params for promo № {0} completed.", promo.Number);
                            handlerLogger.Write(true, logLine, "Message");

                            CalculationTaskManager.UnLockPromo(promo.Id);
                        }

                        foreach (var promo in unlinkedPromoes)
                        {
                            handlerLogger.Write(true, "");
                            logLine = String.Format("Reset of BTL params for unlinked promo № {0} started at {1:yyyy-MM-dd HH:mm:ss}", promo.Number, DateTimeOffset.Now);
                            handlerLogger.Write(true, logLine, "Message");

                            promo.PlanPromoBTL = 0;
                            promo.ActualPromoBTL = 0;

                            string calculateError = PlanPromoParametersCalculation.CalculatePromoParameters(promo.Id, context);

                            if (calculateError != null)
                            {
                                logLine = string.Format("Error when calculating the plan parameters Promo: {0}", calculateError);
                                handlerLogger.Write(true, logLine, "Error");
                            }
                            calculateError = ActualPromoParametersCalculation.CalculatePromoParameters(promo, context);
                            if (calculateError != null)
                            {
                                logLine = string.Format("Error when calculating the actual parameters Promo: {0}", calculateError);
                                handlerLogger.Write(true, logLine, "Error");
                            }

                            logLine = String.Format("Reset of BTL params for unlinked promo № {0} completed.", promo.Number);
                            handlerLogger.Write(true, logLine, "Message");

                            CalculationTaskManager.UnLockPromo(promo.Id);
                        }

                        foreach (var closedPromo in closedPromoes)
                        {
                            CalculationTaskManager.UnLockPromo(closedPromo.Id);
                        }

                        foreach (var promoToUnlink in promoesToUnlink)
                        {
                            handlerLogger.Write(true, "");
                            logLine = String.Format("Promo № {0} unlinking from BTL started at {1:yyyy-MM-dd HH:mm:ss}", promoToUnlink.Number, DateTimeOffset.Now);
                            handlerLogger.Write(true, logLine, "Message");

                            // Отвязываем промо от BTL
                            BTLPromo BTLPromoToUnlink = context.Set<BTLPromo>().Where(x => x.BTLId == guidBTLId && x.PromoId == promoToUnlink.Id && !x.Disabled && x.DeletedDate == null).FirstOrDefault();

                            if (BTLPromoToUnlink != null)
                            {
                                BTLPromoToUnlink.Disabled = true;
                                BTLPromoToUnlink.DeletedDate = DateTimeOffset.Now;
                            }

                            // Обнуляем BTL бюджет для промо
                            promoToUnlink.PlanPromoBTL = 0;
                            promoToUnlink.ActualPromoBTL = 0;

                            logLine = String.Format("Promo № {0} unlinking from BTL completed.", promoToUnlink.Number);
                            handlerLogger.Write(true, logLine, "Message");

                            CalculationTaskManager.UnLockPromo(promoToUnlink.Id);
                        }
                        context.SaveChanges();
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
            logLine = String.Format("The calculation of the BTL budgets ended at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), sw.Elapsed.TotalSeconds);
            handlerLogger.Write(true, logLine, "Message");
            handlerLogger.UploadToBlob();
        }
    }
}