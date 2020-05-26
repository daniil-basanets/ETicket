using System.Linq;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ETicket.DataAccess.Domain.Repositories
{
    public class AreaRepository : IRepository<Area, int>
    {

        #region Private fields

        private readonly ETicketDataContext context;

        #endregion

        #region Constructors

        public AreaRepository(ETicketDataContext context)
        {
            this.context = context;
        }

        #endregion
        
        public IQueryable<Area> GetAll()
        {
            return context.Areas;
        }

        public Area Get(int id)
        {
            return context.Areas.Include(s => s.Stations)
                .FirstOrDefault(a => a.Id == id);
        }

        public void Create(Area area)
        {
            context.Areas.Add(area);
        }

        public void Update(Area area)
        {
            context.Entry(area).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var area = context.Areas.Find(id);
            
            if (area != null)
            {
                context.Areas.Remove(area);
            }
        }
    }
}