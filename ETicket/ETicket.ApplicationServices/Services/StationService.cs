using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;

namespace ETicket.ApplicationServices.Services
{
    public class StationService : IStationService
    {
        private readonly IUnitOfWork uow;
        private readonly MapperService mapper;

        public StationService(IUnitOfWork uow)
        {
            this.uow = uow;
            mapper = new MapperService();
        }

        public void Create(StationDto stationDto)
        {
            var station = mapper.Map<StationDto, Station>(stationDto);
            uow.Stations.Create(station);
            uow.Save();
        }

        public void Delete(int id)
        {
            uow.Stations.Delete(id);
            uow.Save();
        }

        public bool Exists(int id)
        {
            return uow.Stations.StationExists(id);
        }

        public Station Get(int id)
        {
            return uow.Stations.Get(id);
        }

        public IEnumerable<Station> GetAll()
        {
            return uow.Stations.GetAll().ToList();
        }

        public void Update(StationDto stationDto)
        {
            var station = mapper.Map<StationDto, Station>(stationDto);
            uow.Stations.Update(station);
            uow.Save();
        }
    }
}
