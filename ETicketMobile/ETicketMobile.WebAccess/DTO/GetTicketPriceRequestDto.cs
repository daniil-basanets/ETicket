using System.Collections.Generic;
using Newtonsoft.Json;

namespace ETicketMobile.WebAccess.DTO
{
    public class GetTicketPriceRequestDto
    {
        [JsonProperty("ticketTypeId")]
        public int TicketTypeId { get; set; }

        [JsonProperty("areasId")]
        public IEnumerable<int> AreasId { get; set; }
    }
}