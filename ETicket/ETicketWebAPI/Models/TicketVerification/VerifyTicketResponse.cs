using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.WebAPI.Models.TicketVerification
{
    public class VerifyTicketResponse
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
    }
}
