using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Mapping
{
    public class StationMapperProfile : Profile
    { 
        public StationMapperProfile()
        {
            CreateMap<StationDto, Station>()
                .ReverseMap()
                .ForMember(d => d.AreaName, d => d.MapFrom(x => x.Area.Name));
        }
    }
}
