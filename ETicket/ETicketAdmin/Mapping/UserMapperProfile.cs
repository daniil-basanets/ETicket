using AutoMapper;
using ETicket.DataAccess.Domain.Entities;
using ETicketAdmin.DTOs;

namespace ETicketAdmin.Mapping
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<UserDto, User>();
        }
    }
}