using Interfaces.Implementation.Import.FullImport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Data;
using Persist;
using Persist.ScriptGenerator;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using Core.Extensions;
using Looper.Parameters;
using Module.Frontend.TPM.Controllers;
using Core.Security.Models;
using Looper.Core;
using Persist.Model;
using Module.Persist.TPM.CalculatePromoParametersModule;

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
                    // если к промо не прикреплен продукт с указанным EAN выдаем ошибку
                    using (DatabaseContext context = new DatabaseContext())
                    {
                        bool existPromoProduct = context.Set<PromoProduct>().Any(n => n.EAN == importObj.EAN && n.PromoId == promoId && !n.Disabled);

                        if (!existPromoProduct)
                        {
                            errors.Add("No product attached to promo with EAN " + importObj.EAN);
                            success = false;
                        }

                        // проверяем какие единицы измерения указаны
                        if (importObj.ActualProductUOM.ToLower() == "pc")
                        {
                            importObj.ActualProductUOM = "PC";
                            if (!importObj.ActualProductPCQty.HasValue)
                            {
                                errors.Add("Actual Product PC Qty is required");
                                success = false;
                            }
                        }
                        else if (importObj.ActualProductUOM.ToLower() == "case")
                        {
                            importObj.ActualProductUOM = "Case";
                            if (!importObj.ActualProductQty.HasValue)
                            {
                                errors.Add("Actual Product Qty is required");
                                success = false;
                            }
                        }
                        else
                        {
                            errors.Add("Actual Product UOM: value must be PC or Case" + importObj.EAN);
                            success = false;
                        }
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
            IQueryable<PromoProduct> sourceRecords = records.Cast<PromoProduct>().AsQueryable();
            IList<PromoProduct> query = GetQuery(context).ToList();
            IList<PromoProduct> toUpdate = new List<PromoProduct>();

            foreach (PromoProduct newRecord in sourceRecords)
            {
                PromoProduct oldRecord = query.FirstOrDefault(x => x.EAN == newRecord.EAN && x.PromoId == promoId && !x.Disabled);
                if (oldRecord != null)
                {                    
                    oldRecord.ActualProductUOM = newRecord.ActualProductUOM;
                    oldRecord.ActualProductShelfPrice = newRecord.ActualProductShelfPrice;
                    oldRecord.ActualProductPCLSV = newRecord.ActualProductPCLSV;

                    // если указаны штуки, то рассчитываем кейсы, иначе наоборот
                    if (oldRecord.ActualProductUOM == "PC")
                    {
                        oldRecord.ActualProductPCQty = newRecord.ActualProductPCQty;
                        oldRecord.ActualProductQty = newRecord.ActualProductPCQty / (oldRecord.Product.UOM_PC2Case * 1.0);
                    }
                    else if (oldRecord.ActualProductUOM == "Case")
                    {
                        oldRecord.ActualProductQty = newRecord.ActualProductQty;
                        oldRecord.ActualProductPCQty = (int)(newRecord.ActualProductQty * oldRecord.Product.UOM_PC2Case);                        
                    }

                    toUpdate.Add(oldRecord);
                }
            }

            foreach (IEnumerable<PromoProduct> items in toUpdate.Partition(10000))
            {
                string insertScript = generator.BuildUpdateScript(items);
                context.Database.ExecuteSqlCommand(insertScript);
            }

            // Обновляем фактический значения
            CreateTaskCalculateActual(toUpdate.First().Promo.Id);

            return sourceRecords.Count();
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

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Calculate actual parameters",
                    Name = "Module.Host.TPM.Handlers.CalculateActualParamatersHandler",
                    ExecutionPeriod = null,
                    CreateDate = DateTimeOffset.Now,
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