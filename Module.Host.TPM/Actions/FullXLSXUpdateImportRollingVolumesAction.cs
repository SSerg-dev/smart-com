using Interfaces.Implementation.Import.FullImport;
using System;
using System.Collections.Generic;
using System.Linq;
using Core.Data;
using Persist;
using Module.Persist.TPM.Model.TPM;
using Core.Extensions;

namespace Module.Host.TPM.Actions
{
    public class FullXLSXUpdateImportRollingVolumesAction : FullXLSXImportAction
    {
        public FullXLSXUpdateImportRollingVolumesAction(FullImportSettings settings) : base(settings) { }

        protected override int InsertDataToDatabase(IEnumerable<IEntity<Guid>> records, DatabaseContext context)
        { 
            IQueryable<RollingVolume> sourceRecords = records.Cast<RollingVolume>().AsQueryable();
            IList<RollingVolume> query = GetQuery(context).ToList(); 
            IList<RollingVolume> toUpdate = new List<RollingVolume>();

            foreach (RollingVolume newRecord in sourceRecords)
            {
                if (newRecord.Product!= null && !String.IsNullOrEmpty(newRecord.Week) && !String.IsNullOrEmpty(newRecord.DemandGroup))
                {
                    RollingVolume oldRecord = query.FirstOrDefault(x => x.Product.ZREP.Equals(newRecord.Product.ZREP) && x.Week.Equals(newRecord.Week) && x.DemandGroup.Equals(newRecord.DemandGroup));
                    if (oldRecord != null)
                    {
                        if (newRecord.ManualRollingTotalVolumes >= 0)
                        {
                            oldRecord.ManualRollingTotalVolumes = newRecord.ManualRollingTotalVolumes;

                            toUpdate.Add(oldRecord);
                        }
                        else
                        {
                            newRecord.ManualRollingTotalVolumes = 0;
                            oldRecord.ManualRollingTotalVolumes = newRecord.ManualRollingTotalVolumes;

                            toUpdate.Add(oldRecord);
                        }
                    }
                    else
                    {
                        Errors.Add(String.Format("Not found records for ZREP = {0}, Week = {1}, Demand Group = {2}", newRecord.Product.ZREP, newRecord.Week, newRecord.DemandGroup));
                    }
                }
                else
                {
                    Errors.Add(String.Format("ZREP or Week or Demand Group is empty,ZREP = {0}, Week = {1}, Demand Group = {2}", newRecord.Product != null ? newRecord.Product.ZREP: "", newRecord.Week, newRecord.DemandGroup));
                }
            } 

            foreach (IEnumerable<RollingVolume> items in toUpdate.Partition(10000))
            {
                string updateScript = String.Join("", items.Select(y => String.Format("UPDATE RollingVolume SET  ManualRollingTotalVolumes = {0} WHERE ProductId = '{1}' AND Week = '{2}' AND DemandGroup = '{3}';",
                               y.ManualRollingTotalVolumes, y.ProductId,y.Week,y.DemandGroup)));
                context.Database.ExecuteSqlCommand(updateScript);
            }
            context.SaveChanges();

            return sourceRecords.Count();
        }

        private IEnumerable<RollingVolume> GetQuery(DatabaseContext context)
        {
            IQueryable<RollingVolume> query = context.Set<RollingVolume>().AsNoTracking();
            return query.ToList();
        }
    }
}