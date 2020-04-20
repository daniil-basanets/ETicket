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
            var temp = query.Where(expression);
            return query.Where(expression);
        }
    }
}
