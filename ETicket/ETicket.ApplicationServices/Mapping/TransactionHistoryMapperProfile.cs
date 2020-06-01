using AutoMapper;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Mapping
{
    public class TransactionHistoryMapperProfile : Profile
    {
        public TransactionHistoryMapperProfile()
        {
            CreateMap<TransactionHistoryDto, TransactionHistory>();
            CreateMap<PriceListDto, PriceList>().ReverseMap();
        }
    }
}