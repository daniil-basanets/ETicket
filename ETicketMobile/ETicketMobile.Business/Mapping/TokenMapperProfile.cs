using AutoMapper;
using ETicketMobile.Data.Domain.Entities;
using ETicketMobile.WebAccess.DTO;

namespace ETicketMobile.Business.Mapping
{
    public class TokenMapperProfile : Profile
    {
        public TokenMapperProfile()
        {
            CreateMap<TokenDto, Token>();
            CreateMap<Token, TokenDto>();
        }
    }
}