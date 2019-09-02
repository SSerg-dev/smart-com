using ProcessingHost.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using Looper.Core;
using Module.Persist.TPM.Model.TPM;
using Looper.Parameters;
using Persist;
using Utility.LogWriter;
using System.Diagnostics;
using System.Data;
using Core.Settings;
using Core.Dependency;
using Module.Persist.TPM.Utils;

namespace Module.Host.TPM.Handlers {
    /// <summary>
    /// Класс для подбора подходящих промо и вычисления планового аплифта
    /// </summary>
    public class UpdateUpliftHandler : BaseHandler {
        public override void Action(HandlerInfo info, ExecuteData data) {
            ILogWriter handlerLogger = null;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try {
                handlerLogger = new FileLogWriter(info.HandlerId.ToString());
                handlerLogger.Write(true, String.Format("The uplift calculation started at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");
                var promoId = HandlerDataHelper.GetIncomingArgument<Guid>("PromoId", info.Data, false);

                using (DatabaseContext context = new DatabaseContext()) {
                    //экстра-места - физические ДМП(X-sites) - проверка на наличие X-sites
                    //экстра-места - каталоги(Catalog) - проверка на наличие Catalog
                    var promoXsites = context.Set<Promo>().Where(x => x.ActualPromoXSites != null && !x.Disabled);
                    var promoCatalog = context.Set<Promo>().Where(x => x.ActualPromoCatalogue != null && !x.Disabled);

                    var promoQuery = promoXsites.Intersect(promoCatalog);

                    if (promoQuery.Count() != 0) {
                        var currentPromo = context.Set<Promo>().Where(x => x.Id == promoId).FirstOrDefault();

                        //выбираем закрытые промо (дата окончания в пределах N лет до текущей даты)
                        ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                        int closedPromoPeriod = settingsManager.GetSetting<int>("CLOSED_PROMO_PERIOD_YEARS", 1);
                        var now = DateTimeOffset.Now;
                        var someYearsLaterDate = new DateTimeOffset(now.Year - closedPromoPeriod, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Offset);
                        var closedPromoStatusId = context.Set<PromoStatus>().Where(x => x.SystemName == "Closed").FirstOrDefault().Id;
                        promoQuery = promoQuery.Where(x => x.PromoStatusId == closedPromoStatusId && x.EndDate.HasValue && x.EndDate >= someYearsLaterDate && !x.Disabled);

                        //var productTreeArray = context.Set<ProductTree>().Where(x => context.Set<PromoProductTree>().Where(p => p.PromoId == currentPromo.Id && !p.Disabled).Any(p => p.ProductTreeObjectId == x.ObjectId && /*x.Type == "Subrange" &&*/ !x.EndDate.HasValue)).ToArray();

                        //---получение списка исторических промо с таким же набором Subrange---
                        var promoProductTreeArray = context.Set<PromoProductTree>().Where(p => p.PromoId == currentPromo.Id && !p.Disabled);
                        List<Promo> promoSubrangeList = null;
                        foreach (var promoProductTree in promoProductTreeArray) {
                            //выбор узлов типа Subrange текущего промо
                            ProductTree productTree = context.Set<ProductTree>().Where(x => x.ObjectId == promoProductTree.ProductTreeObjectId && x.Type == "Subrange" && !x.EndDate.HasValue).FirstOrDefault();
                            if (productTree != null) {
                                IEnumerable<Promo> promoSubrage = promoQuery.ToArray().Where(x => context.Set<PromoProductTree>().Where(p => p.ProductTreeObjectId == productTree.ObjectId && !p.Disabled).Any(p => x.Id == p.PromoId));
                                if (promoSubrangeList == null) {
                                    promoSubrangeList = promoSubrage.ToList();
                                } else {
                                    promoSubrangeList = promoSubrangeList.Intersect(promoSubrage).ToList();
                                }
                            }
                        }

                        if (promoSubrangeList != null) {
                            Promo[] _promoSubrangeList = new Promo[promoSubrangeList.Count()];
                            promoSubrangeList.CopyTo(_promoSubrangeList);

                            // для каждого промо, которое оказалось в promoSubrangeList проверяем, совпадает ли количество записей в таблице PromoProductTree с таким же PromoId и количество записей в 
                            // promoProductTreeArray(т.е. число выбранных узлов в текущем промо), таким образом проверяется, соотвествует ли каждое промо в полученном списке promoSubrangeList
                            // полному набору сабренжей текущего промо
                            foreach (var promoSubrange in _promoSubrangeList) {
                                // TODO: проверить работу с for без _promoSubrangeList
                                var query = context.Set<PromoProductTree>().Where(x => x.PromoId == promoSubrange.Id && !x.Disabled);
                                if (query.Count() != promoProductTreeArray.Count()) {
                                    promoSubrangeList.Remove(promoSubrange);
                                }
                            }
                        }

                        // если в promoSubrangeList остались промо, то Subrange этих промо полностью соответствуют Subrange текущего промо
                        if (promoSubrangeList != null && promoSubrangeList.Count() != 0) {
                            double? countedPlanUplift = 0;
                            bool success = CalculatePlanUplift(promoSubrangeList, currentPromo, out countedPlanUplift);
                            if (success) {
                                currentPromo.PlanPromoUpliftPercent = countedPlanUplift;
                                context.SaveChanges();

                                //handlerLogger.Write(true, String.Format("Для расчета uplift найдено {0} Promo", promoQuery.Count()));
                                handlerLogger.Write(true, String.Format("Calculated uplift value: {0}%", countedPlanUplift), "Message");
                            }

                        }
                        // если не найдено исторических промо с таким же набором сабренжей, то производится поиск исторических промо отдельно по каждому сабренжу текущего промо
                        else {
                            double? countedPlanUplift = 0;
                            bool singleSubrangeSuccess = true;
                            List<Promo> promoSingleSubrangeList = null;
                            foreach (var promoProductTree in promoProductTreeArray) {
                                //выбор узлов типа Subrange текущего промо
                                var productTree = context.Set<ProductTree>().Where(x => x.ObjectId == promoProductTree.ProductTreeObjectId && x.Type == "Subrange" && !x.EndDate.HasValue).FirstOrDefault();
                                if (productTree != null) {
                                    promoSingleSubrangeList = promoQuery.ToArray().Where(x => context.Set<PromoProductTree>().Where(p => p.ProductTreeObjectId == productTree.ObjectId && !p.Disabled).Any(p => x.Id == p.PromoId)).ToList();

                                    Promo[] _promoSingleSubrangeList = new Promo[promoSingleSubrangeList.Count()];
                                    promoSingleSubrangeList.CopyTo(_promoSingleSubrangeList);
                                    //остаются те промо, у которых есть только проверяемый сабредж
                                    foreach (var promoSingleSubrange in _promoSingleSubrangeList) {
                                        var query = context.Set<PromoProductTree>().Where(x => x.PromoId == promoSingleSubrange.Id);
                                        if (query.Count() > 1) {
                                            promoSingleSubrangeList.Remove(promoSingleSubrange);
                                        }
                                    }
                                }

                                if (promoSingleSubrangeList != null && promoSingleSubrangeList.Count() != 0 && singleSubrangeSuccess) {
                                    double? singleSubrangeCountedPlanUplift = 0;
                                    bool success = CalculatePlanUplift(promoSingleSubrangeList, currentPromo, out singleSubrangeCountedPlanUplift);
                                    if (success) {
                                        countedPlanUplift += singleSubrangeCountedPlanUplift;
                                    } else {
                                        singleSubrangeSuccess = false;
                                    }
                                } else {
                                    singleSubrangeSuccess = false;
                                }
                            }

                            if (singleSubrangeSuccess) {
                                currentPromo.PlanPromoUpliftPercent = countedPlanUplift / promoProductTreeArray.Count();
                                context.SaveChanges();

                                //handlerLogger.Write(true, String.Format("Для расчета uplift найдено {0} Promo", promoQuery.Count()));
                                handlerLogger.Write(true, String.Format("Calculated uplift value: {0}%", countedPlanUplift), "Message");
                            }
                            // если не найдено исторических промо по каждому сабренжу отдельно, то осуществляется поиск исторических промо на основе брендтех текущего промо
                            else {
                                Guid? brandId, technologyId;
                                GetBrandTechnologyGuid(context, currentPromo, out brandId, out technologyId);
                                promoQuery = promoQuery.Where(x => x.BrandId == brandId && x.TechnologyId == technologyId);

                                double? brandTechCountedPlanUplift = 0;
                                bool success = CalculatePlanUplift(promoQuery.ToList(), currentPromo, out brandTechCountedPlanUplift);
                                if (success) {
                                    currentPromo.PlanPromoUpliftPercent = brandTechCountedPlanUplift;
                                    context.SaveChanges();

                                    //handlerLogger.Write(true, String.Format("Для расчета uplift найдено {0} Promo", promoQuery.Count()));
                                    handlerLogger.Write(true, String.Format("Calculated uplift value: {0}%", brandTechCountedPlanUplift), "Message");
                                } else {
                                    handlerLogger.Write(true, "No Promo found to calculate Uplift", "Warning");
                                    if (!currentPromo.InOut.HasValue || !currentPromo.InOut.Value) {
                                        WriteUpliftIncident(promoId, context);
                                    }
                                }
                            }
                        }
                    } else {
                        handlerLogger.Write(true, "No Promo found to calculate Uplift", "Warning");
                        WriteUpliftIncident(promoId, context);
                    }
                }
            } catch (Exception e) {
                if (handlerLogger != null) {
                    handlerLogger.Write(true, e.ToString(), "Error");
                }
            } finally {
                sw.Stop();
                if (handlerLogger != null) {
                    handlerLogger.Write(true, String.Format("The uplift calculation ended at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} second", DateTimeOffset.Now, 1/*, sw.Elapsed.TotalSeconds*/), "Message");
                }
            }
        }

        private void WriteUpliftIncident(Guid promoId, DatabaseContext context) {
            context.Set<PromoUpliftFailIncident>().Add(new PromoUpliftFailIncident() { PromoId = promoId, CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow) });
            context.SaveChanges();
        }

        private static bool CalculatePlanUplift(List<Promo> promoList, Promo currentPromo, out double? countedPlanUplift) {
            if (promoList.Count() != 0) {
                //промо, подходящие по механике
                promoList = promoList.Where(x => x.MarsMechanicId == currentPromo.MarsMechanicId).ToList();

                if (promoList.Count() != 0) {
                    //промо, подходящие по скидке
                    double? marsMechanicDiscountLeft = currentPromo.MarsMechanicDiscount - 3 < 0 ? 0 : currentPromo.MarsMechanicDiscount - 3;
                    double? marsMechanicDiscountRight = currentPromo.MarsMechanicDiscount + 3;

                    promoList = promoList.Where(x => (x.MarsMechanicDiscount <= marsMechanicDiscountRight && x.MarsMechanicDiscount >= marsMechanicDiscountLeft)).ToList();

                    if (promoList.Count() != 0) {
                        //промо, подходящие по длительности
                        if (currentPromo.StartDate != null && currentPromo.EndDate != null) {
                            int currentPromoDuration = (currentPromo.EndDate.Value - currentPromo.StartDate.Value).Days;
                            int currentPromoDurationLeft = currentPromoDuration - 3;
                            int currentPromoDurationRight = currentPromoDuration + 3;
                            promoList = promoList.Where(x => x.StartDate.HasValue && x.EndDate.HasValue &&
                                                         ((x.EndDate.Value - x.StartDate.Value).Days <= currentPromoDurationRight && (x.EndDate.Value - x.StartDate.Value).Days >= currentPromoDurationLeft)).ToList();

                            if (promoList.Count() != 0) {
                                double? factUpliftSum = 0;
                                foreach (Promo promo in promoList) {
                                    factUpliftSum += promo.ActualPromoUpliftPercent;
                                }
                                countedPlanUplift = factUpliftSum / promoList.Count();
                                return true;
                            } else {
                                countedPlanUplift = 0;
                                return false;
                            }
                        } else {
                            countedPlanUplift = 0;
                            return false;
                        }
                    } else {
                        countedPlanUplift = 0;
                        return false;
                    }
                } else {
                    countedPlanUplift = 0;
                    return false;
                }
            } else {
                countedPlanUplift = 0;
                return false;
            }
        }

        private static void GetBrandTechnologyGuid(DatabaseContext context, Promo promo, out Guid? brandId, out Guid? technologyId) {
            brandId = Guid.Empty;
            technologyId = Guid.Empty;
            // можно брать первый, т.к. даже если ProductTree несколько, то они в одной технологии
            int objectId = context.Set<PromoProductTree>().First(n => n.PromoId == promo.Id && !n.Disabled).ProductTreeObjectId;
            var productTree = context.Set<ProductTree>().Where(x => x.ObjectId == objectId && !x.EndDate.HasValue).FirstOrDefault();
            while (productTree.Type != "root") {
                if (productTree.Type == "Brand") {
                    brandId = productTree.BrandId;
                } else if (productTree.Type == "Technology") {
                    technologyId = productTree.TechnologyId;
                }

                productTree = context.Set<ProductTree>().Where(x => x.ObjectId == productTree.parentId && !x.EndDate.HasValue).FirstOrDefault();
            }
        }
    }
}