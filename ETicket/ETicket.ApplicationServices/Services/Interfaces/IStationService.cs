using System;
using System.Collections.Generic;
using System.Text;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface IStationService
    {
        public IEnumerable<StationDto> GetAll();
        public StationDto Get(int id);

        public void Create(StationDto stationDto);

        public void Update(StationDto stationDto);

        public void Delete(int id);
    }
}
