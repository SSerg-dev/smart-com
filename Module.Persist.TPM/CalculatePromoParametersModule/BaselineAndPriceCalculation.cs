using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            List<PromoProduct> promoProducts = databaseContext.Set<PromoProduct>()
                .Include(f => f.PromoProductsCorrections)
                .Where(x => !x.Disabled && x.PromoId == promo.Id)
                .ToList();
            List<PriceList> allPriceLists = databaseContext.Set<PriceList>().Where(x => !x.Disabled && x.StartDate <= promo.DispatchesStart
                                                                    && x.EndDate >= promo.DispatchesStart
                                                                    && x.ClientTreeId == promo.ClientTreeKeyId).ToList();
            List<PriceList> priceListsForPromoAndPromoProducts = allPriceLists.Where(x => promoProducts.Any(y => y.ProductId == x.ProductId && x.FuturePriceMarker == false)).ToList();
            List<PriceList> priceListsForPromoAndPromoProductsFPM = allPriceLists.Where(x => promoProducts.Any(y => y.ProductId == x.ProductId && x.FuturePriceMarker == true)).ToList();

            foreach (PromoProduct promoProduct in promoProducts)
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
            // PriceIncreace
            bool IsOneProductWithFuturePriceMarker = false;
            if (promo.PromoPriceIncrease.PromoProductPriceIncreases is null && promoProducts.Count > 0)
            {
                PlanProductParametersCalculation.FillPriceIncreaseProdusts(promo, promoProducts);
            }
            if (promo.PromoPriceIncrease.PromoProductPriceIncreases != null)
            {
                foreach (PromoProductPriceIncrease promoProductPriceIncrease in promo.PromoPriceIncrease.PromoProductPriceIncreases)
                {
                    var priceListFPM = priceListsForPromoAndPromoProductsFPM.Where(x => x.ProductId == promoProductPriceIncrease.PromoProduct.ProductId)
                                                                      .OrderByDescending(x => x.StartDate).FirstOrDefault();
                    var priceList = priceListsForPromoAndPromoProducts.Where(x => x.ProductId == promoProductPriceIncrease.PromoProduct.ProductId)
                                                                 .OrderByDescending(x => x.StartDate).FirstOrDefault();

                    if (priceListFPM != null)
                    {
                        promoProductPriceIncrease.Price = priceListFPM.Price;
                        IsOneProductWithFuturePriceMarker = true;
                        promoProductPriceIncrease.FuturePriceMarker = true;
                        if (promo.IncrementalPromoes.FirstOrDefault(g => g.ProductId == promoProductPriceIncrease.PromoProduct.ProductId && !g.Disabled) != null)
                        {
                            promo.IncrementalPromoes.FirstOrDefault(g => g.ProductId == promoProductPriceIncrease.PromoProduct.ProductId && !g.Disabled).CasePrice = priceListFPM.Price;
                        }
                    }
                    else if (priceList != null)
                    {
                        promoProductPriceIncrease.Price = priceList.Price;
                        if (promo.IncrementalPromoes.FirstOrDefault(g => g.ProductId == promoProductPriceIncrease.PromoProduct.ProductId && !g.Disabled) != null)
                        {
                            promo.IncrementalPromoes.FirstOrDefault(g => g.ProductId == promoProductPriceIncrease.PromoProduct.ProductId && !g.Disabled).CasePrice = priceList.Price;
                        }
                    }
                    else
                    {
                        promoProductPriceIncrease.Price = null;
                        if (promo.IncrementalPromoes.FirstOrDefault(g => g.ProductId == promoProductPriceIncrease.PromoProduct.ProductId && !g.Disabled) != null)
                        {
                            promo.IncrementalPromoes.FirstOrDefault(g => g.ProductId == promoProductPriceIncrease.PromoProduct.ProductId && !g.Disabled).CasePrice = null;
                        }
                    }
                }
            }
            promo.IsPriceIncrease = IsOneProductWithFuturePriceMarker;
            databaseContext.SaveChanges();
        }

        /// <summary>
        /// Производит подбор подходящий записей из таблицы Baseline.
        /// </summary>
        public static string CalculateBaselineQtyAndLSV(Promo promo, DatabaseContext context, DateTimeOffset? promoStartDate, DateTimeOffset? promoEndDate, bool isOnInvoice)
        {
            string message = null;
            bool baseLineFound = false;
            bool baseLinePIFound = false;
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
                    int promoDuration = Math.Abs((promoEndDate.Value - promoStartDate.Value).Days) + 1;
                    int promoStartDateDayOfWeek = (int)promoStartDate.Value.DayOfWeek;
                    DateTimeOffset startPromoMarsWeek = promoStartDate.Value.AddDays(-promoStartDateDayOfWeek);
                    int promoEndDateDayOfWeek = (int)promoEndDate.Value.DayOfWeek;
                    DateTimeOffset endPromoMarsWeek = promoEndDate.Value.AddDays(-promoEndDateDayOfWeek);

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
                    List<BaseLine> baselines = new List<BaseLine>();
                    List<IncreaseBaseLine> increaseBaseLines = new List<IncreaseBaseLine>(); // PriceIncrease
                    if (productIds != null && clientNode != null && startPromoMarsWeek != null && endPromoMarsWeek != null)
                    {
                        baselines = context.Set<BaseLine>().Where(x => !x.Disabled && productIds.Contains(x.ProductId)
                                    && x.StartDate >= startPromoMarsWeek && x.StartDate <= endPromoMarsWeek
                                    && x.DemandCode == clientNode.DemandCode).ToList();
                        // PriceIncrease
                        increaseBaseLines = context.Set<IncreaseBaseLine>().Where(x => !x.Disabled && productIds.Contains(x.ProductId)
                                    && x.StartDate >= startPromoMarsWeek && x.StartDate <= endPromoMarsWeek
                                    && x.DemandCode == clientNode.DemandCode).ToList();
                    }

                    if (baseLineShareIndex == 0)
                    {
                        if (message == null)
                            message = "";

                        foreach (PromoProduct promoProduct in promoProducts)
                        {
                            promoProduct.PlanProductBaselineCaseQty = 0;
                            promoProduct.PlanProductBaselineLSV = 0;
                        }
                        // PriceIncrease
                        foreach (PromoProductPriceIncrease promoProductPriceIncrease in promo.PromoPriceIncrease.PromoProductPriceIncreases)
                        {
                            promoProductPriceIncrease.PlanProductBaselineCaseQty = 0;
                            promoProductPriceIncrease.PlanProductBaselineLSV = 0;
                        }
                        message += string.Format("\nClientShareIndex was 0");
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
                            List<ShortBaseline> result = (from mws in marsWeekStarts
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
                        if (promo.PromoPriceIncrease.PromoProductPriceIncreases != null)
                        {
                            foreach (PromoProductPriceIncrease promoProductPriceIncrease in promo.PromoPriceIncrease.PromoProductPriceIncreases)
                            {
                                baseLinePIFound = false;
                                var promoProductIncreaseBaselines = increaseBaseLines.Where(x => x.ProductId == promoProductPriceIncrease.PromoProduct.ProductId)
                                                                 .Select(x => new { x.StartDate, x.SellInBaselineQTY, x.SellOutBaselineQTY })
                                                                 .ToList();
                                List<ShortBaseline> result = new List<ShortBaseline>();
                                if (promoProductIncreaseBaselines.Count() != 0 && promoProductPriceIncrease.FuturePriceMarker)
                                {
                                    baseLinePIFound = true;
                                    // left join двух списков (именно left, для того, чтобы в отсутвие baseline записать на соответствующую неделю Qty = 0)
                                    // список №1 - все недели(в виде дат их начала), которые затрагивает промо
                                    // список №2 - найденные baseline для этого ZREP
                                    result = (from mws in marsWeekStarts
                                              join ppb in promoProductIncreaseBaselines on mws equals ppb.StartDate into joined
                                              from j in joined.DefaultIfEmpty()
                                              select new ShortBaseline
                                              {
                                                  StartDate = mws,
                                                  Qty = isOnInvoice ? j?.SellInBaselineQTY ?? 0 : j?.SellOutBaselineQTY ?? 0
                                              })
                                              .OrderBy(x => x.StartDate)
                                              .ToList();
                                }
                                else
                                {
                                    var promoProductBaselines = baselines.Where(x => x.ProductId == promoProductPriceIncrease.PromoProduct.ProductId)
                                                             .Select(x => new { x.StartDate, x.SellInBaselineQTY, x.SellOutBaselineQTY })
                                                             .ToList();
                                    result = (from mws in marsWeekStarts
                                              join ppb in promoProductBaselines on mws equals ppb.StartDate into joined
                                              from j in joined.DefaultIfEmpty()
                                              select new ShortBaseline
                                              {
                                                  StartDate = mws,
                                                  Qty = isOnInvoice ? j?.SellInBaselineQTY ?? 0 : j?.SellOutBaselineQTY ?? 0
                                              })
                                      .OrderBy(x => x.StartDate)
                                      .ToList();
                                }



                                // умножение количества на первой и последней неделе на коэффициент, пропорциональный количеству дней, которое промо занимаент на соответствующей неделе
                                result.First().Qty *= kFirstWeek;
                                if (kLastWeek != 0) result.Last().Qty *= kLastWeek;

                                // если не нашли BaseLine, пишем об этом
                                if (!baseLinePIFound)
                                {
                                    if (message == null)
                                        message = "";

                                    message += String.Format("\nIncreaseBaseline was not found for product with ZREP: {0}", promoProductPriceIncrease.ZREP);
                                }

                                double? planProductBaseLineCaseQty = result.Sum(x => x.Qty) * baseLineShareIndex;
                                promoProductPriceIncrease.PlanProductBaselineCaseQty = planProductBaseLineCaseQty;
                                promoProductPriceIncrease.PlanProductBaselineLSV = planProductBaseLineCaseQty * promoProductPriceIncrease.Price;
                                promoProductPriceIncrease.PlanProductBaselineVolume = promoProductPriceIncrease.PlanProductBaselineCaseQty * promoProductPriceIncrease.PromoProduct.Product.CaseVolume;
                            }
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
            Promo promo = context.Set<Promo>().Where(p => p.Id == ipromo.PromoId && !p.Disabled).Select(p => p).FirstOrDefault();

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
