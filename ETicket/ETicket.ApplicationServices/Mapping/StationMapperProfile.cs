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
        //TODO: Add List of Routes to mapper
        public StationMapperProfile()
        {
            CreateMap<StationDto, Station>().ReverseMap();
            //CreateMap<Station, StationDto>()
            //    .ForMember(d => d.AreaName, d => d.MapFrom(x => x.Area.Name))
            //    .ReverseMap();
        }
    }
}
