using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.PagingServices.Models;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Mapping
{
    public class TicketMapperProfile : Profile
    {
        public TicketMapperProfile()
        {
            CreateMap<Ticket, TicketDto>()
                       .ForMember(d => d.TicketTypeName, d => d.MapFrom(x => x.TicketType.TypeName))
                       .ForMember(d => d.TransactionRRN, d => d.MapFrom(s => s.TransactionHistory.ReferenceNumber))
                       .ForMember(d => d.UserName, d => d.MapFrom(s => s.User.FirstName + ' ' + s.User.LastName))
                       .ForMember(d => d.Areas, d => d.MapFrom(s => s.TicketArea.Select(x => new KeyValuePair<int, string>(x.Area.Id, x.Area.Name))));
            CreateMap<TicketDto, Ticket>();
            CreateMap<Ticket, TicketApiDto>()
                        .ForMember(d => d.TicketTypeName, d => d.MapFrom(x => x.TicketType.TypeName))
                       .ForMember(d => d.TransactionRRN, d => d.MapFrom(s => s.TransactionHistory.ReferenceNumber))
                       .ForMember(d => d.Areas, d => d.MapFrom(s => s.TicketArea.Select(x => x.Area.Name)))
                       .ReverseMap();
            CreateMap<DataTablePage<Ticket>, DataTablePage<TicketDto>>();

            CreateMap<TicketTypeDto, TicketType>().ReverseMap();
            
            CreateMap<TicketVerificationDto, TicketVerification>()
                .ReverseMap()
                .ForMember(d => d.TransportNumber, d => d.MapFrom(x => x.Transport.VehicleNumber))
                .ForMember(d => d.StationName, d => d.MapFrom(x => x.Station.Name));
            CreateMap<DataTablePage<TicketVerification>, DataTablePage<TicketVerificationDto>>();
        }
    }
}
