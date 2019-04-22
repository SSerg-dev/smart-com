using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Utils.Filter {
    /// <summary>
    /// Методы расширения для класса FilterNode.
    /// </summary>
    public static class NodeExtension {

        /// <summary>
        /// Returns an expression tree from the Group/Rules tree.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> ToExpressionTree<T>(this FilterNode node) {
            Type t = typeof(T);
            ParameterExpression param = Expression.Parameter(t/*, FilterNode.Parameter*/);
            Expression body = GetExpressionFromSubgroup(node, t, param);

            if (body != null) {
                return Expression.Lambda<Func<T, bool>>(body, new ParameterExpression[] { param });
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subgroup"></param>
        /// <param name="parameterType"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private static Expression GetExpressionFromSubgroup(FilterNode subgroup, Type parameterType, ParameterExpression param) {
            Expression body = null;

            //
            // Преобразование всех узлов, содержащихся в узле, в Expression.
            //
            foreach (FilterNode g in subgroup.Nodes) {
                Expression subgroupExpression = GetExpressionFromSubgroup(g, parameterType, param);

                if (subgroupExpression == null) {
                    // Пропустить узлы которые не содержат правил.
                    continue;
                }

                if (body == null) {
                    body = subgroupExpression;
                } else {
                    if (subgroup.Operator == NodeOperator.and) {
                        body = Expression.And(body, subgroupExpression);
                    } else {
                        body = Expression.Or(body, subgroupExpression);
                    }
                }
            }

            //
            // Преобразование всех правил, содержащихся в узле, в Expression.
            //
            foreach (FilterRule r in subgroup.Rules) {
                Expression ruleExpression = r.ToExpression(parameterType, param);

                if (ruleExpression == null) {
                    // Пропустить правило, которое не удалось 
                    // преобразовать в Expression.
                    continue;
                }

                if (body == null) {
                    body = ruleExpression;
                } else {
                    if (subgroup.Operator == NodeOperator.and) {
                        body = Expression.And(body, ruleExpression);
                    } else {
                        body = Expression.Or(body, ruleExpression);
                    }
                }
            }

            return body;
        }

    }
}
