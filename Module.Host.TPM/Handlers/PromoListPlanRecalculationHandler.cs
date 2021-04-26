using Core.Dependency;
using Core.Settings;
using Looper.Core;
using Module.Frontend.TPM.Controllers;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl;
using Module.Persist.TPM.Utils;
using Persist;
using ProcessingHost.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.LogWriter;

namespace Module.Host.TPM.Handlers
{
    class PromoListPlanRecalculationHandler : BaseHandler
    {
        public string logLine = "";
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            LogWriter handlerLogger = null;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                var promoesToRecalculate = new List<Promo>();
                var settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));

                using (var prev_context = new DatabaseContext())
                {
                    handlerLogger = new LogWriter(info.HandlerId.ToString());
                    handlerLogger.Write(true, String.Format("Plan parameters recalculation started at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)), "Message");

                    var promoNumbersRecalculatingString = settingsManager.GetSetting<string>("PROMO_LIST_FOR_RECALCULATION");
                    var promoNumbers = promoNumbersRecalculatingString.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    var activePromo = prev_context.Set<Promo>().Where(x => !x.Disabled);

                    String formatStrPromo = "UPDATE [DefaultSchemaSetting].[Promo] SET PromoStatusId = (SELECT Id FROM [DefaultSchemaSetting].PromoStatus WHERE SystemName = 'DraftPublished' AND Disabled = 0) WHERE Number = {0} \n";
                    string updateScript = "";

                    foreach (var promoNumber in promoNumbers)
                    {
                        int number;
                        if (int.TryParse(promoNumber, out number))
                        {
                            var promo = activePromo.FirstOrDefault(x => x.Number == number);
                            if (promo != null)
                            {
                                promoesToRecalculate.Add(promo);
                                updateScript += String.Format(formatStrPromo, promo.Number);
                            }
                            else
                            {
                                handlerLogger.Write(true, $"The promo with { number } number not found.", "Error");
                            }
                        }
                        else
                        {
                            handlerLogger.Write(true, $"The { number } is not a number.", "Error");
                        }
                    }

                    if (!String.IsNullOrEmpty(updateScript))
                    {
                        prev_context.ExecuteSqlCommand(updateScript);
                    }
                }

                using (var context = new DatabaseContext())
                {
                    var activePromo = context.Set<Promo>().Where(x => !x.Disabled);
                    var promoIdsToRecalculate = promoesToRecalculate.Select(y => y.Id);
                    var query = activePromo.Where(x => promoIdsToRecalculate.Contains(x.Id));
                    var promoesList = query.ToList();

                    if (promoesList.Count > 0)
                    {
                        foreach(var promo in promoesList)
                        {
                            handlerLogger.Write(true, String.Format("Calculating promo: №{0}", promo.Number), "Message");
                            handlerLogger.Write(true, "");

                            var promoId = promo.Id;

                            //статусы, в которых не должен производиться пересчет плановых параметров промо
                            string promoStatusesNotPlanRecalculateSetting = settingsManager.GetSetting<string>("NOT_PLAN_RECALCULATE_PROMO_STATUS_LIST", "Started,Finished,Closed");
                            string[] statuses = promoStatusesNotPlanRecalculateSetting.Split(',');
                            string calculateBaselineError = null;
                            bool needReturnToOnApprovalStatus = false;

                            //Считаем Baseline до расчета Uplift
                            if (!statuses.Contains(promo.PromoStatus.SystemName))
                            {
                                if (!promo.LoadFromTLC)
                                {
                                    string setPromoProductError;

                                    needReturnToOnApprovalStatus = PlanProductParametersCalculation.SetPromoProduct(promoId, context, out setPromoProductError);
                                    if (setPromoProductError != null)
                                    {
                                        logLine = String.Format("Error filling Product: {0}", setPromoProductError);
                                        handlerLogger.Write(true, logLine, "Error");
                                    }

                                    calculateBaselineError = PlanProductParametersCalculation.CalculateBaseline(context, promoId);
                                }
                            }

                            //Подбор исторических промо и расчет PlanPromoUpliftPercent
                            // Uplift не расчитывается для промо в статусе Starte, Finished, Closed
                            if (promo.PromoStatus.SystemName != "Started" && promo.PromoStatus.SystemName != "Finished" && promo.PromoStatus.SystemName != "Closed")
                            {
                                Stopwatch swUplift = new Stopwatch();
                                swUplift.Start();
                                logLine = String.Format("Pick plan promo uplift started at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow));
                                handlerLogger.Write(true, logLine, "Message");

                                string upliftMessage;

                                double? planPromoUpliftPercent = PlanPromoUpliftCalculation.FindPlanPromoUplift(promoId, context, out upliftMessage, false, new Guid());

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
                                logLine = String.Format("Pick plan promo uplift completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), swUplift.Elapsed.TotalSeconds);
                                handlerLogger.Write(true, logLine, "Message");
                                handlerLogger.Write(true, "");
                            }
                            else
                            {
                                logLine = String.Format("{0}", "Plan promo uplift is not picking for started, finished and closed promo.");
                                handlerLogger.Write(true, logLine, "Message");
                            }

                            // Плановые параметры не расчитывается для промо в статусах из настроек
                            if (!statuses.Contains(promo.PromoStatus.SystemName))
                            {
                                //Pасчет плановых параметров Product и Promo
                                Stopwatch swPromoProduct = new Stopwatch();
                                swPromoProduct.Start();
                                logLine = String.Format("Calculation of plan parameters began at {0:yyyy-MM-dd HH:mm:ss}. It may take some time.", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow));
                                handlerLogger.Write(true, logLine, "Message");

                                string calculateError = null;
                                if (!promo.LoadFromTLC)
                                {
                                    // пересчет baseline должен происходить до попытки согласовать промо, т.к. к зависимости от результата пересчета
                                    // резльтат согласования может быть разный (после пересчета baseline может оказаться равен 0, тогда автосогласования не будет)
                                    calculateError = PlanProductParametersCalculation.CalculatePromoProductParameters(promoId, context);
                                    if (calculateBaselineError != null && calculateError != null)
                                    {
                                        calculateError += calculateBaselineError;
                                    }
                                    else if (calculateBaselineError != null && calculateError == null)
                                    {
                                        calculateError = calculateBaselineError;
                                    }
                                }

                                if (calculateError != null)
                                {
                                    logLine = String.Format("Error when calculating the planned parameters of products: {0}", calculateError);
                                    handlerLogger.Write(true, logLine, "Error");
                                }

                                // пересчет плановых бюджетов (из-за LSV)
                                BudgetsPromoCalculation.CalculateBudgets(promo, true, false, handlerLogger.CurrentLogWriter, info.HandlerId, context);

                                BTL btl = context.Set<BTLPromo>().Where(x => x.PromoId == promo.Id && !x.Disabled && x.DeletedDate == null).FirstOrDefault()?.BTL;
                                if (btl != null)
                                {
                                    BudgetsPromoCalculation.CalculateBTLBudgets(btl, true, false, handlerLogger.CurrentLogWriter, context);
                                }

                                calculateError = PlanPromoParametersCalculation.CalculatePromoParameters(promoId, context);

                                if (calculateError != null)
                                {
                                    logLine = String.Format("Error when calculating the plan parameters Promo: {0}", calculateError);
                                    handlerLogger.Write(true, logLine, "Error");
                                }

                                swPromoProduct.Stop();
                                logLine = String.Format("Calculation of plan parameters was completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), swPromoProduct.Elapsed.TotalSeconds);
                                handlerLogger.Write(true, logLine, "Message");
                                handlerLogger.Write(true, "");
                            }
                            else
                            {
                                logLine = String.Format("Plan parameters don't recalculate for promo in statuses: {0}", promoStatusesNotPlanRecalculateSetting);
                                handlerLogger.Write(true, logLine, "Warning");
                            }
                           
                            //promo.Calculating = false;
                            context.SaveChanges();
                        }
                    }
                    else
                    {
                        handlerLogger.Write(true, "No promo found.", "Error");
                    }
                }
            }
            catch (Exception e)
            {
                data.SetValue<bool>("HasErrors", true);
                logger.Error(e);

                if (handlerLogger != null)
                {
                    handlerLogger.Write(true, e.ToString(), "Error");
                }
            }
            finally
            {
                logger.Debug("Finish '{0}'", info.HandlerId);
                sw.Stop();

                if (handlerLogger != null)
                {
                    handlerLogger.Write(true, String.Format("Plan parameters recalculation ended at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), sw.Elapsed.TotalSeconds), "Message");
                    handlerLogger.UploadToBlob();
                }
            }
        }
    }
}
