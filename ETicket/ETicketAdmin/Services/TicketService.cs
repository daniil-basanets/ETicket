
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using ETicket.Admin.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ETicket.Admin.Services
{
    public class TicketService : IDataTablePaging<Ticket>
    {
        private readonly IUnitOfWork unitOfWork;
        public TicketService(IUnitOfWork unitOfWork)
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

        public List<Expression<Func<Ticket, bool>>> GetSearchExpression(string searchValue)
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

        public List<Expression<Func<Ticket, string>>> GetSortExpression()
        {
            return new List<Expression<Func<Ticket, string>>>
                {
                    (t => t.TicketType.TypeName),
                    (t => t.CreatedUTCDate.ToString()),
                    (t => t.ActivatedUTCDate.ToString()),
                    (t => t.ExpirationUTCDate.ToString()),
                    (t => t.User.LastName)
                };
        }
    }
}
