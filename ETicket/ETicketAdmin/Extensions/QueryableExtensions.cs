using System;
using System.Linq;
using System.Linq.Expressions;

namespace ETicketAdmin.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplySortBy<T, TResult>(
            this IQueryable<T> query,
            Expression<Func<T, TResult>> expression,
            string sortDirection
        )
        {
            return sortDirection == "asc"
                ? query.OrderBy(expression)
                : query.OrderByDescending(expression);
        }
    }
}