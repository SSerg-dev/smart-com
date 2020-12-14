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
using AutoMapper.Internal;

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

            foreach (var item in sourceRecords)
            {
                var techCode_index = 2;
                var subCode_index = 3;
                var splitedBrandsegTechsub_code = item.BrandsegTechsub_code.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                var codesCount = splitedBrandsegTechsub_code.Length;

                if (codesCount == 4 || (!String.IsNullOrEmpty(item.Technology.SubBrand_code) && codesCount == 3))
                {
                    string subCode = null;
                    if (codesCount == 4)
                    {
                        subCode = splitedBrandsegTechsub_code.GetValue(subCode_index).ToString();
                    }
                    var technology = splitedBrandsegTechsub_code.GetValue(techCode_index).ToString();
                    if (subCode != item.Technology.SubBrand_code)
                    {
                        var changedTech = context.Set<Technology>().FirstOrDefault(tech => tech.Tech_code.Equals(technology) &&
                                                                                  tech.SubBrand_code.Equals(subCode));
                        if(changedTech != null)
                        {
                            item.Technology = changedTech;
                            item.TechnologyId = changedTech.Id;
                        }
                    }
                }
            }

            foreach (BrandTech newRecord in sourceRecords)
            {
                BrandTech oldRecord = query.FirstOrDefault(x => x.BrandId == newRecord.BrandId && 
                                                                x.TechnologyId == newRecord.TechnologyId &&
                                                                !x.Disabled);
                if (oldRecord == null)
                {
                    newRecord.Id = Guid.NewGuid();
                    toCreate.Add(newRecord);

                    // Так как BrandTech_code, BrandsegTechsub_code вычисляемые поля, для истории нужно заполнить их вручную 
                    if (!String.IsNullOrEmpty(newRecord.Brand.Brand_code) && 
                        !String.IsNullOrEmpty(newRecord.Brand.Segmen_code) && 
                        !String.IsNullOrEmpty(newRecord.Technology.Tech_code))
                    {
                        newRecord.BrandTech_code = String.Format("{0}-{1}-{2}", newRecord.Brand.Brand_code, newRecord.Brand.Segmen_code, newRecord.Technology.Tech_code);
                        if(!String.IsNullOrEmpty(newRecord.Technology.SubBrand_code) &&
                            !String.IsNullOrEmpty(newRecord.Technology.SubBrand))
                        newRecord.BrandsegTechsub_code = String.Format("{0}-{1}-{2}-{3}", newRecord.Brand.Brand_code, newRecord.Brand.Segmen_code, newRecord.Technology.Tech_code, newRecord.Technology.SubBrand_code);
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
                String formatStr = "INSERT INTO [DefaultSchemaSetting].[BrandTech] ([Id], [Disabled], [DeletedDate], [BrandId], [TechnologyId]) VALUES ('{0}', 0, NULL, '{1}', '{2}')";
                string insertScript = String.Join("\n", items.Select(y => String.Format(formatStr, y.Id, y.BrandId, y.TechnologyId)).ToList());

                context.ExecuteSqlCommand(insertScript);
            }

            ClientTreeBrandTechesController.FillClientTreeBrandTechTable(context);

            foreach (IEnumerable<BrandTech> items in toUpdate.Partition(10000))
            {
                string insertScript = generator.BuildUpdateScript(items);
                context.ExecuteSqlCommand(insertScript);
            }

            //Добавление в историю
            context.HistoryWriter.Write(toHisCreate, context.AuthManager.GetCurrentUser(), context.AuthManager.GetCurrentRole(), OperationType.Created);

            ClientTreeBrandTechesController.DisableNotActualClientTreeBrandTech(context);
            CreateCoefficientSI2SOHandler(toCreate.Select(b => b.BrandsegTechsub_code).ToList(), null, 1);

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
                    RunGroup = "CreateCoefficientSI2SO",
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