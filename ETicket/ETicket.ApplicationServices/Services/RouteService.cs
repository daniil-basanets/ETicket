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
            Save();
        }

        public void Delete(Guid id)
        {
            unitOfWork.Routes.Delete(id);
            Save();
        }

        public bool Exists(Guid id)
        {
            return unitOfWork.Routes.Get(id) != null;
        }

        public IQueryable<Route> Read()
        {
            return unitOfWork.Routes.GetAll();
        }

        public Route Read(Guid id)
        {
            return unitOfWork.Routes.Get(id);
        }

        public void Update(RouteDto routeDto)
        {
            var routeService = mapper.Map<RouteDto, Route>(routeDto);
            unitOfWork.Routes.Update(routeService);
            Save();
        }

        public void Save()
        {
            unitOfWork.Save();
        }
    }
}
