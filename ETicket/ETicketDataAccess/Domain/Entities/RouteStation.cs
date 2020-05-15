using System.ComponentModel.DataAnnotations.Schema;

namespace ETicket.DataAccess.Domain.Entities
{
    public class RouteStation
    {
        public int RouteId { get; set; }

        [ForeignKey("RouteId")]
        public Route Route { get; set; }

        public int StationId { get; set; }

        [ForeignKey("StationId")]
        public Station Station { get; set; }
    }
}
