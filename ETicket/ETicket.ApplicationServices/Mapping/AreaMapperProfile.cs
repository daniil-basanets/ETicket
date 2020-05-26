using AutoMapper;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Mapping
{
    public class AreaMapperProfile : Profile
    {
        public AreaMapperProfile()
        {
            CreateMap<AreaDto, Area>().ReverseMap();
            CreateMap<PriceListDto, PriceList>().ReverseMap()
                .ForMember(d => d.Area, d => d.MapFrom(x => x.Area.Name));
        }
    }
}