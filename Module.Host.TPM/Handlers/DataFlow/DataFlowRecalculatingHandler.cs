using Core.Dependency;
using Core.Settings;
using Interfaces.Core.Common;
using Looper.Core;
using Looper.Parameters;
using Module.Host.TPM.Actions;
using Module.Host.TPM.Actions.Notifications;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model;
using ProcessingHost.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.LogWriter;

namespace Module.Host.TPM.Handlers.DataFlow
{
    /// <summary>
    /// Класс для запуска перерасчета параметров промо (DataFlow)
    /// </summary>
    public class DataFlowRecalculatingHandler : BaseHandler
    {
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                ILogWriter handlerLogger = null;
                string logLine = null;
                string message = null;
                Stopwatch sw = new Stopwatch();
                Stopwatch swDataFlow_Part01 = new Stopwatch();
                Stopwatch swDataFlow_Part02 = new Stopwatch();
                var success = true;
                sw.Start();
                try
                {
                    //добавляем новый тип тега
                    handlerLogger = new FileLogWriter(info.HandlerId.ToString(), 
                                                      new Dictionary<string, string>(){ ["Timing"] = "TIMING" });
                    handlerLogger.Write(true, String.Format("The recalculation of promo parameters began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");

                    //статусы, в которых не должен производиться пересчет плановых параметров промо
                    ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                    string promoStatusesNotPlanRecalculateSetting = settingsManager.GetSetting<string>("NOT_PLAN_RECALCULATE_PROMO_STATUS_LIST", "Started,Finished,Closed");
                    string[] statuses = promoStatusesNotPlanRecalculateSetting.Split(',');

                    //-------------DataFlow_Part01-----------------------------------
                    swDataFlow_Part01.Start();
                    handlerLogger.Write(true, String.Format("The recalculation of promo parameters (DataFlow Part 1) began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
                    List<Guid> promoIdsForRecalculating = HandlerDataHelper.GetIncomingArgument<List<Guid>>("PromoIdsForRecalculating", info.Data, false);
                    Guid RoleId = HandlerDataHelper.GetIncomingArgument<Guid>("RoleId", info.Data, false);
                    var role = context.Set<Role>().FirstOrDefault(x => x.Id == RoleId);
                    bool isSupportAdmin = (role != null && role.SystemName == "SupportAdministrator") ? true : false;
                    List<int?> promoNumbersForRecalculating = context.Set<Promo>().Where(x => promoIdsForRecalculating.Any(y => y == x.Id)).Select(x => x.Number).ToList();

                    handlerLogger.Write(true, String.Format("Promo numbers for recalculating in DataFLow Part 1: {0}", string.Join(", ", promoNumbersForRecalculating.ToArray())), "Message");
                    handlerLogger.Write(true, String.Format("Total amount of promo for recalculating in DataFLow Part 1: {0}", promoIdsForRecalculating.Count), "Message");

                    if (promoIdsForRecalculating.Count > 0)
                    {
                        Stopwatch swPromoPlanParameters_Part01 = new Stopwatch();
                        double totalSecondPromoCalculatingPart01 = 0;
                        double? minCalculatingDurationPart01 = null;
                        double? maxCalculatingDurationPart01 = null;
                        int? minCalculatinPromoNumber = 0;
                        int? maxCalculatinPromoNumber = 0;
                        int realPromoRecalculatingAmount = 0; //могут отобраться промо в неподходящих статусах

                        foreach (Guid promoId in promoIdsForRecalculating)
                        {
                            Promo promo = context.Set<Promo>().Where(x => x.Id == promoId).FirstOrDefault();
                            if (promo != null)
                            {
                                // возможно, придется это вернуть, НЕ УДАЛЯТЬ КОММЕНТАРИЙ
                                // если уже произошла отгрузка продукта, то перезаполнение таблицы PromoProduct не осуществляется
                                //if (promo.DispatchesStart.HasValue && promo.DispatchesStart.Value > DateTimeOffset.Now)
                                //{
                                //    string setPromoProductError;
                                //    PlanProductParametersCalculation.SetPromoProduct(promoId, context, out setPromoProductError);
                                //    if (setPromoProductError != null)
                                //    {
                                //        logLine = String.Format("Error filling Product: {0}", setPromoProductError);
                                //        handlerLogger.Write(true, logLine, "Error");
                                //    }
                                //}

                                // Плановые параметры не расчитывается для промо в статусах из настроек
                                if (!statuses.Contains(promo.PromoStatus.SystemName) && !promo.LoadFromTLC)
                                {
                                    swPromoPlanParameters_Part01.Restart();

                                    string setPromoProductError;
                                    bool needReturnToOnApprovalStatus = false;

                                    // в день старта промо продукты не переподбираются
                                    bool isStartToday = promo.StartDate != null &&
                                                        (promo.StartDate - ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)).Value.Days == 0;
                                    if (!isStartToday)
                                    {
                                        needReturnToOnApprovalStatus = PlanProductParametersCalculation.SetPromoProduct(promoId, context, out setPromoProductError);
                                        if (setPromoProductError != null)
                                        {
                                            logLine = String.Format("Error filling Product: {0}", setPromoProductError);
                                            handlerLogger.Write(true, logLine, "Error");
                                        }
                                    }

                                    // пересчет baseline должен происходить до попытки согласовать промо, т.к. к зависимости от результата пересчета
                                    // результат согласования может быть разный (после пересчета baseline может оказаться равен 0, тогда автосогласования не будет)
                                    string calculateBaselineError = null;
                                    calculateBaselineError = PlanProductParametersCalculation.CalculateBaseline(context, promoId);
                                    message = PlanProductParametersCalculation.CalculatePromoProductParameters(promoId, context);

                                    if (calculateBaselineError != null && message != null)
                                    {
                                        message += calculateBaselineError;
                                    }
                                    else if (calculateBaselineError != null && message == null)
                                    {
                                        message = calculateBaselineError;
                                    }

                                    string[] canBeReturnedToOnApproval = { "OnApproval", "Approved", "Planned" };
                                    // возврат в статус OnApproval при изменении набора продуктов(с проверкой NoNego)
                                    if (needReturnToOnApprovalStatus && canBeReturnedToOnApproval.Contains(promo.PromoStatus.SystemName) && !isSupportAdmin)
                                    {
                                        PromoStatus draftPublished = context.Set<PromoStatus>().First(x => x.SystemName.ToLower() == "draftpublished" && !x.Disabled);
                                        promo.PromoStatus = draftPublished;
                                        promo.PromoStatusId = draftPublished.Id;
                                        promo.IsAutomaticallyApproved = false;
                                        promo.IsCMManagerApproved = false;
                                        promo.IsDemandFinanceApproved = false;
                                        promo.IsDemandPlanningApproved = false;
                                        PromoStateContext promoStateContext = new PromoStateContext(context, promo);

                                        PromoStatus onApproval = context.Set<PromoStatus>().First(x => x.SystemName.ToLower() == "onapproval" && !x.Disabled);
                                        promo.PromoStatus = onApproval;
                                        promo.PromoStatusId = onApproval.Id;

                                        string statusChangeError;
                                        var status = promoStateContext.ChangeState(promo, "System", out statusChangeError);
                                        if (status)
                                        {
                                            //Сохранение изменения статуса
                                            var promoStatusChange = context.Set<PromoStatusChange>().Create<PromoStatusChange>();
                                            promoStatusChange.PromoId = promo.Id;
                                            promoStatusChange.StatusId = promo.PromoStatusId;
                                            promoStatusChange.Date = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).Value;

                                            context.Set<PromoStatusChange>().Add(promoStatusChange);
                                        }

                                        if (statusChangeError != null && statusChangeError != string.Empty)
                                        {
                                            logLine = String.Format("Error while changing status of promo: {0}", statusChangeError);
                                            handlerLogger.Write(true, logLine, "Error");
                                        }
                                    }
                                    
                                    if (message != null)
                                    {
                                        logLine = String.Format("Error when calculating the planned parameters of the Product: {0}", message);
                                        handlerLogger.Write(true, logLine, "Error");
                                    }

                                    swPromoPlanParameters_Part01.Stop();
                                    totalSecondPromoCalculatingPart01 += swPromoPlanParameters_Part01.Elapsed.TotalSeconds;

                                    if (!minCalculatingDurationPart01.HasValue || minCalculatingDurationPart01.Value > swPromoPlanParameters_Part01.Elapsed.TotalSeconds)
                                    {
                                        minCalculatingDurationPart01 = swPromoPlanParameters_Part01.Elapsed.TotalSeconds;
                                        minCalculatinPromoNumber = promo.Number;
                                    }

                                    if (!maxCalculatingDurationPart01.HasValue || maxCalculatingDurationPart01.Value < swPromoPlanParameters_Part01.Elapsed.TotalSeconds)
                                    {
                                        maxCalculatingDurationPart01 = swPromoPlanParameters_Part01.Elapsed.TotalSeconds;
                                        maxCalculatinPromoNumber = promo.Number;
                                    }

                                    realPromoRecalculatingAmount++;
                                    handlerLogger.Write(true, String.Format("Promo number: {0}. Duration: {1} seconds", promo.Number, swPromoPlanParameters_Part01.Elapsed.TotalSeconds), "Timing");
                                }
                            }
                        }

                        double averageSecond = realPromoRecalculatingAmount != 0 ? totalSecondPromoCalculatingPart01 / realPromoRecalculatingAmount : 0;
                        handlerLogger.Write(true, String.Format("Min duration (Product parameters DataFlow Part 1): {0}. Promo number: {1}.", minCalculatingDurationPart01 ?? 0, minCalculatinPromoNumber ?? 0), "Timing");
                        handlerLogger.Write(true, String.Format("Max duration (Product parameters DataFlow Part 1): {0}. Promo number: {1}.", maxCalculatingDurationPart01 ?? 0, maxCalculatinPromoNumber ?? 0), "Timing");
                        handlerLogger.Write(true, String.Format("Average duration (Product parameters DataFlow Part 1): {0}", averageSecond), "Timing");
                        handlerLogger.Write(true, String.Format("Total duration (Product parameters DataFlow Part 1): {0}", totalSecondPromoCalculatingPart01), "Timing");
                    }

                    List<Guid> promoIdsForBudgetRecalclating = HandlerDataHelper.GetIncomingArgument<List<Guid>>("PromoIdsForBudgetRecalclating", info.Data, false);
                    List<Guid> promoSupportIdsForRecalculating = new List<Guid>();

                    Stopwatch swBudgetParameters_Part01 = new Stopwatch();
                    double totalSecondPlanBudgetCalculatingPart01 = 0;
                    double? minPlanBudgetCalculatingDurationPart01 = null;
                    double? maxPlanBudgetCalculatingDurationPart01 = null;
                    int? minPlanCalculatinBudgetNumber = 0;
                    int? maxPlanCalculatinBudgetNumber = 0;

                    foreach (var promoId in promoIdsForBudgetRecalclating)
                    {
                        List<PromoSupportPromo> pspList = context.Set<PromoSupportPromo>().Where(x => x.PromoId == promoId && !x.Disabled).ToList();
                        foreach(var psp in pspList)
                        {
                            if (!promoSupportIdsForRecalculating.Contains(psp.PromoSupportId))
                            {
                                swBudgetParameters_Part01.Restart();

                                //пересчет плановых бюджетов
                                BudgetsPromoCalculation.CalculateBudgets(psp.PromoSupportId, true, false, context);
                                promoSupportIdsForRecalculating.Add(psp.PromoSupportId);

                                swBudgetParameters_Part01.Stop();
                                totalSecondPlanBudgetCalculatingPart01 += swBudgetParameters_Part01.Elapsed.TotalSeconds;

                                if (!minPlanBudgetCalculatingDurationPart01.HasValue || minPlanBudgetCalculatingDurationPart01.Value > swBudgetParameters_Part01.Elapsed.TotalSeconds)
                                {
                                    minPlanBudgetCalculatingDurationPart01 = swBudgetParameters_Part01.Elapsed.TotalSeconds;
                                    minPlanCalculatinBudgetNumber = psp.PromoSupport.Number;
                                }

                                if (!maxPlanBudgetCalculatingDurationPart01.HasValue || maxPlanBudgetCalculatingDurationPart01.Value < swBudgetParameters_Part01.Elapsed.TotalSeconds)
                                {
                                    maxPlanBudgetCalculatingDurationPart01 = swBudgetParameters_Part01.Elapsed.TotalSeconds;
                                    maxPlanCalculatinBudgetNumber = psp.PromoSupport.Number;
                                }

                                handlerLogger.Write(true, String.Format("Budget number: {0}. Duration: {1} seconds", psp.PromoSupport.Number, swBudgetParameters_Part01.Elapsed.TotalSeconds), "Timing");
                            }
                        }
                    }

                    if (promoSupportIdsForRecalculating.Count > 0)
                    {
                        double averageSecond = promoSupportIdsForRecalculating.Count != 0 ? totalSecondPlanBudgetCalculatingPart01 / promoSupportIdsForRecalculating.Count : 0;
                        handlerLogger.Write(true, String.Format("Min duration (Budgets DataFlow Part 1): {0}. Budget number: {1}.", minPlanBudgetCalculatingDurationPart01 ?? 0, minPlanCalculatinBudgetNumber ?? 0), "Timing");
                        handlerLogger.Write(true, String.Format("Max duration (Budgets DataFlow Part 1): {0}. Budget number: {1}.", maxPlanBudgetCalculatingDurationPart01 ?? 0, maxPlanCalculatinBudgetNumber ?? 0), "Timing");
                        handlerLogger.Write(true, String.Format("Average duration (Budgets DataFlow Part 1): {0}", averageSecond), "Timing");
                        handlerLogger.Write(true, String.Format("Total duration (Budgets DataFlow Part 1): {0}", totalSecondPlanBudgetCalculatingPart01), "Timing");
                    }

                    handlerLogger.Write(true, String.Format("Total amount of promo support for recalculating: {0}", promoSupportIdsForRecalculating.Count), "Message");
                    swDataFlow_Part01.Stop();
                    handlerLogger.Write(true, String.Format("The recalculation of promo parameters (DataFlow Part 1) ended at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", DateTimeOffset.Now, swDataFlow_Part01.Elapsed.TotalSeconds), "Message");

                    //-------------DataFlow_Part02-----------------------------------
                    
                    swDataFlow_Part02.Start();
                    handlerLogger.Write(true, String.Format("The recalculation of promo parameters (DataFlow Part 2) began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
                    List<Guid> promoIdsForAllRecalculating = HandlerDataHelper.GetIncomingArgument<List<Guid>>("PromoIdsForAllRecalculating", info.Data, false);
                    handlerLogger.Write(true, String.Format("Total amount of promo for recalculating in DataFLow Part 2: {0}", promoIdsForAllRecalculating.Count), "Message");

                    Stopwatch swPromoParameters_Part02 = new Stopwatch();
                    double totalSecondPromoCalculatingPart02 = 0;
                    double? minCalculatingDurationPart02 = null;
                    double? maxCalculatingDurationPart02 = null;
                    int? minCalculatinPromoNumberPart02 = 0;
                    int? maxCalculatinPromoNumberPart02 = 0;

                    foreach (var promoId in promoIdsForAllRecalculating)
                    {
                        Promo promo = context.Set<Promo>().Where(x => x.Id == promoId && !x.Disabled).FirstOrDefault();
                        if (promo != null)
                        {
                            logLine = String.Format("Recalculating promo (Plan promo parameters and actual product parameters) number: {0}", promo.Number);
                            handlerLogger.Write(true, logLine, "Message");

                            swPromoParameters_Part02.Restart();
                            // Плановые параметры не расчитывается для промо в статусах из настроек
                            if (!statuses.Contains(promo.PromoStatus.SystemName))
                            {
                                message = PlanPromoParametersCalculation.CalculatePromoParameters(promoId, context);
                                swPromoParameters_Part02.Stop();
                                if (message != null)
                                {
                                    logLine = String.Format("Error when calculating the planned parameters Promo: {0}", message);
                                    handlerLogger.Write(true, logLine, "Error");
                                }
                            }

                            // продуктовые параметры считаются только для промо не из TLC
                            // Фактические параметры могут быть рассчитаны только после завершения промо
                            // Продуктовые параметры считаем только, если были загружены Actuals
                            var promoProductList = context.Set<PromoProduct>().Where(x => x.PromoId == promo.Id && !x.Disabled).ToList();
                            if (!promo.LoadFromTLC && promo.PromoStatus.SystemName == "Finished" && promoProductList.Any(x => x.ActualProductPCQty.HasValue))
                            {
                                swPromoParameters_Part02.Start();
                                message = ActualProductParametersCalculation.CalculatePromoProductParameters(promo, context);
                                swPromoParameters_Part02.Stop();
                                // записываем ошибки если они есть
                                if (message != null)
                                {
                                    WriteErrorsInLog(handlerLogger, message);
                                }
                            }

                            totalSecondPromoCalculatingPart02 += swPromoParameters_Part02.Elapsed.TotalSeconds;

                            if (!minCalculatingDurationPart02.HasValue || minCalculatingDurationPart02.Value > swPromoParameters_Part02.Elapsed.TotalSeconds)
                            {
                                minCalculatingDurationPart02 = swPromoParameters_Part02.Elapsed.TotalSeconds;
                                minCalculatinPromoNumberPart02 = promo.Number;
                            }

                            if (!maxCalculatingDurationPart02.HasValue || maxCalculatingDurationPart02.Value < swPromoParameters_Part02.Elapsed.TotalSeconds)
                            {
                                maxCalculatingDurationPart02 = swPromoParameters_Part02.Elapsed.TotalSeconds;
                                maxCalculatinPromoNumberPart02 = promo.Number;
                            }

                            handlerLogger.Write(true, String.Format("Promo number: {0}. Duration: {1} seconds", promo.Number, swPromoParameters_Part02.Elapsed.TotalSeconds), "Timing");
                        }
                    }

                    if (promoIdsForAllRecalculating.Count > 0)
                    {
                        double averageSecond = promoIdsForAllRecalculating.Count != 0 ? totalSecondPromoCalculatingPart02 / promoIdsForAllRecalculating.Count : 0;
                        handlerLogger.Write(true, String.Format("Min duration (Promo parameters DataFlow Part 2): {0}. Promo number: {1}.", minCalculatingDurationPart02 ?? 0, minCalculatinPromoNumberPart02 ?? 0), "Timing");
                        handlerLogger.Write(true, String.Format("Max duration (Promo parameters DataFlow Part 2): {0}. Promo number: {1}.", maxCalculatingDurationPart02 ?? 0, maxCalculatinPromoNumberPart02 ?? 0), "Timing");
                        handlerLogger.Write(true, String.Format("Average duration (Promo parameters DataFlow Part 2): {0}", averageSecond), "Timing");
                        handlerLogger.Write(true, String.Format("Total duration (Promo parameters DataFlow Part 2: {0}", totalSecondPromoCalculatingPart02), "Timing");
                    }

                    Stopwatch swBudgetParameters_Part02 = new Stopwatch();
                    double totalSecondActualBudgetCalculatingPart01 = 0;
                    double? minActualBudgetCalculatingDurationPart01 = null;
                    double? maxActualBudgetCalculatingDurationPart01 = null;
                    int? minActualCalculatinBudgetNumber = 0;
                    int? maxActualCalculatinBudgetNumber = 0;

                    foreach (var promoSupportId in promoSupportIdsForRecalculating)
                    {
                        swBudgetParameters_Part02.Restart();
                        //пересчет фактических бюджетов
                        PromoSupport ps = context.Set<PromoSupport>().Where(x => x.Id == promoSupportId && !x.Disabled).FirstOrDefault();
                        BudgetsPromoCalculation.CalculateBudgets(promoSupportId, false, true, context);

                        swBudgetParameters_Part02.Stop();
                        totalSecondActualBudgetCalculatingPart01 += swBudgetParameters_Part01.Elapsed.TotalSeconds;

                        if (!minActualBudgetCalculatingDurationPart01.HasValue || minActualBudgetCalculatingDurationPart01.Value > swBudgetParameters_Part02.Elapsed.TotalSeconds)
                        {
                            minActualBudgetCalculatingDurationPart01 = swBudgetParameters_Part02.Elapsed.TotalSeconds;
                            minActualCalculatinBudgetNumber = ps.Number;
                        }

                        if (!maxActualBudgetCalculatingDurationPart01.HasValue || maxActualBudgetCalculatingDurationPart01.Value < swBudgetParameters_Part02.Elapsed.TotalSeconds)
                        {
                            maxActualBudgetCalculatingDurationPart01 = swBudgetParameters_Part02.Elapsed.TotalSeconds;
                            maxActualCalculatinBudgetNumber = ps.Number;
                        }

                        handlerLogger.Write(true, String.Format("Budget number: {0}. Duration: {1} seconds", ps.Number, swBudgetParameters_Part02.Elapsed.TotalSeconds), "Timing");
                    }

                    if (promoSupportIdsForRecalculating.Count > 0)
                    {
                        double averageSecond = promoSupportIdsForRecalculating.Count != 0 ? totalSecondActualBudgetCalculatingPart01 / promoSupportIdsForRecalculating.Count : 0;
                        handlerLogger.Write(true, String.Format("Min duration (Budgets DataFlow Part 2): {0}. Budget number: {1}.", minActualBudgetCalculatingDurationPart01 ?? 0, minActualCalculatinBudgetNumber ?? 0), "Timing");
                        handlerLogger.Write(true, String.Format("Max duration (Budgets DataFlow Part 2): {0}. Budget number: {1}.", maxActualBudgetCalculatingDurationPart01 ?? 0, maxActualCalculatinBudgetNumber ?? 0), "Timing");
                        handlerLogger.Write(true, String.Format("Average duration (Budgets DataFlow Part 2): {0}", averageSecond), "Timing");
                        handlerLogger.Write(true, String.Format("Total duration (Budgets DataFlow Part 2): {0}", totalSecondActualBudgetCalculatingPart01), "Timing");
                    }

                    Stopwatch swActualPromoParameters_Part02 = new Stopwatch();
                    double totalSecondActualPromoCalculatingPart02 = 0;
                    double? minActualCalculatingDurationPart02 = null;
                    double? maxActualCalculatingDurationPart02 = null;
                    int? minActualCalculatinPromoNumberPart02 = 0;
                    int? maxActualCalculatinPromoNumberPart02 = 0;

                    foreach (var promoId in promoIdsForAllRecalculating)
                    {
                        Promo promo = context.Set<Promo>().Where(x => x.Id == promoId && !x.Disabled).FirstOrDefault();
                        var promoProductList = context.Set<PromoProduct>().Where(x => x.PromoId == promo.Id && !x.Disabled).ToList();
                        // Фактические параметры могут быть рассчитаны только после завершения промо
                        // Параметры промо считаем только, если промо из TLC или если были загружены Actuals
                        if (promo != null && promo.PromoStatus.SystemName == "Finished" && 
                           (promo.LoadFromTLC || promoProductList.Any(x => x.ActualProductPCQty.HasValue)))
                        {
                            logLine = String.Format("Recalculating promo (Actual promo parameters) number: {0}", promo.Number);
                            handlerLogger.Write(true, logLine, "Message");

                            swActualPromoParameters_Part02.Restart();
                            message = ActualPromoParametersCalculation.CalculatePromoParameters(promo, context);
                            swActualPromoParameters_Part02.Stop();
                            // записываем ошибки если они есть
                            if (message != null)
                            {
                                WriteErrorsInLog(handlerLogger, message);
                            }

                            totalSecondActualPromoCalculatingPart02 += swActualPromoParameters_Part02.Elapsed.TotalSeconds;

                            if (!minActualCalculatingDurationPart02.HasValue || minActualCalculatingDurationPart02.Value > swActualPromoParameters_Part02.Elapsed.TotalSeconds)
                            {
                                minActualCalculatingDurationPart02 = swActualPromoParameters_Part02.Elapsed.TotalSeconds;
                                minActualCalculatinPromoNumberPart02 = promo.Number;
                            }

                            if (!maxActualCalculatingDurationPart02.HasValue || maxActualCalculatingDurationPart02.Value < swActualPromoParameters_Part02.Elapsed.TotalSeconds)
                            {
                                maxActualCalculatingDurationPart02 = swActualPromoParameters_Part02.Elapsed.TotalSeconds;
                                maxActualCalculatinPromoNumberPart02 = promo.Number;
                            }

                            handlerLogger.Write(true, String.Format("Promo number: {0}. Duration: {1} seconds", promo.Number, swActualPromoParameters_Part02.Elapsed.TotalSeconds), "Timing");
                        }
                    }

                    if (promoIdsForAllRecalculating.Count > 0)
                    {
                        double averageSecond = promoIdsForAllRecalculating.Count != 0 ? totalSecondActualPromoCalculatingPart02 / promoIdsForAllRecalculating.Count : 0;
                        handlerLogger.Write(true, String.Format("Min duration (Actual promo parameters DataFlow Part 2): {0}. Promo number: {1}.", minActualCalculatingDurationPart02 ?? 0, minActualCalculatinPromoNumberPart02 ?? 0), "Timing");
                        handlerLogger.Write(true, String.Format("Max duration (Actual promo parameters DataFlow Part 2): {0}. Promo number: {1}.", maxActualCalculatingDurationPart02 ?? 0, maxActualCalculatinPromoNumberPart02 ?? 0), "Timing");
                        handlerLogger.Write(true, String.Format("Average duration (Actual promo parameters DataFlow Part 2): {0}", averageSecond), "Timing");
                        handlerLogger.Write(true, String.Format("Total duration (Actual promo parameters DataFlow Part 2): {0}", totalSecondActualPromoCalculatingPart02), "Timing");
                    }

                    swDataFlow_Part02.Stop();
                    handlerLogger.Write(true, String.Format("The recalculation of promo parameters (DataFlow Part 2) ended at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", DateTimeOffset.Now, swDataFlow_Part02.Elapsed.TotalSeconds), "Message");
                }
                catch (Exception e)
                {
                    success = false;
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
                        handlerLogger.Write(true, String.Format("The recalculation of promo parameters ended at {0:yyyy-MM-dd HH:mm:ss}. Duration part 1: {1} seconds. Duration part 2: {2} seconds. Total duration: {3} seconds", 
                            DateTimeOffset.Now, swDataFlow_Part01.Elapsed.TotalSeconds, swDataFlow_Part02.Elapsed.TotalSeconds, sw.Elapsed.TotalSeconds), "Message");
                    }
                    CalculationTaskManager.UnLockPromoForHandler(info.HandlerId);

                    if (success)
                    {
                        var changesIncidents = context.Set<ChangesIncident>().Where(x => x.ProcessDate == null && x.Disabled);
                        foreach (var changesIncident in changesIncidents)
                        {
                            changesIncident.ProcessDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                        }

                        var productChangeIncidents = context.Set<ProductChangeIncident>().Where(x => x.RecalculationProcessDate == null);
                        foreach (var productChangeIncident in productChangeIncidents)
                        {
                            productChangeIncident.RecalculationProcessDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                        }
                    }

                    if (context != null)
                    {
                        context.SaveChanges();
                        ((IDisposable)context).Dispose();
                    }
                }
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
