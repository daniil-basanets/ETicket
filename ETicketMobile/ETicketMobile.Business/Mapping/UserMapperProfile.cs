using AutoMapper;
using ETicketMobile.Business.Model.Registration;
using ETicketMobile.WebAccess.DTO;

namespace ETicketMobile.Business.Mapping
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<UserSignInRequestDto, User>().ReverseMap();

            CreateMap<UserSignUpRequestDto, User>().ReverseMap();

            CreateMap<UserSignUpResponseDto, User>().ReverseMap();
        }
    }
}