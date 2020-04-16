using AutoMapper;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Mapping
{
    public class TicketMapperProfile : Profile
    {
        public TicketMapperProfile()
        {
            CreateMap<TicketDto, Ticket>();
            CreateMap<Ticket, TicketDto>();
            CreateMap<TicketTypeDto, TicketType>();
            CreateMap<TicketType, TicketTypeDto>();
        }
    }
}