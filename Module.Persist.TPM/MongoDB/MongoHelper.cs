using Core.Data;
using Core.History;
using Core.Security.Models;
using Persist;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.KeyVault;
using Core.Settings;
using Module.Persist.TPM.Model.History;
using MongoDB.Driver;

namespace Module.Persist.TPM.MongoDB
{
    public class MongoHelper<TKey>
    {
        private readonly IHistoricalEntityFactory<TKey> HistoricalEntityFactory;
        private string colName;

        public MongoHelper()
        {
            var dbName = KeyStorageManager.GetKeyVault().GetSecret("MongoDBName", "");
            var uri = KeyStorageManager.GetKeyVault().GetSecret("MongoUrl", "");
            double ttlSec = AppSettingsManager.GetSetting<double>("MongoTTLSec", 63113904);
            colName = AppSettingsManager.GetSetting<string>("MongoColName", "historicals");

            DocumentStoreHolder.GetConnection(uri, ttlSec);
            DocumentStoreHolder.GetDatabase(dbName);
        }

        public MongoHelper(string uri, string dbName, double ttlSec, IHistoricalEntityFactory<TKey> historicalEntityFactory)
        {
            HistoricalEntityFactory = historicalEntityFactory;

            DocumentStoreHolder.GetConnection(uri, ttlSec);
            DocumentStoreHolder.GetDatabase(dbName);
        }

        public void WriteChanges(OperationDescriptor<TKey> changes, UserInfo user, RoleInfo role)
        {
            var historicalModel = HistoricalEntityFactory.GetHistoryEntity(changes);
            if (historicalModel?.GetType() != null)
            {
                var collection = DocumentStoreHolder.GetCollection(historicalModel.GetType().Name.ToLower());
                var historyEntity = CreateHistoryEntity(changes, historicalModel, user, role);
                var historyObject = GetObject(historyEntity, historicalModel.GetType());
                if (historyEntity != null)
                {
                    collection.InsertOne(historyObject);
                }
            }
        }

        public void WriteChangesFromImport(Tuple<IEntity<TKey>, IEntity<TKey>> changes, UserInfo user, RoleInfo role, OperationType operation)
        {
            var historicalModel = HistoricalEntityFactory.GetHistoryEntity(changes?.Item2);
            if (historicalModel?.GetType() != null)
            {
                var collection = DocumentStoreHolder.GetCollection(historicalModel.GetType().Name.ToLower());
                var historyEntity = CreateHistoryEntity(changes, historicalModel, user, role, operation);
                var historyObject = GetObject(historyEntity, historicalModel.GetType());
                if (historyEntity != null)
                {
                    collection.InsertOne(historyObject);
                }
            }
        }

        public void DeletePromoesHistory<T>(IList<string> promoIds) where T : BaseHistoricalEntity<Guid>
        {
            var collection = DocumentStoreHolder.GetCollection<T>();
            List<Guid> promoGuids = promoIds.Select(s=>Guid.Parse(s)).ToList();
            var filter = Builders<T>.Filter.In("_ObjectId", promoGuids);

            collection.DeleteMany(filter);
        }

        public void MergePromoesHistory<T>(string oldPromoId, string newPromoId) where T : BaseHistoricalEntity<Guid>
        {
            var collection = DocumentStoreHolder.GetCollection<T>();
            var filter = Builders<T>.Filter.Eq("_ObjectId", new Guid(oldPromoId));
            var update = Builders<T>.Update.Set("_ObjectId", new Guid(newPromoId));

            collection.UpdateMany(filter, update);
        }

        //Создание исторической сущности для Create/Edit
        private Dictionary<string, object> CreateHistoryEntity
            (OperationDescriptor<TKey> opDescr, BaseHistoricalEntity<TKey> historicalModel, UserInfo user, RoleInfo role)
        {
            var mainInformation = new Dictionary<string, object>();
            mainInformation = GetMainInformation(historicalModel, opDescr);

            var additionalInformation = GetAdditionalInformation(opDescr.Entity, opDescr.Operation, user?.Login, role?.DisplayName);

            mainInformation = AddAdditionalInformation(additionalInformation, mainInformation);

            return mainInformation.Count > additionalInformation.Count ?
                mainInformation.Union(additionalInformation).ToDictionary(x => x.Key, y => y.Value) : null;
        }

        //Создание исторической сущности для Import
        private Dictionary<string, object> CreateHistoryEntity
            (Tuple<IEntity<TKey>, IEntity<TKey>> items, BaseHistoricalEntity<TKey> historicalModel, 
            UserInfo user, RoleInfo role, OperationType operation)
        {
            var mainInformation = new Dictionary<string, object>();
            mainInformation = GetMainInformation(historicalModel, operation, items);

            var additionalInformation = GetAdditionalInformation(items.Item2, operation, user?.Login, role?.DisplayName);

            mainInformation = AddAdditionalInformation(additionalInformation, mainInformation);

            return mainInformation.Count > additionalInformation.Count ?
                mainInformation.Union(additionalInformation).ToDictionary(x => x.Key, y => y.Value) : null;
        }

        //Подбор изменений для Create/Edit
        private Dictionary<string, object> GetChanges(OperationDescriptor<TKey> opDescr,
            Dictionary<string, object> historicalPropertiesValueDictionary,
            Dictionary<string, PropertyInfo> historicalPropertiesDictionary)
        {
            var mainInformation = new Dictionary<string, object>();
            var originalValues = opDescr.OriginalValues;
            var currentValues = opDescr.CurrentValues;

            var differentDictionary = currentValues.PropertyNames
                    .Select(x => new
                    {
                        Name = x,
                        OriginalValue = originalValues[x],
                        CurrentValue = currentValues[x]
                    })
                    .Where(x => x.OriginalValue?.ToString() != x.CurrentValue?.ToString());

            var resultDifferentDictionary = differentDictionary
            .Where(x => historicalPropertiesValueDictionary.ContainsKey(x.Name))
            .Select(x => new
            {
                Name = x.Name,
                OriginalValue = x.OriginalValue ?? GetDefaultValue(historicalPropertiesDictionary[x.Name].PropertyType),
                CurrentValue = x.CurrentValue ?? GetDefaultValue(historicalPropertiesDictionary[x.Name].PropertyType)
            })
            .ToDictionary(x => x.Name, y => y.CurrentValue);

            var endWith = "Id";
            var differentNavigationallyPropertyNames = differentDictionary
                .Where(x => x.Name.EndsWith(endWith)).Select(x => x.Name.Substring(0, x.Name.Length - endWith.Length));

            var differentNavigationnalyPropertyDictionary = historicalPropertiesValueDictionary
                .Where(x => differentNavigationallyPropertyNames.Any(y => x.Key.StartsWith(y) && !String.IsNullOrEmpty(y)))
                .ToDictionary(x => x.Key, y => y.Value);

            mainInformation = resultDifferentDictionary;
            foreach (var keyValue in differentNavigationnalyPropertyDictionary)
            {
                if (!mainInformation.ContainsKey(keyValue.Key))
                {
                    mainInformation.Add(keyValue.Key, keyValue.Value);
                }
            }
            return mainInformation;
        }

        //Подбор изменений для Import
        private Dictionary<string, object> GetChanges(IEntity<TKey> oldItem, IEntity<TKey> newItem, Type historyType)
        {
            object newProperty = new object();
            object oldProperty = new object();
            Dictionary<string, object> result = new Dictionary<string, object>();
            IEnumerable<string> properties = historyType.GetProperties().Select(x => x.Name);
            if (oldItem != null)
            {
                foreach (var property in newItem?.GetType().GetProperties())
                {
                    if (properties.Contains(property.Name))
                    {
                        newProperty = property.GetValue(newItem);
                        oldProperty = property.GetValue(oldItem);
                        if (!(oldProperty == null && newProperty == null) && ((oldProperty == null && newProperty != null) || !oldProperty.Equals(newProperty)))
                        {
                            if (newProperty != null)
                            {
                                result.Add(property.Name, newProperty);
                            }
                            else
                            {
                                result.Add(property.Name, "");
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var property in newItem?.GetType().GetProperties())
                {
                    if (properties.Contains(property.Name))
                    {
                        newProperty = property.GetValue(newItem);
                        if (property.GetValue(newProperty) != null)
                        {
                            if (newProperty != null)
                            {
                                result.Add(property.Name, newProperty);
                            }
                            else
                            {
                                result.Add(property.Name, "");
                            }
                        }
                    }
                }
            }
            if (result.Count > 0)
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        private Dictionary<string, object> GetMainInformation
            (BaseHistoricalEntity<TKey> historicalModel, OperationDescriptor<TKey> opDescr)
        {
            var mainInformation = new Dictionary<string, object>();

            var historicalPropertiesDictionary = historicalModel.GetType().GetProperties()
                .ToDictionary(x => x.Name, y => y);

            var historicalPropertiesValueDictionary = historicalPropertiesDictionary
                .ToDictionary(x => x.Key, y => GetPropertyValue(y.Value, historicalModel));

            if (opDescr.Operation != OperationType.Updated)
            {
                mainInformation = historicalPropertiesValueDictionary
                    .Where(x => x.Value != GetDefaultValue(historicalPropertiesDictionary[x.Key].PropertyType))
                    .ToDictionary(x => x.Key, y => y.Value);
            }
            else
            {
                mainInformation = GetChanges(opDescr, historicalPropertiesValueDictionary, historicalPropertiesDictionary);
            }

            return mainInformation;
        }

        private Dictionary<string, object> GetMainInformation
            (BaseHistoricalEntity<TKey> historicalModel, OperationType operation,
            Tuple<IEntity<TKey>, IEntity<TKey>> items)
        {
            var mainInformation = new Dictionary<string, object>();

            var historicalPropertiesDictionary = historicalModel.GetType().GetProperties()
                .ToDictionary(x => x.Name, y => y);

            var historicalPropertiesValueDictionary = historicalPropertiesDictionary
                .ToDictionary(x => x.Key, y => GetPropertyValue(y.Value, historicalModel));

            if (operation == OperationType.Updated)
            {
                var differentDictionary = GetChanges(items.Item1, items.Item2, historicalModel.GetType());

                if (differentDictionary != null)
                {
                    var endWith = "Id";
                    var differentNavigationallyPropertyNames = differentDictionary
                        .Where(x => x.Key.EndsWith(endWith)).Select(x => x.Key.Substring(0, x.Key.Length - endWith.Length));

                    var differentNavigationnalyPropertyDictionary = historicalPropertiesValueDictionary
                        .Where(x => differentNavigationallyPropertyNames.Any(y => x.Key.StartsWith(y) && !String.IsNullOrEmpty(y)))
                        .ToDictionary(x => x.Key, y => y.Value);

                    mainInformation = differentDictionary;
                    foreach (var keyValue in differentNavigationnalyPropertyDictionary)
                    {
                        if (!mainInformation.ContainsKey(keyValue.Key))
                        {
                            mainInformation.Add(keyValue.Key, keyValue.Value);
                        }
                    }
                }
            }
            else
            {
                mainInformation = historicalPropertiesValueDictionary
                    .Where(x => x.Value != GetDefaultValue(historicalPropertiesDictionary[x.Key].PropertyType))
                    .ToDictionary(x => x.Key, y => y.Value);
            }

            return mainInformation;
        }

        private Dictionary<string, object> GetAdditionalInformation
            (IEntity<TKey> item, OperationType operation, string login, string displayName)
        {
            return new Dictionary<string, object>
            {
                ["_Id"] = Guid.NewGuid().ToString(),
                ["_ObjectId"] = item.Id,
                ["_EditDate"] = DateTimeOffset.Now,
                ["_Operation"] = operation.ToString(),
                ["_User"] = login,
                ["_Role"] = displayName
            };
        }

        private Dictionary<string, object> AddAdditionalInformation
            (Dictionary<string, object> additionalInformation, Dictionary<string, object> mainInformation)
        {
            foreach (var keyValue in additionalInformation)
            {
                if (mainInformation.ContainsKey(keyValue.Key))
                {
                    mainInformation[keyValue.Key] = additionalInformation[keyValue.Key];
                }
                else
                {
                    mainInformation.Add(keyValue.Key, keyValue.Value);
                }
            }

            return mainInformation;
        }

        private object GetObject(Dictionary<string, object> dict, Type type)
        {
            var obj = Activator.CreateInstance(type);

            foreach (var kv in dict)
            {
                type.GetProperty(kv.Key).SetValue(obj, kv.Value);
            }
            return obj;
        }

        private object GetPropertyValue(PropertyInfo propertyInfo, object obj)
        {
            var objectValue = propertyInfo.GetValue(obj);
            if (objectValue != null)
            {
                var stringValue = objectValue.ToString();
                if (!String.IsNullOrEmpty(stringValue))
                {
                    return objectValue;
                }
            }

            return GetDefaultPropertyValue(propertyInfo);
        }

        private object GetDefaultPropertyValue(PropertyInfo propertyInfo)
        {
            return GetDefaultValue(propertyInfo.PropertyType);
        }

        private object GetDefaultValue(Type type)
        {
            if (type != null)
            {
                var underlyingType = Nullable.GetUnderlyingType(type);
                if (underlyingType != null && underlyingType.IsValueType)
                {
                    var defaultUnderlyingValue = Activator.CreateInstance(underlyingType);
                    if (defaultUnderlyingValue != null)
                    {
                        return defaultUnderlyingValue;
                    }
                }
                else if (type.IsValueType)
                {
                    var defaultValue = Activator.CreateInstance(type);
                    if (defaultValue != null)
                    {
                        return defaultValue;
                    }
                }
            }

            return null;
        }

        public void WriteScenarioPromoes(string source, IList<Guid> promoIds, UserInfo user, RoleInfo role, OperationType operation)
        {
            var newDocs = promoIds.Select(promoId => new
            {
                _id = Guid.NewGuid().ToString(),
                _t = typeof(HistoricalPromo).Name,
                _ObjectId = promoId,
                _Operation = operation.ToString(),
                _Role = role.SystemName,
                _User = user.Login,
                _EditDate = DateTimeOffset.Now,
                Source = source
            }).ToList();

            var collection = DocumentStoreHolder.GetCollection(colName);
            collection.InsertMany(newDocs);
        }
    }
}
