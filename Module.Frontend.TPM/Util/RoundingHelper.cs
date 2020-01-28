using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Module.Frontend.TPM.Util
{
    public static class RoundingHelper
    {
        private static readonly int _precision = 2;

        public static IQueryable<T> ModifyQuery<T>(IQueryable<T> query)
        {
            var queryMaterialized = query.ToList();
            var doubleProperties = typeof(T).GetProperties().Where(x => x.PropertyType == typeof(double) || x.PropertyType == typeof(double?));

            foreach (var item in queryMaterialized)
            {
                foreach (var property in doubleProperties)
                {
                    RoundDoubleProperty(property, item);
                }
            }

            return queryMaterialized.AsQueryable();
        }

        private static void RoundDoubleProperty(PropertyInfo property, object @object, int? precision = null)
        {
            var currentValue = property.GetValue(@object) as double?;
            if (currentValue != null && currentValue.HasValue)
            {
                var newValue = Math.Round(currentValue.Value, precision ?? _precision);
                property.SetValue(@object, newValue);
            }
        }
    }
}
