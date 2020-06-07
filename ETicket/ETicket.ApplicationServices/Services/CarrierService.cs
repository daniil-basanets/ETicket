using System;
using System.Collections.Generic;
using System.Linq;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.Validation;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;

namespace ETicket.ApplicationServices.Services
{
    public class CarrierService : ICarrierService
    {
        #region Private members

        private readonly IUnitOfWork uow;
        private readonly MapperService mapper;
        private readonly CarrierValidator carrierValidator;

        #endregion

        public CarrierService(IUnitOfWork uow)
        {
            this.uow = uow;
            mapper = new MapperService();
            carrierValidator = new CarrierValidator();
        }

        public void Create(CarrierDto carrierDto)
        {
            var validationResult = carrierValidator.Validate(carrierDto);

            if (!validationResult.IsValid)
            {
                throw new ArgumentException(validationResult.Errors.First().ErrorMessage);
            }

            var carrier = mapper.Map<CarrierDto, Carrier>(carrierDto);

            uow.Carriers.Create(carrier);
            uow.Save();
        }

        public void Delete(int id)
        {
            uow.Carriers.Delete(id);
            uow.Save();
        }

        public IEnumerable<CarrierDto> GetAll()
        {
            var carriers = uow.Carriers.GetAll();

            return mapper.Map<IQueryable<Carrier>, IEnumerable<CarrierDto>>(carriers).ToList();
        }

        public CarrierDto Get(int id)
        {
            var carrier = uow.Carriers.Get(id);

            return mapper.Map<Carrier, CarrierDto>(carrier);
        }

        public void Update(CarrierDto carrierDto)
        {
            var validationResult = carrierValidator.Validate(carrierDto);

            if (!validationResult.IsValid)
            {
                throw new ArgumentException(validationResult.Errors.First().ErrorMessage);
            }

            var carrier = mapper.Map<CarrierDto, Carrier>(carrierDto);

            uow.Carriers.Update(carrier);
            uow.Save();
        }
    }
}
