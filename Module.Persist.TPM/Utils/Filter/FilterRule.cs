using System;
using System.Runtime.Serialization;

namespace Module.Persist.TPM.Utils.Filter {
    [DataContract(Namespace = "")]
    [Serializable]
    public class FilterRule {
        /// <summary>
        /// Создает новый экземпляр условия выборки
        /// </summary>
        public FilterRule() { }

        /// <summary>
        /// Создает новый экземпляр условия выборки
        /// </summary>
        /// <param name="field">Название поля</param>
        /// <param name="data">Значение поля</param>
        /// <param name="ruleOperator">Операция</param>
        public FilterRule(String field, String data, RuleOperator ruleOperator = RuleOperator.Equals) {
            Field = field;
            Data = data;
            Operator = ruleOperator;
        }

        /// <summary>
        /// Наименование поля для сравнения
        /// </summary>
        [DataMember]
        public String Field { get; set; }

        /// <summary>
        /// Идентификатор Metha для заданного поля
        /// </summary>
        public Guid MethaId { get; set; }
        /// <summary>
        /// Идентификатор Attribute для заданного поля
        /// </summary>
        public Guid AttrId { get; set; }

        /// <summary>
        /// Тип объекта у которого берется поле. Для будущего использования.
        /// </summary>
        public String ObjectType { get; set; }

        /// <summary>
        /// Оператор между листом и данными (Data)
        /// </summary>
        [DataMember]
        public RuleOperator Operator { get; set; }

        /// <summary>
        /// Данные, с которыми идет сравнение
        /// </summary>
        [DataMember]
        public String Data { get; set; }

        /// <summary>
        /// Вложенное правило
        /// </summary>
        [DataMember]
        public FilterNode Node { get; set; }

        /// <summary>
        /// Получить хеш-код
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            Int32 res = Field.GetHashCode() ^ Operator.GetHashCode();
            if (ObjectType != null) {
                res ^= ObjectType.GetHashCode();
            }
            if (Data != null) {
                res ^= Data.GetHashCode();
            }
            if (Node != null) {
                res ^= Node.GetHashCode();
            }
            return res;
        }

        /// <summary>
        /// Формирует SQL условие для правила
        /// </summary>
        /// <param name="useBraces">По умолчанию не использовать скобки</param>
        public virtual String ConvertToString(Boolean useBraces = false) {
            String content = String.Empty;
            switch (this.Operator) {
                case RuleOperator.Equals:
                    content = this.Field + " = " + "''" + this.Data + "''";
                    break;
                case RuleOperator.NotEqual:
                    content = this.Field + " <> " + "''" + this.Data + "''";
                    break;
                case RuleOperator.StartsWith:
                    content = this.Field + " LIKE ''" + this.Data + "%''";
                    break;
                case RuleOperator.LessThan:
                    content = this.Field + " < " + "''" + this.Data + "''";
                    break;
                case RuleOperator.LessOrEqual:
                    content = this.Field + " <= " + "''" + this.Data + "''";
                    break;
                case RuleOperator.GraterThan:
                    content = this.Field + " > " + "''" + this.Data + "''";
                    break;
                case RuleOperator.GraterOrEqual:
                    content = this.Field + " >= " + "''" + this.Data + "''";
                    break;
                case RuleOperator.EndsWith:
                    content = this.Field + " LIKE(''%" + this.Data + "'')";
                    break;
                case RuleOperator.Contains:
                    content = this.Field + " LIKE(''%" + this.Data + "%'')";
                    break;
                case RuleOperator.NotContains:
                    content = this.Field + " NOT LIKE(''%" + this.Data + "%'')";
                    break;
                case RuleOperator.IsNull:
                    content = this.Field + " IS NULL";
                    break;
                case RuleOperator.NotNull:
                    content = this.Field + " NOT IS NULL";
                    break;
                default:
                    return "";
            }

            if (useBraces)
                return "(" + content + ")";
            else
                return content;
        }

        public FilterRule Clone() {
            FilterRule rule = new FilterRule(this.Field, this.Data, this.Operator);
            rule.MethaId = this.MethaId;
            rule.ObjectType = this.ObjectType;
            rule.AttrId = this.AttrId;
            return rule;
        }

        public override string ToString() {
            return String.Format("Operator: '{0}', Field: '{1}', Data: '{2}'", Operator, Field, Data);
        }
    }

    /// <summary>
    /// Перечисление операторов, доступных листу
    /// </summary>
    [DataContract(Namespace = "")]
    public enum RuleOperator {
        /// <summary>
        /// ==
        /// </summary>
        [EnumMember]
        Equals,

        /// <summary>
        /// !=
        /// </summary>
        [EnumMember]
        NotEqual,

        /// <summary>
        /// начиная с
        /// </summary>
        [EnumMember]
        StartsWith,

        /// <summary>
        /// <
        /// </summary>
        [EnumMember]
        LessThan,

        /// <summary>
        /// <=
        /// </summary>
        [EnumMember]
        LessOrEqual,

        /// <summary>
        /// >
        /// </summary>
        [EnumMember]
        GraterThan,

        /// <summary>
        /// >=
        /// </summary>
        [EnumMember]
        GraterOrEqual,

        /// <summary>
        /// заканчивая
        /// </summary>
        [EnumMember]
        EndsWith,

        /// <summary>
        /// содержит
        /// </summary>
        [EnumMember]
        Contains,

        /// <summary>
        /// содержит
        /// </summary>
        [EnumMember]
        NotContains,

        /// <summary>
        /// null
        /// </summary>
        [EnumMember]
        IsNull,

        /// <summary>
        /// не null
        /// </summary>
        [EnumMember]
        NotNull,

        [EnumMember]
        ContainsEx,

        /// <summary>
        /// в списке есть элементы удовлетворяющие условию
        /// </summary>
        [EnumMember]
        Any,

        ///// <summary>
        ///// в списке все элементы удовлетворяют условию
        ///// </summary>
        //All
    }
}
