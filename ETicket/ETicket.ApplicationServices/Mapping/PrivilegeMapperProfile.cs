using AutoMapper;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

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