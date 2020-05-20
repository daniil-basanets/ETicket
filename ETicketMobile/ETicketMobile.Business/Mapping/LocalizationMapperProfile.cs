using AutoMapper;

namespace ETicketMobile.Business.Mapping
{
    public class LocalizationMapperProfile : Profile
    {
        public LocalizationMapperProfile()
        {
            CreateMap<Model.UserAccount.Localization, Data.Entities.Localization>().ReverseMap();
        }
    }
}