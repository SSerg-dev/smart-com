using Core.History;
using ElasticLinq;
using ElasticLinq.Mapping;
using History.ElasticSearch;
using Nest;
using Persist;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.ElasticSearch
{
    public class LocalElasticHistoryReader : IHistoryReader
    {
        private readonly IHistoricalEntityFactory<Guid> historicalEntityFactory;

        private string elasticUri;
        private string elasticIndexName;

        public class CustomElasticMapping : ElasticMapping
        {
            private string typeName;

            public CustomElasticMapping(string typeName = null) : base(true, true, true, true, EnumFormat.String, null)
            {
                this.typeName = typeName;
            }

            public override string GetDocumentType(Type type)
            {
                if (typeName == null)
                {
                    return base.GetDocumentType(type);
                }

                //return ElasticHistoryReader.GetElasticTypeName(type);
                return typeName;
            }
        }

        public LocalElasticHistoryReader(string uri, string indexName, IHistoricalEntityFactory<Guid> historicalEntityFactory)
        {
            elasticUri = uri;
            elasticIndexName = indexName;
            this.historicalEntityFactory = historicalEntityFactory;
        }

        public void Dispose()
        {
            //connection.Dispose();
        }

        public IQueryable<T> Query<T>() where T : BaseHistoricalEntity<System.Guid>
        {
            return GetAll<T>();
        }

        public IQueryable<T> NewQuery<T>() where T : BaseHistoricalEntity<System.Guid>
        {
            var query = new ElasticQuery<T>(DocumentStoreHolder.GetElasticQueryProvider(elasticUri));
            return query;
        }

        public IQueryable<T> GetById<T>(string id) where T : BaseHistoricalEntity<System.Guid>
        {
            var t = typeof(T);
            var ty = historicalEntityFactory.GetEntityType(t);
            var en = GetElasticTypeName(ty);

            var entityName = GetElasticTypeName(historicalEntityFactory.GetEntityType(typeof(T)));
            var elastic = DocumentStoreHolder.GetConnection(elasticUri, elasticIndexName);

            var searchResponse = elastic.Search<T>(s => s
                                        .Index(elasticIndexName)
                                        .Type(entityName)
                                        .Query(q => q
                                            .QueryString(qs => qs.DefaultField(f => f._ObjectId)
                                            .Query(id)
                                            .DefaultOperator(Operator.And)
                                        ))
                                        .Take(1000)
            );

            return searchResponse.Documents.AsQueryable();
        }

        public IQueryable<T> GetAll<T>() where T : BaseHistoricalEntity<Guid>
        {
            var t = typeof(T);
            var ty = historicalEntityFactory.GetEntityType(t);
            var en = GetElasticTypeName(ty);

            var entityName = GetElasticTypeName(historicalEntityFactory.GetEntityType(typeof(T)));
            var elastic = DocumentStoreHolder.GetConnection(elasticUri, elasticIndexName);

            var searchResponse = elastic.Search<T>(s => s
                                        .Index(elasticIndexName)
                                        .Type(entityName)
                                        .Query(q => q
                                            .QueryString(qs => qs.DefaultField(f => f._ObjectId)
                                            .DefaultOperator(Operator.And)
                                        ))
                                        .Take(1000)
            );


            var resultHistoricalModels = new List<T>();
            var historicalModels = searchResponse.Documents.AsQueryable();

            foreach (var historicalModel in historicalModels)
            {
                var historicalModelType = historicalModel.GetType();
                var historicalPropertiesDictionary = historicalModelType.GetProperties().Select(x => new
                {
                    Name = x.Name,
                    Value = x.GetValue(historicalModel)?.ToString()
                })
                .Where(x => x.Value != null)
                .ToDictionary(x => x.Name, y => y.Value);

                if (historicalPropertiesDictionary.Count > 0)
                {
                    var newHistoricalModel = (T)Activator.CreateInstance(t);
                    var newHistoricalModelProperties = newHistoricalModel.GetType().GetProperties();

                    foreach (var newHistoricalModelProperty in newHistoricalModelProperties)
                    {
                        var converter = TypeDescriptor.GetConverter(newHistoricalModelProperty.PropertyType);

                        if (historicalPropertiesDictionary.ContainsKey(newHistoricalModelProperty.Name))
                        {
                            var value = converter.ConvertFromString(historicalPropertiesDictionary[newHistoricalModelProperty.Name]);
                            newHistoricalModelProperty.SetValue(newHistoricalModel, value);
                        }
                        else
                        {
                            var whereParameter = Expression.Parameter(typeof(T), "x");
                            var whereLambda = Expression.Lambda<Func<T, bool>>
                            (
                                Expression.NotEqual
                                (
                                    Expression.Property(whereParameter, newHistoricalModelProperty.Name),
                                    Expression.Constant(null, typeof(string))
                                ),
                                whereParameter
                            );

                            var selectParameter = Expression.Parameter(typeof(T), "y");
                            var selectLambda = Expression.Lambda<Func<T, object>>
                            (
                                Expression.Convert
                                (
                                    Expression.Property(selectParameter, newHistoricalModelProperty.Name),
                                    typeof(object)
                                ),
                                selectParameter
                            );

                            var actualValue = historicalModels
                                .Where(x => x._EditDate <= historicalModel._EditDate)
                                .OrderByDescending(x => x._EditDate)
                                .Where(whereLambda)
                                .Select(selectLambda)
                                .FirstOrDefault();

                            if (actualValue != null)
                            {
                                var value = converter.ConvertFrom(actualValue);
                                if (value != null)
                                {
                                    newHistoricalModelProperty.SetValue(newHistoricalModel, value);
                                }
                                else
                                {
                                    if (newHistoricalModelProperty.PropertyType.IsValueType)
                                    {
                                        var defaultValue = Activator.CreateInstance(newHistoricalModelProperty.PropertyType);
                                        newHistoricalModelProperty.SetValue(newHistoricalModel, defaultValue);
                                    }
                                    else
                                    {
                                        var underlyingType = Nullable.GetUnderlyingType(newHistoricalModelProperty.PropertyType);
                                        if (underlyingType != null && underlyingType.IsValueType)
                                        {
                                            var defaultValue = Activator.CreateInstance(newHistoricalModelProperty.PropertyType);
                                            newHistoricalModelProperty.SetValue(newHistoricalModel, defaultValue);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    resultHistoricalModels.Add(newHistoricalModel);
                }
            }

            return resultHistoricalModels.AsQueryable();
        }

        private static string GetElasticTypeName(Type entityType)
        {
            var entityName = entityType.Name;
            entityName = entityName.ToPlural(CultureInfo.CurrentCulture);
            entityName = entityName.ToCamelCase(CultureInfo.CurrentCulture);
            return entityName;
        }
    }
}
