using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETicket.DataAccess.Domain.Entities
{
    public class Transport
    {
        [Key]
        public long Id { get; set; }

        [DisplayName("Number")]
        [Required]
        public string Number { get; set; }

        [DisplayName("Carrier")]
        public int CarriersId { get; set; }

        [ForeignKey("CarriersId")]
        [Required]
        public Carrier Carriers { get; set; }

        [DisplayName("Route")]
        public int RouteId { get; set; }

        [ForeignKey("RouteId")]
        [Required]
        public Route Route { get; set; }
    }
}
