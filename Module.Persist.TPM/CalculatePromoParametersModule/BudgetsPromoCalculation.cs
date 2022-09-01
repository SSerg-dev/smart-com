using Core.Dependency;
using Core.Settings;
using Looper.Core;
using Module.Persist.TPM.Model.Interfaces;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.LogWriter;

namespace Module.Persist.TPM.CalculatePromoParametersModule
{
    public class BudgetsPromoCalculation
    {
        /// <summary>
        /// Расчет бюджетов
        /// </summary>
        /// <param name="promoSupportId">ID подстать</param>
        /// <param name="plan">True, если необходимо считать плановые</param>
        /// <param name="actual">True, если необходимо считать фактические</param>
        /// <param name="context">Контекст БД</param>
        /// <returns></returns>
        public static string CalculateBudgets(Guid promoSupportId, bool plan, bool actual, DatabaseContext context)
        {
            try
            {
                CalculateFromLSV(promoSupportId, plan, actual, context);

                context.SaveChanges();

                return null;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        /// <summary>
        /// Расчет бюджетов при полном перерассчете промо
        /// </summary>
        /// <param name="promo">Промо</param>
        /// <param name="plan">True, если необходимо считать плановые</param>
        /// <param name="actual">True, если необходимо считать фактические</param>
        /// <param name="handlerLogger">Лог</param>
        /// <param name="handlerId">ID обработчика</param>
        /// <param name="context">Контекст БД</param>
        public static void CalculateBudgets(Promo promo, bool plan, bool actual, ILogWriter handlerLogger, Guid handlerId, DatabaseContext context)
        {
            string logLine = "";
            handlerLogger.Write(true, "");
            logLine = String.Format("The calculation of the budgets started at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow));
            handlerLogger.Write(true, logLine, "Message");

            // находим все подстатьи к которым привязано промо
            Guid[] promoSupportIds = context.Set<PromoSupportPromo>().Where(n => n.PromoId == promo.Id && !n.Disabled
                && n.PromoSupport.BudgetSubItem.BudgetItem.Budget.Name.ToLower().IndexOf("marketing") >= 0).Select(n => n.PromoSupportId).ToArray();

            // список промо, участвующих в расчете
            Promo[] promoes = CalculationTaskManager.GetBlockedPromo(handlerId, context);

            // копируем старые значения, чтобы определить нужно ли пересчитывать остальные параметры
            double?[] oldPlanMarketingTI = promoes.Select(n => n.PlanPromoTIMarketing).ToArray();
            double?[] oldActualMarketingTI = promoes.Select(n => n.ActualPromoTIMarketing).ToArray();
            double?[] oldPlanCostProd = promoes.Select(n => n.PlanPromoCostProduction).ToArray();
            double?[] oldActualCostProd = promoes.Select(n => n.ActualPromoCostProduction).ToArray();

            string promoNumbers = "";
            foreach (Promo p in promoes)
                promoNumbers += p.Number.Value + ", ";

            context.SaveChanges();
            logLine = String.Format("At the time of calculation, the following promo are blocked for editing: {0}", promoNumbers);
            handlerLogger.Write(true, logLine, "Message");

            // пересчитываем распределенные значения
            string error = "";

            try
            {
                foreach (Guid promoSupportId in promoSupportIds)
                {
                    error = CalculateBudgets(promoSupportId, plan, actual, context);

                    if (error != null)
                        throw new Exception();
                }
            }
            catch
            {
                logLine = String.Format("Error calculating planned Marketing TI: {0}", error);
                handlerLogger.Write(true, logLine, "Error");
            }

            logLine = String.Format("The calculation of the budgets completed");
            handlerLogger.Write(true, logLine, "Message");

            for (int i = 0; i < promoes.Length; i++)
            {
                Promo p = promoes[i];

                // для основного промо пересчет будет произведен в вызывающей функции
                if (p.Id != promo.Id)
                {
                    handlerLogger.Write(true, "");
                    logLine = String.Format("Calculation of parameters for promo № {0} started at {1:yyyy-MM-dd HH:mm:ss}", p.Number, DateTimeOffset.Now);
                    handlerLogger.Write(true, logLine, "Message");

                    if (oldPlanMarketingTI[i] != p.PlanPromoTIMarketing || oldPlanCostProd[i] != p.PlanPromoCostProduction)
                    {
                        string setPromoProductFieldsError = PlanPromoParametersCalculation.CalculatePromoParameters(p.Id, context);

                        if (setPromoProductFieldsError != null)
                        {
                            logLine = String.Format("Error when calculating the planned parameters of the PromoProduct table: {0}", setPromoProductFieldsError);
                            handlerLogger.Write(true, logLine, "Error");
                        }
                    }

                    var promoProductList = context.Set<PromoProduct>().Where(x => x.PromoId == promo.Id && !x.Disabled).ToList();
                    // Параметры промо считаем только, если промо из TLC или если были загружены Actuals
                    if ((oldActualMarketingTI[i] != p.ActualPromoTIMarketing || oldActualCostProd[i] != p.ActualPromoCostProduction) &&
                        (promo.LoadFromTLC || promoProductList.Any(x => x.ActualProductPCQty.HasValue)))
                    {
                        string errorString = ActualPromoParametersCalculation.CalculatePromoParameters(p, context);

                        // записываем ошибки если они есть
                        if (errorString != null)
                            WriteErrorsInLog(handlerLogger, errorString);
                    }

                    logLine = String.Format("Calculation of parameters for promo № {0} completed.", p.Number);
                    handlerLogger.Write(true, logLine, "Message");

                    context.SaveChanges();
                }
            }

            handlerLogger.Write(true, "");
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

        /// <summary>
        /// Получить список промо ID, участвующих в расчетах бюджетов
        /// </summary>
        /// <param name="promoId">ID промо, являющегося причиной пересчета</param>
        /// <param name="context">Контекст БД<</param>
        /// <returns></returns>
        public static List<Guid> GetLinkedPromoId(Guid promoId, DatabaseContext context)
        {
            // находим все подстатьи Marketing к которым привязано промо            
            PromoSupportPromo[] promoSupports = context.Set<PromoSupportPromo>().Where(n => n.PromoId == promoId && !n.Disabled
                && n.PromoSupport.BudgetSubItem.BudgetItem.Budget.Name.ToLower().IndexOf("marketing") >= 0
            ).ToArray();

            // список промо ID, участвующих в расчетах
            List<Guid> promoIds = new List<Guid>();

            foreach (PromoSupportPromo psp in promoSupports)
            {
                Promo[] promoLinked = context.Set<PromoSupportPromo>().Where(n => n.PromoSupportId == psp.PromoSupportId && !n.Disabled).Select(n => n.Promo).ToArray();

                // страховка от повторений, сразу при включении
                foreach (Promo p in promoLinked)
                {
                    if (!promoIds.Contains(p.Id))
                        promoIds.Add(p.Id);
                }
            }

            return promoIds;
        }

        /// <summary>
        /// Получить список промо ID, участвующих в расчетах бюджетов
        /// </summary>
        /// <param name="promoSupportIds">Список ID подстатей</param>
        /// <param name="unlinkedPromoIds">Список ID отвязанных промо от подстатьи</param>
        /// <param name="context">Контекст БД</param>
        /// <returns></returns>
        public static List<Guid> GetLinkedPromoId(string promoSupportIds, string unlinkedPromoIds, DatabaseContext context, TPMmode tPMmode = TPMmode.Current)
        {

            // список промо ID, участвующих в расчетах
            List<Guid> promoIds = new List<Guid>();

            if (promoSupportIds != null && promoSupportIds.Length > 0)
            {
                // распарсенный список ID подстатей
                List<Guid> promoSupportIdsList = new List<Guid>();
                promoSupportIdsList = promoSupportIds.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(n => Guid.Parse(n)).ToList();

                foreach (Guid promoSupportId in promoSupportIdsList)
                {
                    Promo[] promoes = context.Set<PromoSupportPromo>().Where(n => n.PromoSupportId == promoSupportId && !n.Disabled && n.TPMmode == tPMmode).Select(n => n.Promo).ToArray();

                    // страховка от повторений, сразу при включении
                    foreach (Promo p in promoes)
                    {
                        if (!promoIds.Contains(p.Id))
                            promoIds.Add(p.Id);
                    }
                }
            }

            // включаем также отвязанные от подстатьи промо
            if (unlinkedPromoIds != null && unlinkedPromoIds.Length > 0)
            {
                List<Guid> unlinkedPromoIdsList = new List<Guid>();
                unlinkedPromoIdsList = unlinkedPromoIds.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(n => Guid.Parse(n)).ToList();

                // страховка от повторений, сразу при включении
                foreach (Guid promoId in unlinkedPromoIdsList)
                {
                    if (!promoIds.Contains(promoId))
                        promoIds.Add(promoId);
                }
            }

            return promoIds;
        }

        /// <summary>
        /// Получить список промо ID, участвующих в расчетах бюджетов
        /// </summary>
        /// <param name="promoSupportIds">Список ID подстатей</param>
        /// <param name="context">Контекст БД</param>
        /// <returns></returns>
        public static List<Guid> GetLinkedPromoId(List<Guid> promoSupportIdsList, DatabaseContext context)
        {
            // список промо ID, участвующих в расчетах
            List<Guid> promoIds = new List<Guid>();

            foreach (Guid promoSupportId in promoSupportIdsList)
            {
                Promo[] promoes = context.Set<PromoSupportPromo>().Where(n => n.PromoSupportId == promoSupportId).Select(n => n.Promo).ToArray();

                // страховка от повторений, сразу при включении
                foreach (Promo p in promoes)
                {
                    if (!promoIds.Contains(p.Id))
                        promoIds.Add(p.Id);
                }
            }

            return promoIds;
        }

        /// <summary>
        /// Получить список промо ID, участвующих в расчетах бюджетов BTL
        /// </summary>
        /// <param name="btlId">Id BTL статьи</param>
        /// <param name="context">Контекст БД</param>
        /// <returns></returns>
        public static List<Guid> GetLinkedPromoId(string btlId, DatabaseContext context, List<Guid> unlinkedPromoIds = null, TPMmode tPMmode = TPMmode.Current)
        {
            List<Guid> promoIds = new List<Guid>();
            var guidBTLId = Guid.Empty;
            Guid.TryParse(btlId, out guidBTLId);

            if (guidBTLId != Guid.Empty)
            {
                promoIds = context.Set<BTLPromo>().Where(x => !x.Disabled && x.DeletedDate == null && x.BTLId == guidBTLId && x.TPMmode == tPMmode).Select(x => x.PromoId).ToList();

                if(unlinkedPromoIds != null)
                    promoIds.AddRange(unlinkedPromoIds);
            }
            return promoIds.Distinct().ToList();
        }

        /// <summary>
        /// Расчет бюждетов по LSV
        /// </summary>
        /// <param name="promoSupportId">ID подстатьи</param>
        /// <param name="plan">True, если необходимо считать плановые</param>
        /// <param name="actual">True, если необходимо считать фактические</param>
        /// <param name="context">Контекст БД</param>
        private static void CalculateFromLSV(Guid promoSupportId, bool plan, bool actual, DatabaseContext context)
        {
            PromoSupport ps = context.Set<PromoSupport>().Find(promoSupportId);
            PromoSupportPromo[] psps = context.Set<PromoSupportPromo>().Where(n => n.PromoSupportId == ps.Id && !n.Disabled).ToArray();

            Promo[] notClosedPromo = psps.Select(n => n.Promo).Where(n => n.PromoStatus.SystemName != "Closed").ToArray();

            PromoSupportPromo[] pspsNotClosed = psps.Where(n => n.Promo.PromoStatus.SystemName != "Closed").ToArray();

            double summPlanLSV = notClosedPromo.Where(n => n.PlanPromoLSV.HasValue).Sum(n => n.PlanPromoLSV.Value);
            //double summActualLSV = notClosedPromo.Where(n => n.ActualPromoLSVByCompensation.HasValue).Sum(n => n.ActualPromoLSVByCompensation.Value);

            // ушедший бюджет на закрытые промо  
            double closedBudgetMarketingTi = psps.Where(n => n.Promo.PromoStatus.SystemName == "Closed").Sum(n => n.FactCalculation);
            double closedBudgetCostProd = psps.Where(n => n.Promo.PromoStatus.SystemName == "Closed").Sum(n => n.FactCostProd);

            foreach (PromoSupportPromo psp in pspsNotClosed)
            {
                Promo promo = psp.Promo;

                double kPlan = promo.PlanPromoLSV.HasValue ? promo.PlanPromoLSV.Value / summPlanLSV : 0;

				if (double.IsNaN(kPlan))
				{
					kPlan = 0;
				}
                //double kActual = promo.ActualPromoLSVByCompensation.HasValue ? promo.ActualPromoLSVByCompensation.Value / summActualLSV : 0;

                if (plan)
                {
                    psp.PlanCalculation = Math.Round((ps.PlanCostTE.Value - closedBudgetMarketingTi) * kPlan, 2, MidpointRounding.AwayFromZero);
                    psp.PlanCostProd = Math.Round((ps.PlanProdCost.Value - closedBudgetCostProd) * kPlan, 2, MidpointRounding.AwayFromZero);
                }

                if (actual)
                {
                    psp.FactCalculation = Math.Round((ps.ActualCostTE.Value - closedBudgetMarketingTi) * kPlan, 2, MidpointRounding.AwayFromZero);
                    psp.FactCostProd = Math.Round((ps.ActualProdCost.Value - closedBudgetCostProd) * kPlan, 2, MidpointRounding.AwayFromZero);
                }
                RecalculateSummBudgets(promo.Id, context);
            }
        }

        /// <summary>
        /// Пересчитать фактические значения
        /// </summary>
        /// <param name="psp">Подстатья</param>
        /// <param name="recalculateCostTE">Необходимо ли перерассчитывать значения Cost TE</param>
        /// <param name="recalculateCostProd">Необходимо ли перерассчитывать значения Cost Production</param>
        public static void RecalculateSummBudgets(Guid promoId, DatabaseContext context)
        {
            // у контекста проблемы с маппингом при создании
            Promo promo = context.Set<Promo>().Find(promoId);

            PromoSupportPromo[] subItems = context.Set<PromoSupportPromo>().Where(n => n.PromoId == promoId && !n.Disabled
                && n.PromoSupport.BudgetSubItem.BudgetItem.Budget.Name.ToLower().IndexOf("marketing") >= 0
                && n.TPMmode == promo.TPMmode).ToArray();


            PromoSupportPromo[] xsites = subItems.Where(n => n.PromoSupport.BudgetSubItem.BudgetItem.Name.ToLower().IndexOf("x-sites") >= 0).ToArray();
            PromoSupportPromo[] catalog = subItems.Where(n => n.PromoSupport.BudgetSubItem.BudgetItem.Name.ToLower().IndexOf("catalog") >= 0).ToArray();
            PromoSupportPromo[] posm = subItems.Where(n => n.PromoSupport.BudgetSubItem.BudgetItem.Name.ToLower().IndexOf("posm") >= 0).ToArray();

            // Plan Marketing TI
            promo.PlanPromoXSites = xsites.Length > 0 ? xsites.Sum(n => n.PlanCalculation) : new double?();
            promo.PlanPromoCatalogue = catalog.Length > 0 ? catalog.Sum(n => n.PlanCalculation) : new double?();
            promo.PlanPromoPOSMInClient = posm.Length > 0 ? posm.Sum(n => n.PlanCalculation) : new double?();
            promo.PlanPromoTIMarketing = subItems.Length > 0 ? subItems.Sum(n => n.PlanCalculation) : new double?();

            // Actual Marketing TI
            promo.ActualPromoXSites = xsites.Length > 0 ? xsites.Sum(n => n.FactCalculation) : new double?();
            promo.ActualPromoCatalogue = catalog.Length > 0 ? catalog.Sum(n => n.FactCalculation) : new double?();
            promo.ActualPromoPOSMInClient = posm.Length > 0 ? posm.Sum(n => n.FactCalculation) : new double?();
            promo.ActualPromoTIMarketing = subItems.Length > 0 ? subItems.Sum(n => n.FactCalculation) : new double?();

            // Plan Cost Prod
            promo.PlanPromoCostProdXSites = xsites.Length > 0 ? xsites.Sum(n => n.PlanCostProd) : new double?();
            promo.PlanPromoCostProdCatalogue = catalog.Length > 0 ? catalog.Sum(n => n.PlanCostProd) : new double?();
            promo.PlanPromoCostProdPOSMInClient = posm.Length > 0 ? posm.Sum(n => n.PlanCostProd) : new double?();
            promo.PlanPromoCostProduction = subItems.Length > 0 ? subItems.Sum(n => n.PlanCostProd) : new double?();

            // Actual Cost Prod
            promo.ActualPromoCostProdXSites = xsites.Length > 0 ? xsites.Sum(n => n.FactCostProd) : new double?();
            promo.ActualPromoCostProdCatalogue = catalog.Length > 0 ? catalog.Sum(n => n.FactCostProd) : new double?();
            promo.ActualPromoCostProdPOSMInClient = posm.Length > 0 ? posm.Sum(n => n.FactCostProd) : new double?();
            promo.ActualPromoCostProduction = subItems.Length > 0 ? subItems.Sum(n => n.FactCostProd) : new double?();
        }

        /// <summary>
        /// Расчет бюджетов BTL
        /// </summary>
        /// <param name="btl">BTL статья</param>
        /// <param name="plan">True, если необходимо считать плановые</param>
        /// <param name="actual">True, если необходимо считать фактические</param>
        /// <param name="context">Контекст БД</param>
        /// <returns></returns>
        public static void CalculateBTLBudgets(BTL btl, bool plan, bool actual, ILogWriter handlerLogger, DatabaseContext context)
        {
            try
            {
                // Статусы, в которых промо необходимо отвязать от BTL
                var settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                string notAllowedBTLStatusesList = settingsManager.GetSetting<string>("NOT_ALLOWED_BTL_STATUS_LIST", "Draft,Cancelled,Deleted");

                var promoes = context.Set<BTLPromo>().Where(x => x.BTLId == btl.Id && !x.Disabled && x.DeletedDate == null).Select(x => x.Promo);
                var promoesToUnlink = promoes.Where(x => notAllowedBTLStatusesList.Contains(x.PromoStatus.SystemName));
                promoes = promoes.Except(promoesToUnlink);
                var closedPromoes = promoes.Where(x => x.PromoStatus.SystemName == "Closed");
                promoes = promoes.Except(closedPromoes);
                string logLine = string.Empty;

                if (handlerLogger != null)
                {
                    logLine = String.Format("The calculation of the BTL budgets started at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow));
                    handlerLogger.Write(true, logLine, "Message");
                }

                if (promoes.Any())
                {
                    double? promoSummPlanLSV = promoes.Any() ? promoes.Where(n => n.PlanPromoLSV.HasValue).Sum(n => n.PlanPromoLSV.Value) : 0;
                    double? closedBudgetBTL = closedPromoes.Any() ? closedPromoes.Where(n => n.ActualPromoBTL.HasValue).Sum(n => n.ActualPromoBTL.Value) : 0;
                    if (promoSummPlanLSV == null) promoSummPlanLSV = 0;
                    if (closedBudgetBTL == null) closedBudgetBTL = 0;

                    foreach (var promo in promoes)
                    {
                        double kPlan = promo.PlanPromoLSV.HasValue ? promo.PlanPromoLSV.Value / promoSummPlanLSV.Value : 0;

                        if (double.IsNaN(kPlan))
                            kPlan = 0;

                        if (plan)
                        {
                            promo.PlanPromoBTL = Math.Round((btl.PlanBTLTotal.Value - closedBudgetBTL.Value) * kPlan, 2, MidpointRounding.AwayFromZero);
                        }
                        if (actual)
                        {
                            promo.ActualPromoBTL = Math.Round((btl.ActualBTLTotal.Value - closedBudgetBTL.Value) * kPlan, 2, MidpointRounding.AwayFromZero);
                        }
                    }

                    foreach (var promoToUnlink in promoesToUnlink)
                    {
                        // Отвязываем промо от BTL
                        BTLPromo BTLPromoToUnlink = context.Set<BTLPromo>().Where(x => x.BTLId == btl.Id && x.PromoId == promoToUnlink.Id && !x.Disabled && x.DeletedDate == null).FirstOrDefault();

                        if (BTLPromoToUnlink != null)
                        {
                            BTLPromoToUnlink.Disabled = true;
                            BTLPromoToUnlink.DeletedDate = DateTimeOffset.Now;
                        }

                        // Обнуляем BTL бюджет для промо
                        promoToUnlink.PlanPromoBTL = 0;
                        promoToUnlink.ActualPromoBTL = 0;
                    }
                    context.SaveChanges();
                }

                if (handlerLogger != null)
                {
                    logLine = String.Format("The calculation of the BTL budgets completed at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow));
                    handlerLogger.Write(true, logLine, "Message");
                }
            }
            catch (Exception e)
            {
                if (handlerLogger != null)
                {
                    var logLine = String.Format("Error while BTL budgets calculation: ", e.Message);
                    handlerLogger.Write(true, logLine, "Error");
                }
                else
                {
                    throw e;
                }
            }
        }
    }
}
