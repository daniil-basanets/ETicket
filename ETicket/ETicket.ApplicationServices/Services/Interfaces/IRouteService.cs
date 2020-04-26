using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    interface IRouteService
    {
        public IQueryable<Route> Read();
        public Route Read(Guid id);

        public void Create(RouteDto documentTypeDto);

        public void Update(RouteDto documentTypeDto);

        public void Delete(Guid id);

        public bool Exists(Guid id);
    }
}
