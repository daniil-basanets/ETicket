using AutoMapper;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicketAdmin.Mapping
{
    public class TransactionHistoryMapperProfile : Profile
    {
        public TransactionHistoryMapperProfile()
        {
            CreateMap<TransactionHistoryDto, TransactionHistory>();
        }
    }
}