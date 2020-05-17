using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETicket.DataAccess.Domain.Entities
{
    public class PriceList
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(0.0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [DisplayName("Start date")]
        public DateTime StartDate { get; set; }

        [Required]
        public int AreaId { get; set; }
        
        [ForeignKey("AreaId")]
        public Area Area { get; set; }
    }
}