using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ETicket.Admin.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplySortBy<T, TResult>(
            this IQueryable<T> query,
            Expression<Func<T, TResult>> expression,
            string sortDirection
        )
        {
            return (sortDirection == "asc") ? query.OrderBy(expression) : query.OrderByDescending(expression);
        }

        public static IQueryable<T> ApplySearchBy<T>(
            this IQueryable<T> query,
            Expression<Func<T, bool>> expression
        )
        {
            return query.Where(expression);
        }

        public static Expression<Func<T, bool>> ExpressionsCombinerByOr<T>(
            this IEnumerable<Expression<Func<T, bool>>> expressions)
        {
            Expression<Func<T, bool>> firstFilter = expressions.FirstOrDefault();
            if (firstFilter == null)
            {
                Expression<Func<T, bool>> alwaysTrue = x => true;
                return alwaysTrue;
            }

            var body = firstFilter.Body;
            var param = firstFilter.Parameters.ToArray();
            foreach (var nextFilter in expressions.Skip(1))
            {
                var nextBody = Expression.Invoke(nextFilter, param);
                body = Expression.OrElse(body, nextBody);
            }
            Expression<Func<T, bool>> result = Expression.Lambda<Func<T, bool>>(body, param);
            return result;
        }
    }
}
