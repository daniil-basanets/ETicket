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
    public class TicketTypeService : ITicketTypeService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly MapperService mapper;
        private readonly TicketTypeValidator ticketTypeValidator;

        public TicketTypeService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            mapper = new MapperService();
            ticketTypeValidator = new TicketTypeValidator();
        }

        public void Create(TicketTypeDto ticketTypeDto)
        {
            if (!ticketTypeValidator.Validate(ticketTypeDto).IsValid)
            {
                throw new ArgumentException(ticketTypeValidator.Validate(ticketTypeDto).Errors.First().ErrorMessage);
            }

            var ticketType = mapper.Map<TicketTypeDto, TicketType>(ticketTypeDto);

            unitOfWork.TicketTypes.Create(ticketType);
            unitOfWork.Save();
        }
        
        public IEnumerable<TicketTypeDto> GetTicketTypes()
        {
            var ticketTypes = unitOfWork.TicketTypes.GetAll();
            
            return mapper.Map<IQueryable<TicketType>, IEnumerable<TicketTypeDto>>(ticketTypes).ToList();
        }

        public TicketTypeDto GetTicketTypeById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id),"id should be greater than zero");
            }

            return mapper.Map<TicketType, TicketTypeDto>(unitOfWork.TicketTypes.Get(id));
        }

        public void Update(TicketTypeDto ticketTypeDto)
        {
            if (!ticketTypeValidator.Validate(ticketTypeDto).IsValid)
            {
                throw new ArgumentException(ticketTypeValidator.Validate(ticketTypeDto).Errors.First().ErrorMessage);
            }

            var ticketType = mapper.Map<TicketTypeDto, TicketType>(ticketTypeDto);

            unitOfWork.TicketTypes.Update(ticketType);
            unitOfWork.Save();
        }

        public void Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id),"id should be greater than zero");
            }
            
            unitOfWork.TicketTypes.Delete(id);
            unitOfWork.Save();
        }
    }
}