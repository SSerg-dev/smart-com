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
using Core.History;
using Module.Persist.TPM.Utils;

namespace Module.Host.TPM.Actions {
    class FullXLSXImportPromoProductFromTLCAction : FullXLSXImportAction {
        /// <summary>
        /// Id промо для которого загружается Actuals
        /// </summary>
        private Guid promoId;
        private Guid userId;
        private Guid roleId;

        public FullXLSXImportPromoProductFromTLCAction(FullImportSettings settings, Guid promoId, Guid userId, Guid roleId) : base(settings) {
            this.promoId = promoId;
            this.userId = userId;
            this.roleId = roleId;
        }

        protected override int InsertDataToDatabase(IEnumerable<IEntity<Guid>> records, DatabaseContext context) {
            ScriptGenerator generator = GetScriptGenerator();
            double? ActualPromoLSVByCompensation = 0;
            IQueryable<PromoProduct> sourceRecords = records.Cast<PromoProduct>().AsQueryable();
            var promo = context.Set<Promo>().Where(x => x.Id == promoId && !x.Disabled).FirstOrDefault();

            //Добавление изменений в историю
            foreach (PromoProduct item in records) {
               ActualPromoLSVByCompensation += (item.ActualProductSellInPrice * item.ActualProductPCQty);
            }
            promo.ActualPromoLSVByCompensation = ActualPromoLSVByCompensation;

            // Обновляем фактический значения
            CreateTaskCalculateActual(promoId);

            return sourceRecords.Count();
        }

        /// <summary>
        /// Создание отложенной задачи, выполняющей расчет фактических параметров продуктов и промо
        /// </summary>
        /// <param name="promoId">ID промо</param>
        private void CreateTaskCalculateActual(Guid promoId) {
            // к этому моменту промо уже заблокировано

            using (DatabaseContext context = new DatabaseContext()) {
                HandlerData data = new HandlerData();
                HandlerDataHelper.SaveIncomingArgument("PromoId", promoId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler() {
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

                BlockedPromo bp = context.Set<BlockedPromo>().First(n => n.PromoBlockedStatusId == promoId && !n.Disabled);
                bp.HandlerId = handler.Id;

                handler.SetParameterData(data);
                context.LoopHandlers.Add(handler);
                context.SaveChanges();
            }
        }

        private IEnumerable<PromoProduct> GetQuery(DatabaseContext context) {
            IQueryable<PromoProduct> query = context.Set<PromoProduct>().AsNoTracking();
            return query.ToList();
        }

        protected override void Fail() {
            // в случае ошибки разблокируем промо
            CalculationTaskManager.UnLockPromo(promoId);

            base.Fail();
        }
    }
}