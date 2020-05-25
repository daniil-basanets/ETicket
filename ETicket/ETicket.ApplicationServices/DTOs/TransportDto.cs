using System.ComponentModel;

namespace ETicket.ApplicationServices.DTOs
{
    public class TransportDto
    {
        public int Id { get; set; }

        public string VehicleNumber { get; set; }

        [DisplayName("Carrier")]
        public int CarrierId { get; set; }

        [DisplayName("Carrier")]
        public string Carrier { get; set; }

        [DisplayName("Route")]
        public int RouteId { get; set; }

        [DisplayName("Route")]
        public string Route { get; set; }
    }
}
