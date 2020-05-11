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

        public Expression<Func<Ticket, bool>> GetSingleFilterExpression(string columnName, string filterValue)
        {
            return columnName switch
            {
                "ticketType" => (t => t.TicketType.TypeName == filterValue),
                "createdUTCDate" => (t => t.CreatedUTCDate.ToString().Contains(filterValue)),
                "activatedUTCDate" => (t => t.ActivatedUTCDate.ToString().Contains(filterValue)),
                "expirationUTCDate" => (t => t.ExpirationUTCDate.ToString().Contains(filterValue)),
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
