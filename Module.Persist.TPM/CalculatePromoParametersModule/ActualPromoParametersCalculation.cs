﻿using Module.Persist.TPM.Model.SimpleModel;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.CalculatePromoParametersModule
{
    public class ActualPromoParametersCalculation
    {
        /// <summary>
        /// Расчитать фактические параметры для промо
        /// </summary>
        /// <param name="promo">Промо</param>
        /// <param name="context">Контекст БД</param>
        /// <param name="lockedActualLSV">Блокировка значений, введенных Demand'ом</param>
        /// <returns>Null при успешном расчете, иначе строку с ошибками</returns>
        public static string CalculatePromoParameters(Promo promo, DatabaseContext context, bool lockedActualLSV = false, bool needToSaveChanges = true, bool useActualCOGS = false, bool useActualTI = false)
        {
            if (promo != null && (promo.PromoStatus.SystemName == "Finished" || (promo.PromoStatus.SystemName == "Closed" && (useActualCOGS || useActualTI))))
            {
                List<ClientTree> clientTrees = context.Set<ClientTree>().Where(g => g.EndDate == null).ToList();
                List<BrandTech> brandTeches = context.Set<BrandTech>().Where(g => !g.Disabled).ToList();

                PromoStatus finishedStatus = context.Set<PromoStatus>().Where(x => x.SystemName.ToLower() == "finished" && !x.Disabled).FirstOrDefault();
                PromoStatus closedStatus = context.Set<PromoStatus>().Where(x => x.SystemName.ToLower() == "closed" && !x.Disabled).FirstOrDefault();
                Promo promoCopy = AutomapperProfiles.PromoCopy(promo);

                bool isActualPromoBaseLineLSVChangedByDemand = promo.PromoStatusId == closedStatus.Id ||
                                                            (promo.PromoStatusId == finishedStatus.Id
                                                            && promo.ActualPromoBaselineLSV != null
                                                            && promo.ActualPromoBaselineLSV != promo.PlanPromoBaselineLSV);
                bool isActualPromoLSVChangedByDemand = promo.PromoStatusId == closedStatus.Id ||
                                                    (promo.PromoStatusId == finishedStatus.Id
                                                    && promo.ActualPromoLSVSO != null
                                                    && promo.ActualPromoLSVSO != 0);
                bool isActualPromoProstPromoEffectLSVChangedByDemand = promo.PromoStatusId == closedStatus.Id ||
                                                                    (promo.PromoStatusId == finishedStatus.Id
                                                                    && promo.ActualPromoPostPromoEffectLSV != null
                                                                    && promo.ActualPromoPostPromoEffectLSV != 0);

                ResetValues(promo, context, !isActualPromoBaseLineLSVChangedByDemand, !isActualPromoProstPromoEffectLSVChangedByDemand);
                // подготовительная часть, проверяем все ли данные имеются
                string errors = "";

                if (!promo.InOut.HasValue || !promo.InOut.Value)
                {
                    if (!promo.PlanPromoBaselineLSV.HasValue)
                    {
                        errors += Log.GenerateLogLine(Log.MessageType["Error"], "For promo №") + promo.Number + " is no Plan Promo Baseline LSV value. Actual parameters will not be calculated.";
                    }
                }

                // ищем TI Base
                string message = null;
                bool error;
                double? TIBasePercent;
                double? COGSPercent;
                double? COGSTn;
                SimplePromoTradeInvestment simplePromoTradeInvestment = new SimplePromoTradeInvestment(promo);

                useActualTI = promo.UseActualTI == true ? promo.UseActualTI : useActualTI;

                if (useActualTI)
                {
                    List<ActualTradeInvestment> actualTIQuery = context.Set<ActualTradeInvestment>().Where(x => !x.Disabled).ToList();
                    TIBasePercent = PromoUtils.GetTIBasePercent(simplePromoTradeInvestment, new List<BaseTradeInvestment>(actualTIQuery), clientTrees, brandTeches, out message, out error);
                    promo.ActualTIBasePercent = TIBasePercent;
                    if (TIBasePercent == null)
                    {
                        List<TradeInvestment> TIQuery = context.Set<TradeInvestment>().Where(x => !x.Disabled).ToList();
                        TIBasePercent = PromoUtils.GetTIBasePercent(simplePromoTradeInvestment, new List<BaseTradeInvestment>(TIQuery), clientTrees, brandTeches, out message, out error);
                        promo.ActualTIBasePercent = TIBasePercent;
                    }
                }
                else
                {
                    List<TradeInvestment> TIQuery = context.Set<TradeInvestment>().Where(x => !x.Disabled).ToList();
                    TIBasePercent = PromoUtils.GetTIBasePercent(simplePromoTradeInvestment, new List<BaseTradeInvestment>(TIQuery), clientTrees, brandTeches, out message, out error);
                    promo.PlanTIBasePercent = TIBasePercent;
                }

                if (message != null)
                    errors += message + ";";

                // ищем COGS
                SimplePromoCOGS simplePromoCOGS = new SimplePromoCOGS(promo);
                SimplePromoCOGS simplePromoCOGStn = new SimplePromoCOGS(promo);
                useActualCOGS = promo.UseActualCOGS == true ? promo.UseActualCOGS : useActualCOGS;

                if (useActualCOGS)
                {
                    List<ActualCOGS> actualcogsQuery = context.Set<ActualCOGS>().Where(x => !x.Disabled).ToList();
                    COGSPercent = PromoUtils.GetCOGSPercent(simplePromoCOGS, new List<BaseCOGS>(actualcogsQuery), clientTrees, brandTeches, out message);
                    promo.ActualCOGSPercent = COGSPercent;
                    if (COGSPercent == null)
                    {
                        List<COGS> cogsQuery = context.Set<COGS>().Where(x => !x.Disabled).ToList();
                        COGSPercent = PromoUtils.GetCOGSPercent(simplePromoCOGS, new List<BaseCOGS>(cogsQuery), clientTrees, brandTeches, out message);
                        promo.ActualCOGSPercent = COGSPercent;
                    }
                }
                else
                {
                    List<COGS> cogsQuery = context.Set<COGS>().Where(x => !x.Disabled).ToList();
                    COGSPercent = PromoUtils.GetCOGSPercent(simplePromoCOGS, new List<BaseCOGS>(cogsQuery), clientTrees, brandTeches, out message);
                    promo.PlanCOGSPercent = COGSPercent;
                }

                if (useActualCOGS)
                {
                    List<ActualCOGSTn> actualcogsQueryTn = context.Set<ActualCOGSTn>().Where(x => !x.Disabled).ToList();
                    COGSTn = PromoUtils.GetCOGSTonCost(simplePromoCOGStn, new List<BaseCOGSTn>(actualcogsQueryTn), clientTrees, brandTeches, out message);
                    promo.ActualCOGSTn = COGSTn;
                    if (COGSTn == null)
                    {
                        List<PlanCOGSTn> cogsQueryTn = context.Set<PlanCOGSTn>().Where(x => !x.Disabled).ToList();
                        COGSTn = PromoUtils.GetCOGSTonCost(simplePromoCOGStn, new List<BaseCOGSTn>(cogsQueryTn), clientTrees, brandTeches, out message);
                        promo.ActualCOGSTn = COGSTn;
                    }
                }
                else
                {
                    List<PlanCOGSTn> cogsQueryTn = context.Set<PlanCOGSTn>().Where(x => !x.Disabled).ToList();
                    COGSTn = PromoUtils.GetCOGSTonCost(simplePromoCOGStn, new List<BaseCOGSTn>(cogsQueryTn), clientTrees, brandTeches, out message);
                    promo.PlanCOGSTn = COGSTn;
                }


                if (message != null)
                    errors += message + ";";

                //зачем этот тут? О_о
                //promo.PlanPromoIncrementalBaseTI = promo.PlanPromoIncrementalLSV * TIBasePercent / 100;

                // обращение к БД в try-catch, всё не нужно и производительнее будет
                ClientTree clientTree = null;
                try
                {
                    clientTree = context.Set<ClientTree>().Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue).FirstOrDefault();
                    if (clientTree == null)
                        errors += Log.GenerateLogLine(Log.MessageType["Error"], "For promo №") + promo.Number + " client not found. Actual parameters will not be calculated.";
                }
                catch (Exception e)
                {
                    errors += e.Message + ";";
                }
                List<PromoProduct> promoProducts = context.Set<PromoProduct>().Where(p => !p.Disabled && p.PromoId == promo.Id).ToList();
                // если ошибок нет - считаем
                if (errors.Length == 0)
                {
                    // если значения введены вручную через грид ActualLSV, то не нужно обновлять
                    if (promo.IsOnInvoice)
                    {
                        promo.ActualPromoLSV = promo.ActualPromoLSVSI;
                    }
                    else if (!isActualPromoLSVChangedByDemand)
                    {
                        promo.ActualPromoLSV = 0;
                    }

                    if (promo.ManualInputSumInvoice == false)
                    {
                        promo.SumInvoice = promo.ActualPromoLSVByCompensation * promo.MarsMechanicDiscount / 100;
                    }
                    promo.ActualPromoTIShopper = promo.SumInvoice;

                    promo.ActualPromoCost = (promo.ActualPromoTIShopper ?? 0) + (promo.ActualPromoTIMarketing ?? 0) + (promo.ActualPromoBranding ?? 0) + (promo.ActualPromoBTL ?? 0) + (promo.ActualPromoCostProduction ?? 0);

                    promo.ActualPromoBaseTI = (promo.ActualPromoLSV ?? 0) * TIBasePercent / 100;

                    if (!promo.InOut.HasValue || !promo.InOut.Value)
                    {
                        if (!isActualPromoBaseLineLSVChangedByDemand)
                        {
                            promo.ActualPromoBaselineLSV = promo.PlanPromoBaselineLSV;
                        }

                        promo.ActualPromoIncrementalLSV = (promo.ActualPromoLSV ?? 0) - (promo.ActualPromoBaselineLSV ?? 0);

                        if (promo.IsOnInvoice && (promo.PlanPromoPostPromoEffectLSV.HasValue && promo.PlanPromoPostPromoEffectLSV != 0))
                        {
                            promo.ActualPromoPostPromoEffectLSVW1 = promoProducts.Sum(g => g.ActualProductPostPromoEffectLSVW1);
                            promo.ActualPromoPostPromoEffectLSVW2 = promoProducts.Sum(g => g.ActualProductPostPromoEffectLSVW2);
                        }

                        promo.ActualPromoPostPromoEffectLSV = promo.ActualPromoPostPromoEffectLSVW1 + promo.ActualPromoPostPromoEffectLSVW2;
                        promo.ActualPromoNetIncrementalLSV = (promo.ActualPromoIncrementalLSV ?? 0) + (promo.ActualPromoPostPromoEffectLSV ?? 0);
                        promo.ActualPromoUpliftPercent = promo.ActualPromoBaselineLSV == 0 ? 0 : promo.ActualPromoIncrementalLSV / promo.ActualPromoBaselineLSV * 100;
                        promo.ActualPromoNetUpliftPercent = promo.ActualPromoBaselineLSV == 0 ? 0 : promo.ActualPromoNetIncrementalLSV / promo.ActualPromoBaselineLSV * 100;
                        //volume
                        promo.ActualPromoPostPromoEffectVolume = promoProducts.Sum(g => g.ActualProductPostPromoEffectLSV / (g.Price / g.Product.UOM_PC2Case) * g.Product.PCVolume);
                    }
                    else
                    {
                        if (!isActualPromoBaseLineLSVChangedByDemand)
                        {
                            promo.ActualPromoBaselineLSV = 1;
                        }

                        promo.ActualPromoIncrementalLSV = (promo.ActualPromoLSV ?? 0) - (promo.ActualPromoBaselineLSV ?? 0);
                        if (promo.ActualPromoIncrementalLSV < 0) promo.ActualPromoIncrementalLSV = 0;

                        promo.ActualPromoNetIncrementalLSV = (promo.ActualPromoIncrementalLSV ?? 0);

                        promo.ActualPromoUpliftPercent = null;
                        promo.ActualPromoNetUpliftPercent = null;
                        //volume
                        promo.ActualPromoPostPromoEffectVolume = 0;
                    }

                    promo.ActualPromoIncrementalBaseTI = promo.ActualPromoIncrementalLSV * TIBasePercent / 100;
                    promo.ActualPromoNetIncrementalBaseTI = promo.ActualPromoNetIncrementalLSV * TIBasePercent / 100;

                    promo.ActualPromoIncrementalCOGS = promo.ActualPromoIncrementalLSV * COGSPercent / 100;
                    promo.ActualPromoNetIncrementalCOGS = promo.ActualPromoNetIncrementalLSV * COGSPercent / 100;

                    promo.ActualPromoNetLSV = (promo.ActualPromoBaselineLSV ?? 0) + (promo.ActualPromoNetIncrementalLSV ?? 0);
                    promo.ActualPromoNetBaseTI = promo.ActualPromoNetLSV * TIBasePercent / 100;

                    promo.ActualPromoTotalCost = (promo.ActualPromoCost ?? 0) + (promo.ActualPromoBaseTI ?? 0);
                    promo.ActualPromoIncrementalNSV = (promo.ActualPromoIncrementalLSV ?? 0) - (promo.ActualPromoTIShopper ?? 0) - (promo.ActualPromoTIMarketing ?? 0) - (promo.ActualPromoIncrementalBaseTI ?? 0);
                    promo.ActualPromoNetIncrementalNSV = (promo.ActualPromoNetIncrementalLSV ?? 0) - (promo.ActualPromoTIShopper ?? 0) - (promo.ActualPromoTIMarketing ?? 0) - (promo.ActualPromoNetIncrementalBaseTI ?? 0);
                    promo.ActualPromoNetNSV = (promo.ActualPromoNetLSV ?? 0) - (promo.ActualPromoTIShopper ?? 0) - (promo.ActualPromoTIMarketing ?? 0) - (promo.ActualPromoNetBaseTI ?? 0);

                    //if (!promo.InOut.HasValue || !promo.InOut.Value)
                    //{
                    //    // +1 / -1 ?
                    //    promo.ActualPromoROIPercent = promo.ActualPromoCost == 0 ? 0 : (promo.ActualPromoIncrementalEarnings / promo.ActualPromoCost + 1) * 100;
                    //    promo.ActualPromoNetROIPercent = promo.ActualPromoCost == 0 ? 0 : (promo.ActualPromoNetIncrementalEarnings / promo.ActualPromoCost + 1) * 100;
                    //}
                    //else
                    //{
                    //    // +1 / -1 ?
                    //    promo.ActualPromoROIPercent = promo.ActualPromoTotalCost == 0 ? 0 : (promo.ActualPromoIncrementalEarnings / promo.ActualPromoTotalCost + 1) * 100;
                    //    promo.ActualPromoNetROIPercent = promo.ActualPromoTotalCost == 0 ? 0 : (promo.ActualPromoNetIncrementalEarnings / promo.ActualPromoTotalCost + 1) * 100;
                    //}

                    //Volume
                    promo.ActualPromoBaselineVolume = promoProducts.Sum(g => g.ActualProductBaselineLSV / (g.Price / g.Product.UOM_PC2Case) *  g.Product.PCVolume);
                    if (!promo.IsOnInvoice || promo.InOut.HasValue || promo.InOut.Value)
                    {
                        promo.ActualPromoVolume = promoProducts.Sum(g => g.ActualProductQtySO * g.Product.PCVolume);
                    }
                    else
                    {
                        promo.ActualPromoVolume = promo.ActualPromoVolumeSI;
                    }

                    promo.ActualPromoIncrementalVolume = promo.ActualPromoVolume - promo.ActualPromoBaselineVolume;
                    promo.ActualPromoNetIncrementalVolume = promo.ActualPromoIncrementalVolume + promo.ActualPromoPostPromoEffectVolume;

                    promo.ActualPromoIncrementalCOGSTn = promo.ActualPromoIncrementalVolume * COGSTn;
                    promo.ActualPromoNetIncrementalCOGSTn = promo.ActualPromoNetIncrementalVolume * COGSTn;

                    if (promo.IsLSVBased)
                    {
                        promo.ActualPromoNetIncrementalMAC = (promo.ActualPromoNetIncrementalNSV ?? 0) - (promo.ActualPromoNetIncrementalCOGS ?? 0);
                        promo.ActualPromoIncrementalMAC = (promo.ActualPromoIncrementalNSV ?? 0) - (promo.ActualPromoIncrementalCOGS ?? 0);
                        promo.ActualPromoIncrementalEarnings = (promo.ActualPromoIncrementalMAC ?? 0) - (promo.ActualPromoBranding ?? 0) - (promo.ActualPromoBTL ?? 0) - (promo.ActualPromoCostProduction ?? 0);
                        promo.ActualPromoNetIncrementalEarnings = (promo.ActualPromoNetIncrementalMAC ?? 0) - (promo.ActualPromoBranding ?? 0) - (promo.ActualPromoBTL ?? 0) - (promo.ActualPromoCostProduction ?? 0);
                        promo.ActualPromoROIPercent = promo.ActualPromoCost == 0 ? 0 : (promo.ActualPromoIncrementalEarnings / promo.ActualPromoCost + 1) * 100;
                        promo.ActualPromoNetROIPercent = promo.ActualPromoCost == 0 ? 0 : (promo.ActualPromoNetIncrementalEarnings / promo.ActualPromoCost + 1) * 100;
                    }
                    else
                    {
                        promo.ActualPromoNetIncrementalMACLSV = (promo.ActualPromoNetIncrementalNSV ?? 0) - (promo.ActualPromoNetIncrementalCOGS ?? 0);
                        promo.ActualPromoNetIncrementalMAC = (promo.ActualPromoNetIncrementalNSV ?? 0) - (promo.ActualPromoNetIncrementalCOGSTn ?? 0);
                        promo.ActualPromoIncrementalMACLSV = (promo.ActualPromoIncrementalNSV ?? 0) - (promo.ActualPromoIncrementalCOGS ?? 0);
                        promo.ActualPromoIncrementalMAC = (promo.ActualPromoIncrementalNSV ?? 0) - (promo.ActualPromoIncrementalCOGSTn ?? 0);
                        promo.ActualPromoIncrementalEarningsLSV = (promo.ActualPromoIncrementalMACLSV ?? 0) - (promo.ActualPromoBranding ?? 0) - (promo.ActualPromoBTL ?? 0) - (promo.ActualPromoCostProduction ?? 0);
                        promo.ActualPromoIncrementalEarnings = (promo.ActualPromoIncrementalMAC ?? 0) - (promo.ActualPromoBranding ?? 0) - (promo.ActualPromoBTL ?? 0) - (promo.ActualPromoCostProduction ?? 0);
                        promo.ActualPromoNetIncrementalEarningsLSV = (promo.ActualPromoNetIncrementalMACLSV ?? 0) - (promo.ActualPromoBranding ?? 0) - (promo.ActualPromoBTL ?? 0) - (promo.ActualPromoCostProduction ?? 0);
                        promo.ActualPromoNetIncrementalEarnings = (promo.ActualPromoNetIncrementalMAC ?? 0) - (promo.ActualPromoBranding ?? 0) - (promo.ActualPromoBTL ?? 0) - (promo.ActualPromoCostProduction ?? 0);
                        promo.ActualPromoROIPercentLSV = promo.ActualPromoCost == 0 ? 0 : (promo.ActualPromoIncrementalEarningsLSV / promo.ActualPromoCost + 1) * 100;
                        promo.ActualPromoROIPercent = promo.ActualPromoCost == 0 ? 0 : (promo.ActualPromoIncrementalEarnings / promo.ActualPromoCost + 1) * 100;
                        promo.ActualPromoNetROIPercentLSV = promo.ActualPromoCost == 0 ? 0 : (promo.ActualPromoNetIncrementalEarningsLSV / promo.ActualPromoCost + 1) * 100;
                        promo.ActualPromoNetROIPercent = promo.ActualPromoCost == 0 ? 0 : (promo.ActualPromoNetIncrementalEarnings / promo.ActualPromoCost + 1) * 100;

                    }

                    //if (!promo.InOut.HasValue || !promo.InOut.Value)
                    //{
                    //    promo.ActualPromoTotalCost = (promo.ActualPromoCost ?? 0) + (promo.ActualPromoBaseTI ?? 0);
                    //    promo.ActualPromoIncrementalNSV = (promo.ActualPromoIncrementalLSV ?? 0) - (promo.ActualPromoTIShopper ?? 0) - (promo.ActualPromoTIMarketing ?? 0) - (promo.ActualPromoIncrementalBaseTI ?? 0);
                    //    promo.ActualPromoNetIncrementalNSV = (promo.ActualPromoNetIncrementalLSV ?? 0) - (promo.ActualPromoTIShopper ?? 0) - (promo.ActualPromoTIMarketing ?? 0) - (promo.ActualPromoNetIncrementalBaseTI ?? 0);
                    //    promo.ActualPromoNetNSV = (promo.ActualPromoNetLSV ?? 0) - (promo.ActualPromoTIShopper ?? 0) - (promo.ActualPromoTIMarketing ?? 0) - (promo.ActualPromoNetBaseTI ?? 0);
                    //    promo.ActualPromoNetIncrementalMAC = (promo.ActualPromoNetIncrementalNSV ?? 0) - (promo.ActualPromoNetIncrementalCOGS ?? 0);
                    //}
                    //else
                    //{
                    //    promo.ActualPromoTotalCost = (promo.ActualPromoCost ?? 0) + (promo.ActualPromoIncrementalBaseTI ?? 0) + (promo.ActualPromoIncrementalCOGS ?? 0);
                    //    promo.ActualPromoIncrementalNSV = (promo.ActualPromoNetLSV ?? 0) - (promo.ActualPromoTIShopper ?? 0) - (promo.ActualPromoTIMarketing ?? 0) - (promo.ActualPromoIncrementalBaseTI ?? 0);
                    //    promo.ActualPromoNetIncrementalNSV = (promo.ActualPromoNetIncrementalLSV ?? 0) - (promo.ActualPromoTIShopper ?? 0) - (promo.ActualPromoTIMarketing ?? 0) - (promo.ActualPromoIncrementalBaseTI ?? 0);
                    //    promo.ActualPromoNetNSV = (promo.ActualPromoNetLSV ?? 0) - (promo.ActualPromoTIShopper ?? 0) - (promo.ActualPromoTIMarketing ?? 0) - (promo.ActualPromoBaseTI ?? 0);
                    //    promo.ActualPromoNetIncrementalMAC = (promo.ActualPromoNetIncrementalNSV ?? 0) - (promo.ActualPromoIncrementalCOGS ?? 0);
                    //}

                    promo.ActualPromoBaselineBaseTI = (promo.ActualPromoBaselineLSV ?? 0) * TIBasePercent / 100;

                    promo.ActualPromoNSV = (promo.ActualPromoLSV ?? 0) - (promo.ActualPromoTIShopper ?? 0) - (promo.ActualPromoTIMarketing ?? 0) - (promo.ActualPromoBaseTI ?? 0);
                    if(promo.ActualPromoNSV.HasValue && promo.ActualPromoNSV != 0)
                        promo.ActualPromoNSVtn = promo.ActualPromoNSV / promo.ActualPromoVolumeSI;

                    // ищем RATIShopper
                    double? RATIShopperPercent;
                    SimplePromoRATIShopper simplePromoRATIShopper = new SimplePromoRATIShopper(promo);

                    IQueryable<RATIShopper> ratishopperQuery = context.Set<RATIShopper>().Where(x => !x.Disabled);
                    RATIShopperPercent = PromoUtils.GetRATIShopperPercent(simplePromoRATIShopper, context, ratishopperQuery, out message);

                    if (promo.IsInExchange)
                    {
                        promo.ActualAddTIShopper = promo.ActualPromoTIShopper - promo.Promoes.Sum(g=>g.PlanPromoTIShopper) - (promo.ActualPromoNetIncrementalLSV - promo.Promoes.Sum(g => g.PlanPromoNetIncrementalLSV)) * (RATIShopperPercent ?? 0) / 100;
                    }
                    else
                    {
                        promo.ActualAddTIShopper = promo.ActualPromoTIShopper - promo.ActualPromoNetIncrementalLSV * (RATIShopperPercent ?? 0) / 100;
                    }

                    promo.ActualAddTIMarketing = promo.ActualPromoTIMarketing - (promo.PlanPromoTIMarketing - promo.PlanAddTIMarketingApproved) > 0 ?
                        promo.ActualPromoTIMarketing - (promo.PlanPromoTIMarketing - promo.PlanAddTIMarketingApproved) : 0;

                    if (message != null)
                        errors += message + ";";


                    if (PromoUtils.HasChanges(context.ChangeTracker, promo.Id))
                    {
                        promo.LastChangedDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                        if (IsDemandChanged(promo, promoCopy))
                        {
                            promo.LastChangedDateDemand = promo.LastChangedDate;
                            promo.LastChangedDateFinance = promo.LastChangedDate;
                        }
                    }

                    try
                    {
                        if (needToSaveChanges)
                        {
                            context.SaveChanges();
                        }
                    }
                    catch (Exception e)
                    {
                        errors += e.Message + ";";
                    }
                }

                if (!promo.ActualPromoLSV.HasValue)
                {
                    errors += Log.GenerateLogLine(Log.MessageType["Warning"], "For promo №") + promo.Number + " is no Actual Promo LSV. Please, fill this parameter.";
                }

                if (!promo.ActualPromoXSites.HasValue || !promo.ActualPromoCatalogue.HasValue || !promo.ActualPromoBranding.HasValue
                    || !promo.ActualPromoBTL.HasValue || !promo.ActualPromoCostProdXSites.HasValue || !promo.ActualPromoCostProdCatalogue.HasValue)
                {
                    errors += Log.GenerateLogLine(Log.MessageType["Warning"], "For promo №") + promo.Number + " is no Budget values.";
                }

                return errors.Length == 0 ? null : errors;
            }

            return null;
        }

        /// <summary>
        /// Сбросить значения фактических полей
        /// </summary>
        /// <param name="promo">Промо</param>
        /// <param name="context">Контекст БД</param>
        private static void ResetValues(Promo promo, DatabaseContext context, bool resetActualPromoBaselineLSV, bool resetActualPromoPostPromoEffectLSV)
        {
            // если значения введены вручную через грид ActualLSV, то ненужно обновлять
            if (resetActualPromoBaselineLSV)
            {
                promo.ActualPromoBaselineLSV = promo.ActualPromoBaselineLSV != 0 ? null : promo.ActualPromoBaselineLSV;
            }

            if (resetActualPromoPostPromoEffectLSV)
            {
                promo.ActualPromoPostPromoEffectLSVW1 = 0;
                promo.ActualPromoPostPromoEffectLSVW2 = 0;
                promo.ActualPromoPostPromoEffectLSV = promo.ActualPromoPostPromoEffectLSV != 0 ? null : promo.ActualPromoPostPromoEffectLSV;
            }

            promo.ActualPromoIncrementalLSV = promo.ActualPromoIncrementalLSV != 0 ? null : promo.ActualPromoIncrementalLSV;
            promo.ActualPromoUpliftPercent = promo.ActualPromoUpliftPercent != 0 ? null : promo.ActualPromoUpliftPercent;
            promo.ActualPromoNetBaseTI = promo.ActualPromoNetBaseTI != 0 ? null : promo.ActualPromoNetBaseTI;
            promo.ActualPromoNSV = promo.ActualPromoNSV != 0 ? null : promo.ActualPromoNSV;
            promo.ActualPromoTIShopper = promo.ActualPromoTIShopper != 0 ? null : promo.ActualPromoTIShopper;
            promo.ActualPromoCost = promo.ActualPromoCost != 0 ? null : promo.ActualPromoCost;
            promo.ActualPromoIncrementalBaseTI = promo.ActualPromoIncrementalBaseTI != 0 ? null : promo.ActualPromoIncrementalBaseTI;
            promo.ActualPromoNetIncrementalBaseTI = promo.ActualPromoNetIncrementalBaseTI != 0 ? null : promo.ActualPromoNetIncrementalBaseTI;
            promo.ActualPromoIncrementalCOGS = promo.ActualPromoIncrementalCOGS != 0 ? null : promo.ActualPromoIncrementalCOGS;
            promo.ActualPromoNetIncrementalCOGS = promo.ActualPromoNetIncrementalCOGS != 0 ? null : promo.ActualPromoNetIncrementalCOGS;
            promo.ActualPromoTotalCost = promo.ActualPromoTotalCost != 0 ? null : promo.ActualPromoTotalCost;
            promo.ActualPromoNetLSV = promo.ActualPromoNetLSV != 0 ? null : promo.ActualPromoNetLSV;
            promo.ActualPromoIncrementalNSV = promo.ActualPromoIncrementalNSV != 0 ? null : promo.ActualPromoIncrementalNSV;
            promo.ActualPromoNetIncrementalNSV = promo.ActualPromoNetIncrementalNSV != 0 ? null : promo.ActualPromoNetIncrementalNSV;
            promo.ActualPromoIncrementalMAC = promo.ActualPromoIncrementalMAC != 0 ? null : promo.ActualPromoIncrementalMAC;
            promo.ActualPromoNetIncrementalMAC = promo.ActualPromoNetIncrementalMAC != 0 ? null : promo.ActualPromoNetIncrementalMAC;
            promo.ActualPromoIncrementalEarnings = promo.ActualPromoIncrementalEarnings != 0 ? null : promo.ActualPromoIncrementalEarnings;
            promo.ActualPromoNetIncrementalEarnings = promo.ActualPromoNetIncrementalEarnings != 0 ? null : promo.ActualPromoNetIncrementalEarnings;
            promo.ActualPromoROIPercent = promo.ActualPromoROIPercent != 0 ? null : promo.ActualPromoROIPercent;
            promo.ActualPromoNetROIPercent = promo.ActualPromoNetROIPercent != 0 ? null : promo.ActualPromoNetROIPercent;
            promo.ActualPromoNetUpliftPercent = promo.ActualPromoNetUpliftPercent != 0 ? null : promo.ActualPromoNetUpliftPercent;
        }

        /// <summary>
        /// Устанавливает ActualproductBaselineLSV для записей из таблицы PromoProduct.
        /// Рассчитывает ActualPromoBaselineLSV (сумма всех ActualProductBaselineLSV).
        /// </summary>
        /// <param name="context"></param>
        /// <param name="promo"></param>
        /// <returns></returns>
        private static double CalculateActualPromoBaselineLSV(DatabaseContext context, Promo promo)
        {
            // Получаем все записи из таблицы PromoProduct для текущего промо.
            var promoProductsForCurrentPromo = context.Set<PromoProduct>()
                .Where(x => x.PromoId == promo.Id);

            // Сумма всех ActualProductBaselineLSV.
            double actualPromoBaselineLSV = 0;

            // Если есть от чего считать долю.
            if (promo.PlanPromoBaselineLSV.HasValue)
            {
                // Перебираем все найденные для текущего промо записи из таблицы PromoProduct.
                foreach (var promoProduct in promoProductsForCurrentPromo)
                {
                    // Если PlanProductBaselineLSV нет, то мы не сможем посчитать долю
                    if (promoProduct.PlanProductBaselineLSV.HasValue)
                    {
                        double currentProductBaselineLSVPercent = 0;
                        // Если показатель PlanProductBaselineLSV == 0, то он составляет 0 процентов от показателя PlanPromoBaselineLSV.
                        if (promoProduct.PlanProductBaselineLSV.Value != 0)
                        {
                            // Считаем долю PlanProductBaselineLSV от PlanPromoBaselineLSV.
                            currentProductBaselineLSVPercent = promoProduct.PlanProductBaselineLSV.Value / promo.PlanPromoBaselineLSV.Value;
                        }
                        // Устанавливаем ActualProductBaselineLSV в запись таблицы PromoProduct.
                        promoProduct.ActualProductBaselineLSV = promo.PlanPromoBaselineLSV.Value * currentProductBaselineLSVPercent;

                        // Суммираем все ActualProductBaselineLSV для получения ActualPromoBaselineLSV.
                        actualPromoBaselineLSV += promoProduct.ActualProductBaselineLSV.Value;
                    }
                }
                context.SaveChangesAsync();
            }

            return actualPromoBaselineLSV;
        }

        private static bool IsDemandChanged(Promo oldPromo, Promo newPromo)
        {
            if (oldPromo.ActualPromoUpliftPercent != newPromo.ActualPromoUpliftPercent
                || oldPromo.ActualPromoLSV != oldPromo.ActualPromoLSV
                || oldPromo.ActualPromoLSVByCompensation != oldPromo.ActualPromoLSVByCompensation
                || oldPromo.ActualPromoIncrementalLSV != oldPromo.ActualPromoIncrementalLSV)
                return true;
            else return false;
        }
    }
}
