using AutoMapper;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Mapping
{
    public class DocumentMapperProfile : Profile
    {
        public DocumentMapperProfile()
        {
            CreateMap<DocumentDto, Document>();
            CreateMap<Document, DocumentDto>();
            CreateMap<DocumentTypeDto, DocumentType>();
            CreateMap<DocumentType, DocumentTypeDto>();
        }
    }
}