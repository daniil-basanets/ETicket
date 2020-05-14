using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using ETicket.ApplicationServices.Services.DataTable.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;

using ETicket.ApplicationServices.Extensions;

namespace ETicket.ApplicationServices.Services.PagingServices
{
    public class TicketPagingService : IDataTablePagingService<Ticket>
    {
        private readonly IUnitOfWork unitOfWork;

        public TicketPagingService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<Ticket> GetAll()
        {
            return unitOfWork
                    .Tickets
                    .GetAll()
                    .Include(t => t.TicketType)
                    .Include(t => t.User);
        }

        public Expression<Func<Ticket, bool>> GetSingleFilterExpression(string columnName, string filterValue)
        {
            return columnName switch
            {
                "ticketType" => (t => t.TicketType.TypeName == filterValue),
                "createdUTCDate" => (t => t.CreatedUTCDate.Date == filterValue.ParseToDate()),
                "activatedUTCDate" => (t => t.ActivatedUTCDate.Value.Date == filterValue.ParseToDate()),
                "expirationUTCDate" => (t => t.ExpirationUTCDate.Value.Date == filterValue.ParseToDate()),
                "user" => (t => t.User.FirstName.StartsWith(filterValue) || t.User.LastName.StartsWith(filterValue)),
                _ => (t => true)
            };
        }

        public IList<Expression<Func<Ticket, bool>>> GetGlobalSearchExpressions(string searchValue)
        {
            return new List<Expression<Func<Ticket, bool>>>
            {
                (t => t.TicketType.TypeName.StartsWith(searchValue)),
                (t => t.CreatedUTCDate.ToString().Contains(searchValue)),
                (t => t.ActivatedUTCDate.ToString().Contains(searchValue)),
                (t => t.ExpirationUTCDate.ToString().Contains(searchValue)),
                (t => t.User.FirstName.StartsWith(searchValue)),
                (t => t.User.LastName.StartsWith(searchValue))
            };
        }

        public IList<Expression<Func<Ticket, bool>>> GetFilterExpressions(string[] columnNames, string[] filterValues)
        {
            var result = new List<Expression<Func<Ticket, bool>>>();

            for (int i = 0; i < columnNames.Length; i++)
            {
                result.Add(GetSingleFilterExpression(columnNames[i], filterValues[i]));
            }

            return result;
        }

        public IDictionary<string, Expression<Func<Ticket, object>>> GetSortExpressions()
        {
            return new Dictionary<string, Expression<Func<Ticket, object>>>
            {
                { "ticketType", (t => t.TicketType.TypeName) },
                { "createdUTCDate", (t => t.CreatedUTCDate) },
                { "activatedUTCDate", (t => t.ActivatedUTCDate) },
                { "expirationUTCDate", (t => t.ExpirationUTCDate) },
                { "user", (t => t.User.LastName) }
            };
        }
    }
}
