using Interfaces.Implementation.Import.FullImport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Data;
using Persist;
using Persist.ScriptGenerator;
using Module.Persist.TPM.Model.TPM;
using Looper.Parameters;
using Looper.Core;
using Persist.Model;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Utils;
using System.Data.Entity.Migrations;

namespace Module.Host.TPM.Actions
{
    class FullXLSXImportPromoProductAction : FullXLSXImportAction
    {
        /// <summary>
        /// Id промо для которого загружается Actuals
        /// </summary>
        private Guid promoId;
        private Guid userId;
        private Guid roleId;

        public FullXLSXImportPromoProductAction(FullImportSettings settings, Guid promoId, Guid userId, Guid roleId) : base(settings)
        {
            this.promoId = promoId;
            this.userId = userId;
            this.roleId = roleId;
        }

        protected override bool IsFilterSuitable(IEntity<Guid> rec, out IList<string> errors)
        {
            errors = new List<string>();
            bool success = true;

            try
            {
                PromoProduct importObj = rec as PromoProduct;
                if (importObj != null)
                {
                    // если к промо не прикреплен продукт с указанным EAN_Case выдаем ошибку
                    using (DatabaseContext context = new DatabaseContext())
                    {
                        bool existPromoProduct = context.Set<PromoProduct>().Any(n => n.EAN_PC == importObj.EAN_PC && n.PromoId == promoId && !n.Disabled);

                        if (!existPromoProduct)
                        {
                            errors.Add("No product attached to promo with EAN PC " + importObj.EAN_PC);
                            success = false;
                        }

                        importObj.ActualProductUOM = "PC";
                    }
                }
            }
            catch
            {
                // если что-то пошло не так
                success = false;
            }

            return success;
        }

        protected override int InsertDataToDatabase(IEnumerable<IEntity<Guid>> records, DatabaseContext context)
        {
            ScriptGenerator generator = GetScriptGenerator();
            IList<PromoProduct> query = GetQuery(context).ToList();

            List<PromoProduct> toUpdate = new List<PromoProduct>();
            List<Tuple<IEntity<Guid>, IEntity<Guid>>> toHisUpdate = new List<Tuple<IEntity<Guid>, IEntity<Guid>>>();
            Promo promo = context.Set<Promo>().Where(x => x.Id == promoId && !x.Disabled).FirstOrDefault();

            if (promo != null)
            {
                if (!promo.InOut.HasValue || !promo.InOut.Value)
                {
                    foreach (PromoProduct newRecord in records)
                    {
                        // выбор продуктов с ненулевым BaseLine (проверка Baseline ниже)
                        var productsWithRealBaseline = query.Where(x => x.EAN_PC == newRecord.EAN_PC && x.PromoId == promoId && !x.Disabled).ToList();

                        if (productsWithRealBaseline != null && productsWithRealBaseline.Count() > 0)
                        {
                            //распределение импортируемого количества пропорционально PlanProductBaselineLSV
                            var sumBaseline = productsWithRealBaseline.Sum(x => x.PlanProductBaselineLSV);
                            List<Tuple<IEntity<Guid>, IEntity<Guid>>> promoProductsToUpdateHis = new List<Tuple<IEntity<Guid>, IEntity<Guid>>>();
                            List<PromoProduct> promoProductsToUpdate = new List<PromoProduct>();
                            bool isRealBaselineExist = false;
                            foreach (var p in productsWithRealBaseline)
                            {
                                var newRecordClone = ClonePromoProduct(newRecord);
                                p.ActualProductUOM = "PC";
                                // проверка Baseline (исправляет ActualProductPCQty)
                                if (p.PlanProductBaselineLSV != null && p.PlanProductBaselineLSV != 0)
                                {
                                    if (p.PlanProductIncrementalLSV != 0 && sumBaseline != 0)
                                    {
                                        p.ActualProductPCQty = (int?)(newRecord.ActualProductPCQty / sumBaseline * p.PlanProductBaselineLSV);
                                    }
                                    newRecord.ActualProductPCQty = p.ActualProductPCQty;
                                    isRealBaselineExist = true;
                                }
                                else
                                {
                                    p.ActualProductPCQty = null;
                                    newRecordClone.ActualProductPCQty = null;
                                }

                                promoProductsToUpdateHis.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(p, newRecordClone));
                                promoProductsToUpdate.Add(p);
                            }

                            if (isRealBaselineExist)
                            {
                                var differenceActualProductPCQty = newRecord.ActualProductPCQty - promoProductsToUpdate.Sum(x => x.ActualProductPCQty);
                                if (differenceActualProductPCQty.HasValue && differenceActualProductPCQty != 0)
                                {
                                    var firstRealBaselineItem = promoProductsToUpdate.FirstOrDefault(x => x.ActualProductPCQty != null);
                                    firstRealBaselineItem.ActualProductPCQty += differenceActualProductPCQty;
                                }
                            }
                            else
                            {
                                //TODO: вывод предупреждения
                                //если не найдено продуктов с ненулевым basline, просто записываем импортируемое количество в первый попавшийся продукт, чтобы сохранилось
                                PromoProduct oldRecord = query.FirstOrDefault(x => x.EAN_PC == newRecord.EAN_PC && x.PromoId == promoId && !x.Disabled);
                                if (oldRecord != null)
                                {
                                    oldRecord.ActualProductUOM = "PC";
                                    oldRecord.ActualProductPCQty = newRecord.ActualProductPCQty;
                                    toUpdate.Add(oldRecord);
                                    toHisUpdate.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(oldRecord, newRecord));
                                }
                            }

                            toUpdate.AddRange(promoProductsToUpdate);
                            toHisUpdate.AddRange(promoProductsToUpdateHis);
                        }
                    }
                }
                else
                {
                    foreach (PromoProduct newRecord in records)
                    {
                        //в случае inout промо выбираем продукты с ненулевой ценой PlanProductPCPrice, которая подбирается из справочника IncrementalPromo
                        var productsWithRealPCPrice = query.Where(x => x.EAN_PC == newRecord.EAN_PC && x.PromoId == promoId && !x.Disabled).ToList();

                        if (productsWithRealPCPrice != null && productsWithRealPCPrice.Count() > 0)
                        {
                            //распределение импортируемого количества пропорционально PlanProductIncrementalLSV
                            var sumIncremental = productsWithRealPCPrice.Sum(x => x.PlanProductIncrementalLSV);
                            List<PromoProduct> promoProductsToUpdate = new List<PromoProduct>();
                            List<Tuple<IEntity<Guid>, IEntity<Guid>>> promoProductsToUpdateHis = new List<Tuple<IEntity<Guid>, IEntity<Guid>>>();
                            bool isRealPCPriceExist = false;
                            foreach (var p in productsWithRealPCPrice)
                            {
                                var newRecordClone = ClonePromoProduct(newRecord);
                                p.ActualProductUOM = "PC";
                                // проверка Price (исправляет ActualProductPCQty)
                                if (p.PlanProductPCPrice != null && p.PlanProductPCPrice != 0)
                                {
                                    if(p.PlanProductIncrementalLSV != 0 && sumIncremental != 0)
                                    {
                                        p.ActualProductPCQty = (int?)(newRecord.ActualProductPCQty / sumIncremental * p.PlanProductIncrementalLSV);
                                    }
                                    newRecord.ActualProductPCQty = p.ActualProductPCQty;
                                    isRealPCPriceExist = true;
                                }
                                else
                                {
                                    p.ActualProductPCQty = null;
                                    newRecordClone.ActualProductPCQty = null;
                                }

                                promoProductsToUpdateHis.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(p, newRecordClone));
                                promoProductsToUpdate.Add(p);
                            }

                            if (isRealPCPriceExist)
                            {
                                var differenceActualProductPCQty = newRecord.ActualProductPCQty - promoProductsToUpdate.Sum(x => x.ActualProductPCQty);
                                if (differenceActualProductPCQty.HasValue && differenceActualProductPCQty != 0)
                                {
                                    var firstRealPriceItem = promoProductsToUpdate.FirstOrDefault(x => x.ActualProductPCQty != null);
                                    firstRealPriceItem.ActualProductPCQty += differenceActualProductPCQty;
                                }
                            }
                            else
                            {
                                //TODO: вывод предупреждения
                                //если не найдено продуктов с ненулевым basline, просто записываем импортируемое количество в первый попавшийся продукт, чтобы сохранилось
                                PromoProduct oldRecord = query.FirstOrDefault(x => x.EAN_PC == newRecord.EAN_PC && x.PromoId == promoId && !x.Disabled);
                                if (oldRecord != null)
                                {
                                    oldRecord.ActualProductUOM = "PC";
                                    oldRecord.ActualProductPCQty = newRecord.ActualProductPCQty;
                                    toUpdate.Add(oldRecord);
                                }
                            }

                            toUpdate.AddRange(promoProductsToUpdate);
                        }
                    }
                }
            }

            foreach(PromoProduct item in toUpdate)
            {
                context.Set<PromoProduct>().AddOrUpdate(item);
            }
            context.SaveChanges();
            // Обновляем фактические значения
            CreateTaskCalculateActual(promoId);

            return records.Count();
        }

        /// <summary>
        /// Создание клона продукта
        /// </summary>
        /// <returns></returns>
        private PromoProduct ClonePromoProduct(PromoProduct promoProduct)
        {
            var promoProductClone = new PromoProduct();
            promoProductClone.ActualProductPCQty = promoProduct.ActualProductPCQty;
            promoProductClone.EAN_PC = promoProduct.EAN_PC;
            promoProductClone.ActualProductUOM = promoProduct.ActualProductUOM;
            return promoProductClone;
        }

        /// <summary>
        /// Создание отложенной задачи, выполняющей расчет фактических параметров продуктов и промо
        /// </summary>
        /// <param name="promoId">ID промо</param>
        private void CreateTaskCalculateActual(Guid promoId)
        {
            // к этому моменту промо уже заблокировано

            using (DatabaseContext context = new DatabaseContext())
            {
                HandlerData data = new HandlerData();
                HandlerDataHelper.SaveIncomingArgument("PromoId", promoId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("needRedistributeLSV", true, data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Calculate actual parameters",
                    Name = "Module.Host.TPM.Handlers.CalculateActualParamatersHandler",
                    ExecutionPeriod = null,
                    CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                    LastExecutionDate = null,
                    NextExecutionDate = null,
                    ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                    UserId = userId,
                    RoleId = roleId
                };

                BlockedPromo bp = context.Set<BlockedPromo>().First(n => n.PromoId == promoId && !n.Disabled);
                bp.HandlerId = handler.Id;

                handler.SetParameterData(data);
                context.LoopHandlers.Add(handler);
                context.SaveChanges();
            }
        }

        private IEnumerable<PromoProduct> GetQuery(DatabaseContext context)
        {
            IQueryable<PromoProduct> query = context.Set<PromoProduct>().AsNoTracking();
            return query.ToList();
        }

        protected override void Fail()
        {
            // в случае ошибки разблокируем промо
            CalculationTaskManager.UnLockPromo(promoId);

            base.Fail();
        }
    }
}