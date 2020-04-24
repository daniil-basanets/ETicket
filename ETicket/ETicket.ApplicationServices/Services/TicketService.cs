using System;
using System.Linq;
using System.Collections.Generic;
using ETicket.DataAccess.Domain.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.DTOs;

namespace ETicket.ApplicationServices.Services
{
    public class TicketService : ITicketService
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
    }
}
