using ETicket.ApplicationServices.DTOs;
using System.Collections.Generic;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface IRouteService
    {
        public IEnumerable<RouteDto> GetRoutes();

        public RouteDto GetRouteById(int id);

        public void Create(RouteDto documentTypeDto);

        public void Update(RouteDto documentTypeDto);

        public void Delete(int id);

        public bool Exists(int id);
    }
}
