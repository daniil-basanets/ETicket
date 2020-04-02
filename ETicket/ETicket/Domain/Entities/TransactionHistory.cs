using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETicket.Domain.Entities
{
    public class TransactionHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
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