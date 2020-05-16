using System;

namespace ETicket.WebAPI.Models.TicketVerification
{
    public class VerifyTicketRequest
    {
        public Guid TicketId { get; set; }
        public long TransportId { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
    }
}
