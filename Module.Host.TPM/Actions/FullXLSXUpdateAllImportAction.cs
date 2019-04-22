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
using System.Reflection;

namespace Module.Host.TPM.Actions {
    class FullXLSXUpdateByPropertyImportAction : FullXLSXImportAction {
        Type TypeTo;
        List<String> UniqueProperties;
        public FullXLSXUpdateByPropertyImportAction(FullImportSettings settings, Type type, List<String> uniqueProperties) : base(settings) {
            TypeTo = type;
            UniqueProperties = uniqueProperties;
        }

        //проверки в соответствии с типом
        protected override bool IsFilterSuitable(IEntity<Guid> rec, out IList<string> errors) {
            errors = new List<string>();
            bool isSuitable = true;
            if (TypeTo == typeof(Product)) {
                Product typedRec = (Product) rec;
                if (typedRec.UOM_PC2Case <0) {
                    errors.Add("UOM_PC2Case can not be less than 0");
                    isSuitable =  false;
                }
            }            
            return isSuitable;
        }

        protected override int InsertDataToDatabase(IEnumerable<IEntity<Guid>> records, DatabaseContext context) {
            ScriptGenerator generator = GetScriptGenerator();
            IQueryable<IEntity<Guid>> sourceRecords = records.Cast<IEntity<Guid>>().AsQueryable();
            IList<IEntity<Guid>> query = GetQuery(context).ToList();
            IList<IEntity<Guid>> toCreate = new List<IEntity<Guid>>();
            IList<IEntity<Guid>> toUpdate = new List<IEntity<Guid>>();

            IEnumerable<PropertyInfo> properties = TypeTo.GetProperties()
            .Where(p => !typeof(IEntity<Guid>).IsAssignableFrom(p.PropertyType));
            IEnumerable<PropertyInfo> unProperties = properties.Where(p => UniqueProperties.Contains(p.Name));
            List<String> nonProperties = new List<String>() { "Id", "Disabled", "DeletedDate" };
            IEnumerable<PropertyInfo> nonUnProperties = properties.Where(p => !UniqueProperties.Contains(p.Name) && !nonProperties.Contains(p.Name));
            PropertyInfo disableProperty = properties.FirstOrDefault(p=>p.Name == "Disabled");

            foreach (IEntity<Guid> newRecord in sourceRecords) {
                IEntity<Guid> oldRecord = query.FirstOrDefault(x => unProperties.All(p => CompariseByProperty(p, x, newRecord)) && (disableProperty!=null ? !(bool)disableProperty.GetValue(x) : true));
                if (oldRecord == null) {
                    toCreate.Add(newRecord);
                } else {
                    foreach (PropertyInfo property in nonUnProperties) {
                        property.SetValue(oldRecord, property.GetValue(newRecord));
                    }

                    toUpdate.Add(oldRecord);
                }
            }

            foreach (IEnumerable<IEntity<Guid>> items in toCreate.Partition(10000)) {
                string insertScript = generator.BuildInsertScript(items);
                context.Database.ExecuteSqlCommand(insertScript);
            }

            foreach (IEnumerable<IEntity<Guid>> items in toUpdate.Partition(10000)) {
                string insertScript = generator.BuildUpdateScript(items);
                context.Database.ExecuteSqlCommand(insertScript);
            }

            return sourceRecords.Count();
        }

        private IEnumerable<IEntity<Guid>> GetQuery(DatabaseContext context) {
            IQueryable<IEntity<Guid>> query = null;
            if (TypeTo == typeof(Brand)) {
                query = context.Set<Brand>().AsNoTracking();
            } else if (TypeTo == typeof(Technology)) {
                query = context.Set<Technology>().AsNoTracking();
            } else if (TypeTo == typeof(Product)) {
                query = context.Set<Product>().AsNoTracking();
            } else if (TypeTo == typeof(Budget)) {
                query = context.Set<Budget>().AsNoTracking();
            } else if (TypeTo == typeof(BudgetItem)) {
                query = context.Set<BudgetItem>().AsNoTracking();
            } else if (TypeTo == typeof(BudgetSubItem)) {
                query = context.Set<BudgetSubItem>().AsNoTracking();
            } else if (TypeTo == typeof(Mechanic)) {
                query = context.Set<Mechanic>().AsNoTracking();
            } else if (TypeTo == typeof(MechanicType)) {
                query = context.Set<MechanicType>().AsNoTracking();
            } else if (TypeTo == typeof(PromoStatus)) {
                query = context.Set<PromoStatus>().AsNoTracking();
            } else if (TypeTo == typeof(RejectReason)) {
                query = context.Set<RejectReason>().AsNoTracking();
            } else if (TypeTo == typeof(Color)) {
                query = context.Set<Color>().AsNoTracking();
            } else if (TypeTo == typeof(Event)) {
                query = context.Set<Event>().AsNoTracking();
            } else if (TypeTo == typeof(NodeType)) {
                query = context.Set<NodeType>().AsNoTracking();
            } else if (TypeTo == typeof(RetailType)) {
                query = context.Set<RetailType>().AsNoTracking();
            }

            return query.ToList();
        }

        private bool CompariseByProperty(PropertyInfo pi, IEntity<Guid> nv, IEntity<Guid> ov) {
            if (pi.PropertyType == typeof(String)) {
                return (String) pi.GetValue(nv) == (String) pi.GetValue(ov);

            } else if (pi.PropertyType == typeof(Guid?)) {
                return (Guid?) pi.GetValue(nv) == (Guid?) pi.GetValue(ov);

            } else if (pi.PropertyType == typeof(Guid)) {
                return (Guid) pi.GetValue(nv) == (Guid) pi.GetValue(ov);

            } else if (pi.PropertyType == typeof(bool)) {
                return (bool) pi.GetValue(nv) == (bool) pi.GetValue(ov);

            } else if (pi.PropertyType == typeof(int?)) {
                return (int?) pi.GetValue(nv) == (int?) pi.GetValue(ov);

            } else if (pi.PropertyType == typeof(double?)) {
                return (double?) pi.GetValue(nv) == (double?) pi.GetValue(ov);

            } else if (pi.PropertyType == typeof(int)) {
                return (int) pi.GetValue(nv) == (int) pi.GetValue(ov);

            } else if (pi.PropertyType == typeof(double)) {
                return (double) pi.GetValue(nv) == (double) pi.GetValue(ov);

            } else if (pi.PropertyType == typeof(DateTimeOffset?)) {
                return (DateTimeOffset?) pi.GetValue(nv) == (DateTimeOffset?) pi.GetValue(ov);

            }
            return false;
        }

    }
}