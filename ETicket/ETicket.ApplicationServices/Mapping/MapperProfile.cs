using AutoMapper;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicketAdmin.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<DocumentTypeDto, DocumentType>(); 
            CreateMap<DocumentDto, Document>(); 
            CreateMap<TicketTypeDto, TicketType>(); 
            CreateMap<UserDto, User>(); 
            CreateMap<TicketDto, Ticket>(); 
            CreateMap<Ticket, TicketDto>();
        }
        
    }
}