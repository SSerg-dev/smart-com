using Core.Dependency;
using Core.Settings;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.CalculatePromoParametersModule
{
    public class PlanPromoUpliftCalculation
    {
        public static double? FindPlanPromoUplift(Guid promoId, DatabaseContext context, out string upliftMessage, bool needResetUpliftCorrections, Guid UserId)
        {
            try
            {
                var currentPromo = context.Set<Promo>().Where(x => x.Id == promoId).FirstOrDefault();
                // сбрасываем uplift
                currentPromo.PlanPromoUpliftPercent = null;
               
                //Получаем все продукты по этому промо
                List<PromoProduct> currentPromoProducts = context.Set<PromoProduct>().Where(x => x.PromoId == currentPromo.Id && x.Disabled != true).ToList();
                foreach(var promoProduct in currentPromoProducts)
                {
                    promoProduct.PlanProductUpliftPercent = null;
                }
                context.SaveChanges();


                upliftMessage = "";

                if (needResetUpliftCorrections)
                {
                    User user = context.Set<User>().Where(x => x.Id == UserId && x.Disabled != true).FirstOrDefault();
                    List<PromoProductsCorrection> promoProductsCorrectionsToDelete = context.Set<PromoProductsCorrection>().Where(x => x.PromoProduct.PromoId == promoId && x.Disabled != true).ToList();
                    if(promoProductsCorrectionsToDelete.Count > 0)
                    {
                        foreach (PromoProductsCorrection promoProductsCorrection in promoProductsCorrectionsToDelete)
                        {
                            promoProductsCorrection.Disabled = true;
                            promoProductsCorrection.DeletedDate = DateTimeOffset.UtcNow;
                            promoProductsCorrection.ChangeDate = DateTimeOffset.UtcNow;
                            promoProductsCorrection.UserId = user.Id;
                            promoProductsCorrection.UserName = user.Name;
                        }
                    }
                    context.SaveChanges();
                }

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
                            bool success = CalculatePlanPromoUpliftPersent(ref promoSubrangeList, currentPromo, out countedPlanUplift, context, currentPromoProducts);
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
                                double? planPromoUpliftPercent = SingleSubrangeFinding(promoId, promoQuery, context, promoProductTreeArray, currentPromo, out message, currentPromoProducts);
                                upliftMessage = message;
                                return planPromoUpliftPercent;
                            }
                        }
                        // если не найдено исторических промо с таким же набором сабренжей, то производится поиск исторических промо отдельно по каждому сабренжу текущего промо
                        else
                        {
                            string message;
                            double? planPromoUpliftPercent = SingleSubrangeFinding(promoId, promoQuery, context, promoProductTreeArray, currentPromo, out message, currentPromoProducts);
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

        private static double? SingleSubrangeFinding(Guid promoId, IQueryable<Promo> promoQuery, DatabaseContext context, IQueryable<PromoProductTree> promoProductTreeArray, Promo currentPromo, out string message, List<PromoProduct> currentPromoProducts)
        {
            double? countedPlanUplift = 0;
            bool singleSubrangeSuccess = true;
            List<Promo> promoSingleSubrangeList = null;
            List<Promo> promoMultipleSubrangeList = new List<Promo>();
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
                    if (promoSingleSubrangeList.Count > 0)
                    {
                        promoMultipleSubrangeList.AddRange(promoSingleSubrangeList);
                    }
                }
            }
            if (promoMultipleSubrangeList != null && promoMultipleSubrangeList.Count() != 0 && singleSubrangeSuccess)
            {
                double? singleSubrangeCountedPlanUplift = 0;
                bool success = CalculatePlanPromoUpliftPersent(ref promoMultipleSubrangeList, currentPromo, out singleSubrangeCountedPlanUplift, context, currentPromoProducts);
                if (success)
                {
                    countedPlanUplift += singleSubrangeCountedPlanUplift;
                    usedPromoes.AddRange(promoMultipleSubrangeList);  // так надёжнее
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

            if (singleSubrangeSuccess)
            {
                currentPromo.PlanPromoUpliftPercent = countedPlanUplift;
                context.SaveChanges();

                message = GetMessagePromoList(usedPromoes);
                //message = "Рассчитанное значение uplift";
                return countedPlanUplift;
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
                bool success = CalculatePlanPromoUpliftPersent(ref promoQueryList, currentPromo, out brandTechCountedPlanUplift, context, currentPromoProducts);
                if (success)
                {
                    currentPromo.PlanPromoUpliftPercent = brandTechCountedPlanUplift;
                    context.SaveChanges();

                    message = GetMessagePromoList(promoQueryList);
                    //message = "Рассчитанное значение uplift";

                    return brandTechCountedPlanUplift;
                }
                //Если не нашли промо, но есть коррекции
                else if (brandTechCountedPlanUplift != 0)
                {
                    currentPromo.PlanPromoUpliftPercent = brandTechCountedPlanUplift;
                    context.SaveChanges();

                    message = "Не найдено Promo для расчета Uplift. Uplift рассчитан по корректировкам";
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
			// Закрываем неактуальные инциденты
			var oldIncidents = context.Set<PromoUpliftFailIncident>().Where(x => x.PromoId == promoId && x.ProcessDate == null);
			foreach (var incident in oldIncidents)
			{
				incident.ProcessDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
			}

			context.Set<PromoUpliftFailIncident>().Add(new PromoUpliftFailIncident() { PromoId = promoId, CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow) });
            context.SaveChanges();
        }

        private static bool CalculatePlanPromoUpliftPersent(ref List<Promo> promoList, Promo currentPromo, out double? countedPlanUplift, DatabaseContext context, List<PromoProduct> currentPromoProducts)
        {
            countedPlanUplift = 0;

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
                                CalculateRealPlanPromoUpliftPercent(ref promoList, out countedPlanUplift, currentPromoProducts, context);
                                return true;
                            }
                        }
                    }
                }
            }
            countedPlanUplift = CalculateOnlyByCorrections(currentPromo, context, currentPromoProducts);
            return false;
        }

        private static double? CalculateOnlyByCorrections(Promo currentPromo, DatabaseContext context, List<PromoProduct> currentPromoProducts)
        {
            double? countedPlanUplift = 0;
            var promoProductsIds = currentPromoProducts.Select(y => y.Id);
            List<PromoProductsCorrection> promoProductsCorrections = context.Set<PromoProductsCorrection>().Where(x => promoProductsIds.Contains(x.PromoProductId) && x.TempId == null && x.Disabled != true).ToList();
            if (promoProductsCorrections.Count > 0)
            {
                double? summPlanIncremental = 0;
                double? summPlanBaseline = 0;

                foreach (PromoProductsCorrection promoProductsCorrection in promoProductsCorrections)
                {
                    summPlanIncremental += promoProductsCorrection.PromoProduct.PlanProductBaselineLSV * promoProductsCorrection.PlanProductUpliftPercentCorrected / 100;
                }
                summPlanBaseline += currentPromoProducts.Sum(x => x.PlanProductBaselineLSV);

                if (summPlanBaseline != 0)
                {
                    countedPlanUplift = summPlanIncremental / summPlanBaseline * 100;
                }
            }
            return countedPlanUplift;
        }

        public static void CalculateRealPlanPromoUpliftPercent(ref List<Promo> promoList, out double? countedPlanUplift, List<PromoProduct> currentPromoProducts, DatabaseContext context)
        {
            countedPlanUplift = 0;
            List<PromoProduct> promoProductsList = new List<PromoProduct>();
            List<PromoProduct> promoProductsSubList = new List<PromoProduct>();
            double? factUpliftSum = 0;
            double? factProductUplift = 0;
            double? summPlanIncremental = 0;
            double? summPlanBaseline = 0;
            double? oldCountedPlanUplift;

            foreach (Promo promo in promoList)
            {
                promoProductsList.AddRange(context.Set<PromoProduct>().Where(x => x.PromoId == promo.Id && x.Disabled != true).ToList());
                if (promo.ActualPromoUpliftPercent != null)
                {
                    factUpliftSum += promo.ActualPromoUpliftPercent;
                }
            }
            oldCountedPlanUplift = factUpliftSum / promoList.Count();
            var promoProductsIds = currentPromoProducts.Select(y => y.Id);
            List<PromoProductsCorrection> promoProductsCorrections = context.Set<PromoProductsCorrection>().Where(x => promoProductsIds.Contains(x.PromoProductId) && x.TempId == null && x.Disabled != true).ToList();
            var promoProductsWithCorrectionIds = promoProductsCorrections.Select(y => y.PromoProductId);
            //Подбираем аплифт для каждого отдельного продукта
            foreach (PromoProduct promoProduct in currentPromoProducts)
            {
                promoProductsSubList = promoProductsList.Where(x => x.ZREP == promoProduct.ZREP).ToList();
                if (promoProductsSubList.Count() > 0)
                {
                    factProductUplift = promoProductsSubList.Average(x => x.ActualProductUpliftPercent);
                    promoProduct.AverageMarker = false;
                }
                //Если не подобрали аплифт по предыдущим промо - считаем средний аплифт для продукта
                if(!(promoProductsSubList.Count() > 0) || factProductUplift == null)
                {
                    factProductUplift = oldCountedPlanUplift;
                    promoProduct.AverageMarker = true;
                }
                promoProduct.PlanProductUpliftPercent = factProductUplift;
                summPlanBaseline += promoProduct.PlanProductBaselineLSV;
                //Если есть коррекция - считаем аплифт из коррекции
                if (promoProductsWithCorrectionIds.Contains(promoProduct.Id))
                {
                    var uplift = promoProductsCorrections.Where(x => x.PromoProductId == promoProduct.Id).FirstOrDefault().PlanProductUpliftPercentCorrected / 100;
                    summPlanIncremental += promoProduct.PlanProductBaselineLSV * uplift;
                }
                else
                {
                    summPlanIncremental += promoProduct.PlanProductBaselineLSV * factProductUplift / 100;
                }
            }
            if (summPlanBaseline != 0)
            {
                countedPlanUplift = summPlanIncremental / summPlanBaseline * 100;
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

        public static void DistributePlanPromoUpliftToProducts(Promo currentPromo, DatabaseContext context, Guid UserId)
        {
            double? newUplift = currentPromo.PlanPromoUpliftPercent;
            if (newUplift != null)
            {
                User user = context.Set<User>().Where(x => x.Id == UserId && x.Disabled != true).FirstOrDefault();
                List<PromoProduct> promoProducts = context.Set<PromoProduct>().Where(x => x.PromoId == currentPromo.Id && x.Disabled != true).ToList();
                List<double?> incrementalList = promoProducts.Select(x => x.PlanProductIncrementalLSV).ToList();
                if (promoProducts != null && promoProducts.Count != 0)
                {
                    double? summPlanBaseline = 0;
                    double? summPlanIncremental = 0;
                    double? upliftToInsert;
                    var promoProductsIds = promoProducts.Select(y => y.Id);
                    List <PromoProductsCorrection> promoProductsCorrections = context.Set<PromoProductsCorrection>().Where(x => promoProductsIds.Contains(x.PromoProductId) && x.Disabled != true && x.TempId == null).ToList();

                    foreach (var singlePromoProduct in promoProducts)
                    {
                        if (singlePromoProduct.PlanProductIncrementalLSV != null && singlePromoProduct.PlanProductIncrementalLSV != 0
                            && singlePromoProduct.PlanProductBaselineLSV != null && singlePromoProduct.PlanProductBaselineLSV != 0)
                        {
                            summPlanIncremental += singlePromoProduct.PlanProductIncrementalLSV;
                            summPlanBaseline += singlePromoProduct.PlanProductBaselineLSV;
                        }

                        if (!promoProductsCorrections.Select(x => x.PromoProductId).Contains(singlePromoProduct.Id))
                        {
                            if (singlePromoProduct.PlanProductBaselineLSV != 0 && singlePromoProduct.PlanProductBaselineLSV != null && singlePromoProduct.PlanProductUpliftPercent != null)
                            {
                                upliftToInsert = singlePromoProduct.PlanProductUpliftPercent;
                            }
                            else
                            {
                                upliftToInsert = newUplift;
                            }
                            promoProductsCorrections.Add(context.Set<PromoProductsCorrection>().Add(new PromoProductsCorrection()
                            {
                                PromoProductId = singlePromoProduct.Id,
                                PlanProductUpliftPercentCorrected = upliftToInsert,
                                CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                                Id = new Guid()
                            }));
                        }
                    }

                    promoProductsCorrections = promoProductsCorrections.Where(x => x.PlanProductUpliftPercentCorrected != null).ToList();

                    foreach (var singlePromoProductCorrection in promoProductsCorrections)
                    {
                        singlePromoProductCorrection.UserId = user.Id;
                        singlePromoProductCorrection.UserName = user.Name;
                        singlePromoProductCorrection.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                    }

                    double? oldUplift = 0;
                    if (summPlanBaseline != 0)
                    {
                        oldUplift = summPlanIncremental / summPlanBaseline;
                    }

                    //Находим соотношение старого Uplift к новому
                    double? upliftRatio = 0;
                    if (oldUplift != 0)
                    {
                        upliftRatio = newUplift / oldUplift;
                    }
                    foreach (var singlePromoProductCorrection in promoProductsCorrections)
                    {
                        if (!(singlePromoProductCorrection.PromoProduct.PlanProductBaselineLSV == null || singlePromoProductCorrection.PromoProduct.PlanProductBaselineLSV == 0) 
                            && !(singlePromoProductCorrection.PromoProduct.PlanProductIncrementalLSV == null || singlePromoProductCorrection.PromoProduct.PlanProductIncrementalLSV == 0)) { 
                            singlePromoProductCorrection.PlanProductUpliftPercentCorrected = (singlePromoProductCorrection.PromoProduct.PlanProductIncrementalLSV * upliftRatio) / singlePromoProductCorrection.PromoProduct.PlanProductBaselineLSV;
                        } else
                        {
                            singlePromoProductCorrection.PlanProductUpliftPercentCorrected = newUplift;
                        }
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
