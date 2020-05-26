using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ETicket.DataAccess.Domain.Entities
{
    public class TicketType
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        [DisplayName("Type name")]
        public string TypeName { get; set; }
        
        [Required]
        [Range(1, uint.MaxValue)]
        [DisplayName("Duration (in hours)")]
        public uint DurationHours { get; set; }
        
        [Required]
        [DisplayName("Personal")]
        public bool IsPersonal { get; set; }
        
        [Required]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue)]
        public decimal Coefficient { get; set; }
    }
}
