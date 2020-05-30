using System;
using AutoMapper;

namespace ETicketMobile.Business.Mapping
{
    public static class AutoMapperConfiguration
    {
        public static IMapper Mapper => mapper.Value;

        private static readonly Lazy<IMapper> mapper = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<SignUpMapperProfile>();
                cfg.AddProfile<TicketMapperProfile>();
                cfg.AddProfile<UserMapperProfile>();
                cfg.AddProfile<TokenMapperProfile>();
                cfg.AddProfile<TransactionMapperProfile>();
                cfg.AddProfile<LocalizationMapperProfile>();
            });

            var mapper = config.CreateMapper();
            return mapper;
        });
    }
}