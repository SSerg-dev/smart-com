using Interfaces.Implementation.Import.FullImport;
using System;
using System.Collections.Generic;
using System.Linq;

using Core.Data;
using Persist;

using Module.Persist.TPM.Model.TPM;
using Core.Extensions;

using System.Reflection;
using Core.History;
using Module.Persist.TPM.Utils;

using Module.Frontend.TPM.Util;
using System.Text.RegularExpressions;
using Module.Frontend.TPM.Controllers;
using Utility.FileWorker;
using Core.Settings;
using System.IO;
using Utility.Import.ImportModelBuilder;
using Utility.Import;
using Module.Host.TPM.Util;

namespace Module.Host.TPM.Actions
{
    class FullXLSXUpdateByPropertyImportAction : FullXLSXImportAction
    {
        Type TypeTo;
        List<String> UniqueProperties;
        public FullXLSXUpdateByPropertyImportAction(FullImportSettings settings, Type type, List<String> uniqueProperties) : base(settings)
        {
            TypeTo = type;
            UniqueProperties = uniqueProperties;
        }

        /// <summary>
        /// Выполнить разбор файла импорта
        /// </summary>
        /// <returns></returns>
        protected override IList<IEntity<Guid>> ParseImportFile()
        {
            var fileDispatcher = new FileDispatcher();
            string importDir = AppSettingsManager.GetSetting<string>("IMPORT_DIRECTORY", "ImportFiles");
            string importFilePath = Path.Combine(importDir, ImportFile.Name);
            if (!fileDispatcher.IsExists(importDir, ImportFile.Name))
            {
                throw new Exception("Import File not found");
            }

            IImportModelBuilder<string[]> builder = ImportModelFactory.GetCSVImportModelBuilder(ImportType);
            IImportValidator validator = ImportModelFactory.GetImportValidator(ImportType);
            int sourceRecordCount;
            List<string> errors;
            IList<Tuple<string, string>> buildErrors;
            IList<Tuple<IEntity<Guid>, string>> validateErrors;
            logger.Trace("before parse file");
            IList<IEntity<Guid>> records = ImportUtilityTPM.ParseXLSXFile(importFilePath, null, builder, validator, Separator, Quote, HasHeader, out sourceRecordCount, out errors, out buildErrors, out validateErrors);
            logger.Trace("after parse file");

            // Обработать ошибки
            foreach (string err in errors)
            {
                Errors.Add(err);
            }
            if (errors.Any())
            {
                HasErrors = true;
                throw new ImportException("An error occurred while loading the import file.");
            }

            return records;
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

                using (DatabaseContext context = new DatabaseContext())
                {
                    typedRec.SubBrand_code = string.IsNullOrEmpty(typedRec.SubBrand_code) ? null : typedRec.SubBrand_code;
                    isSuitable = ProductsController.BrandTechExist(context, typedRec.Brand_code, typedRec.Segmen_code, typedRec.SubBrand_code, typedRec.Tech_code, ref errors);
                }
            }


            if (TypeTo == typeof(Brand))
            {
                Brand typedRec = (Brand)rec;
                if (String.IsNullOrEmpty(typedRec.Name)) { errors.Add("Brand must have a value"); isSuitable = false; }
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

                Guid emptyGuid = Guid.Empty;
                if (String.IsNullOrEmpty(typedRec.Name)) { errors.Add("Name must have a value"); isSuitable = false; }
                if (String.IsNullOrEmpty(typedRec.SystemName)) { errors.Add("System Name must have a value"); isSuitable = false; }
                if (typedRec.PromoTypesId == null || typedRec.PromoTypesId == emptyGuid)
                {
                    errors.Add("PromoTypesId must have a value"); isSuitable = false;
                }

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

            if (TypeTo == typeof(NonPromoSupport))
            {
                NonPromoSupport typedRec = (NonPromoSupport)rec;
                Guid emptyGuid = Guid.Empty;
                if (typedRec.ClientTreeId != 0) { errors.Add("ClientTreeId must have a value"); isSuitable = false; }
                if (typedRec.NonPromoEquipmentId != null && typedRec.NonPromoEquipmentId != emptyGuid) { errors.Add("NonPromoEquipmentId must have a value"); isSuitable = false; }
                if (typedRec.StartDate == null || typedRec.EndDate == null) { errors.Add("StartDate and EndDate must have a value"); isSuitable = false; }
            }

            if (TypeTo == typeof(NonPromoEquipment))
            {
                NonPromoEquipment typedRec = (NonPromoEquipment)rec;
                if (String.IsNullOrEmpty(typedRec.EquipmentType)) { errors.Add("EquipmentType must have a value"); isSuitable = false; }
            }

            if (TypeTo == typeof(BTL))
            {
                BTL typedRec = (BTL)rec;
                if (String.IsNullOrEmpty(typedRec.Number.ToString())) { errors.Add("BTL must have a value"); isSuitable = false; }
            }

            return isSuitable;
        }

        protected override int InsertDataToDatabase(IEnumerable<IEntity<Guid>> records, DatabaseContext context)
        {
            NoGuidGeneratingScriptGenerator generatorCreate = new NoGuidGeneratingScriptGenerator(TypeTo, false);

            //здесь класс ScriptGenerator перенесен из ядра в Module.Frontend.TPM.Util
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

            foreach (IEntity<Guid> newRecord in sourceRecords)
            {
                bool hasNewRecordChanges = false;
                IEntity<Guid> oldRecord = query.FirstOrDefault(x => unProperties.All(p => CompariseByProperty(p, x, newRecord)) && (disableProperty != null ? !(bool)disableProperty.GetValue(x) : true));
                if (oldRecord == null)
                {
                    newRecord.Id = Guid.NewGuid();

                    if (TypeTo == typeof(Product))
                    {
                        bool @continue = ValidateProductCodes(newRecord);
                        if (@continue) { continue; }
                        IEntity<Guid> productWithComputedProps = GetComputedProps(newRecord, context);
                        toCreate.Add(productWithComputedProps);
                        toHisCreate.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(null, productWithComputedProps));
                    }
                    else
                    {
                        toCreate.Add(newRecord);
                        toHisCreate.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(null, newRecord));
                    }
                }
                else
                {
                    hasNewRecordChanges = checkChanges(oldRecord, newRecord, nonUnProperties);

                    // добавление записи в Update только в случае изменения какого-либо поля в импортируемой записи
                    if (hasNewRecordChanges)
                    {
                        //Cоздание ProductChangeIncident, если тип Product
                        if (TypeTo == typeof(Product))
                        {
                            if (ValidateProductCodes(newRecord)) { continue; }
                            // Поулчаем вычисляемые поля
                            var newProductChanged = GetComputedProps(newRecord, context);
                            //Для нормальной записи в историю необходимо передавать Id в новой записи
                            newProductChanged.Id = oldRecord.Id;

                            toHisUpdate.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(oldRecord, newProductChanged));
                            toUpdate.Add(newProductChanged);

                            ProductChangeIncident pci = new ProductChangeIncident
                            {
                                CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                                ProductId = oldRecord.Id,
                                IsCreate = false,
                                IsDelete = false,
                                IsCreateInMatrix = false,
                                IsDeleteInMatrix = false,
                                IsChecked = false,
                            };
                            context.Set<ProductChangeIncident>().Add(pci);
                        }
                        else
                        {
                            newRecord.Id = oldRecord.Id;
                            toHisUpdate.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(oldRecord, newRecord));
                            toUpdate.Add(newRecord);
                        }                  
                    }
                }
            }

            foreach (IEnumerable<IEntity<Guid>> items in toCreate.Partition(100))
            {
                string insertScript = generatorCreate.BuildInsertScript(items);
                context.ExecuteSqlCommand(insertScript);
            }

            foreach (IEnumerable<IEntity<Guid>> items in toUpdate.Partition(10000))
            {
                string updateScript = generatorUpdate.BuildUpdateScript(items);
                context.ExecuteSqlCommand(updateScript);
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
            else if (TypeTo == typeof(NonPromoSupport))
            {
                query = context.Set<NonPromoSupport>().AsNoTracking();
            }
            else if (TypeTo == typeof(NonPromoEquipment))
            {
                query = context.Set<NonPromoEquipment>().AsNoTracking();
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

        private bool checkChanges(IEntity<Guid> oldRecord, IEntity<Guid> newRecord, IEnumerable<PropertyInfo> nonUnProperties)
        {
            var isChange = false;
            foreach (PropertyInfo property in nonUnProperties)
            {
                var newValue = property.GetValue(newRecord);
                var oldValue = property.GetValue(oldRecord);
                if (!isChange && !EqualityComparer<object>.Default.Equals(newValue, oldValue))
                {
                    isChange = true;
                    break;
                }
            }
            return isChange;
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

        private IEntity<Guid> GetComputedProps(IEntity<Guid> newRecord, DatabaseContext context, IEntity<Guid> oldRecord = null)
        {
            var product = (Product)newRecord;
            var brandCode = product.Brand_code;
            var segCode = product.Segmen_code;
            var techCode = product.Tech_code;
            var subCode = product.SubBrand_code;

            var brandName = context.Set<Brand>().Where(b => b.Segmen_code == segCode && b.Brand_code == brandCode && !b.Disabled).Select(b => b.Name).FirstOrDefault();
            var techName = context.Set<Technology>().Where(t => t.Tech_code == techCode && t.SubBrand_code == subCode && !t.Disabled).Select(b => b.Name).FirstOrDefault();
            var brandTech = context.Set<BrandTech>().Where(bt =>
                                                               bt.Technology != null && bt.Brand != null &&
                                                               bt.Technology.Tech_code == techCode &&
                                                               bt.Technology.SubBrand_code == subCode &&
                                                               !bt.Technology.Disabled &&
                                                               bt.Brand.Brand_code == brandCode &&
                                                               bt.Brand.Segmen_code == segCode &&
                                                               !bt.Disabled).FirstOrDefault();


            product.Brand = !String.IsNullOrEmpty(brandName) ? brandName : String.Empty;
            product.Technology = !String.IsNullOrEmpty(techName) ? techName : String.Empty;
            product.BrandTech = !String.IsNullOrEmpty(brandTech?.Name) ? brandTech.Name : String.Empty;
            product.BrandTech_code = brandTech != null ? String.Format("{0}-{1}", brandCode, techCode) : String.Empty;
            product.BrandsegTech_code = brandTech != null ? String.Format("{0}-{1}-{2}", brandCode, segCode, techCode) : String.Empty;

            return (IEntity<Guid>)product;
        }

        private bool ValidateProductCodes(IEntity<Guid> record)
        {
            var product = (Product)record;
            if (String.IsNullOrWhiteSpace(product.Brand_code) || String.IsNullOrWhiteSpace(product.Segmen_code) || String.IsNullOrWhiteSpace(product.Tech_code))
            {
                Errors.Add(String.Format("Brand, tech, segment codes shouldn't be null or empty. An error occurred while importing the product with ZREP {0}", product.ZREP));
                return true;
            }

            var num = 0;
            var brandCodeIsNum = int.TryParse(product.Brand_code.TrimStart('0'), out num);
            var segmenCodeIsNum = int.TryParse(product.Segmen_code.TrimStart('0'), out num);
            var techCodeIsNum = int.TryParse(product.Tech_code.TrimStart('0'), out num);
            if (!brandCodeIsNum || !segmenCodeIsNum || !techCodeIsNum)
            {
                Errors.Add(String.Format("Brand, tech, segment codes should be numeric. An error occurred while importing the product with ZREP {0}", product.ZREP));
                return true;
            }

            var threeDigitReg = new Regex(@"^\d{3}$");
            var twoDigitReg = new Regex(@"^\d{2}$");
            bool result = false;
            if (!threeDigitReg.IsMatch(product.Brand_code))
            {
                Errors.Add(String.Format("Brand code should contain 3 digits. An error occurred while importing the product with ZREP {0}", product.ZREP));
                result = true;
            }
            if (!twoDigitReg.IsMatch(product.Segmen_code))
            {
                Errors.Add(String.Format("Segment code should contain 2 digits. An error occurred while importing the product with ZREP {0}", product.ZREP));
                result = true;
            }
            if (!threeDigitReg.IsMatch(product.Tech_code))
            {
                Errors.Add(String.Format("Technology code should contain 3 digits. An error occurred while importing the product with ZREP {0}", product.ZREP));
                result = true;
            }
            return result;
        }
    }
}