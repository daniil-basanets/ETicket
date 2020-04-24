using ETicket.DataAccess.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ETicket.Admin.Services.Interfaces
{
    public interface IDataTablePaging<T>
    {
        List<Expression<Func<T, string>>> GetSortExpression();
        List<Expression<Func<T, bool>>> GetSearchExpression(string searchValue);
        IQueryable<T> GetAll();
    }
}
