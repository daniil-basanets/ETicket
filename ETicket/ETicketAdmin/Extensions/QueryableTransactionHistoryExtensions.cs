using DBContextLibrary.Domain.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ETicketAdmin.Extensions
{
    public static class QueryableTransactionHistoryExtensions
    {
        public static IQueryable<TransactionHistory> ApplySortBy<T>(
            this IQueryable<TransactionHistory> query,
            Expression<Func<TransactionHistory, T>> expression,
            string sortDirection
        )
        {
            return sortDirection == "asc"
                ? query.OrderBy(expression)
                : query.OrderByDescending(expression);
        }
    }
}
