using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETicket.ApplicationServices.Services
{
    public class RouteService : IRouteService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly MapperService mapper;

        public RouteService(IUnitOfWork uow)
        {
            unitOfWork = uow;

            mapper = new MapperService();
        }

        public void Create(RouteDto routeDto)
        {
            var routeService = mapper.Map<RouteDto, Route>(routeDto);
            unitOfWork.Routes.Create(routeService);
            unitOfWork.Save();
        }

        public IEnumerable<RouteDto> GetRoutes()
        {
            var routes= unitOfWork.Routes.GetAll();
            
            return mapper.Map<IQueryable<Route>, IEnumerable<RouteDto>>(routes).ToList();
        }       

        public RouteDto GetRouteById(int id)
        {
            var route = unitOfWork.Routes.Get(id);

            return mapper.Map<Route, RouteDto>(route);
        }

        public void Update(RouteDto routeDto)
        {
            var routeService = mapper.Map<RouteDto, Route>(routeDto);
            unitOfWork.Routes.Update(routeService);
            unitOfWork.Save();
        }

        public void Delete(int id)
        {
            unitOfWork.Routes.Delete(id);
            unitOfWork.Save();
        }
        
        public bool Exists(int id)
        {
            return unitOfWork.Routes.Get(id) != null;
        }
    }
}
