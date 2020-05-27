using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ETicket.ApplicationServices.DTOs
{
    public class TicketDto
    {
        public Guid Id { get; set; }

        [DisplayName("Ticket type")]
        public int TicketTypeId { get; set; }

        [DisplayName("Ticket type")]
        public string TicketTypeName { get; set; }

        [DisplayName("Created")]
        public DateTime CreatedUTCDate { get; set; }

        [DisplayName("Activated")]
        public DateTime? ActivatedUTCDate { get; set; }

        [DisplayName("Expiration")]
        public DateTime? ExpirationUTCDate { get; set; }

        [DisplayName("User")]
        public Guid? UserId { get; set; }

        [DisplayName("User")]
        public string UserName { get; set; }

        [Required]
        [DisplayName("Transaction")]
        public Guid TransactionHistoryId { get; set; }

        [DisplayName("Transaction")]
        public string TransactionRRN { get; set; }

        [Required]
        public IList<int> SelectedAreaIds { get; set; }

        [DisplayName("Areas")]
        public IDictionary<int, string> Areas { get; set; }

        public TicketDto()
        {
            SelectedAreaIds = new List<int>();
            Areas = new Dictionary<int, string>();
        }
    }

    public class TicketsByUserEmailDto
    {
        public Guid Id { get; set; }

        [DisplayName("Ticket type")]
        public string TicketTypeName { get; set; }

        [DisplayName("Created")]
        public DateTime CreatedUTCDate { get; set; }

        [DisplayName("Activated")]
        public DateTime? ActivatedUTCDate { get; set; }

        [DisplayName("Expiration")]
        public DateTime? ExpirationUTCDate { get; set; }

        [DisplayName("Transaction")]
        public string TransactionRRN { get; set; }

        [DisplayName("Areas")]
        public IEnumerable<string> Areas { get; set; }
    }
}