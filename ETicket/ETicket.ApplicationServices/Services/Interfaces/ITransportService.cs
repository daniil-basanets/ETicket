using System.Collections.Generic;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface ITransportService
    {
        public IEnumerable<TransportDto> GetAll();

        public TransportDto Get(int id);

        public void Create(TransportDto transportDto);

        public void Update(TransportDto transportDto);

        public void Delete(int id);
    }
}
