using AutoMapper;
using ETicketMobile.Business.Model.Tickets;
using ETicketMobile.WebAccess.DTO;

namespace ETicketMobile.Business.Mapping
{
    public class AreasMapperProfile : Profile
    {
        public AreasMapperProfile()
        {
            CreateMap<Area, AreaDto>().ReverseMap();
        }
    }
}