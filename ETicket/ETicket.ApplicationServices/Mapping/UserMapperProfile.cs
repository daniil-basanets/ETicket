using AutoMapper;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

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