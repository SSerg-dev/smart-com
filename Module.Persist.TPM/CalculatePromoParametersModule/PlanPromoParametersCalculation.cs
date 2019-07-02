using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module.Persist.TPM.Model.TPM;
using Persist;
using Module.Persist.TPM.Utils.Filter;
using System.Data.Entity;

namespace Module.Persist.TPM.CalculatePromoParametersModule
{
    public class PlanPromoParametersCalculation
    {
        /// <summary>
        /// Метод для расчета плановых параметров Promo.
        /// </summary>
        /// <param name="promoId">Id создаваемого/редактируемого промо</param>
        /// <param name="context">Текущий контекст</param>
        public static string CalculatePromoParameters(Guid promoId, DatabaseContext context)
        {
            try
            {
                Promo promo = context.Set<Promo>().Where(x => x.Id == promoId && !x.Disabled).FirstOrDefault();
                ResetValues(promo, context);
                double? sumPlanProductBaseLineLSV = context.Set<PromoProduct>().Where(x => x.PromoId == promoId && !x.Disabled).Sum(x => x.PlanProductBaselineLSV);
                ClientTree clientTree = context.Set<ClientTree>().Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue).FirstOrDefault();

                //promo.PlanPromoBaselineLSV = sumPlanProductBaseLineLSV;
                //promo.PlanPromoIncrementalLSV = sumPlanProductBaseLineLSV * promo.PlanPromoUpliftPercent / 100;
                //promo.PlanPromoLSV = promo.PlanPromoBaselineLSV + promo.PlanPromoIncrementalLSV;  
                promo.PlanPromoTIShopper = promo.PlanPromoLSV * promo.MarsMechanicDiscount / 100;
                // бюджеты пересчитывать не требуется (пусть пока будет закомментировано)
                //promo.PlanPromoTIMarketing = promo.PlanPromoXSites + promo.PlanPromoCatalogue + promo.PlanPromoPOSMInClient;
                //promo.PlanPromoCostProduction = promo.PlanPromoCostProdXSites + promo.PlanPromoCostProdCatalogue + promo.PlanPromoCostProdPOSMInClient;
                promo.PlanPromoCost = (promo.PlanPromoTIShopper ?? 0) + (promo.PlanPromoTIMarketing ?? 0) + (promo.PlanPromoBranding ?? 0) + (promo.PlanPromoBTL ?? 0) + (promo.PlanPromoCostProduction ?? 0);

                string message = null;
                bool error;

                double? TIBasePercent = GetTIBasePercent(promo, context, out message, out error);
                if (message == null)
                {
                    promo.PlanPromoIncrementalBaseTI = promo.PlanPromoIncrementalLSV * TIBasePercent / 100;
                    double? COGSPercent = GetCOGSPercent(promo, context, out message);
                    if (message == null)
                    {
                        promo.PlanPromoIncrementalCOGS = promo.PlanPromoIncrementalLSV * COGSPercent / 100;

                        promo.PlanPromoBaseTI = promo.PlanPromoLSV * TIBasePercent / 100;
                        promo.PlanPromoTotalCost = promo.PlanPromoCost + promo.PlanPromoBaseTI;

                        // если стоит флаг inout, PlanPromoPostPromoEffect = 0
                        if (!promo.InOut.HasValue || !promo.InOut.Value)
                        {
                            if (clientTree != null)
                            {
                                //TODO: Уточнить насчет деления на 100
                                promo.PlanPromoPostPromoEffectLSVW1 = promo.PlanPromoBaselineLSV * clientTree.PostPromoEffectW1 / 100;
                                promo.PlanPromoPostPromoEffectLSVW2 = promo.PlanPromoBaselineLSV * clientTree.PostPromoEffectW2 / 100;
                                promo.PlanPromoPostPromoEffectLSV = promo.PlanPromoPostPromoEffectLSVW1 + promo.PlanPromoPostPromoEffectLSVW2;
                            }
                        }
                        else
                        {
                            promo.PlanPromoPostPromoEffectLSVW1 = 0;
                            promo.PlanPromoPostPromoEffectLSVW2 = 0;
                            promo.PlanPromoPostPromoEffectLSV = 0;
                        }

                        // плюс, так как Post Promo Effect отрицательный
                        promo.PlanPromoNetIncrementalLSV = promo.PlanPromoIncrementalLSV + promo.PlanPromoPostPromoEffectLSV;

                        promo.PlanPromoNetIncrementalCOGS = promo.PlanPromoNetIncrementalLSV * COGSPercent / 100;
                        promo.PlanPromoNetIncrementalBaseTI = promo.PlanPromoNetIncrementalLSV * TIBasePercent / 100;
                        promo.PlanPromoNetLSV = promo.PlanPromoBaselineLSV + promo.PlanPromoNetIncrementalLSV;

                        if (!promo.InOut.HasValue || !promo.InOut.Value)
                        {
                            promo.PlanPromoNetBaseTI = promo.PlanPromoNetLSV * TIBasePercent / 100;
                            promo.PlanPromoBaselineBaseTI = promo.PlanPromoBaselineLSV * TIBasePercent / 100;
                        }
                        else
                        {
                            promo.PlanPromoNetBaseTI = 0;
                            promo.PlanPromoBaselineBaseTI = 0;
                        }
                        
                        promo.PlanPromoNSV = (promo.PlanPromoLSV ?? 0) - (promo.PlanPromoTIShopper ?? 0) - (promo.PlanPromoTIMarketing ?? 0) - (promo.PlanPromoBaseTI ?? 0);
                        promo.PlanPromoNetNSV = (promo.PlanPromoNetLSV ?? 0) - (promo.PlanPromoTIShopper ?? 0) - (promo.PlanPromoTIMarketing ?? 0) - (promo.PlanPromoNetBaseTI ?? 0);

                        promo.PlanPromoIncrementalNSV = (promo.PlanPromoIncrementalLSV ?? 0) - (promo.PlanPromoTIShopper ?? 0) - (promo.PlanPromoTIMarketing ?? 0) - (promo.PlanPromoIncrementalBaseTI ?? 0);
                        promo.PlanPromoNetIncrementalNSV = (promo.PlanPromoNetIncrementalLSV ?? 0) - (promo.PlanPromoTIShopper ?? 0) - (promo.PlanPromoTIMarketing ?? 0) - (promo.PlanPromoIncrementalBaseTI ?? 0);

                        promo.PlanPromoIncrementalMAC = (promo.PlanPromoIncrementalNSV ?? 0) - (promo.PlanPromoIncrementalCOGS ?? 0);
                        promo.PlanPromoNetIncrementalMAC = (promo.PlanPromoNetIncrementalNSV ?? 0) - (promo.PlanPromoNetIncrementalCOGS ?? 0);

                        promo.PlanPromoIncrementalEarnings = (promo.PlanPromoIncrementalMAC ?? 0) - (promo.PlanPromoBranding ?? 0) - (promo.PlanPromoBTL ?? 0) - (promo.PlanPromoCostProduction ?? 0);
                        promo.PlanPromoNetIncrementalEarnings = (promo.PlanPromoNetIncrementalMAC ?? 0) - (promo.PlanPromoBranding ?? 0) - (promo.PlanPromoBTL ?? 0) - (promo.PlanPromoCostProduction ?? 0);

                        // +1 / -1 ?
                        promo.PlanPromoROIPercent = promo.PlanPromoCost != 0 ? (promo.PlanPromoIncrementalEarnings / promo.PlanPromoCost + 1) * 100 : 0;
                        promo.PlanPromoNetROIPercent = promo.PlanPromoCost != 0 ? (promo.PlanPromoNetIncrementalEarnings / promo.PlanPromoCost + 1) * 100 : 0;
                        promo.PlanPromoNetUpliftPercent = promo.PlanPromoBaselineLSV != 0 ? promo.PlanPromoNetIncrementalLSV / promo.PlanPromoBaselineLSV * 100 : 0;

                        context.SaveChanges();
                    }
                    else
                    {
                        return message;
                    }
                }
                else
                {
                    return message;
                }

                return null;
            }
            catch(Exception e)
            {
                return e.ToString();
            }
        }
    
        /// <summary>
        /// Для текущего клиента и брендтеха (если брендтеха нет, то его тоже берем) 
        /// нужно проссумировать проценты из записей TI, 
        /// если даты подходят под текущее промо.
        /// </summary>
        /// <param name="promo"></param>
        /// <param name="context"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static double? GetTIBasePercent(Promo promo, DatabaseContext context, out string message, out bool error)
        {
            error = false;
            try
            {
                // Список всех подошедших записей из таблицы TI
                List<TradeInvestment> tradeInvestments = null;
                // Сумма процентов всех подошедших записей из таблицы TI
                double percentSum = 0;

                // Получаем текущего клиента по ObjectId 
                ClientTree currentClient = context.Set<ClientTree>()
                    .Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue)
                    .FirstOrDefault();

                // Пока в отфильтрованном списке пусто и мы не достигли корневого элемента
                while ((tradeInvestments == null || tradeInvestments.Count() == 0) && currentClient.Type != "root")
                {
                    tradeInvestments = context.Set<TradeInvestment>()
                        // Фильтр по клиенту
                        .Where(x => x.ClientTreeId == currentClient.Id)
                        // Фильтр по брендтеху
                        .Where(x => (x.BrandTech == null || x.BrandTechId == promo.BrandTechId) && !x.Disabled)
                        // Фильтр по StartDate.
                        .Where(x => x.StartDate.HasValue && promo.StartDate.HasValue && DateTimeOffset.Compare(x.StartDate.Value, promo.StartDate.Value) <= 0)
                        // Фильтр по EndDate.
                        .Where(x => x.EndDate.HasValue && promo.EndDate.HasValue && DateTimeOffset.Compare(x.EndDate.Value, promo.EndDate.Value) >= 0).ToList();

                    currentClient = context.Set<ClientTree>().Where(x => x.ObjectId == currentClient.parentId && !x.EndDate.HasValue).FirstOrDefault();
                }

                if (tradeInvestments.Count() == 0)
                {
                    error = true;
                    message = GetMessageTiCogs("TI base was not found", promo, true, context);
                    return null;
                }
                else
                {
                    var tradeInvestmentsList = new List<TradeInvestment>(tradeInvestments);
                    bool containsDublicate = false;

                    // Если присутсвуют записи с пустым и заполненным брендтехом, берем только с заполненным
                    // при условии, что тип и подтип совпадают
                    if (tradeInvestments.Any(x => x.BrandTechId == null) && tradeInvestments.Any(x => x.BrandTechId != null))
                    {
                        tradeInvestmentsList = new List<TradeInvestment>();
                        // Группируем по типу и подтипу
                        var tradeInvestmentTypeSubtypeGroups = tradeInvestments.GroupBy(x => new { x.TIType, x.TISubType });

                        // Перебираем группы с ключами типом и подтипом
                        foreach (var tradeInvestmentTypeSubtypeGroup in tradeInvestmentTypeSubtypeGroups)
                        {
                            // Если в списке TI есть запись с пустым брендтехом
                            if (!containsDublicate && tradeInvestmentTypeSubtypeGroup.Any(x => x.BrandTechId == null))
                            {
                                error = false;
                                message = "TI base duplicate record warning";
                                containsDublicate = true;
                            }

                            // Формируем новый список TI записей (без пустых брендтехов)
                            tradeInvestmentsList.AddRange(tradeInvestmentTypeSubtypeGroup.Where(x => x.BrandTechId != null));
                        }
                    }

                    // Группируем записи по клиенту, брендтеху, типу, подтипу
                    var tradeInvestmentGroups = tradeInvestments.GroupBy(x => new { x.ClientTreeId, x.BrandTechId, x.TIType, x.TISubType })
                        .Where(x => x.Count() > 1)
                        .Select(x => x.Key);

                    // В группе не должно быть несколько элементов с одинаковым клиентом, брендтехом, типом, подтипом
                    if (tradeInvestmentGroups.Count() > 0)
                    {
                        error = true;
                        message = GetMessageTiCogs("TI base duplicate record error", promo, true, context);
                        return null;
                    }

                    // Суммируем все проценты подошедшых записей из TI
                    foreach (var tradeInvestment in tradeInvestmentsList)
                    {
                        percentSum += tradeInvestment.SizePercent;
                    }

                    message = null;
                    return percentSum;
                }
            }
            catch(Exception e)
            {
                error = true;
                message = e.ToString();
                return null;
            }
        }

        public static double? GetCOGSPercent(Promo promo, DatabaseContext context, out string message)
        {
            try
            {
                ClientTree clientNode = context.Set<ClientTree>().Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue).FirstOrDefault();
                List<COGS> cogsList = context.Set<COGS>().Where(x => x.ClientTreeId == clientNode.Id && x.BrandTechId == promo.BrandTechId && !x.Disabled).ToList();
                cogsList = cogsList.Where(x => x.StartDate.HasValue && promo.StartDate.HasValue && DateTimeOffset.Compare(x.StartDate.Value, promo.DispatchesStart.Value) <= 0).ToList();
                cogsList = cogsList.Where(x => x.EndDate.HasValue && promo.EndDate.HasValue && DateTimeOffset.Compare(x.EndDate.Value, promo.DispatchesEnd.Value) >= 0).ToList();
                clientNode = context.Set<ClientTree>().Where(x => x.ObjectId == clientNode.parentId && !x.EndDate.HasValue).FirstOrDefault();

                while (cogsList.Count == 0 && clientNode.Type != "root")
                {
                    cogsList = context.Set<COGS>().Where(x => x.ClientTreeId == clientNode.Id && x.BrandTechId == promo.BrandTechId && !x.Disabled).ToList();
                    cogsList = cogsList.Where(x => x.StartDate.HasValue && promo.StartDate.HasValue && DateTimeOffset.Compare(x.StartDate.Value, promo.DispatchesStart.Value) <= 0).ToList();
                    cogsList = cogsList.Where(x => x.EndDate.HasValue && promo.EndDate.HasValue && DateTimeOffset.Compare(x.EndDate.Value, promo.DispatchesEnd.Value) >= 0).ToList();
                    clientNode = context.Set<ClientTree>().Where(x => x.ObjectId == clientNode.parentId && !x.EndDate.HasValue).FirstOrDefault();
                }

                //если не найдено COGS для конкретного BranTech, ищем COGS с пустым BrandTech(пустое=любое)
                if (cogsList.Count == 0)
                {
                    clientNode = context.Set<ClientTree>().Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue).FirstOrDefault();
                    cogsList = context.Set<COGS>().Where(x => x.ClientTreeId == clientNode.Id && x.BrandTechId == null && !x.Disabled).ToList();
                    cogsList = cogsList.Where(x => x.StartDate.HasValue && promo.StartDate.HasValue && DateTimeOffset.Compare(x.StartDate.Value, promo.DispatchesStart.Value) <= 0).ToList();
                    cogsList = cogsList.Where(x => x.EndDate.HasValue && promo.EndDate.HasValue && DateTimeOffset.Compare(x.EndDate.Value, promo.DispatchesEnd.Value) >= 0).ToList();
                    clientNode = context.Set<ClientTree>().Where(x => x.ObjectId == clientNode.parentId && !x.EndDate.HasValue).FirstOrDefault();

                    while (cogsList.Count == 0 && clientNode.Type != "root")
                    {
                        cogsList = context.Set<COGS>().Where(x => x.ClientTreeId == clientNode.Id && x.BrandTechId == null && !x.Disabled).ToList();
                        cogsList = cogsList.Where(x => x.StartDate.HasValue && promo.StartDate.HasValue && DateTimeOffset.Compare(x.StartDate.Value, promo.DispatchesStart.Value) <= 0).ToList();
                        cogsList = cogsList.Where(x => x.EndDate.HasValue && promo.EndDate.HasValue && DateTimeOffset.Compare(x.EndDate.Value, promo.DispatchesEnd.Value) >= 0).ToList();
                        clientNode = context.Set<ClientTree>().Where(x => x.ObjectId == clientNode.parentId && !x.EndDate.HasValue).FirstOrDefault();
                    }
                }

                if (cogsList.Count == 0)
                {
                    message = GetMessageTiCogs("COGS was not found", promo, false, context);
                    return null;
                }
                else if (cogsList.Count > 1)
                {
                    message = GetMessageTiCogs("COGS duplicate record error", promo, false, context);
                    return null;
                }
                else
                {
                    message = null;
                    return cogsList[0].LVSpercent;
                }
            }
            catch (Exception e)
            {
                message = e.ToString();
                return null;
            }
        }

        /// <summary>
        /// Сформировать сообщения об ошибке подбора для TI или COGS
        /// </summary>
        /// <param name="baseMessage">Базовое сообщение</param>
        /// <param name="promo">Промо</param>
        /// <param name="ti">True если TI, False если COGS</param>
        /// <param name="context">Контекст БД</param>
        /// <returns></returns>
        private static string GetMessageTiCogs(string baseMessage, Promo promo, bool ti, DatabaseContext context)
        {
            BrandTech brandTech = context.Set<BrandTech>().FirstOrDefault(n => n.Id == promo.BrandTechId);            

            string result = baseMessage + " for client " + promo.ClientHierarchy;

            if (brandTech != null)
                result += " and BrandTech " + brandTech.Name;

            if (ti)
                result += " for the period from " + promo.StartDate.Value.ToString("dd.MM.yyyy") + " to " + promo.EndDate.Value.ToString("dd.MM.yyyy") + ".";
            else
                result += " for the period from " + promo.DispatchesStart.Value.ToString("dd.MM.yyyy") + " to " + promo.DispatchesEnd.Value.ToString("dd.MM.yyyy") + ".";


            return result;
        }

        /// <summary>
        /// Сбросить значения плановых полей
        /// </summary>
        /// <param name="promo">Промо</param>
        /// <param name="context">Контекст БД</param>
        private static void ResetValues(Promo promo, DatabaseContext context)
        {
            //promo.PlanPromoBaselineLSV = null;
            //promo.PlanPromoIncrementalLSV = null;
            //promo.PlanPromoLSV = null;
            promo.PlanPromoTIShopper = null;
            promo.PlanPromoCost = null;
            promo.PlanPromoIncrementalBaseTI = null;
            promo.PlanPromoNetIncrementalBaseTI = null;
            promo.PlanPromoIncrementalCOGS = null;
            promo.PlanPromoNetIncrementalCOGS = null;
            promo.PlanPromoTotalCost = null;
            promo.PlanPromoPostPromoEffectLSVW1 = null;
            promo.PlanPromoPostPromoEffectLSVW2 = null;
            promo.PlanPromoPostPromoEffectLSV = null;
            promo.PlanPromoNetIncrementalLSV = null;
            promo.PlanPromoNetLSV = null;
            promo.PlanPromoBaselineBaseTI = null;
            promo.PlanPromoBaseTI = null;
            promo.PlanPromoNetBaseTI = null;
            promo.PlanPromoNSV = null;
            promo.PlanPromoNetNSV = null;
            promo.PlanPromoIncrementalNSV = null;
            promo.PlanPromoNetIncrementalNSV = null;
            promo.PlanPromoIncrementalMAC = null;
            promo.PlanPromoNetIncrementalMAC = null;
            promo.PlanPromoIncrementalEarnings = null;
            promo.PlanPromoNetIncrementalEarnings = null;
            promo.PlanPromoROIPercent = null;
            promo.PlanPromoNetROIPercent = null;
            promo.PlanPromoNetUpliftPercent = null;
            context.SaveChanges();
        }
    }
}