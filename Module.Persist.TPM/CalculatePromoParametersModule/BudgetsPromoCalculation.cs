using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.CalculatePromoParametersModule
{
    public class BudgetsPromoCalculation
    {
        public static string CalculateBudgets(PromoSupportPromo psp, bool calculatePlanCostTE, bool calculateFactCostTE, bool calculatePlanCostProd, bool calculateFactCostProd, DatabaseContext context)
        {
            try
            {
                RecalculatePlanValue(psp, calculatePlanCostTE, calculatePlanCostProd, context);
                RecalculateFactValue(psp, calculateFactCostTE, calculateFactCostProd, context);
                context.SaveChanges();

                return null;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
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
            // при смене длительности пересчитывать необходимо только x-sites
            PromoSupportPromo[] promoSupports = context.Set<PromoSupportPromo>().Where(n => n.PromoId == promoId && !n.Disabled
                && n.PromoSupport.BudgetSubItem.BudgetItem.Budget.Name.ToLower().IndexOf("marketing") >= 0
                && n.PromoSupport.BudgetSubItem.BudgetItem.Name.ToLower().IndexOf("x-sites") >= 0
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
        /// <param name="promoSupportPromoIds">Список ID подстатей/промо</param>
        /// <param name="calculatePlanCostTE">True, если необходимо пересчитать распределение подстатьи</param>
        /// <param name="context">Контекст БД</param>
        /// <returns></returns>
        public static List<Guid> GetLinkedPromoId(string promoSupportPromoIds, bool calculatePlanCostTE, DatabaseContext context)
        {
            // распарсенный список ID подстатей/промо
            List<Guid> subItemsIdsList = new List<Guid>();
            // список промо ID, участвующих в расчетах
            List<Guid> promoIds = new List<Guid>();

            if (promoSupportPromoIds != null && promoSupportPromoIds.Length > 0)
            {
                subItemsIdsList = promoSupportPromoIds.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(n => Guid.Parse(n)).ToList();

                // список подстатей/промо
                PromoSupportPromo[] psps = context.Set<PromoSupportPromo>().Where(n => subItemsIdsList.Any(m => m == n.Id)).ToArray();

                foreach (PromoSupportPromo psp in psps)
                {
                    // при создании нет маппа, поэтому грузим явно
                    PromoSupport promoSupport = psp.PromoSupport ?? context.Set<PromoSupport>().Find(psp.PromoSupportId);
                    // при изменении списка прикрепленных x-sites или catalog необходимо пересчитать распределение бюджетов
                    if (calculatePlanCostTE && promoSupport.BudgetSubItem.BudgetItem.Name.ToLower().IndexOf("posm") < 0)
                    {
                        List<Promo> promoLinked = new List<Promo>();

                        // Промо из списка
                        promoLinked.AddRange(psps.Select(n => n.Promo));
                        // Связанные промо
                        promoLinked.AddRange(context.Set<PromoSupportPromo>().Where(n => n.PromoSupportId == psp.PromoSupportId
                            && n.Id != psp.Id && !n.Disabled).Select(n => n.Promo));

                        // страховка от повторений, сразу при включении
                        foreach (Promo p in promoLinked)
                        {
                            if (!promoIds.Contains(p.Id))
                                promoIds.Add(p.Id);
                        }
                    }
                    else
                    {
                        // страховка от повторений, сразу при включении
                        if (!promoIds.Contains(psp.PromoId))
                            promoIds.Add(psp.PromoId);
                    }
                }
            }

            return promoIds;
        }

        /// <summary>
        /// Пересчитать плановые значения
        /// </summary>
        /// <param name="psp">Подстатья</param>
        /// <param name="recalculateCostTE">Необходимо ли пересчитывать значения Cost TE</param>
        /// <param name="recalculateCostProd">Необходимо ли пересчитывать значения Cost Production</param>
        private static void RecalculatePlanValue(PromoSupportPromo psp, bool recalculateCostTE, bool recalculateCostProd, DatabaseContext context)
        {
            // у контекста проблемы с маппингом при создании            
            Promo promo = context.Set<Promo>().Find(psp.PromoId);
            PromoSupport promoSupport = context.Set<PromoSupport>().Find(psp.PromoSupportId);

            // защита от возможных будущих реализаций
            if (promoSupport.BudgetSubItem.BudgetItem.Budget.Name.ToLower().IndexOf("marketing") >= 0)
            {
                if (recalculateCostTE)
                {
                    if (psp.PromoSupport.BudgetSubItem.BudgetItem.Name.ToLower().IndexOf("posm") >= 0)
                        UpdateMarketingTI(new Guid[] { psp.PromoId }, context);
                    else
                        CalculateMarketingTI(psp.PromoSupportId, psp.PromoId, context);
                }

                if (recalculateCostProd)
                {
                    PromoSupportPromo[] subItems = context.Set<PromoSupportPromo>().Where(n => n.PromoId == psp.PromoId && !n.Disabled
                        && n.PromoSupport.BudgetSubItem.BudgetItem.Budget.Id == promoSupport.BudgetSubItem.BudgetItem.Budget.Id).ToArray();

                    PromoSupportPromo[] xsites = subItems.Where(n => n.PromoSupport.BudgetSubItem.BudgetItem.Name.ToLower().IndexOf("x-sites") >= 0).ToArray();
                    PromoSupportPromo[] catalog = subItems.Where(n => n.PromoSupport.BudgetSubItem.BudgetItem.Name.ToLower().IndexOf("catalog") >= 0).ToArray();
                    PromoSupportPromo[] posm = subItems.Where(n => n.PromoSupport.BudgetSubItem.BudgetItem.Name.ToLower().IndexOf("posm") >= 0).ToArray();

                    promo.PlanPromoCostProdXSites = xsites.Length > 0 ? xsites.Sum(n => n.PlanCostProd) : new double?();
                    promo.PlanPromoCostProdCatalogue = catalog.Length > 0 ? catalog.Sum(n => n.PlanCostProd) : new double?();
                    promo.PlanPromoCostProdPOSMInClient = posm.Length > 0 ? posm.Sum(n => n.PlanCostProd) : new double?();
                    promo.PlanPromoCostProduction = subItems.Length > 0 ? subItems.Sum(n => n.PlanCostProd) : new double?();
                }
            }
        }

        /// <summary>
        /// Пересчитать фактические значения
        /// </summary>
        /// <param name="psp">Подстатья</param>
        /// <param name="recalculateCostTE">Необходимо ли перерассчитывать значения Cost TE</param>
        /// <param name="recalculateCostProd">Необходимо ли перерассчитывать значения Cost Production</param>
        private static void RecalculateFactValue(PromoSupportPromo psp, bool recalculateCostTE, bool recalculateCostProd, DatabaseContext context)
        {
            // у контекста проблемы с маппингом при создании            
            Promo promo = context.Set<Promo>().Find(psp.PromoId);
            PromoSupport promoSupport = context.Set<PromoSupport>().Find(psp.PromoSupportId);

            PromoSupportPromo[] subItems = context.Set<PromoSupportPromo>().Where(n => n.PromoId == psp.PromoId && !n.Disabled
                && n.PromoSupport.BudgetSubItem.BudgetItem.Budget.Id == promoSupport.BudgetSubItem.BudgetItem.Budget.Id).ToArray();

            // защита от возможных будущих реализаций
            if (promoSupport.BudgetSubItem.BudgetItem.Budget.Name.ToLower().IndexOf("marketing") >= 0)
            {
                PromoSupportPromo[] xsites = subItems.Where(n => n.PromoSupport.BudgetSubItem.BudgetItem.Name.ToLower().IndexOf("x-sites") >= 0).ToArray();
                PromoSupportPromo[] catalog = subItems.Where(n => n.PromoSupport.BudgetSubItem.BudgetItem.Name.ToLower().IndexOf("catalog") >= 0).ToArray();
                PromoSupportPromo[] posm = subItems.Where(n => n.PromoSupport.BudgetSubItem.BudgetItem.Name.ToLower().IndexOf("posm") >= 0).ToArray();

                if (recalculateCostTE)
                {
                    promo.ActualPromoXSites = xsites.Length > 0 ? xsites.Sum(n => n.FactCalculation) : new double?();
                    promo.ActualPromoCatalogue = catalog.Length > 0 ? catalog.Sum(n => n.FactCalculation) : new double?();
                    promo.ActualPromoPOSMInClient = posm.Length > 0 ? posm.Sum(n => n.FactCalculation) : new double?();
                    promo.ActualPromoTIMarketing = subItems.Length > 0 ? subItems.Sum(n => n.FactCalculation) : new double?();
                }

                if (recalculateCostProd)
                {
                    promo.ActualPromoCostProdXSites = xsites.Length > 0 ? xsites.Sum(n => n.FactCostProd) : new double?();
                    promo.ActualPromoCostProdCatalogue = catalog.Length > 0 ? catalog.Sum(n => n.FactCostProd) : new double?();
                    promo.ActualPromoCostProdPOSMInClient = posm.Length > 0 ? posm.Sum(n => n.FactCostProd) : new double?();
                    promo.ActualPromoCostProduction = subItems.Length > 0 ? subItems.Sum(n => n.FactCostProd) : new double?();
                }
            }
        }

        /// <summary>
        /// Пересчитать распределенные значения для подстатьи Marketing и обновить Marketing TI для затронутых промо
        /// </summary>
        /// <param name="promoSupportId">Id PromoSupport, для которого производится перерасчет (должен относиться к Marketing)<</param>
        /// <param name="currentPromoId">Id Промо, послужившего причиной перерасчета</param>
        private static void CalculateMarketingTI(Guid promoSupportId, Guid currentPromoId, DatabaseContext context)
        {
            List<Guid> promoIds = CalculateMarketingSubItems(promoSupportId, currentPromoId, context);

            // Пересчитываем для затронутых промо Marketing TI (Сумма распределенных значений)
            UpdateMarketingTI(promoIds, context);
        }

        private static void UpdateMarketingTI(IEnumerable<Guid> promoIds, DatabaseContext context)
        {
            foreach (Guid promoId in promoIds)
            {
                Promo promo = context.Set<Promo>().Find(promoId);

                PromoSupportPromo[] subItems = context.Set<PromoSupportPromo>().Where(n => n.PromoId == promo.Id && !n.Disabled
                    && n.PromoSupport.BudgetSubItem.BudgetItem.Budget.Name.ToLower().IndexOf("marketing") >= 0).ToArray();

                PromoSupportPromo[] xsites = subItems.Where(n => n.PromoSupport.BudgetSubItem.BudgetItem.Name.ToLower().IndexOf("x-sites") >= 0).ToArray();
                PromoSupportPromo[] catalog = subItems.Where(n => n.PromoSupport.BudgetSubItem.BudgetItem.Name.ToLower().IndexOf("catalog") >= 0).ToArray();
                PromoSupportPromo[] posm = subItems.Where(n => n.PromoSupport.BudgetSubItem.BudgetItem.Name.ToLower().IndexOf("posm") >= 0).ToArray();

                promo.PlanPromoXSites = xsites.Length > 0 ? xsites.Sum(n => n.PlanCalculation) : new double?();
                promo.PlanPromoCatalogue = catalog.Length > 0 ? catalog.Sum(n => n.PlanCalculation) : new double?();
                promo.PlanPromoPOSMInClient = posm.Length > 0 ? posm.Sum(n => n.PlanCalculation) : new double?();
                promo.PlanPromoTIMarketing = subItems.Length > 0 ? subItems.Sum(n => n.PlanCalculation) : new double?();
            }

            context.SaveChanges();
        }

        /// <summary>
        /// Пересчитать распределенные значения для подстатьи Marketing
        /// </summary>
        /// <param name="promoSupportId">Id PromoSupport, относящийся к Marketing</param>
        /// <param name="currentPromoId">Id Промо, послужившего причиной перерасчета</param>
        /// <param name="context">Контекст БД</param>
        /// <returns>Список затронутых промо</returns>
        private static List<Guid> CalculateMarketingSubItems(Guid promoSupportId, Guid currentPromoId, DatabaseContext context)
        {
            // Выборка необходимых для перерасчета PromoSupportPromo
            // !!! Предполагается что PromoSupport для Marketing (RecalculatePlanValueCostTE есть проверка)
            PromoSupportPromo[] recalculatePromoSupportPromo = context.Set<PromoSupportPromo>().Where(n => n.PromoSupportId == promoSupportId && !n.Disabled).ToArray();

            // Подстатья может относиться только к одной статье
            if (recalculatePromoSupportPromo.Any(n => n.PromoSupport.BudgetSubItem.BudgetItem.Name.ToLower().IndexOf("x-sites") >= 0))
            {
                CalculateXsites(recalculatePromoSupportPromo);
            }
            else if (recalculatePromoSupportPromo.Any(n => n.PromoSupport.BudgetSubItem.BudgetItem.Name.ToLower().IndexOf("catalog") >= 0))
            {
                CalculateCatalog(recalculatePromoSupportPromo);
            }

            context.SaveChanges();

            // Список затронутых промо, для которых нужно пересчитать Marketing TI
            List<Guid> promoIds = recalculatePromoSupportPromo.GroupBy(n => n.PromoId).Select(n => n.Key).ToList();
            // если промо открепляется, то в списке его нет
            if (!promoIds.Contains(currentPromoId))
                promoIds.Add(currentPromoId);

            return promoIds;
        }

        /// <summary>
        /// Рассчитать x-sites
        /// </summary>
        /// <param name="recalculatePromoSupportPromo">Список для перерасчета</param>
        private static void CalculateXsites(PromoSupportPromo[] recalculatePromoSupportPromo)
        {
            // группируем по подстатьям
            var promoSupportGroups = recalculatePromoSupportPromo.GroupBy(n => n.PromoSupportId);

            // рассчитываем для каждой подстатьи отдельно
            foreach (var subItems in promoSupportGroups)
            {
                // ушедший бюджет на закрытые промо  
                double closedBudget = subItems.Where(n => n.Promo.PromoStatus.SystemName == "Closed").Sum(n => n.FactCalculation);
                // незакрытые промо
                PromoSupportPromo[] notClosedPromo = subItems.Where(n => n.Promo.PromoStatus.SystemName != "Closed").ToArray();

                // сумма длительностей промо из подстатьи
                int countDays = notClosedPromo.Select(n => n.Promo).Sum(n => (int)(n.EndDate - n.StartDate).Value.TotalDays);
                // стоимость за 1 день
                double costPerDay = Math.Round((notClosedPromo.First().PromoSupport.PlanCostTE.Value - closedBudget) / countDays, 2, MidpointRounding.AwayFromZero);

                // получаем и записываем распределенное значение (только незакрытые промо)
                foreach (PromoSupportPromo p in notClosedPromo)
                {
                    double sumInSubItem = costPerDay * (int)(p.Promo.EndDate - p.Promo.StartDate).Value.TotalDays;
                    p.PlanCalculation = sumInSubItem;
                }
            }
        }

        /// <summary>
        /// Рассчитать catalogs
        /// </summary>
        /// <param name="recalculatePromoSupportPromo">Список для перерасчета</param>
        private static void CalculateCatalog(PromoSupportPromo[] recalculatePromoSupportPromo)
        {
            // группируем по подстатьям
            var promoSupportGroups = recalculatePromoSupportPromo.GroupBy(n => n.PromoSupportId);

            // рассчитываем для каждой подстатьи отдельно
            foreach (var subItems in promoSupportGroups)
            {
                // ушедший бюджет на закрытые промо  
                double closedBudget = subItems.Where(n => n.Promo.PromoStatus.SystemName == "Closed").Sum(n => n.FactCalculation);
                // незакрытые промо
                PromoSupportPromo[] notClosedPromo = subItems.Where(n => n.Promo.PromoStatus.SystemName != "Closed").ToArray();

                // количество привязанных промо
                double countPromo = notClosedPromo.Count();

                // получаем и записываем распределенное значение
                foreach (PromoSupportPromo p in notClosedPromo)
                {
                    p.PlanCalculation = Math.Round((p.PromoSupport.PlanCostTE.Value - closedBudget) / countPromo, 2, MidpointRounding.AwayFromZero);
                }
            }
        }
    }
}
