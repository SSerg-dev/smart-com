using Core.Dependency;
using Core.Settings;
using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.CalculatePromoParametersModule
{
    public class PlanPromoUpliftCalculation
    {
        public static double? FindPlanPromoUplift(Guid promoId, DatabaseContext context, out string upliftMessage)
        {
            try
            {
                var currentPromo = context.Set<Promo>().Where(x => x.Id == promoId).FirstOrDefault();
                // сбрасываем uplift
                currentPromo.PlanPromoUpliftPercent = null;
                context.SaveChanges();

                upliftMessage = "";

                //временно убрана проверка на наличие бюджетов
                // TODO: решить, что с этим делать!
                /*
                    //экстра-места - физические ДМП(X-sites) - проверка на наличие X-sites
                    //экстра-места - каталоги(Catalog) - проверка на наличие Catalog
                    var promoXsites = context.Set<Promo>().Where(x => x.ActualPromoXSites != null && !x.Disabled);
                    var promoCatalog = context.Set<Promo>().Where(x => x.ActualPromoCatalogue != null && !x.Disabled);

                    var promoQuery = promoXsites.Intersect(promoCatalog);

                    // Исключить промо с признаком InOut из подбора uplift.
                    promoQuery = promoQuery.Where(x => x.InOut != true);
                */

                var promoQuery = context.Set<Promo>().Where(x => x.InOut != true);
                if (promoQuery.Count() != 0)
                {
                    //выбираем закрытые промо (дата окончания в пределах N лет до текущей даты)
                    ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                    int closedPromoPeriod = settingsManager.GetSetting<int>("CLOSED_PROMO_PERIOD_YEARS", 1);
                    var now = DateTimeOffset.Now;
                    var someYearsLaterDate = new DateTimeOffset(now.Year - closedPromoPeriod, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Offset);
                    var closedPromoStatusId = context.Set<PromoStatus>().Where(x => x.SystemName == "Closed").FirstOrDefault().Id;
                    promoQuery = promoQuery.Where(x => x.PromoStatusId == closedPromoStatusId && x.EndDate.HasValue && x.EndDate >= someYearsLaterDate && !x.Disabled);

                    //проверка на совпадение по клиенту
                    promoQuery = promoQuery.Where(x => x.ClientTreeId == currentPromo.ClientTreeId);

                    if (promoQuery.Count() != 0)
                    {
                        //---получение списка исторических промо с таким же набором Subrange---
                        var promoProductTreeArray = context.Set<PromoProductTree>().Where(p => p.PromoId == currentPromo.Id && !p.Disabled);
                        List<Promo> promoSubrangeList = null;
                        foreach (var promoProductTree in promoProductTreeArray)
                        {
                            //выбор узлов типа Subrange текущего промо
                            ProductTree productTree = context.Set<ProductTree>().Where(x => x.ObjectId == promoProductTree.ProductTreeObjectId && x.Type == "Subrange" && !x.EndDate.HasValue).FirstOrDefault();
                            if (productTree != null)
                            {
                                IEnumerable<Promo> promoSubrage = promoQuery.ToArray().Where(x => context.Set<PromoProductTree>().Where(p => p.ProductTreeObjectId == productTree.ObjectId && !p.Disabled).Any(p => x.Id == p.PromoId));
                                if (promoSubrangeList == null)
                                {
                                    promoSubrangeList = promoSubrage.ToList();
                                }
                                else
                                {
                                    promoSubrangeList = promoSubrangeList.Intersect(promoSubrage).ToList();
                                }
                            }
                        }

                        if (promoSubrangeList != null)
                        {
                            Promo[] _promoSubrangeList = new Promo[promoSubrangeList.Count()];
                            promoSubrangeList.CopyTo(_promoSubrangeList);

                            // для каждого промо, которое оказалось в promoSubrangeList проверяем, совпадает ли количество записей в таблице PromoProductTree с таким же PromoId и количество записей в 
                            // promoProductTreeArray(т.е. число выбранных узлов в текущем промо), таким образом проверяется, соотвествует ли каждое промо в полученном списке promoSubrangeList
                            // полному набору сабренжей текущего промо
                            foreach (var promoSubrange in _promoSubrangeList)
                            {
                                // TODO: проверить работу с for без _promoSubrangeList
                                var query = context.Set<PromoProductTree>().Where(x => x.PromoId == promoSubrange.Id && !x.Disabled);
                                if (query.Count() != promoProductTreeArray.Count())
                                {
                                    promoSubrangeList.Remove(promoSubrange);
                                }
                            }
                        }

                        // если в promoSubrangeList остались промо, то Subrange этих промо полностью соответствуют Subrange текущего промо
                        if (promoSubrangeList != null && promoSubrangeList.Count() != 0)
                        {
                            double? countedPlanUplift = 0;
                            bool success = CalculatePlanPromoUpliftPersent(ref promoSubrangeList, currentPromo, out countedPlanUplift);
                            if (success)
                            {
                                currentPromo.PlanPromoUpliftPercent = countedPlanUplift;
                                context.SaveChanges();

                                upliftMessage = GetMessagePromoList(promoSubrangeList);
                                //upliftMessage = "Рассчитанное значение uplift";
                                return countedPlanUplift;
                            }
                            else
                            {
                                string message;
                                double? planPromoUpliftPercent = SingleSubrangeFinding(promoId, promoQuery, context, promoProductTreeArray, currentPromo, out message);
                                upliftMessage = message;
                                return planPromoUpliftPercent;
                            }
                        }
                        // если не найдено исторических промо с таким же набором сабренжей, то производится поиск исторических промо отдельно по каждому сабренжу текущего промо
                        else
                        {
                            string message;
                            double? planPromoUpliftPercent = SingleSubrangeFinding(promoId, promoQuery, context, promoProductTreeArray, currentPromo, out message);
                            upliftMessage = message;
                            return planPromoUpliftPercent;
                        }
                    }
                    else
                    {
                        WriteUpliftIncident(promoId, context);
                        upliftMessage = "Не найдено Promo для расчета Uplift";
                        return -1;
                    }
                }
                else
                {
                    WriteUpliftIncident(promoId, context);
                    upliftMessage = "Не найдено Promo для расчета Uplift";
                    return -1;
                }
            }
            catch (Exception e)
            {
                upliftMessage = e.ToString();
                return -1;
            }
        }

        private static double? SingleSubrangeFinding(Guid promoId, IQueryable<Promo> promoQuery, DatabaseContext context, IQueryable<PromoProductTree> promoProductTreeArray, Promo currentPromo, out string message)
        {
            double? countedPlanUplift = 0;
            bool singleSubrangeSuccess = true;
            List<Promo> promoSingleSubrangeList = null;
            List<Promo> usedPromoes = new List<Promo>();
            foreach (var promoProductTree in promoProductTreeArray)
            {
                //выбор узлов типа Subrange текущего промо
                var productTree = context.Set<ProductTree>().Where(x => x.ObjectId == promoProductTree.ProductTreeObjectId && x.Type == "Subrange" && !x.EndDate.HasValue).FirstOrDefault();
                if (productTree != null)
                {
                    promoSingleSubrangeList = promoQuery.ToArray().Where(x => context.Set<PromoProductTree>().Where(p => p.ProductTreeObjectId == productTree.ObjectId && !p.Disabled).Any(p => x.Id == p.PromoId)).ToList();

                    Promo[] _promoSingleSubrangeList = new Promo[promoSingleSubrangeList.Count()];
                    promoSingleSubrangeList.CopyTo(_promoSingleSubrangeList);
                    //остаются те промо, у которых есть только проверяемый сабредж
                    foreach (var promoSingleSubrange in _promoSingleSubrangeList)
                    {
                        var query = context.Set<PromoProductTree>().Where(x => x.PromoId == promoSingleSubrange.Id);
                        if (query.Count() > 1)
                        {
                            promoSingleSubrangeList.Remove(promoSingleSubrange);
                        }
                    }
                }

                if (promoSingleSubrangeList != null && promoSingleSubrangeList.Count() != 0 && singleSubrangeSuccess)
                {
                    double? singleSubrangeCountedPlanUplift = 0;
                    bool success = CalculatePlanPromoUpliftPersent(ref promoSingleSubrangeList, currentPromo, out singleSubrangeCountedPlanUplift);
                    if (success)
                    {
                        countedPlanUplift += singleSubrangeCountedPlanUplift;                       
                        usedPromoes.AddRange(promoSingleSubrangeList);  // так надёжнее
                    }
                    else
                    {
                        singleSubrangeSuccess = false;
                    }
                }
                else
                {
                    singleSubrangeSuccess = false;
                }
            }

            if (singleSubrangeSuccess)
            {
                double? resultPlanUplift = countedPlanUplift / promoProductTreeArray.Count();

                currentPromo.PlanPromoUpliftPercent = resultPlanUplift;
                context.SaveChanges();

                message = GetMessagePromoList(usedPromoes);
                //message = "Рассчитанное значение uplift";
                return resultPlanUplift;
            }
            // если не найдено исторических промо по каждому сабренжу отдельно, то осуществляется поиск исторических промо на основе брендтех текущего промо
            else
            {
                Guid? brandId, technologyId;
                GetBrandTechnologyGuid(context, currentPromo, out brandId, out technologyId);
                promoQuery = promoQuery.Where(x => x.BrandId.HasValue && x.BrandId == brandId && x.TechnologyId.HasValue && x.TechnologyId == technologyId);
                List<Promo> promoQueryList = new List<Promo>();

                // по brand и technology должен быть выбран именно узел Technology
                foreach (Promo p in promoQuery)
                {
                    IQueryable<PromoProductTree> productNodes = context.Set<PromoProductTree>().Where(n => n.PromoId == p.Id);
                    
                    if (productNodes.Count() == 1)
                    {
                        int objectId = productNodes.First().ProductTreeObjectId;

                        if (context.Set<ProductTree>().First(n => n.ObjectId == objectId).Type.ToLower().IndexOf("technology") >= 0)
                            promoQueryList.Add(p);
                    }
                }

                double? brandTechCountedPlanUplift = 0;                
                bool success = CalculatePlanPromoUpliftPersent(ref promoQueryList, currentPromo, out brandTechCountedPlanUplift);
                if (success)
                {
                    currentPromo.PlanPromoUpliftPercent = brandTechCountedPlanUplift;
                    context.SaveChanges();

                    message = GetMessagePromoList(promoQueryList);
                    //message = "Рассчитанное значение uplift";

                    return brandTechCountedPlanUplift;
                }
                else
                {
                    WriteUpliftIncident(promoId, context);

                    message = "Не найдено Promo для расчета Uplift";
                    return -1;
                }
            }
        }

        private static void WriteUpliftIncident(Guid promoId, DatabaseContext context)
        {
            context.Set<PromoUpliftFailIncident>().Add(new PromoUpliftFailIncident() { PromoId = promoId, CreateDate = DateTime.Now });
            context.SaveChanges();
        }

        private static bool CalculatePlanPromoUpliftPersent(ref List<Promo> promoList, Promo currentPromo, out double? countedPlanUplift)
        {
            if (promoList.Count() != 0)
            {
                //промо, подходящие по механике
                promoList = promoList.Where(x => x.MarsMechanicId == currentPromo.MarsMechanicId).ToList();

                if (promoList.Count() != 0)
                {
                    //промо, подходящие по скидке
                    double? marsMechanicDiscountLeft = currentPromo.MarsMechanicDiscount - 3 < 0 ? 0 : currentPromo.MarsMechanicDiscount - 3;
                    double? marsMechanicDiscountRight = currentPromo.MarsMechanicDiscount + 3;

                    promoList = promoList.Where(x => (x.MarsMechanicDiscount <= marsMechanicDiscountRight && x.MarsMechanicDiscount >= marsMechanicDiscountLeft)).ToList();

                    if (promoList.Count() != 0)
                    {
                        //промо, подходящие по длительности
                        if (currentPromo.StartDate != null && currentPromo.EndDate != null)
                        {
                            int currentPromoDuration = (currentPromo.EndDate.Value - currentPromo.StartDate.Value).Days;
                            int currentPromoDurationLeft = currentPromoDuration - 3;
                            int currentPromoDurationRight = currentPromoDuration + 3;
                            promoList = promoList.Where(x => x.StartDate.HasValue && x.EndDate.HasValue &&
                                                         ((x.EndDate.Value - x.StartDate.Value).Days <= currentPromoDurationRight && (x.EndDate.Value - x.StartDate.Value).Days >= currentPromoDurationLeft)).ToList();

                            if (promoList.Count() != 0)
                            {
                                double? factUpliftSum = 0;
                                foreach (Promo promo in promoList)
                                {
                                    factUpliftSum += promo.ActualPromoUpliftPercent;
                                }
                                countedPlanUplift = factUpliftSum / promoList.Count();
                                return true;
                            }
                            else
                            {
                                countedPlanUplift = 0;
                                return false;
                            }
                        }
                        else
                        {
                            countedPlanUplift = 0;
                            return false;
                        }
                    }
                    else
                    {
                        countedPlanUplift = 0;
                        return false;
                    }
                }
                else
                {
                    countedPlanUplift = 0;
                    return false;
                }
            }
            else
            {
                countedPlanUplift = 0;
                return false;
            }
        }

        private static void GetBrandTechnologyGuid(DatabaseContext context, Promo promo, out Guid? brandId, out Guid? technologyId)
        {
            brandId = Guid.Empty;
            technologyId = Guid.Empty;
            // можно брать первый, т.к. даже если ProductTree несколько, то они в одной технологии
            int objectId = context.Set<PromoProductTree>().First(n => n.PromoId == promo.Id && !n.Disabled).ProductTreeObjectId;
            var productTree = context.Set<ProductTree>().Where(x => x.ObjectId == objectId && !x.EndDate.HasValue).FirstOrDefault();
            while (productTree.Type != "root")
            {
                if (productTree.Type == "Brand")
                {
                    brandId = productTree.BrandId;
                }
                else if (productTree.Type == "Technology")
                {
                    technologyId = productTree.TechnologyId;
                }

                productTree = context.Set<ProductTree>().Where(x => x.ObjectId == productTree.parentId && !x.EndDate.HasValue).FirstOrDefault();
            }
        }

        private static string GetMessagePromoList(List<Promo> promoList)
        {
            string message = "Для определения Uplift использовались следующие промо: ";
            foreach (Promo p in promoList)
                message += p.Number + " ";

            message += "\nРассчитанное значение uplift";

            return message;
        }
    }
}
