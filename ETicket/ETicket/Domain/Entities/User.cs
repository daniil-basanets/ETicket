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
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
        [ForeignKey("PrivilegieID")]
        public Privilegie Privilegie { get; set; }
        [ForeignKey("DocumentID")]
        public Document Document { get; set; }


        //public int RoleID { get; set; }
        //public virtual Role Role { get; set; }
        //public  int PrivilegieID { get; set; }
        //public virtual Privilegie Privilegie { get; set; }
        //public virtual int DocumentID { get; set; }
        //public virtual Document Document { get; set; }
    }
}
