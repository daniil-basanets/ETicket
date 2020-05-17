using System.Collections.Generic;
using System.Linq;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;

namespace ETicket.ApplicationServices.Services
{
    public class CarrierService : ICarrierService
    {
        #region Private members

        private readonly IUnitOfWork uow;
        private readonly MapperService mapper;

        #endregion

        public CarrierService(IUnitOfWork uow)
        {
            this.uow = uow;
            mapper = new MapperService();
        }

        public void Create(CarrierDto carrierDto)
        {
            var carrier = mapper.Map<CarrierDto, Carrier>(carrierDto);

            uow.Carriers.Create(carrier);
            uow.Save();
        }

        public void Delete(int id)
        {
            uow.Carriers.Delete(id);
            uow.Save();
        }

        public IEnumerable<Carrier> GetAll()
        {
            return uow.Carriers.GetAll().ToList();
        }

        public CarrierDto Get(int id)
        {
            var carrier = uow.Carriers.Get(id);

            return mapper.Map<Carrier, CarrierDto>(carrier);
        }

        public void Update(CarrierDto carrierDto)
        {
            var carrier = mapper.Map<CarrierDto, Carrier>(carrierDto);

            uow.Carriers.Update(carrier);
            uow.Save();
        }
    }
}
