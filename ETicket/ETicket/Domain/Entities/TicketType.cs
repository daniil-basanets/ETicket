using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.Domain.Entities
{
    public class TicketType
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [MaxLength(50)]
        public string TypeName { get; set; }
        [Required]
        [Range(1, Int32.MaxValue)]
        public int DurationHours { get; set; }
        [Required]
        public bool IsPersonal { get; set; }
        [Required]
        [Range(0, Double.MaxValue)]
        public float Price { get; set; }

    }
}
