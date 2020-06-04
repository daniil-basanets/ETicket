using AutoMapper;
using ETicketMobile.Business.Model.Registration;
using ETicketMobile.WebAccess.DTO;

namespace ETicketMobile.Business.Mapping
{
    public class SignUpMapperProfile : Profile
    {
        public SignUpMapperProfile()
        {
            CreateMap<SignUpRequestDto, SignUp>().ReverseMap();

            CreateMap<SignUpResponseDto, SignUp>().ReverseMap();
        }
    }
}