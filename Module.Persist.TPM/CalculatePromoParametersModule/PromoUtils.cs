﻿using Core.Dependency;
using Core.Settings;
using Module.Persist.TPM.Model.SimpleModel;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Module.Persist.TPM.CalculatePromoParametersModule
{
    public static class PromoUtils
    {
        private static ISettingsManager ConfigSettingsManager
        {
            get
            {
                return (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
            }
        }

        public static bool NeedBackToOnApproval(Promo promo)
        {
            var backToOnApprovalDispatchDays = ConfigSettingsManager.GetSetting<int>("BACK_TO_ON_APPROVAL_DISPATCH_DAYS_COUNT", 7 * 8);

            bool isCorrectDispatchDifference = promo.DispatchesStart < promo.StartDate ?
                                               (promo.DispatchesStart - ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)).Value.Days >= backToOnApprovalDispatchDays :
                                               true;

            return !isCorrectDispatchDifference;
        }

        public static bool HasChanges(DbChangeTracker changeTracker, Guid promoId)
        {
            changeTracker.DetectChanges();
            var hasChanges = changeTracker.Entries<Promo>().Any(x => x.Entity.Id == promoId && x.State == EntityState.Modified);
            return hasChanges;
        }

        /// <summary>
        /// Для текущего клиента и брендтеха (если брендтеха нет, то его тоже берем) 
        /// нужно проссумировать проценты из записей TI, 
        /// если даты подходят под текущее промо.
        /// </summary>
        /// <param name="promo"></param>
        /// <param name="context"></param>
        /// <param name="query"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static double? GetTIBasePercent(SimplePromoTradeInvestment promo, List<BaseTradeInvestment> baseTradeInvestments, List<ClientTree> clientTrees, List<BrandTech> brandTeches, out string message, out bool error)
        {
            error = false;
            try
            {
                // Список всех подошедших записей из таблицы TI
                List<BaseTradeInvestment> tradeInvestments = new List<BaseTradeInvestment>();
                // Сумма процентов всех подошедших записей из таблицы TI
                double percentSum = 0;

                // Получаем текущего клиента по ObjectId 
                ClientTree currentClient = clientTrees
                    .Where(x => x.ObjectId == promo.ClientTreeObjectId && !x.EndDate.HasValue)
                    .FirstOrDefault();

                // Пока в отфильтрованном списке пусто и мы не достигли корневого элемента
                while (baseTradeInvestments != null && (tradeInvestments == null || tradeInvestments.Count() == 0) && currentClient != null && currentClient.Type != "root")
                {
                    tradeInvestments = baseTradeInvestments
                        // Фильтр по клиенту
                        .Where(x => x.ClientTreeId == currentClient.Id && !x.Disabled)
                        // Фильтр по брендтеху
                        .Where(x => x.BrandTech == null || x.BrandTech.BrandsegTechsub == promo.BrandTechName)
                        // promo start date должна лежать в интервале между TI start date и TI end date
                        .Where(x => x.StartDate.HasValue && x.EndDate.HasValue && promo.StartDate.HasValue
                               && DateTimeOffset.Compare(x.StartDate.Value, promo.StartDate.Value) <= 0
                               && DateTimeOffset.Compare(x.EndDate.Value, promo.StartDate.Value) >= 0).ToList();

                    currentClient = clientTrees.Where(x => x.ObjectId == currentClient.parentId && !x.EndDate.HasValue).FirstOrDefault();
                }

                if (tradeInvestments.Count() == 0)
                {
                    error = true;
                    message = GetMessageTiCogs("TI base was not found", promo, true, brandTeches);
                    return null;
                }
                else
                {
                    var tradeInvestmentsList = new List<BaseTradeInvestment>(tradeInvestments);
                    bool containsDublicate = false;

                    // Если присутсвуют записи с пустым и заполненным брендтехом, берем только с заполненным
                    // при условии, что тип и подтип совпадают
                    if (tradeInvestments.Any(x => x.BrandTechId == null) && tradeInvestments.Any(x => x.BrandTechId != null))
                    {
                        tradeInvestmentsList = new List<BaseTradeInvestment>();
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
                        message = GetMessageTiCogs("TI base duplicate record error", promo, true, brandTeches);
                        return null;
                    }

                    // Суммируем все проценты подошедших записей из TI
                    foreach (var tradeInvestment in tradeInvestmentsList)
                    {
                        percentSum += tradeInvestment.SizePercent;
                    }

                    message = null;
                    return percentSum;
                }
            }
            catch (Exception e)
            {
                error = true;
                message = e.ToString();
                return null;
            }
        }
        
        public static double? GetCOGSPercent(SimplePromoCOGS promo, List<BaseCOGS> query, List<ClientTree> clientTrees, List<BrandTech> brandTeches, out string message)
        {
            try
            {
                List<BaseCOGS> cogsList = new List<BaseCOGS>();
                ClientTree clientNode = clientTrees.Where(x => x.ObjectId == promo.ClientTreeObjectId && !x.EndDate.HasValue).FirstOrDefault();

                var notNullBrandtechCOGS = query.Where(x => x.BrandTech != null);
                while (notNullBrandtechCOGS != null && (cogsList == null || cogsList.Count() == 0) && clientNode != null && clientNode.Type != "root")
                {
                    cogsList = notNullBrandtechCOGS
                        .Where(x => x.ClientTreeId == clientNode.Id && x.BrandTech.BrandsegTechsub == promo.BrandTechName && !x.Disabled)
                        //promo DispatchesStart date должна лежать в интервале между COGS start date и COGS end date
                        .Where(x => x.StartDate.HasValue && x.EndDate.HasValue && promo.DispatchesStart.HasValue
                               && DateTimeOffset.Compare(x.StartDate.Value, promo.DispatchesStart.Value) <= 0
                               && DateTimeOffset.Compare(x.EndDate.Value, promo.DispatchesStart.Value) >= 0).ToList();

                    clientNode = clientTrees.Where(x => x.ObjectId == clientNode.parentId && !x.EndDate.HasValue).FirstOrDefault();
                }

                //если не найдено COGS для конкретного BranTech, ищем COGS с пустым BrandTech(пустое=любое)
                if (cogsList.Count == 0)
                {
                    clientNode = clientTrees.Where(x => x.ObjectId == promo.ClientTreeObjectId && !x.EndDate.HasValue).FirstOrDefault();

                    while (query != null && (cogsList == null || cogsList.Count() == 0) && clientNode != null && clientNode.Type != "root")
                    {
                        cogsList = query
                            .Where(x => x.ClientTreeId == clientNode.Id && x.BrandTechId == null && !x.Disabled)
                            //promo DispatchesStart date должна лежать в интервале между COGS start date и COGS end date
                            .Where(x => x.StartDate.HasValue && x.EndDate.HasValue && promo.DispatchesStart.HasValue
                                   && DateTimeOffset.Compare(x.StartDate.Value, promo.DispatchesStart.Value) <= 0
                                   && DateTimeOffset.Compare(x.EndDate.Value, promo.DispatchesStart.Value) >= 0).ToList();

                        clientNode = clientTrees.Where(x => x.ObjectId == clientNode.parentId && !x.EndDate.HasValue).FirstOrDefault();
                    }
                }

                if (cogsList.Count == 0)
                {
                    message = GetMessageTiCogs("COGS was not found", promo, false, brandTeches);
                    return null;
                }
                else if (cogsList.Count > 1)
                {
                    message = GetMessageTiCogs("COGS duplicate record error", promo, false, brandTeches);
                    return null;
                }
                else
                {
                    message = null;
                    return cogsList[0].LSVpercent;
                }
            }
            catch (Exception e)
            {
                message = e.ToString();
                return null;
            }
        }

        public static double? GetCOGSTonCost(SimplePromoCOGS promo, List<BaseCOGSTn> query, List<ClientTree> clientTrees, List<BrandTech> brandTeches, out string message)
        {
            try
            {
                List<BaseCOGSTn> cogsList = new List<BaseCOGSTn>();
                ClientTree clientNode = clientTrees.Where(x => x.ObjectId == promo.ClientTreeObjectId && !x.EndDate.HasValue).FirstOrDefault();

                var notNullBrandtechCOGS = query.Where(x => x.BrandTech != null);
                while (notNullBrandtechCOGS != null && (cogsList == null || cogsList.Count() == 0) && clientNode != null && clientNode.Type != "root")
                {
                    cogsList = notNullBrandtechCOGS
                        .Where(x => x.ClientTreeId == clientNode.Id && x.BrandTech.BrandsegTechsub == promo.BrandTechName && !x.Disabled)
                        //promo DispatchesStart date должна лежать в интервале между COGS/Tn start date и COGS/Tn end date
                        .Where(x => x.StartDate.HasValue && x.EndDate.HasValue && promo.DispatchesStart.HasValue
                               && DateTimeOffset.Compare(x.StartDate.Value, promo.DispatchesStart.Value) <= 0
                               && DateTimeOffset.Compare(x.EndDate.Value, promo.DispatchesStart.Value) >= 0).ToList();

                    clientNode = clientTrees.Where(x => x.ObjectId == clientNode.parentId && !x.EndDate.HasValue).FirstOrDefault();
                }

                //если не найдено COGS/Tn для конкретного BranTech, ищем COGS/Tn с пустым BrandTech(пустое=любое)
                if (cogsList.Count == 0)
                {
                    clientNode = clientTrees.Where(x => x.ObjectId == promo.ClientTreeObjectId && !x.EndDate.HasValue).FirstOrDefault();

                    while (query != null && (cogsList == null || cogsList.Count() == 0) && clientNode != null && clientNode.Type != "root")
                    {
                        cogsList = query
                            .Where(x => x.ClientTreeId == clientNode.Id && x.BrandTechId == null && !x.Disabled)
                            //promo DispatchesStart date должна лежать в интервале между COGS/Tn start date и COGS/Tn end date
                            .Where(x => x.StartDate.HasValue && x.EndDate.HasValue && promo.DispatchesStart.HasValue
                                   && DateTimeOffset.Compare(x.StartDate.Value, promo.DispatchesStart.Value) <= 0
                                   && DateTimeOffset.Compare(x.EndDate.Value, promo.DispatchesStart.Value) >= 0).ToList();

                        clientNode = clientTrees.Where(x => x.ObjectId == clientNode.parentId && !x.EndDate.HasValue).FirstOrDefault();
                    }
                }

                if (cogsList.Count == 0)
                {
                    message = GetMessageTiCogs("COGS/Tn was not found", promo, false, brandTeches);
                    return null;
                }
                else if (cogsList.Count > 1)
                {
                    message = GetMessageTiCogs("COGS/Tn duplicate record error", promo, false, brandTeches);
                    return null;
                }
                else
                {
                    message = null;
                    return cogsList[0].TonCost;
                }
            }
            catch (Exception e)
            {
                message = e.ToString();
                return null;
            }
        }

        public static double? GetRATIShopperPercent(SimplePromoRATIShopper promo, DatabaseContext context, IQueryable<RATIShopper> query, out string message)
        {
            try
            {
                List<RATIShopper> ratishopperList = new List<RATIShopper>();
                ClientTree clientNode = context.Set<ClientTree>().Where(x => x.ObjectId == promo.ClientTreeObjectId && !x.EndDate.HasValue).FirstOrDefault();

                while ((ratishopperList == null || ratishopperList.Count() == 0) && clientNode != null && clientNode.Type != "root")
                {
                    ratishopperList = query
                        .Where(x => x.ClientTreeId == clientNode.Id && !x.Disabled)
                        .Where(x => x.Year == promo.Year).ToList();

                    clientNode = context.Set<ClientTree>().Where(x => x.ObjectId == clientNode.parentId && !x.EndDate.HasValue).FirstOrDefault();
                }

                if (ratishopperList.Count > 1)
                {
                    message = GetMessageRaTiShopper("RA TI Shopper duplicate record error", promo, context);
                    return null;
                }
                else if (ratishopperList.Count == 1)
                {
                    message = null;
                    return ratishopperList[0].RATIShopperPercent;
                }
                else
                {
                    message = null;
                    return 0;
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
        private static string GetMessageTiCogs(string baseMessage, SimplePromoCOGSTI promo, bool ti, List<BrandTech> brandTeches)
        {
            BrandTech brandTech = brandTeches.FirstOrDefault(n => n.Id == promo.BrandTechId);
            string result = baseMessage + " for client " + promo.ClientHierarchy;

            if (brandTech != null)
                result += " and BrandTech " + brandTech.BrandsegTechsub;

            if (ti)
                result += " for the period from " + promo.StartDate.Value.ToString("dd.MM.yyyy") + " to " + promo.EndDate.Value.ToString("dd.MM.yyyy") + ".";
            else
                result += " for the period from " + promo.DispatchesStart.Value.ToString("dd.MM.yyyy") + " to " + promo.DispatchesEnd.Value.ToString("dd.MM.yyyy") + ".";

            return result;
        }

        /// <summary>
        /// Сформировать сообщения об ошибке подбора для RA TI Shopper
        /// </summary>
        /// <param name="baseMessage">Базовое сообщение</param>
        /// <param name="promo">Промо</param>
        /// <param name="context">Контекст БД</param>
        /// <returns></returns>
        private static string GetMessageRaTiShopper(string baseMessage, SimplePromoRATIShopper promo, DatabaseContext context)
        {
            string result = baseMessage + " for client " + promo.ClientHierarchy;
            result += " for the year " + promo.Year + ".";
            return result;
        }
    }
}
