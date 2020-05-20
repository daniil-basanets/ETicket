using AutoMapper;
using ETicketMobile.Data.Entities;
using ETicketMobile.WebAccess.DTO;

namespace ETicketMobile.Business.Mapping
{
    public class TokenMapperProfile : Profile
    {
        public TokenMapperProfile()
        {
            CreateMap<TokenDto, Token>().ReverseMap();
        }
    }
}