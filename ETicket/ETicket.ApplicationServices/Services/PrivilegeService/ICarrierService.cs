using System;
using System.Collections.Generic;
using System.Linq;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface ICarrierService
    {
        public IEnumerable<Carrier> GetAll();

        public IQueryable<Carrier> Get();

        public Carrier Get(int id);

        public CarrierDto GetDto(int id);

        public void Create(CarrierDto carrierDto);

        public void Update(CarrierDto carrierDto);

        public void Delete(int id);

        public bool Exists(int id);
    }
}