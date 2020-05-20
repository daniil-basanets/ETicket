using System;

namespace ETicket.WebAPI.Models.TicketVerification
{
    public class VerifyTicketRequest
    {
        public int TransportId { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
    }
}
