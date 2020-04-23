using System.Collections;
using System.Linq;
using ETicket.DataAccess.Domain.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using System;
using ETicket.ApplicationServices.Services.Interfaces;
using System.Collections.Generic;

namespace ETicket.ApplicationServices.Services
{
    public class TicketService: ITicketService
    {
        private readonly IUnitOfWork uow;

        public TicketService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        IEnumerable<Ticket> ITicketService.GetAll()
        {
            return uow.Tickets.GetAll().ToList();
        }

        public Ticket Get(Guid id)
        {
            return uow.Tickets.Get(id);
        }

        public void Create(Ticket ticket)
        {
            ticket.Id = Guid.NewGuid();
            ticket.CreatedUTCDate = DateTime.UtcNow;

            //TODO change to service
           /* if (ticket.TicketType == null)
            {
                ticket.TicketType = ticketTypeService.Get(ticket.TicketTypeId);
            }*/

            if (ticket.ActivatedUTCDate != null)
            {
                ticket.ExpirationUTCDate = ticket.ActivatedUTCDate?.AddHours(ticket.TicketType.DurationHours);
            }

            uow.Tickets.Create(ticket);
            uow.Save();
        }

        public void Update(Ticket ticket)
        {
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
