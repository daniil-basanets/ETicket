using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETicket.DataAccess.Domain.Repositories
{
    public class RouteRepository : IRepository<Route, Guid>
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

        public void Delete(Guid id)
        {
            Route route = db.Routes.Find(id);
            if (route != null)
            {
                db.Routes.Remove(route);
            }
        }

        public Route Get(Guid id)
        {
            return db.Routes.Include(r => r.FirstStation)
                .Include(r => r.LastStation).FirstOrDefault(r => r.Id == id);
        }

        public IQueryable<Route> GetAll()
        {
            return db.Routes.Include(r => r.FirstStation).Include(r => r.LastStation);
        }

        public void Update(Route item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
