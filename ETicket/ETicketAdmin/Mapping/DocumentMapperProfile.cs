using AutoMapper;
using ETicket.DataAccess.Domain.Entities;
using ETicketAdmin.DTOs;

namespace ETicketAdmin.Mapping
{
    public class DocumentMapperProfile:Profile
    {
        public DocumentMapperProfile()
        {
            CreateMap<DocumentDto, Document>();
            CreateMap<DocumentTypeDto, DocumentType>();
        }
    }
}