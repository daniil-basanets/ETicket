using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETicket.DataAccess.Domain.Entities
{
    public class Transport
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Vehicle number")]
        [MaxLength(25)]
        [Required]
        public string VehicleNumber { get; set; }
        public int CarriersId { get; set; }

        [ForeignKey("CarriersId")]
        [DisplayName("Carrier")]
        [Required]
        public Carrier Carriers { get; set; }

        public int RouteId { get; set; }

        [ForeignKey("RouteId")]
        [DisplayName("Route")]
        [Required]
        public Route Route { get; set; }
    }
}
