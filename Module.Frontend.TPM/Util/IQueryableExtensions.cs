using Core.Data;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.Http.OData.Query;

namespace Module.Frontend.TPM.Util
{
    public static class IQueryableExtensions
    {
        public static string ToTraceQuery(this IQueryable query)
        {
            return IQueryableExtensions.ToTraceQuery((dynamic)query);
        }
        /// <summary>
        /// For an Entity Framework IQueryable, returns the SQL with inlined Parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string ToTraceQuery<T>(this IQueryable<T> query)
        {

            ObjectQuery objectQuery;
            var method = typeof(IQueryableExtensions).GetMethod("GetQueryFromQueryable");
            var genMethod = method.MakeGenericMethod(query.GetType().GenericTypeArguments[0]);

            objectQuery = (dynamic)genMethod.Invoke(null, new object[] { query });

            var result = objectQuery.ToTraceString();
            foreach (var parameter in objectQuery.Parameters)
            {
                if (parameter.Value == null)
                    continue;
                var name = "@" + parameter.Name;
                var value = "'" + parameter.Value.ToString() + "'";
                result = result.Replace(name, value);
            }
            return result;

        }
        public static System.Data.Entity.Core.Objects.ObjectQuery<T> GetQueryFromQueryable<T>(IQueryable<T> query)
        {
            var internalQueryField = query.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Where(f => f.Name.Equals("_internalQuery")).FirstOrDefault();
            var internalQuery = internalQueryField.GetValue(query);
            var objectQueryField = internalQuery.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Where(f => f.Name.Equals("_objectQuery")).FirstOrDefault();
            var objectQueryValue = objectQueryField.GetValue(internalQuery);

            return objectQueryValue as System.Data.Entity.Core.Objects.ObjectQuery<T>;
        }
        public static IQueryable<T> FixOdataExpand<T>(this IQueryable<T> query, ODataQueryOptions options) where T : IEntity
        {
            var result = options.ApplyTo(query);
            if (result is IQueryable<T> resultEntity)
            {
                return resultEntity;
            }
            var resultList = new List<T>();

            foreach (var item in result)
            {
                if (item is T item1)
                {
                    resultList.Add(item1);
                }
                else if (item.GetType().Name == "SelectAllAndExpand`1")
                {
                    var entityProperty = item.GetType().GetProperty("Instance");
                    resultList.Add((T)entityProperty.GetValue(item));
                }
            }
            return resultList.AsQueryable();
        }
    }
}
