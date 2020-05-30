using System;

namespace ETicket.WebAPI.Controllers.Payments
{
    public class BuyTicketResponse
    {
        public string PayResult { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime BoughtAt { get; set; }
    }
}