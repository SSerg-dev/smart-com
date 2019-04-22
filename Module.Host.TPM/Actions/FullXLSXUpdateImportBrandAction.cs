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
    class FullXLSXUpdateImportBrandAction : FullXLSXImportAction
    {
        public FullXLSXUpdateImportBrandAction(FullImportSettings settings) : base(settings) { }

        protected override int InsertDataToDatabase(IEnumerable<IEntity<Guid>> records, DatabaseContext context)
        {
            ScriptGenerator generator = GetScriptGenerator();
            IQueryable<Brand> sourceRecords = records.Cast<Brand>().AsQueryable();
            IList<Brand> query = GetQuery(context).ToList();
            IList<Brand> toCreate = new List<Brand>();
            IList<Brand> toUpdate = new List<Brand>();

            foreach (Brand newRecord in sourceRecords)
            {
                Brand oldRecord = query.FirstOrDefault(x => x.Name == newRecord.Name && !x.Disabled);
                if (oldRecord == null)
                {
                    toCreate.Add(newRecord);
                }
                else
                {
                    oldRecord.Name = newRecord.Name;

                    toUpdate.Add(oldRecord);
                }
            }

            foreach (IEnumerable<Brand> items in toCreate.Partition(10000))
            {
                string insertScript = generator.BuildInsertScript(items);
                context.Database.ExecuteSqlCommand(insertScript);
            }

            foreach (IEnumerable<Brand> items in toUpdate.Partition(10000))
            {
                string insertScript = generator.BuildUpdateScript(items);
                context.Database.ExecuteSqlCommand(insertScript);
            }

            return sourceRecords.Count();
        }

        private IEnumerable<Brand> GetQuery(DatabaseContext context)
        {
            IQueryable<Brand> query = context.Set<Brand>().AsNoTracking();
            return query.ToList();
        }
    }
}