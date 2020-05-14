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
        } 
    }
}
