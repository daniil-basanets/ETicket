using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ETicket.ApplicationServices.DTOs
{
    public abstract class TickeBaseDto
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
    }

    public class TicketDto : TickeBaseDto
    {
        [DisplayName("Ticket type")]
        public int TicketTypeId { get; set; }

        [DisplayName("User")]
        public Guid? UserId { get; set; }

        [DisplayName("User")]
        public string UserName { get; set; }

        [Required]
        [DisplayName("Transaction")]
        public Guid TransactionHistoryId { get; set; }

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

    public class TicketApiDto : TickeBaseDto
    {
        public IEnumerable<string> Areas { get; set; }
    }
}