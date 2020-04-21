using AutoMapper;
using ETicket.DataAccess.Domain.Entities;
using ETicketAdmin.DTOs;

namespace ETicketAdmin.Mapping
{
    public class PrivilegeMapperProfile : Profile
    {
        public PrivilegeMapperProfile()
        {
            CreateMap<PrivilegeDto, Privilege>();
        }
    }
}