using AutoMapper;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Mapping
{
    public class DocumentMapperProfile : Profile
    {
        public DocumentMapperProfile()
        {
            CreateMap<DocumentDto, Document>().ReverseMap();
            CreateMap<DocumentTypeDto, DocumentType>().ReverseMap();
        }
    }
}