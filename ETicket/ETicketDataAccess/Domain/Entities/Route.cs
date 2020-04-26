using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ETicket.DataAccess.Domain.Entities
{
    public class Route
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Number { get; set; }

        [ForeignKey("StationId")]
        public Station FirstStation { get; set; }

        [Required]
        public int FirstStationId { get; set; }
                
        public Station LastStation { get; set; }

        //[Required]
        public int LastStationId { get; set; }
    }
}
