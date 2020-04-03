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
        public int ID { get; set; }
        [Required]
        [MaxLength(25)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(25)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(13)]
        public string Phone { get; set; }
        [ForeignKey("RoleID")]
        public Role Role { get; set; }
        [ForeignKey("PrivelegeID")]
        public Privelege Privelege { get; set; }
        [ForeignKey("DocumentID")]
        public Document Document { get; set; }
    }
}
