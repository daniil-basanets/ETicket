using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ETicket.DataAccess.Domain.Entities
{
    public class Carrier
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(13)]
        [DisplayName("Phone number")]
        public string Phone { get; set; }

        [Required]
        [MaxLength(50)]
        public string Address { get; set; }

        [MaxLength(30)]
        [DisplayName("International Bank Account Number (IBAN)")]
        public string IBAN { get; set; }
    }
}
