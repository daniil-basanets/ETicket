using AutoMapper;
using ETicket.DataAccess.Domain.Entities;
using ETicketAdmin.DTOs;

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