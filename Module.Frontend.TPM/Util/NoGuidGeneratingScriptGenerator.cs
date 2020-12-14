using Core.Data;
using Module.Persist.TPM.Utils;
using Persist.ScriptGenerator;
using Persist.ScriptGenerator.Filter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Module.Frontend.TPM.Util
{
    /// <summary>
    /// Генератор, использующий при генерации поступившие в модели ключевые ID
    /// </summary>
    public class NoGuidGeneratingScriptGenerator
    {
        public string BuildInsertScript(IEnumerable<object> items)
        {
            StringBuilder script = new StringBuilder();
            foreach (object item in items)
            {
                script.AppendLine(InsertBuilder(item));
            }
            return script.ToString();
        }

        public string BuildUpdateScript(IEnumerable<IEntity<Guid>> items)
        {
            StringBuilder script = new StringBuilder();
            foreach (IEntity<Guid> item in items)
            {
                script.AppendLine(UpdateBuilder(item));
            }
            return script.ToString();
        }

        public virtual string BuildSelectScript(PersistFilter filter)
        {
            throw new NotImplementedException();
        }

        public string BuildDeleteScript(IEnumerable<IEntity<Guid>> items)
        {
            StringBuilder script = new StringBuilder();
            foreach (IEntity<Guid> item in items)
            {
                script.AppendLine(DeleteBuilder(item));
            }
            return script.ToString();
        }

        public string BuildDeleteScript(PersistFilter filter)
        {
            return DeleteByFilterBuilder(filter);
        }

        private const string DatabaseDateTimeOffsetFormat = "yyyy-MM-ddTHH:mm:sszzz";
        private const string DatabaseDateTimeFormat = "yyyy-MM-ddTHH:mm:ss";

        private const string InsertTemplate = "INSERT INTO [DefaultSchemaSetting].{0} ({1}) VALUES ({2})";

        private const string UpdateTemplate = "UPDATE [DefaultSchemaSetting].{0} SET {1} WHERE {2}";
        private const string DeleteTemplate = "UPDATE [DefaultSchemaSetting].{0} SET {1} WHERE {2}";
        private const string HardDeleteTemplate = "DELETE FROM [DefaultSchemaSetting].{0} WHERE {1}";

        private PropertyInfo DisabledProperty { get; set; }
        private PropertyInfo DeletedDateProperty { get; set; }

        private bool IsHardDelete { get; set; }
        private bool GenerateIds { get; set; }

        private Type Type { get; set; }

        private string RootTableName { get; set; }

        public NoGuidGeneratingScriptGenerator(Type type, bool generateIds, string rootTableName = null)
        {
            RootTableName = rootTableName;
            GenerateIds = generateIds;

            Type = type;
            IEnumerable<PropertyInfo> properties = type.GetProperties()
                .Where(p => !typeof(IEntity<Guid>).IsAssignableFrom(p.PropertyType) && !IsSpecialNotKeyProperty(p));

            // Подготовка переменных для работы с типами которые помечаются к удалению, а не удаляются физически
            IsHardDelete = !typeof(IDeactivatable).IsAssignableFrom(type);
            IEnumerable<PropertyInfo> deactivatableProperties = typeof(IDeactivatable).GetProperties();
            DisabledProperty = deactivatableProperties.FirstOrDefault(p => p.PropertyType == typeof(bool));
            DeletedDateProperty = deactivatableProperties.FirstOrDefault(p => p.PropertyType == typeof(DateTimeOffset?));

            string fieldNames = String.Join(", ", properties.Select(p => String.Format("{0}", GetColumnName(p))));

            IEnumerable<PropertyInfo> updateableProperties = properties.Where(pi => !IsKeyProperty(pi));
            IEnumerable<PropertyInfo> keyProperties = properties.Where(pi => IsKeyProperty(pi));

            InsertBuilder = (x) => {
                string values = String.Join(", ", properties.Select(p => GetDbPropertyValue(p, x)));
                return String.Format(InsertTemplate, GetTableName(type), fieldNames, values);
            };

            UpdateBuilder = (x) => {
                string values = String.Join(", ", updateableProperties.Select(p => String.Format("{0} = {1}", GetColumnName(p), GetDbPropertyValue(p, x))));
                string where = String.Join(" AND ", keyProperties.Select(p => String.Format("{0} = {1}", GetColumnName(p), GetDbPropertyValue(p, x, false))));
                return String.Format(UpdateTemplate, GetTableName(type), values, where);
            };

            if (IsHardDelete)
            {
                DeleteBuilder = (x) => {
                    string where = String.Join(" AND ", keyProperties.Select(p => String.Format("{0} = {1}", GetColumnName(p), GetDbPropertyValue(p, x, false))));
                    return String.Format(HardDeleteTemplate, GetTableName(type), where);
                };
                DeleteByFilterBuilder = (f) => {
                    string where = GetDbWhere(f);
                    return String.Format(HardDeleteTemplate, GetTableName(type), where);
                };
            }
            else
            {
                DeleteBuilder = (x) => {
                    string values = String.Join(", ", deactivatableProperties.Select(p => String.Format("{0} = {1}", GetColumnName(p), GetDbPropertyValue(p, x))));
                    string where = String.Join(" AND ", keyProperties.Select(p => String.Format("{0} = {1}", GetColumnName(p), GetDbPropertyValue(p, x, false))));
                    return String.Format(DeleteTemplate, GetTableName(type), values, where);
                };
                DeleteByFilterBuilder = (f) => {
                    string values = String.Format("{0} = {1}, {2} = {3}", GetColumnName(DisabledProperty), GetDbValue(DisabledProperty, true), GetColumnName(DeletedDateProperty), GetDbValue(DeletedDateProperty, DateTimeOffset.Now));
                    string where = GetDbWhere(f);
                    return String.Format(DeleteTemplate, GetTableName(type), values, where);
                };
            }

        }

        private Func<object, string> InsertBuilder { get; set; }

        private Func<IEntity<Guid>, string> UpdateBuilder { get; set; }

        private Func<IEntity<Guid>, string> DeleteBuilder { get; set; }

        private Func<PersistFilter, string> DeleteByFilterBuilder { get; set; }


        protected string GetDbWhere(PersistFilter filter, string rootAlias = null)
        {
            StringBuilder script = new StringBuilder();

            string status = null;
            if (!IsHardDelete)
            {
                if (filter.QueryMode == FilterQueryModes.Active)
                {
                    status = String.Format("{0} = {1}", GetColumnName(DisabledProperty, rootAlias), GetDbValue(DisabledProperty, false));
                }
                if (filter.QueryMode == FilterQueryModes.Deleted)
                {
                    status = String.Format("{0} = {1}", GetColumnName(DisabledProperty, rootAlias), GetDbValue(DisabledProperty, true));
                }
            }
            script.Append(status);
            string where = GetFilterNodeScript(filter.Where, rootAlias);
            if (!String.IsNullOrEmpty(status) && !String.IsNullOrEmpty(where))
            {
                script.Append(" AND ");
            }
            script.Append(where);

            string result = script.ToString();
            return String.IsNullOrWhiteSpace(result) ? "1 = 1" : result; // Костыль для пустого фильтра
        }

        private string GetRootPrefix(string rootAlias)
        {
            return String.IsNullOrEmpty(rootAlias) ? "" : rootAlias + ".";
        }

        private string GetFilterNodeScript(FilterNode node, string rootAlias)
        {
            string op = GetDbOperator(node.Operator);
            return String.Join(op, node.Rules.Select(r => GetFilterRuleScript(r, rootAlias)));
        }

        private string GetFilterRuleScript(FilterRule rule, string rootAlias)
        {
            // Если поиск по иерархии то операция In
            string[] fieldPath = rule.FieldName.Split('.');
            string rootFieldName = GetRootPrefix(rootAlias) + GetRootFieldName(fieldPath);
            string op;
            string template;
            if (fieldPath.Length > 1)
            {
                op = GetDbOperation(Operations.In);
                template = "{0}{1}({2})";
            }
            else
            {
                op = GetDbOperation(rule.Operation);
                template = "{0}{1}{2}";
            }
            string filterValue = GetOperationValue(rule.Value, fieldPath, rule.Operation);
            return String.Format(template, rootFieldName, op, filterValue);
        }

        private string GetRootFieldName(string[] fieldPath)
        {
            if (fieldPath.Length > 1)
            {
                return String.Format("{0}Id", fieldPath[0]);
            }
            else
            {
                return fieldPath[0];
            }
        }

        private string GetOperationValue(object value, IList<string> path, Operations op)
        {
            if (path.Count > 1)
            {
                return BuildNavigationString(op, value, Type, path);
            }
            else
            {
                PropertyInfo property = Type.GetProperty(path.Last());
                return GetWhereValue(op, value, property);
            }
        }

        private string BuildNavigationString(Operations op, object value, Type type, IList<string> path)
        {
            IList<UsingModel> usings = new List<UsingModel>();
            IList<LinkModel> links = new List<LinkModel>();
            IList<PropertyInfo> fields = new List<PropertyInfo>();
            BuildUsings(type, path, ref fields, ref usings, ref links);
            UsingModel rootUsing = usings.First();

            string from = String.Join(", ", usings.Select(u => String.Format("{0} AS {1}", GetTableName(u.ModelType), u.Alias)));
            string link = String.Join(" AND ", links.Select(l => String.Format("{0}.[{1}] = {2}.[{3}]", l.ParentAlias, l.ParentFieldName, l.ChildAlias, l.ChildFieldName)));
            if (!String.IsNullOrEmpty(link))
            {
                link += " AND ";
            }
            string val = GetWhereValue(op, value, fields.Last());
            string where = String.Format("{0}.[{1}]{2}{3}", usings.Last().Alias, path.Last(), GetDbOperation(op), val);
            return String.Format("SELECT {0}.[Id] FROM {1} WHERE {2}{3}", rootUsing.Alias, from, link, where);
        }


        private string GetWhereValue(Operations op, object value, PropertyInfo field)
        {
            string result;
            if (op == Operations.In)
            {
                if (value is IEnumerable<object>)
                {
                    IEnumerable<object> valueList = (IEnumerable<object>)value;
                    result = String.Format("({0})", String.Join(", ", valueList.Select(v => GetDbValue(field, v, false))));
                }
                else
                {
                    throw new ApplicationException(String.Format("Значение для операции '{0}' должно быть списком", op));
                }
            }
            else
            {
                result = GetDbValue(field, value, false);
            }
            return result;
        }


        private void BuildUsings(Type type, IList<string> path, ref IList<PropertyInfo> fieldPath, ref IList<UsingModel> usings, ref IList<LinkModel> links)
        {
            string propertyName = path.First();
            PropertyInfo field = type.GetProperty(propertyName);
            if (field == null)
            {
                throw new ApplicationException(String.Format("Поле '{0}' не найдено в '{1}'", propertyName, type.Name));
            }
            fieldPath.Add(field);
            if (path.Count > 1)
            {
                string alias = "A" + usings.Count;
                usings.Add(new UsingModel()
                {
                    Alias = alias,
                    ModelType = field.PropertyType
                });
                if (path.Count > 2)
                {
                    string child = path[1];
                    links.Add(new LinkModel()
                    {
                        ParentAlias = alias,
                        ParentFieldName = child + "Id",
                        ChildAlias = "A" + usings.Count,
                        ChildFieldName = "Id"
                    });
                }
                BuildUsings(field.PropertyType, path.Skip(1).ToList(), ref fieldPath, ref usings, ref links);
            }
        }

        private class UsingModel
        {
            //public string FieldName { get; set; }
            public string Alias { get; set; }
            public Type ModelType { get; set; }
        }

        private class LinkModel
        {
            public string ParentFieldName { get; set; }
            public string ParentAlias { get; set; }
            public string ChildFieldName { get; set; }
            public string ChildAlias { get; set; }
        }

        private string GetDbOperation(Operations op)
        {
            switch (op)
            {
                case Operations.Equals:
                    return " = ";
                case Operations.Less:
                    return " < ";
                case Operations.LessOrEquals:
                    return " <= ";
                case Operations.More:
                    return " > ";
                case Operations.MoreOrEquals:
                    return " >= ";
                case Operations.In:
                    return " IN ";
                //case Operations.Contains:
                //    return " LIKE ";
                case Operations.None:
                    throw new ApplicationException("В фильтре не указано значение Operation");
                default:
                    throw new ApplicationException(String.Format("Значение '{0}' поля Operation не поддерживается", op));
            }
        }

        private string GetDbOperator(Operators op)
        {
            switch (op)
            {
                case Operators.And:
                    return " AND ";
                case Operators.Or:
                    return " OR ";
                case Operators.None:
                    throw new ApplicationException("В фильтре не указано значение Operator");
                default:
                    throw new ApplicationException(String.Format("Значение '{0}' поля Operator не поддерживается", op));
            }
        }

        private string GetColumnName(PropertyInfo pi, string rootAlias = null)
        {
            return String.Format("{0}[{1}]", GetRootPrefix(rootAlias), pi.Name);
        }

        private string GetTableName(Type type)
        {
            return String.Format("[{0}]", String.IsNullOrEmpty(RootTableName) ? type.Name : RootTableName);
        }

        private string GetDbPropertyValue(PropertyInfo pi, object entity, bool generateId = true)
        {
            object val = pi.GetValue(entity);
            return GetDbValue(pi, val, generateId);
        }

        private string GetDbValue(PropertyInfo pi, object value, bool generateId = true)
        {
            generateId = this.GenerateIds;
            if (value == null)
            {
                return "NULL";
            }
            else if (pi.PropertyType == typeof(string))
            {
                return String.Format("N'{0}'", ((string)value).Replace("'", "''"));
            }
            else if (pi.PropertyType == typeof(double) || pi.PropertyType == typeof(double?))
            {
                return ((double)value).ToString(CultureInfo.InvariantCulture);
            }
            else if (pi.PropertyType == typeof(int) || pi.PropertyType == typeof(int?))
            {
                return String.Format("{0}", value);
            }
            else if (pi.PropertyType == typeof(bool) || pi.PropertyType == typeof(bool?))
            {
                return (bool)value ? "1" : "0";
            }
            else if (pi.PropertyType == typeof(Guid) || pi.PropertyType == typeof(Guid?))
            {
                bool isDbGen = IsKeyProperty(pi);
                return String.Format("'{0}'", generateId && isDbGen ? Guid.NewGuid() : value);
            }
            else if (pi.PropertyType == typeof(DateTimeOffset) || pi.PropertyType == typeof(DateTimeOffset?))
            {
                return String.Format("'{0:" + DatabaseDateTimeOffsetFormat + "}'", value);
            }
            else if (pi.PropertyType == typeof(DateTime) || pi.PropertyType == typeof(DateTime?))
            {
                return String.Format("'{0:" + DatabaseDateTimeFormat + "}'", value);
            }
            else
            {
                throw new ApplicationException(String.Format("Db Value Type '{0}' is not supported", pi.PropertyType.Name));
            }
        }

        private bool IsKeyProperty(PropertyInfo pi)
        {
            return false;
        }

        private bool IsSpecialNotKeyProperty(PropertyInfo pi)
        {
            bool isSpecialNotKey = pi.GetCustomAttribute<SpecialNotKeyPropertyAttribute>() != null;
            return isSpecialNotKey;
        }
    }
}
