using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Module.Persist.TPM.CalculatePromoParametersModule
{
    public class BaselineAndPriceCalculation
    {
        /// <summary>
        /// Заполняет поля Price сущности PromoProdct из таблицы PriceList.
        /// </summary>
        public static void SetPriceForPromoProducts(DatabaseContext databaseContext, Promo promo)
        {
            var promoProducts = databaseContext.Set<PromoProduct>().Where(x => !x.Disabled && x.PromoId == promo.Id).ToList();
            var priceLists = databaseContext.Set<PriceList>().Where(x => !x.Disabled && x.StartDate <= promo.DispatchesStart 
                                                                    && x.EndDate >= promo.DispatchesStart 
                                                                    && x.ClientTreeId == promo.ClientTreeKeyId).ToList();
            var priceListsForPromoAndPromoProducts = priceLists.Where(x => promoProducts.Any(y => y.ProductId == x.ProductId));

            foreach (var promoProduct in promoProducts)
            {
                var priceList = priceListsForPromoAndPromoProducts.Where(x => x.ProductId == promoProduct.ProductId)
                                                                  .OrderByDescending(x => x.StartDate).FirstOrDefault();
                var incrementalPromo = databaseContext.Set<IncrementalPromo>().Where(x => !x.Disabled && x.PromoId == promo.Id 
                                                                              && x.ProductId == promoProduct.ProductId).FirstOrDefault();
                if (priceList != null)
                {
                    promoProduct.Price = priceList.Price;
                    if (incrementalPromo != null) incrementalPromo.CasePrice = priceList.Price;
                }
                else
                {
                    promoProduct.Price = null;
                    if (incrementalPromo != null) incrementalPromo.CasePrice = null;
                }
            }

            databaseContext.SaveChanges();
        }

        /// <summary>
        /// Производит подбор подходящий записей из таблицы Baseline.
        /// </summary>
        public static string CalculateBaselineQtyAndLSV(Promo promo, DatabaseContext context, DateTimeOffset? promoStartDate, DateTimeOffset? promoEndDate, bool isOnInvoice)
        {
            string message = null;
            bool baseLineFound = false;
            List<Tuple<string, double?, double?>> resultValues = new List<Tuple<string, double?, double?>>();

            if (promoStartDate.HasValue && promoEndDate.HasValue)
            {
                if (!promo.InOut.HasValue || !promo.InOut.Value)
                {
                    ClientTree clientNode = context.Set<ClientTree>().Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue).FirstOrDefault();
                    double baseLineShareIndex = 1;
                    double kFirstWeek, kLastWeek;
                    ClientTreeBrandTech clientTreeBrandTech = null;
                    List<ClientTreeBrandTech> disabledClientTreeBrandTechList = new List<ClientTreeBrandTech>();
                    List<DateTimeOffset?> marsWeekStarts = new List<DateTimeOffset?>();

                    //длительность промо (проведение или отгрузка)
                    var promoDuration = Math.Abs((promoEndDate.Value - promoStartDate.Value).Days) + 1;
                    var promoStartDateDayOfWeek = (int)promoStartDate.Value.DayOfWeek;
                    var startPromoMarsWeek = promoStartDate.Value.AddDays(-promoStartDateDayOfWeek);
                    var promoEndDateDayOfWeek = (int)promoEndDate.Value.DayOfWeek;
                    var endPromoMarsWeek = promoEndDate.Value.AddDays(-promoEndDateDayOfWeek);

                    //количество недель, которое затрагивает Promo
                    int d = Math.Abs((endPromoMarsWeek - startPromoMarsWeek).Days) + 1;
                    int weekDuration = (int)Math.Ceiling(d / 7.0);

                    if (weekDuration == 1)
                    {
                        kFirstWeek = promoDuration / 7.0;
                        kLastWeek = 0;
                    }
                    else
                    {
                        kFirstWeek = (7 - promoStartDateDayOfWeek) / 7.0;
                        kLastWeek = (promoEndDateDayOfWeek + 1) / 7.0;
                    }

                    //получаем список всех недель(в виде дат их начала), которые затрагивает промо
                    for (int i = 0; i < weekDuration; i++)
                    {
                        marsWeekStarts.Add(startPromoMarsWeek.AddDays(i * 7));
                    }

                    //поиск узла в дереве с Demandcode(для подбора Baseline) и определение доли
                    while (clientNode != null && clientNode.Type != "root" && String.IsNullOrWhiteSpace(clientNode.DemandCode))
                    {
                        clientTreeBrandTech = context.Set<ClientTreeBrandTech>().Where(x => x.ClientTreeId == clientNode.Id && x.BrandTechId == promo.BrandTechId && !x.Disabled).FirstOrDefault();
                        if (clientTreeBrandTech == null)
                        {
                            disabledClientTreeBrandTechList = context.Set<ClientTreeBrandTech>().Where(x => x.ClientTreeId == clientNode.Id && x.BrandTechId == promo.BrandTechId && x.Disabled)
                                                                     .OrderByDescending(x => x.DeletedDate)
                                                                     .ToList();

                            if (disabledClientTreeBrandTechList.Count > 0)
                            {
                                baseLineShareIndex *= disabledClientTreeBrandTechList[0].Share / 100;
                            }
                        }
                        else
                        {
                            baseLineShareIndex *= clientTreeBrandTech.Share / 100;
                        }
                        clientNode = context.Set<ClientTree>().Where(x => x.ObjectId == clientNode.parentId && !x.EndDate.HasValue).FirstOrDefault();
                    }

                    List<PromoProduct> promoProducts = context.Set<PromoProduct>().Where(x => x.PromoId == promo.Id && !x.Disabled).ToList();
                    List<Guid> productIds = promoProducts.Select(x => x.ProductId).ToList();
                    var baselines = new List<BaseLine>();
                    if (productIds != null && clientNode != null && startPromoMarsWeek != null && endPromoMarsWeek != null)
                    {
                        baselines = context.Set<BaseLine>().Where(x => !x.Disabled && productIds.Contains(x.ProductId)
                                    && x.StartDate >= startPromoMarsWeek && x.StartDate <= endPromoMarsWeek
                                    && x.DemandCode == clientNode.DemandCode).ToList();
                    }

                    if (baseLineShareIndex == 0)
                    {
                        if (message == null)
                            message = "";

                        foreach (var promoProduct in promoProducts)
                        {
                            promoProduct.PlanProductBaselineCaseQty = 0;
                            promoProduct.PlanProductBaselineLSV = 0;
                        }
                        message += String.Format("\nClientShareIndex was 0");
                    }
                    else
                    {
                        foreach (var promoProduct in promoProducts)
                        {
                            baseLineFound = false;
                            var promoProductBaselines = baselines.Where(x => x.ProductId == promoProduct.ProductId)
                                                             .Select(x => new { x.StartDate, x.SellInBaselineQTY, x.SellOutBaselineQTY })
                                                             .ToList();

                            if (promoProductBaselines.Count() != 0) baseLineFound = true;


                            // left join двух списков (именно left, для того, чтобы в отсутвие baseline записать на соответствующую неделю Qty = 0)
                            // список №1 - все недели(в виде дат их начала), которые затрагивает промо
                            // список №2 - найденные baseline для этого ZREP
                            var result = (from mws in marsWeekStarts
                                          join ppb in promoProductBaselines on mws equals ppb.StartDate into joined
                                          from j in joined.DefaultIfEmpty()
                                          select new ShortBaseline
                                          {
                                              StartDate = mws,
                                              Qty = isOnInvoice ? j?.SellInBaselineQTY ?? 0 : j?.SellOutBaselineQTY ?? 0
                                          })
                                      .OrderBy(x => x.StartDate)
                                      .ToList();

                            // умножение количества на первой и последней неделе на коэффициент, пропорциональный количеству дней, которое промо занимаент на соответствующей неделе
                            result.First().Qty *= kFirstWeek;
                            if (kLastWeek != 0) result.Last().Qty *= kLastWeek;

                            // если не нашли BaseLine, пишем об этом
                            if (!baseLineFound)
                            {
                                if (message == null)
                                    message = "";

                                message += String.Format("\nBaseline was not found for product with ZREP: {0}", promoProduct.ZREP);
                            }

                            double? planProductBaseLineCaseQty = result.Sum(x => x.Qty) * baseLineShareIndex;
                            promoProduct.PlanProductBaselineCaseQty = planProductBaseLineCaseQty;
                            promoProduct.PlanProductBaselineLSV = planProductBaseLineCaseQty * promoProduct.Price;
                            promoProduct.PlanProductBaselineVolume = promoProduct.PlanProductBaselineCaseQty * promoProduct.Product.CaseVolume;
                        }
                    }
                }
            }

            context.SaveChanges();
            return message;
        }

        public static double? CalculateCasePrice(IncrementalPromo ipromo, DatabaseContext context)
        {
            double? casePrice = null;
            var promo = context.Set<Promo>().Where(p => p.Id == ipromo.PromoId && !p.Disabled).Select(p => p).FirstOrDefault();

            if (promo != null)
            {
                casePrice = context.Set<PriceList>().Where(x => !x.Disabled && x.StartDate <= promo.DispatchesStart
                                                                        && x.EndDate >= promo.DispatchesStart
                                                                        && x.ClientTreeId == promo.ClientTreeKeyId
                                                                        && x.ProductId == ipromo.ProductId)
                                                                .OrderByDescending(x => x.StartDate)
                                                                .Select(x => x.Price)
                                                                .FirstOrDefault();
            }

            return casePrice;
        }
    }

    class ShortBaseline
    {
        public DateTimeOffset? StartDate { get; set; }
        public double? Qty { get; set; }
    }
}
