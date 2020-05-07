
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
        #region

        private readonly IUnitOfWork unitOfWork;

        #endregion

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

        public Expression<Func<Ticket, bool>> GetSearchExpression(int columnNumber, string searchValue)
        {
            return columnNumber switch
            {
                0 => (t => t.TicketType.TypeName.StartsWith(searchValue)),
                1 => (t => t.CreatedUTCDate.ToString().Contains(searchValue)),
                2 => (t => t.ActivatedUTCDate.ToString().Contains(searchValue)),
                3 => (t => t.ExpirationUTCDate.ToString().Contains(searchValue)),
                4 => (t => t.User.FirstName.StartsWith(searchValue) || t.User.LastName.StartsWith(searchValue)),
                _ => (t => true)
            };
        }

        public IList<Expression<Func<Ticket, bool>>> GetSearchExpressions(string searchValue)
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

        public IList<Expression<Func<Ticket, bool>>> GetFilterExpressions(int[] columnNumbers, string[] filterValues)
        {
            var result = new List<Expression<Func<Ticket, bool>>>();

            for (int i = 0; i < columnNumbers.Length; i++)
            {
                result.Add(GetSearchExpression(columnNumbers[i], filterValues[i]));
            }

            return result;
        }

        public IList<Expression<Func<Ticket, string>>> GetSortExpressions()
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
