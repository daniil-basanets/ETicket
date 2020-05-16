using System.ComponentModel;

namespace ETicket.ApplicationServices.DTOs
{
    public class TransportDto
    {
        public long Id { get; set; }

        [DisplayName("Carrier")]
        public int CarrierId { get; set; }

        [DisplayName("Route")]
        public int RouteId { get; set; }
    }
}
