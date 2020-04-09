using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBContextLibrary.Domain.Entities
{
    public class TransactionHistory
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(0.0, double.MaxValue)]
        public decimal TotalPrice { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int TicketTypeId { get; set; }

        [ForeignKey("TicketTypeId")]
        public TicketType TicketType { get; set; }

        [Required]
        public int Count { get; set; }
    }
}