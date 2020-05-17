using AutoMapper;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Mapping
{
    public class TicketMapperProfile : Profile
    {
        public TicketMapperProfile()
        {
            CreateMap<TicketDto, Ticket>().ReverseMap();
            CreateMap<TicketTypeDto, TicketType>().ReverseMap();
            CreateMap<TicketVerificationDto, TicketVerification>()
                .ReverseMap()
                .ForMember(d => d.TransportNumber, d => d.MapFrom(x => x.Transport.Number))
                .ForMember(d => d.StationName, d => d.MapFrom(x => x.Station.Name));

        }
    }
}