using ETicket.DataAccess.Domain.Entities;
using System.Linq;
using ETicket.DataAccess.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ETicket.DataAccess.Domain.Repositories
{
    public class TransportRepository : IRepository<Transport,int>
    {
        #region

        private readonly ETicketDataContext context;

        #endregion

        public TransportRepository(ETicketDataContext eTicketDataContext)
        {
            context = eTicketDataContext;
        }

        public void Create(Transport item)
        {
            context.Transports.Add(item);
        }

        public void Delete(int id)
        {
            var transport = context.Transports.Find(id);

            if (transport != null)
            {
                context.Transports.Remove(transport);
            }
        }

        public Transport Get(int id)
        {
            return context.Transports
                .Include(r => r.Route)
                .Include(c => c.Carriers)
                .FirstOrDefault(t => t.Id == id);
        }

        public IQueryable<Transport> GetAll()
        {
            return context.Transports
                .Include(c => c.Carriers)
                .Include(r => r.Route);
        }

        public void Update(Transport item)
        {
            context.Update(item);
        }
    }
}
