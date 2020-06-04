using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ETicket.ApplicationServices.Services
{
    public class PriceListService:IPriceListService
    {
        #region private members

        private readonly IUnitOfWork uow;

        private readonly MapperService mapper;

        #endregion

        public PriceListService(IUnitOfWork uow)
        {
            this.uow = uow;
            mapper = new MapperService();
        }

        public IEnumerable<PriceListDto> GetAll()
        {
            var priceLists = uow.PriceList.GetAll();
            return mapper.Map<IQueryable<PriceList>, IEnumerable<PriceListDto>>(priceLists).ToList();
        }

        public PriceListDto Get(int id)
        {
            return mapper.Map<PriceList, PriceListDto>(uow.PriceList.Get(id));
        }

        public void Create(PriceListDto priceListDto)
        {
            var priceList = mapper.Map<PriceListDto, PriceList>(priceListDto);
            uow.PriceList.Create(priceList);
            uow.Save();
        }

        public bool Exists(int id)
        {
            return uow.PriceList.Get(id) != null;
        }

    }
}

