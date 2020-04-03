using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.Domain.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(25)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(25)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(13)]
        public string Phone { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }
        public int RoleId { get; set; }

        [ForeignKey("PrivelegeId")]
        public Privilege Privilege { get; set; }
        public int PrivilegeId { get; set; }

        [ForeignKey("DocumentId")]
        public Document Document { get; set; }
        public int DocumentId { get; set; }
    }
}
