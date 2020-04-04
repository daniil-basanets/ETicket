using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DBContextLibrary.Domain.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

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
        public Guid DocumentId { get; set; }
    }
}
