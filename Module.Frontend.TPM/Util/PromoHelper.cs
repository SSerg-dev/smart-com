using Core.Dependency;
using Core.Settings;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.OData;

namespace Module.Frontend.TPM.Util {
    public static class PromoHelper {
        /// <summary>
        /// Создание записи о создании/удалении нового промо
        /// </summary>
        /// <param name="newRecord"></param>
        /// <param name="isDelete"></param>
        public static void WritePromoDemandChangeIncident(DatabaseContext Context, Promo record, bool isDelete = false) {
            PromoDemandChangeIncident change = new PromoDemandChangeIncident(record, isDelete) { };
            Context.Set<PromoDemandChangeIncident>().Add(change);
            Context.SaveChanges();
        }

        /// <summary>
        /// Создание записи об изменении/удалении промо
        /// </summary>
        /// <param name="newRecord"></param>
        /// <param name="patch"></param>
        /// <param name="oldRecord"></param>
        public static void WritePromoDemandChangeIncident(DatabaseContext Context, Promo newRecord, Delta<Promo> patch, Promo oldRecord) {
            bool needCreateIncident = CheckCreateIncidentCondition(oldRecord, newRecord, patch);
            if (needCreateIncident) {
                PromoDemandChangeIncident change = new PromoDemandChangeIncident(oldRecord, newRecord) { };
                Context.Set<PromoDemandChangeIncident>().Add(change);
                Context.SaveChanges();
            }
        }

        /// <summary>
        /// Проверка на то, что о изменениях применённых к промо необходимо создавать сообщение
        /// </summary>
        /// <param name="oldRecord"></param>
        /// <param name="newRecord"></param>
        /// <param name="patch"></param>
        /// <returns></returns>
        private static bool CheckCreateIncidentCondition(Promo oldRecord, Promo newRecord, Delta<Promo> patch) {
            bool result = false;
            // TODO: Изменения продуктов не учитывается из-за удаления ProductTreeId из Promo
            // Получение настроек
            ISettingsManager settingsManager = (ISettingsManager) IoC.Kernel.GetService(typeof(ISettingsManager));

            string promoPropertiesSetting = settingsManager.GetSetting<string>("PROMO_CHANGE_PROPERTIES",
                "ClientTreeId, ProductHierarchy, StartDate, EndDate, DispatchesStart, DispatchesEnd, PlanUplift, PlanIncrementalLsv, MarsMechanicDiscount, InstoreMechanicDiscount, Mechanic, MarsMechanic, InstoreMechanic");
            int daysToCheckSetting = settingsManager.GetSetting<int>("PROMO_CHANGE_PERIOD_DAYS", 84);
            int marsDiscountSetting = settingsManager.GetSetting<int>("PROMO_CHANGE_MARS_DISCOUNT", 3);
            int instoreDiscountSetting = settingsManager.GetSetting<int>("PROMO_CHANGE_INSTORE_DISCOUNT", 5);
            int durationSetting = settingsManager.GetSetting<int>("PROMO_CHANGE_DURATION_DAYS", 3);
            int dispatchSetting = settingsManager.GetSetting<int>("PROMO_CHANGE_DISPATCH_DAYS", 5);

            string[] propertiesToCheck = promoPropertiesSetting.Split(',').Select(x => x.Trim(' ')).ToArray();

            DateTimeOffset today = DateTimeOffset.Now;
            bool isOldStartInCheckPeriod = oldRecord.StartDate.HasValue && (oldRecord.StartDate.Value - today).Days <= daysToCheckSetting;
            bool isNewStartInCheckPeriod = newRecord.StartDate.HasValue && (newRecord.StartDate.Value - today).Days <= daysToCheckSetting;
            IEnumerable<string> changedProperties = patch.GetChangedPropertyNames();
            changedProperties = changedProperties.Where(p => propertiesToCheck.Contains(p));
            bool relevantByTime = isOldStartInCheckPeriod || isNewStartInCheckPeriod;
            // Если дата начала промо соответствует настройке и изменилось какое-либо поле из указанных в настройках создаётся запись об изменении
            if (relevantByTime && changedProperties.Any()) {
                bool productChange = oldRecord.ProductHierarchy != newRecord.ProductHierarchy;
                bool clientChange = oldRecord.ClientTreeId != newRecord.ClientTreeId;
                bool marsMechanicChange = oldRecord.MarsMechanic != newRecord.MarsMechanic;
                bool instoreMechanicChange = oldRecord.PlanInstoreMechanic != newRecord.PlanInstoreMechanic;
                bool marsDiscountChange = oldRecord.MarsMechanicDiscount.HasValue ? newRecord.MarsMechanicDiscount.HasValue ? Math.Abs(oldRecord.MarsMechanicDiscount.Value - newRecord.MarsMechanicDiscount.Value) > marsDiscountSetting : true : false;
                bool instoreDiscountChange = oldRecord.PlanInstoreMechanicDiscount.HasValue ? newRecord.PlanInstoreMechanicDiscount.HasValue ? Math.Abs(oldRecord.PlanInstoreMechanicDiscount.Value - newRecord.PlanInstoreMechanicDiscount.Value) > instoreDiscountSetting : true : false;
                TimeSpan? oldDuration = oldRecord.EndDate - oldRecord.StartDate;
                TimeSpan? newDuration = newRecord.EndDate - newRecord.StartDate;
                bool durationChange = oldDuration.HasValue && newDuration.HasValue && Math.Abs(oldDuration.Value.Days - newDuration.Value.Days) > durationSetting;
                TimeSpan? dispatchStartDif = oldRecord.DispatchesStart - newRecord.DispatchesStart;
                TimeSpan? dispatchEndDif = oldRecord.DispatchesEnd - newRecord.DispatchesEnd;
                bool dispatchChange = dispatchStartDif.HasValue && dispatchEndDif.HasValue && Math.Abs(dispatchStartDif.Value.Days + dispatchEndDif.Value.Days) > dispatchSetting;
                bool isDelete = newRecord.PromoStatus != null && newRecord.PromoStatus.SystemName == "Deleted";

                List<bool> conditionnCheckResults = new List<bool>() { productChange, clientChange, marsMechanicChange, instoreMechanicChange, marsDiscountChange, instoreDiscountChange, durationChange, dispatchChange, isDelete };
                result = conditionnCheckResults.Any(x => x == true);
            }
            return result;
        }

        /// <summary>
        /// Создание отложенной задачи, выполняющей перерасчет бюджетов
        /// </summary>
        /// <param name="promoSupportPromoIds">список ID подстатей</param>
        /// <param name="calculatePlanCostTE">Необходимо ли пересчитывать значения плановые Cost TE</param>
        /// <param name="calculateFactCostTE">Необходимо ли пересчитывать значения фактические Cost TE</param>
        /// <param name="calculatePlanCostProd">Необходимо ли пересчитывать значения плановые Cost Production</param>
        /// <param name="calculateFactCostProd">Необходимо ли пересчитывать значения фактические Cost Production</param>
        public static void CalculateBudgetsCreateTask(string promoSupportPromoIds, bool calculatePlanCostTE, bool calculateFactCostTE, bool calculatePlanCostProd, bool calculateFactCostProd, Guid userId, Guid roleId, DatabaseContext Context) {
            HandlerData data = new HandlerData();
            HandlerDataHelper.SaveIncomingArgument("PromoSupportPromoIds", promoSupportPromoIds, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("CalculatePlanCostTE", calculatePlanCostTE, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("CalculateFactCostTE", calculateFactCostTE, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("CalculatePlanCostProd", calculatePlanCostProd, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("CalculateFactCostProd", calculateFactCostProd, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);

            bool success = CalculationTaskManager.CreateCalculationTask(CalculationTaskManager.CalculationAction.Budgets, data, Context);

            if (!success)
                throw new Exception("Promo was blocked for calculation");
        }

        public static IEnumerable<Column> GetExportSettings() {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 0, Field = "Number", Header = "Promo ID", Quoting = false },
                new Column() { Order = 0, Field = "Name", Header = "Promo name", Quoting = false },
                new Column() { Order = 0, Field = "PromoStatus.Name", Header = "Status", Quoting = false },
                new Column() { Order = 0, Field = "MarsMechanic.Name", Header = "Mars mechanic", Quoting = false },
                new Column() { Order = 0, Field = "MarsMechanicType.Name", Header = "Mars mechanic type", Quoting = false },
                new Column() { Order = 0, Field = "MarsMechanicDiscount", Header = "Mars mechanic discount, %", Quoting = false },
                new Column() { Order = 0, Field = "InstoreMechanic.Name", Header = "IA mechanic", Quoting = false },
                new Column() { Order = 0, Field = "InstoreMechanicType.Name", Header = "IA mechanic type", Quoting = false },
                new Column() { Order = 0, Field = "InstoreMechanicDiscount", Header = "IA mechanic discount, %", Quoting = false },
                new Column() { Order = 0, Field = "Brand.Name", Header = "Brand", Quoting = false },
                new Column() { Order = 0, Field = "BrandTech.Name", Header = "Brandtech", Quoting = false },
                new Column() { Order = 0, Field = "StartDate", Header = "Start date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "EndDate", Header = "End date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "DispatchesStart", Header = "Dispatch start", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "DispatchesEnd", Header = "Dispatch end", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "EventName", Header = "Event", Quoting = false },
                new Column() { Order = 0, Field = "Priority", Header = "Priority", Quoting = false },
                new Column() { Order = 0, Field = "ClientHierarchy", Header = "Client hierarchy", Quoting = false },
                new Column() { Order = 0, Field = "ProductHierarchy", Header = "Product hierarchy", Quoting = false },
                new Column() { Order = 0, Field = "ShopperTi", Header = "Shopper TI, RUR", Quoting = false },
                new Column() { Order = 0, Field = "MarketingTi", Header = "Marketing TI, RUR", Quoting = false },
                new Column() { Order = 0, Field = "Branding", Header = "Branding, RUR", Quoting = false },
                new Column() { Order = 0, Field = "BTL", Header = "BTL, RUR", Quoting = false },
                new Column() { Order = 0, Field = "CostProduction", Header = "Cost production, RUR", Quoting = false },
                new Column() { Order = 0, Field = "TotalCost", Header = "Total cost, RUR", Quoting = false },
                new Column() { Order = 0, Field = "PlanUplift", Header = "Uplift, %", Quoting = false },
                new Column() { Order = 0, Field = "PlanIncrementalLsv", Header = "Incremental LSV, RUR", Quoting = false },
                new Column() { Order = 0, Field = "PlanTotalPromoLsv", Header = "Total promo LSV, RUR", Quoting = false },
                new Column() { Order = 0, Field = "PlanPromoPostPromoEffectLSV", Header = "Post promo effect, RUR", Quoting = false },
                new Column() { Order = 0, Field = "PlanRoi", Header = "ROI, %", Quoting = false },
                new Column() { Order = 0, Field = "PlanIncrementalNsv", Header = "Incremental NSV, RUR", Quoting = false },
                new Column() { Order = 0, Field = "PlanTotalPromoNsv", Header = "Total promo NSV, RUR", Quoting = false },
                new Column() { Order = 0, Field = "PlanIncrementalMac", Header = "Incremental Mac, RUR", Quoting = false },
                new Column() { Order = 0, Field = "FactShopperTi", Header = "Actual Shopper TI, RUR", Quoting = false },
                new Column() { Order = 0, Field = "FactMarketingTi", Header = "Actual Marketing TI, RUR", Quoting = false },
                new Column() { Order = 0, Field = "FactBranding", Header = "Actual Branding, RUR", Quoting = false },
                new Column() { Order = 0, Field = "FactBTL", Header = "Actual BTL, RUR", Quoting = false },
                new Column() { Order = 0, Field = "FactCostProduction", Header = "Actual Cost production, RUR", Quoting = false },
                new Column() { Order = 0, Field = "FactTotalCost", Header = "Actual Total cost, RUR", Quoting = false },
                new Column() { Order = 0, Field = "FactUplift", Header = "Fact Uplift, %", Quoting = false },
                new Column() { Order = 0, Field = "FactIncrementalLsv", Header = "Fact Incremental LSV, RUR", Quoting = false },
                new Column() { Order = 0, Field = "FactTotalPromoLsv", Header = "Fact Total promo LSV, RUR", Quoting = false },
                new Column() { Order = 0, Field = "ActualPromoPostPromoEffectLSV", Header = "Fact Post promo effect, RUR", Quoting = false },
                new Column() { Order = 0, Field = "FactRoi", Header = "Fact ROI, %", Quoting = false },
                new Column() { Order = 0, Field = "FactIncrementalNsv", Header = "Fact Incremental NSV, RUR", Quoting = false },
                new Column() { Order = 0, Field = "FactTotalPromoNsv", Header = "Fact Total promo NSV, RUR", Quoting = false },
                new Column() { Order = 0, Field = "FactIncrementalMac", Header = "Fact Incremental Mac, RUR", Quoting = false },
                new Column() { Order = 0, Field = "Color.DisplayName", Header = "Color name", Quoting = false }
            };
            return columns;
        }

    }
}