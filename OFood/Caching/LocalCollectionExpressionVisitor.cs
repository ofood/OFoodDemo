using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using OFood.Extensions;


namespace OFood.Caching
{
    internal class LocalCollectionExpressionVisitor : ExpressionVisitor
    {
        public static Expression Rewrite(Expression expression)
        {
            return new LocalCollectionExpressionVisitor().Visit(expression);
        }

        #region Overrides of ExpressionVisitor

        /// <summary>
        /// 访问<see cref="T:System.Linq.Expressions.MethodCallExpression"/>子项.
        /// </summary>
        /// <returns>
        /// 修改的表达式，如果它或任何子表达式被修改; 否则，返回原始表达式.
        /// </returns>
        /// <param name="node">表达访问.</param>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            Type instanceType = node.Object == null ? null : node.Object.Type;
            var map = new[] { new { Param = instanceType, Arg = node.Object } }.ToList();
            map.AddRange(node.Method.GetParameters().Zip(node.Arguments, (m, n) => new { Param = m.ParameterType, Arg = n }));

            var replacements = map.Where(m => m.Param != null && m.Param.IsGenericType)
                .Select(m => new { m, type = m.Param.GetGenericTypeDefinition() })
                .Where(o => typeof(IEnumerable<>).IsGenericAssignableFrom(o.type))
                .Where(o => o.m.Arg.NodeType == ExpressionType.Constant)
                .Select(o => new { o, type = o.m.Param.GetGenericArguments().Single() })
                .Select(p => new
                {
                    p.o.m.Arg,
                    Replacement = Expression.Constant("{" + string.Join("|", (IEnumerable)((ConstantExpression)p.o.m.Arg).Value) + "}")
                }).ToList();

            if (replacements.Any())
            {
                List<Expression> args =
                    map.Select(m => (from c in replacements where m.Arg == c.Arg select c.Replacement).SingleOrDefault() ?? m.Arg).ToList();
                try
                {
                    node = node.Update(args.First(), args.Skip(1));
                }
                catch (ArgumentException)
                {
                    return node;
                }
            }

            return base.VisitMethodCall(node);
        }

        #endregion
    }
}