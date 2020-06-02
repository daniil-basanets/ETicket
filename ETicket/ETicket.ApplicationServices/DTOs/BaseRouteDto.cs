using System.ComponentModel;

namespace ETicket.ApplicationServices.DTOs
{
    public class BaseRouteDto
    {
        public int Id { get; set; }

        [DisplayName("Route number")]
        public string Number { get; set; }
    }
}
