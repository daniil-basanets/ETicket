using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.ApplicationServices.DTOs
{
    public class TicketVerificationDto
    {
        public Guid Id { get; set; }

        public Guid TicketId { get; set; }

        public DateTime VerificationUTCDate { get; set; }

        public int StationId { get; set; }

        public long TransportId { get; set; }

        public bool IsVerified { get; set; }
    }
}
