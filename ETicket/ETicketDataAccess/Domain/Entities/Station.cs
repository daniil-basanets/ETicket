using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ETicket.DataAccess.Domain.Entities
{
    public class Station
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(25)]
        [DisplayName("Name")]
        public string Name { get; set; }

        [ForeignKey("AreaId")]
        public Area Area { get; set; }

        [Column(TypeName = "xml")]
        public Location Location { get; set; }
    }
}
