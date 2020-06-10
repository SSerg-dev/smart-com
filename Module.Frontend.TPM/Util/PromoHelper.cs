using Core.Dependency;
using Core.Security.Models;
using Core.Settings;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using Utility;

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
            //необходимо удалить все коррекции
            var promoProductToDeleteListIds = promoProductToDeleteList.Select(x => x.Id).ToList();
            List<PromoProductsCorrection> promoProductCorrectionToDeleteList = Context.Set<PromoProductsCorrection>()
                .Where(x => promoProductToDeleteListIds.Contains(x.PromoProductId) && x.Disabled != true).ToList();
            foreach (PromoProductsCorrection promoProductsCorrection in promoProductCorrectionToDeleteList)
            {
                promoProductsCorrection.DeletedDate = DateTimeOffset.UtcNow;
                promoProductsCorrection.Disabled = true;
                promoProductsCorrection.UserId = (Guid)user.Id;
                promoProductsCorrection.UserName = user.Login;
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
                                                                    x.PromoStatus.SystemName != "Cancelled" );

                        Parallel.ForEach(Promoes, promo => {
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

                        Parallel.ForEach(Promoes, promo => {
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

        public static void CalculateInvoiceTotalProduct(DatabaseContext context, Promo promo)
        {
            // Получаем все записи из таблицы PromoProduct для текущего промо.
            var promoProductsForCurrentPromo = context.Set<PromoProduct>()
                .Where(x => x.PromoId == promo.Id);

            double sumActualProductPCQty = 0; 
            // Доля от всего InvoiceTotal
            double invoiceTotalProductPart = 0;

            sumActualProductPCQty = Convert.ToDouble(promoProductsForCurrentPromo.Select(p => p.ActualProductPCQty).Sum());

            // Перебираем все найденные для текущего промо записи из таблицы PromoProduct.
            foreach (var promoProduct in promoProductsForCurrentPromo)
            {
                // Если ActualProductPCQty нет, то мы не сможем посчитать долю
                if (promoProduct.ActualProductPCQty.HasValue)
                {
                    invoiceTotalProductPart = 0;
                    // Если показатель ActualProductPCQty == 0, то он составляет 0 процентов от показателя PlanPromoBaselineLSV.
                    if (promoProduct.ActualProductPCQty.Value != 0)
                    {
                        // Считаем долю ActualProductPCQty от InvoiceTotal.
                        invoiceTotalProductPart = promoProduct.ActualProductPCQty.Value/sumActualProductPCQty;
                    }
                    // Устанавливаем InvoiceTotalProduct в запись таблицы PromoProduct.
                    promoProduct.InvoiceTotalProduct = invoiceTotalProductPart*promo.InvoiceTotal;

                }
                else
                {
                    promoProduct.InvoiceTotalProduct = 0;
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
                new Column() { Order = 0, Field = "IsGrowthAcceleration", Header = "Growth acceleration", Quoting = false },
                new Column() { Order = 0, Field = "IsApolloExport", Header = "Apollo export", Quoting = false },
                new Column() { Order = 0, Field = "Name", Header = "Promo name", Quoting = false },
                new Column() { Order = 0, Field = "BrandTech.Name", Header = "Brandtech", Quoting = false },
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
                new Column() { Order = 0, Field = "IsOnInvoice", Header = "Is On Invoice", Quoting = false },

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
    }
}