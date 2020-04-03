using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public virtual Role RoleID { get; set; }
        public virtual Privilegie PrivilegieID { get; set; }
        public virtual Document DocumentID { get; set; }
    }
}
