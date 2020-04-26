using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETicket.DataAccess.Domain.Entities
{
    public class Transport
    {
        [Key]
        public long Id { get; set; }

        public int CarriersId { get; set; }

        [ForeignKey("CarriersId")]
        [Required]
        public Carrier Carriers { get; set; }

        public int RouteId { get; set; }

        [ForeignKey("RouteId")]
        [Required]
        public Route Route { get; set; }
    }
}
