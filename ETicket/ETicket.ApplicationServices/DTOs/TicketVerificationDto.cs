using System;
using System.ComponentModel;

namespace ETicket.ApplicationServices.DTOs
{
    public class TicketVerificationDto
    {
        public Guid Id { get; set; }

        [DisplayName("Ticket")]
        public Guid TicketId { get; set; }

        [DisplayName("Verification date")]
        public DateTime VerificationUTCDate { get; set; }

        [DisplayName("Station")]
        public int StationId { get; set; }

        [DisplayName("Station")]
        public string StationName { get; set; }

        [DisplayName("Transport")]
        public int TransportId { get; set; }

        [DisplayName("Transport")]
        public string TransportNumber { get; set; }

        [DisplayName("Verified")]
        public bool IsVerified { get; set; }
    }
}
