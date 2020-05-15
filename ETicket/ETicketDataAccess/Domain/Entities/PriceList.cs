using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETicket.DataAccess.Domain.Entities
{
    public class PriceList
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public int AreaId { get; set; }
        [ForeignKey("AreaId")]
 
        public Area Area { get; set; }
    }
}