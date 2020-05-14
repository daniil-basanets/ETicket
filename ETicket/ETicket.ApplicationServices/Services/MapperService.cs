using AutoMapper;
using ETicket.ApplicationServices.Mapping;

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
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TicketMapperProfile>();
                cfg.AddProfile<DocumentMapperProfile>();
                cfg.AddProfile<PrivilegeMapperProfile>();
                cfg.AddProfile<UserMapperProfile>();
                cfg.AddProfile<TransactionHistoryMapperProfile>();
            });

            return config.CreateMapper();
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return mapper.Map<TSource, TDestination>(source);
        }
    }
}