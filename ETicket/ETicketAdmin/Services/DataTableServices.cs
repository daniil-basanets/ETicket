using ETicket.Admin.Extensions;
using ETicket.Admin.Models.DataTables;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ETicket.Admin.Services
{
    public class DataTableServices
    {
        public object GetJsonDataTable<T>(
            IQueryable<T> data,
            int drawStep,
            int countRecords,
            int countFiltered
)
        {
            return new
            {
                draw = ++drawStep,
                recordsTotal = countRecords,
                recordsFiltered = countFiltered,
                data = data
            };
        }

        public IQueryable<T> GetSortedQuery<T>(
            IQueryable<T> query,
            List<DataOrder> orders,
            List<Expression<Func<T, string>>> expressions
        )
        {
            var order = orders.First();
            return query.ApplySortBy(expressions[order.Column], order.Dir);
        }

        public IQueryable<T> GetSearchedQuery<T>(
            IQueryable<T> query,
            Expression<Func<T, bool>> expression
        )
        {
            return query.ApplySearchBy(expression);
        }

        public IQueryable<T> GetSearchedQuery<T>(
           IQueryable<T> query,
           List<Expression<Func<T, bool>>> expressions
        )
        {
            var exp = expressions.ExpressionsCombinerByOr();

            return query.Where(exp);
        }
    }
}
