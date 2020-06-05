using System.Collections.Generic;
using System.ComponentModel;

namespace ETicket.ApplicationServices.DTOs
{
    public class RouteDto
    {
        public int Id { get; set; }

        [DisplayName("Route number")]
        public string Number { get; set; }
                
        public int FirstStationId { get; set; }
        
        [DisplayName("First station")]
        public string FirstStationName { get; set; }
        
        [DisplayName("Last station")]
        public string LastStationName { get; set; }
                
        public int LastStationId { get; set; }

        public IList<int> StationIds { get; set; }

        public IList<string> StationNames { get; set; }

        public RouteDto()
        {
            StationIds = new List<int>();
            StationNames = new List<string>();
        }
    }
}
