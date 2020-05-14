using System.Collections.Generic;

namespace ETicket.WebAPI.Services.BuyTicket
{
    public class GetTicketPriceRequest
    {
        public int TicketTypeId { get; set; }

        public IEnumerable<int> AreasId { get; set; }
    }
}