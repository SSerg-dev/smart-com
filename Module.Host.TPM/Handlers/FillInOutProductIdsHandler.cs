using Interfaces.Core.Common;
using Looper.Core;
using Module.Host.TPM.Actions;
using Module.Host.TPM.Actions.Notifications;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils.Filter;
using Persist;
using ProcessingHost.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.LogWriter;

namespace Module.Host.TPM.Handlers
{
    /// <summary>
    /// Класс для запуска экшена по автоматическому сбросу статуса промо по различным условиям
    /// </summary>
    public class FillInOutProductIdsHandler : BaseHandler
    {
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            ILogWriter handlerLogger = null;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                handlerLogger = new FileLogWriter(info.HandlerId.ToString());
                handlerLogger.Write(true, String.Format("Handler began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now), "Message");

                using (DatabaseContext context = new DatabaseContext())
                {
                    var promoWithEmptyInOutProductIds = context.Set<Promo>().Where(x => (x.InOutProductIds == null || x.InOutProductIds == "") && !x.Disabled && (!x.InOut.HasValue || !x.InOut.Value));
                    handlerLogger.Write(true, String.Format("Count: {0}", promoWithEmptyInOutProductIds.Count()), "Message");
                    foreach (var promo in promoWithEmptyInOutProductIds)
                    {
                        
                            List<Product> filteredProductList = new List<Product>();
                            string inOutProductIds = "";
                            List<Product> product = null;
                            var promoProductTreeList = context.Set<PromoProductTree>().Where(x => x.PromoId == promo.Id).ToList();

                            product = context.Set<Product>().Where(x => !x.Disabled).ToList();

                            foreach (var promoProductTree in promoProductTreeList)
                            {
                                ProductTree productTree = context.Set<ProductTree>().Where(x => x.ObjectId == promoProductTree.ProductTreeObjectId && !x.EndDate.HasValue).FirstOrDefault();
                                if (productTree != null && !String.IsNullOrEmpty(productTree.Filter))
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
                            }

                            foreach (var filteredProduct in filteredProductList)
                            {
                                inOutProductIds += filteredProduct.Id + ";";
                            }

                            handlerLogger.Write(true, String.Format("{0} - {1}", promo.Number, inOutProductIds), "Message");
                            promo.InOutProductIds = inOutProductIds;
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                data.SetValue<bool>("HasErrors", true);
                logger.Error(e);
                if (handlerLogger != null)
                {
                    handlerLogger.Write(true, e.ToString(), "Error");
                }
            }
            finally
            {
                logger.Debug("Finish '{0}'", info.HandlerId);
                sw.Stop();
                if (handlerLogger != null)
                {
                    handlerLogger.Write(true, String.Format("Handler ended at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", DateTimeOffset.Now, sw.Elapsed.TotalSeconds), "Message");
                }
            }
        }
    }
}
