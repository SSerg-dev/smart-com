using Looper.Core;
using Looper.Parameters;
using Microsoft.Ajax.Utilities;
using Module.Persist.TPM.Model.Interfaces;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.CalculatePromoParametersModule
{
    public class CalculationTaskManager
    {
        public enum CalculationAction { Uplift, BaseLine, Budgets, Actual, DataFlowFiltering, DataFlow, BTL }
        private static object locker = new object();

        /// <summary>
        /// Создать задачу на пересчет
        /// </summary>
        /// <param name="action">Тип пересчета</param>
        /// <param name="data">Параметры</param>
        /// <param name="context">Контекст БД</param>
        /// <param name="promoId">ID блокируемого Промо</param>
        /// <returns></returns>
        public static bool CreateCalculationTask(CalculationAction action, HandlerData data, DatabaseContext context, Guid? promoId = null, bool safe = false)
        {
            bool promoAvaible = true;

            // монопольный доступ позволит провести корректную блокировку
            lock (locker)
            {
                List<Guid> promoIdsForBlock = new List<Guid>();

                string description = "";
                string nameHandler = "";

                switch (action)
                {
                    case CalculationAction.Uplift:
                        description = "Calculate promo parameters (Promo was changed)";
                        nameHandler = "Module.Host.TPM.Handlers.CalculatePromoParametersHandler";

                        promoIdsForBlock = BudgetsPromoCalculation.GetLinkedPromoId(promoId.Value, context);
                        if (!promoIdsForBlock.Contains(promoId.Value))
                            promoIdsForBlock.Add(promoId.Value);

                        //if (HandlerDataHelper.GetIncomingArgument<bool>("NeedCalculatePlanMarketingTI", data, false))
                        //{
                        //    promoIdsForBlock = BudgetsPromoCalculation.GetLinkedPromoId(promoId.Value, context);
                        //    if (!promoIdsForBlock.Contains(promoId.Value))
                        //        promoIdsForBlock.Add(promoId.Value);
                        //}
                        //else
                        //{
                        //    promoIdsForBlock.Add(promoId.Value);
                        //}

                        break;

                    case CalculationAction.BaseLine:
                        // TODO: BaseLine
                        break;

                    case CalculationAction.Budgets:
                        // список ID подстатей/промо
                        string promoSupportIds = HandlerDataHelper.GetIncomingArgument<string>("PromoSupportIds", data, false);
                        string unlinkedPromoIds = HandlerDataHelper.GetIncomingArgument<string>("UnlinkedPromoIds", data, false);
                        TPMmode tPMmode1 = HandlerDataHelper.GetIncomingArgument<TPMmode>("TPMmode", data, false);
                        promoIdsForBlock = BudgetsPromoCalculation.GetLinkedPromoId(promoSupportIds, unlinkedPromoIds, context, tPMmode1);
                        description = "Calculate promo budgets";
                        nameHandler = "Module.Host.TPM.Handlers.CalculateBudgetsHandler";
                        break;

                    case CalculationAction.BTL:
                        // список ID подстатей/промо
                        string btlId = HandlerDataHelper.GetIncomingArgument<string>("BTLId", data, false);
                        var unlinkedPromoIdsList = HandlerDataHelper.GetIncomingArgument<List<Guid>>("UnlinkedPromoIds", data, false);
                        TPMmode tPMmode = HandlerDataHelper.GetIncomingArgument<TPMmode>("TPMmode", data, false);
                        promoIdsForBlock = unlinkedPromoIdsList != null
                            ? BudgetsPromoCalculation.GetLinkedPromoId(btlId, context, unlinkedPromoIdsList, tPMmode)
                            : BudgetsPromoCalculation.GetLinkedPromoId(btlId, context, null, tPMmode);
                        description = "Calculate promo BTL budgets";
                        nameHandler = "Module.Host.TPM.Handlers.CalculateBTLBudgetsHandler";
                        break;

                    case CalculationAction.Actual:
                        promoIdsForBlock.Add(promoId.Value);
                        description = "Calculate actual parameters";
                        nameHandler = "Module.Host.TPM.Handlers.CalculateActualParamatersHandler";
                        break;

                    case CalculationAction.DataFlow:
                        promoIdsForBlock = HandlerDataHelper.GetIncomingArgument<List<Guid>>("PromoIdsForBlock", data, false);
                        description = "Nightly recalculation (DataFlow)";
                        nameHandler = "Module.Host.TPM.Handlers.DataFlow.DataFlowRecalculatingHandler";
                        break;
                }

                if (action != CalculationAction.DataFlow)
                {
                    // при вызове patch/post идёт транзакция, для гарантии, что задача запустится только после изменения промо мы используем контекст этого метода
                    // чтобы другому потоку была видна блокировка используем другой контекст, который не входит в транзакцию
                    // иначе другой поток может просто её не увидеть
                    using (DatabaseContext contextOutOfTransaction = new DatabaseContext())
                    {
                        Guid handlerId = Guid.NewGuid();

                        foreach (Guid idPromo in promoIdsForBlock)
                        {
                            promoAvaible = promoAvaible && BlockPromo(idPromo, handlerId, contextOutOfTransaction, safe);

                            if (!promoAvaible)
                                break;
                        }

                        if (promoAvaible)
                        {
                            CreateHandler(handlerId, description, nameHandler, data, context);
                            contextOutOfTransaction.SaveChanges();
                        }
                    }
                }
                else
                {
                    var handlerId = Guid.NewGuid();
                    var handlerCreated = true;
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        BlockPromoRangeDataFlow(context, promoIdsForBlock, handlerId);
                        transaction.Commit();
                    }

                    var blockedPromoes = GetBlockedPromoRangeDataFlow(context);
                    if (!blockedPromoes.Any())
                    {
                        CreateHandler(handlerId, description, nameHandler, data, context);
                    }
                    else
                    {
                        handlerCreated = false;
                        return handlerCreated;
                    }
                }
            }

            return promoAvaible;
        }

        private static void CreateHandler(Guid handlerId, string description, string nameHandler, HandlerData data, DatabaseContext context)
        {
            Guid? userId = HandlerDataHelper.GetIncomingArgument<Guid>("UserId", data, false);
            Guid? roleId = HandlerDataHelper.GetIncomingArgument<Guid>("RoleId", data, false);

            LoopHandler handler = new LoopHandler()
            {
                Id = handlerId,
                ConfigurationName = "PROCESSING",
                Description = description,
                Name = nameHandler,
                ExecutionPeriod = null,
                CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                LastExecutionDate = null,
                NextExecutionDate = null,
                ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                UserId = userId == Guid.Empty ? null : userId,
                RoleId = roleId == Guid.Empty ? null : roleId
            };

            handler.SetParameterData(data);
            context.LoopHandlers.Add(handler);
            context.SaveChanges();
        }

        /// <summary>
        /// Заблокировать Промо
        /// </summary>
        /// <param name="promoId">ID блокируемого Промо</param>
        /// <param name="handlerId">ID обработчика</param>
        /// <param name="context">Контекст БД</param>
        /// <returns></returns>
        public static bool BlockPromo(Guid promoId, Guid handlerId, DatabaseContext context, bool safe = false)
        {
            bool promoAvaible = false;

            try
            {
                promoAvaible = !context.Set<BlockedPromo>().Any(n => n.PromoId == promoId && !n.Disabled);

                if (promoAvaible)
                {
                    BlockedPromo bp = new BlockedPromo
                    {
                        Id = Guid.NewGuid(),
                        PromoId = promoId,
                        HandlerId = handlerId,
                        CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                        Disabled = false,
                    };

                    context.Set<BlockedPromo>().Add(bp);
                }
            }
            catch
            {
                promoAvaible = false;
            }

            return safe ? safe : promoAvaible;
        }

        /// <summary>
        /// Заблокировать Промо
        /// </summary>
        /// <param name="promoId">ID блокируемого Промо</param>
        /// <param name="handlerId">ID обработчика</param>
        /// <returns></returns>
        public static bool BlockPromo(Guid promoId, Guid handlerId)
        {
            bool promoAvaible = false;

            try
            {
                lock (locker)
                {
                    using (DatabaseContext context = new DatabaseContext())
                    {
                        promoAvaible = !context.Set<BlockedPromo>().Any(n => n.PromoId == promoId && !n.Disabled);

                        if (promoAvaible)
                        {
                            BlockedPromo bp = new BlockedPromo
                            {
                                Id = Guid.NewGuid(),
                                PromoId = promoId,
                                HandlerId = handlerId,
                                CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                                Disabled = false,
                            };

                            context.Set<BlockedPromo>().Add(bp);
                            context.SaveChanges();
                        }
                    }
                }
            }
            catch
            {
                promoAvaible = false;
            }

            return promoAvaible;
        }

        /// <summary>
        /// Заблокировать перечень промо
        /// </summary>
        /// <param name="promo">Перечень промо</param>
        /// <param name="handlerId">ID обработчика</param>
        /// <returns></returns>
        public static bool BlockPromoRange(List<Promo> promo, Guid handlerId)
        {
            bool successBlock = true;

            try
            {
                lock (locker)
                {
                    using (DatabaseContext context = new DatabaseContext())
                    {
                        foreach (Promo p in promo)
                        {
                            if (p != null)
                            {
                                successBlock = successBlock && BlockPromo(p.Id, handlerId, context);
                            }
                        }

                        if (successBlock)
                        {
                            context.SaveChanges();
                        }
                    }
                }
            }
            catch { }

            return successBlock;
        }

        private static void BlockPromoRangeDataFlow(DatabaseContext databaseContext, List<Guid> promoIds, Guid handlerId)
        {
            var blockedPromoes = new List<BlockedPromo>();
            foreach (var promoId in promoIds)
            {
                blockedPromoes.Add(BlockPromoDataFlow(promoId, handlerId));
            }
            databaseContext.Set<BlockedPromo>().AddRange(blockedPromoes);
        }

        private static BlockedPromo BlockPromoDataFlow(Guid promoId, Guid handlerId)
        {
            return new BlockedPromo
            {
                Id = Guid.NewGuid(),
                Disabled = false,
                DeletedDate = null,
                PromoId = promoId,
                HandlerId = handlerId,
                CreateDate = DateTimeOffset.Now
            };
        }

        private static IEnumerable<BlockedPromo> GetBlockedPromoRangeDataFlow(DatabaseContext databasesContext)
        {
            var blockedPromoes = databasesContext.Set<BlockedPromo>().Where(x => !x.Disabled);
            return blockedPromoes;
        }


        /// <summary>
        /// Получить список заблокированных Промо
        /// </summary>
        /// <param name="handlerId">ID обработчика</param>
        /// <param name="context">Контекст БД</param>
        /// <returns></returns>
        public static Promo[] GetBlockedPromo(Guid handlerId, DatabaseContext context)
        {
            // список блокировок
            Guid[] blockedPromoIds = context.Set<BlockedPromo>().Where(n => n.HandlerId == handlerId && !n.Disabled).Select(n => n.PromoId).ToArray();
            // список заблокированных промо
            return context.Set<Promo>().Where(n => blockedPromoIds.Contains(n.Id)).ToArray();
        }

        /// <summary>
        /// Разблокировать Промо
        /// </summary>
        /// <param name="promoId">ID промо</param>
        public static void UnLockPromo(Guid promoId)
        {
            try
            {
                using (DatabaseContext contextOutOfTransaction = new DatabaseContext())
                {
                    BlockedPromo bp = contextOutOfTransaction.Set<BlockedPromo>().FirstOrDefault(n => n.PromoId == promoId && !n.Disabled);
                    if (bp != null)
                    {
                        bp.Disabled = true;
                        bp.DeletedDate = DateTime.Now;

                        contextOutOfTransaction.SaveChanges();
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Разблокировать Промо
        /// </summary>
        /// <param name="promoId">ID промо</param>
        public static void UnLockPromoForHandler(Guid handlerId)
        {
            try
            {
                using (DatabaseContext contextOutOfTransaction = new DatabaseContext())
                {
                    BlockedPromo[] blockedPromoes = contextOutOfTransaction.Set<BlockedPromo>().Where(n => n.HandlerId == handlerId && !n.Disabled).ToArray();

                    foreach (BlockedPromo bp in blockedPromoes)
                    {
                        bp.Disabled = true;
                        bp.DeletedDate = DateTime.Now;
                    }

                    contextOutOfTransaction.SaveChanges();
                }
            }
            catch { }
        }
    }
}
