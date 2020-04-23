using System.Collections.Generic;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface ITicketTypeService
    {
        public IEnumerable<TicketType> GetAll();
        
        public TicketType Get(int id);

        public void Create(TicketTypeDto documentTypeDto);

        public void Update(TicketTypeDto documentTypeDto);

        public void Delete(int id);

        public bool Exists(int id);
    }
}