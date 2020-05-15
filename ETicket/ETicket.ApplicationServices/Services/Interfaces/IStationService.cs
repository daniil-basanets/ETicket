using System;
using System.Collections.Generic;
using System.Text;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface IStationService
    {
        public IEnumerable<Station> GetAll();
        public Station Get(int id);

        public void Create(StationDto stationDto);

        public void Update(StationDto stationDto);

        public void Delete(int id);

        public bool Exists(int id);
    }
}
