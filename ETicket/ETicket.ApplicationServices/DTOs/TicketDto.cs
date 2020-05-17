using System;
using System.ComponentModel;

namespace ETicket.ApplicationServices.DTOs
{
    public class TicketDto
    {
        public Guid Id { get; set; }

        [DisplayName("Ticket type")]
        public int TicketTypeId { get; set; }
        
        [DisplayName("Created")]
        public DateTime CreatedUTCDate { get; set; }

        [DisplayName("Activated")]
        public DateTime? ActivatedUTCDate { get; set; }

        [DisplayName("Expiration")]
        public DateTime? ExpirationUTCDate { get; set; }

        [DisplayName("User")]
        public Guid? UserId { get; set; }

        [DisplayName("Transaction")]
        public Guid TransactionHistoryId { get; set; }
    }
}