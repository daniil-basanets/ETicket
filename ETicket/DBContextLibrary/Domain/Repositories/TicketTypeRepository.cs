using System.Collections.Generic;
using DBContextLibrary.Domain.Entities;
using DBContextLibrary.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DBContextLibrary.Domain.Repositories
{
    public class TicketTypeRepository : IRepository<TicketType>
    {
        private readonly ETicketDataContext context;

        public TicketTypeRepository(ETicketDataContext context)
        {
            this.context = context;
        }
        
        public IEnumerable<TicketType> GetAll()
        {
            return context.TicketTypes;
        }

        public TicketType Get(int id)
        {
            return context.TicketTypes.Find(id);
        }

        public void Create(TicketType ticketType)
        {
            context.TicketTypes.Add(ticketType);
        }

        public void Update(TicketType ticketType)
        {
            context.Entry(ticketType).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var ticketType = context.TicketTypes.Find(id);
            
            if (ticketType != null)
            {
                context.TicketTypes.Remove(ticketType);
            }
        }
    }
}