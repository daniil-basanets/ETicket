using System.Collections.Generic;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    interface ITransportService
    {
        public IEnumerable<Transport> GetAll();

        public Transport Get(long id);

        public void Create(TransportDto transportDto);

        public void Update(TransportDto transportDto);

        public void Delete(long id);

        public bool Exists(long id);
    }
}
