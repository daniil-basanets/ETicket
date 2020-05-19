using System.ComponentModel;

namespace ETicket.ApplicationServices.DTOs
{
    public class TransportDto
    {
        public int Id { get; set; }

        public string Number { get; set; }

        [DisplayName("Carrier")]
        public int CarriersId { get; set; }

        [DisplayName("Carrier")]
        public string Carrier { get; set; }

        [DisplayName("Route")]
        public int RouteId { get; set; }

        [DisplayName("Route")]
        public string Route { get; set; }
    }
}
