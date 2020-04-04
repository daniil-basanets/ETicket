using System;
using System.Collections.Generic;
using DBContextLibrary.Domain.Entities;
using DBContextLibrary.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DBContextLibrary.Domain.Repositories
{
    public class TicketRepository : IRepository<Ticket>
    {
        private readonly ETicketDataContext context;

        public TicketRepository(ETicketDataContext eTicketDataContext)
        {
            this.context = eTicketDataContext;
        }

        public void Create(Ticket item)
        {
            context.Tickets.Add(item);
        }

        public void Delete(int id)
        {// TODO DeleteTicket?
            throw new NotImplementedException();
            Ticket ticket = Get(id);
            if (ticket != null)
            {
                context.Tickets.Remove(ticket);
            }
        }

        public Ticket Get(int id)
        {
            return context.Tickets.Find(id);
        }

        public IEnumerable<Ticket> GetAll()
        {
            return context.Tickets;
        }

        public void Update(Ticket item)
        {//TODO UpdateTicket?
            throw new NotImplementedException();
            context.Entry(item).State = EntityState.Modified;
        }
    }
}
