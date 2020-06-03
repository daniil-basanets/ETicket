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

        public StationDto Get(int id)
        {
            var station = uow.Stations.Get(id);
            return mapper.Map<Station, StationDto>(station);
        }

        public IEnumerable<StationDto> GetAll()
        {
            var stations = uow.Stations.GetAll();
            
            return mapper.Map<IQueryable<Station>, IEnumerable<StationDto>>(stations).ToList();
        }

        public void Update(StationDto stationDto)
        {
            var station = mapper.Map<StationDto, Station>(stationDto);
            uow.Stations.Update(station);
            uow.Save();
        }
    }
}
