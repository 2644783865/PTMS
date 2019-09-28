using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PTMS.Common
{
    public static class LinqExtensions
    {
        public static Expression<T> AddExpression<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            var map = first.Parameters.Select((left, i) => new
            {
                left,
                right = second.Parameters[i]
            }).ToDictionary(p => p.right, p => p.left);

            var rightBody = ReplaceParameterVisitor.ReplacementExpression(map, second.Body);

            return Expression.Lambda<T>(merge(first.Body, rightBody), first.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.AddExpression(second, Expression.And);
        }

        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.AddExpression(second, Expression.AndAlso);
        }

        public static Expression<Func<T, bool>> AndAny<T, TItem>(this Expression<Func<T, bool>> first, Expression<Func<T, IEnumerable<TItem>>> collection, Expression<Func<TItem, bool>> itemFilter)
        {
            var second = LambdaVisitor<T, TItem>.CallAny(collection, itemFilter);
            return first.AndAlso(second);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.AddExpression(second, Expression.Or);
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("Source parameter should be present.");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("Key selector should be present.");
            }

            var knownKeys = new HashSet<TKey>();
            foreach (var element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static Expression<Func<T, bool>> AndIf<T>(this Expression<Func<T, bool>> first, bool apply, Expression<Func<T, bool>> second)
        {
            if (apply)
            {
                if (first != null)
                {
                    return first.AddExpression(second, Expression.And);
                }
                else
                {
                    return second;
                }
            }
            return first;
        }

        public static Expression<Func<T, bool>> OrIf<T>(this Expression<Func<T, bool>> first, bool apply, Expression<Func<T, bool>> second)
        {
            if (apply)
            {
                return first.AddExpression(second, Expression.Or);
            }
            return first;
        }

        public static Expression<Func<T, bool>> FilterIf<T>(bool apply, Expression<Func<T, bool>> first, bool and = true)
        {
            if (!apply)
            {
                first = x => and;
            }
            return first;
        }

        public static Expression<Func<T, bool>> AndAnyIf<T, TItem>(this Expression<Func<T, bool>> first, bool apply, Expression<Func<T, IEnumerable<TItem>>> collection, Expression<Func<TItem, bool>> itemFilter)
        {
            if (apply)
            {
                return first.AndAny(collection, itemFilter);
            }
            return first;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            return _(); IEnumerable<TSource> _()
            {
                var knownKeys = new HashSet<TKey>(comparer);
                foreach (var element in source)
                {
                    if (knownKeys.Add(keySelector(element)))
                        yield return element;
                }
            }
        }

        internal class ReplaceParameterVisitor : ExpressionVisitor
        {
            private readonly Dictionary<ParameterExpression, ParameterExpression> _map;

            public ReplaceParameterVisitor(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            public static Expression ReplacementExpression(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ReplaceParameterVisitor(map).Visit(exp);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                ParameterExpression replacement;
                if (_map.TryGetValue(node, out replacement))
                {
                    node = replacement;
                }
                return base.VisitParameter(node);
            }
        }

        internal class LambdaVisitor<T, TItem> : ExpressionVisitor
        {
            private static ConcurrentDictionary<string, System.Reflection.MethodInfo> _methods = new ConcurrentDictionary<string, System.Reflection.MethodInfo>();
            private Expression<Func<TItem, bool>> _filterExpression;
            private System.Reflection.MethodInfo _linqMethod;

            public LambdaVisitor(Expression<Func<TItem, bool>> filterExpression, string linqMethod)
            {
                _filterExpression = filterExpression;
                _linqMethod = GetLinqMethod(linqMethod);
            }

            public static Expression<Func<T, bool>> CallAny(Expression<Func<T, IEnumerable<TItem>>> collection, Expression<Func<TItem, bool>> filterExpression)
            {
                var callExpression = new LambdaVisitor<T, TItem>(filterExpression, "Any").Visit(collection);
                return callExpression as Expression<Func<T, bool>>;
            }

            protected override Expression VisitLambda<TData>(Expression<TData> node)
            {
                var callExpression = Expression.Call(null, _linqMethod, node.Body, _filterExpression);
                var lambdaExpression = Expression.Lambda<Func<T, bool>>(callExpression, node.Parameters);
                return lambdaExpression;
            }

            private static System.Reflection.MethodInfo GetLinqMethod(string methodName)
            {
                var method = _methods.GetOrAdd(
                    methodName,
                    key =>
                    {
                        var methodInfo = typeof(Enumerable)
                        .GetMethods()
                        .Where(x => x.Name == methodName && x.GetParameters().Count() == 2)
                        .First()
                        .MakeGenericMethod(typeof(TItem));
                        return methodInfo;
                    });

                return method;
            }
        }
    }
}
