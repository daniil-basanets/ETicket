using AutoMapper;
using ETicketMobile.Business.Model.Registration;
using ETicketMobile.WebAccess.DTO;

namespace ETicketMobile.Business.Mapping
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<UserSignInRequestDto, User>();
            CreateMap<User, UserSignInRequestDto>();

            CreateMap<UserSignUpRequestDto, User>();
            CreateMap<User, UserSignUpRequestDto>();

            CreateMap<UserSignUpResponseDto, User>();
            CreateMap<User, UserSignUpResponseDto>();
        }
    }
}