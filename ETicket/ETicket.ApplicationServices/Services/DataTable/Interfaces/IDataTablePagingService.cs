using ETicket.DataAccess.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ETicket.ApplicationServices.Services.DataTable.Interfaces
{
    public interface IDataTablePagingService<T>
    {
        IDictionary<string, Expression<Func<T, object>>> GetSortExpressions();
        IList<Expression<Func<T, bool>>> GetFilterExpressions(string[] columnNames, string[] filterValues);
        IList<Expression<Func<T, bool>>> GetGlobalSearchExpressions(string searchValue);
        IQueryable<T> GetAll();
    }
}
