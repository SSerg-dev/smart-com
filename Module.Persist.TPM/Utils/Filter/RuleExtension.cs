using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Utils.Filter {
    /// <summary>
    /// Методы расширения для класса FilterRule.
    /// </summary>
    internal static class RuleExtension {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rule">Условие выборки.</param>
        /// <param name="parameterType"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Expression ToExpression(this FilterRule rule, Type parameterType, ParameterExpression param) {
            // get the property that will be evaluated
            PropertyInfo pi = null;
            MemberExpression member = null;
            // check for subproperties
            if (rule.Field.Contains(".")) {
                foreach (string f in rule.Field.Split(".".ToCharArray())) {
                    if (pi == null) {
                        pi = parameterType.GetProperty(f);
                        member = Expression.PropertyOrField(param, f);
                    } else {
                        pi = pi.PropertyType.GetProperty(f);
                        member = Expression.PropertyOrField(member, f);
                    }
                }
            } else {
                pi = parameterType.GetProperty(rule.Field);
                member = Expression.PropertyOrField(param, rule.Field);
            }

            Type innerType = member.Type;
            Expression selector = null;
            if (rule.Node == null) {
                if (rule.Operator == RuleOperator.Any) {
                    var values = GetEnumerable<dynamic>(rule.CastDataAs(pi.PropertyType));
                    List<Expression> listOfValconst = new List<Expression>();
                    foreach (var val in values) {
                        listOfValconst.Add(Expression.Constant(val));
                    }
                    selector = Expression.NewArrayInit(pi.PropertyType, listOfValconst);
                } else {
                    selector = rule.Operator == RuleOperator.IsNull || rule.Operator == RuleOperator.NotNull
                        ? Expression.Constant(null, pi.PropertyType)
                        : Expression.Constant(rule.CastDataAs(pi.PropertyType), pi.PropertyType);
                }
            } else {
                // Если поле к которому применяется правило является списком,
                // то получить тип его элементов.
                foreach (Type interfaceType in member.Type.GetInterfaces()) {
                    if (interfaceType.IsGenericType &&
                        interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>)) {
                        innerType = innerType.GetGenericArguments().Single();
                        break;
                    }
                }

                //ParameterExpression p = Expression.Parameter(innerType/*, FilterNode.Parameter*/);
                selector = (Expression) typeof(FilterNode).GetMethod("ToExpressionTree")
                    .MakeGenericMethod(innerType)
                    .Invoke(rule.Node, null);
            }
            switch (rule.Operator) {
                // it's the same for null
                case RuleOperator.IsNull:
                case RuleOperator.Equals:
                    BinaryExpression eqExpr = pi.PropertyType == typeof(String) ?
                        Expression.Equal(Expression.Call(member, typeof(String).GetMethod("ToUpper", new Type[] { })), selector) :
                        Expression.Equal(member, selector);
                    return Expression.AndAlso(Expression.NotEqual(member, Expression.Constant(null, member.Type)),
                        eqExpr);
                // it's the same for not null
                case RuleOperator.NotNull:
                case RuleOperator.NotEqual:
                    BinaryExpression expr = pi.PropertyType == typeof(String) ?
                        Expression.Equal(Expression.Call(member, typeof(String).GetMethod("ToUpper", new Type[] { })), selector) :
                        Expression.Equal(member, selector);
                    return Expression.AndAlso(Expression.NotEqual(member, Expression.Constant(null, member.Type)),
                     Expression.Not(expr));

                case RuleOperator.LessThan:
                    return Expression.LessThan(member, selector);

                case RuleOperator.LessOrEqual:
                    return Expression.LessThanOrEqual(member, selector);

                case RuleOperator.GraterThan:
                    return Expression.GreaterThan(member, selector);

                case RuleOperator.GraterOrEqual:
                    return Expression.GreaterThanOrEqual(member, selector);

                // available only for string fields
                case RuleOperator.StartsWith:
                    return Expression.AndAlso(Expression.NotEqual(member, Expression.Constant(null, member.Type)),
                        Expression.Call(Expression.Call(member, typeof(String).GetMethod("ToUpper", new Type[] { })),
                        typeof(String).GetMethod("StartsWith", new Type[] { typeof(String) }),
                        new Expression[] { selector }));

                // available only for string fields
                case RuleOperator.EndsWith:
                    return Expression.AndAlso(Expression.NotEqual(member, Expression.Constant(null, member.Type)),
                        Expression.Call(Expression.Call(member, typeof(String).GetMethod("ToUpper", new Type[] { })),
                        typeof(String).GetMethod("EndsWith", new Type[] { typeof(String) }),
                        new Expression[] { selector }));

                // available only for string fields
                case RuleOperator.Contains:
                    return Expression.AndAlso(Expression.NotEqual(member, Expression.Constant(null, member.Type)),
                        Expression.Call(Expression.Call(member, typeof(String).GetMethod("ToUpper", new Type[] { })),
                        typeof(String).GetMethod("Contains", new Type[] { typeof(String) }),
                        new Expression[] { selector }));

                // available only for string fields
                case RuleOperator.NotContains:
                    return Expression.AndAlso(Expression.NotEqual(member, Expression.Constant(null, member.Type)),
                        Expression.Not(Expression.Call(Expression.Call(member, typeof(String).GetMethod("ToUpper", new Type[] { })),
                        typeof(String).GetMethod("Contains", new Type[] { typeof(String) }),
                        new Expression[] { selector })));

                case RuleOperator.Any:
                    var parY = Expression.Parameter(member.Type, "y");
                    var eq = Expression.Equal(parY, member);
                    var innerExpression = Expression.Lambda(eq, parY);
                    var methodAny = Expression.Call(anyT.MakeGenericMethod(member.Type), selector, innerExpression);
                    return methodAny;

                    //case RuleOperator.NotAny:
                    //var methodNotAny = typeof(Enumerable)
                    //    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    //    .FirstOrDefault(x => x.Name == "Any" && x.GetParameters().Count() == 2)
                    //    .MakeGenericMethod(innerType);
                    //return Expression.Not(Expression.Call(methodNotAny, new Expression[] { member, selector }));

                    //case RuleOperator.All:
                    //var methodAll = typeof(Enumerable)
                    //    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    //    .FirstOrDefault(x => x.Name == "All" && x.GetParameters().Count() == 2)
                    //    .MakeGenericMethod(innerType);
                    //return Expression.Call(methodAll, new Expression[] { member, selector });
            }

            return null;
        }

        private static IEnumerable<T> GetEnumerable<T>(object list) {
            IEnumerable<T> values = list as IEnumerable<T>;
            return values;
        }

        private static readonly MethodInfo anyT = (from x in typeof(Enumerable).GetMethods(BindingFlags.Public | BindingFlags.Static)
                                                   where x.Name == nameof(Enumerable.Any) && x.IsGenericMethod
                                                   let gens = x.GetGenericArguments()
                                                   where gens.Length == 1
                                                   let pars = x.GetParameters()
                                                   where pars.Length == 2 &&
                                                       pars[0].ParameterType == typeof(IEnumerable<>).MakeGenericType(gens[0]) &&
                                                       pars[1].ParameterType == typeof(Func<,>).MakeGenericType(gens[0], typeof(bool))
                                                   select x).Single();

        private static IEnumerable<Type> GetGenericIEnumerables(Type type) {
            return type.GetInterfaces()
                        .Where(t => t.IsGenericType == true
                            && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                        .Select(t => t.GetGenericArguments()[0]);
        }

            /// <summary>
            /// This method is used to cast the Data field to a specifical item type.
            /// As data will only hold numbers, strings or date, we don't need a big
            /// method to reallize this cast
            /// </summary>
            /// <param name="t"></param>
            /// <returns></returns>
            private static object CastDataAs(this FilterRule rule, Type t) {
            // ignore invalid casts
            if (rule.Data == null) {
                return null;
            }

            if (rule.Operator == RuleOperator.Any) {
                var valuesArray = rule.Data.Split(',');
                List<dynamic> castedList = new List<dynamic>();
                foreach (string value in valuesArray) {
                    castedList.Add(CastDataAs(value, t));
                }
                return castedList;
            } else {
                return CastDataAs(rule.Data, t);
            }
        }


        private static object CastDataAs(String data, Type t) {

            if (t == typeof(String)) {
                return data.ToUpper();
            }

            if (t == typeof(Int32) || t == typeof(Int32?)) {
                return Int32.Parse(data);
            }

            if (t == typeof(Single)) {
                return Single.Parse(data);
            }

            if (t == typeof(Decimal)) {
                return decimal.Parse(data);
            }

            if (t == typeof(Double) || t == typeof(Double?)) {
                return Double.Parse(data);
            }

            if (t == typeof(DateTime) || t == typeof(DateTime?)) {
                return DateTime.Parse(data);
            }

            if (t == typeof(Boolean) || t == typeof(Boolean?)) {
                return Boolean.Parse(data);
            }

            if (t == typeof(Guid)) {
                return Guid.Parse(data);
            }

            if (t == typeof(Guid?)) {
                Guid g = Guid.NewGuid();
                var ok = Guid.TryParse(data, out g);
                if (ok) {
                    return g;
                } else {
                    return null;
                }
            }

            return data;
        }

    }
}
