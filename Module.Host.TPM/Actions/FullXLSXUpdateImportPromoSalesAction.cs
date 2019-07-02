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
    class FullXLSXUpdateImportPromoSalesAction : FullXLSXImportAction
    {
        public FullXLSXUpdateImportPromoSalesAction(FullImportSettings settings) : base(settings) { }

        protected override int InsertDataToDatabase(IEnumerable<IEntity<Guid>> records, DatabaseContext context)
        {
            ScriptGenerator generator = GetScriptGenerator();
            PromoSalesComparer cmp = new PromoSalesComparer();
            IQueryable<PromoSales> sourceRecords = records.Cast<PromoSales>().AsQueryable();
            IList<PromoSales> query = GetQuery(context).ToList();
            IList<PromoSales> toCreate = new List<PromoSales>();
            IList<PromoSales> toUpdate = new List<PromoSales>();

            foreach (PromoSales newRecord in sourceRecords)
            {
                PromoSales oldRecord = query.FirstOrDefault(x => cmp.Equals(x, newRecord));
                if (oldRecord == null)
                {
                    toCreate.Add(newRecord);
                }
                else
                {
                    oldRecord.ClientId = newRecord.ClientId;
                    oldRecord.BrandId = newRecord.BrandId;
                    oldRecord.BrandTechId = newRecord.BrandTechId;
                    oldRecord.PromoStatusId = newRecord.PromoStatusId;
                    oldRecord.MechanicId = newRecord.MechanicId;
                    oldRecord.Name = newRecord.Name;
                    oldRecord.StartDate = newRecord.StartDate;
                    oldRecord.EndDate = newRecord.EndDate;
                    oldRecord.DispatchesStart = newRecord.DispatchesStart;
                    oldRecord.DispatchesEnd = newRecord.DispatchesEnd;
                    oldRecord.Plan = newRecord.Plan;
                    oldRecord.Fact = newRecord.Fact;

                    toUpdate.Add(oldRecord);
                }
            }

            foreach (IEnumerable<PromoSales> items in toCreate.Partition(10000))
            {
                string insertScript = generator.BuildInsertScript(items);
                context.Database.ExecuteSqlCommand(insertScript);
            }

            foreach (IEnumerable<PromoSales> items in toUpdate.Partition(10000))
            {
                string insertScript = generator.BuildUpdateScript(items);
                context.Database.ExecuteSqlCommand(insertScript);
            }

            context.SaveChanges();
            return sourceRecords.Count();
        }

        private IEnumerable<PromoSales> GetQuery(DatabaseContext context)
        {
            IQueryable<PromoSales> query = context.Set<PromoSales>().AsNoTracking();
            return query.ToList();
        }
    }
}