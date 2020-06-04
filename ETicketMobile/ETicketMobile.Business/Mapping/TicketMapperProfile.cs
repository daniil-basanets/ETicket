using AutoMapper;
using ETicketMobile.Business.Model.Tickets;
using ETicketMobile.WebAccess;
using ETicketMobile.WebAccess.DTO;

namespace ETicketMobile.Business.Mapping
{
    public class TicketMapperProfile : Profile
    {
        public TicketMapperProfile()
        {
            CreateMap<TicketTypeDto, TicketType>().ReverseMap();
            CreateMap<TicketDto, Ticket>().ReverseMap();
        }
    }
}