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
    public class PlanProductParametersCalculation
    {
        /// <summary>
        /// Метод для создания записей в таблице PromoProduct.
        /// Производится подбор списка продуктов по фильтрам выбранных узлов в иерархии продуктов
        /// и запись списка пар PromoId-ProductId в таблицу PromoProduct.
        /// </summary>
        /// <param name="promoId">Id создаваемого/редактируемого промо</param>
        /// <param name="context">Текущий контекст</param>
        public static void SetPromoProduct(Guid promoId, DatabaseContext context, out string error)
        {
            try
            {
                var promo = context.Set<Promo>().Where(x => x.Id == promoId && !x.Disabled).FirstOrDefault();

                var productTreeArray = context.Set<ProductTree>().Where(x => context.Set<PromoProductTree>().Where(p => p.PromoId == promoId && !p.Disabled).Any(p => p.ProductTreeObjectId == x.ObjectId && !x.EndDate.HasValue)).ToArray();
                List<Product> product = GetProductFiltered(promoId, context, out error);
                if (error == null)
                {
                    IQueryable<PromoProduct> promoproducts = context.Set<PromoProduct>().Where(x => x.PromoId == promoId && !x.Disabled);
                    // проверяем список, удаляем не вошедшие, оставляем предыдущие
                    foreach (var oldPromoProduct in promoproducts)
                    {
                        Product existProduct = product.FirstOrDefault(n => n.ZREP == oldPromoProduct.ZREP);

                        if (existProduct != null)
                        {
                            product.Remove(existProduct);
                        }
                        else
                        {
                            oldPromoProduct.Disabled = true;
                            oldPromoProduct.DeletedDate = DateTime.Now;
                        }
                    }

                    foreach (var p in product)
                    {
                        //заполнение таблицы PromoProduct
                        PromoProduct promoproduct = new PromoProduct()
                        {
                            PromoId = promoId,
                            ProductId = p.Id,
                            ZREP = p.ZREP,
                            EAN = p.EAN,
                            ProductEN = p.ProductEN
                        };

                        context.Set<PromoProduct>().Add(promoproduct);
                    }

                    context.SaveChanges();

                    error = null;
                }
            }
            catch (Exception e)
            {
                error = e.ToString();
            }
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

                if (promo.StartDate.HasValue && promo.EndDate.HasValue)
                {
                    List<PromoProduct> promoProducts = context.Set<PromoProduct>().Where(x => x.PromoId == promo.Id && !x.Disabled).ToList();
                    double sumPlanProductBaseLineLSV = 0;

                    // вначале сбрасываем значения                    
                    ResetProductParams(promoProducts, context);                    

                    foreach (var promoProduct in promoProducts)
                    {
                        // коэффициент для BaseLine с учетом долевого распределения
                        double baseLineShareIndex = 1;

                        ClientTree clientNode = context.Set<ClientTree>().Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue).FirstOrDefault();
                        // выбор BaseLine, на неделю которого попадает начало текущего промо (с учетом выбранного клиента промо)
                        BaseLine baseLine = context.Set<BaseLine>().Where(x => x.ProductId == promoProduct.ProductId && x.ClientTreeId == clientNode.Id && x.StartDate.HasValue && DbFunctions.DiffDays(x.StartDate, promo.StartDate) <= 6 && x.StartDate <= promo.StartDate).FirstOrDefault();

                        while (clientNode.Type != "root" && baseLine == null)
                        {
                            baseLineShareIndex *= ((double)clientNode.Share / 100);
                            clientNode = context.Set<ClientTree>().Where(x => x.ObjectId == clientNode.parentId && !x.EndDate.HasValue).FirstOrDefault();
                            baseLine = context.Set<BaseLine>().Where(x => x.ProductId == promoProduct.ProductId && x.ClientTreeId == clientNode.Id && x.StartDate.HasValue && DbFunctions.DiffDays(x.StartDate, promo.StartDate) <= 6 && x.StartDate <= promo.StartDate).FirstOrDefault();
                        }

                        //расчетные параметры для каждого продукта в промо
                        double planProductBaseLineLSV = 0;
                        double planProductBaseLineQty = 0;
                        double productBaseLinePrice = 0;

                        if (baseLine != null)
                        {
                            //проверка, не наступает ли окончание промо на этой же неделе(т.е. промо укладывается в неполную неделю BaseLine)
                            if (baseLine.StartDate.Value.AddDays(6) >= promo.EndDate)
                            {
                                //длительность промо
                                var promoDuration = Math.Abs((promo.EndDate.Value - promo.StartDate.Value).Days) + 1;

                                planProductBaseLineLSV += (baseLine.BaselineLSV.Value * baseLineShareIndex / 7) * promoDuration;
                                planProductBaseLineQty += (baseLine.QTY.Value * baseLineShareIndex / 7) * promoDuration;
                                productBaseLinePrice += (baseLine.Price.Value * baseLineShareIndex / 7) * promoDuration;
                            }
                            else
                            {
                                //количество дней, которое надо взять от первого BaseLine
                                var firstBaseLineDays = Math.Abs((baseLine.StartDate.Value.AddDays(7) - promo.StartDate.Value).Days);

                                planProductBaseLineLSV += (baseLine.BaselineLSV.Value * baseLineShareIndex / 7) * firstBaseLineDays;
                                planProductBaseLineQty += (baseLine.QTY.Value * baseLineShareIndex / 7) * firstBaseLineDays;
                                productBaseLinePrice += (baseLine.Price.Value * baseLineShareIndex / 7) * firstBaseLineDays;

                                var nextBaseLineStartDate = baseLine.StartDate.Value.AddDays(7);
                                baseLine = context.Set<BaseLine>().Where(x => x.ProductId == promoProduct.ProductId && x.StartDate.HasValue && x.StartDate.Value == nextBaseLineStartDate).FirstOrDefault();

                                //цикл для поиска BaseLine, которые целиком входят в промо
                                while (baseLine != null && baseLine.StartDate.HasValue && baseLine.StartDate.Value.AddDays(6) <= promo.EndDate)
                                {
                                    planProductBaseLineLSV += baseLine.BaselineLSV.Value * baseLineShareIndex;
                                    planProductBaseLineQty += baseLine.QTY.Value * baseLineShareIndex;
                                    productBaseLinePrice += baseLine.Price.Value * baseLineShareIndex;

                                    nextBaseLineStartDate = baseLine.StartDate.Value.AddDays(7);
                                    baseLine = context.Set<BaseLine>().Where(x => x.ProductId == promoProduct.ProductId && x.StartDate.HasValue && x.StartDate.Value == nextBaseLineStartDate).FirstOrDefault();
                                }

                                //если промо захватывает часть дней следующего BaseLine
                                if (baseLine != null && baseLine.StartDate.HasValue && promo.EndDate >= baseLine.StartDate)
                                {
                                    //количество дней, которое надо взять от последнего BaseLine
                                    var lastBaseLineDays = Math.Abs((promo.EndDate.Value - baseLine.StartDate.Value).Days) + 1;

                                    planProductBaseLineLSV += (baseLine.BaselineLSV.Value * baseLineShareIndex / 7) * lastBaseLineDays;
                                    planProductBaseLineQty += (baseLine.QTY.Value * baseLineShareIndex / 7) * lastBaseLineDays;
                                    productBaseLinePrice += (baseLine.Price.Value * baseLineShareIndex / 7) * lastBaseLineDays;
                                }
                            }
                            sumPlanProductBaseLineLSV += planProductBaseLineLSV;

                            //Расчет плановых значений PromoProduct
                            promoProduct.PlanProductBaselineLSV = planProductBaseLineLSV;
                            promoProduct.PlanProductBaselineQty = planProductBaseLineQty;
                            promoProduct.ProductBaselinePrice = productBaseLinePrice;
                            promoProduct.ProductBaselinePCPrice = promoProduct.Product.UOM_PC2Case != 0 ? productBaseLinePrice / promoProduct.Product.UOM_PC2Case : null;
                            promoProduct.PlanProductIncrementalQty = planProductBaseLineQty * promo.PlanPromoUpliftPercent / 100;
                            promoProduct.PlanProductQty = planProductBaseLineQty + promoProduct.PlanProductIncrementalQty;
                            promoProduct.PlanProductPCQty = promoProduct.Product.UOM_PC2Case != 0 ? (int?)promoProduct.PlanProductQty * promoProduct.Product.UOM_PC2Case : null;
                            promoProduct.PlanProductLSV = planProductBaseLineQty * productBaseLinePrice;
                            promoProduct.PlanProductPCLSV = promoProduct.Product.UOM_PC2Case != 0 ? (int?)promoProduct.PlanProductLSV / promoProduct.Product.UOM_PC2Case : null;
                            promoProduct.PlanProductUpliftPercent = promo.PlanPromoUpliftPercent;
                        }
                        else
                        {
                            return "BaseLine was not found";
                        }
                    }
                    context.SaveChanges();
                }

                return null;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        /// <summary>
        /// Получить продукты, подходящие под фильтр
        /// </summary>
        /// <param name="promoId">ID промо</param>
        /// <param name="context">Контекст БД</param>
        /// <param name="error">Сообщения об ошибках</param>
        /// <returns></returns>
        public static List<Product> GetProductFiltered(Guid promoId, DatabaseContext context, out string error)
        {
            // также используется в промо для проверки, если нет продуктов, то сохранение/редактирование отменяется
            // отдельно т.к. заполнение может оказаться очень долгой операцией
            List<Product> product = null;
            error = null;

            try
            {
                var promo = context.Set<Promo>().Where(x => x.Id == promoId && !x.Disabled).FirstOrDefault();
                var productTreeArray = context.Set<ProductTree>().Where(x => context.Set<PromoProductTree>().Where(p => p.PromoId == promoId && !p.Disabled).Any(p => p.ProductTreeObjectId == x.ObjectId && !x.EndDate.HasValue)).ToArray();
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
                }

                if (product.Count == 0)
                    throw new Exception("No suitable products were found for the current PROMO");
            }
            catch (Exception e)
            {
                error = e.Message;
                product = null;
            }

            return product;
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
                product.PlanProductBaselineLSV = null;
                product.PlanProductBaselineQty = null;
                product.ProductBaselinePrice = null;
                product.ProductBaselinePCPrice = null;
                product.PlanProductIncrementalQty = null;
                product.PlanProductQty = null;
                product.PlanProductPCQty = null;
                product.PlanProductLSV = null;
                product.PlanProductPCLSV = null;
                product.PlanProductUpliftPercent = null;
            }

            context.SaveChanges();
        }
    }
}