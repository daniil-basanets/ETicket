using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using ETicket.ApplicationServices.Extensions;
using ETicket.ApplicationServices.Services.DataTable.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;

namespace ETicket.ApplicationServices.Services.PagingServices
{
    public class UserPagingService : IDataTablePagingService<User>
    {
        private readonly IUnitOfWork unitOfWork;

        public UserPagingService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<User> GetAll()
        {
            return unitOfWork.Users
                    .GetAll()
                    .Include(t => t.Privilege)
                    .Include(t => t.Document);
        }

        public IDictionary<string, Expression<Func<User, object>>> GetSortExpressions()
        {
            return new Dictionary<string, Expression<Func<User, object>>>
            {
                { "firstName", (t => t.FirstName) },
                { "lastName", (t => t.LastName) },
                { "dateOfBirth", (t => t.DateOfBirth) },
                { "privilege", (t => t.Privilege.Name) },
                { "document", (t => t.Document.Number) }
            };
        }

        public Expression<Func<User, bool>> GetSingleFilterExpression(string columnName, string filterValue)
        {
            return columnName switch
            {
                "firstName" => (t => t.FirstName.StartsWith(filterValue)),
                "lastName" => (t => t.LastName.StartsWith(filterValue)),
                "dateOfBirth" => (t => t.DateOfBirth.Date == filterValue.ParseToDate()),
                "privilege" => (t => t.Privilege.Name == filterValue),
                "document" => (t => t.Document.Number.StartsWith(filterValue)),
                _ => (t => true)
            };
        }

        public IList<Expression<Func<User, bool>>> GetFilterExpressions(string[] columnNames, string[] filterValues)
        {
            var result = new List<Expression<Func<User, bool>>>();

            for (int i = 0; i < columnNames.Length; i++)
            {
                result.Add(GetSingleFilterExpression(columnNames[i], filterValues[i]));
            }

            return result;
        }

        public IList<Expression<Func<User, bool>>> GetGlobalSearchExpressions(string searchValue)
        {
            return new List<Expression<Func<User, bool>>>
            {
                (t => t.FirstName.StartsWith(searchValue)),
                (t => t.LastName.StartsWith(searchValue)),
                (t => t.DateOfBirth.ToString().Contains(searchValue)),
                (t => t.Privilege.Name.StartsWith(searchValue)),
                (t => t.Document.Number.StartsWith(searchValue))
            };
        }
    }
}
