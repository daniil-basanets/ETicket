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
            CreateMap<RouteDto, Route>().ReverseMap();
            CreateMap<TransportDto, Transport>().ReverseMap();
        }        
    }
}
