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

namespace Module.Host.TPM.Actions
{
    class FullXLSXUpdateImportProductAction : FullXLSXImportAction
    {
        public FullXLSXUpdateImportProductAction(FullImportSettings settings) : base(settings) { }

        protected override int InsertDataToDatabase(IEnumerable<IEntity<Guid>> records, DatabaseContext context)
        {
            ScriptGenerator generator = GetScriptGenerator();
            IQueryable<Product> sourceRecords = records.Cast<Product>().AsQueryable();
            IList<Product> query = GetQuery(context).ToList();
            IList<Product> toCreate = new List<Product>();
            IList<Product> toUpdate = new List<Product>();

            foreach (Product newRecord in sourceRecords)
            {
                Product oldRecord = query.FirstOrDefault(x => x.ZREP == newRecord.ZREP && !x.Disabled);
                if (oldRecord == null)
                {
                    toCreate.Add(newRecord);
                }
                else
                {
                    oldRecord.EAN = newRecord.EAN;
                    oldRecord.ProductRU = newRecord.ProductRU;
                    oldRecord.ProductEN = newRecord.ProductEN;
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

                    toUpdate.Add(oldRecord);
                }
            }

            foreach (IEnumerable<Product> items in toCreate.Partition(10000))
            {
                string insertScript = generator.BuildInsertScript(items);
                context.Database.ExecuteSqlCommand(insertScript);
            }

            foreach (IEnumerable<Product> items in toUpdate.Partition(10000))
            {
                string insertScript = generator.BuildUpdateScript(items);
                context.Database.ExecuteSqlCommand(insertScript);
            }

            return sourceRecords.Count();
        }

        private IEnumerable<Product> GetQuery(DatabaseContext context)
        {
            IQueryable<Product> query = context.Set<Product>().AsNoTracking();
            return query.ToList();
        }
    }
}