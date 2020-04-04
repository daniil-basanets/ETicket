using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBContextLibrary.Domain.Entities
{
    public class Ticket
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int TicketTypeId { get; set; }

        [Required]
        [ForeignKey("TicketTypeId")]
        public TicketType TicketType { get; set; }

        [Required]
        public DateTime CreatedUTCDate { get; set; }

        public DateTime? ActivatedUTCDate { get; set; }

        [Required]
        public DateTime ExpirationUTCDate { get; set; }
                
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public int TransactionHistoryId { get; set; }

        [ForeignKey("TransactionHistoryId")]

        public TransactionHistory TransactionHistory { get; set; }
    }
}
