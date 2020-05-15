using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ETicket.DataAccess.Domain.Repositories
{
    public class StationRepository : IRepository<Station, int>
    {
        private readonly ETicketDataContext context;

        public StationRepository(ETicketDataContext context)
        {
            this.context = context;
        }

        public void Create(Station item)
        {
            context.Stations.Add(item);
        }

        public void Delete(int id)
        {
            var station = context.Stations.Find(id);

            if (station != null)
            {
                context.Stations.Remove(station);
            }
        }

        public Station Get(int id)
        {
            var station = context.Stations
                 .Include(s => s.Area)
                 .FirstOrDefault(m => m.Id == id);
            return station;
        }

        public IQueryable<Station> GetAll()
        {
            return context.Stations.Include(s => s.Area);
        }

        public void Update(Station item)
        {
            context.Update(item);
        }
    }
}
