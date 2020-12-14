using Core.History;
using LiteDB;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.MongoDB
{
    public class MongoDBHistoryReader : IHistoryReader
    {
        private readonly IHistoricalEntityFactory<Guid> HistoricalEntityFactory;
        private string Uri;
        private string DBName;

        public MongoDBHistoryReader(string uri, string dbName, double ttlSec, IHistoricalEntityFactory<Guid> historicalEntityFactory)
        {
            Uri = uri;
            DBName = dbName;
            HistoricalEntityFactory = historicalEntityFactory;

            DocumentStoreHolder.GetConnection(Uri, ttlSec);
            DocumentStoreHolder.GetDatabase(DBName);
        }

        public void Dispose()
        {

        }

        public IQueryable<T> GetAll<T>() where T : BaseHistoricalEntity<Guid>
        {
            var collection = DocumentStoreHolder.GetCollection<T>();
            var filter = Builders<T>.Filter.Empty;
            var historicalModels = collection.Find(filter).ToList().AsQueryable();
            var result = new List<T>();
            var notCompositeHistoricalModelAttribute = typeof(T)
                .GetCustomAttributes(false)
                .Select(y => y.GetType().Name)
                .Contains("NotCompositeHistoricalModelAttribute");

            foreach (var historicalModel in historicalModels)
            {
                var historicalModelType = historicalModel.GetType();
                var historicalPropertiesDictionary = historicalModelType.GetProperties().Select(x => new
                {
                    Name = x.Name,
                    Value = x.GetValue(historicalModel)?.ToString()
                })
                .ToDictionary(x => x.Name, y => y.Value);

                var newHistoricalModel = (T)Activator.CreateInstance(typeof(T));
                var newHistoricalModelProperties = newHistoricalModel.GetType().GetProperties();

                foreach (var newHistoricalModelProperty in newHistoricalModelProperties)
                {
                    var converter = TypeDescriptor.GetConverter(newHistoricalModelProperty.PropertyType);

                    if (historicalPropertiesDictionary[newHistoricalModelProperty.Name] != null)
                    {
                        var value = converter.ConvertFromString(historicalPropertiesDictionary[newHistoricalModelProperty.Name]);
                        newHistoricalModelProperty.SetValue(newHistoricalModel, value);
                    }
                    else if (!notCompositeHistoricalModelAttribute)
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
                            newHistoricalModelProperty.SetValue(newHistoricalModel, value);
                        }
                    }
                }
                result.Add(newHistoricalModel);
            }

            return result.AsQueryable();
        }

        public IQueryable<T> GetAllById<T>(string id) where T : BaseHistoricalEntity<Guid>
        {
            var result = new List<T>();
            var historicalModels = GetById<T>(id);

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
                    var newHistoricalModel = (T)Activator.CreateInstance(typeof(T));
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

                    result.Add(newHistoricalModel);
                }
            }

            return result.AsQueryable();
        }

        public IQueryable<T> GetById<T>(string id) where T : BaseHistoricalEntity<Guid>
        {
            var collection = DocumentStoreHolder.GetCollection<T>();
            var filter = Builders<T>.Filter.Eq("_ObjectId", new Guid(id));
            var result = collection.Find(filter).ToList();

            return result.AsQueryable();
        }

        public IQueryable<T> Query<T>() where T : BaseHistoricalEntity<Guid>
        {
            return GetAll<T>();
        }
    }
}
