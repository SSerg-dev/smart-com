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
    class FullXLSXUpdateImportDemandAction : FullXLSXImportAction
    {
        public FullXLSXUpdateImportDemandAction(FullImportSettings settings) : base(settings) { }

        protected override int InsertDataToDatabase(IEnumerable<IEntity<Guid>> records, DatabaseContext context)
        {
            ScriptGenerator generator = GetScriptGenerator();
            DemandComparer cmp = new DemandComparer();
            IQueryable<Demand> sourceRecords = records.Cast<Demand>().AsQueryable();
            IList<Demand> query = GetQuery(context).ToList();
            IList<Demand> toCreate = new List<Demand>();
            IList<Demand> toUpdate = new List<Demand>();

            foreach (Demand newRecord in sourceRecords)
            {
                Demand oldRecord = query.FirstOrDefault(x => cmp.Equals(x, newRecord));
                if (oldRecord == null)
                {
                    toCreate.Add(newRecord);
                }
                else
                {
                    oldRecord.ClientId = newRecord.ClientId;
                    oldRecord.BrandId = newRecord.BrandId;
                    oldRecord.BrandTechId = newRecord.BrandTechId;
                    oldRecord.Name = newRecord.Name;
                    oldRecord.StartDate = newRecord.StartDate;
                    oldRecord.EndDate = newRecord.EndDate;
                    oldRecord.DispatchesStart = newRecord.DispatchesStart;
                    oldRecord.DispatchesEnd = newRecord.DispatchesEnd;
                    oldRecord.PlanBaseline = newRecord.PlanBaseline;
                    oldRecord.PlanDuration = newRecord.PlanDuration;
                    oldRecord.PlanUplift = newRecord.PlanUplift;
                    oldRecord.PlanIncremental = newRecord.PlanIncremental;
                    oldRecord.PlanActivity = newRecord.PlanActivity;
                    oldRecord.PlanSteal = newRecord.PlanSteal;
                    oldRecord.FactBaseline = newRecord.FactBaseline;
                    oldRecord.FactDuration = newRecord.FactDuration;
                    oldRecord.FactUplift = newRecord.FactUplift;
                    oldRecord.FactIncremental = newRecord.FactIncremental;
                    oldRecord.FactActivity = newRecord.FactActivity;
                    oldRecord.FactSteal = newRecord.FactSteal;

                    toUpdate.Add(oldRecord);
                }
            }

            foreach (IEnumerable<Demand> items in toCreate.Partition(10000))
            {
                string insertScript = generator.BuildInsertScript(items);
                context.Database.ExecuteSqlCommand(insertScript);
            }

            foreach (IEnumerable<Demand> items in toUpdate.Partition(10000))
            {
                string insertScript = generator.BuildUpdateScript(items);
                context.Database.ExecuteSqlCommand(insertScript);
            }

            return sourceRecords.Count();
        }

        private IEnumerable<Demand> GetQuery(DatabaseContext context)
        {
            IQueryable<Demand> query = context.Set<Demand>().AsNoTracking();
            return query.ToList();
        }
    }
}