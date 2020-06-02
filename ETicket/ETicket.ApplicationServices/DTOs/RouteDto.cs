using System.ComponentModel;

namespace ETicket.ApplicationServices.DTOs
{
    public class RouteDto : BaseRouteDto
    {
        [DisplayName("First station")]
        public int FirstStationId { get; set; }
        
        [DisplayName("First station")]
        public string FirstStationName { get; set; }
        
        [DisplayName("Last station")]
        public string LastStationName { get; set; }

        [DisplayName("Last station")]
        public int LastStationId { get; set; }
    }
}
