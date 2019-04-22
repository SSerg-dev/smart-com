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
    class FullXLSXUpdateImportTechnologyAction : FullXLSXImportAction
    {
        public FullXLSXUpdateImportTechnologyAction(FullImportSettings settings) : base(settings) { }

        protected override int InsertDataToDatabase(IEnumerable<IEntity<Guid>> records, DatabaseContext context)
        {
            ScriptGenerator generator = GetScriptGenerator();
            IQueryable<Technology> sourceRecords = records.Cast<Technology>().AsQueryable();
            IList<Technology> query = GetQuery(context).ToList();
            IList<Technology> toCreate = new List<Technology>();
            IList<Technology> toUpdate = new List<Technology>();

            foreach (Technology newRecord in sourceRecords)
            {
                Technology oldRecord = query.FirstOrDefault(x => x.Name == newRecord.Name && !x.Disabled);
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

            foreach (IEnumerable<Technology> items in toCreate.Partition(10000))
            {
                string insertScript = generator.BuildInsertScript(items);
                context.Database.ExecuteSqlCommand(insertScript);
            }

            foreach (IEnumerable<Technology> items in toUpdate.Partition(10000))
            {
                string insertScript = generator.BuildUpdateScript(items);
                context.Database.ExecuteSqlCommand(insertScript);
            }

            return sourceRecords.Count();
        }

        private IEnumerable<Technology> GetQuery(DatabaseContext context)
        {
            IQueryable<Technology> query = context.Set<Technology>().AsNoTracking();
            return query.ToList();
        }
    }
}