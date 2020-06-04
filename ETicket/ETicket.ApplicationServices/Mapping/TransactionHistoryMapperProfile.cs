using AutoMapper;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.PagingServices.Models;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Mapping
{
    public class TransactionHistoryMapperProfile : Profile
    {
        public TransactionHistoryMapperProfile()
        {
            CreateMap<TransactionHistoryDto, TransactionHistory>().ReverseMap();
            CreateMap<DataTablePage<TransactionHistory>, DataTablePage<TransactionHistoryDto>>();
        }
    }
}