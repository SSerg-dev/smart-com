using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace Module.Persist.TPM.Utils.Filter {
    /// <summary>
    /// Конвертации, используемые для фильтра
    /// </summary>
    public static class FilterConverter {

        /// <summary>
        /// Преобразовывает строку в экземпляр FilterNode
        /// </summary>
        /// <param name="jsonString">json выражение</param>
        public static FilterNode ConvertToNode(this String jsonString) {
            JsonReader reader = new JsonTextReader(new StringReader(jsonString)) {
                DateParseHandling = DateParseHandling.DateTime,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            JObject container = JObject.Load(reader);
            var parsedFilterString = DeserializeJsonFilter(container);
            FilterNode n = DeserializeNodeFromJSON(parsedFilterString);
            return n;
        }

        /// <summary>
        /// Преобразование фильтра в структурированный вид для последующего преобразования в FilterNode
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static JToken DeserializeJsonFilter(JToken value, JObject result = null) {
            if (value is JArray) {
                JArray arr = new JArray();
                foreach (JObject property in value) {
                    arr.Add(DeserializeJsonFilter(property));
                }
                JObject node = new JObject();
                node.Add("Nodes", arr);
                return arr;
            } else {
                IEnumerable<JProperty> properties = ((JObject) value).Properties();
                JObject obj = result == null ? new JObject() : result;
                string op;
                foreach (JProperty property in properties) {
                    if (property.Name == "and" || property.Name == "or") {
                        obj.Add("Operator", property.Name);
                        obj.Add("Rules", DeserializeJsonFilter(property.Value));
                    } else if (operations.TryGetValue(property.Name, out op)) {
                        if (op == "Any") {
                            obj.Add("Operation", op);
                            DeserializeJsonFilter(property.Value, obj);
                        } else {
                            obj.Add("Operation", op);
                            obj.Add("Data", property.Value);
                        }
                    } else if (property.Name == "values") {
                        List<string> valList = new List<string>();
                        foreach (string val in property.Value) {
                            valList.Add(val);
                        }
                        obj.Add("Data", String.Join(",", valList));
                    } else {
                        obj.Add("Field", property.Name);
                        DeserializeJsonFilter(property.Value, obj);
                    }
                }
                return obj;
            }
        }

        private static FilterNode DeserializeNodeFromJSON(JToken value) {
            FilterNode n = null;
            if (value != null) {
                n = new FilterNode { Rules = new List<FilterRule>(), Nodes = new List<FilterNode>() };
                try {
                    n.Operator = ((string) value["Operator"]).ToLower() == "and" ? NodeOperator.and : NodeOperator.or;
                } catch {
                    n.Operator = ((int) value["Operator"]) == 0 ? NodeOperator.and : NodeOperator.or;
                }
                //if (value["Nodes"] != null)
                //    foreach (JToken token in (JArray) value["Nodes"])
                //        n.Nodes.Add(DeserializeNodeFromJSON(token));

                if (value["Rules"] != null)
                    foreach (JToken token in (JArray) value["Rules"])
                        if (token["Operator"] != null) {
                            n.Nodes.Add(DeserializeNodeFromJSON(token));
                        } else {
                            n.Rules.Add(DeserializeRuleFromJSON(token));
                        }
            }
            return n;
        }

        private static FilterRule DeserializeRuleFromJSON(JToken value) {
            FilterRule r = new FilterRule();

            r.Field = value["Field"].ToString();
            r.Data = value["Data"].ToString();
            //Object objectType = value["ObjectType"];
            //if (objectType != null) {
            //    //r.ObjectType = objectType.ToString();
            //}
            switch (value["Operation"].ToString()) {
                case "ContainsEx": r.Operator = RuleOperator.ContainsEx; break;
                case "Contains": r.Operator = RuleOperator.Contains; break;
                case "EndsWith": r.Operator = RuleOperator.EndsWith; break;
                case "Equals": r.Operator = RuleOperator.Equals; break;
                case "GraterOrEqual": r.Operator = RuleOperator.GraterOrEqual; break;
                case "GraterThan": r.Operator = RuleOperator.GraterThan; break;
                case "IsNull": r.Operator = RuleOperator.IsNull; break;
                case "LessOrEqual": r.Operator = RuleOperator.LessOrEqual; break;
                case "LessThan": r.Operator = RuleOperator.LessThan; break;
                case "NotEqual": r.Operator = RuleOperator.NotEqual; break;
                case "NotNull": r.Operator = RuleOperator.NotNull; break;
                case "StartsWith": r.Operator = RuleOperator.StartsWith; break;
                case "Any": r.Operator = RuleOperator.Any; break;
                    //case "All": r.Operator = RuleOperator.All; break;
            }
            r.Node = DeserializeNodeFromJSON(value["Node"]);

            return r;
        }

        private static Dictionary<string, string> operations = new Dictionary<string, string>() {
            { "eq", "Equals"},
            { "contains", "Contains"},
            //{ "", "ContainsEx"},
            { "endswith", "EndsWith"},
            { "ge", "GraterOrEqual"},
            { "isnull", "IsNull"},
            { "", "LessOrEqual"},
            { "lt", "LessThan"},
            { "ne", "NotEqual"},
            { "notnull", "NotNull"},
            { "startswith", "StartsWith"},
            { "gt", "GraterThan"},
            { "in", "Any"},
        };
    }
}
