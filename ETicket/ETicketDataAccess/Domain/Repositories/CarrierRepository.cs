using System.Linq;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ETicket.DataAccess.Domain.Repositories
{
    public class CarrierRepository : IRepository<Carrier, int>
    {
        #region Private members

        private readonly ETicketDataContext context;

        #endregion

        public CarrierRepository(ETicketDataContext eTicketDataContext)
        {
            context = eTicketDataContext;
        }

        public void Create(Carrier item)
        {
            context.Carriers.Add(item);
        }

        public void Delete(int id)
        {
            var carrier = Get(id);
            if (carrier != null)
            {
                context.Carriers.Remove(carrier);
            }
        }

        public Carrier Get(int id)
        {
            return context.Carriers.FirstOrDefault(m => m.Id == id);
        }

        public IQueryable<Carrier> GetAll()
        {
            return context.Carriers;
        }

        public void Update(Carrier item)
        {
            context.Entry(item).State = EntityState.Modified;
        }
    }
}
