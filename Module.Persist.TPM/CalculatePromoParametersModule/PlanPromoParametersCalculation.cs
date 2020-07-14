using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module.Persist.TPM.Model.TPM;
using Persist;
using Module.Persist.TPM.Utils.Filter;
using System.Data.Entity;
using System.Data.Entity.Validation;
using Module.Persist.TPM.Utils;
using Module.Persist.TPM.Model.SimpleModel;

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
                Promo promo = context.Set<Promo>().Where(x => x.Id == promoId && !x.Disabled).FirstOrDefault();
                Promo promoCopy = new Promo(promo);

                ResetValues(promo, context);
                double? sumPlanProductBaseLineLSV = context.Set<PromoProduct>().Where(x => x.PromoId == promoId && !x.Disabled).Sum(x => x.PlanProductBaselineLSV);
                ClientTree clientTree = context.Set<ClientTree>().Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue).FirstOrDefault();

                //promo.PlanPromoBaselineLSV = sumPlanProductBaseLineLSV;
                //promo.PlanPromoIncrementalLSV = sumPlanProductBaseLineLSV * promo.PlanPromoUpliftPercent / 100;
                //promo.PlanPromoLSV = promo.PlanPromoBaselineLSV + promo.PlanPromoIncrementalLSV;  
                promo.PlanPromoTIShopper = promo.PlanPromoLSV * (promo.MarsMechanicDiscount / 100) * (clientTree?.DistrMarkUp ?? 1);
                // бюджеты пересчитывать не требуется (пусть пока будет закомментировано)
                //promo.PlanPromoTIMarketing = promo.PlanPromoXSites + promo.PlanPromoCatalogue + promo.PlanPromoPOSMInClient;
                //promo.PlanPromoCostProduction = promo.PlanPromoCostProdXSites + promo.PlanPromoCostProdCatalogue + promo.PlanPromoCostProdPOSMInClient;
                promo.PlanPromoCost = (promo.PlanPromoTIShopper ?? 0) + (promo.PlanPromoTIMarketing ?? 0) + (promo.PlanPromoBranding ?? 0) + (promo.PlanPromoBTL ?? 0) + (promo.PlanPromoCostProduction ?? 0);

                string message = null;
                bool error;

                IQueryable<TradeInvestment> TIQuery = context.Set<TradeInvestment>().Where(x => !x.Disabled);
                SimplePromoTradeInvestment simplePromoTradeInvestment = new SimplePromoTradeInvestment(promo);
                double? TIBasePercent = PromoUtils.GetTIBasePercent(simplePromoTradeInvestment, context, TIQuery, out message, out error);
                promo.PlanTIBasePercent = TIBasePercent;
                if (message == null)
                {
                    promo.PlanPromoIncrementalBaseTI = promo.PlanPromoIncrementalLSV * TIBasePercent / 100;

                    IQueryable<COGS> cogsQuery = context.Set<COGS>().Where(x => !x.Disabled);
                    SimplePromoCOGS simplePromoCOGS = new SimplePromoCOGS(promo);
                    double? COGSPercent = PromoUtils.GetCOGSPercent(simplePromoCOGS, context, cogsQuery, out message);
                    promo.PlanCOGSPercent = COGSPercent;
                    if (message == null)
                    {
                        promo.PlanPromoIncrementalCOGS = promo.PlanPromoIncrementalLSV * COGSPercent / 100;

                        promo.PlanPromoBaseTI = promo.PlanPromoLSV * TIBasePercent / 100;

                        // если стоит флаг inout, PlanPromoPostPromoEffect = 0
                        if (!promo.InOut.HasValue || !promo.InOut.Value)
                        {
                            promo.PlanPromoTotalCost = (promo.PlanPromoCost ?? 0) + (promo.PlanPromoBaseTI ?? 0);

                            if (clientTree != null)
                            {
                                //TODO: Уточнить насчет деления на 100
                                promo.PlanPromoPostPromoEffectLSVW1 = promo.PlanPromoBaselineLSV * clientTree.PostPromoEffectW1 / 100;
                                promo.PlanPromoPostPromoEffectLSVW2 = promo.PlanPromoBaselineLSV * clientTree.PostPromoEffectW2 / 100;
                                promo.PlanPromoPostPromoEffectLSV = promo.PlanPromoPostPromoEffectLSVW1 + promo.PlanPromoPostPromoEffectLSVW2;
                            }

                            promo.PlanPromoNetIncrementalLSV = (promo.PlanPromoIncrementalLSV ?? 0) + (promo.PlanPromoPostPromoEffectLSV ?? 0);
                        }
                        else
                        {
                            promo.PlanPromoTotalCost = (promo.PlanPromoCost ?? 0) + (promo.PlanPromoBaseTI ?? 0); // (promo.PlanPromoCost ?? 0) + (promo.PlanPromoIncrementalBaseTI ?? 0) + (promo.PlanPromoIncrementalCOGS ?? 0);

                            promo.PlanPromoPostPromoEffectLSVW1 = 0;
                            promo.PlanPromoPostPromoEffectLSVW2 = 0;
                            promo.PlanPromoPostPromoEffectLSV = 0;

                            promo.PlanPromoNetIncrementalLSV = (promo.PlanPromoIncrementalLSV ?? 0) + (promo.PlanPromoPostPromoEffectLSV ?? 0);
                        }

                        promo.PlanPromoNetLSV = (promo.PlanPromoBaselineLSV ?? 0) + (promo.PlanPromoNetIncrementalLSV ?? 0);
                        promo.PlanPromoNetIncrementalBaseTI = promo.PlanPromoNetIncrementalLSV * TIBasePercent / 100;
                        promo.PlanPromoNetIncrementalCOGS = promo.PlanPromoNetIncrementalLSV * COGSPercent / 100;

                        if (!promo.InOut.HasValue || !promo.InOut.Value)
                        {
                            promo.PlanPromoNetBaseTI = promo.PlanPromoNetLSV * TIBasePercent / 100;
                            promo.PlanPromoBaselineBaseTI = promo.PlanPromoBaselineLSV * TIBasePercent / 100;
                            promo.PlanPromoNSV = (promo.PlanPromoLSV ?? 0) - (promo.PlanPromoTIShopper ?? 0) - (promo.PlanPromoTIMarketing ?? 0) - (promo.PlanPromoBaseTI ?? 0);
                            promo.PlanPromoIncrementalNSV = (promo.PlanPromoIncrementalLSV ?? 0) - (promo.PlanPromoTIShopper ?? 0) - (promo.PlanPromoTIMarketing ?? 0) - (promo.PlanPromoIncrementalBaseTI ?? 0);
                            promo.PlanPromoNetIncrementalNSV = (promo.PlanPromoNetIncrementalLSV ?? 0) - (promo.PlanPromoTIShopper ?? 0) - (promo.PlanPromoTIMarketing ?? 0) - (promo.PlanPromoNetIncrementalBaseTI ?? 0);
                            promo.PlanPromoNetIncrementalMAC = (promo.PlanPromoNetIncrementalNSV ?? 0) - (promo.PlanPromoNetIncrementalCOGS ?? 0);
                        }
                        else
                        {
                            promo.PlanPromoNetBaseTI = 0;
                            promo.PlanPromoBaselineBaseTI = 0;
                            promo.PlanPromoNSV = (promo.PlanPromoLSV ?? 0) - (promo.PlanPromoTIShopper ?? 0) - (promo.PlanPromoTIMarketing ?? 0) - (promo.PlanPromoBaseTI ?? 0);
                            promo.PlanPromoIncrementalNSV = (promo.PlanPromoIncrementalLSV ?? 0) - (promo.PlanPromoTIShopper ?? 0) - (promo.PlanPromoTIMarketing ?? 0) - (promo.PlanPromoIncrementalBaseTI ?? 0);
                            promo.PlanPromoNetIncrementalNSV = (promo.PlanPromoNetIncrementalLSV ?? 0) - (promo.PlanPromoTIShopper ?? 0) - (promo.PlanPromoTIMarketing ?? 0) - (promo.PlanPromoNetIncrementalBaseTI ?? 0);
                            promo.PlanPromoNetIncrementalMAC = (promo.PlanPromoNetIncrementalNSV ?? 0) - (promo.PlanPromoNetIncrementalCOGS ?? 0);
                        }

                        promo.PlanPromoNetNSV = (promo.PlanPromoNetLSV ?? 0) - (promo.PlanPromoTIShopper ?? 0) - (promo.PlanPromoTIMarketing ?? 0) - (promo.PlanPromoNetBaseTI ?? 0);
                        promo.PlanPromoIncrementalMAC = (promo.PlanPromoIncrementalNSV ?? 0) - (promo.PlanPromoIncrementalCOGS ?? 0);
                        promo.PlanPromoIncrementalEarnings = (promo.PlanPromoIncrementalMAC ?? 0) - (promo.PlanPromoBranding ?? 0) - (promo.PlanPromoBTL ?? 0) - (promo.PlanPromoCostProduction ?? 0);
                        promo.PlanPromoNetIncrementalEarnings = (promo.PlanPromoNetIncrementalMAC ?? 0) - (promo.PlanPromoBranding ?? 0) - (promo.PlanPromoBTL ?? 0) - (promo.PlanPromoCostProduction ?? 0);

                        promo.PlanPromoROIPercent = promo.PlanPromoCost != 0 ? (promo.PlanPromoIncrementalEarnings / promo.PlanPromoCost + 1) * 100 : 0;
                        promo.PlanPromoNetROIPercent = promo.PlanPromoCost != 0 ? (promo.PlanPromoNetIncrementalEarnings / promo.PlanPromoCost + 1) * 100 : 0;
                        
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
            catch(Exception e)
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
            //promo.PlanPromoIncrementalLSV = null;
            //promo.PlanPromoLSV = null;
            promo.PlanPromoTIShopper = promo.PlanPromoTIShopper != 0 ? null : promo.PlanPromoTIShopper;
            promo.PlanPromoCost = promo.PlanPromoCost != 0 ? null : promo.PlanPromoCost;
            promo.PlanPromoIncrementalBaseTI = promo.PlanPromoIncrementalBaseTI != 0 ? null : promo.PlanPromoIncrementalBaseTI;
            promo.PlanPromoNetIncrementalBaseTI = promo.PlanPromoNetIncrementalBaseTI != 0 ? null : promo.PlanPromoNetIncrementalBaseTI;
            promo.PlanPromoIncrementalCOGS = promo.PlanPromoIncrementalCOGS != 0 ? null : promo.PlanPromoIncrementalCOGS;
            promo.PlanPromoNetIncrementalCOGS = promo.PlanPromoNetIncrementalCOGS != 0 ? null : promo.PlanPromoNetIncrementalCOGS;
            promo.PlanPromoTotalCost = promo.PlanPromoTotalCost != 0 ? null : promo.PlanPromoTotalCost;
            promo.PlanPromoPostPromoEffectLSVW1 = promo.PlanPromoPostPromoEffectLSVW1 != 0 ? null : promo.PlanPromoPostPromoEffectLSVW1;
            promo.PlanPromoPostPromoEffectLSVW2 = promo.PlanPromoPostPromoEffectLSVW2 != 0 ? null : promo.PlanPromoPostPromoEffectLSVW2;
            promo.PlanPromoPostPromoEffectLSV = promo.PlanPromoPostPromoEffectLSV != 0 ? null : promo.PlanPromoPostPromoEffectLSV;
            promo.PlanPromoNetIncrementalLSV = promo.PlanPromoNetIncrementalLSV != 0 ? null : promo.PlanPromoNetIncrementalLSV;
            promo.PlanPromoNetLSV = promo.PlanPromoNetLSV != 0 ? null : promo.PlanPromoNetLSV;
            promo.PlanPromoBaselineBaseTI = promo.PlanPromoBaselineBaseTI != 0 ? null : promo.PlanPromoBaselineBaseTI;
            promo.PlanPromoBaseTI = promo.PlanPromoBaseTI != 0 ? null : promo.PlanPromoBaseTI;
            promo.PlanPromoNetBaseTI = promo.PlanPromoNetBaseTI != 0 ? null : promo.PlanPromoNetBaseTI;
            promo.PlanPromoNSV = promo.PlanPromoNSV != 0 ? null : promo.PlanPromoNSV;
            promo.PlanPromoNetNSV = promo.PlanPromoNetNSV  != 0 ? null : promo.PlanPromoNetNSV;
            promo.PlanPromoIncrementalNSV = promo.PlanPromoIncrementalNSV != 0 ? null : promo.PlanPromoIncrementalNSV;
            promo.PlanPromoNetIncrementalNSV = promo.PlanPromoNetIncrementalNSV  != 0 ? null : promo.PlanPromoNetIncrementalNSV;
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