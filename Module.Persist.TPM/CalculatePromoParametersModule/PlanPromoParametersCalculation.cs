using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module.Persist.TPM.Model.TPM;
using Persist;
using Module.Persist.TPM.Utils.Filter;
using System.Data.Entity;

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
                ResetValues(promo, context);
                double? sumPlanProductBaseLineLSV = context.Set<PromoProduct>().Where(x => x.PromoId == promoId && !x.Disabled).Sum(x => x.PlanProductBaselineLSV);
                ClientTree clientTree = context.Set<ClientTree>().Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue).FirstOrDefault();

                promo.PlanPromoBaselineLSV = sumPlanProductBaseLineLSV;
                promo.PlanPromoIncrementalLSV = sumPlanProductBaseLineLSV * promo.PlanPromoUpliftPercent / 100;
                promo.PlanPromoLSV = promo.PlanPromoBaselineLSV + promo.PlanPromoIncrementalLSV;  
                promo.PlanPromoTIShopper = promo.PlanPromoLSV * promo.MarsMechanicDiscount / 100;
                // бюджеты пересчитывать не требуется (пусть пока будет закомментировано)
                //promo.PlanPromoTIMarketing = promo.PlanPromoXSites + promo.PlanPromoCatalogue + promo.PlanPromoPOSMInClient;
                //promo.PlanPromoCostProduction = promo.PlanPromoCostProdXSites + promo.PlanPromoCostProdCatalogue + promo.PlanPromoCostProdPOSMInClient;
                promo.PlanPromoCost = promo.PlanPromoTIShopper + promo.PlanPromoTIMarketing + promo.PlanPromoBranding + promo.PlanPromoBTL + promo.PlanPromoCostProduction;

                string message = null;
                double? TIBasePercent = GetTIBasePercent(promo, context, out message);
                if (message == null)
                {
                    promo.PlanPromoIncrementalBaseTI = promo.PlanPromoIncrementalLSV * TIBasePercent / 100;
                    double? COGSPercent = GetCOGSPercent(promo, context, out message);
                    if (message == null)
                    {
                        promo.PlanPromoIncrementalCOGS = promo.PlanPromoIncrementalLSV * COGSPercent / 100;
                        promo.PlanPromoTotalCost = promo.PlanPromoCost + promo.PlanPromoIncrementalBaseTI + promo.PlanPromoIncrementalCOGS;
                        promo.PlanPostPromoEffectW1 = promo.PlanPromoTotalCost * clientTree.PostPromoEffectW1 / 100;
                        promo.PlanPostPromoEffectW2 = promo.PlanPromoTotalCost * clientTree.PostPromoEffectW2 / 100;
                        promo.PlanPostPromoEffect = promo.PlanPostPromoEffectW1 + promo.PlanPostPromoEffectW2;
                        promo.PlanPromoNetIncrementalLSV = promo.PlanPromoIncrementalLSV - promo.PlanPostPromoEffect;
                        promo.PlanPromoNetLSV = promo.PlanPromoBaselineLSV + promo.PlanPromoNetIncrementalLSV;
                        promo.PlanPromoBaselineBaseTI = promo.PlanPromoBaselineLSV * TIBasePercent / 100;
                        promo.PlanPromoBaseTI = promo.PlanPromoLSV * TIBasePercent / 100;
                        promo.PlanPromoNetNSV = promo.PlanPromoNetNSV - promo.PlanPromoTIShopper - promo.PlanPromoTIMarketing - promo.PlanPromoBaseTI;
                        promo.PlanPromoIncrementalNSV = promo.PlanPromoNetNSV - promo.PlanPromoTIShopper - promo.PlanPromoTIMarketing - promo.PlanPromoIncrementalBaseTI;
                        promo.PlanPromoNetIncrementalNSV = promo.PlanPromoNetIncrementalNSV - promo.PlanPromoTIShopper - promo.PlanPromoTIMarketing - promo.PlanPromoIncrementalBaseTI;
                        promo.PlanPromoIncrementalMAC = promo.PlanPromoIncrementalNSV - promo.PlanPromoIncrementalCOGS;
                        promo.PlanPromoNetIncrementalMAC = promo.PlanPromoNetIncrementalNSV - promo.PlanPromoIncrementalCOGS;
                        promo.PlanPromoIncrementalEarnings = promo.PlanPromoIncrementalMAC - promo.PlanPromoBranding - promo.PlanPromoBTL - promo.PlanPromoCostProduction;
                        promo.PlanPromoNetIncrementalEarnings = promo.PlanPromoNetIncrementalMAC - promo.PlanPromoBranding - promo.PlanPromoBTL - promo.PlanPromoCostProduction;
                        promo.PlanPromoROIPercent = (int?)(promo.PlanPromoIncrementalEarnings / promo.PlanPromoTotalCost) * 100;
                        promo.PlanPromoNetROIPercent = (int?)(promo.PlanPromoNetIncrementalEarnings / promo.PlanPromoTotalCost) * 100;
                        promo.PlanPromoNetUpliftPercent = (int?)(promo.PlanPromoNetIncrementalLSV / promo.PlanPromoBaselineLSV) * 100;
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

        public static double? GetTIBasePercent(Promo promo, DatabaseContext context, out string message)
        {
            try
            {
                ClientTree clientNode = context.Set<ClientTree>().Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue).FirstOrDefault();
                List<TradeInvestment> tiList = context.Set<TradeInvestment>().Where(x => x.ClientTreeId == clientNode.Id && x.BrandTechId == promo.BrandTechId && !x.Disabled).ToList();
                BrandTech brandTech = context.Set<BrandTech>().FirstOrDefault(n => n.Id == promo.BrandTechId);

                while(tiList.Count == 0 && clientNode.Type != "root")
                {
                    clientNode = context.Set<ClientTree>().Where(x => x.ObjectId == clientNode.parentId && !x.EndDate.HasValue).FirstOrDefault();
                    tiList = context.Set<TradeInvestment>().Where(x => x.ClientTreeId == clientNode.Id && !x.Disabled).ToList();
                }

                if(tiList.Count == 0)
                {
                    message = GetMessageTiCogs("TI base was not found", promo, true, context);
                    return null;
                }
                else
                {
                    tiList = tiList.Where(x => x.StartDate.HasValue && promo.StartDate.HasValue && DateTimeOffset.Compare(x.StartDate.Value, promo.StartDate.Value) < 0).ToList();
                    if (tiList.Count > 1)
                    {
                        message = GetMessageTiCogs("TI base duplicate record error", promo, true, context);
                        return null;
                    }
                    else if (tiList.Count != 0)
                    {
                        message = null;
                        return tiList[0].SizePercent;
                    }
                    else
                    {
                        message = GetMessageTiCogs("TI base was not found", promo, true, context);
                        return null;
                    }
                }
            }
            catch(Exception e)
            {
                message = e.ToString();
                return null;
            }
        }

        public static double? GetCOGSPercent(Promo promo, DatabaseContext context, out string message)
        {
            try
            {
                ClientTree clientNode = context.Set<ClientTree>().Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue).FirstOrDefault();
                List<COGS> cogsList = context.Set<COGS>().Where(x => x.ClientTreeId == clientNode.Id && x.BrandTechId == promo.BrandTechId && !x.Disabled).ToList();
                

                while (cogsList.Count == 0 && clientNode.Type != "root")
                {
                    clientNode = context.Set<ClientTree>().Where(x => x.ObjectId == clientNode.parentId && !x.EndDate.HasValue).FirstOrDefault();
                    cogsList = context.Set<COGS>().Where(x => x.ClientTreeId == clientNode.Id && !x.Disabled).ToList();
                }

                if (cogsList.Count == 0)
                {
                    message = GetMessageTiCogs("COGS was not found", promo, false, context);
                    return null;
                }
                else
                {
                    cogsList = cogsList.Where(x => x.StartDate.HasValue && promo.StartDate.HasValue && DateTimeOffset.Compare(x.StartDate.Value, promo.DispatchesStart.Value) < 0).ToList();
                    if (cogsList.Count > 1)
                    {                        
                        message = GetMessageTiCogs("COGS duplicate record error", promo, false, context);
                        return null;
                    }
                    else if (cogsList.Count != 0)
                    {
                        message = null;
                        return cogsList[0].LVSpercent;
                    }
                    else
                    {
                        message = GetMessageTiCogs("COGS was not found", promo, false, context);
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                message = e.ToString();
                return null;
            }
        }

        /// <summary>
        /// Сформировать сообщения об ошибке подбора для TI или COGS
        /// </summary>
        /// <param name="baseMessage">Базовое сообщение</param>
        /// <param name="promo">Промо</param>
        /// <param name="ti">True если TI, False если COGS</param>
        /// <param name="context">Контекст БД</param>
        /// <returns></returns>
        private static string GetMessageTiCogs(string baseMessage, Promo promo, bool ti, DatabaseContext context)
        {
            BrandTech brandTech = context.Set<BrandTech>().FirstOrDefault(n => n.Id == promo.BrandTechId);            

            string result = baseMessage + " for client " + promo.ClientHierarchy;

            if (brandTech != null)
                result += " and BrandTech " + brandTech.Name;

            if (ti)
                result += " for the period from " + promo.StartDate.Value.ToString("dd.MM.yyyy") + " to " + promo.EndDate.Value.ToString("dd.MM.yyyy") + ".";
            else
                result += " for the period from " + promo.DispatchesStart.Value.ToString("dd.MM.yyyy") + " to " + promo.DispatchesEnd.Value.ToString("dd.MM.yyyy") + ".";


            return result;
        }

        /// <summary>
        /// Сбросить значения плановых полей
        /// </summary>
        /// <param name="promo">Промо</param>
        /// <param name="context">Контекст БД</param>
        private static void ResetValues(Promo promo, DatabaseContext context)
        {
            promo.PlanPromoBaselineLSV = null;
            promo.PlanPromoIncrementalLSV = null;
            promo.PlanPromoLSV = null;
            promo.PlanPromoTIShopper = null;
            promo.PlanPromoCost = null;
            promo.PlanPromoIncrementalBaseTI = null;
            promo.PlanPromoIncrementalCOGS = null;
            promo.PlanPromoTotalCost = null;
            promo.PlanPostPromoEffectW1 = null;
            promo.PlanPostPromoEffectW2 = null;
            promo.PlanPostPromoEffect = null;
            promo.PlanPromoNetIncrementalLSV = null;
            promo.PlanPromoNetLSV = null;
            promo.PlanPromoBaselineBaseTI = null;
            promo.PlanPromoBaseTI = null;
            promo.PlanPromoNetNSV = null;
            promo.PlanPromoIncrementalNSV = null;
            promo.PlanPromoNetIncrementalNSV = null;
            promo.PlanPromoIncrementalMAC = null;
            promo.PlanPromoNetIncrementalMAC = null;
            promo.PlanPromoIncrementalEarnings = null;
            promo.PlanPromoNetIncrementalEarnings = null;
            promo.PlanPromoROIPercent = null;
            promo.PlanPromoNetROIPercent = null;
            promo.PlanPromoNetUpliftPercent = null;
            context.SaveChanges();
        }
    }
}