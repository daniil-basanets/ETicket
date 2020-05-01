using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ETicket.DataAccess.Domain.Repositories
{
    public class RouteStationRepository : IRepository<RouteStation, RouteStation>
    {
        #region Private members

        private readonly ETicketDataContext context;

        #endregion

        public RouteStationRepository(ETicketDataContext eTicketDataContext)
        {
            this.context = eTicketDataContext;
        }

        public void Create(RouteStation item)
        {
            context.RouteStations.Add(item);
        }

        public void Delete(RouteStation item)
        {
            if (item != null)
            {
                context.RouteStations.Remove(item);
            }
         
        }

        public RouteStation Get(RouteStation id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<RouteStation> GetAll()
        {
            return context.RouteStations.Include(r => r.Route)
                .Include(s => s.Station);
        }

        public void Update(RouteStation item)
        {
            context.Entry(item).State = EntityState.Modified;
        }
    }
}
