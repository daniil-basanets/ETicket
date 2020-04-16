using System;

namespace ETicket.WebAPI.Services.BuyTicket
{
    public class BuyTicketRequest
    {
        public Guid UserId { get; set; }

        public int TicketTypeId { get; set; }

        public int Amount { get; set; }
    }
}