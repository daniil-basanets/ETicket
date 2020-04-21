using AutoMapper;
using ETicket.DataAccess.Domain.Entities;
using ETicketAdmin.DTOs;

namespace ETicketAdmin.Mapping
{
    public class TicketMapperProfile : Profile
    {
        public TicketMapperProfile()
        {
            CreateMap<TicketDto, Ticket>();
            CreateMap<TicketTypeDto, TicketType>();
        }
    }
}