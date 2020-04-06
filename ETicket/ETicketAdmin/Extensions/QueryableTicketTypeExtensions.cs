using System;
using System.Linq;
using System.Linq.Expressions;
using DBContextLibrary.Domain.Entities;

namespace ETicketAdmin.Extensions
{
    public static class QueryableTicketTypeExtensions
    {
        public static IQueryable<TicketType> ApplySortBy<T>(
            this IQueryable<TicketType> query,
            Expression<Func<TicketType, T>> expression,
            string sortDirection
        )
        {
            return sortDirection == "asc"
                ? query.OrderBy(expression)
                : query.OrderByDescending(expression);
        }
    }
}