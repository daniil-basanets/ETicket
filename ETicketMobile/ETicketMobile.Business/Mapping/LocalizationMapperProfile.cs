using AutoMapper;
using ETicketMobile.Business.Model.UserAccount;

namespace ETicketMobile.Business.Mapping
{
    public class LocalizationMapperProfile : Profile
    {
        public LocalizationMapperProfile()
        {
            CreateMap<Localization, Data.Entities.Localization>().ReverseMap();
        }
    }
}