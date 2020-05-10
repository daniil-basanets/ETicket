using System;
using System.Collections.Generic;
using System.Linq;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface ITicketService
    {
        public IEnumerable<Ticket> GetAll();

        public Ticket Get(Guid id);

        public IQueryable<Ticket> Get();

        public TicketDto GetDto(Guid id);

        public void Create(TicketDto ticketDto);

        public void Update(TicketDto ticketDto);

        public void Delete(Guid id);

        public bool Exists(Guid id);

        public void Activate(Guid ticketId, Guid userId);
    }
}
