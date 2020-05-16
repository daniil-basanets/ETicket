using System;
using System.Collections.Generic;

namespace ETicket.ApplicationServices.DTOs
{
    public class TicketDto
    {
        public Guid Id { get; set; }

        public int TicketTypeId { get; set; }

        public string TicketTypeName { get; set; }

        public DateTime CreatedUTCDate { get; set; }

        public DateTime? ActivatedUTCDate { get; set; }

        public DateTime? ExpirationUTCDate { get; set; }

        public Guid? UserId { get; set; }

        public string UserName { get; set; }

        public Guid TransactionHistoryId { get; set; }

        public string TransactionRRN { get; set; }

        public IList<KeyValuePair<int, string>> Areas { get; set; }
}
}