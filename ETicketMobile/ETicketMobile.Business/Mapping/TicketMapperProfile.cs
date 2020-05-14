using AutoMapper;
using ETicketMobile.Business.Model;
using ETicketMobile.Business.Model.Tickets;
using ETicketMobile.WebAccess;

namespace ETicketMobile.Business.Mapping
{
    public class TicketMapperProfile : Profile
    {
        public TicketMapperProfile()
        {
            CreateMap<TicketDto, Ticket>();
            CreateMap<Ticket, TicketDto>();
        }
    }
}