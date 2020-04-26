using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;
using System.Collections.Generic;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface IPriceListService
    {
        public IEnumerable<PriceList> GetAll();
        public PriceList Get(int id);
        public void Create(PriceListDto priceListDto);
        public bool Exists(int id);
    }
}
