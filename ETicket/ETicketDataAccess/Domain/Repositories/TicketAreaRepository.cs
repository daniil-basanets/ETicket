using System;
using System.Linq;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ETicket.DataAccess.Domain.Repositories
{
    public class TicketAreaRepository : IRepository<TicketArea, TicketArea>
    {
        #region 

        private readonly ETicketDataContext context;

        #endregion

        public TicketAreaRepository(ETicketDataContext eTicketDataContext)
        {
            context = eTicketDataContext;
        }

        public void Create(TicketArea item)
        {
            context.Add(item);
        }

        public void Delete(TicketArea item)
        {
            if (item != null)
            {
                context.TicketAreas.Remove(item);
            }
        }

        public TicketArea Get(TicketArea id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TicketArea> GetAll()
        {
            return context.TicketAreas.Include(m=> m.Ticket)
                .Include(s => s.Area);
        }

        public void Update(TicketArea item)
        {
            context.Entry(item).State = EntityState.Modified;
        }
    }
}
