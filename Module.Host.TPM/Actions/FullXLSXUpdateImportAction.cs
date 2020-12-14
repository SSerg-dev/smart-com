using Interfaces.Implementation.Import.FullImport;
using System;
using System.Collections.Generic;
using System.Linq;
using Core.Data;
using Persist;
using Persist.ScriptGenerator;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using Core.Extensions;

namespace Module.Host.TPM.Actions {
    class FullXLSXUpdateImportAction : FullXLSXImportAction {
        public FullXLSXUpdateImportAction(FullImportSettings settings) : base(settings) { }

        protected override int InsertDataToDatabase(IEnumerable<IEntity<Guid>> records, DatabaseContext context) {
            ScriptGenerator generator = GetScriptGenerator();
            PromoComparer cmp = new PromoComparer();
            IQueryable<Promo> sourceRecords = records.Cast<Promo>().AsQueryable();
            IList<Promo> query = GetQuery(context).ToList();
            IList<Promo> toCreate = new List<Promo>();
            IList<Promo> toUpdate = new List<Promo>();

            foreach (Promo newRecord in sourceRecords) {
                Promo oldRecord = query.FirstOrDefault(x => cmp.Equals(x, newRecord));
                if (oldRecord == null) {
                    toCreate.Add(newRecord);
                } else {
                    oldRecord.BrandId = newRecord.BrandId;
                    oldRecord.BrandTechId = newRecord.BrandTechId;
                    oldRecord.PromoStatusId = newRecord.PromoStatusId;
                    oldRecord.MarsMechanicId = newRecord.MarsMechanicId;
                    oldRecord.ColorId = newRecord.ColorId;
                    oldRecord.EventId = newRecord.EventId;

                    oldRecord.StartDate = newRecord.StartDate;
                    oldRecord.EndDate = newRecord.EndDate;
                    oldRecord.DispatchesStart = newRecord.DispatchesStart;
                    oldRecord.DispatchesEnd = newRecord.DispatchesEnd;

                    oldRecord.CalendarPriority = newRecord.CalendarPriority;

                    toUpdate.Add(oldRecord);
                }
            }

            foreach (IEnumerable<Promo> items in toCreate.Partition(10000)) {
                string insertScript = generator.BuildInsertScript(items);
                context.ExecuteSqlCommand(insertScript);
            }

            foreach (IEnumerable<Promo> items in toUpdate.Partition(10000)) {
                string insertScript = generator.BuildUpdateScript(items);
                context.ExecuteSqlCommand(insertScript);
            }

            return sourceRecords.Count();
        }

        private IEnumerable<Promo> GetQuery(DatabaseContext context) {
            IQueryable<Promo> query = context.Set<Promo>().AsNoTracking();
            return query.ToList();
        }
    }
}