using AutoMapper;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETicket.ApplicationServices.Mapping
{
    public class RouteMapperProfile : Profile
    {
        public RouteMapperProfile()
        {
            CreateMap<RouteDto, Route>().ReverseMap()
                .ForMember(r=>r.FirstStationName,r=>r.MapFrom(s=>s.FirstStation.Name))
                .ForMember(r=>r.LastStationName,r=>r.MapFrom(s=>s.LastStation.Name))
                .ForMember(r=>r.StationIds,r=>r.MapFrom(s=>s.RouteStations.OrderBy(z => z.StationOrderNumber).Select(x => x.StationId)))
                .ForMember(r=>r.StationNames,r=>r.MapFrom(s => s.RouteStations.OrderBy(z => z.StationOrderNumber).Select(x => x.Station.Name)));
        }        
    }
}
