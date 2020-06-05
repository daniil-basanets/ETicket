using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ETicket.DataAccess.Domain.Repositories
{
    public class RouteStationRepository : IRepository<RouteStation, int>
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

        public void Delete(int routeId)
        {
            var routeStationList = context.RouteStations.Where(m => m.RouteId == routeId);

            if (routeStationList != null)
            {
                context.RemoveRange(routeStationList);
            }
        }

        public void DeleteRouteFromStations(int routeId)
        {
            var routeStationList = context.RouteStations.Where(m => m.RouteId == routeId);

            if (routeStationList != null)
            {
                context.RemoveRange(routeStationList);
            }
        }

        public void DeleteStationFromRoutes(int stationId)
        {
            var routeStationList = context.RouteStations.Where(m => m.RouteId == stationId);

            if (routeStationList != null)
            {
                context.RemoveRange(routeStationList);
            }
        }

        public RouteStation Get(int id)
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
