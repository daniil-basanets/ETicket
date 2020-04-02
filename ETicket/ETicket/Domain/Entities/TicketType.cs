using System.ComponentModel.DataAnnotations;

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
        [Range(1, int.MaxValue)]
        public int DurationHours { get; set; }
        
        [Required]
        public bool IsPersonal { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        public float Price { get; set; }
        
    }
}
