using System;
using System.Linq;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ETicket.DataAccess.Domain.Repositories
{
    public class TicketRepository : IRepository<Ticket, Guid>
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

        public void Delete(Guid id)
        {
            Ticket ticket = Get(id);
            if (ticket != null)
            {
                context.Tickets.Remove(ticket);
            }
        }

        public Ticket Get(Guid id)
        {
            return context.Tickets.Include(t => t.TicketType)
                .Include(t => t.TransactionHistory)
                .Include(t => t.User)
                .Include(t => t.TicketArea)
                .ThenInclude(t => t.Area)
                .FirstOrDefault(m => m.Id == id);
        }

        public IQueryable<Ticket> GetAll()
        {
            return context.Tickets.Include(t => t.TicketType)
               .Include(t => t.TransactionHistory)
               .Include(t => t.User);
        }

        public void Update(Ticket item)
        {
            context.Entry(item).State = EntityState.Modified;
        }
    }
}
