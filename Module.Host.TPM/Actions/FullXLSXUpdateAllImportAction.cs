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
using Core.History;
using Module.Persist.TPM.Utils;
using Module.Persist.TPM.ElasticSearch;

namespace Module.Host.TPM.Actions {
    class FullXLSXUpdateByPropertyImportAction : FullXLSXImportAction {
        Type TypeTo;
        List<String> UniqueProperties;
        public FullXLSXUpdateByPropertyImportAction(FullImportSettings settings, Type type, List<String> uniqueProperties) : base(settings) {
            TypeTo = type;
            UniqueProperties = uniqueProperties;
        }

        //проверки в соответствии с типом
        protected override bool IsFilterSuitable(IEntity<Guid> rec, out IList<string> errors)
        {
            errors = new List<string>();
            bool isSuitable = true;
            if (TypeTo == typeof(Product))
            {
                Product typedRec = (Product)rec;
                if (typedRec.UOM_PC2Case < 0)
                {
                    errors.Add("UOM_PC2Case can not be less than 0");
                    isSuitable = false;
                }
                if (String.IsNullOrEmpty(typedRec.ZREP)) { errors.Add("ZREP must have a value"); isSuitable = false; }
            }


            if (TypeTo == typeof(Brand))
            {
                Brand typedRec = (Brand)rec;
                if (String.IsNullOrEmpty(typedRec.Name)) { errors.Add("Brand must have a value"); isSuitable = false; }
            }

            if (TypeTo == typeof(Technology))
            {
                Technology typedRec = (Technology)rec;
                if (String.IsNullOrEmpty(typedRec.Name)) { errors.Add("Name must have a value"); isSuitable = false; }
            }

            if (TypeTo == typeof(Budget))
            {
                Budget typedRec = (Budget)rec;
                if (String.IsNullOrEmpty(typedRec.Name)) { errors.Add("Budget must have a value"); isSuitable = false; }
            }

            if (TypeTo == typeof(BudgetItem))
            {
                BudgetItem typedRec = (BudgetItem)rec;
                if (String.IsNullOrEmpty(typedRec.Name)) { errors.Add("Item must have a value"); isSuitable = false; }
            }

            if (TypeTo == typeof(BudgetSubItem))
            {
                BudgetSubItem typedRec = (BudgetSubItem)rec;
                if (String.IsNullOrEmpty(typedRec.Name)) { errors.Add("Name must have a value"); isSuitable = false; }
            }

            if (TypeTo == typeof(Mechanic))
            {
                Mechanic typedRec = (Mechanic)rec;
                if (String.IsNullOrEmpty(typedRec.Name)) { errors.Add("Name must have a value"); isSuitable = false; }
                if (String.IsNullOrEmpty(typedRec.SystemName)) { errors.Add("System Name must have a value"); isSuitable = false; }
            }

            if (TypeTo == typeof(MechanicType))
            {
                MechanicType typedRec = (MechanicType)rec;
                if (String.IsNullOrEmpty(typedRec.Name)) { errors.Add("Name must have a value"); isSuitable = false; }
                if (typedRec.Discount < 0) { errors.Add("Discount can not be less than 0"); isSuitable = false; }
            }

            if (TypeTo == typeof(PromoStatus))
            {
                PromoStatus typedRec = (PromoStatus)rec;
                if (String.IsNullOrEmpty(typedRec.Name)) { errors.Add("Name must have a value"); isSuitable = false; }
                if (String.IsNullOrEmpty(typedRec.SystemName)) { errors.Add("System Name must have a value"); isSuitable = false; }
            }

            if (TypeTo == typeof(RejectReason))
            {
                RejectReason typedRec = (RejectReason)rec;
                if (String.IsNullOrEmpty(typedRec.Name)) { errors.Add("Name must have a value"); isSuitable = false; }
                if (String.IsNullOrEmpty(typedRec.SystemName)) { errors.Add("System Name must have a value"); isSuitable = false; }
            }

            if (TypeTo == typeof(Color))
            {
                Color typedRec = (Color)rec;
                if (String.IsNullOrEmpty(typedRec.SystemName)) { errors.Add("Color RGB must have a value"); isSuitable = false; }
            }

            if (TypeTo == typeof(Event))
            {
                Event typedRec = (Event)rec;
                if (String.IsNullOrEmpty(typedRec.Name)) { errors.Add("Event must have a value"); isSuitable = false; }
            }

            if (TypeTo == typeof(NodeType))
            {
                NodeType typedRec = (NodeType)rec;
                if (String.IsNullOrEmpty(typedRec.Name)) { errors.Add("Node name must have a value"); isSuitable = false; }
                if (String.IsNullOrEmpty(typedRec.Type)) { errors.Add("Node type must have a value"); isSuitable = false; }
            }

            if (TypeTo == typeof(RetailType))
            {
                RetailType typedRec = (RetailType)rec;
                if (String.IsNullOrEmpty(typedRec.Name)) { errors.Add("Name must have a value"); isSuitable = false; }
            }
            return isSuitable;
        }

        protected override int InsertDataToDatabase(IEnumerable<IEntity<Guid>> records, DatabaseContext context)
        {
            NoGuidGeneratingScriptGenerator generatorCreate = new NoGuidGeneratingScriptGenerator(TypeTo, false);
            ScriptGenerator generatorUpdate = new ScriptGenerator(TypeTo);
            IQueryable<IEntity<Guid>> sourceRecords = records.Cast<IEntity<Guid>>().AsQueryable();
            IList<IEntity<Guid>> query = GetQuery(context).ToList();
            IList<IEntity<Guid>> toCreate = new List<IEntity<Guid>>();
            IList<IEntity<Guid>> toUpdate = new List<IEntity<Guid>>();

            IEnumerable<PropertyInfo> properties = TypeTo.GetProperties()
            .Where(p => !typeof(IEntity<Guid>).IsAssignableFrom(p.PropertyType));
            IEnumerable<PropertyInfo> unProperties = properties.Where(p => UniqueProperties.Contains(p.Name));
            List<String> nonProperties = new List<String>() { "Id", "Disabled", "DeletedDate" };
            IEnumerable<PropertyInfo> nonUnProperties = properties.Where(p => !UniqueProperties.Contains(p.Name) && !nonProperties.Contains(p.Name));
            PropertyInfo disableProperty = properties.FirstOrDefault(p => p.Name == "Disabled");

            List<Tuple<IEntity<Guid>, IEntity<Guid>>> toHisCreate = new List<Tuple<IEntity<Guid>, IEntity<Guid>>>();
            List<Tuple<IEntity<Guid>, IEntity<Guid>>> toHisUpdate = new List<Tuple<IEntity<Guid>, IEntity<Guid>>>();
            IEntity<Guid> oldRecordCopy;

            foreach (IEntity<Guid> newRecord in sourceRecords)
            {
                bool hasNewRecordChanges = false;
                IEntity<Guid> oldRecord = query.FirstOrDefault(x => unProperties.All(p => CompariseByProperty(p, x, newRecord)) && (disableProperty != null ? !(bool)disableProperty.GetValue(x) : true));
                if (oldRecord == null)
                {
                    newRecord.Id = Guid.NewGuid();
                    toCreate.Add(newRecord);
                    toHisCreate.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(null, newRecord));
                }
                else
                {
                    oldRecordCopy = oldRecord;
                    foreach (PropertyInfo property in nonUnProperties)
                    {
                        // Проверка на необходимость создания ProductChangeIncident
                        if (TypeTo == typeof(Product))
                        {
                            var newValue = property.GetValue(newRecord);
                            var oldValue = property.GetValue(oldRecord);
                            if (!hasNewRecordChanges && !EqualityComparer<object>.Default.Equals(newValue, oldValue))
                            {
                                hasNewRecordChanges = true;
                            }
                        }

                        property.SetValue(oldRecord, property.GetValue(newRecord));
                    }

                    // добавление записи в Update и создание ProductChangeIncident только в случае изменения какого-либо поля в импортируемой записи
                    if (TypeTo == typeof(Product) && hasNewRecordChanges)
                    {
                        //Для нормальной записи в историю необходимо передавать Id в новой записи
                        newRecord.Id = oldRecordCopy.Id;
                        toHisUpdate.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(oldRecordCopy, newRecord));
                        toUpdate.Add(oldRecord);
                        ProductChangeIncident pci = new ProductChangeIncident
                        {
                            CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                            ProductId = oldRecord.Id,
                            IsCreate = false,
                            IsDelete = false
                        };
                        context.Set<ProductChangeIncident>().Add(pci);
                    }
                }
            }

            foreach (IEnumerable<IEntity<Guid>> items in toCreate.Partition(100))
            {
                string insertScript = generatorCreate.BuildInsertScript(items);
                context.Database.ExecuteSqlCommand(insertScript);
            }

            foreach (IEnumerable<IEntity<Guid>> items in toUpdate.Partition(10000))
            {
                string insertScript = generatorUpdate.BuildUpdateScript(items);
                context.Database.ExecuteSqlCommand(insertScript);
            }
            //Добавление в историю
            context.HistoryWriter.Write(toHisCreate, context.AuthManager.GetCurrentUser(), context.AuthManager.GetCurrentRole(), OperationType.Created);
            context.HistoryWriter.Write(toHisUpdate, context.AuthManager.GetCurrentUser(), context.AuthManager.GetCurrentRole(), OperationType.Updated);

            context.SaveChanges();

            return sourceRecords.Count();
        }

        private IEnumerable<IEntity<Guid>> GetQuery(DatabaseContext context)
        {
            IQueryable<IEntity<Guid>> query = null;
            if (TypeTo == typeof(Brand))
            {
                query = context.Set<Brand>().AsNoTracking();
            }
            else if (TypeTo == typeof(Technology))
            {
                query = context.Set<Technology>().AsNoTracking();
            }
            else if (TypeTo == typeof(Product))
            {
                query = context.Set<Product>().AsNoTracking();
            }
            else if (TypeTo == typeof(Budget))
            {
                query = context.Set<Budget>().AsNoTracking();
            }
            else if (TypeTo == typeof(BudgetItem))
            {
                query = context.Set<BudgetItem>().AsNoTracking();
            }
            else if (TypeTo == typeof(BudgetSubItem))
            {
                query = context.Set<BudgetSubItem>().AsNoTracking();
            }
            else if (TypeTo == typeof(Mechanic))
            {
                query = context.Set<Mechanic>().AsNoTracking();
            }
            else if (TypeTo == typeof(MechanicType))
            {
                query = context.Set<MechanicType>().AsNoTracking();
            }
            else if (TypeTo == typeof(PromoStatus))
            {
                query = context.Set<PromoStatus>().AsNoTracking();
            }
            else if (TypeTo == typeof(RejectReason))
            {
                query = context.Set<RejectReason>().AsNoTracking();
            }
            else if (TypeTo == typeof(Color))
            {
                query = context.Set<Color>().AsNoTracking();
            }
            else if (TypeTo == typeof(Event))
            {
                query = context.Set<Event>().AsNoTracking();
            }
            else if (TypeTo == typeof(NodeType))
            {
                query = context.Set<NodeType>().AsNoTracking();
            }
            else if (TypeTo == typeof(RetailType))
            {
                query = context.Set<RetailType>().AsNoTracking();
            }

            return query.ToList();
        }

        private bool CompariseByProperty(PropertyInfo pi, IEntity<Guid> nv, IEntity<Guid> ov)
        {
            if (pi.PropertyType == typeof(String))
            {
                return (String)pi.GetValue(nv) == (String)pi.GetValue(ov);

            }
            else if (pi.PropertyType == typeof(Guid?))
            {
                return (Guid?)pi.GetValue(nv) == (Guid?)pi.GetValue(ov);

            }
            else if (pi.PropertyType == typeof(Guid))
            {
                return (Guid)pi.GetValue(nv) == (Guid)pi.GetValue(ov);

            }
            else if (pi.PropertyType == typeof(bool))
            {
                return (bool)pi.GetValue(nv) == (bool)pi.GetValue(ov);

            }
            else if (pi.PropertyType == typeof(int?))
            {
                return (int?)pi.GetValue(nv) == (int?)pi.GetValue(ov);

            }
            else if (pi.PropertyType == typeof(double?))
            {
                return (double?)pi.GetValue(nv) == (double?)pi.GetValue(ov);

            }
            else if (pi.PropertyType == typeof(int))
            {
                return (int)pi.GetValue(nv) == (int)pi.GetValue(ov);

            }
            else if (pi.PropertyType == typeof(double))
            {
                return (double)pi.GetValue(nv) == (double)pi.GetValue(ov);

            }
            else if (pi.PropertyType == typeof(DateTimeOffset?))
            {
                return (DateTimeOffset?)pi.GetValue(nv) == (DateTimeOffset?)pi.GetValue(ov);

            }
            return false;
        }

    }
}