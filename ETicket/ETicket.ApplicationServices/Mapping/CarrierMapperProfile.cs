using AutoMapper;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;
namespace ETicket.ApplicationServices.Mapping
{
    public class CarrierMapperProfile : Profile
    {
        public CarrierMapperProfile()
        {
            CreateMap<CarrierDto, Carrier>().ReverseMap();
            CreateMap<Transport, TransportDto>()
                .ForMember(c => c.Carrier, c => c.MapFrom(x => x.Carriers.Name))
                .ForMember(r => r.Route, r => r.MapFrom(y => y.Route.Number))
                .ReverseMap();
        } 
    }
}
