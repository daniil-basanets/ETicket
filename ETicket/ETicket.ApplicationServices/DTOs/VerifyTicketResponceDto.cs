using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.ApplicationServices.DTOs
{
    public class VerifyTicketResponceDto
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
    }
}
