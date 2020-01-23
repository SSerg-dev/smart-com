using Interfaces.Implementation.Import.FullImport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Data;
using Persist;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using Core.Extensions;
using Looper.Parameters;
using Core.History;
using Module.Frontend.TPM.Util;

namespace Module.Host.TPM.Actions
{
    class FullXLSXUpdateImportProductAction : FullXLSXImportAction
    {
        public FullXLSXUpdateImportProductAction(FullImportSettings settings) : base(settings) { }

        protected override int InsertDataToDatabase(IEnumerable<IEntity<Guid>> records, DatabaseContext context)
        {
            NoGuidGeneratingScriptGenerator generatorCreate = new NoGuidGeneratingScriptGenerator(typeof (Product), false);
            ScriptGenerator generatorUpdate = new ScriptGenerator(typeof(Product));
            IQueryable<Product> sourceRecords = records.Cast<Product>().AsQueryable();
            IList<Product> query = GetQuery(context).ToList();
            IList<Product> toCreate = new List<Product>();
            IList<Product> toUpdate = new List<Product>();
            List<Tuple<IEntity<Guid>, IEntity<Guid>>> toHisCreate = new List<Tuple<IEntity<Guid>, IEntity<Guid>>>();
            List<Tuple<IEntity<Guid>, IEntity<Guid>>> toHisUpdate = new List<Tuple<IEntity<Guid>, IEntity<Guid>>>();

            foreach (Product newRecord in sourceRecords)
            {
                Product oldRecord = query.FirstOrDefault(x => x.ZREP == newRecord.ZREP && !x.Disabled);
                var oldRecordCopy = oldRecord;
                if (oldRecord == null)
                {
                    newRecord.Id = Guid.NewGuid();
                    toCreate.Add(newRecord);
                    toHisCreate.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(null, newRecord));
                }
                else
                {
                    newRecord.Id = oldRecord.Id;

                    oldRecord.EAN_Case = newRecord.EAN_Case;
                    oldRecord.EAN_PC = newRecord.EAN_PC;
                    oldRecord.ProductEN = newRecord.ProductEN;
					oldRecord.Brand = newRecord.Brand;
					oldRecord.Brand_code = newRecord.Brand_code;
					oldRecord.Technology = newRecord.Technology;
					oldRecord.Tech_code = newRecord.Tech_code;
					oldRecord.BrandTech = newRecord.BrandTech;
					oldRecord.BrandTech_code = newRecord.BrandTech_code;
					oldRecord.Segmen_code = newRecord.Segmen_code;
					oldRecord.BrandsegTech_code = newRecord.BrandsegTech_code;
                    oldRecord.Brandsegtech = newRecord.Brandsegtech;
                    oldRecord.BrandFlagAbbr = newRecord.BrandFlagAbbr;
                    oldRecord.BrandFlag = newRecord.BrandFlag;
                    oldRecord.SubmarkFlag = newRecord.SubmarkFlag;
                    oldRecord.IngredientVariety = newRecord.IngredientVariety;
                    oldRecord.ProductCategory = newRecord.ProductCategory;
                    oldRecord.ProductType = newRecord.ProductType;
                    oldRecord.MarketSegment = newRecord.MarketSegment;
                    oldRecord.SupplySegment = newRecord.SupplySegment;
                    oldRecord.FunctionalVariety = newRecord.FunctionalVariety;
                    oldRecord.Size = newRecord.Size;
                    oldRecord.BrandEssence = newRecord.BrandEssence;
                    oldRecord.PackType = newRecord.PackType;
                    oldRecord.GroupSize = newRecord.GroupSize;
                    oldRecord.TradedUnitFormat = newRecord.TradedUnitFormat;
                    oldRecord.ConsumerPackFormat = newRecord.ConsumerPackFormat;
                    oldRecord.UOM_PC2Case = newRecord.UOM_PC2Case;
					oldRecord.Division = newRecord.Division;

					toHisUpdate.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(oldRecordCopy, oldRecord));
                    toUpdate.Add(oldRecord);
                }
            }

            foreach (IEnumerable<Product> items in toCreate.Partition(10000))
            {
                string insertScript = generatorCreate.BuildInsertScript(items);
                context.Database.ExecuteSqlCommand(insertScript);
            }

            foreach (IEnumerable<Product> items in toUpdate.Partition(10000))
            {
                string insertScript = generatorUpdate.BuildUpdateScript(items);
                context.Database.ExecuteSqlCommand(insertScript);
            }
            //Добавление записей в историю
            context.HistoryWriter.Write(toHisCreate, context.AuthManager.GetCurrentUser(), context.AuthManager.GetCurrentRole(), OperationType.Created);
            context.HistoryWriter.Write(toHisUpdate, context.AuthManager.GetCurrentUser(), context.AuthManager.GetCurrentRole(), OperationType.Updated);

            context.SaveChanges();
            return sourceRecords.Count();
        }

        private IEnumerable<Product> GetQuery(DatabaseContext context)
        {
            IQueryable<Product> query = context.Set<Product>().AsNoTracking();
            return query.ToList();
        }
    }
}