using System;
using System.Collections.Generic;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface ITicketService
    {
        public IEnumerable<Ticket> GetTickets();

        public Ticket GetTicketById(Guid id);

        public TicketDto GetDto(Guid id);

        public void Create(TicketDto ticketDto);

        public void Update(TicketDto ticketDto);

        public void Delete(Guid id);

        public bool Exists(Guid id);
    }
}
