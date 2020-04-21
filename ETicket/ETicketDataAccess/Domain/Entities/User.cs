using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.DataAccess.Domain.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(25)]
        [DisplayName("First name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(25)]
        [DisplayName("Last name")]
        public string LastName { get; set; }

        [Required]
        [MaxLength(13)]
        [DisplayName("Phone number")]
        public string Phone { get; set; }

        [MaxLength(50)]
        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Date of birth")]
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        [ForeignKey("PrivilegeId")]
        public Privilege Privilege { get; set; }

        [DisplayName("Privilege")]
        public int? PrivilegeId { get; set; }

        [ForeignKey("DocumentId")]
        public Document Document { get; set; }

        [DisplayName("Document")]
        public Guid? DocumentId { get; set; }
    }
}
