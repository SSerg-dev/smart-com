using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Module.Persist.TPM.Utils.Filter {
    [DataContract(Namespace = "")]
    [Serializable]
    public class FilterNode {

        /// <summary>
        /// Создает корень дерева.
        /// </summary>
        public FilterNode() {
            Rules = new List<FilterRule>();
            Nodes = new List<FilterNode>();
        }

        /// <summary>
        /// Тип объекта узла
        /// </summary>
        public String ObjectType { get; set; }

        /// <summary>
        /// Оператор и/или
        /// </summary>
        [DataMember]
        public NodeOperator Operator { get; set; }

        /// <summary>
        /// Перечень листьев
        /// </summary>
        [DataMember]
        public IList<FilterRule> Rules { get; set; }

        /// <summary>
        /// Перечень вложеных узлов
        /// </summary>
        [DataMember]
        public IList<FilterNode> Nodes { get; set; }

        /// <summary>
        /// Получить хеш-код
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            Int32 res = Operator.GetHashCode();
            if (ObjectType != null) {
                res ^= ObjectType.GetHashCode();
            }
            foreach (FilterRule r in Rules) {
                res ^= r.GetHashCode();
            }
            foreach (FilterNode n in Nodes) {
                res ^= n.GetHashCode();
            }
            return res;
        }

        /// <summary>
        /// Возвращает текстовое значние логической операции для генерирования SQL строки
        /// </summary>
        /// <returns></returns>
        public virtual String SQLLogicOperation() {

            switch (Operator) {
                case NodeOperator.and:
                    return "AND";
                case NodeOperator.or:
                    return "OR";
                case NodeOperator.not:
                    return "NOT";
            }

            return "###Error###";
        }

        /// <summary>
        /// Клонирование объекта
        /// </summary>
        /// <returns></returns>
        public FilterNode Clone() {
            FilterNode node = new FilterNode();
            node.ObjectType = this.ObjectType;
            node.Operator = this.Operator;
            foreach (var child in Nodes)
                node.Nodes.Add(child.Clone());
            foreach (var child in Rules)
                node.Rules.Add(child.Clone());
            return node;
        }

        public override string ToString() {
            StringBuilder result = new StringBuilder();
            String rulesString = String.Join(", ", Rules.Select(r => "{" + r.ToString() + "}"));
            String nodesString = String.Join(", ", Nodes.Select(n => "{" + n.ToString() + "}"));
            result.AppendFormat("Operator: '{0}', Rules: [{1}], Nodes: [{2}]", Operator.ToString(), rulesString, nodesString);
            return result.ToString();
        }
    }
    /// <summary>
    /// Операторы между узлами
    /// </summary>
    [DataContract(Namespace = "")]
    public enum NodeOperator {
        [EnumMember]
        and = 0,
        [EnumMember]
        or = 1,
        [EnumMember]
        not = 2
    }
}
