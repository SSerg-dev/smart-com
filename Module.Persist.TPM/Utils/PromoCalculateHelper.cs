using Core.Security.Models;
using Looper.Core;
using Looper.Parameters;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Utils
{
    public static class PromoCalculateHelper
    {
        public static void RecalculateBudgets(Promo promo, UserInfo user, DatabaseContext context)
        {
            List<Guid> promoSupportIds = DetachPromoSupport(promo, context);
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = user.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);
            CalculateBudgetsCreateTask(promoSupportIds, userId, roleId, context);
        }

        public static void RecalculateBTLBudgets(Promo promo, UserInfo user, DatabaseContext context, bool safe = false)
        {
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = user.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            string btlId = context.Set<BTLPromo>().Where(x => x.PromoId == promo.Id && !x.Disabled && x.DeletedDate == null).FirstOrDefault()?.BTLId.ToString();
            if (!string.IsNullOrEmpty(btlId))
            {
                CalculateBTLBudgetsCreateTask(btlId, userId, roleId, context, safe: safe);
            }
        }

        /// <summary>
        /// Создание задачи на пересчет бюджетов
        /// </summary>
        /// <param name="promoSupportIds">Список Id бюджетов для пересчета</param>
        /// <param name="userId">Id пользователя</param>
        /// <param name="roleId">Id роли</param>
        /// <param name="context">Текущий контекст</param>
        public static void CalculateBudgetsCreateTask(List<Guid> promoSupportIds, Guid? userId, Guid? roleId, DatabaseContext context)
        {
            string promoSupportIdsString = FromListToString(promoSupportIds);
            string unlinkedPromoIdsString = FromListToString(null);

            HandlerData data = new HandlerData();
            HandlerDataHelper.SaveIncomingArgument("PromoSupportIds", promoSupportIdsString, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UnlinkedPromoIds", unlinkedPromoIdsString, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", userId == null ? Guid.Empty : userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId == null ? Guid.Empty : roleId, data, visible: false, throwIfNotExists: false);

            bool success = CalculationTaskManager.CreateCalculationTask(CalculationTaskManager.CalculationAction.Budgets, data, context);

            if (!success)
                throw new Exception("Promo was blocked for calculation");
        }

        /// <summary>
        /// Создание отложенной задачи, выполняющей перерасчет бюджетов BTL
        /// </summary>
        /// <param name="btlId">Id BTL статьи</param>
        /// <param name="userId">Id пользователя</param>
        /// <param name="roleId">Id роли</param>
        /// <param name="context">Текущий контекст</param>
        /// <param name="unlinkedPromoIds">Список Id открпряемых промо (default = null)</param>
        public static void CalculateBTLBudgetsCreateTask(string btlId, Guid? userId, Guid? roleId, DatabaseContext context, List<Guid> unlinkedPromoIds = null, bool safe = false)
        {
            HandlerData data = new HandlerData();
            HandlerDataHelper.SaveIncomingArgument("BTLId", btlId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", userId == null ? Guid.Empty : userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId == null ? Guid.Empty : roleId, data, visible: false, throwIfNotExists: false);
            if (unlinkedPromoIds != null)
            {
                HandlerDataHelper.SaveIncomingArgument("UnlinkedPromoIds", unlinkedPromoIds, data, visible: false, throwIfNotExists: false);
            }

            bool success = CalculationTaskManager.CreateCalculationTask(CalculationTaskManager.CalculationAction.BTL, data, context, safe: safe);

            if (!success)
                throw new Exception("Promo was blocked for calculation");
        }

        private static string FromListToString(List<Guid> list)
        {
            string result = "";

            if (list != null)
                foreach (Guid el in list.Distinct())
                    result += el + ";";

            return result;
        }

        /// <summary>
        /// Отвязывание промо от бюджетов. Метод возвращает список Id бюджетов для пересчета
        /// </summary>
        /// <param name="promo">Промо, от которого отвязываются бюджеты</param>
        /// <param name="context">Текущий контекст</param>
        public static List<Guid> DetachPromoSupport(Promo promo, DatabaseContext context)
        {
            //список Guid бюджетов, которые надо пересчитать
            List<Guid> promoSupportIds = new List<Guid>();
            //получаем список записей, которые надо удалить, чтобы отвязать промо от всех бюджетных статей
            List<PromoSupportPromo> pspToDeleteList = context.Set<PromoSupportPromo>().Where(x => x.PromoId == promo.Id && !x.Disabled).ToList();
            foreach (PromoSupportPromo psp in pspToDeleteList)
            {
                psp.DeletedDate = System.DateTime.Now;
                psp.Disabled = true;
            }

            ResetValues(promo);

            context.SaveChanges();

            foreach (PromoSupportPromo psp in pspToDeleteList)
            {
                promoSupportIds.Add(psp.PromoSupportId);
            }

            return promoSupportIds;
        }
        private static void ResetValues(Promo promo)
        {
            if (promo.PromoStatus.SystemName != "Cancelled" && !promo.Disabled)
            {
                // Budgets
                promo.PlanPromoXSites = null;
                promo.PlanPromoCatalogue = null;
                promo.PlanPromoPOSMInClient = null;
                promo.PlanPromoCostProdXSites = null;
                promo.PlanPromoCostProdCatalogue = null;
                promo.PlanPromoCostProdPOSMInClient = null;
                promo.ActualPromoXSites = null;
                promo.ActualPromoCatalogue = null;
                promo.ActualPromoPOSMInClient = null;
                promo.ActualPromoCostProdXSites = null;
                promo.ActualPromoCostProdCatalogue = null;
                promo.ActualPromoCostProdPOSMInClient = null;
                promo.PlanPromoTIShopper = null;
                promo.PlanPromoTIMarketing = null;
                promo.PlanPromoBranding = null;
                promo.PlanPromoCost = null;
                promo.PlanPromoBTL = null;
                promo.PlanPromoCostProduction = null;
                promo.ActualPromoTIShopper = null;
                promo.ActualPromoTIMarketing = null;
                promo.ActualPromoBranding = null;
                promo.ActualPromoBTL = null;
                promo.ActualPromoCostProduction = null;
                promo.ActualPromoCost = null;

                // Activity
                promo.PlanPromoBaselineLSV = null;
                promo.PlanPromoIncrementalLSV = null;
                promo.PlanPromoLSV = null;
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
                promo.PlanPromoUpliftPercent = null;
                promo.PlanPromoNetROIPercent = null;
                promo.PlanPromoNetUpliftPercent = null;
                promo.ActualPromoBaselineLSV = null;
                promo.ActualPromoIncrementalLSV = null;
                promo.ActualPromoUpliftPercent = null;
                promo.ActualPromoNetBaseTI = null;
                promo.ActualPromoNSV = null;
                promo.ActualPromoTIShopper = null;
                promo.ActualPromoCost = null;
                promo.ActualPromoIncrementalBaseTI = null;
                promo.ActualPromoNetIncrementalBaseTI = null;
                promo.ActualPromoIncrementalCOGS = null;
                promo.ActualPromoNetIncrementalCOGS = null;
                promo.ActualPromoTotalCost = null;
                promo.ActualPromoPostPromoEffectLSVW1 = null;
                promo.ActualPromoPostPromoEffectLSVW2 = null;
                promo.ActualPromoNetIncrementalLSV = null;
                promo.ActualPromoNetLSV = null;
                promo.ActualPromoIncrementalNSV = null;
                promo.ActualPromoNetIncrementalNSV = null;
                promo.ActualPromoIncrementalMAC = null;
                promo.ActualPromoNetIncrementalMAC = null;
                promo.ActualPromoIncrementalEarnings = null;
                promo.ActualPromoNetIncrementalEarnings = null;
                promo.ActualPromoROIPercent = null;
                promo.ActualPromoNetROIPercent = null;
                promo.ActualPromoNetUpliftPercent = null;

                promo.PlanPromoBaselineVolume = null;
                promo.PlanPromoPostPromoEffectVolume = null;
                promo.PlanPromoPostPromoEffectVolumeW1 = null;
                promo.PlanPromoPostPromoEffectVolumeW2 = null;
                promo.PlanPromoIncrementalVolume = null;
                promo.PlanPromoNetIncrementalVolume = null;
                promo.ActualPromoBaselineVolume = null;
                promo.ActualPromoPostPromoEffectVolume = null;
                promo.ActualPromoVolumeByCompensation = null;
                promo.ActualPromoVolumeSI = null;
                promo.ActualPromoVolume = null;
                promo.ActualPromoIncrementalVolume = null;
                promo.ActualPromoNetIncrementalVolume = null;
                promo.PlanPromoIncrementalCOGSTn = null;
                promo.PlanPromoNetIncrementalCOGSTn = null;
                promo.ActualPromoIncrementalCOGSTn = null;
                promo.ActualPromoNetIncrementalCOGSTn = null;
            }
        }
    }
}
