using Core.Dependency;
using Core.MarsCalendar;
using Core.Security.Models;
using Core.Settings;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.FunctionalHelpers.RA;
using Module.Frontend.TPM.FunctionalHelpers.Scenario;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Interfaces;
using Module.Persist.TPM.Model.SimpleModel;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.OData;

namespace Module.Frontend.TPM.Util
{
    public static class PromoHelper
    {
        /// <summary>
        /// Создание записи о создании/удалении нового промо
        /// </summary>
        /// <param name="newRecord"></param>
        /// <param name="isDelete"></param>
        public static void WritePromoDemandChangeIncident(DatabaseContext Context, Promo record, bool isDelete = false)
        {
            PromoDemandChangeIncident change = new PromoDemandChangeIncident(record, isDelete) { };
            Context.Set<PromoDemandChangeIncident>().Add(change);
            Context.SaveChanges();
        }

        /// <summary>
        /// Создание записи об изменении промо после пересчёта
        /// </summary>
        /// <param name="newRecord"></param>
        /// <param name="isDelete"></param>
        public static void WritePromoDemandChangeIncident(DatabaseContext Context, Promo record, string oldMarsMechanic, double? oldMarsMechanicDiscount, DateTimeOffset? oldDispatchesStart, double? oldPlanPromoUpliftPercent, double? oldPlanPromoIncrementalLSV)
        {
            PromoDemandChangeIncident change = new PromoDemandChangeIncident(record, oldMarsMechanic, oldMarsMechanicDiscount, oldDispatchesStart, oldPlanPromoUpliftPercent, oldPlanPromoIncrementalLSV) { };
            Context.Set<PromoDemandChangeIncident>().Add(change);
            Context.SaveChanges();
        }

        /// <summary>
        /// Создание записи об изменении/удалении промо
        /// </summary>
        /// <param name="newRecord"></param>
        /// <param name="patch"></param>
        /// <param name="oldRecord"></param>
        public static void WritePromoDemandChangeIncident(DatabaseContext Context, Promo newRecord, Delta<Promo> patch, Promo oldRecord, bool isSubrangeChanged)
        {
            bool needCreateIncident = CheckCreateIncidentCondition(oldRecord, newRecord, patch, isSubrangeChanged);
            if (needCreateIncident)
            {
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
        public static bool CheckCreateIncidentCondition(Promo oldRecord, Promo newRecord, Delta<Promo> patch, bool isSubrangeChanged)
        {
            bool result = false;
            // TODO: Изменения продуктов не учитывается из-за удаления ProductTreeId из Promo
            // Получение настроек
            ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));

            string promoPropertiesSetting = settingsManager.GetSetting<string>("PROMO_CHANGE_PROPERTIES",
                "InOutProductIds, ProductHierarchy, ProductTreeObjectIds, StartDate, EndDate, DispatchesStart, DispatchesEnd, MarsMechanicDiscount, PlanInstoreMechanicDiscount, MarsMechanicTypeId, PlanInstoreMechanicTypeId");
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
            if (relevantByTime && changedProperties.Any())
            {
                bool productChange = oldRecord.ProductHierarchy != newRecord.ProductHierarchy;
                bool productListChange = oldRecord.InOutProductIds != newRecord.InOutProductIds;
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

                List<bool> conditionnCheckResults = new List<bool>() { productChange, productListChange, isSubrangeChanged, marsMechanicChange, instoreMechanicChange, marsDiscountChange, instoreDiscountChange, durationChange, dispatchChange, isDelete };
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
        public static void CalculateBudgetsCreateTask(string promoSupportPromoIds, bool calculatePlanCostTE, bool calculateFactCostTE, bool calculatePlanCostProd, bool calculateFactCostProd, Guid userId, Guid roleId, DatabaseContext Context)
        {
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
        public static string ChangeResponsible(DatabaseContext Context, Promo model, string userName)
        {
            HashSet<UserRole> result = new HashSet<UserRole>();
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            Promo promo = model;
            var client = promo.ClientTreeId.ToString();
            IQueryable<Constraint> constraintsRes = Context.Constraints.AsQueryable();
            IQueryable<ClientTree> clientTree = Context.Set<ClientTree>();
            var hierarchyRes = hierarchy.Where(e => client.Equals(e.Id.ToString())).FirstOrDefault();
            var hierarchyesId = hierarchyRes.Hierarchy.Split('.').ToList();
            hierarchyesId.Add(client);
            hierarchy = hierarchy.Where(x =>
                     hierarchyesId.Any(h => h.Equals(x.Id.ToString())));

            HashSet<UserRole> userRoleConstraints = new HashSet<UserRole>();
            foreach (var item in constraintsRes)
            {
                userRoleConstraints.Add(item.UserRole);
            }

            var userRole = Context.Set<UserRole>().ToList();
            var userNotConstraint = userRole.Where(e => !userRoleConstraints.Any(r => r.Id.Equals(e.Id)));

            constraintsRes = ModuleApplyFilterHelper.ApplyFilter(constraintsRes, hierarchy);


            foreach (var item in constraintsRes)
            {
                result.Add(item.UserRole);
            }
            foreach (var item in userNotConstraint)
            {
                result.Add(item);
            }
            HashSet<User> userResult = new HashSet<User>();
            foreach (var item in result)
            {
                if (!item.User.Disabled)
                    userResult.Add(item.User);
            }
            Guid userId = Context.Set<User>().Where(e => e.Name == userName).FirstOrDefault().Id;
            if (!userResult.Any(e => e.Id == userId))
                return "This user has restrictions on this client.";
            if (model.CreatorId == userId)
                return "This user is attached to the promo.";
            model.CreatorId = userId;
            model.CreatorLogin = userName;
            Context.SaveChanges();
            return null;
        }

        public static void ResetPromo(DatabaseContext Context, Promo model, UserInfo user)
        {

            List<PromoProduct> promoProductToDeleteList = Context.Set<PromoProduct>().Where(x => x.PromoId == model.Id && !x.Disabled).ToList();
            foreach (PromoProduct promoProduct in promoProductToDeleteList)
            {
                promoProduct.DeletedDate = System.DateTime.Now;
                promoProduct.Disabled = true;
            }
            model.NeedRecountUplift = true;
            //необходимо удалить все коррекции/инкременталы
            //var promoProductToDeleteListIds = promoProductToDeleteList.Select(x => x.Id).ToList();
            if (model.InOut.HasValue && model.InOut.Value)
            {
                var productIdsToDelete = promoProductToDeleteList.Select(pp => pp.ProductId).ToList();
                List<IncrementalPromo> promoIncrementalPromoesToDeleteList = Context.Set<IncrementalPromo>()
                    .Where(x => productIdsToDelete.Contains(x.ProductId) && x.Disabled != true).ToList();

                foreach (IncrementalPromo incrementalPromoes in promoIncrementalPromoesToDeleteList)
                {
                    incrementalPromoes.DeletedDate = DateTimeOffset.UtcNow;
                    incrementalPromoes.Disabled = true;
                }
            }
            else
            {
                var productIdsToDelete = promoProductToDeleteList.Select(pp => pp.Id).ToList();
                List<PromoProductsCorrection> promoProductCorrectionToDeleteList = Context.Set<PromoProductsCorrection>()
                    .Where(x => productIdsToDelete.Contains(x.PromoProductId) && x.Disabled != true).ToList();

                foreach (PromoProductsCorrection promoProductsCorrection in promoProductCorrectionToDeleteList)
                {
                    promoProductsCorrection.DeletedDate = DateTimeOffset.UtcNow;
                    promoProductsCorrection.Disabled = true;
                    promoProductsCorrection.UserId = (Guid)user.Id;
                    promoProductsCorrection.UserName = user.Login;
                }
            }

            model.ActualPromoLSV = null;
            model.InvoiceNumber = null;
            model.ActualInStoreShelfPrice = null;
            model.PlanInStoreShelfPrice = null;
            model.ActualPromoBaselineBaseTI = null;
            model.ActualPromoBaseTI = null;
            model.ActualPromoNetNSV = null;
            model.DocumentNumber = null;
            model.ActualPromoPostPromoEffectLSV = null;
            model.ActualPromoLSVByCompensation = null;
            model.ActualInStoreDiscount = null;
            model.ActualInStoreMechanic = null;
            model.ActualInStoreMechanicId = null;
            model.ActualInStoreMechanicType = null;
            model.ActualInStoreMechanicTypeId = null;

            PromoCalculateHelper.RecalculateBudgets(model, user, Context);
            PromoCalculateHelper.RecalculateBTLBudgets(model, user, Context, safe: true);

            Context.SaveChanges();
        }

        /// <summary>
        /// Удаление записей IncrementalPromo, связанных с текущим промо
        /// </summary>
        public static void DisableIncrementalPromo(DatabaseContext context, Promo promo)
        {
            var incrementalPromoes = context.Set<IncrementalPromo>().Where(x => x.PromoId == promo.Id).ToList();
            foreach (var incrementalPromo in incrementalPromoes)
            {
                incrementalPromo.Disabled = true;
                incrementalPromo.DeletedDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
            }
            context.SaveChanges();
        }

        //TODO: Оптимизировать и сделать вызовы синхроннымы
        public static async Task UpdateProductHierarchy(string ChangedType, string NewName, string OldName, Guid? Id = null)
        {
            DatabaseContext context = new DatabaseContext();
            IQueryable<Promo> Promoes;
            switch (ChangedType)
            {
                case "Brand":
                    if (Id != null)
                    {
                        Promoes = context.Set<Promo>().Where(x => x.BrandId == Id &&
                                                                    x.PromoStatus.SystemName != "Closed" &&
                                                                    x.PromoStatus.SystemName != "Deleted" &&
                                                                    x.PromoStatus.SystemName != "Cancelled");

                        Parallel.ForEach(Promoes, promo =>
                        {
                            if (promo.ProductHierarchy.StartsWith(OldName + " >"))
                            {
                                promo.ProductHierarchy = promo.ProductHierarchy.Remove(0, OldName.Length);
                                promo.ProductHierarchy = promo.ProductHierarchy.Insert(0, NewName);
                            }
                        });
                    }
                    break;

                case "Technology":
                    if (Id != null)
                    {
                        Promoes = context.Set<Promo>().Where(x => x.TechnologyId == Id);
                        var s = Promoes.Count();

                        Parallel.ForEach(Promoes, promo =>
                        {
                            if (promo.ProductHierarchy.StartsWith(OldName + " >"))
                            {
                                promo.ProductHierarchy = promo.ProductHierarchy.Remove(0, OldName.Length);
                                promo.ProductHierarchy = promo.ProductHierarchy.Insert(0, NewName);
                            }
                            else if (promo.ProductHierarchy.Contains("> " + OldName + " >"))
                            {
                                promo.ProductHierarchy = promo.ProductHierarchy.Replace("> " + OldName + " >", "> " + NewName + " >");
                            }
                            else if (promo.ProductHierarchy.EndsWith("> " + OldName))
                            {
                                promo.ProductHierarchy = promo.ProductHierarchy.Remove(promo.ProductHierarchy.Length - OldName.Length);
                                promo.ProductHierarchy = promo.ProductHierarchy.Insert(promo.ProductHierarchy.Length, NewName);
                            }
                        });
                    }
                    break;

                case "Subrange":
                    Promoes = context.Set<Promo>().Where(x => x.ProductHierarchy == OldName);

                    Parallel.ForEach(Promoes, promo => promo.ProductHierarchy = NewName);

                    break;
            }
            await context.SaveChangesAsync();
        }

        public static void CalculateSumInvoiceProduct(DatabaseContext context, Promo promo)
        {
            // Получаем все записи из таблицы PromoProduct для текущего промо.
            var promoProductsForCurrentPromo = context.Set<PromoProduct>()
                .Where(x => x.PromoId == promo.Id && x.Disabled != true);

            double sumActualProductPCQty = 0;
            // Доля от всего SumInvoice
            double SumInvoiceProductPart = 0;

            sumActualProductPCQty = Convert.ToDouble(promoProductsForCurrentPromo.Select(p => p.ActualProductPCQty).Sum());

            // Перебираем все найденные для текущего промо записи из таблицы PromoProduct.
            foreach (var promoProduct in promoProductsForCurrentPromo)
            {
                // Если ActualProductPCQty нет, то мы не сможем посчитать долю
                if (promoProduct.ActualProductPCQty.HasValue)
                {
                    SumInvoiceProductPart = 0;
                    // Если показатель ActualProductPCQty == 0, то он составляет 0 процентов от показателя PlanPromoBaselineLSV.
                    if (promoProduct.ActualProductPCQty.Value != 0)
                    {
                        // Считаем долю ActualProductPCQty от SumInvoice.
                        SumInvoiceProductPart = promoProduct.ActualProductPCQty.Value / sumActualProductPCQty;
                    }
                    // Устанавливаем SumInvoiceProduct в запись таблицы PromoProduct.
                    promoProduct.SumInvoiceProduct = SumInvoiceProductPart * promo.SumInvoice;

                }
                else
                {
                    promoProduct.SumInvoiceProduct = 0;
                }
            }
            context.SaveChanges();
        }

        public static IEnumerable<Column> GetExportSettings()
        {
            IEnumerable<Column> columns = new List<Column>()
            {
                new Column() { Order = 0, Field = "Number", Header = "Promo ID", Quoting = false },
                new Column() { Order = 0, Field = "ClientHierarchy", Header = "Client", Quoting = false },
                new Column() { Order = 0, Field = "InOut", Header = "In-Out", Quoting = false },
                new Column() { Order = 0, Field = "IsOnInvoice", Header = "Invoice Type (True - On-invoice)", Quoting = false },
                new Column() { Order = 0, Field = "IsGrowthAcceleration", Header = "Growth acceleration", Quoting = false },
                new Column() { Order = 0, Field = "IsInExchange", Header = "In Exchange", Quoting = false },
                new Column() { Order = 0, Field = "IsApolloExport", Header = "Anaplan Export", Quoting = false },
                new Column() { Order = 0, Field = "DeviationCoefficient", Header = "Adjustment, %", Quoting = false },
                new Column() { Order = 0, Field = "Name", Header = "Promo name", Quoting = false },
                new Column() { Order = 0, Field = "BrandTech.BrandsegTechsub", Header = "Brandtech", Quoting = false },
                new Column() { Order = 0, Field = "EventName", Header = "Event", Quoting = false },
                new Column() { Order = 0, Field = "PlanPromoUpliftPercent", Header = "Plan Promo Uplift, %", Quoting = false },
                new Column() { Order = 0, Field = "PlanPromoIncrementalLSV", Header = "Plan Promo Incremental LSV", Quoting = false },
                new Column() { Order = 0, Field = "PlanPromoBaselineLSV", Header = "Plan Promo Baseline LSV", Quoting = false },
                new Column() { Order = 0, Field = "MarsMechanic.Name", Header = "Mars mechanic", Quoting = false },
                new Column() { Order = 0, Field = "MarsMechanicType.Name", Header = "Mars mechanic type", Quoting = false },
                new Column() { Order = 0, Field = "MarsMechanicDiscount", Header = "Mars mechanic discount, %", Quoting = false },
                new Column() { Order = 0, Field = "InstoreMechanic.Name", Header = "IA mechanic", Quoting = false },
                new Column() { Order = 0, Field = "InstoreMechanicType.Name", Header = "IA mechanic type", Quoting = false },
                new Column() { Order = 0, Field = "InstoreMechanicDiscount", Header = "IA mechanic discount, %", Quoting = false },
                new Column() { Order = 0, Field = "StartDate", Header = "Start date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "MarsStartDate", Header = "Mars Start date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "EndDate", Header = "End date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "MarsEndDate", Header = "Mars End date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "DispatchesStart", Header = "Dispatch start", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "MarsDispatchesStart", Header = "Mars Dispatch start", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "DispatchesEnd", Header = "Dispatch end", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "MarsDispatchesEnd", Header = "Mars Dispatch end", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "PromoStatus.Name", Header = "Status", Quoting = false },
                new Column() { Order = 0, Field = "PromoTypes.Name", Header = "Promo Types Name", Quoting = false },
                new Column() { Order = 0, Field = "IsPriceIncrease", Header = "Price Increase", Quoting = false },

                //new Column() { Order = 0, Field = "Brand.Name", Header = "Brand", Quoting = false },
                //new Column() { Order = 0, Field = "Priority", Header = "Priority", Quoting = false },
                //new Column() { Order = 0, Field = "ProductHierarchy", Header = "Product hierarchy", Quoting = false },
                //new Column() { Order = 0, Field = "ShopperTi", Header = "Shopper TI, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "MarketingTi", Header = "Marketing TI, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "Branding", Header = "Branding, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "BTL", Header = "BTL, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "CostProduction", Header = "Cost production, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "TotalCost", Header = "Total cost, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "PlanUplift", Header = "Uplift, %", Quoting = false },
                //new Column() { Order = 0, Field = "PlanIncrementalLsv", Header = "Incremental LSV, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "PlanTotalPromoLsv", Header = "Total promo LSV, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "PlanPromoPostPromoEffectLSV", Header = "Post promo effect, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "PlanRoi", Header = "ROI, %", Quoting = false },
                //new Column() { Order = 0, Field = "PlanIncrementalNsv", Header = "Incremental NSV, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "PlanTotalPromoNsv", Header = "Total promo NSV, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "PlanIncrementalMac", Header = "Incremental Mac, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactShopperTi", Header = "Actual Shopper TI, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactMarketingTi", Header = "Actual Marketing TI, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactBranding", Header = "Actual Branding, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactBTL", Header = "Actual BTL, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactCostProduction", Header = "Actual Cost production, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactTotalCost", Header = "Actual Total cost, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactUplift", Header = "Fact Uplift, %", Quoting = false },
                //new Column() { Order = 0, Field = "FactIncrementalLsv", Header = "Fact Incremental LSV, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactTotalPromoLsv", Header = "Fact Total promo LSV, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "ActualPromoPostPromoEffectLSV", Header = "Fact Post promo effect, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactRoi", Header = "Fact ROI, %", Quoting = false },
                //new Column() { Order = 0, Field = "FactIncrementalNsv", Header = "Fact Incremental NSV, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactTotalPromoNsv", Header = "Fact Total promo NSV, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactIncrementalMac", Header = "Fact Incremental Mac, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "Color.DisplayName", Header = "Color name", Quoting = false }
            };
            return columns;
        }

        public static IEnumerable<Column> GetViewExportSettings()
        {
            IEnumerable<Column> columns = new List<Column>()
            {
                new Column() { Order = 0, Field = "Number", Header = "Promo ID", Quoting = false },
                new Column() { Order = 0, Field = "ClientHierarchy", Header = "Client", Quoting = false },
                new Column() { Order = 0, Field = "InOut", Header = "In-Out", Quoting = false },
                new Column() { Order = 0, Field = "IsOnInvoice", Header = "Invoice Type (True - On-invoice)", Quoting = false },
                new Column() { Order = 0, Field = "IsGrowthAcceleration", Header = "Growth acceleration", Quoting = false },
                new Column() { Order = 0, Field = "IsInExchange", Header = "In Exchange", Quoting = false },
                new Column() { Order = 0, Field = "IsApolloExport", Header = "Anaplan Export", Quoting = false },
                new Column() { Order = 0, Field = "DeviationCoefficient", Header = "Adjustment, %", Quoting = false },
                new Column() { Order = 0, Field = "Name", Header = "Promo name", Quoting = false },
                new Column() { Order = 0, Field = "BrandTechName", Header = "Brandtech", Quoting = false },
                new Column() { Order = 0, Field = "PromoEventName", Header = "Event", Quoting = false },
                new Column() { Order = 0, Field = "PlanPromoUpliftPercent", Header = "Plan Promo Uplift, %", Quoting = false },
                new Column() { Order = 0, Field = "PlanPromoIncrementalLSV", Header = "Plan Promo Incremental LSV", Quoting = false },
                new Column() { Order = 0, Field = "PlanPromoBaselineLSV", Header = "Plan Promo Baseline LSV", Quoting = false },
                new Column() { Order = 0, Field = "MarsMechanicName", Header = "Mars mechanic", Quoting = false },
                new Column() { Order = 0, Field = "MarsMechanicTypeName", Header = "Mars mechanic type", Quoting = false },
                new Column() { Order = 0, Field = "MarsMechanicDiscount", Header = "Mars mechanic discount, %", Quoting = false },
                new Column() { Order = 0, Field = "PlanInstoreMechanicName", Header = "IA mechanic", Quoting = false },
                new Column() { Order = 0, Field = "PlanInstoreMechanicTypeName", Header = "IA mechanic type", Quoting = false },
                new Column() { Order = 0, Field = "InstoreMechanicDiscount", Header = "IA mechanic discount, %", Quoting = false },
                new Column() { Order = 0, Field = "StartDate", Header = "Start date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "MarsStartDate", Header = "Mars Start date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "EndDate", Header = "End date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "MarsEndDate", Header = "Mars End date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "DispatchesStart", Header = "Dispatch start", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "MarsDispatchesStart", Header = "Mars Dispatch start", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "DispatchesEnd", Header = "Dispatch end", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "MarsDispatchesEnd", Header = "Mars Dispatch end", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "PromoStatusName", Header = "Status", Quoting = false },
                new Column() { Order = 0, Field = "PromoTypesName", Header = "Promo Types Name", Quoting = false },
                new Column() { Order = 0, Field = "IsPriceIncrease", Header = "Price Increase", Quoting = false },

                //new Column() { Order = 0, Field = "Brand.Name", Header = "Brand", Quoting = false },
                //new Column() { Order = 0, Field = "Priority", Header = "Priority", Quoting = false },
                //new Column() { Order = 0, Field = "ProductHierarchy", Header = "Product hierarchy", Quoting = false },
                //new Column() { Order = 0, Field = "ShopperTi", Header = "Shopper TI, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "MarketingTi", Header = "Marketing TI, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "Branding", Header = "Branding, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "BTL", Header = "BTL, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "CostProduction", Header = "Cost production, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "TotalCost", Header = "Total cost, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "PlanUplift", Header = "Uplift, %", Quoting = false },
                //new Column() { Order = 0, Field = "PlanIncrementalLsv", Header = "Incremental LSV, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "PlanTotalPromoLsv", Header = "Total promo LSV, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "PlanPromoPostPromoEffectLSV", Header = "Post promo effect, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "PlanRoi", Header = "ROI, %", Quoting = false },
                //new Column() { Order = 0, Field = "PlanIncrementalNsv", Header = "Incremental NSV, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "PlanTotalPromoNsv", Header = "Total promo NSV, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "PlanIncrementalMac", Header = "Incremental Mac, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactShopperTi", Header = "Actual Shopper TI, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactMarketingTi", Header = "Actual Marketing TI, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactBranding", Header = "Actual Branding, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactBTL", Header = "Actual BTL, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactCostProduction", Header = "Actual Cost production, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactTotalCost", Header = "Actual Total cost, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactUplift", Header = "Fact Uplift, %", Quoting = false },
                //new Column() { Order = 0, Field = "FactIncrementalLsv", Header = "Fact Incremental LSV, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactTotalPromoLsv", Header = "Fact Total promo LSV, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "ActualPromoPostPromoEffectLSV", Header = "Fact Post promo effect, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactRoi", Header = "Fact ROI, %", Quoting = false },
                //new Column() { Order = 0, Field = "FactIncrementalNsv", Header = "Fact Incremental NSV, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactTotalPromoNsv", Header = "Fact Total promo NSV, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactIncrementalMac", Header = "Fact Incremental Mac, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "Color.DisplayName", Header = "Color name", Quoting = false }
            };
            return columns;
        }

        public static IEnumerable<Column> GetViewExportSettingsRS()
        {
            IEnumerable<Column> columns = new List<Column>()
            {
                new Column() { Order = 0, Field = "Number", Header = "Promo ID", Quoting = false },
                new Column() { Order = 0, Field = "ClientHierarchy", Header = "Client", Quoting = false },
                new Column() { Order = 0, Field = "TPMmode", Header = "Indicator", Quoting = false },
                new Column() { Order = 0, Field = "InOut", Header = "In-Out", Quoting = false },
                new Column() { Order = 0, Field = "IsOnInvoice", Header = "Invoice Type (True - On-invoice)", Quoting = false },
                new Column() { Order = 0, Field = "IsGrowthAcceleration", Header = "Growth acceleration", Quoting = false },
                new Column() { Order = 0, Field = "IsInExchange", Header = "In Exchange", Quoting = false },
                new Column() { Order = 0, Field = "IsApolloExport", Header = "Anaplan Export", Quoting = false },
                new Column() { Order = 0, Field = "DeviationCoefficient", Header = "Adjustment, %", Quoting = false },
                new Column() { Order = 0, Field = "Name", Header = "Promo name", Quoting = false },
                new Column() { Order = 0, Field = "BrandTechName", Header = "Brandtech", Quoting = false },
                new Column() { Order = 0, Field = "PromoEventName", Header = "Event", Quoting = false },
                new Column() { Order = 0, Field = "PlanPromoUpliftPercent", Header = "Plan Promo Uplift, %", Quoting = false },
                new Column() { Order = 0, Field = "PlanPromoIncrementalLSV", Header = "Plan Promo Incremental LSV", Quoting = false },
                new Column() { Order = 0, Field = "PlanPromoBaselineLSV", Header = "Plan Promo Baseline LSV", Quoting = false },
                new Column() { Order = 0, Field = "MarsMechanicName", Header = "Mars mechanic", Quoting = false },
                new Column() { Order = 0, Field = "MarsMechanicTypeName", Header = "Mars mechanic type", Quoting = false },
                new Column() { Order = 0, Field = "MarsMechanicDiscount", Header = "Mars mechanic discount, %", Quoting = false },
                new Column() { Order = 0, Field = "PlanInstoreMechanicName", Header = "IA mechanic", Quoting = false },
                new Column() { Order = 0, Field = "PlanInstoreMechanicTypeName", Header = "IA mechanic type", Quoting = false },
                new Column() { Order = 0, Field = "InstoreMechanicDiscount", Header = "IA mechanic discount, %", Quoting = false },
                new Column() { Order = 0, Field = "StartDate", Header = "Start date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "MarsStartDate", Header = "Mars Start date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "EndDate", Header = "End date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "MarsEndDate", Header = "Mars End date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "DispatchesStart", Header = "Dispatch start", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "MarsDispatchesStart", Header = "Mars Dispatch start", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "DispatchesEnd", Header = "Dispatch end", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "MarsDispatchesEnd", Header = "Mars Dispatch end", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "PromoStatusName", Header = "Status", Quoting = false },
                new Column() { Order = 0, Field = "PromoTypesName", Header = "Promo Types Name", Quoting = false },
                new Column() { Order = 0, Field = "IsPriceIncrease", Header = "Price Increase", Quoting = false },

                //new Column() { Order = 0, Field = "Brand.Name", Header = "Brand", Quoting = false },
                //new Column() { Order = 0, Field = "Priority", Header = "Priority", Quoting = false },
                //new Column() { Order = 0, Field = "ProductHierarchy", Header = "Product hierarchy", Quoting = false },
                //new Column() { Order = 0, Field = "ShopperTi", Header = "Shopper TI, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "MarketingTi", Header = "Marketing TI, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "Branding", Header = "Branding, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "BTL", Header = "BTL, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "CostProduction", Header = "Cost production, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "TotalCost", Header = "Total cost, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "PlanUplift", Header = "Uplift, %", Quoting = false },
                //new Column() { Order = 0, Field = "PlanIncrementalLsv", Header = "Incremental LSV, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "PlanTotalPromoLsv", Header = "Total promo LSV, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "PlanPromoPostPromoEffectLSV", Header = "Post promo effect, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "PlanRoi", Header = "ROI, %", Quoting = false },
                //new Column() { Order = 0, Field = "PlanIncrementalNsv", Header = "Incremental NSV, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "PlanTotalPromoNsv", Header = "Total promo NSV, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "PlanIncrementalMac", Header = "Incremental Mac, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactShopperTi", Header = "Actual Shopper TI, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactMarketingTi", Header = "Actual Marketing TI, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactBranding", Header = "Actual Branding, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactBTL", Header = "Actual BTL, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactCostProduction", Header = "Actual Cost production, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactTotalCost", Header = "Actual Total cost, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactUplift", Header = "Fact Uplift, %", Quoting = false },
                //new Column() { Order = 0, Field = "FactIncrementalLsv", Header = "Fact Incremental LSV, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactTotalPromoLsv", Header = "Fact Total promo LSV, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "ActualPromoPostPromoEffectLSV", Header = "Fact Post promo effect, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactRoi", Header = "Fact ROI, %", Quoting = false },
                //new Column() { Order = 0, Field = "FactIncrementalNsv", Header = "Fact Incremental NSV, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactTotalPromoNsv", Header = "Fact Total promo NSV, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "FactIncrementalMac", Header = "Fact Incremental Mac, RUR", Quoting = false },
                //new Column() { Order = 0, Field = "Color.DisplayName", Header = "Color name", Quoting = false }
            };
            return columns;
        }

        public static IEnumerable<Column> GetPromoProductCorrectionExportSettings()
        {
            int orderNumber = 0;
            IEnumerable<Column> columns = new List<Column>
            {
                 new Column { Order = orderNumber++, Field = "Number", Header = "Promo ID", Quoting = false,  Format = "0" },
                 new Column { Order = orderNumber++, Field = "ClientTreeFullPathName", Header = "Client", Quoting = false },
                 new Column { Order = orderNumber++, Field = "BrandTechName", Header = "BrandTech", Quoting = false },
                 new Column { Order = orderNumber++, Field = "ProductSubrangesList", Header = "Subranges", Quoting = false },
                 new Column { Order = orderNumber++, Field = "MarsMechanicName", Header = "Mars Mechanic", Quoting = false },
                 new Column { Order = orderNumber++, Field = "EventName", Header = "Event", Quoting = false },
                 new Column { Order = orderNumber++, Field = "PromoStatusSystemName", Header = "Promo Status", Quoting = false },
                 new Column { Order = orderNumber++, Field = "MarsStartDate", Header = "Mars Start Date", Quoting = false },
                 new Column { Order = orderNumber++, Field = "MarsEndDate", Header = "Mars End Date", Quoting = false },
                 new Column { Order = orderNumber++, Field = "PlanProductBaselineLSV", Header = "Plan Product Baseline LSV", Quoting = false,  Format = "0.00" },
                 new Column { Order = orderNumber++, Field = "PlanProductIncrementalLSV", Header = "Plan Product Incremental LSV", Quoting = false,  Format = "0.00" },
                 new Column { Order = orderNumber++, Field = "PlanProductLSV", Header = "Plan Product LSV", Quoting = false,  Format = "0.00" },
                 new Column { Order = orderNumber++, Field = "ZREP", Header = "ZREP", Quoting = false,  Format = "0" },
                 new Column { Order = orderNumber++, Field = "PlanProductUpliftPercentCorrected", Header = "Plan Product Uplift Percent Corrected", Quoting = false,  Format = "0.00"  },
                 new Column { Order = orderNumber++, Field = "CreateDate", Header = "CreateDate", Quoting = false,Format = "dd.MM.yyyy"},
                 new Column { Order = orderNumber++, Field = "ChangeDate", Header = "ChangeDate", Quoting = false,Format = "dd.MM.yyyy"},
                 new Column { Order = orderNumber++, Field = "UserName", Header = "UserName", Quoting = false }
            };

            return columns;
        }

        public static IEnumerable<Column> GetExportCorrectionSettings()
        {
            int orderNumber = 1;
            IEnumerable<Column> columns = new List<Column>()
            {
                new Column { Order = orderNumber++, Field = "Promo.Number", Header = "Promo ID", Quoting = false,  Format = "0" },
                new Column { Order = orderNumber++, Field = "ZREP", Header = "ZREP", Quoting = false,  Format = "0" },
                new Column { Order = orderNumber++, Field = "PlanProductUpliftPercent", Header = "Plan Product Uplift, %", Quoting = false,  Format = "0.00"},
            };
            return columns;
        }
        public static IEnumerable<Column> GetExportCorrectionPISettings()
        {
            int orderNumber = 1;
            IEnumerable<Column> columns = new List<Column>()
            {
                new Column { Order = orderNumber++, Field = "PromoPriceIncrease.Promo.Number", Header = "Promo ID", Quoting = false,  Format = "0" },
                new Column { Order = orderNumber++, Field = "ZREP", Header = "ZREP", Quoting = false,  Format = "0" },
                new Column { Order = orderNumber++, Field = "PlanProductUpliftPercent", Header = "Plan Product Uplift, %", Quoting = false,  Format = "0.00"},
            };
            return columns;
        }
        public static Promo CreateDefaultPromo(DatabaseContext context)
        {
            int dayspromo = 4;
            DateTimeOffset startD = TimeHelper.TodayStartDay();
            DateTimeOffset endD = startD.AddDays(dayspromo);
            DateTimeOffset startP = startD.AddDays(14);
            DateTimeOffset endP = endD.AddDays(14);
            Guid PromoTypesId = context.Set<PromoTypes>().FirstOrDefault(g => g.SystemName == "Regular").Id;
            Guid EventId = context.Set<Event>().FirstOrDefault(g => g.Name == "Standard promo").Id;
            Guid MechanicId = context.Set<Mechanic>().FirstOrDefault(g => g.SystemName == "Other" && g.PromoTypesId == PromoTypesId).Id;
            Guid PromoStatusId = context.Set<PromoStatus>().FirstOrDefault(g => g.SystemName == "Draft").Id;
            Promo promo = new Promo
            {
                TPMmode = TPMmode.Current,
                ActualPromoBTL = 0,
                ActualPromoBranding = 0,
                ActualPromoCost = 0,
                ActualPromoLSV = 0,
                ActualPromoLSVSO = 0,
                ActualPromoTIShopper = 0,
                AdditionalUserTimestamp = "",
                BudgetYear = 2023,
                CalendarPriority = 3,
                ClientHierarchy = "NA > Tander > Magnit MM",
                ClientTreeId = 5000004,
                ClientTreeKeyId = 19,
                DeviationCoefficient = 10,
                DispatchesStart = startD,
                DispatchesEnd = endD,
                StartDate = startP,
                EndDate = endP,
                EventId = EventId,
                Id = Guid.NewGuid(),
                InOutProductIds = "to change!!! 8af37b51-e73d-ea11-a86c-000d3a46085b;a5f37b51-e73d-ea11-a86c-000d3a46085b;",
                IsApolloExport = true,
                MarsMechanicDiscount = 20,
                MarsMechanicId = MechanicId,
                Name = "Unpublish Promo",
                NeedRecountUplift = true,
                ProductHierarchy = "Kitekat > Dry",
                ProductTreeObjectIds = "1000043",
                PromoStatusId = PromoStatusId,
                PromoTypesId = PromoTypesId,

            };
            return promo;
        }
        public static Promo CreateMLDefaultPromo(DatabaseContext context, Guid PromoTypesId, Event Event, Guid PromoStatusId)
        {
            Promo promo = new Promo
            {
                TPMmode = TPMmode.RS,
                ActualPromoBTL = 0,
                ActualPromoBranding = 0,
                ActualPromoCost = 0,
                ActualPromoLSV = 0,
                ActualPromoLSVSO = 0,
                ActualPromoTIShopper = 0,
                AdditionalUserTimestamp = "",
                BudgetYear = 2023,
                CalendarPriority = 3,
                ClientHierarchy = "NA > Tander > Magnit MM",
                ClientTreeId = 5000004,
                ClientTreeKeyId = 19,
                DeviationCoefficient = 10,
                EventId = Event.Id,
                EventName = Event.Name,
                Id = Guid.NewGuid(),
                InOutProductIds = "to change!!! 8af37b51-e73d-ea11-a86c-000d3a46085b;a5f37b51-e73d-ea11-a86c-000d3a46085b;",
                IsApolloExport = true,
                Name = "Unpublish Promo",
                NeedRecountUplift = true,
                ProductHierarchy = "Kitekat > Dry",
                ProductTreeObjectIds = "1000043",
                PromoStatusId = PromoStatusId,
                PromoTypesId = PromoTypesId,

            };
            return promo;
        }
        public static List<InputMLRS> GetInputMLRS(string pathfile, string delimiter)
        {
            var Lines = File.ReadAllLines(pathfile, Encoding.UTF8).ToList();
            List<InputMLRS> inputMLs = Lines
                   .Skip(1)
                   .Select(x => x.Split(char.Parse(delimiter)))
                   .Select(x => new InputMLRS
                   {
                       PromoId = int.Parse(x[0]),
                       PPG = x[1],
                       Format = x[2],
                       ZREP = int.Parse(x[3]),
                       StartDate = ChangeTimeZoneUtil.ResetTimeZone(DateTimeOffset.Parse(x[4])),
                       EndDate = ChangeTimeZoneUtil.ResetTimeZone(DateTimeOffset.Parse(x[5])),
                       MechanicMars = x[6],
                       DiscountMars = double.Parse(x[7], CultureInfo.InvariantCulture),
                       MechInstore = x[8],
                       InstoreDiscount = double.Parse(x[9], CultureInfo.InvariantCulture),
                       PlannedUplift = double.Parse(x[10], CultureInfo.InvariantCulture),
                       PlanInStoreShelfPrice = double.Parse(x[11], CultureInfo.InvariantCulture),
                       FormatCode = int.Parse(x[12]),
                       Source = x[13],
                       BaseLSV = double.Parse(x[14], CultureInfo.InvariantCulture),
                       TotalLSV = double.Parse(x[15], CultureInfo.InvariantCulture),
                   })
                   .Where(g => g.Source == "optimizer")
                   .ToList();
            return inputMLs;
        }
        public static List<InputMLRA> GetInputMLRA(string pathfile, string delimiter)
        {
            var Lines = File.ReadAllLines(pathfile, Encoding.UTF8).ToList();
            List<InputMLRA> inputMLs = Lines
                   .Skip(1)
                   .Select(x => x.Split(char.Parse(delimiter)))
                   .Select(x => new InputMLRA
                   {
                       PromoId = int.Parse(x[0]),
                       PPG = x[1],
                       Format = x[2],
                       ZREP = int.Parse(x[3]),
                       StartDate = ChangeTimeZoneUtil.ResetTimeZone(DateTimeOffset.Parse(x[4])),
                       EndDate = ChangeTimeZoneUtil.ResetTimeZone(DateTimeOffset.Parse(x[5])),
                       MechanicMars = x[6],
                       DiscountMars = double.Parse(x[7], CultureInfo.InvariantCulture),
                       MechInstore = x[8],
                       InstoreDiscount = double.Parse(x[9], CultureInfo.InvariantCulture),
                       PlannedUplift = double.Parse(x[10], CultureInfo.InvariantCulture),
                       PlanInStoreShelfPrice = double.Parse(x[11], CultureInfo.InvariantCulture),
                       FormatCode = int.Parse(x[12]),
                       Source = x[13],
                       Year = int.Parse(x[14], CultureInfo.InvariantCulture),
                   })
                   .Where(g => g.Source == "optimizer")
                   .ToList();
            return inputMLs;
        }
        public static string GetNamePromo(DatabaseContext context, Mechanic mechanic, ProductTree productTree, double MarsMechanicDiscount)
        {
            // доработать если нужен тип VP
            var promoNameProductTreeAbbreviations = "";

            if (productTree != null)
            {
                if (productTree.Type != "Brand")
                {
                    var currentTreeNode = productTree;
                    while (currentTreeNode != null && currentTreeNode.Type != "Brand")
                    {
                        currentTreeNode = context.Set<ProductTree>().FirstOrDefault(x => x.ObjectId == currentTreeNode.parentId);
                    }
                    promoNameProductTreeAbbreviations = currentTreeNode.Abbreviation;
                }
                promoNameProductTreeAbbreviations = promoNameProductTreeAbbreviations + " " + productTree.Abbreviation;
            }


            var promoNameMechanic = "";
            if (mechanic != null)
            {
                promoNameMechanic = mechanic.Name;
                if (mechanic.SystemName == "TPR" || mechanic.SystemName == "Other")
                {
                    promoNameMechanic += " " + MarsMechanicDiscount + "%";
                }
                else
                {
                    throw new NotImplementedException();
                    //promoNameMechanic += " " + promo.MarsMechanicType.Name;
                }
            }

            return promoNameProductTreeAbbreviations + " " + promoNameMechanic;
        }
        public static ReturnName GetNamePromo(Mechanic mechanic, Product product, double MarsMechanicDiscount, List<ProductTree> productTrees, List<Brand> brands, List<Technology> technologies)
        {
            // доработать если нужен тип VP
            var promoNameProductTreeAbbreviations = "";
            ProductTree productTreeTech = new ProductTree();
            if (product != null)
            {
                Brand brand = brands.FirstOrDefault(g => g.Brand_code == product.Brand_code);

                Technology technology = new Technology();
                string compositname;
                if (string.IsNullOrEmpty(product.SubBrand_code))
                {
                    technology = technologies.FirstOrDefault(g => g.Tech_code == product.Tech_code && g.SubBrand == null);
                    compositname = technology.Name;
                }
                else
                {
                    technology = technologies.FirstOrDefault(g => g.Tech_code == product.Tech_code && g.SubBrand_code == product.SubBrand_code);
                    compositname = technology.Name + " " + technology.SubBrand;
                }

                ProductTree productTreeBrand = productTrees.FirstOrDefault(g => g.BrandId == brand.Id);

                productTreeTech = productTrees.FirstOrDefault(g => g.parentId == productTreeBrand.ObjectId && g.Name == compositname);
                promoNameProductTreeAbbreviations = productTreeBrand.Abbreviation + " " + productTreeTech.Abbreviation;
            }


            var promoNameMechanic = "";
            if (mechanic != null)
            {
                promoNameMechanic = mechanic.Name;
                if (mechanic.SystemName == "TPR" || mechanic.SystemName == "Other")
                {
                    promoNameMechanic += " " + MarsMechanicDiscount + "%";
                }
                else
                {
                    throw new NotImplementedException();
                    //promoNameMechanic += " " + promo.MarsMechanicType.Name;
                }
            }

            return new ReturnName
            {
                Name = promoNameProductTreeAbbreviations + " " + promoNameMechanic,
                ProductTree = productTreeTech
            };
        }
        public class ReturnName
        {
            public string Name { get; set; }
            public ProductTree ProductTree { get; set; }
        }
        public static ClientDispatchDays GetClientDispatchDays(ClientTree clientTree)
        {
            ClientDispatchDays clientDispatchDays = new ClientDispatchDays();
            if (clientTree.IsBeforeStart != null)
            {
                if (clientTree.IsBeforeStart.Value)
                {
                    clientDispatchDays.IsStartAdd = false;
                }
                else
                {
                    clientDispatchDays.IsStartAdd = true;
                }
                if (clientTree.IsDaysStart.Value)
                {
                    clientDispatchDays.StartDays = clientTree.DaysStart.Value;
                }
                else
                {
                    clientDispatchDays.StartDays = clientTree.DaysStart.Value * 7;
                }
            }
            else
            {
                throw new Exception($"Client {clientTree.ObjectId} without dispatch dates");
            }
            if (clientTree.IsBeforeEnd != null)
            {
                if (clientTree.IsBeforeEnd.Value)
                {
                    clientDispatchDays.IsEndAdd = false;
                }
                else
                {
                    clientDispatchDays.IsEndAdd = true;
                }
                if (clientTree.IsDaysEnd.Value)
                {
                    clientDispatchDays.EndDays = clientTree.DaysEnd.Value;
                }
                else
                {
                    clientDispatchDays.EndDays = clientTree.DaysEnd.Value * 7;
                }
            }
            else
            {
                throw new Exception($"Client {clientTree.ObjectId} without dispatch dates");
            }

            return clientDispatchDays;
        }
        public class ClientDispatchDays
        {
            public int StartDays { get; set; }
            public bool IsStartAdd { get; set; } // true прибавляет дни
            public int EndDays { get; set; }
            public bool IsEndAdd { get; set; } // true прибавляет дни
        }
        public static Promo SavePromo(Promo model, DatabaseContext context, UserInfo user, RoleInfo role)
        {
            model.DeviationCoefficient /= 100;
            // делаем UTC +3
            model.StartDate = ChangeTimeZoneUtil.ResetTimeZone(model.StartDate);
            model.EndDate = ChangeTimeZoneUtil.ResetTimeZone(model.EndDate);
            model.DispatchesStart = ChangeTimeZoneUtil.ResetTimeZone(model.DispatchesStart);
            model.DispatchesEnd = ChangeTimeZoneUtil.ResetTimeZone(model.DispatchesEnd);

            DateTimeOffset? ChangedDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.Now);

            if (model.EventId == null)
            {
                Event promoEvent = context.Set<Event>().FirstOrDefault(x => !x.Disabled && x.Name == "Standard promo");
                if (promoEvent == null)
                {
                    throw new Exception("Event 'Standard promo' not found");
                }

                model.EventId = promoEvent.Id;
                model.EventName = promoEvent.Name;
            }
            else
            {
                Event promoEvent = context.Set<Event>().FirstOrDefault(x => !x.Disabled && x.Id == model.EventId);
                if (promoEvent == null)
                {
                    throw new Exception("Event not found");
                }
                model.EventName = promoEvent.Name;
            }

            string userRole = role.SystemName;

            string message;

            PromoStateContext promoStateContext = new PromoStateContext(context, null);
            bool status = promoStateContext.ChangeState(model, userRole, out message);

            if (!status)
            {
                throw new Exception(message);
            }

            Promo proxy = new Promo();
            Promo result = AutomapperProfiles.PromoCopy(model, proxy);

            if (result.CreatorId == null)
            {
                result.CreatorId = user.Id;
                result.CreatorLogin = user.Login;
            }

            context.Set<Promo>().Add(result);
            context.SaveChanges();

            List<Mechanic> mechanics = context.Set<Mechanic>().Where(g => !g.Disabled).ToList();
            List<MechanicType> mechanicTypes = context.Set<MechanicType>().Where(g => !g.Disabled).ToList();
            List<ClientTree> clientTrees = context.Set<ClientTree>().Where(g => g.EndDate == null).ToList();
            List<ProductTree> productTrees = context.Set<ProductTree>().Where(g => g.EndDate == null).ToList();
            List<Brand> brands = context.Set<Brand>().Where(g => !g.Disabled).ToList();
            List<Technology> technologies = context.Set<Technology>().Where(g => !g.Disabled).ToList();
            List<BrandTech> brandTeches = context.Set<BrandTech>().Where(g => !g.Disabled).ToList();
            List<Color> colors = context.Set<Color>().Where(g => !g.Disabled).ToList();
            // Добавление продуктов
            List<PromoProductTree> promoProductTrees = AddProductTrees(model.ProductTreeObjectIds, result, out bool isSubrangeChanged, context);

            //Установка полей по дереву ProductTree
            SetPromoByProductTree(result, promoProductTrees, productTrees, brands, technologies, brandTeches, colors);
            //Установка дат в Mars формате
            SetPromoMarsDates(result);
            //Установка полей по дереву ClientTree
            SetPromoByClientTree(result, clientTrees);
            //Установка механик
            SetMechanic(result, mechanics, mechanicTypes);
            SetMechanicIA(result, mechanics, mechanicTypes);

            //Установка начального статуса
            PromoStatusChange psc = context.Set<PromoStatusChange>().Create<PromoStatusChange>();
            psc.PromoId = result.Id;
            psc.StatusId = result.PromoStatusId;
            psc.UserId = user.Id;
            psc.RoleId = role.Id;
            psc.Date = DateTimeOffset.UtcNow;
            context.Set<PromoStatusChange>().Add(psc);

            //Установка времени последнгего присвоения статуса Approved
            if (result.PromoStatus != null && result.PromoStatus.SystemName == "Approved")
            {
                result.LastApprovedDate = ChangeTimeZoneUtil.ResetTimeZone(DateTimeOffset.UtcNow);
            }

            if (!result.CalculateML)
            {
                // Для draft не проверяем и не считаем && если у промо есть признак InOut, то Uplift считать не нужно.
                if (result.PromoStatus.SystemName.ToLower() != "draft")
                {
                    // если нет TI, COGS или продукты не подобраны по фильтрам, запретить сохранение (будет исключение)
                    List<Product> filteredProducts; // продукты, подобранные по фильтрам
                    CheckSupportInfo(result, promoProductTrees, out filteredProducts, context);
                    //создание отложенной задачи, выполняющей подбор аплифта и расчет параметров
                    CalculatePromo(result, context, (Guid)user.Id, (Guid)role.Id, false, false, false, true);
                }
                else
                {
                    // Добавить запись в таблицу PromoProduct при сохранении.
                    PlanProductParametersCalculation.SetPromoProduct(context.Set<Promo>().First(x => x.Id == result.Id).Id, context, out string error, true, promoProductTrees);
                    // Создаём инцидент для draft сразу
                    WritePromoDemandChangeIncident(context, result);
                }
            }

            // привязывает дочерние промо
            if (!string.IsNullOrEmpty(result.LinkedPromoes) && result.IsInExchange)
            {
                List<string> LinkedStringIds = model.LinkedPromoes.Split(',').ToList();
                List<int> LinkedIds = LinkedStringIds.Select(s => int.Parse(s)).ToList();
                List<Promo> ChildPromoes = context.Set<Promo>().Where(g => LinkedIds.Contains((int)g.Number) && !g.Disabled).ToList();
                foreach (var ChildPromo in ChildPromoes)
                {
                    ChildPromo.MasterPromoId = result.Id;
                }
            }

            result.LastChangedDate = ChangedDate;
            context.SaveChanges();
            if (result.TPMmode == TPMmode.RS && !result.CalculateML)
            {
                ScenarioHelper.CreateScenarioPeriod(result, context, TPMmode.RS);
            }
            if (result.TPMmode == TPMmode.RA && !result.CalculateML)
            {
                ScenarioHelper.CreateScenarioPeriod(result, context, TPMmode.RA);
            }
            return result;
        }
        /// <summary>
        /// Добавить продукты из иерархии к промо
        /// </summary>
        /// <param name="objectIds">Список ObjectId продуктов в иерархии</param>
        /// <param name="promo">Промо к которому прикрепляются продукты</param>
        public static List<PromoProductTree> AddProductTrees(string objectIds, Promo promo, out bool isSubrangeChanged, DatabaseContext context)
        {
            // сформированный список продуктов - приходится использовать из-за отказа SaveChanges
            List<PromoProductTree> currentProducTrees = context.Set<PromoProductTree>().Where(n => n.PromoId == promo.Id && !n.Disabled).ToList();
            List<string> currentProducTreesIds = currentProducTrees.Select(x => x.ProductTreeObjectId.ToString()).ToList();
            List<string> newProductTreesIds = new List<string>();

            if (!string.IsNullOrEmpty(objectIds))
            {
                newProductTreesIds = objectIds.Split(';').ToList();
            }
            List<string> dispatchIds = currentProducTreesIds.Except(newProductTreesIds).ToList();
            dispatchIds.AddRange(newProductTreesIds.Except(currentProducTreesIds));
            if (dispatchIds.Count == 0)
            {
                isSubrangeChanged = false;
            }
            else
            {
                isSubrangeChanged = true;
            }

            // Если Null, значит продукты не менялись
            if (objectIds != null)
            {
                List<int> productTreeObjectIds = new List<int>();

                if (objectIds.Length > 0)
                {
                    productTreeObjectIds = objectIds.Split(';').Select(n => Int32.Parse(n)).ToList();
                }

                // находим прежние записи, если они остались то ислючаем их из нового списка
                // иначе удаляем
                //var oldRecords = context.Set<PromoProductTree>().Where(n => n.PromoId == promo.Id && !n.Disabled);
                //PromoProductTree[] oldRecords = new PromoProductTree[currentProducTrees.Count];
                //currentProducTrees.CopyTo(oldRecords);
                foreach (var rec in currentProducTrees)
                {
                    int index = productTreeObjectIds.IndexOf(rec.ProductTreeObjectId);

                    if (index >= 0)
                    {
                        productTreeObjectIds.RemoveAt(index);
                    }
                    else
                    {
                        rec.DeletedDate = System.DateTime.Now;
                        rec.Disabled = true;
                    }
                }

                // Добавляем новые продукты в промо
                foreach (int objectId in productTreeObjectIds)
                {
                    PromoProductTree promoProductTree = new PromoProductTree()
                    {
                        Id = Guid.NewGuid(),
                        ProductTreeObjectId = objectId,
                        Promo = promo,
                        TPMmode = promo.TPMmode
                    };

                    currentProducTrees.Add(promoProductTree);
                    context.Set<PromoProductTree>().Add(promoProductTree);
                }
            }

            return currentProducTrees.Where(n => !n.Disabled).ToList();
        }
        /// <summary>
        /// Установка в промо цвета, бренда и BrandTech на основании дерева продуктов
        /// </summary>
        /// <param name="promo"></param>
        public static void SetPromoByProductTree(Promo promo, List<PromoProductTree> promoProducts, List<ProductTree> productTrees, List<Brand> brands, List<Technology> technologies, List<BrandTech> brandTeches, List<Color> colors)
        {
            PromoProductTree product = promoProducts.FirstOrDefault();
            DateTime dt = DateTime.Now;
            if (product != null)
            {
                //Заполнение Subranges
                IEnumerable<ProductTree> ptQuery = productTrees.Where(x => x.Type == "root"
                    || (DateTime.Compare(x.StartDate, dt) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dt) > 0)));
                IEnumerable<int> promoProductsPTOIds = promoProducts.Select(z => z.ProductTreeObjectId);
                IEnumerable<ProductTree> pts = ptQuery.Where(y => promoProductsPTOIds.Contains(y.ObjectId));
                promo.ProductSubrangesList = string.Join(";", pts.Where(x => x.Type == "Subrange").Select(z => z.Name));
                promo.ProductSubrangesListRU = string.Join(";", pts.Where(x => x.Type == "Subrange").Select(z => z.Description_ru));

                int objectId = product.ProductTreeObjectId;
                ProductTree pt = productTrees.FirstOrDefault(x => (x.StartDate < dt && (x.EndDate > dt || !x.EndDate.HasValue)) && x.ObjectId == objectId);
                if (pt != null)
                {
                    Guid? BrandId = null;
                    Guid? TechId = null;
                    Brand brandTo = null;
                    bool end = false;
                    do
                    {
                        if (pt.Type == "Brand")
                        {
                            brandTo = brands.FirstOrDefault(x => x.Name == pt.Name);
                            if (brandTo != null)
                            {
                                BrandId = brandTo.Id;
                                promo.BrandId = brandTo.Id;
                            }
                        }
                        if (pt.Type == "Technology")
                        {
                            var tech = technologies.FirstOrDefault(x => (x.Name + " " + x.SubBrand).Trim() == pt.Name.Trim());
                            if (tech != null)
                            {
                                TechId = tech.Id;
                                promo.TechnologyId = tech.Id;
                            }
                        }
                        if (pt.parentId != 1000000)
                        {
                            pt = productTrees.FirstOrDefault(x => (x.StartDate < dt && (x.EndDate > dt || !x.EndDate.HasValue)) && x.ObjectId == pt.parentId);
                        }
                        else
                        {
                            end = true;
                        }
                    } while (!end && pt != null);

                    if (brandTo == null)
                    {
                        promo.BrandId = null;
                    }

                    BrandTech bt = brandTeches.FirstOrDefault(x => !x.Disabled && x.TechnologyId == TechId && x.BrandId == BrandId);
                    if (bt != null)
                    {
                        promo.BrandTechId = bt.Id;
                        var color = colors.Where(x => !x.Disabled && x.BrandTechId == bt.Id).ToList();
                        if (color.Count() == 1)
                        {
                            promo.ColorId = color.First().Id;
                        }
                        else
                        {
                            promo.ColorId = null;
                        }
                    }
                    else
                    {
                        promo.ColorId = null;
                        promo.BrandTechId = null;
                    }
                }
                else
                {
                    promo.ColorId = null;
                }
            }
        }
        //Простановка дат в формате Mars
        public static void SetPromoMarsDates(Promo promo)
        {
            string stringFormatYP2WD = "{0}P{1:D2}W{2}D{3}";

            if (promo.StartDate != null)
            {
                promo.MarsStartDate = (new MarsDate((DateTimeOffset)promo.StartDate)).ToString(stringFormatYP2WD);
            }
            if (promo.EndDate != null)
            {
                promo.MarsEndDate = (new MarsDate((DateTimeOffset)promo.EndDate)).ToString(stringFormatYP2WD);
            }
            if (promo.EndDate != null && promo.StartDate != null)
            {
                promo.PromoDuration = (promo.EndDate - promo.StartDate).Value.Days + 1;
            }
            else
            {
                promo.PromoDuration = null;
            }

            if (promo.DispatchesStart != null)
            {
                promo.MarsDispatchesStart = (new MarsDate((DateTimeOffset)promo.DispatchesStart)).ToString(stringFormatYP2WD);
            }
            if (promo.DispatchesEnd != null)
            {
                promo.MarsDispatchesEnd = (new MarsDate((DateTimeOffset)promo.DispatchesEnd)).ToString(stringFormatYP2WD);
            }
            if (promo.DispatchesStart != null && promo.DispatchesEnd != null)
            {
                promo.DispatchDuration = (promo.DispatchesEnd - promo.DispatchesStart).Value.Days + 1;
            }
            else
            {
                promo.DispatchDuration = null;
            }
        }
        /// <summary>
        /// Установка промо по дереву клиентов
        /// </summary>
        /// <param name="promo"></param>
        public static void SetPromoByClientTree(Promo promo, List<ClientTree> clientTrees)
        {
            int? ClientTreeId = promo.ClientTreeId;
            string resultMultiBaseStr = "";
            if (promo.ClientTreeId != null)
            {
                int? upBaseClientId = RecursiveUpBaseClientsFind(ClientTreeId, clientTrees);
                if (upBaseClientId.HasValue)
                {
                    resultMultiBaseStr = upBaseClientId.ToString();
                }
                else
                {
                    resultMultiBaseStr =
                        string.Join("|", RecursiveDownBaseClientsFind(promo.ClientTreeId, clientTrees));
                }

            }
            promo.BaseClientTreeIds = resultMultiBaseStr;
        }
        /// <summary>
        /// Поиск базовых клиентов в дереве в корень
        /// </summary>
        /// <param name="clientTreeId"></param>
        /// <returns></returns>
        private static int? RecursiveUpBaseClientsFind(int? clientTreeId, List<ClientTree> clientTrees)
        {
            if (!clientTreeId.HasValue)
            {
                return null;
            }
            else
            {
                ClientTree ctn = clientTrees.FirstOrDefault(x => x.ObjectId == clientTreeId &&
                 (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, DateTime.Now) > 0));
                if (ctn == null)
                {
                    return null;
                }
                else if (ctn.IsBaseClient)
                {
                    return ctn.ObjectId;
                }
                else if (ctn.ObjectId == ctn.parentId)
                {
                    return null;
                }
                else
                {
                    return RecursiveUpBaseClientsFind(ctn.parentId, clientTrees);
                }

            }
        }


        /// <summary>
        /// Поиск базовых клиентов в дереве назад
        /// </summary>
        /// <param name="clientTreeId"></param>
        /// <returns></returns>
        private static List<int> RecursiveDownBaseClientsFind(int? clientTreeId, List<ClientTree> clientTrees)
        {
            if (!clientTreeId.HasValue)
            {
                return new List<int>();
            }
            else
            {
                ClientTree ct = clientTrees.FirstOrDefault(x => x.ObjectId == clientTreeId &&
                 (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, DateTime.Now) > 0));
                if (ct.IsBaseClient)
                {
                    return new List<int>() { ct.ObjectId };
                }

                List<ClientTree> ctChilds = clientTrees.Where(
                 x =>
                 //DateTime.Compare(x.StartDate, ct.StartDate) <= 0 && 
                 x.parentId == ct.ObjectId &&
                 (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, DateTime.Now) > 0)).ToList();

                List<int> res = new List<int>();
                res.AddRange(ctChilds.Where(y => y.IsBaseClient).Select(z => z.ObjectId).ToList());

                foreach (var item in ctChilds.Where(y => !y.IsBaseClient))
                {
                    res.AddRange(RecursiveDownBaseClientsFind(item.ObjectId, clientTrees));
                }
                return res;
            }
        }
        /// <summary>
        /// Установка значения единого поля для Mechanic
        /// </summary>
        /// <param name="promo"></param>
        public static void SetMechanic(Promo promo, List<Mechanic> mechanics, List<MechanicType> mechanicTypes)
        {
            // нет механики - нет остального
            if (promo.MarsMechanicId != null)
            {
                var mechanic = mechanics.FirstOrDefault(g => g.Id == promo.MarsMechanicId);
                string result = mechanic.Name;

                if (promo.MarsMechanicTypeId != null)
                {
                    var mechanicType = mechanicTypes.FirstOrDefault(g => g.Id == promo.MarsMechanicTypeId);
                    result += " " + mechanicType.Name;
                }

                if (promo.MarsMechanicDiscount != null)
                    result += " " + promo.MarsMechanicDiscount + "%";

                promo.Mechanic = result;
            }
        }

        /// <summary>
        /// Установка значения единого поля для Mechanic IA
        /// </summary>
        /// <param name="promo"></param>
        public static void SetMechanicIA(Promo promo, List<Mechanic> mechanics, List<MechanicType> mechanicTypes)
        {
            string result = null;

            // нет механики - нет остального
            if (promo.PlanInstoreMechanicId != null)
            {
                var mechanic = mechanics.FirstOrDefault(g => g.Id == promo.PlanInstoreMechanicId);
                result = mechanic.Name;

                if (promo.PlanInstoreMechanicTypeId != null)
                {
                    var mechanicType = mechanicTypes.FirstOrDefault(g => g.Id == promo.PlanInstoreMechanicTypeId);
                    result += " " + mechanicType.Name;
                }

                if (promo.MarsMechanicDiscount != null)
                    result += " " + promo.PlanInstoreMechanicDiscount + "%";
            }

            promo.MechanicIA = result;
        }
        /// <summary>
        /// Проверка TI, COGS и наличия продуктов, попадающих под фильтрация
        /// </summary>
        /// <param name="promo">Проверяемое промо</param>
        /// <param name="promoProductTrees">Список узлов продуктового дерева</param>
        /// <exception cref="Exception">Исключение генерируется при отсутсвии одного из проверяемых параметров</exception>
        public static void CheckSupportInfo(Promo promo, List<PromoProductTree> promoProductTrees, out List<Product> products, DatabaseContext context, IQueryable<Product> productQuery = null)
        {
            List<string> messagesError = new List<string>();
            string message = null;
            bool error;
            bool isProductListEmpty = false;
            products = null;

            // проверка на наличие TI
            IQueryable<TradeInvestment> TIQuery = context.Set<TradeInvestment>().Where(x => !x.Disabled);
            SimplePromoTradeInvestment simplePromoTradeInvestment = new SimplePromoTradeInvestment(promo);
            PromoUtils.GetTIBasePercent(simplePromoTradeInvestment, context, TIQuery, out message, out error);
            if (message != null && error)
            {
                messagesError.Add(message);
                message = null;
            }
            else if (message != null)
            {
                throw new Exception(message);
            }

            // проверка на наличие COGS
            //if (promo.IsLSVBased)
            //{
            IQueryable<COGS> cogsQuery = context.Set<COGS>().Where(x => !x.Disabled);
            SimplePromoCOGS simplePromoCOGS = new SimplePromoCOGS(promo);
            PromoUtils.GetCOGSPercent(simplePromoCOGS, context, cogsQuery, out message);
            //}
            if (message != null)
            {
                messagesError.Add(message);
                message = null;
            }

            IQueryable<PlanCOGSTn> cogsTnQuery = context.Set<PlanCOGSTn>().Where(x => !x.Disabled);
            simplePromoCOGS = new SimplePromoCOGS(promo);
            PromoUtils.GetCOGSTonCost(simplePromoCOGS, context, cogsTnQuery, out message);

            if (message != null)
            {
                messagesError.Add(message);
                message = null;
            }

            // проверка на наличие продуктов, попадающих под фильтр (для промо не из TLC)
            if (!promo.LoadFromTLC)
            {
                if (promo.InOut.HasValue && promo.InOut.Value)
                {
                    products = PlanProductParametersCalculation.GetCheckedProducts(context, promo, productQuery);
                }
                else
                {
                    isProductListEmpty = PlanProductParametersCalculation.IsProductListEmpty(promo, context, out message, promoProductTrees);
                }
                if (message != null)
                {
                    messagesError.Add(message);
                }

                if (isProductListEmpty)
                {
                    messagesError.Add("Product list is empty. Please, check assortment matrix or/and Dispath period");
                }
            }

            // если что-то не найдено, то генерируем ошибку
            if (messagesError.Count > 0)
            {
                string messageError = "";
                for (int i = 0; i < messagesError.Count; i++)
                {
                    string endString = i == messagesError.Count - 1 ? "" : " ";
                    messageError += messagesError[i] + endString;
                }

                throw new Exception(messageError);
            }
        }
        /// <summary>
        /// Создание отложенной задачи, выполняющей подбор аплифта и расчет параметров промо и продуктов
        /// </summary>
        /// <param name="promo"></param>
        public static void CalculatePromo(Promo promo, DatabaseContext context, Guid userId, Guid roleId, bool needCalculatePlanMarketingTI, bool needResetUpliftCorrections, bool needResetUpliftCorrectionsPI, bool createDemandIncidentCreate = false, bool createDemandIncidentUpdate = false, string oldMarsMechanic = null, double? oldMarsMechanicDiscount = null, DateTimeOffset? oldDispatchesStart = null, double? oldPlanPromoUpliftPercent = null, double? oldPlanPromoIncrementalLSV = null)
        {
            HandlerData data = new HandlerData();
            HandlerDataHelper.SaveIncomingArgument("PromoId", promo.Id, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("NeedCalculatePlanMarketingTI", needCalculatePlanMarketingTI, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("needResetUpliftCorrections", needResetUpliftCorrections, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("needResetUpliftCorrectionsPI", needResetUpliftCorrectionsPI, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("createDemandIncidentCreate", createDemandIncidentCreate, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("createDemandIncidentUpdate", createDemandIncidentUpdate, data, visible: false, throwIfNotExists: false);

            HandlerDataHelper.SaveIncomingArgument("oldMarsMechanic", oldMarsMechanic, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("oldMarsMechanicDiscount", oldMarsMechanicDiscount, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("oldDispatchesStart", oldDispatchesStart, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("oldPlanPromoUpliftPercent", oldPlanPromoUpliftPercent, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("oldPlanPromoIncrementalLSV", oldPlanPromoIncrementalLSV, data, visible: false, throwIfNotExists: false);


            bool success = CalculationTaskManager.CreateCalculationTask(CalculationTaskManager.CalculationAction.Uplift, data, context, promo.Id);

            if (!success)
                throw new Exception("Promo was blocked for calculation");
        }
        public static Promo SaveMLPromo(Promo model, DatabaseContext context, UserInfo user, RoleInfo role, List<Mechanic> mechanics, List<MechanicType> mechanicTypes, List<ClientTree> clientTrees,
            List<ProductTree> productTrees, List<Brand> brands, List<Technology> technologies, List<BrandTech> brandTeches, List<Color> colors)
        {
            model.DeviationCoefficient /= 100;
            // делаем UTC +3
            model.StartDate = ChangeTimeZoneUtil.ResetTimeZone(model.StartDate);
            model.EndDate = ChangeTimeZoneUtil.ResetTimeZone(model.EndDate);
            model.DispatchesStart = ChangeTimeZoneUtil.ResetTimeZone(model.DispatchesStart);
            model.DispatchesEnd = ChangeTimeZoneUtil.ResetTimeZone(model.DispatchesEnd);

            DateTimeOffset? ChangedDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.Now);

            string userRole = role.SystemName;

            string message;

            PromoStateContext promoStateContext = new PromoStateContext(context, null);
            bool status = promoStateContext.ChangeState(model, userRole, out message);

            if (!status)
            {
                throw new Exception(message);
            }

            Promo proxy = new Promo();
            Promo result = AutomapperProfiles.PromoCopy(model, proxy);

            if (result.CreatorId == null)
            {
                result.CreatorId = user.Id;
                result.CreatorLogin = user.Login;
            }

            //context.Set<Promo>().Add(result);
            //context.SaveChanges();
            // Добавление продуктов
            List<PromoProductTree> promoProductTrees = AddProductTrees(model.ProductTreeObjectIds, result, out bool isSubrangeChanged, context);

            //Установка полей по дереву ProductTree
            SetPromoByProductTree(result, promoProductTrees, productTrees, brands, technologies, brandTeches, colors);
            //Установка дат в Mars формате
            SetPromoMarsDates(result);
            //Установка полей по дереву ClientTree
            SetPromoByClientTree(result, clientTrees);
            //Установка механик
            SetMechanic(result, mechanics, mechanicTypes);
            SetMechanicIA(result, mechanics, mechanicTypes);

            //Установка начального статуса
            PromoStatusChange psc = context.Set<PromoStatusChange>().Create<PromoStatusChange>();
            psc.PromoId = result.Id;
            psc.StatusId = result.PromoStatusId;
            psc.UserId = user.Id;
            psc.RoleId = role.Id;
            psc.Date = DateTimeOffset.UtcNow;
            context.Set<PromoStatusChange>().Add(psc);

            //Установка времени последнгего присвоения статуса Approved
            if (result.PromoStatus != null && result.PromoStatus.SystemName == "Approved")
            {
                result.LastApprovedDate = ChangeTimeZoneUtil.ResetTimeZone(DateTimeOffset.UtcNow);
            }

            result.LastChangedDate = ChangedDate;
            return result;
        }
    }
}