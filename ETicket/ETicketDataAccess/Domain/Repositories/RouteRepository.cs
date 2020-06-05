using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETicket.DataAccess.Domain.Repositories
{
    public class RouteRepository : IRepository<Route, int>
    {
        private readonly ETicketDataContext db;

        public RouteRepository(ETicketDataContext context)
        {
            db = context;
        }

        public void Create(Route item)
        {
            db.Routes.Add(item);
        }

        public void Delete(int id)
        {
            Route route = db.Routes.Find(id);
            if (route != null)
            {
                db.Routes.Remove(route);
            }
        }

        public Route Get(int id)
        {
            return db.Routes.Include(r => r.FirstStation)
                            .Include(r => r.LastStation)
                            .Include(r => r.RouteStations)
                            .ThenInclude(r => r.Station)
                            .FirstOrDefault(r => r.Id == id);
        }

        public IQueryable<Route> GetAll()
        {
            return db.Routes.Include(r => r.FirstStation)
                            .Include(r => r.LastStation)
                            .Include(r => r.RouteStations)
                            .ThenInclude(r => r.Station);
        }

        public void Update(Route item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
