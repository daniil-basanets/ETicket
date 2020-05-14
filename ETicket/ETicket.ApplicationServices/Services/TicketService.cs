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

        IEnumerable<Ticket> ITicketService.GetTickets()
        {
            return uow.Tickets.GetAll().ToList();
        }

        public Ticket GetTicketById(Guid id)
        {
            return uow.Tickets.Get(id);
        }

        public TicketDto GetDto(Guid id)
        {
            var ticket = GetTicketById(id); ;
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
                ticket.TicketType = uow.TicketTypes.Get(ticket.TicketTypeId);
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

        public void Activate(Guid ticketId)
        {
            var ticket = uow.Tickets.GetAll().Where(t => t.Id == ticketId).FirstOrDefault();


            if (ticket == null)
            {
                var e = new ApplicationException(nameof(Activate) + " ticket with id = " + ticketId + " does not exists");
                e.Data.Add("ticketId", ticketId);

                throw e;
            }

            ticket.ActivatedUTCDate = DateTime.UtcNow;
            ticket.ExpirationUTCDate = ticket.ActivatedUTCDate?.AddHours(ticket.TicketType.DurationHours);
            uow.Tickets.Update(ticket);
            uow.Save();
        }

        public IEnumerable<Ticket> GetTicketsByUserId(Guid userId)
        {
           return  uow.Tickets.GetAll().Where(t => t.UserId == userId);
        }
    }
}
