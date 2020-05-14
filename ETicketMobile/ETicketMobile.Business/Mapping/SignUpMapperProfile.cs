using AutoMapper;
using ETicketMobile.Business.Model.Registration;
using ETicketMobile.WebAccess.DTO;

namespace ETicketMobile.Business.Mapping
{
    public class SignUpMapperProfile : Profile
    {
        public SignUpMapperProfile()
        {
            CreateMap<SignUpRequestDto, SignUp>();
            CreateMap<SignUp, SignUpRequestDto>();

            CreateMap<SignUpResponseDto, SignUp>();
            CreateMap<SignUp, SignUpResponseDto>();
        }
    }
}