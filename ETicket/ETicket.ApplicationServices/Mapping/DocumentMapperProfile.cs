using AutoMapper;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Mapping
{
    public class DocumentMapperProfile : Profile
    {
        public DocumentMapperProfile()
        {
            CreateMap<DocumentDto, Document>().ReverseMap()
                .ForMember(d=>d.DocumentTypeName,d=>d.MapFrom(t=>t.DocumentType.Name));
            CreateMap<DocumentTypeDto, DocumentType>().ReverseMap();
        }
    }
}