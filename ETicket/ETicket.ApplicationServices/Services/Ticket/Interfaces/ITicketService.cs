using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface ITicketService
    {
        public IEnumerable<Ticket> GetAll();
        public Ticket Get(Guid id);

        public void Create(Ticket ticket);

        public void Update(Ticket ticket);

        public void Delete(Guid id);

        public bool Exists(Guid id);
    }
}
