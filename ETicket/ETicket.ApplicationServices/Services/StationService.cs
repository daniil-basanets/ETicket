using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.Validation;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;

namespace ETicket.ApplicationServices.Services
{
    public class StationService : IStationService
    {
        private readonly IUnitOfWork uow;
        private readonly MapperService mapper;
        private readonly StationValidator stationValidator;

        public StationService(IUnitOfWork uow)
        {
            this.uow = uow;
            mapper = new MapperService();
            stationValidator = new StationValidator();
        }

        public void Create(StationDto stationDto)
        {
            if (!stationValidator.Validate(stationDto).IsValid)
            {
                throw new ArgumentException(stationValidator.Validate(stationDto).Errors.First().ErrorMessage);
            }

            var station = mapper.Map<StationDto, Station>(stationDto);
            uow.Stations.Create(station);
            uow.Save();
        }

        public void Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "id should be greater than zero");
            }

            uow.Stations.Delete(id);
            uow.Save();
        }

        public StationDto Get(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "id should be greater than zero");
            }

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
            if (!stationValidator.Validate(stationDto).IsValid)
            {
                throw new ArgumentException(stationValidator.Validate(stationDto).Errors.First().ErrorMessage);
            }

            var station = mapper.Map<StationDto, Station>(stationDto);
            uow.Stations.Update(station);
            uow.Save();
        }
    }
}
