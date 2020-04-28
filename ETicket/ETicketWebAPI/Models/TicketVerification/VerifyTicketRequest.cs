using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
