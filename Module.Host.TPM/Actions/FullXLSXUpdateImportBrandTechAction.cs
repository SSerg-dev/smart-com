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
using Looper.Core;
using Persist.Model;
using Core.History;
using Module.Persist.TPM.Utils;
using Module.Host.TPM.Handlers;
using Core.Security.Models;
using Core.Security;

namespace Module.Host.TPM.Actions
{
    class FullXLSXUpdateImportBrandTechAction : FullXLSXImportAction
    {
        private Guid roleId;
        private Guid userId;

        public FullXLSXUpdateImportBrandTechAction(FullImportSettings settings, Guid userId, Guid roleId) : base(settings) 
        {
            this.roleId = roleId;
            this.userId = userId;
        }

        protected override int InsertDataToDatabase(IEnumerable<IEntity<Guid>> records, DatabaseContext context)
        {
            ScriptGenerator generator = GetScriptGenerator();
            IQueryable<BrandTech> sourceRecords = records.Cast<BrandTech>().AsQueryable();
            IList<BrandTech> query = GetQuery(context).ToList();
            IList<BrandTech> toCreate = new List<BrandTech>();
            IList<BrandTech> toUpdate = new List<BrandTech>();
            var toHisCreate = new List<Tuple<IEntity<Guid>, IEntity<Guid>>>();

            foreach (BrandTech newRecord in sourceRecords)
            {
                BrandTech oldRecord = query.FirstOrDefault(x => x.BrandId == newRecord.BrandId && x.TechnologyId == newRecord.TechnologyId && !x.Disabled);
                if (oldRecord == null)
                {
                    newRecord.Id = Guid.NewGuid();
                    toCreate.Add(newRecord);

                    // Так как BrandTech_code вычисляемое поле, для истории нужно заполнить его вручную 
                    if (!String.IsNullOrEmpty(newRecord.Brand.Brand_code) && 
                        !String.IsNullOrEmpty(newRecord.Brand.Segmen_code) && 
                        !String.IsNullOrEmpty(newRecord.Technology.Tech_code))
                    {
                        newRecord.BrandTech_code = String.Format("{0}-{1}-{2}", newRecord.Brand.Brand_code, newRecord.Brand.Segmen_code, newRecord.Technology.Tech_code);
                    }

                    toHisCreate.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(null, newRecord));
                }
                else
                {
                    //Запись не изменяется
                    //oldRecord.BrandId = newRecord.BrandId;
                    //oldRecord.TechnologyId = newRecord.TechnologyId;
                    //toUpdate.Add(oldRecord);
                }
            }

            foreach (IEnumerable<BrandTech> items in toCreate.Partition(10000))
            {
                String formatStr = "INSERT INTO [BrandTech] ([Id], [Disabled], [DeletedDate], [BrandId], [TechnologyId]) VALUES ('{0}', 0, NULL, '{1}', '{2}')";
                string insertScript = String.Join("\n", items.Select(y => String.Format(formatStr, y.Id, y.BrandId, y.TechnologyId)).ToList());

                context.Database.ExecuteSqlCommand(insertScript);
            }

            ClientTreeBrandTechesController.FillClientTreeBrandTechTable(context);

            foreach (IEnumerable<BrandTech> items in toUpdate.Partition(10000))
            {
                string insertScript = generator.BuildUpdateScript(items);
                context.Database.ExecuteSqlCommand(insertScript);
            }

            //Добавление в историю
            context.HistoryWriter.Write(toHisCreate, context.AuthManager.GetCurrentUser(), context.AuthManager.GetCurrentRole(), OperationType.Created);

            ClientTreeBrandTechesController.DisableNotActualClientTreeBrandTech(context);
            CreateCoefficientSI2SOHandler(toCreate.Select(b => b.BrandTech_code).ToList(), null, 1);

            return sourceRecords.Count();
        }

        private IEnumerable<BrandTech> GetQuery(DatabaseContext context)
        {
            IQueryable<BrandTech> query = context.Set<BrandTech>().AsNoTracking();
            return query.ToList();
        }

        private void CreateCoefficientSI2SOHandler(List<string> brandTechCode, string demandCode, double cValue)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                HandlerData data = new HandlerData();
                HandlerDataHelper.SaveIncomingArgument("brandTechCode", brandTechCode, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("demandCode", demandCode, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("cValue", cValue, data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Adding new records for coefficients SI/SO",
                    Name = "Module.Host.TPM.Handlers.CreateCoefficientSI2SOHandler",
                    ExecutionPeriod = null,
                    CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
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
}