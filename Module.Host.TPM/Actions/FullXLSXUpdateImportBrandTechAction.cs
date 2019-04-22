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
    class FullXLSXUpdateImportBrandTechAction : FullXLSXImportAction
    {
        public FullXLSXUpdateImportBrandTechAction(FullImportSettings settings) : base(settings) { }

        protected override int InsertDataToDatabase(IEnumerable<IEntity<Guid>> records, DatabaseContext context)
        {
            ScriptGenerator generator = GetScriptGenerator();
            IQueryable<BrandTech> sourceRecords = records.Cast<BrandTech>().AsQueryable();
            IList<BrandTech> query = GetQuery(context).ToList();
            IList<BrandTech> toCreate = new List<BrandTech>();
            IList<BrandTech> toUpdate = new List<BrandTech>();

            foreach (BrandTech newRecord in sourceRecords)
            {
                BrandTech oldRecord = query.FirstOrDefault(x => x.BrandId == newRecord.BrandId && x.TechnologyId == newRecord.TechnologyId && !x.Disabled);
                if (oldRecord == null)
                {
                    toCreate.Add(newRecord);
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
                string insertScript = String.Join("\n", items.Select(y => String.Format(formatStr, Guid.NewGuid(), y.BrandId, y.TechnologyId)).ToList());
                context.Database.ExecuteSqlCommand(insertScript);
            }

            foreach (IEnumerable<BrandTech> items in toUpdate.Partition(10000))
            {
                string insertScript = generator.BuildUpdateScript(items);
                context.Database.ExecuteSqlCommand(insertScript);
            }

            return sourceRecords.Count();
        }

        private IEnumerable<BrandTech> GetQuery(DatabaseContext context)
        {
            IQueryable<BrandTech> query = context.Set<BrandTech>().AsNoTracking();
            return query.ToList();
        }
    }
}