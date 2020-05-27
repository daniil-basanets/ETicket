using System.ComponentModel;

namespace ETicket.ApplicationServices.DTOs
{
    public class TransportDto
    {
        public int Id { get; set; }

        [DisplayName("Vehicle number")]
        public string VehicleNumber { get; set; }

        public int CarrierId { get; set; }

        [DisplayName("Carrier")]
        public string Carrier { get; set; }

        public int RouteId { get; set; }

        [DisplayName("Route")]
        public string Route { get; set; }
    }
}
