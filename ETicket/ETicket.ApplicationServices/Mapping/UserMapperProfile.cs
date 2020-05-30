using AutoMapper;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.PagingServices.Models;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Mapping
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<UserDto, User>()
                .ReverseMap()
                .ForMember(d => d.PrivilegeName, d => d.MapFrom(x => x.Privilege.Name))
                .ForMember(d => d.DocumentNumber, d => d.MapFrom(x => x.Document.Number));
            CreateMap<DataTablePage<User>, DataTablePage<UserDto>>();
        }
    }
}