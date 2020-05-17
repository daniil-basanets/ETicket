using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface IRouteService
    {
        public IEnumerable<Route> GetRoutes();

        public Route GetRouteById(int id);

        public void Create(RouteDto documentTypeDto);

        public void Update(RouteDto documentTypeDto);

        public void Delete(int id);

        public bool Exists(int id);
    }
}
