using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module.Persist.TPM.Model.TPM;
using Persist;
using Module.Persist.TPM.Utils.Filter;
using System.Data.Entity;
using Core.Extensions;
using System.Data.Entity.Validation;
using Module.Persist.TPM.Utils;
using Module.Persist.TPM.PromoStateControl;

namespace Module.Persist.TPM.CalculatePromoParametersModule
{
    public class PlanProductParametersCalculation
    {
        public enum BaseLineState
        {
            InitBaseLine,
            NullBaseLine,
            NextBaseLine,
            SingleWeek,
            FirstWeek,
            FullWeek,
            LastWeek
        }

        /// <summary>
        /// Метод для создания записей в таблице PromoProduct.
        /// Производится подбор списка продуктов по фильтрам выбранных узлов в иерархии продуктов
        /// и запись списка пар PromoId-ProductId в таблицу PromoProduct.
        /// </summary>
        /// <param name="promoId">Id создаваемого/редактируемого промо</param>
        /// <param name="context">Текущий контекст</param>
        public static bool SetPromoProduct(Guid promoId, DatabaseContext context, out string error, bool? duringTheSave = false, List<PromoProductTree> promoProductTrees = null)
        {
			try
			{
				string[] statusesForIncidents = {"OnApproval", "Approved", "Planned"};
				var addedZREPs = new List<string>();
				var deletedZREPs = new List<string>();
                bool needReturnToOnApprovalStatus = false;
                var promo = context.Set<Promo>().Where(x => x.Id == promoId && !x.Disabled).FirstOrDefault();
				var changeProductIncidents = context.Set<ProductChangeIncident>().Where(x => x.NotificationProcessDate == null);
				var changedProducts = changeProductIncidents.Select(x => x.Product.ZREP).Distinct();
				var createdProducts = changeProductIncidents.Where(p => p.IsCreate && !p.IsChecked).Select(i => i.Product.ZREP);
				bool createIncidents = statusesForIncidents.Any(s => s.ToLower() == promo.PromoStatus.SystemName.ToLower());

				var productTreeArray = context.Set<ProductTree>().Where(x => context.Set<PromoProductTree>().Where(p => p.PromoId == promoId && !p.Disabled).Any(p => p.ProductTreeObjectId == x.ObjectId && !x.EndDate.HasValue)).ToArray();

                // добавление записей в таблицу PromoProduct может производиться и при сохранении промо (статус Draft) и при расчете промо (статус !Draft)
                List<Product> filteredProducts = (duringTheSave.HasValue && duringTheSave.Value) ? GetProductFiltered(promoId, context, out error, promoProductTrees) : GetProductFiltered(promoId, context, out error);
                List<string> eanPCs = GetProductListFromAssortmentMatrix(promo, context);
                List<Product> resultProductList = null;

                if (promo.InOut.HasValue && promo.InOut.Value)
                {
                    resultProductList = GetCheckedProducts(context, promo);
                }
                else
                {
                    resultProductList = GetResultProducts(filteredProducts, eanPCs, promo, context);
                }

                var promoProducts = context.Set<PromoProduct>().Where(x => x.PromoId == promoId);
                var incrementalPromoes = context.Set<IncrementalPromo>().Where(x => x.PromoId == promoId);
                var promoProductsNotDisabled = promoProducts.Where(x => !x.Disabled);

                foreach (var promoProduct in promoProductsNotDisabled)
                {
					if (!resultProductList.Any(x => x.ZREP == promoProduct.ZREP))
					{
						if (changedProducts.Contains(promoProduct.ZREP) && createIncidents)
						{
							deletedZREPs.Add(promoProduct.ZREP);
						}
						promoProduct.Disabled = true;
						promoProduct.DeletedDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
						needReturnToOnApprovalStatus = true;
					}
				}

                var draftStatus = context.Set<PromoStatus>().FirstOrDefault(x => x.SystemName == PromoStates.Draft.ToString() && !x.Disabled);
                if (promo.PromoStatus.Id != draftStatus.Id)
                {
                    // Делаем для ускорения вставки записей, через Mapping всё очень долго                    
                    String formatStrPromoProduct = "INSERT INTO [PromoProduct] ([Id], [Disabled], [DeletedDate], [PromoId], [ProductId], [ZREP], [EAN_Case], [EAN_PC], [ProductEN]) VALUES ('{0}', 0, NULL, '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')";
                    String formatStrIncremental = "INSERT INTO [IncrementalPromo] ([Id], [Disabled], [DeletedDate], [PromoId], [ProductId]) VALUES ('{0}', 0, NULL, '{1}', '{2}')";
                    foreach (IEnumerable<Product> items in resultProductList.Partition(100))
                    {
                        string insertScript = String.Empty;

                        foreach (Product p in items)
                        {
                            var promoProduct = promoProducts.FirstOrDefault(x => x.ZREP == p.ZREP);
                            if (promoProduct != null && promoProduct.Disabled)
                            {
                                promoProduct.Disabled = false;
                                promoProduct.DeletedDate = null;
                                needReturnToOnApprovalStatus = true;
                            }
                            else if (promoProduct == null)
                            {
								if (changedProducts.Contains(p.ZREP) && createIncidents)
								{
									addedZREPs.Add(p.ZREP);
								}
                                insertScript += String.Format(formatStrPromoProduct, Guid.NewGuid(), promoId, p.Id, p.ZREP, p.EAN_Case, p.EAN_PC, p.ProductEN);
                                needReturnToOnApprovalStatus = true;
                            }

							if (createdProducts.Any(x => x == p.ZREP) && !addedZREPs.Any(x => x == p.ZREP) && createIncidents)
							{
								addedZREPs.Add(p.ZREP);
							}

							if (promo.InOut.HasValue && promo.InOut.Value)
                            {
                                var incrementalPromo = incrementalPromoes.FirstOrDefault(x => x.Product.ZREP == p.ZREP);
                                if (incrementalPromo != null && incrementalPromo.Disabled)
                                {
                                    incrementalPromo.Disabled = false;
                                    incrementalPromo.DeletedDate = null;
                                    needReturnToOnApprovalStatus = true;
                                }
                                else if (incrementalPromo == null)
                                {
                                    insertScript += String.Format(formatStrIncremental, Guid.NewGuid(), promoId, p.Id);
                                    needReturnToOnApprovalStatus = true;
                                }
                            }
                        }

                        if (!String.IsNullOrEmpty(insertScript))
                        {
                            context.Database.ExecuteSqlCommand(insertScript);
                        }
                    }
                }

				if (addedZREPs.Any() || deletedZREPs.Any())
				{
					Product p = changeProductIncidents.Select(x => x.Product).FirstOrDefault();
					if (p != null)
					{
						ProductChangeIncident pci = new ProductChangeIncident()
						{
							Product = p,
							ProductId = p.Id,
							IsRecalculated = true,
							RecalculatedPromoId = promoId,
							AddedProductIds = addedZREPs.Any() ? String.Join(";", addedZREPs) : null,
							ExcludedProductIds = deletedZREPs.Any() ? String.Join(";", deletedZREPs) : null,
							CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).Value,
							IsCreate = false,
							IsChecked = false
						};
						context.Set<ProductChangeIncident>().Add(pci);
					}
				}
				
                // если добавление записей происходит при сохранении промо (статус Draft), то контекст сохранится в контроллере промо,
                // а если добавление записей происходит при расчете промо (статус !Draft), то сохраняем контекст тут
                if (duringTheSave.HasValue && !duringTheSave.Value)
                {
                    context.SaveChanges();
                }

                return needReturnToOnApprovalStatus;
            }
            catch (Exception e)
            {
                error = e.Message.ToString();
                return false;
            }
        }

        /// <summary>
        /// Метод для формирования списка продуктов для промо.
        /// Возвращает список продуктов для текущего промо с учетом фильтров, ассортиментной матрицы и baseline.
        /// </summary>
        /// <param name="filteredProducts">Список продуктов, подходящих по фильтрам</param>
        /// <param name="eanPCs">Список EAN PC из ассортиментной матрицы</param>
        /// <param name="promo">Промо, для которого подбираются продукты</param>
        /// <param name="context">Текущий контекст</param>
        public static List<Product> GetResultProducts(List<Product> filteredProducts, List<string> eanPCs, Promo promo, DatabaseContext context)
        {
            List<Product> resultProductList = new List<Product>();

            if (filteredProducts != null)
            {
                resultProductList = filteredProducts.Where(x => eanPCs.Any(y => y == x.EAN_PC)).ToList();
                resultProductList = resultProductList.Intersect(GetCheckedProducts(context, promo)).ToList();
            }

            return resultProductList;
        }

        /// <summary>
        /// Метод для выбора подходящего ZREP.
        /// В случае, когда один и тот же EAN PC соответствует нескольким ZREP, выбирается ZREP с максимальным baseline.
        /// </summary>
        /// <param name="productList">Список продуктов с одинаковым EAN PC и разными ZREP</param>
        /// <param name="promo">Промо, для которого подбираются продукты</param>
        /// <param name="context">Текущий контекст</param>
        private static Product GetProductFromList(List<Product> productList, Promo promo, DatabaseContext context)
        {
            ClientTree clientNode = null;
            Product returningProduct = null;
            double? maxPrice = 0;

            foreach (var product in productList)
            {
                BaseLine baseLine = null;
                DateTimeOffset? nextWeekPromoStartDate = null;
                DateTimeOffset? currentWeekPromoStartDate = null;
                double? price = 0;

                bool exit = false;
                BaseLineState state = BaseLineState.InitBaseLine;
                while (!exit)
                {
                    switch (state)
                    {
                        case BaseLineState.InitBaseLine:
                            // выбор BaseLine, на неделю которого попадает начало текущего промо (с учетом выбранного клиента промо)
                            clientNode = context.Set<ClientTree>().Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue).FirstOrDefault();
                            baseLine = context.Set<BaseLine>().Where(x => x.ProductId == product.Id && x.DemandCode == clientNode.DemandCode && x.StartDate.HasValue && DbFunctions.DiffDays(x.StartDate, promo.StartDate) <= 6 && x.StartDate <= promo.StartDate && !x.Disabled).FirstOrDefault();

                            while (clientNode.Type != "root" && baseLine == null)
                            {
                                clientNode = context.Set<ClientTree>().Where(x => x.ObjectId == clientNode.parentId && !x.EndDate.HasValue).FirstOrDefault();
                                baseLine = context.Set<BaseLine>().Where(x => x.ProductId == product.Id && x.DemandCode == clientNode.DemandCode && x.StartDate.HasValue && DbFunctions.DiffDays(x.StartDate, promo.StartDate) <= 6 && x.StartDate <= promo.StartDate && !x.Disabled).FirstOrDefault();
                            }

                            if (baseLine == null)
                            {
                                //если не подобран baseline на начало промо, прибавляем к дате начала промо 1 день до тех пор, пока не найдем подходящий baseline или пока не дойдем до даты окончания промо
                                currentWeekPromoStartDate = promo.StartDate.Value;
                                state = BaseLineState.NullBaseLine;
                            }
                            else
                            {
                                price = baseLine.Price;
                                exit = true;
                            }
                            break;

                        case BaseLineState.NullBaseLine:
                            nextWeekPromoStartDate = currentWeekPromoStartDate.Value.AddDays(1);
                            clientNode = context.Set<ClientTree>().Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue).FirstOrDefault();
                            baseLine = context.Set<BaseLine>().Where(x => x.ProductId == product.Id && x.DemandCode == clientNode.DemandCode && x.StartDate.HasValue && DbFunctions.DiffDays(x.StartDate, nextWeekPromoStartDate) <= 6 && x.StartDate <= nextWeekPromoStartDate && !x.Disabled).FirstOrDefault();

                            while (clientNode.Type != "root" && baseLine == null)
                            {
                                clientNode = context.Set<ClientTree>().Where(x => x.ObjectId == clientNode.parentId && !x.EndDate.HasValue).FirstOrDefault();
                                baseLine = context.Set<BaseLine>().Where(x => x.ProductId == product.Id && x.DemandCode == clientNode.DemandCode && x.StartDate.HasValue && DbFunctions.DiffDays(x.StartDate, nextWeekPromoStartDate) <= 6 && x.StartDate <= nextWeekPromoStartDate && !x.Disabled).FirstOrDefault();
                            }

                            if (nextWeekPromoStartDate > promo.EndDate)
                            {
                                exit = true;
                            }
                            else if (baseLine != null)
                            {
                                price = baseLine.Price;
                                exit = true;
                            }
                            else
                            {
                                currentWeekPromoStartDate = currentWeekPromoStartDate.Value.AddDays(1);
                            }
                            break;
                    }
                }

                if (price > maxPrice)
                {
                    maxPrice = price;
                    returningProduct = product;
                }
            }

            // если baseline не подобрался ни для одного продукта, то возвращаем первый продукт из списка, если впоследствии baseline обновится, то продукты пересчитаются и выберется верный продукт (с максимальным baseline)
            // TODO: Выяснить по какому критерию выбирать ZREP, если не подобрался baseline (пока что выбирается первые из списка, то есть случайный)
            return maxPrice != 0 ? returningProduct : productList[0];
        }

        /// <summary>
        /// Метод для расчета параметров PromoProduct.
        /// Производится подбор подходящих BaseLine и их распределение по всей длительности промо.
        /// </summary>
        /// <param name="promoId">Id создаваемого/редактируемого промо</param>
        /// <param name="context">Текущий контекст</param>
        public static string CalculatePromoProductParameters(Guid promoId, DatabaseContext context)
        {
            try
            {
                var promo = context.Set<Promo>().Where(x => x.Id == promoId && !x.Disabled).FirstOrDefault();
                string message = null;
                Promo promoCopy = new Promo(promo);

                if (promo.StartDate.HasValue && promo.EndDate.HasValue)
                {
                    ClientTree clientNode = context.Set<ClientTree>().Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue).FirstOrDefault();
                    if (clientNode != null)
                    {
                        List<PromoProduct> promoProducts = context.Set<PromoProduct>().Where(x => x.PromoId == promo.Id && !x.Disabled).ToList();
                        double? clientPostPromoEffectW1 = clientNode.PostPromoEffectW1;
                        double? clientPostPromoEffectW2 = clientNode.PostPromoEffectW2;

                        // вначале сбрасываем значения                    
                        ResetProductParams(promoProducts, context);

                        // если стоит флаг inout, расчет производися по другим формулам, подбирать baseline не требуется
                        if (!promo.InOut.HasValue || !promo.InOut.Value)
                        {
                            var promoProductCorrections = context.Set<PromoProductsCorrection>().Where(x => !x.Disabled && x.PromoProduct.PromoId == promo.Id && x.TempId == null);

                            if (!promo.PlanPromoUpliftPercent.HasValue)
                            {
                                message = String.Format("For promo №{0} is no Plan Promo Uplift value. Plan parameters will not be calculated.", promo.Number);
                            }
                            
                            foreach (var promoProduct in promoProducts)
                            {
                                var promoProductCorrection = promoProductCorrections.FirstOrDefault(x => x.PromoProductId == promoProduct.Id && !x.Disabled);
                                var promoProductUplift = promoProductCorrection?.PlanProductUpliftPercentCorrected ?? promoProduct.PlanProductUpliftPercent;
                                promoProduct.PlanProductIncrementalLSV = promoProduct.PlanProductBaselineLSV * promoProductUplift / 100;
                                promoProduct.PlanProductLSV = promoProduct.PlanProductBaselineLSV + promoProduct.PlanProductIncrementalLSV;

                                //Расчет плановых значений PromoProduct
                                promoProduct.PlanProductPCPrice = promoProduct.Product.UOM_PC2Case != 0 ? promoProduct.ProductBaselinePrice / promoProduct.Product.UOM_PC2Case : null;
                                promoProduct.PlanProductIncrementalCaseQty = promoProduct.PlanProductBaselineCaseQty * promoProductUplift / 100;
                                promoProduct.PlanProductCaseQty = promoProduct.PlanProductBaselineCaseQty + promoProduct.PlanProductIncrementalCaseQty;
                                promoProduct.PlanProductPCQty = promoProduct.Product.UOM_PC2Case != 0 ? (int?)promoProduct.PlanProductCaseQty * promoProduct.Product.UOM_PC2Case : null;
                                promoProduct.PlanProductCaseLSV = promoProduct.PlanProductBaselineCaseQty * promoProduct.ProductBaselinePrice;
                                promoProduct.PlanProductPCLSV = promoProduct.Product.UOM_PC2Case != 0 ? (int?)promoProduct.PlanProductCaseLSV / promoProduct.Product.UOM_PC2Case : null;

                                if (clientNode != null)
                                {
                                    //TODO: Уточнить насчет деления на 100
                                    promoProduct.PlanProductPostPromoEffectQtyW1 = promoProduct.PlanProductBaselineCaseQty * clientPostPromoEffectW1 / 100 ?? 0;
                                    promoProduct.PlanProductPostPromoEffectQtyW2 = promoProduct.PlanProductBaselineCaseQty * clientPostPromoEffectW2 / 100 ?? 0;
                                    promoProduct.PlanProductPostPromoEffectQty = promoProduct.PlanProductPostPromoEffectQtyW1 + promoProduct.PlanProductPostPromoEffectQtyW2;

                                    promoProduct.PlanProductPostPromoEffectLSVW1 = promoProduct.PlanProductBaselineLSV * clientPostPromoEffectW1 / 100 ?? 0;
                                    promoProduct.PlanProductPostPromoEffectLSVW2 = promoProduct.PlanProductBaselineLSV * clientPostPromoEffectW2 / 100 ?? 0;
                                    promoProduct.PlanProductPostPromoEffectLSV = promoProduct.PlanProductPostPromoEffectLSVW1 + promoProduct.PlanProductPostPromoEffectLSVW2;
                                }
                            }

                            double? sumPlanProductBaseLineLSV = promoProducts.Sum(x => x.PlanProductBaselineLSV);
                            double? sumPlanProductIncrementalLSV = promoProducts.Sum(x => x.PlanProductIncrementalLSV);

                            promo.PlanPromoUpliftPercent = sumPlanProductBaseLineLSV != 0 ? sumPlanProductIncrementalLSV / sumPlanProductBaseLineLSV * 100 : null;

                            promo.PlanPromoIncrementalLSV = sumPlanProductIncrementalLSV;
                            promo.PlanPromoBaselineLSV = sumPlanProductBaseLineLSV;
                            promo.PlanPromoLSV = promo.PlanPromoBaselineLSV + promo.PlanPromoIncrementalLSV;
                        }
                        else
                        {
                            foreach (var promoProduct in promoProducts)
                            {
                                IncrementalPromo incrementalPromo = context.Set<IncrementalPromo>().Where(x => x.PromoId == promo.Id && x.ProductId == promoProduct.ProductId && !x.Disabled).FirstOrDefault();

                                if (incrementalPromo != null)
                                {
                                    //Расчет плановых значений PromoProduct
                                    promoProduct.ProductBaselinePrice = incrementalPromo.CasePrice;
                                    promoProduct.PlanProductPCPrice = promoProduct.Product.UOM_PC2Case != 0 ? promoProduct.ProductBaselinePrice / promoProduct.Product.UOM_PC2Case : null;
                                    promoProduct.PlanProductIncrementalCaseQty = incrementalPromo.PlanPromoIncrementalCases;
                                    promoProduct.PlanProductCaseQty = promoProduct.PlanProductIncrementalCaseQty;
                                    promoProduct.PlanProductPCQty = promoProduct.Product.UOM_PC2Case != 0 ? (int?)promoProduct.PlanProductCaseQty * promoProduct.Product.UOM_PC2Case : null;
                                    promoProduct.PlanProductCaseLSV = promoProduct.PlanProductCaseQty * promoProduct.ProductBaselinePrice;
                                    promoProduct.PlanProductIncrementalLSV = incrementalPromo.PlanPromoIncrementalLSV;
                                    promoProduct.PlanProductLSV = promoProduct.PlanProductIncrementalLSV;

                                    // TODO: удаляем?
                                    //promoProduct.PlanProductPCLSV = promoProduct.Product.UOM_PC2Case != 0 ? (int?)promoProduct.PlanProductCaseLSV / promoProduct.Product.UOM_PC2Case : null;
                                }
                                else
                                {
                                    message = String.Format("Incremental promo was not found for product with ZREP: {0}", promoProduct.Product.ZREP);
                                }

                                //promoProduct.PlanProductUpliftPercent = promo.PlanPromoUpliftPercent;

                                promoProduct.PlanProductPostPromoEffectQtyW1 = 0;
                                promoProduct.PlanProductPostPromoEffectQtyW2 = 0;
                                promoProduct.PlanProductPostPromoEffectQty = 0;
                                promoProduct.PlanProductPostPromoEffectLSVW1 = 0;
                                promoProduct.PlanProductPostPromoEffectLSVW2 = 0;
                                promoProduct.PlanProductPostPromoEffectLSV = 0;
                            }

                            promo.PlanPromoBaselineLSV = null;

                            double? sumPlanProductIncrementalLSV = promoProducts.Sum(x => x.PlanProductIncrementalLSV);
                            // LSV = Qty ?
                            promo.PlanPromoIncrementalLSV = sumPlanProductIncrementalLSV;
                            promo.PlanPromoLSV = promo.PlanPromoIncrementalLSV;
                        }

                        if (PromoUtils.HasChanges(context.ChangeTracker, promo.Id))
                        {
                            promo.LastChangedDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                            if (IsDemandChanged(promo, promoCopy))
                            {
                                promo.LastChangedDateDemand = promo.LastChangedDate;
                                promo.LastChangedDateFinance = promo.LastChangedDate;
                            }
                        }

                        context.SaveChanges();
                    }
                    else
                    {
                        message = String.Format("Plan parameters can not be recalculated, because client was not found for this promo.");
                    }
                }
                else
                {
                    message = String.Format("Promo has not start date or end date");
                }
                return message;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);

                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public static string CalculateBaseline(DatabaseContext context, Guid promoId)
        {
            string message = null;
            var promo = context.Set<Promo>().Where(x => x.Id == promoId && !x.Disabled).FirstOrDefault();

            if (promo.StartDate.HasValue && promo.EndDate.HasValue)
            {
                if (!promo.InOut.HasValue || !promo.InOut.Value)
                {
                    bool baseLineFound = false; // по "0" проверять не очень, а вдруг он есть, но равен нулю, поэтому через переменную

                    ClientTree clientNode = context.Set<ClientTree>().Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue).FirstOrDefault();
                    List<PromoProduct> promoProducts = context.Set<PromoProduct>().Where(x => x.PromoId == promo.Id && !x.Disabled).ToList();

                    foreach (var promoProduct in promoProducts)
                    {
                        ClientTreeBrandTech clientTreeBrandTech = null;
                        List<ClientTreeBrandTech> disabledClientTreeBrandTechList = new List<ClientTreeBrandTech>();

                        //Сбрасываем параметры
                        promoProduct.PlanProductBaselineLSV = null;
                        promoProduct.PlanProductBaselineCaseQty = null;
                        promoProduct.ProductBaselinePrice = null;

                        //расчетные параметры для каждого продукта в промо
                        double planProductBaseLineLSV = 0;
                        double planProductBaseLineCaseQty = 0;
                        double productBaseLinePrice = 0;
                        double price = 0;

                        // коэффициент для BaseLine с учетом долевого распределения
                        double baseLineShareIndex = 1;
                        BaseLine baseLine = null;
                        DateTimeOffset? nextBaseLineStartDate = null;
                        DateTimeOffset? currentBaseLineStartDate = null;
                        DateTimeOffset? nextWeekPromoStartDate = null;
                        DateTimeOffset? currentWeekPromoStartDate = null;

                        bool exit = false;
                        BaseLineState state = BaseLineState.InitBaseLine;
                        while (!exit)
                        {
                            switch (state)
                            {
                                case BaseLineState.InitBaseLine:
                                    // выбор BaseLine, на неделю которого попадает начало текущего промо (с учетом выбранного клиента промо)
                                    clientNode = context.Set<ClientTree>().Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue).FirstOrDefault();
                                    baseLine = context.Set<BaseLine>().Where(x => x.ProductId == promoProduct.ProductId && x.DemandCode == clientNode.DemandCode && x.StartDate.HasValue && DbFunctions.DiffDays(x.StartDate, promo.StartDate) <= 6 && x.StartDate <= promo.StartDate && !x.Disabled).FirstOrDefault();

                                    while (clientNode != null && clientNode.Type != "root" && baseLine == null)
                                    {
                                        clientTreeBrandTech = context.Set<ClientTreeBrandTech>().Where(x => x.ClientTreeId == clientNode.Id && x.BrandTechId == promo.BrandTechId && !x.Disabled).FirstOrDefault();
                                        if (clientTreeBrandTech == null)
                                        {
                                            disabledClientTreeBrandTechList = context.Set<ClientTreeBrandTech>().Where(x => x.ClientTreeId == clientNode.Id && x.BrandTechId == promo.BrandTechId && x.Disabled).OrderByDescending(x => x.DeletedDate).ToList();
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
                                        baseLine = context.Set<BaseLine>().Where(x => x.ProductId == promoProduct.ProductId && x.DemandCode == clientNode.DemandCode && x.StartDate.HasValue && DbFunctions.DiffDays(x.StartDate, promo.StartDate) <= 6 && x.StartDate <= promo.StartDate && !x.Disabled).FirstOrDefault();
                                    }

                                    if (baseLine == null)
                                    {
                                        //если не подобран baseline на начало промо, прибавляем к дате начала промо 1 день до тех пор, пока не найдем подходящий baseline или пока не дойдем до даты окончания промо
                                        currentWeekPromoStartDate = promo.StartDate.Value;
                                        state = BaseLineState.NullBaseLine;
                                    }
                                    else if (baseLine.StartDate.Value.AddDays(6) >= promo.EndDate)
                                    {
                                        state = BaseLineState.SingleWeek;
                                    }
                                    else
                                    {
                                        state = BaseLineState.FirstWeek;
                                    }
                                    break;

                                case BaseLineState.NullBaseLine:
                                    nextWeekPromoStartDate = currentWeekPromoStartDate.Value.AddDays(1);
                                    baseLineShareIndex = 1;
                                    clientNode = context.Set<ClientTree>().Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue).FirstOrDefault();
                                    baseLine = context.Set<BaseLine>().Where(x => x.ProductId == promoProduct.ProductId && x.DemandCode == clientNode.DemandCode && x.StartDate.HasValue && DbFunctions.DiffDays(x.StartDate, nextWeekPromoStartDate) <= 6 && x.StartDate <= nextWeekPromoStartDate && !x.Disabled).FirstOrDefault();

                                    while (clientNode != null && clientNode.Type != "root" && baseLine == null)
                                    {
                                        clientTreeBrandTech = context.Set<ClientTreeBrandTech>().Where(x => x.ClientTreeId == clientNode.Id && x.BrandTechId == promo.BrandTechId && !x.Disabled).FirstOrDefault();
                                        if (clientTreeBrandTech == null)
                                        {
                                            disabledClientTreeBrandTechList = context.Set<ClientTreeBrandTech>().Where(x => x.ClientTreeId == clientNode.Id && x.BrandTechId == promo.BrandTechId && x.Disabled).OrderByDescending(x => x.DeletedDate).ToList();
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
                                        baseLine = context.Set<BaseLine>().Where(x => x.ProductId == promoProduct.ProductId && x.DemandCode == clientNode.DemandCode && x.StartDate.HasValue && DbFunctions.DiffDays(x.StartDate, nextWeekPromoStartDate) <= 6 && x.StartDate <= nextWeekPromoStartDate && !x.Disabled).FirstOrDefault();
                                    }

                                    if (nextWeekPromoStartDate > promo.EndDate)
                                    {
                                        exit = true;
                                    }
                                    else if (baseLine != null && baseLine.StartDate.HasValue && baseLine.StartDate.Value.AddDays(6) <= promo.EndDate)
                                    {
                                        //BaseLine, которые целиком входят в промо
                                        state = BaseLineState.FullWeek;
                                    }
                                    else if (baseLine != null && baseLine.StartDate.HasValue && promo.EndDate >= baseLine.StartDate)
                                    {
                                        //если промо захватывает часть дней следующего BaseLine
                                        state = BaseLineState.LastWeek;
                                    }
                                    else
                                    {
                                        currentWeekPromoStartDate = currentWeekPromoStartDate.Value.AddDays(1);
                                    }

                                    break;

                                case BaseLineState.NextBaseLine:
                                    nextBaseLineStartDate = currentBaseLineStartDate.Value.AddDays(7);
                                    baseLineShareIndex = 1;
                                    clientNode = context.Set<ClientTree>().Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue).FirstOrDefault();
                                    baseLine = context.Set<BaseLine>().Where(x => x.ProductId == promoProduct.ProductId && x.DemandCode == clientNode.DemandCode && x.StartDate.HasValue && x.StartDate.Value == nextBaseLineStartDate && !x.Disabled).FirstOrDefault();

                                    while (clientNode != null && clientNode.Type != "root" && baseLine == null)
                                    {
                                        clientTreeBrandTech = context.Set<ClientTreeBrandTech>().Where(x => x.ClientTreeId == clientNode.Id && x.BrandTechId == promo.BrandTechId && !x.Disabled).FirstOrDefault();
                                        if (clientTreeBrandTech == null)
                                        {
                                            disabledClientTreeBrandTechList = context.Set<ClientTreeBrandTech>().Where(x => x.ClientTreeId == clientNode.Id && x.BrandTechId == promo.BrandTechId && x.Disabled).OrderByDescending(x => x.DeletedDate).ToList();
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
                                        baseLine = context.Set<BaseLine>().Where(x => x.ProductId == promoProduct.ProductId && x.DemandCode == clientNode.DemandCode && x.StartDate.HasValue && x.StartDate.Value == nextBaseLineStartDate && !x.Disabled).FirstOrDefault();
                                    }

                                            if (nextBaseLineStartDate > promo.EndDate)
                                            {
                                                exit = true;
                                            }
                                            else if (baseLine != null && baseLine.StartDate.HasValue && baseLine.StartDate.Value.AddDays(6) <= promo.EndDate)
                                            {
                                                //BaseLine, которые целиком входят в промо
                                                state = BaseLineState.FullWeek;
                                            }
                                            else if (baseLine != null && baseLine.StartDate.HasValue && promo.EndDate >= baseLine.StartDate)
                                            {
                                                //если промо захватывает часть дней следующего BaseLine
                                                state = BaseLineState.LastWeek;
                                            }
                                            else
                                            {
                                                currentBaseLineStartDate = currentBaseLineStartDate.Value.AddDays(7);
                                            }

                                    break;

                                case BaseLineState.SingleWeek:
                                    //длительность промо
                                    var promoDuration = Math.Abs((promo.EndDate.Value - promo.StartDate.Value).Days) + 1;

                                    planProductBaseLineLSV += (baseLine.BaselineLSV.Value * baseLineShareIndex / 7) * promoDuration;
                                    planProductBaseLineCaseQty += (baseLine.QTY.Value * baseLineShareIndex / 7) * promoDuration;
                                    productBaseLinePrice += (baseLine.Price.Value * baseLineShareIndex / 7) * promoDuration;
                                    price = baseLine.Price.Value; //значение цены должно быть равно полной цене для этой недели

                                    exit = true;
                                    baseLineFound = true;
                                    break;

                                case BaseLineState.FirstWeek:
                                    //количество дней, которое надо взять от первого BaseLine
                                    var firstBaseLineDays = Math.Abs((baseLine.StartDate.Value.AddDays(7) - promo.StartDate.Value).Days);

                                    planProductBaseLineLSV += (baseLine.BaselineLSV.Value * baseLineShareIndex / 7) * firstBaseLineDays;
                                    planProductBaseLineCaseQty += (baseLine.QTY.Value * baseLineShareIndex / 7) * firstBaseLineDays;
                                    productBaseLinePrice += (baseLine.Price.Value * baseLineShareIndex / 7) * firstBaseLineDays;
                                    price = baseLine.Price.Value; //значение цены должно быть равно цене из baseline, ближайшего к дате начала

                                    currentBaseLineStartDate = baseLine.StartDate.Value;
                                    state = BaseLineState.NextBaseLine;
                                    baseLineFound = true;
                                    break;

                                case BaseLineState.FullWeek:
                                    planProductBaseLineLSV += baseLine.BaselineLSV.Value * baseLineShareIndex;
                                    planProductBaseLineCaseQty += baseLine.QTY.Value * baseLineShareIndex;
                                    productBaseLinePrice += baseLine.Price.Value * baseLineShareIndex;

                                    currentBaseLineStartDate = baseLine.StartDate.Value;
                                    state = BaseLineState.NextBaseLine;
                                    baseLineFound = true;
                                    break;

                                case BaseLineState.LastWeek:
                                    //количество дней, которое надо взять от последнего BaseLine
                                    var lastBaseLineDays = Math.Abs((promo.EndDate.Value - baseLine.StartDate.Value).Days) + 1;

                                    planProductBaseLineLSV += (baseLine.BaselineLSV.Value * baseLineShareIndex / 7) * lastBaseLineDays;
                                    planProductBaseLineCaseQty += (baseLine.QTY.Value * baseLineShareIndex / 7) * lastBaseLineDays;
                                    productBaseLinePrice += (baseLine.Price.Value * baseLineShareIndex / 7) * lastBaseLineDays;

                                    exit = true;
                                    baseLineFound = true;
                                    break;
                            }
                        }

                        // если не нашли BaseLine, пишем об этом
                        if (!baseLineFound)
                        {
                            if (message == null)
                                message = "";

                            message += String.Format("\nPlan Product Baseline LSV was not found for product with ZREP: {0}", promoProduct.Product.ZREP);
                        }

                        //Расчет плановых значений PromoProduct
                        promoProduct.PlanProductBaselineLSV = planProductBaseLineLSV;
                        promoProduct.PlanProductBaselineCaseQty = planProductBaseLineCaseQty;
                        promoProduct.ProductBaselinePrice = price; //productBaseLinePrice;
                    }
                }
            }
            context.SaveChanges();
            return message;
        }

        /// <summary>
        /// Получить продукты, подходящие под фильтр
        /// </summary>
        /// <param name="promoId">ID промо</param>
        /// <param name="context">Контекст БД</param>
        /// <param name="error">Сообщения об ошибках</param>
        /// <returns></returns>
        public static List<Product> GetProductFiltered(Guid promoId, DatabaseContext context, out string error, List<PromoProductTree> promoProductTrees = null)
        {
            // также используется в промо для проверки, если нет продуктов, то сохранение/редактирование отменяется
            // отдельно т.к. заполнение может оказаться очень долгой операцией
            List<Product> product = null;
            List<Product> filteredProductList = new List<Product>();
            error = null;

            try
            {
                var promo = context.Set<Promo>().Where(x => x.Id == promoId && !x.Disabled).FirstOrDefault();

                // из-за отказа от множества SaveChanges приходиться возить с собой список узлов в прод. дереве
                ProductTree[] productTreeArray;
                if (promoProductTrees == null)
                {
                    productTreeArray = context.Set<ProductTree>().Where(x => context.Set<PromoProductTree>().Where(p => p.PromoId == promoId && !p.Disabled).Any(p => p.ProductTreeObjectId == x.ObjectId && !x.EndDate.HasValue)).ToArray();
                }
                else
                {
                    productTreeArray = context.Set<ProductTree>().Where(x => !x.EndDate.HasValue).ToArray().Where(n => promoProductTrees.Any(p => p.ProductTreeObjectId == n.ObjectId)).ToArray();
                }

                product = context.Set<Product>().Where(x => !x.Disabled).ToList();

                foreach (var productTree in productTreeArray)
                {
                    var stringFilter = productTree.Filter;
                    // можно и на 0 проверить, но вдруг будет пустой фильтр вида "{}"
                    if (stringFilter.Length < 2)
                        throw new Exception("Filter for product " + productTree.FullPathName + " is empty");

                    // Преобразование строки фильтра в соответствующий класс
                    FilterNode filter = stringFilter.ConvertToNode();

                    // Создание функции фильтрации на основе построенного фильтра
                    var expr = filter.ToExpressionTree<Product>();

                    // Список продуктов, подходящих по параметрам фильтрации
                    product = product.Where(expr.Compile()).ToList();

                    filteredProductList = filteredProductList.Union(product).ToList();
                    product = context.Set<Product>().Where(x => !x.Disabled).ToList();
                }

                if (filteredProductList.Count == 0)
                {
                    throw new Exception("No suitable products were found for the current PROMO");
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                filteredProductList = null;
            }

            return filteredProductList;
        }

        public static bool IsProductListEmpty(Promo promo, DatabaseContext context, out string error, List<PromoProductTree> promoProductTrees = null)
        {
            List<Product> filteredProducts = GetProductFiltered(promo.Id, context, out error, promoProductTrees);
            List<string> eanPCs = GetProductListFromAssortmentMatrix(promo, context);
            List<Product> resultProductList = null;

            if (promo.InOut.HasValue && promo.InOut.Value)
            {
                resultProductList = GetCheckedProducts(context, promo);
            }
            else
            {
                resultProductList = GetResultProducts(filteredProducts, eanPCs, promo, context);
            }

            if (resultProductList.Count() > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Формирует список продуктов из записей таблицы AssortimentMatrix, которые предварительно фильтруются по клиенту и дате.
        /// Promo.Dispatches.Start должна быть между AssortimentMatrix.Start и AssortimentMatrix.End включительно.
        /// </summary>
        /// <param name="promo">Модель Promo</param>
        /// <param name="context">Контекст базы данных</param>
        /// <returns>Возвращает список продуктов, сформированный из записей таблицы AssortimentMatrix.</returns>
        public static List<string> GetProductListFromAssortmentMatrix(Promo promo, DatabaseContext context)
        {
            // Список продуктов из ассортиментной матрицы, который будет возвращен.
            var productListFromAssortimentMatrix = new List<Product>();

            // Отфильтрованные записи из таблицы AssortimentMatrix.
            var assortimentMatrixFilteredRecords = context.Set<AssortmentMatrix>().Where(x => !x.Disabled);
            assortimentMatrixFilteredRecords = assortimentMatrixFilteredRecords.Where(x => x.ClientTreeId == promo.ClientTreeKeyId);
            assortimentMatrixFilteredRecords = assortimentMatrixFilteredRecords.Where(x => promo.DispatchesStart >= x.StartDate && promo.DispatchesStart <= x.EndDate);
            List<string> eanPCList = assortimentMatrixFilteredRecords.Select(x => x.Product.EAN_PC).ToList();

            return eanPCList;
        }

        public static List<string> GetProductListFromAssortmentMatrix(DatabaseContext context, int clientTreeKeyId, DateTimeOffset dispatchesStart, DateTimeOffset dispatchesEnd)
        {
            // Список продуктов из ассортиментной матрицы, который будет возвращен.
            var productListFromAssortimentMatrix = new List<Product>();

            // Отфильтрованные записи из таблицы AssortimentMatrix.
            var assortimentMatrixFilteredRecords = context.Set<AssortmentMatrix>().Where(x => !x.Disabled);
            assortimentMatrixFilteredRecords = assortimentMatrixFilteredRecords.Where(x => x.ClientTreeId == clientTreeKeyId);
            assortimentMatrixFilteredRecords = assortimentMatrixFilteredRecords.Where(x => dispatchesStart >= x.StartDate && dispatchesStart <= x.EndDate);
            List<string> eanPCList = assortimentMatrixFilteredRecords.Select(x => x.Product.EAN_PC).ToList();

            return eanPCList;
        }

        /// <summary>
        /// Сбросить значения для продуктов
        /// </summary>
        /// <param name="promoProducts">Список продуктов</param>
        /// <param name="context">Контекст БД</param>
        private static void ResetProductParams(List<PromoProduct> promoProducts, DatabaseContext context)
        {
            foreach (PromoProduct product in promoProducts)
            {
                //product.PlanProductBaselineLSV = null;
                //product.PlanProductBaselineCaseQty = null;
                //product.ProductBaselinePrice = null;
                product.PlanProductPCPrice = null;
                product.PlanProductIncrementalCaseQty = null;
                product.PlanProductCaseQty = null;
                product.PlanProductPCQty = null;
                product.PlanProductCaseLSV = null;
                product.PlanProductPCLSV = null;
                //product.PlanProductUpliftPercent = null;
                product.PlanProductPostPromoEffectQtyW1 = null;
                product.PlanProductPostPromoEffectQtyW2 = null;
                product.PlanProductPostPromoEffectQty = null;
                product.PlanProductPostPromoEffectLSVW1 = null;
                product.PlanProductPostPromoEffectLSVW2 = null;
                product.PlanProductPostPromoEffectLSV = null;
            }
        }

        public static List<Product> GetCheckedProducts(DatabaseContext context, Promo promo)
        {
            List<Product> products = new List<Product>();

            if (!String.IsNullOrEmpty(promo.InOutProductIds))
            {
                var productIds = promo.InOutProductIds.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(x => Guid.Parse(x)).ToList();

                foreach (var productId in productIds)
                {
                    var product = context.Set<Product>().FirstOrDefault(x => x.Id == productId && !x.Disabled);

                    if (product != null)
                    {
                        products.Add(product);
                    }
                }
            }

            return products;
        }

        public static bool IsDemandChanged(Promo oldPromo, Promo newPromo)
        {
            if (oldPromo.PlanPromoLSV != newPromo.PlanPromoLSV
                || oldPromo.PlanPromoIncrementalLSV != newPromo.PlanPromoIncrementalLSV)
                return true;
            else return false;
        }
    }
}