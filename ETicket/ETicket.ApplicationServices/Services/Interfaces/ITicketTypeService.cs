using System.Collections.Generic;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface ITicketTypeService
    {
        public IEnumerable<TicketTypeDto> GetTicketTypes();
        
        public TicketTypeDto GetTicketTypeById(int id);

        public void Create(TicketTypeDto documentTypeDto);

        public void Update(TicketTypeDto documentTypeDto);

        public void Delete(int id);
    }
}