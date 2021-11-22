using Looper.Core;
using Looper.Parameters;
using Microsoft.Ajax.Utilities;
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
    public class RPAUploadCalculationTaskManager
    {   
        private static object locker = new object();

        /// <summary>
        /// Создать задачу на пересчет
        /// </summary>
        /// <param name="action">Тип пересчета</param>
        /// <param name="data">Параметры</param>
        /// <param name="context">Контекст БД</param>
        /// <param name="promoId">ID блокируемого Промо</param>
        /// <returns></returns>
        public static Guid CreateCalculationTask(HandlerData data, DatabaseContext context, Guid? promoId = null, bool safe = false)
        {
            bool promoAvaible = true;
            Guid returnHandlerId = Guid.Empty;

            // монопольный доступ позволит провести корректную блокировку
            lock (locker)
            {
                List<Guid> promoIdsForBlock = new List<Guid>();

                string description = "";
                string nameHandler = "";

                string promoSupportIds = HandlerDataHelper.GetIncomingArgument<string>("PromoSupportIds", data, false);
                string unlinkedPromoIds = HandlerDataHelper.GetIncomingArgument<string>("UnlinkedPromoIds", data, false);
                promoIdsForBlock = BudgetsPromoCalculation.GetLinkedPromoId(promoSupportIds, unlinkedPromoIds, context);
                description = "Calculate promo budgets";
                nameHandler = "Module.Host.TPM.Handlers.CalculateBudgetsHandler";

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
                        CreateHandler(handlerId, description, nameHandler, data, context, "INPROGRESS");
                        contextOutOfTransaction.SaveChanges();
                    }
                }
            }

            return returnHandlerId;
        }

        private static void CreateHandler(Guid handlerId, string description, string nameHandler, HandlerData data, DatabaseContext context, string status = "WAITING")
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
                RoleId = roleId == Guid.Empty ? null : roleId,
                Status = status
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
    }
}
