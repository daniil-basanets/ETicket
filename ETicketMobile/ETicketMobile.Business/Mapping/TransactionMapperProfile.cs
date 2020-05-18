using AutoMapper;
using ETicketMobile.Business.Model.Transactions;
using ETicketMobile.WebAccess.DTO;

namespace ETicketMobile.Business.Mapping
{
    public class TransactionMapperProfile : Profile
    {
        public TransactionMapperProfile()
        {
            CreateMap<TransactionDto, Transaction>().ReverseMap();
        }
    }
}