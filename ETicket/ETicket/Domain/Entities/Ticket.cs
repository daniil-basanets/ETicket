using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETicket.Domain.Entities
{
    public class Ticket
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("TicketTypeId")]
        public TicketType TicketTypeID { get; set; }

        [Required]
        public DateTime CreatedUTCDate { get; set; }

        public DateTime? ActivatedUTCDate { get; set; }

        [Required]
        public DateTime ExpirationUTCDate { get; set; }

        public long? UserID { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public int TransactionHistoryId { get; set; }

        [ForeignKey("TransactionHistoryId")]
        public TransactionHistory TransactionHistory { get; set; }
    }
}
