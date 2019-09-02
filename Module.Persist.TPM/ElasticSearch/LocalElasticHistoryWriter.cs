using Core.Data;
using Core.Extensions;
using Core.History;
using Core.Security.Models;
using ElasticLinq.Mapping;
using History.ElasticSearch;
using Module.Persist.TPM.Model.TPM;
using Nest;
using Newtonsoft.Json;
using Persist;
using Raven.Abstractions.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.ElasticSearch
{
    public class LocalElasticHistoryWriter<TKey> : IHistoryWriter<TKey>
    {
        private readonly IHistoricalEntityFactory<TKey> historicalEntityFactory;
        private const int REQUESTS_PER_SESSION = 30;

        private string elasticUri;
        private string elasticIndexName;

        public LocalElasticHistoryWriter(string uri, string indexName, IHistoricalEntityFactory<TKey> historicalEntityFactory)
        {
            elasticUri = uri;
            elasticIndexName = indexName;
            this.historicalEntityFactory = historicalEntityFactory;
        }

        public void Write(IEnumerable<OperationDescriptor<TKey>> changes, UserInfo user, RoleInfo role)
        {
            var elastic = DocumentStoreHolder.GetConnection(elasticUri, elasticIndexName);
            foreach (var batch in changes.Where(x => x != null).Partition(REQUESTS_PER_SESSION))
            {
                BulkDescriptor descriptor = new BulkDescriptor();

                foreach (var item in batch)
                {
                    var typeName = GetElasticTypeName(item.EntityType);
                    var historyEntity = CreateHistoryEntity(item, user, role);

                    if (historyEntity != null)
                    {
                        descriptor.Index<object>(record => record
                            .Index(elasticIndexName)
                            .Type(typeName)
                            .Document(historyEntity));
                    }
                }

                elastic.Bulk(descriptor);
            }
        }

        private Dictionary<string, string> CreateHistoryEntity
            (OperationDescriptor<TKey> opDescr, UserInfo user, RoleInfo role)
        {
            var originalValues = opDescr.OriginalValues;
            var currentValues = opDescr.CurrentValues;

            var historicalModel = historicalEntityFactory.GetHistoryEntity(opDescr);
            if (historicalModel != null)
            {
                var mainInformation = new Dictionary<string, string>();
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
                    var differentDictionary = currentValues.PropertyNames
                    .Select(x => new
                    {
                        Name = x,
                        OriginalValue = originalValues[x]?.ToString(),
                        CurrentValue = currentValues[x]?.ToString()
                    })
                    .Where(x => x.OriginalValue != x.CurrentValue);

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
                        .Where(x => differentNavigationallyPropertyNames.Any(y => x.Key.StartsWith(y)))
                        .ToDictionary(x => x.Key, y => y.Value);

                    mainInformation = resultDifferentDictionary;
                    foreach (var keyValue in differentNavigationnalyPropertyDictionary)
                    {
                        if (!mainInformation.ContainsKey(keyValue.Key))
                        {
                            mainInformation.Add(keyValue.Key, keyValue.Value);
                        }
                    }
                }

                var additionalInformation = new Dictionary<string, string>
                {
                    ["_Id"] = Guid.NewGuid().ToString(),
                    ["_ObjectId"] = opDescr.Entity.Id.ToString(),
                    ["_EditDate"] = DateTimeOffset.Now.ToString(),
                    ["_Operation"] = opDescr.Operation.ToString(),
                    ["_User"] = user?.Login,
                    ["_Role"] = role?.DisplayName
                };

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

                return mainInformation.Count > additionalInformation.Count ? mainInformation.Union(additionalInformation)
                    .ToDictionary(x => x.Key, y => y.Value) : null;
            }

            return null;
        }

        private string GetPropertyValue(PropertyInfo propertyInfo, object obj)
        {
            var objectValue = propertyInfo.GetValue(obj);
            if (objectValue != null)
            {
                var stringValue = objectValue.ToString();
                if (!String.IsNullOrEmpty(stringValue))
                {
                    return stringValue;
                }
            }

            return GetDefaultPropertyValue(propertyInfo);
        }

        private string GetDefaultPropertyValue(PropertyInfo propertyInfo)
        {
            return GetDefaultValue(propertyInfo.PropertyType);
        }

        private string GetDefaultValue(Type type)
        {
            if (type != null)
            {
                var underlyingType = Nullable.GetUnderlyingType(type);
                if (underlyingType != null && underlyingType.IsValueType)
                {
                    var defaultUnderlyingValue = Activator.CreateInstance(underlyingType);
                    if (defaultUnderlyingValue != null)
                    {
                        return defaultUnderlyingValue.ToString();
                    }
                }
                else if (type.IsValueType)
                {
                    var defaultValue = Activator.CreateInstance(type);
                    if (defaultValue != null)
                    {
                        return defaultValue.ToString();
                    }
                }
            }

            return String.Empty;
        }

        private static string GetElasticTypeName(Type entityType)
        {
            var entityName = entityType.Name;
            entityName = entityName.ToPlural(CultureInfo.CurrentCulture);
            entityName = entityName.ToCamelCase(CultureInfo.CurrentCulture);
            return entityName;
        }

        public void Write(IEnumerable<Tuple<IEntity<TKey>, IEntity<TKey>>> changes, UserInfo user, RoleInfo role, OperationType operation)
        {
            throw new NotImplementedException();
        }
    }
}
