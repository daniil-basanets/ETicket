using System.Collections.Generic;
using System.Linq;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;

namespace ETicket.ApplicationServices.Services 
{
    public class TicketTypeService : ITicketTypeService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly MapperService mapper;

        public TicketTypeService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            mapper = new MapperService();
        }
        
        public void Create(TicketTypeDto ticketTypeDto)
        {
            var ticketType = mapper.Map<TicketTypeDto,TicketType>(ticketTypeDto);
            
            unitOfWork.TicketTypes.Create(ticketType);
            unitOfWork.Save();
        }
        
        public IEnumerable<TicketTypeDto> GetTicketTypes()
        {
            return mapper.Map<IQueryable<TicketType>, IEnumerable<TicketTypeDto>>(unitOfWork.TicketTypes.GetAll()).ToList();
        }
        
        public TicketTypeDto GetTicketTypeById(int id)
        {
            return mapper.Map<TicketType, TicketTypeDto>(unitOfWork.TicketTypes.Get(id));
        }
        
        public void Update(TicketTypeDto ticketTypeDto)
        {
            var ticketType = mapper.Map<TicketTypeDto,TicketType>(ticketTypeDto);
            
            unitOfWork.TicketTypes.Update(ticketType);
            unitOfWork.Save();
        }
        
        public void Delete(int id)
        {
            unitOfWork.TicketTypes.Delete(id);
            unitOfWork.Save();
        }
    }
}
