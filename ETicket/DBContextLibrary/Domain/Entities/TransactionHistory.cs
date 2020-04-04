using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBContextLibrary.Domain.Entities
{
    public class TransactionHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(0.0, double.MaxValue)]
        public float TotalPrice { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int TicketTypeId { get; set; }

        [ForeignKey("TicketTypeId")]
        public TicketType TicketType { get; set; }

        [Required]
        public int Count { get; set; }
    }
}