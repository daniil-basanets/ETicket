using System.ComponentModel.DataAnnotations;

namespace ETicket.Domain.Entities
{
    public class Role
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
