using System.Collections.Generic;
using System.ComponentModel;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.DTOs
{
    public class StationDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DisplayName("Area")]
        public int AreaId { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public ICollection<RouteStation> RouteStations { get; set; }
    }
}
