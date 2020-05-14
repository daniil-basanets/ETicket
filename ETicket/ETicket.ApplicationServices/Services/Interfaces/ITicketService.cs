using System;
using System.Collections.Generic;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface ITicketService
    {
        public IEnumerable<Ticket> GetTickets();

        public IEnumerable<Ticket> GetTicketsByUserId(Guid userId);

        public Ticket GetTicketById(Guid id);

        public TicketDto GetDto(Guid id);

        public void Create(TicketDto ticketDto);

        public void Update(TicketDto ticketDto);

        public void Delete(Guid id);

        public void Activate(Guid ticketId);
    }
}
