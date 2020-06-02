using AutoMapper;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.ApplicationServices.Mapping
{
    public class RouteMapperProfile : Profile
    {
        public RouteMapperProfile()
        {
            CreateMap<RouteDto, Route>().ReverseMap()
                .ForMember(r=>r.FirstStationName,r=>r.MapFrom(s=>s.FirstStation.Name))
                .ForMember(r=>r.LastStationName,r=>r.MapFrom(s=>s.LastStation.Name));

            CreateMap<BaseRouteDto, Route>().ReverseMap();
        }        
    }
}
