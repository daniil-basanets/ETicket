using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace ETicket.ApplicationServices.DTOs
{
    public class TicketDto
    {
        public Guid Id { get; set; }

        [DisplayName("Ticket type")]
        public int TicketTypeId { get; set; }

        public string TicketTypeName { get; set; }
        
        [DisplayName("Created")]
        public DateTime CreatedUTCDate { get; set; }

        [DisplayName("Activated")]
        public DateTime? ActivatedUTCDate { get; set; }

        [DisplayName("Expiration")]
        public DateTime? ExpirationUTCDate { get; set; }

        [DisplayName("User")]
        public Guid? UserId { get; set; }

        public string UserName { get; set; }
                
        [DisplayName("Transaction")]
        public Guid TransactionHistoryId { get; set; }

        public string TransactionRRN { get; set; }

        public IList<KeyValuePair<int, string>> Areas { get; set; }
    }
}