using ETicket.DataAccess.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ETicket.Admin.Services.Interfaces
{
    public interface IDataTablePagingService<T>
    {
        IList<Expression<Func<T, string>>> GetSortExpressions();
        IList<Expression<Func<T, bool>>> GetFilterExpressions(int[] columnNumbers, string[] filterValues);
        IList<Expression<Func<T, bool>>> GetSearchExpressions(string searchValue);
        IQueryable<T> GetAll();
    }
}
