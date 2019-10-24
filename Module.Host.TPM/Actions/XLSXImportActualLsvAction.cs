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
using Utility.LogWriter;
using Module.Persist.TPM.Utils;

namespace Module.Host.TPM.Actions {
    class XLSXImportActualLsvAction : FullXLSXImportAction
    {
        private ILogWriter handlerLogger;
        private Guid userId;
        private Guid roleId;
        private Guid calculationIdHandler;

        public XLSXImportActualLsvAction(FullImportSettings settings, Guid handlerId, Guid userId, Guid roleId) : base(settings)
        {
            handlerLogger = new FileLogWriter(handlerId.ToString());            
            AllowPartialApply = true;

            this.userId = userId;
            this.roleId = roleId;
        }

        protected override bool IsFilterSuitable(IEntity<Guid> rec, out IList<string> errors)
        {
            errors = new List<string>();
            bool success = true;

            try
            {
                ActualLSV importObj = rec as ActualLSV;
                if (importObj != null)
                {
                    // если нет промо с таким номером, то генерируем ошибку
                    using (DatabaseContext context = new DatabaseContext())
                    {
                        bool existPromo = context.Set<Promo>().Any(n => n.PromoStatus.SystemName.ToLower().IndexOf("finished") >= 0 && !n.Disabled && n.Number == importObj.Number);

                        if (!existPromo)
                        {
                            errors.Add("No Promo with ID: " + importObj.Number);
                            handlerLogger.Write(true, "No Promo with ID: " + importObj.Number, "Warning");
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
            IQueryable<ActualLSV> sourceRecords = records.Cast<ActualLSV>().AsQueryable();
            IList<Promo> query = GetQuery(context).ToList();
            IList<Promo> toUpdate = new List<Promo>();

            calculationIdHandler = Guid.NewGuid();

            foreach (ActualLSV record in sourceRecords)
            {
                Promo oldRecord = query.FirstOrDefault(n => n.Number == record.Number);
                if (oldRecord != null)
                {
                    // если промо не заблокировано, то блокируем его и пересчитываем
                    if (CalculationTaskManager.BlockPromo(oldRecord.Id, calculationIdHandler))
                    {
                        oldRecord.ActualPromoBaselineLSV = record.ActualPromoBaselineLSV;
                        oldRecord.ActualPromoLSV = record.ActualPromoLSV;
                        oldRecord.ActualPromoPostPromoEffectLSVW1 = record.ActualPromoPostPromoEffectLSVW1;
                        oldRecord.ActualPromoPostPromoEffectLSVW2 = record.ActualPromoPostPromoEffectLSVW2;
                        toUpdate.Add(oldRecord);
                    }
                    else
                    {
                        handlerLogger.Write(true, "Promo with ID " + oldRecord.Number + " already blocked for calculation" , "Warning");
                    }
                }
            }

            String formatStrRegularPromo = "UPDATE [Promo] SET ActualPromoBaselineLSV={0}, ActualPromoLSV={1}, ActualPromoPostPromoEffectLSVW1={2}, ActualPromoPostPromoEffectLSVW2={3} WHERE Id='{4}' \n";
            String formatStrInOutPromo = "UPDATE [Promo] SET ActualPromoLSV={0} WHERE Id='{1}' \n";

            foreach (IEnumerable<Promo> items in toUpdate.Partition(10000))
            {                
                string updateScript = "";

                foreach (Promo p in items)
                {
                    if (!p.InOut.HasValue || !p.InOut.Value)
                    {
                        updateScript += String.Format(formatStrRegularPromo,
                        p.ActualPromoBaselineLSV.HasValue ? p.ActualPromoBaselineLSV.Value.ToString() : "NULL",
                        p.ActualPromoLSV.HasValue ? p.ActualPromoLSV.Value.ToString() : "NULL",
                        p.ActualPromoPostPromoEffectLSVW1.HasValue ? p.ActualPromoPostPromoEffectLSVW1.Value.ToString() : "NULL",
                        p.ActualPromoPostPromoEffectLSVW2.HasValue ? p.ActualPromoPostPromoEffectLSVW2.Value.ToString() : "NULL",
                        p.Id);
                    }
                    else
                    {
                        updateScript += String.Format(formatStrInOutPromo,
                        p.ActualPromoLSV.HasValue ? p.ActualPromoLSV.Value.ToString() : "NULL",
                        p.Id);
                    }
                }

                context.Database.ExecuteSqlCommand(updateScript);
            }

            //Добавление изменений в историю
            List<Core.History.OperationDescriptor<Guid>> toHis = new List<Core.History.OperationDescriptor<Guid>>();
            foreach (var item in toUpdate)
            {
                toHis.Add(new Core.History.OperationDescriptor<Guid>() { Operation = OperationType.Updated, Entity = item });
            }
            
            CreateTaskCalculation(calculationIdHandler, toUpdate.Select(n => n.Id).ToArray(), context);

            return sourceRecords.Count();
        }


        private IEnumerable<Promo> GetQuery(DatabaseContext context)
        {
            IQueryable<Promo> query = context.Set<Promo>().AsNoTracking().Where(n => n.PromoStatus.SystemName.ToLower().IndexOf("finished") >= 0 && !n.Disabled);
            return query.ToList();
        }

        protected override void Fail()
        {

            CalculationTaskManager.UnLockPromoForHandler(calculationIdHandler);

            base.Fail();
        }

        /// <summary>
        /// Создать задачу на пересчет распределения BaseLine и ActualLSV, а также фактических параметров
        /// </summary>
        /// <param name="handlerId">ID обработчика</param>
        /// <param name="promoId">ID промо</param>
        private void CreateTaskCalculation(Guid handlerId, Guid[] promoIds, DatabaseContext context)
        {
            HandlerData data = new HandlerData();
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("PromoIds", promoIds, data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = handlerId,
                ConfigurationName = "PROCESSING",
                Description = "Calculate Actuals after change ActualLSV",
                Name = "Module.Host.TPM.Handlers.ActualLSVChangeHandler",
                ExecutionPeriod = null,
                CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                LastExecutionDate = null,
                NextExecutionDate = null,
                ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                UserId = userId,
                RoleId = roleId
            };

            handler.SetParameterData(data);
            context.LoopHandlers.Add(handler);
            context.SaveChanges();
        }
    }
}