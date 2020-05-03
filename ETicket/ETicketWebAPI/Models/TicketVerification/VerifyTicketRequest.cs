using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.WebAPI.Models.TicketVerification
{
    public class VerifyTicketInfo
    {
        public long TransportId { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
    }
}
