using System;
using System.Collections.Generic;

namespace ETicketMobile.Business.Model.Tickets
{
    public class Ticket
    {
        public string TicketType { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ActivatedAt { get; set; }

        public DateTime? ExpiredAt { get; set; }

        public string ReferenceNumber { get; set; }

        public IEnumerable<string> TicketAreas { get; set; }
    }
}