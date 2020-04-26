using AutoMapper;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Services
{
    public class MapperService
    {
        private readonly IMapper mapper;

        public MapperService()
        {
            mapper = ConfigureMapper();
        }

        private IMapper ConfigureMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<DocumentTypeDto, DocumentType>();
                cfg.CreateMap<DocumentDto, Document>();
                cfg.CreateMap<TicketTypeDto, TicketType>();
                cfg.CreateMap<UserDto, User>();
                cfg.CreateMap<TicketDto, Ticket>();
                cfg.CreateMap<Ticket, TicketDto>();
                cfg.CreateMap<CarrierDto, Carrier>();
                cfg.CreateMap<Carrier, CarrierDto>();
                cfg.CreateMap<Area, AreaDto>();
                cfg.CreateMap<RouteStationDto, RouteStation>();
                cfg.CreateMap<RouteStation, RouteStationDto>();
            });

            var mapper = config.CreateMapper();

            return mapper;
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return mapper.Map<TSource, TDestination>(source);
        }
    }
}