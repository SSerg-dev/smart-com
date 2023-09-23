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
using Core.Settings;
using Core.Dependency;
using Module.Persist.TPM.PromoStateControl;
using Module.Persist.TPM.Utils;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.SimpleModel;

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
                LogWriter handlerLogger = null;
                Stopwatch sw = new Stopwatch();
                sw.Start();

                handlerLogger = new LogWriter(info.HandlerId.ToString());
                logLine = string.Format("The calculation of the parameters started at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow));
                handlerLogger.Write(true, logLine, "Message");
                handlerLogger.Write(true, "");

                Guid nullGuid = new Guid();
                Guid promoId = HandlerDataHelper.GetIncomingArgument<Guid>("PromoId", info.Data, false);
                Guid UserId = HandlerDataHelper.GetIncomingArgument<Guid>("UserId", info.Data, false);
                Guid RoleId = HandlerDataHelper.GetIncomingArgument<Guid>("RoleId", info.Data, false);
                bool needCalculatePlanMarketingTI = HandlerDataHelper.GetIncomingArgument<bool>("NeedCalculatePlanMarketingTI", info.Data, false);
                bool needResetUpliftCorrections = HandlerDataHelper.GetIncomingArgument<bool>("needResetUpliftCorrections", info.Data, false);
                bool needResetUpliftCorrectionsPI = HandlerDataHelper.GetIncomingArgument<bool>("needResetUpliftCorrectionsPI", info.Data, false);
                bool createDemandIncidentCreate = HandlerDataHelper.GetIncomingArgument<bool>("createDemandIncidentCreate", info.Data, false);
                bool createDemandIncidentUpdate = HandlerDataHelper.GetIncomingArgument<bool>("createDemandIncidentUpdate", info.Data, false);

                string oldMarsMechanic = HandlerDataHelper.GetIncomingArgument<string>("oldMarsMechanic", info.Data, false);
                double? oldMarsMechanicDiscount = HandlerDataHelper.GetIncomingArgument<double?>("oldMarsMechanicDiscount", info.Data, false);
                DateTimeOffset? oldDispatchesStart = HandlerDataHelper.GetIncomingArgument<DateTimeOffset?>("oldDispatchesStart", info.Data, false);
                double? oldPlanPromoUpliftPercent = HandlerDataHelper.GetIncomingArgument<double?>("oldPlanPromoUpliftPercent", info.Data, false);
                double? oldPlanPromoIncrementalLSV = HandlerDataHelper.GetIncomingArgument<double?>("oldPlanPromoIncrementalLSV", info.Data, false);
                Promo promoCopy = null;

                try
                {
                    if (promoId != nullGuid)
                    {
                        var role = context.Set<Role>().FirstOrDefault(x => x.Id == RoleId);
                        bool isSupportAdmin = (role != null && role.SystemName == "SupportAdministrator");
                        Promo promo = context.Set<Promo>().Include(x => x.PromoPriceIncrease).FirstOrDefault(x => x.Id == promoId);
                        promoCopy = AutomapperProfiles.PromoCopy(promo);

                        //добавление номера рассчитываемого промо в лог
                        handlerLogger.Write(true, string.Format("Calculating promo: №{0}", promo.Number), "Message");
                        handlerLogger.Write(true, "");

                        //статусы, в которых не должен производиться пересчет плановых параметров промо
                        ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                        string promoStatusesNotPlanRecalculateSetting = settingsManager.GetSetting<string>("NOT_PLAN_RECALCULATE_PROMO_STATUS_LIST", "Started,Finished,Closed");
                        string[] statuses = new string[0];
                        if (!isSupportAdmin)
                        {
                            statuses = promoStatusesNotPlanRecalculateSetting.Split(',');
                        }
                        string calculateBaselineError = null;
                        bool needReturnToOnApprovalStatus = false;

                        //Считаем Baseline до расчета Uplift
                        if (!statuses.Contains(promo.PromoStatus.SystemName))
                        {
                            if (!promo.LoadFromTLC)
                            {

                                needReturnToOnApprovalStatus = PlanProductParametersCalculation.SetPromoProduct(promoId, context, out string setPromoProductError);
                                if (setPromoProductError != null)
                                {
                                    logLine = string.Format("Error filling Product: {0}", setPromoProductError);
                                    handlerLogger.Write(true, logLine, "Error");
                                }
                                calculateBaselineError = PlanProductParametersCalculation.CalculateBaseline(context, promoId);
                                promo = context.Set<Promo>().FirstOrDefault(x => x.Id == promoId);
                            }
                        }
                        //Подбор исторических промо и расчет PlanPromoUpliftPercent
                        if (NeedUpliftFinding(promo))
                        {
                            // Uplift не расчитывается для промо в статусе Starte, Finished, Closed
                            if ((promo.PromoStatus.SystemName != "Started" && promo.PromoStatus.SystemName != "Finished" && promo.PromoStatus.SystemName != "Closed") || isSupportAdmin)
                            {
                                Stopwatch swUplift = new Stopwatch();
                                swUplift.Start();
                                logLine = string.Format("Pick plan promo uplift started at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow));
                                handlerLogger.Write(true, logLine, "Message");

                                string upliftMessage;

                                PlanUplift planPromoUpliftPercent = PlanPromoUpliftCalculation.FindPlanPromoUplift(promoId, context, out upliftMessage, needResetUpliftCorrections, UserId);

                                if (planPromoUpliftPercent.CountedPlanUplift != -1)
                                {
                                    logLine = string.Format("{0}: {1}", upliftMessage, planPromoUpliftPercent.CountedPlanUplift);
                                    handlerLogger.Write(true, logLine, "Message");
                                }
                                else
                                {
                                    logLine = string.Format("{0}", upliftMessage);
                                    handlerLogger.Write(true, logLine, "Message");
                                }

                                swUplift.Stop();
                                logLine = string.Format("Pick plan promo uplift completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), swUplift.Elapsed.TotalSeconds);
                                handlerLogger.Write(true, logLine, "Message");
                                handlerLogger.Write(true, "");
                            }
                            else
                            {
                                logLine = string.Format("{0}", "Plan promo uplift is not picking for started, finished and closed promo.");
                                handlerLogger.Write(true, logLine, "Message");
                            }
                        }
                        else
                        {
                            PlanPromoUpliftCalculation.DistributePlanPromoUpliftToProducts(promo, context, UserId);
                        }
                        if (promo.IsPriceIncrease && promo.TPMmode == Persist.TPM.Model.Interfaces.TPMmode.Current)
                        {
                            //Подбор исторических промо и расчет PlanPromoUpliftPercent PriceIncrease
                            if (!promo.NeedRecountUpliftPI && promoCopy.IsPriceIncrease)
                            {
                                // Uplift не расчитывается для промо в статусе Starte, Finished, Closed
                                if ((promo.PromoStatus.SystemName != "Started" && promo.PromoStatus.SystemName != "Finished" && promo.PromoStatus.SystemName != "Closed") || isSupportAdmin)
                                {
                                    Stopwatch swUplift = new Stopwatch();
                                    swUplift.Start();
                                    logLine = string.Format("Pick plan promoPriceIncrease uplift started at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow));
                                    handlerLogger.Write(true, logLine, "Message");

                                    string upliftMessage;

                                    PlanUplift planPromoUpliftPercent = PlanPromoUpliftCalculation.FindPlanPromoUpliftPI(promoId, context, out upliftMessage, needResetUpliftCorrectionsPI, UserId);

                                    if (planPromoUpliftPercent.CountedPlanUpliftPI != -1)
                                    {
                                        logLine = string.Format("{0}: {1}", upliftMessage, planPromoUpliftPercent.CountedPlanUpliftPI);
                                        handlerLogger.Write(true, logLine, "Message");
                                    }
                                    else
                                    {
                                        logLine = string.Format("{0}", upliftMessage);
                                        handlerLogger.Write(true, logLine, "Message");
                                    }

                                    swUplift.Stop();
                                    logLine = string.Format("Pick plan promoPriceIncrease  uplift completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), swUplift.Elapsed.TotalSeconds);
                                    handlerLogger.Write(true, logLine, "Message");
                                    handlerLogger.Write(true, "");
                                }
                                else
                                {
                                    logLine = string.Format("{0}", "Plan promoPriceIncrease  uplift is not picking for started, finished and closed promo.");
                                    handlerLogger.Write(true, logLine, "Message");
                                }
                            }
                            else // если выбран общий процент
                            {
                                PlanPromoUpliftCalculation.DistributePlanPromoUpliftToProductsPriceIncrease(promo, context, UserId);
                            }
                        }
                        else
                        {
                            //Подбор исторических промо и расчет PlanPromoUpliftPercent PriceIncrease
                            if (NeedUpliftFinding(promo))
                            {
                                // Uplift не расчитывается для промо в статусе Starte, Finished, Closed
                                if ((promo.PromoStatus.SystemName != "Started" && promo.PromoStatus.SystemName != "Finished" && promo.PromoStatus.SystemName != "Closed") || isSupportAdmin)
                                {
                                    Stopwatch swUplift = new Stopwatch();
                                    swUplift.Start();
                                    logLine = string.Format("Pick plan promoPriceIncrease uplift started at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow));
                                    handlerLogger.Write(true, logLine, "Message");

                                    string upliftMessage;

                                    PlanUplift planPromoUpliftPercent = PlanPromoUpliftCalculation.FindPlanPromoUpliftPI(promoId, context, out upliftMessage, needResetUpliftCorrections, UserId);

                                    if (planPromoUpliftPercent.CountedPlanUpliftPI != -1)
                                    {
                                        logLine = string.Format("{0}: {1}", upliftMessage, planPromoUpliftPercent.CountedPlanUpliftPI);
                                        handlerLogger.Write(true, logLine, "Message");
                                    }
                                    else
                                    {
                                        logLine = string.Format("{0}", upliftMessage);
                                        handlerLogger.Write(true, logLine, "Message");
                                    }

                                    swUplift.Stop();
                                    logLine = string.Format("Pick plan promoPriceIncrease  uplift completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), swUplift.Elapsed.TotalSeconds);
                                    handlerLogger.Write(true, logLine, "Message");
                                    handlerLogger.Write(true, "");
                                }
                                else
                                {
                                    logLine = string.Format("{0}", "Plan promoPriceIncrease  uplift is not picking for started, finished and closed promo.");
                                    handlerLogger.Write(true, logLine, "Message");
                                }
                            }
                            else // если выбран общий процент
                            {
                                PlanPromoUpliftCalculation.DistributePlanPromoUpliftToProductsPriceIncrease(promo, context, UserId);
                            }
                        }


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
                        if (!statuses.Contains(promo.PromoStatus.SystemName))
                        {
                            //Pасчет плановых параметров Product и Promo
                            Stopwatch swPromoProduct = new Stopwatch();
                            swPromoProduct.Start();
                            logLine = string.Format("Calculation of plan parameters began at {0:yyyy-MM-dd HH:mm:ss}. It may take some time.", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow));
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

                                string[] canBeReturnedToOnApproval = { "OnApproval", "Approved", "Planned" };
                                bool needReturnToOnApprovalStatusByZeroUplift =
                                    promo.PromoStatus.SystemName.ToLower() == "approved"
                                    && (!promo.PlanPromoUpliftPercent.HasValue || promo.PlanPromoUpliftPercent.Value == 0)
                                    && (!promo.InOut.HasValue || !promo.InOut.Value);

                                if (((needReturnToOnApprovalStatus && canBeReturnedToOnApproval.Contains(promo.PromoStatus.SystemName))
                                    || needReturnToOnApprovalStatusByZeroUplift)
                                    && !isSupportAdmin)
                                {
                                    PromoStatus draftPublished = context.Set<PromoStatus>().First(x => x.SystemName.ToLower() == "draftpublished" && !x.Disabled);
                                    promo.PromoStatus = draftPublished;
                                    promo.PromoStatusId = draftPublished.Id;
                                    promo.IsAutomaticallyApproved = false;
                                    promo.IsCMManagerApproved = false;
                                    promo.IsDemandFinanceApproved = false;
                                    promo.IsDemandPlanningApproved = false;
                                    promo.IsGAManagerApproved = false;
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
                                        logLine = string.Format("Error while changing status of promo: {0}", statusChangeError);
                                        handlerLogger.Write(true, logLine, "Error");
                                    }
                                }
                            }

                            if (calculateError != null)
                            {
                                logLine = string.Format("Error when calculating the planned parameters of products: {0}", calculateError);
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
                                logLine = string.Format("Error when calculating the plan parameters Promo: {0}", calculateError);
                                handlerLogger.Write(true, logLine, "Error");
                            }

                            swPromoProduct.Stop();
                            logLine = string.Format("Calculation of plan parameters was completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), swPromoProduct.Elapsed.TotalSeconds);
                            handlerLogger.Write(true, logLine, "Message");
                            handlerLogger.Write(true, "");
                        }
                        else
                        {
                            logLine = string.Format("Plan parameters don't recalculate for promo in statuses: {0}", promoStatusesNotPlanRecalculateSetting);
                            handlerLogger.Write(true, logLine, "Warning");
                        }

                        if (promo.PromoStatus.SystemName == "Finished" || (isSupportAdmin && promo.PromoStatus.SystemName == "Closed"))
                        {
                            Stopwatch swActual = new Stopwatch();
                            swActual.Start();
                            logLine = string.Format("The calculation of the actual parameters began at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow));
                            handlerLogger.Write(true, logLine, "Message");
                            handlerLogger.Write(true, "");

                            CalulateActual(promo, context, handlerLogger.CurrentLogWriter, info.HandlerId, isSupportAdmin);

                            swActual.Stop();
                            handlerLogger.Write(true, "");
                            logLine = string.Format("The calculation of the actual parameters was completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), swActual.Elapsed.TotalSeconds);
                            handlerLogger.Write(true, logLine, "Message");
                        }

                        if (PromoUtils.HasChanges(context.ChangeTracker, promo.Id))
                        {
                            promo.LastChangedDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                            if (PlanProductParametersCalculation.IsDemandChanged(promo, promoCopy))
                            {
                                promo.LastChangedDateDemand = promo.LastChangedDate;
                                promo.LastChangedDateFinance = promo.LastChangedDate;
                            }
                        }

                        if (createDemandIncidentCreate)
                        {
                            PromoHelper.WritePromoDemandChangeIncident(context, promo);
                        }
                        else if (createDemandIncidentUpdate)
                        {
                            PromoHelper.WritePromoDemandChangeIncident(context, promo, oldMarsMechanic, oldMarsMechanicDiscount, oldDispatchesStart, oldPlanPromoUpliftPercent, oldPlanPromoIncrementalLSV);
                        }
                        //promo.Calculating = false;
                        //promo.NeedRecountUpliftPI = false;
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
                    logLine = string.Format("Calculation of parameters completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), sw.Elapsed.TotalSeconds);
                    handlerLogger.Write(true, logLine, "Message");
                    handlerLogger.UploadToBlob();
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
        public static void CalulateActual(Promo promo, DatabaseContext context, ILogWriter handlerLogger, Guid handlerId,
                                        bool isSupportAdmin = false, bool calculateBudgets = true,
                                        bool useActualCOGS = false, bool useActualTI = false)
        {
            string errorString = null;
            // Продуктовые параметры считаем только, если были загружены Actuals
            var promoProductList = context.Set<PromoProduct>().Where(x => x.PromoId == promo.Id && !x.Disabled).ToList();
            if (!promo.LoadFromTLC && promoProductList.Any(x => x.ActualProductPCQty.HasValue))
            {
                // если есть ошибки, они перечисленны через ;
                errorString = ActualProductParametersCalculation.CalculatePromoProductParameters(promo, context, isSupportAdmin: isSupportAdmin);
                // записываем ошибки если они есть
                if (errorString != null)
                    WriteErrorsInLog(handlerLogger, errorString);
            }

            if (calculateBudgets)
            {
                // пересчет актуальных бюджетов (из-за LSV)
                BudgetsPromoCalculation.CalculateBudgets(promo, false, true, handlerLogger, handlerId, context);

                BTL btl = context.Set<BTLPromo>().Where(x => x.PromoId == promo.Id && !x.Disabled && x.DeletedDate == null).FirstOrDefault()?.BTL;
                if (btl != null)
                {
                    BudgetsPromoCalculation.CalculateBTLBudgets(btl, false, true, handlerLogger, context);
                }
            }

            // Параметры промо считаем только, если промо из TLC или если были загружены Actuals
            if (promo.LoadFromTLC || promoProductList.Any(x => x.ActualProductPCQty.HasValue))
            {
                errorString = ActualPromoParametersCalculation.CalculatePromoParameters(promo, context, useActualCOGS: useActualCOGS, useActualTI: useActualTI);
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
        private static void WriteErrorsInLog(ILogWriter handlerLogger, string errorString)
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