using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ETicket.DataAccess.Domain.Entities
{
    public class Route
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        [DisplayName("Route number")]
        public string Number { get; set; }

        [Required]
        [DisplayName("First station")]
        public int FirstStationId { get; set; }

        [ForeignKey("FirstStationId")]
        public Station FirstStation { get; set; }

        [Required]
        [DisplayName("Last station")]
        public int LastStationId { get; set; }

        [ForeignKey("LastStationId")]
        public Station LastStation { get; set; }

        public ICollection<RouteStation> RouteStations { get; set; }
    }
}
