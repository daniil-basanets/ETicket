using ETicket.DataAccess.Domain.Entities;
using System.Linq;
using ETicket.DataAccess.Domain.Interfaces;

namespace ETicket.DataAccess.Domain.Repositories
{
    public class TransportRepository : IRepository<Transport,long>
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
            return context.Transports.FirstOrDefault(t => t.Id == id);
        }

        public IQueryable<Transport> GetAll()
        {
            return context.Transports;
        }

        public void Update(Transport item)
        {
            context.Update(item);
        }
    }
}
