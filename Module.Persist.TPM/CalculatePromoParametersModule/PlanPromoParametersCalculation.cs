using System;
using System.Linq;
using Module.Persist.TPM.Model.TPM;
using Persist;
using Module.Persist.TPM.Utils;
using Module.Persist.TPM.Model.SimpleModel;
using System.Data.Entity;
using System.Collections.Generic;

namespace Module.Persist.TPM.CalculatePromoParametersModule
{
    public class PlanPromoParametersCalculation
    {
        /// <summary>
        /// Метод для расчета плановых параметров Promo.
        /// </summary>
        /// <param name="promoId">Id создаваемого/редактируемого промо</param>
        /// <param name="context">Текущий контекст</param>
        public static string CalculatePromoParameters(Guid promoId, DatabaseContext context)
        {
            try
            {
                List<ClientTree> clientTrees = context.Set<ClientTree>().Where(g => g.EndDate == null).ToList();
                List<BrandTech> brandTeches = context.Set<BrandTech>().Where(g => !g.Disabled).ToList();

                Promo promo = context.Set<Promo>()
                    .Include(g => g.PromoPriceIncrease.PromoProductPriceIncreases)
                    .Include(g => g.PromoProducts)
                    .Where(x => x.Id == promoId && !x.Disabled)
                    .FirstOrDefault();

                ResetValues(promo, context);
                List<PromoProduct> promoProducts = promo.PromoProducts
                    .Where(x => !x.Disabled)
                    .ToList();
                //double? sumPlanProductBaseLineLSV = promoProducts.Sum(x => x.PlanProductBaselineLSV);
                double sumPlanProductPostPromoEffectVolumeW1 = promoProducts.Sum(x => (double)x.PlanProductPostPromoEffectVolumeW1);
                double sumPlanProductPostPromoEffectVolumeW2 = promoProducts.Sum(x => (double)x.PlanProductPostPromoEffectVolumeW2);
                double sumPlanProductPostPromoEffectLSVW1 = promoProducts.Sum(x => (double)x.PlanProductPostPromoEffectLSVW1);
                double sumPlanProductPostPromoEffectLSVW2 = promoProducts.Sum(x => (double)x.PlanProductPostPromoEffectLSVW2);

                List<PromoProductPriceIncrease> promoProductPIs = promo.PromoPriceIncrease.PromoProductPriceIncreases
                    .Where(x => !x.Disabled)
                    .ToList();
                double sumPlanProductPostPromoEffectVolumeW1PI = promoProductPIs.Sum(x => (double)x.PlanProductPostPromoEffectVolumeW1);
                double sumPlanProductPostPromoEffectVolumeW2PI = promoProductPIs.Sum(x => (double)x.PlanProductPostPromoEffectVolumeW2);
                double sumPlanProductPostPromoEffectLSVW1PI = promoProductPIs.Sum(x => (double)x.PlanProductPostPromoEffectLSVW1);
                double sumPlanProductPostPromoEffectLSVW2PI = promoProductPIs.Sum(x => (double)x.PlanProductPostPromoEffectLSVW2);
                ClientTree clientTree = context.Set<ClientTree>().Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue).FirstOrDefault();

                //promo.PlanPromoBaselineLSV = sumPlanProductBaseLineLSV;
                //promo.PlanPromoIncrementalLSV = sumPlanProductBaseLineLSV * promo.PlanPromoUpliftPercent / 100;
                //promo.PlanPromoLSV = promo.PlanPromoBaselineLSV + promo.PlanPromoIncrementalLSV;  
                promo.PlanPromoTIShopper = promo.PlanPromoLSV * (promo.MarsMechanicDiscount / 100) * (clientTree?.DistrMarkUp ?? 1);
                // бюджеты пересчитывать не требуется (пусть пока будет закомментировано)
                //promo.PlanPromoTIMarketing = promo.PlanPromoXSites + promo.PlanPromoCatalogue + promo.PlanPromoPOSMInClient;
                //promo.PlanPromoCostProduction = promo.PlanPromoCostProdXSites + promo.PlanPromoCostProdCatalogue + promo.PlanPromoCostProdPOSMInClient;
                promo.PlanPromoCost = (promo.PlanPromoTIShopper ?? 0) + (promo.PlanPromoTIMarketing ?? 0) + (promo.PlanPromoBranding ?? 0) + (promo.PlanPromoBTL ?? 0) + (promo.PlanPromoCostProduction ?? 0);
                // PriceIncrease
                if (promo.PromoPriceIncrease != null)
                {
                    promo.PromoPriceIncrease.PlanPromoTIShopper = promo.PromoPriceIncrease.PlanPromoLSV * (promo.MarsMechanicDiscount / 100) * (clientTree?.DistrMarkUp ?? 1);
                    promo.PromoPriceIncrease.PlanPromoCost = (promo.PromoPriceIncrease.PlanPromoTIShopper ?? 0) + (promo.PlanPromoTIMarketing ?? 0) + (promo.PlanPromoBranding ?? 0) + (promo.PlanPromoBTL ?? 0) + (promo.PlanPromoCostProduction ?? 0);
                }
                string message = null;
                bool error;

                List<TradeInvestment> TIQuery = context.Set<TradeInvestment>().Where(x => !x.Disabled).ToList();
                SimplePromoTradeInvestment simplePromoTradeInvestment = new SimplePromoTradeInvestment(promo);
                double? TIBasePercent = PromoUtils.GetTIBasePercent(simplePromoTradeInvestment, new List<BaseTradeInvestment>(TIQuery), clientTrees, brandTeches, out message, out error);
                promo.PlanTIBasePercent = TIBasePercent;
                if (message == null)
                {
                    promo.PlanPromoIncrementalBaseTI = promo.PlanPromoIncrementalLSV * TIBasePercent / 100;
                    // PriceIncrease
                    if (promo.PromoPriceIncrease != null)
                    {
                        promo.PromoPriceIncrease.PlanPromoIncrementalBaseTI = promo.PromoPriceIncrease.PlanPromoIncrementalLSV * TIBasePercent / 100;
                    }
                    List<COGS> cogsQuery = context.Set<COGS>().Where(x => !x.Disabled).ToList();
                    SimplePromoCOGS simplePromoCOGS = new SimplePromoCOGS(promo);
                    double? COGSPercent = PromoUtils.GetCOGSPercent(simplePromoCOGS, new List<BaseCOGS>(cogsQuery), clientTrees, brandTeches, out message);
                    List<PlanCOGSTn> cogsTnQuery = context.Set<PlanCOGSTn>().Where(x => !x.Disabled).ToList();
                    double? COGSTnTonCost = PromoUtils.GetCOGSTonCost(simplePromoCOGS, new List<BaseCOGSTn>(cogsTnQuery), clientTrees, brandTeches, out message);
                    promo.PlanCOGSPercent = COGSPercent;
                    promo.PlanCOGSTn = COGSTnTonCost;
                    if (message == null)
                    {
                        promo.PlanPromoIncrementalCOGS = promo.PlanPromoIncrementalLSV * COGSPercent / 100;

                        promo.PlanPromoBaseTI = promo.PlanPromoLSV * TIBasePercent / 100;
                        // PriceIncrease
                        if (promo.PromoPriceIncrease != null)
                        {
                            promo.PromoPriceIncrease.PlanPromoIncrementalCOGS = promo.PromoPriceIncrease.PlanPromoIncrementalLSV * COGSPercent / 100;

                            promo.PromoPriceIncrease.PlanPromoBaseTI = promo.PromoPriceIncrease.PlanPromoLSV * TIBasePercent / 100;
                        }
                        // если стоит флаг inout, PlanPromoPostPromoEffect = 0
                        if (!promo.InOut.HasValue || !promo.InOut.Value)
                        {
                            promo.PlanPromoTotalCost = (promo.PlanPromoCost ?? 0) + (promo.PlanPromoBaseTI ?? 0);
                            // PriceIncrease
                            if (promo.PromoPriceIncrease != null)
                            {
                                promo.PromoPriceIncrease.PlanPromoTotalCost = (promo.PromoPriceIncrease.PlanPromoCost ?? 0) + (promo.PlanPromoBaseTI ?? 0);
                            }
                            //TODO: Уточнить насчет деления на 100
                            promo.PlanPromoPostPromoEffectLSVW1 = sumPlanProductPostPromoEffectLSVW1;
                            promo.PlanPromoPostPromoEffectLSVW2 = sumPlanProductPostPromoEffectLSVW2;
                            promo.PlanPromoPostPromoEffectLSV = promo.PlanPromoPostPromoEffectLSVW1 + promo.PlanPromoPostPromoEffectLSVW2;

                            promo.PlanPromoPostPromoEffectVolumeW1 = sumPlanProductPostPromoEffectVolumeW1;
                            promo.PlanPromoPostPromoEffectVolumeW2 = sumPlanProductPostPromoEffectVolumeW2;
                            promo.PlanPromoPostPromoEffectVolume = promo.PlanPromoPostPromoEffectVolumeW1 + promo.PlanPromoPostPromoEffectVolumeW2;

                            // PriceIncrease
                            if (promo.PromoPriceIncrease != null)
                            {
                                promo.PromoPriceIncrease.PlanPromoPostPromoEffectLSVW1 = sumPlanProductPostPromoEffectLSVW1PI;
                                promo.PromoPriceIncrease.PlanPromoPostPromoEffectLSVW2 = sumPlanProductPostPromoEffectLSVW2PI;
                                promo.PromoPriceIncrease.PlanPromoPostPromoEffectLSV = promo.PromoPriceIncrease.PlanPromoPostPromoEffectLSVW1 + promo.PromoPriceIncrease.PlanPromoPostPromoEffectLSVW2;

                                promo.PromoPriceIncrease.PlanPromoPostPromoEffectVolumeW1 = sumPlanProductPostPromoEffectVolumeW1PI;
                                promo.PromoPriceIncrease.PlanPromoPostPromoEffectVolumeW2 = sumPlanProductPostPromoEffectVolumeW2PI;
                                promo.PromoPriceIncrease.PlanPromoPostPromoEffectVolume = promo.PromoPriceIncrease.PlanPromoPostPromoEffectVolumeW1 + promo.PromoPriceIncrease.PlanPromoPostPromoEffectVolumeW2;
                            }
                        }

                        promo.PlanPromoNetIncrementalLSV = (promo.PlanPromoIncrementalLSV ?? 0) + (promo.PlanPromoPostPromoEffectLSV ?? 0);
                        // PriceIncrease
                        if (promo.PromoPriceIncrease != null)
                        {
                            promo.PromoPriceIncrease.PlanPromoNetIncrementalLSV = (promo.PromoPriceIncrease.PlanPromoIncrementalLSV ?? 0) + (promo.PromoPriceIncrease.PlanPromoPostPromoEffectLSV ?? 0);
                        }
                    }
                    else
                    {
                        promo.PlanPromoTotalCost = (promo.PlanPromoCost ?? 0) + (promo.PlanPromoBaseTI ?? 0); // (promo.PlanPromoCost ?? 0) + (promo.PlanPromoIncrementalBaseTI ?? 0) + (promo.PlanPromoIncrementalCOGS ?? 0);

                        promo.PlanPromoPostPromoEffectLSVW1 = 0;
                        promo.PlanPromoPostPromoEffectLSVW2 = 0;
                        promo.PlanPromoPostPromoEffectLSV = 0;

                        promo.PlanPromoNetIncrementalLSV = (promo.PlanPromoIncrementalLSV ?? 0) + (promo.PlanPromoPostPromoEffectLSV ?? 0);

                        promo.PlanPromoPostPromoEffectVolumeW1 = 0;
                        promo.PlanPromoPostPromoEffectVolumeW2 = 0;
                        promo.PlanPromoPostPromoEffectVolume = 0;
                        // PriceIncrease
                        if (promo.PromoPriceIncrease != null)
                        {
                            promo.PromoPriceIncrease.PlanPromoTotalCost = (promo.PromoPriceIncrease.PlanPromoCost ?? 0) + (promo.PlanPromoBaseTI ?? 0);

                            promo.PromoPriceIncrease.PlanPromoPostPromoEffectLSVW1 = 0;
                            promo.PromoPriceIncrease.PlanPromoPostPromoEffectLSVW2 = 0;
                            promo.PromoPriceIncrease.PlanPromoPostPromoEffectLSV = 0;

                            promo.PromoPriceIncrease.PlanPromoNetIncrementalLSV = (promo.PromoPriceIncrease.PlanPromoIncrementalLSV ?? 0) + (promo.PromoPriceIncrease.PlanPromoPostPromoEffectLSV ?? 0);

                            promo.PromoPriceIncrease.PlanPromoPostPromoEffectVolumeW1 = 0;
                            promo.PromoPriceIncrease.PlanPromoPostPromoEffectVolumeW2 = 0;
                            promo.PromoPriceIncrease.PlanPromoPostPromoEffectVolume = 0;
                        }
                    }

                    promo.PlanPromoNetLSV = (promo.PlanPromoBaselineLSV ?? 0) + (promo.PlanPromoNetIncrementalLSV ?? 0);
                    promo.PlanPromoNetIncrementalBaseTI = promo.PlanPromoNetIncrementalLSV * TIBasePercent / 100;
                    promo.PlanPromoNetIncrementalCOGS = promo.PlanPromoNetIncrementalLSV * COGSPercent / 100;
                    // PriceIncrease
                    if (promo.PromoPriceIncrease != null)
                    {
                        promo.PromoPriceIncrease.PlanPromoNetLSV = (promo.PromoPriceIncrease.PlanPromoBaselineLSV ?? 0) + (promo.PromoPriceIncrease.PlanPromoNetIncrementalLSV ?? 0);
                        promo.PromoPriceIncrease.PlanPromoNetIncrementalBaseTI = promo.PromoPriceIncrease.PlanPromoNetIncrementalLSV * TIBasePercent / 100;
                        promo.PromoPriceIncrease.PlanPromoNetIncrementalCOGS = promo.PromoPriceIncrease.PlanPromoNetIncrementalLSV * COGSPercent / 100;
                    }
                    if (!promo.InOut.HasValue || !promo.InOut.Value)
                    {
                        promo.PlanPromoNetBaseTI = promo.PlanPromoNetLSV * TIBasePercent / 100;
                        promo.PlanPromoBaselineBaseTI = promo.PlanPromoBaselineLSV * TIBasePercent / 100;
                        promo.PlanPromoNSV = (promo.PlanPromoLSV ?? 0) - (promo.PlanPromoTIShopper ?? 0) - (promo.PlanPromoTIMarketing ?? 0) - (promo.PlanPromoBaseTI ?? 0);
                        promo.PlanPromoIncrementalNSV = (promo.PlanPromoIncrementalLSV ?? 0) - (promo.PlanPromoTIShopper ?? 0) - (promo.PlanPromoTIMarketing ?? 0) - (promo.PlanPromoIncrementalBaseTI ?? 0);
                        promo.PlanPromoNetIncrementalNSV = (promo.PlanPromoNetIncrementalLSV ?? 0) - (promo.PlanPromoTIShopper ?? 0) - (promo.PlanPromoTIMarketing ?? 0) - (promo.PlanPromoNetIncrementalBaseTI ?? 0);
                        promo.PlanPromoNetIncrementalMAC = (promo.PlanPromoNetIncrementalNSV ?? 0) - (promo.PlanPromoNetIncrementalCOGS ?? 0);

                        double? sumPlanProductBaseLineVolume = context.Set<PromoProduct>().Where(x => x.PromoId == promoId && !x.Disabled).Sum(x => x.PlanProductBaselineVolume);
                        promo.PlanPromoBaselineVolume = sumPlanProductBaseLineVolume;
                        promo.PlanPromoIncrementalVolume = sumPlanProductBaseLineVolume * promo.PlanPromoUpliftPercent / 100;
                        promo.PlanPromoNetIncrementalVolume = (promo.PlanPromoIncrementalVolume ?? 0) + (promo.PlanPromoPostPromoEffectVolume ?? 0);
                        // PriceIncrease
                        if (promo.PromoPriceIncrease != null)
                        {
                            promo.PromoPriceIncrease.PlanPromoNetBaseTI = promo.PromoPriceIncrease.PlanPromoNetLSV * TIBasePercent / 100;
                            promo.PromoPriceIncrease.PlanPromoBaselineBaseTI = promo.PromoPriceIncrease.PlanPromoBaselineLSV * TIBasePercent / 100;
                            promo.PromoPriceIncrease.PlanPromoNSV = (promo.PromoPriceIncrease.PlanPromoLSV ?? 0) - (promo.PromoPriceIncrease.PlanPromoTIShopper ?? 0) - (promo.PlanPromoTIMarketing ?? 0) - (promo.PromoPriceIncrease.PlanPromoBaseTI ?? 0);
                            promo.PromoPriceIncrease.PlanPromoIncrementalNSV = (promo.PromoPriceIncrease.PlanPromoIncrementalLSV ?? 0) - (promo.PromoPriceIncrease.PlanPromoTIShopper ?? 0) - (promo.PlanPromoTIMarketing ?? 0) - (promo.PromoPriceIncrease.PlanPromoIncrementalBaseTI ?? 0);
                            promo.PromoPriceIncrease.PlanPromoNetIncrementalNSV = (promo.PromoPriceIncrease.PlanPromoNetIncrementalLSV ?? 0) - (promo.PromoPriceIncrease.PlanPromoTIShopper ?? 0) - (promo.PlanPromoTIMarketing ?? 0) - (promo.PromoPriceIncrease.PlanPromoNetIncrementalBaseTI ?? 0);
                            promo.PromoPriceIncrease.PlanPromoNetIncrementalMAC = (promo.PromoPriceIncrease.PlanPromoNetIncrementalNSV ?? 0) - (promo.PromoPriceIncrease.PlanPromoNetIncrementalCOGS ?? 0);

                            double? sumPlanProductBaseLineVolumePI = context.Set<PromoProductPriceIncrease>().Where(x => x.PromoPriceIncreaseId == promoId && !x.Disabled).Sum(x => x.PlanProductBaselineVolume);
                            promo.PromoPriceIncrease.PlanPromoBaselineVolume = sumPlanProductBaseLineVolumePI;
                            promo.PromoPriceIncrease.PlanPromoIncrementalVolume = sumPlanProductBaseLineVolumePI * promo.PromoPriceIncrease.PlanPromoUpliftPercent / 100;
                            promo.PromoPriceIncrease.PlanPromoNetIncrementalVolume = (promo.PromoPriceIncrease.PlanPromoIncrementalVolume ?? 0) + (promo.PromoPriceIncrease.PlanPromoPostPromoEffectVolume ?? 0);
                        }
                    }
                    else
                    {
                        promo.PlanPromoNetBaseTI = 0;
                        promo.PlanPromoBaselineBaseTI = 0;
                        promo.PlanPromoNSV = (promo.PlanPromoLSV ?? 0) - (promo.PlanPromoTIShopper ?? 0) - (promo.PlanPromoTIMarketing ?? 0) - (promo.PlanPromoBaseTI ?? 0);
                        promo.PlanPromoIncrementalNSV = (promo.PlanPromoIncrementalLSV ?? 0) - (promo.PlanPromoTIShopper ?? 0) - (promo.PlanPromoTIMarketing ?? 0) - (promo.PlanPromoIncrementalBaseTI ?? 0);
                        promo.PlanPromoNetIncrementalNSV = (promo.PlanPromoNetIncrementalLSV ?? 0) - (promo.PlanPromoTIShopper ?? 0) - (promo.PlanPromoTIMarketing ?? 0) - (promo.PlanPromoNetIncrementalBaseTI ?? 0);
                        double? sumPlanPromoIncrementalCase = context.Set<PromoProduct>().Where(x => x.PromoId == promoId && !x.Disabled).Sum(x => x.PlanProductIncrementalCaseQty * x.Product.CaseVolume);
                        promo.PlanPromoIncrementalVolume = sumPlanPromoIncrementalCase;
                        promo.PlanPromoNetIncrementalVolume = sumPlanPromoIncrementalCase;
                        // PriceIncrease
                        if (promo.PromoPriceIncrease != null)
                        {
                            promo.PromoPriceIncrease.PlanPromoNetBaseTI = 0;
                            promo.PromoPriceIncrease.PlanPromoBaselineBaseTI = 0;
                            promo.PromoPriceIncrease.PlanPromoNSV = (promo.PromoPriceIncrease.PlanPromoLSV ?? 0) - (promo.PromoPriceIncrease.PlanPromoTIShopper ?? 0) - (promo.PlanPromoTIMarketing ?? 0) - (promo.PromoPriceIncrease.PlanPromoBaseTI ?? 0);
                            promo.PromoPriceIncrease.PlanPromoIncrementalNSV = (promo.PromoPriceIncrease.PlanPromoIncrementalLSV ?? 0) - (promo.PromoPriceIncrease.PlanPromoTIShopper ?? 0) - (promo.PlanPromoTIMarketing ?? 0) - (promo.PromoPriceIncrease.PlanPromoIncrementalBaseTI ?? 0);
                            promo.PromoPriceIncrease.PlanPromoNetIncrementalNSV = (promo.PromoPriceIncrease.PlanPromoNetIncrementalLSV ?? 0) - (promo.PromoPriceIncrease.PlanPromoTIShopper ?? 0) - (promo.PlanPromoTIMarketing ?? 0) - (promo.PromoPriceIncrease.PlanPromoNetIncrementalBaseTI ?? 0);
                            double? sumPlanPromoIncrementalCasePI = context.Set<PromoProductPriceIncrease>().Include(g => g.PromoProduct).Where(x => x.PromoPriceIncreaseId == promoId && !x.Disabled).Sum(x => x.PlanProductIncrementalCaseQty * x.PromoProduct.Product.CaseVolume);
                            promo.PromoPriceIncrease.PlanPromoIncrementalVolume = sumPlanPromoIncrementalCasePI;
                            promo.PromoPriceIncrease.PlanPromoNetIncrementalVolume = sumPlanPromoIncrementalCasePI;
                        }
                    }

                    promo.PlanPromoNetNSV = (promo.PlanPromoNetLSV ?? 0) - (promo.PlanPromoTIShopper ?? 0) - (promo.PlanPromoTIMarketing ?? 0) - (promo.PlanPromoNetBaseTI ?? 0);
                    promo.PlanPromoIncrementalCOGSTn = promo.PlanPromoIncrementalVolume * COGSTnTonCost;
                    promo.PlanPromoNetIncrementalCOGSTn = promo.PlanPromoNetIncrementalVolume * COGSTnTonCost;
                    promo.PlanPromoVolume = promo.PlanPromoBaselineVolume + promo.PlanPromoIncrementalVolume;
                    promo.PlanPromoNSVtn = promo.PlanPromoNSV / promo.PlanPromoVolume;
                    // PriceIncrease
                    if (promo.PromoPriceIncrease != null)
                    {
                        promo.PromoPriceIncrease.PlanPromoVolume = promo.PromoPriceIncrease.PlanPromoBaselineVolume + promo.PromoPriceIncrease.PlanPromoIncrementalVolume;
                        promo.PromoPriceIncrease.PlanPromoNSVtn = promo.PromoPriceIncrease.PlanPromoNSV / promo.PromoPriceIncrease.PlanPromoVolume;
                        promo.PromoPriceIncrease.PlanPromoNetNSV = (promo.PromoPriceIncrease.PlanPromoNetLSV ?? 0) - (promo.PromoPriceIncrease.PlanPromoTIShopper ?? 0) - (promo.PlanPromoTIMarketing ?? 0) - (promo.PromoPriceIncrease.PlanPromoNetBaseTI ?? 0);
                        promo.PromoPriceIncrease.PlanPromoIncrementalCOGSTn = promo.PromoPriceIncrease.PlanPromoIncrementalVolume * COGSTnTonCost;
                        promo.PromoPriceIncrease.PlanPromoNetIncrementalCOGSTn = promo.PromoPriceIncrease.PlanPromoNetIncrementalVolume * COGSTnTonCost;
                    }
                    if (promo.IsLSVBased)
                    {
                        promo.PlanPromoNetIncrementalMAC = (promo.PlanPromoNetIncrementalNSV ?? 0) - (promo.PlanPromoNetIncrementalCOGS ?? 0);
                        promo.PlanPromoIncrementalMAC = (promo.PlanPromoIncrementalNSV ?? 0) - (promo.PlanPromoIncrementalCOGS ?? 0);
                        promo.PlanPromoIncrementalEarnings = (promo.PlanPromoIncrementalMAC ?? 0) - (promo.PlanPromoBranding ?? 0) - (promo.PlanPromoBTL ?? 0) - (promo.PlanPromoCostProduction ?? 0);
                        promo.PlanPromoNetIncrementalEarnings = (promo.PlanPromoNetIncrementalMAC ?? 0) - (promo.PlanPromoBranding ?? 0) - (promo.PlanPromoBTL ?? 0) - (promo.PlanPromoCostProduction ?? 0);
                        promo.PlanPromoROIPercent = promo.PlanPromoCost != 0 ? (promo.PlanPromoIncrementalEarnings / promo.PlanPromoCost + 1) * 100 : 0;
                        promo.PlanPromoNetROIPercent = promo.PlanPromoCost != 0 ? (promo.PlanPromoNetIncrementalEarnings / promo.PlanPromoCost + 1) * 100 : 0;
                        // PriceIncrease
                        if (promo.PromoPriceIncrease != null)
                        {
                            promo.PromoPriceIncrease.PlanPromoNetIncrementalMAC = (promo.PromoPriceIncrease.PlanPromoNetIncrementalNSV ?? 0) - (promo.PromoPriceIncrease.PlanPromoNetIncrementalCOGS ?? 0);
                            promo.PromoPriceIncrease.PlanPromoIncrementalMAC = (promo.PromoPriceIncrease.PlanPromoIncrementalNSV ?? 0) - (promo.PromoPriceIncrease.PlanPromoIncrementalCOGS ?? 0);
                            promo.PromoPriceIncrease.PlanPromoIncrementalEarnings = (promo.PromoPriceIncrease.PlanPromoIncrementalMAC ?? 0) - (promo.PlanPromoBranding ?? 0) - (promo.PlanPromoBTL ?? 0) - (promo.PlanPromoCostProduction ?? 0);
                            promo.PromoPriceIncrease.PlanPromoNetIncrementalEarnings = (promo.PromoPriceIncrease.PlanPromoNetIncrementalMAC ?? 0) - (promo.PlanPromoBranding ?? 0) - (promo.PlanPromoBTL ?? 0) - (promo.PlanPromoCostProduction ?? 0);
                            promo.PromoPriceIncrease.PlanPromoROIPercent = promo.PromoPriceIncrease.PlanPromoCost != 0 ? (promo.PromoPriceIncrease.PlanPromoIncrementalEarnings / promo.PromoPriceIncrease.PlanPromoCost + 1) * 100 : 0;
                            promo.PromoPriceIncrease.PlanPromoNetROIPercent = promo.PromoPriceIncrease.PlanPromoCost != 0 ? (promo.PromoPriceIncrease.PlanPromoNetIncrementalEarnings / promo.PromoPriceIncrease.PlanPromoCost + 1) * 100 : 0;
                        }
                    }
                    else
                    {
                        promo.PlanPromoNetIncrementalMACLSV = (promo.PlanPromoNetIncrementalNSV ?? 0) - (promo.PlanPromoNetIncrementalCOGS ?? 0);
                        promo.PlanPromoNetIncrementalMAC = (promo.PlanPromoNetIncrementalNSV ?? 0) - (promo.PlanPromoNetIncrementalCOGSTn ?? 0);
                        promo.PlanPromoIncrementalMACLSV = (promo.PlanPromoIncrementalNSV ?? 0) - (promo.PlanPromoIncrementalCOGS ?? 0);
                        promo.PlanPromoIncrementalMAC = (promo.PlanPromoIncrementalNSV ?? 0) - (promo.PlanPromoIncrementalCOGSTn ?? 0);
                        promo.PlanPromoIncrementalEarningsLSV = (promo.PlanPromoIncrementalMACLSV ?? 0) - (promo.PlanPromoBranding ?? 0) - (promo.PlanPromoBTL ?? 0) - (promo.PlanPromoCostProduction ?? 0);
                        promo.PlanPromoIncrementalEarnings = (promo.PlanPromoIncrementalMAC ?? 0) - (promo.PlanPromoBranding ?? 0) - (promo.PlanPromoBTL ?? 0) - (promo.PlanPromoCostProduction ?? 0);
                        promo.PlanPromoNetIncrementalEarningsLSV = (promo.PlanPromoNetIncrementalMACLSV ?? 0) - (promo.PlanPromoBranding ?? 0) - (promo.PlanPromoBTL ?? 0) - (promo.PlanPromoCostProduction ?? 0);
                        promo.PlanPromoNetIncrementalEarnings = (promo.PlanPromoNetIncrementalMAC ?? 0) - (promo.PlanPromoBranding ?? 0) - (promo.PlanPromoBTL ?? 0) - (promo.PlanPromoCostProduction ?? 0);
                        promo.PlanPromoROIPercentLSV = promo.PlanPromoCost != 0 ? (promo.PlanPromoIncrementalEarningsLSV / promo.PlanPromoCost + 1) * 100 : 0;
                        promo.PlanPromoROIPercent = promo.PlanPromoCost != 0 ? (promo.PlanPromoIncrementalEarnings / promo.PlanPromoCost + 1) * 100 : 0;
                        promo.PlanPromoNetROIPercentLSV = promo.PlanPromoCost != 0 ? (promo.PlanPromoNetIncrementalEarningsLSV / promo.PlanPromoCost + 1) * 100 : 0;
                        promo.PlanPromoNetROIPercent = promo.PlanPromoCost != 0 ? (promo.PlanPromoNetIncrementalEarnings / promo.PlanPromoCost + 1) * 100 : 0;
                        // PriceIncrease
                        if (promo.PromoPriceIncrease != null)
                        {
                            promo.PromoPriceIncrease.PlanPromoNetIncrementalMACLSV = (promo.PromoPriceIncrease.PlanPromoNetIncrementalNSV ?? 0) - (promo.PromoPriceIncrease.PlanPromoNetIncrementalCOGS ?? 0);
                            promo.PromoPriceIncrease.PlanPromoNetIncrementalMAC = (promo.PromoPriceIncrease.PlanPromoNetIncrementalNSV ?? 0) - (promo.PromoPriceIncrease.PlanPromoNetIncrementalCOGSTn ?? 0);
                            promo.PromoPriceIncrease.PlanPromoIncrementalMACLSV = (promo.PromoPriceIncrease.PlanPromoIncrementalNSV ?? 0) - (promo.PromoPriceIncrease.PlanPromoIncrementalCOGS ?? 0);
                            promo.PromoPriceIncrease.PlanPromoIncrementalMAC = (promo.PromoPriceIncrease.PlanPromoIncrementalNSV ?? 0) - (promo.PromoPriceIncrease.PlanPromoIncrementalCOGSTn ?? 0);
                            promo.PromoPriceIncrease.PlanPromoIncrementalEarningsLSV = (promo.PromoPriceIncrease.PlanPromoIncrementalMACLSV ?? 0) - (promo.PlanPromoBranding ?? 0) - (promo.PlanPromoBTL ?? 0) - (promo.PlanPromoCostProduction ?? 0);
                            promo.PromoPriceIncrease.PlanPromoIncrementalEarnings = (promo.PromoPriceIncrease.PlanPromoIncrementalMAC ?? 0) - (promo.PlanPromoBranding ?? 0) - (promo.PlanPromoBTL ?? 0) - (promo.PlanPromoCostProduction ?? 0);
                            promo.PromoPriceIncrease.PlanPromoNetIncrementalEarningsLSV = (promo.PromoPriceIncrease.PlanPromoNetIncrementalMACLSV ?? 0) - (promo.PlanPromoBranding ?? 0) - (promo.PlanPromoBTL ?? 0) - (promo.PlanPromoCostProduction ?? 0);
                            promo.PromoPriceIncrease.PlanPromoNetIncrementalEarnings = (promo.PromoPriceIncrease.PlanPromoNetIncrementalMAC ?? 0) - (promo.PlanPromoBranding ?? 0) - (promo.PlanPromoBTL ?? 0) - (promo.PlanPromoCostProduction ?? 0);
                            promo.PromoPriceIncrease.PlanPromoROIPercentLSV = promo.PromoPriceIncrease.PlanPromoCost != 0 ? (promo.PromoPriceIncrease.PlanPromoIncrementalEarningsLSV / promo.PromoPriceIncrease.PlanPromoCost + 1) * 100 : 0;
                            promo.PromoPriceIncrease.PlanPromoROIPercent = promo.PromoPriceIncrease.PlanPromoCost != 0 ? (promo.PromoPriceIncrease.PlanPromoIncrementalEarnings / promo.PromoPriceIncrease.PlanPromoCost + 1) * 100 : 0;
                            promo.PromoPriceIncrease.PlanPromoNetROIPercentLSV = promo.PromoPriceIncrease.PlanPromoCost != 0 ? (promo.PromoPriceIncrease.PlanPromoNetIncrementalEarningsLSV / promo.PromoPriceIncrease.PlanPromoCost + 1) * 100 : 0;
                            promo.PromoPriceIncrease.PlanPromoNetROIPercent = promo.PromoPriceIncrease.PlanPromoCost != 0 ? (promo.PromoPriceIncrease.PlanPromoNetIncrementalEarnings / promo.PromoPriceIncrease.PlanPromoCost + 1) * 100 : 0;
                        }
                    }

                    double? RATIShopperPercent;
                    SimplePromoRATIShopper simplePromoRATIShopper = new SimplePromoRATIShopper(promo);

                    IQueryable<RATIShopper> ratishopperQuery = context.Set<RATIShopper>().Where(x => !x.Disabled);
                    RATIShopperPercent = PromoUtils.GetRATIShopperPercent(simplePromoRATIShopper, context, ratishopperQuery, out message);

                    if (message == null)
                    {
                        if (promo.IsInExchange)
                        {
                            promo.PlanAddTIShopperCalculated = promo.PlanPromoTIShopper - promo.Promoes.Sum(g => g.PlanPromoTIShopper) - (promo.PlanPromoNetIncrementalLSV - promo.Promoes.Sum(g => g.PlanPromoNetIncrementalLSV)) * (RATIShopperPercent ?? 0) / 100;
                        }
                        else
                        {
                            promo.PlanAddTIShopperCalculated = promo.PlanPromoTIShopper - promo.PlanPromoNetIncrementalLSV * (RATIShopperPercent ?? 0) / 100;
                        }
                        bool isApproved = promo.LastApprovedDate != null;
                        if (!isApproved)
                        {
                            promo.PlanAddTIShopperApproved = promo.PlanPromoTIShopper - promo.PlanPromoNetIncrementalLSV * (RATIShopperPercent ?? 0) / 100;
                        }

                        // +1 / -1 ?
                        //if (!promo.InOut.HasValue || !promo.InOut.Value)
                        //{
                        //    promo.PlanPromoROIPercent = promo.PlanPromoCost != 0 ? (promo.PlanPromoIncrementalEarnings / promo.PlanPromoCost + 1) * 100 : 0;
                        //    promo.PlanPromoNetROIPercent = promo.PlanPromoCost != 0 ? (promo.PlanPromoNetIncrementalEarnings / promo.PlanPromoCost + 1) * 100 : 0;
                        //}
                        //else
                        //{
                        //    promo.PlanPromoROIPercent = promo.PlanPromoTotalCost != 0 ? promo.PlanPromoIncrementalEarnings / promo.PlanPromoTotalCost * 100 : 0;
                        //    promo.PlanPromoNetROIPercent = promo.PlanPromoTotalCost != 0 ? promo.PlanPromoNetIncrementalEarnings / promo.PlanPromoTotalCost * 100 : 0;
                        //}

                        promo.PlanPromoNetUpliftPercent = promo.PlanPromoBaselineLSV != 0 ? promo.PlanPromoNetIncrementalLSV / promo.PlanPromoBaselineLSV * 100 : 0;
                        // PriceIncrease
                        if (promo.PromoPriceIncrease != null)
                        {
                            if (promo.IsInExchange)
                            {
                                promo.PromoPriceIncrease.PlanAddTIShopperCalculated = promo.PromoPriceIncrease.PlanPromoTIShopper - promo.Promoes.Sum(g => g.PromoPriceIncrease.PlanPromoTIShopper) - (promo.PromoPriceIncrease.PlanPromoNetIncrementalLSV - promo.Promoes.Sum(g => g.PromoPriceIncrease.PlanPromoNetIncrementalLSV)) * (RATIShopperPercent ?? 0) / 100;
                            }
                            else
                            {
                                promo.PromoPriceIncrease.PlanAddTIShopperCalculated = promo.PromoPriceIncrease.PlanPromoTIShopper - promo.PromoPriceIncrease.PlanPromoNetIncrementalLSV * (RATIShopperPercent ?? 0) / 100;
                            }
                            bool isApprovedPI = promo.LastApprovedDate != null;
                            if (!isApprovedPI)
                            {
                                promo.PromoPriceIncrease.PlanAddTIShopperApproved = promo.PromoPriceIncrease.PlanPromoTIShopper - promo.PromoPriceIncrease.PlanPromoNetIncrementalLSV * (RATIShopperPercent ?? 0) / 100;
                            }
                            promo.PromoPriceIncrease.PlanPromoNetUpliftPercent = promo.PromoPriceIncrease.PlanPromoBaselineLSV != 0 ? promo.PromoPriceIncrease.PlanPromoNetIncrementalLSV / promo.PromoPriceIncrease.PlanPromoBaselineLSV * 100 : 0;
                        }
                        if (PromoUtils.HasChanges(context.ChangeTracker, promo.Id))
                        {
                            promo.LastChangedDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                        }

                        context.SaveChanges();
                    }
                    else
                    {
                        return message;
                    }
                }
                else
                {
                    return message;
                }

                return null;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        /// <summary>
        /// Сбросить значения плановых полей
        /// </summary>
        /// <param name="promo">Промо</param>
        /// <param name="context">Контекст БД</param>
        private static void ResetValues(Promo promo, DatabaseContext context)
        {
            //promo.PlanPromoBaselineLSV = null;
            //promo.PlanPromoBaselineVolume = null;
            //promo.PlanPromoIncrementalLSV = null;
            //promo.PlanPromoIncrementalVolume = null;
            //promo.PlanPromoLSV = null;
            promo.PlanPromoTIShopper = promo.PlanPromoTIShopper != 0 ? null : promo.PlanPromoTIShopper;
            promo.PlanPromoCost = promo.PlanPromoCost != 0 ? null : promo.PlanPromoCost;
            promo.PlanPromoIncrementalBaseTI = promo.PlanPromoIncrementalBaseTI != 0 ? null : promo.PlanPromoIncrementalBaseTI;
            promo.PlanPromoNetIncrementalBaseTI = promo.PlanPromoNetIncrementalBaseTI != 0 ? null : promo.PlanPromoNetIncrementalBaseTI;
            promo.PlanPromoIncrementalCOGS = promo.PlanPromoIncrementalCOGS != 0 ? null : promo.PlanPromoIncrementalCOGS;
            promo.PlanPromoIncrementalCOGSTn = promo.PlanPromoIncrementalCOGSTn != 0 ? null : promo.PlanPromoIncrementalCOGSTn;
            promo.PlanPromoNetIncrementalCOGS = promo.PlanPromoNetIncrementalCOGS != 0 ? null : promo.PlanPromoNetIncrementalCOGS;
            promo.PlanPromoNetIncrementalCOGSTn = promo.PlanPromoNetIncrementalCOGSTn != 0 ? null : promo.PlanPromoNetIncrementalCOGSTn;
            promo.PlanPromoTotalCost = promo.PlanPromoTotalCost != 0 ? null : promo.PlanPromoTotalCost;
            promo.PlanPromoPostPromoEffectLSVW1 = promo.PlanPromoPostPromoEffectLSVW1 != 0 ? null : promo.PlanPromoPostPromoEffectLSVW1;
            promo.PlanPromoPostPromoEffectLSVW2 = promo.PlanPromoPostPromoEffectLSVW2 != 0 ? null : promo.PlanPromoPostPromoEffectLSVW2;
            promo.PlanPromoPostPromoEffectLSV = promo.PlanPromoPostPromoEffectLSV != 0 ? null : promo.PlanPromoPostPromoEffectLSV;
            promo.PlanPromoPostPromoEffectVolumeW1 = promo.PlanPromoPostPromoEffectVolumeW1 != 0 ? null : promo.PlanPromoPostPromoEffectVolumeW1;
            promo.PlanPromoPostPromoEffectVolumeW2 = promo.PlanPromoPostPromoEffectVolumeW2 != 0 ? null : promo.PlanPromoPostPromoEffectVolumeW2;
            promo.PlanPromoPostPromoEffectVolume = promo.PlanPromoPostPromoEffectVolume != 0 ? null : promo.PlanPromoPostPromoEffectVolume;
            promo.PlanPromoNetIncrementalLSV = promo.PlanPromoNetIncrementalLSV != 0 ? null : promo.PlanPromoNetIncrementalLSV;
            promo.PlanPromoNetIncrementalVolume = promo.PlanPromoNetIncrementalVolume != 0 ? null : promo.PlanPromoNetIncrementalVolume;
            promo.PlanPromoNetLSV = promo.PlanPromoNetLSV != 0 ? null : promo.PlanPromoNetLSV;
            promo.PlanPromoBaselineBaseTI = promo.PlanPromoBaselineBaseTI != 0 ? null : promo.PlanPromoBaselineBaseTI;
            promo.PlanPromoBaseTI = promo.PlanPromoBaseTI != 0 ? null : promo.PlanPromoBaseTI;
            promo.PlanPromoNetBaseTI = promo.PlanPromoNetBaseTI != 0 ? null : promo.PlanPromoNetBaseTI;
            promo.PlanPromoNSV = promo.PlanPromoNSV != 0 ? null : promo.PlanPromoNSV;
            promo.PlanPromoNetNSV = promo.PlanPromoNetNSV != 0 ? null : promo.PlanPromoNetNSV;
            promo.PlanPromoIncrementalNSV = promo.PlanPromoIncrementalNSV != 0 ? null : promo.PlanPromoIncrementalNSV;
            promo.PlanPromoNetIncrementalNSV = promo.PlanPromoNetIncrementalNSV != 0 ? null : promo.PlanPromoNetIncrementalNSV;
            promo.PlanPromoIncrementalMAC = promo.PlanPromoIncrementalMAC != 0 ? null : promo.PlanPromoIncrementalMAC;
            promo.PlanPromoNetIncrementalMAC = promo.PlanPromoNetIncrementalMAC != 0 ? null : promo.PlanPromoNetIncrementalMAC;
            promo.PlanPromoIncrementalEarnings = promo.PlanPromoIncrementalEarnings != 0 ? null : promo.PlanPromoIncrementalEarnings;
            promo.PlanPromoNetIncrementalEarnings = promo.PlanPromoNetIncrementalEarnings != 0 ? null : promo.PlanPromoNetIncrementalEarnings;
            promo.PlanPromoROIPercent = promo.PlanPromoROIPercent != 0 ? null : promo.PlanPromoROIPercent;
            promo.PlanPromoNetROIPercent = promo.PlanPromoNetROIPercent != 0 ? null : promo.PlanPromoNetROIPercent;
            promo.PlanPromoNetUpliftPercent = promo.PlanPromoNetUpliftPercent != 0 ? null : promo.PlanPromoNetUpliftPercent;
        }
    }
}