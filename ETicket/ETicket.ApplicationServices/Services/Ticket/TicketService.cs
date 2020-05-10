using System;
using System.Linq;
using System.Collections.Generic;
using ETicket.DataAccess.Domain.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.DataTable.Interfaces;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ETicket.ApplicationServices.Services
{
    public class TicketService : ITicketService, IDataTablePagingService<Ticket>
    {
        private readonly IUnitOfWork uow;
        private readonly MapperService mapper;
        private readonly ITicketTypeService ticketTypeService;

        public TicketService(IUnitOfWork uow, ITicketTypeService ticketTypeService)
        {
            this.uow = uow;
            this.ticketTypeService = ticketTypeService;
            mapper = new MapperService();
        }

        IEnumerable<Ticket> ITicketService.GetAll()
        {
            return uow.Tickets.GetAll().ToList();
        }

        public Ticket Get(Guid id)
        {
            return uow.Tickets.Get(id);
        }

        public TicketDto GetDto(Guid id)
        {
            var ticket = Get(id); ;
            var ticketDto = mapper.Map<Ticket, TicketDto>(ticket);

            return ticketDto;
        }

        public void Create(TicketDto ticketDto)
        {
            var ticket = mapper.Map<TicketDto, Ticket>(ticketDto);

            ticket.Id = Guid.NewGuid();
            ticket.CreatedUTCDate = DateTime.UtcNow;

            if (ticket.TicketType == null)
            {
                ticket.TicketType = ticketTypeService.Get(ticket.TicketTypeId);
            }

            if (ticket.ActivatedUTCDate != null)
            {
                ticket.ExpirationUTCDate = ticket.ActivatedUTCDate?.AddHours(ticket.TicketType.DurationHours);
            }

            uow.Tickets.Create(ticket);
            uow.Save();
        }

        public void Update(TicketDto ticketDto)
        {
            var ticket = mapper.Map<TicketDto, Ticket>(ticketDto);

            uow.Tickets.Update(ticket);
            uow.Save();
        }

        public void Delete(Guid id)
        {
            uow.Tickets.Delete(id);
            uow.Save();
        }

        public bool Exists(Guid id)
        {
            return uow.Tickets.Get(id) != null;
        }

        public IQueryable<Ticket> GetAll()
        {
            return uow
                    .Tickets
                    .GetAll()
                    .Include(t => t.TicketType)
                    .Include(t => t.User);
        }

        public Expression<Func<Ticket, bool>> GetSingleFilterExpression(int columnNumber, string searchValue)
        {
            return columnNumber switch
            {
                0 => (t => t.TicketType.TypeName == searchValue),
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
                result.Add(GetSingleFilterExpression(columnNumbers[i], filterValues[i]));
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
