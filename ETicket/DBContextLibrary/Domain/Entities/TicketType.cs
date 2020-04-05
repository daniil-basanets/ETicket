using System.ComponentModel.DataAnnotations;

namespace DBContextLibrary.Domain.Entities
{
    public class TicketType
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string TypeName { get; set; }
        
        [Required]
        [Range(1, uint.MaxValue)]
        public uint DurationHours { get; set; }
        
        [Required]
        public bool IsPersonal { get; set; }
        
        [Required]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue)]
        public float Price { get; set; }
    }
}
