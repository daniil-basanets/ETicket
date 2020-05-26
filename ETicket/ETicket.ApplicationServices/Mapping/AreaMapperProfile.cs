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
            CreateMap<PriceListDto, PriceList>().ReverseMap();
        }
    }
}