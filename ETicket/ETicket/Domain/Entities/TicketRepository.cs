using System;
using System.Collections.Generic;
using ETicket.Domain.Interfaces;

namespace ETicket.Domain.Entities
{
    public class TicketRepository : IRepository<Ticket>
    {
        private readonly ETicketDataContext eTicketDataContext;

        public TicketRepository(ETicketDataContext eTicketDataContext)
        {
            this.eTicketDataContext = eTicketDataContext;
        }

        public void Create(Ticket item)
        {// TODO CreateTicket
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {// TODO DeleteTicket
            throw new NotImplementedException();
        }

        public Ticket Get(int id)
        {//TODO GetTicket
            throw new NotImplementedException();
        }

        public IEnumerable<Ticket> GetAll()
        {//TODO IEnumerable<Ticket>
            throw new NotImplementedException();
        }

        public void Update(Ticket item)
        {// TODO UpdateTicket
            throw new NotImplementedException();
        }
    }
}
