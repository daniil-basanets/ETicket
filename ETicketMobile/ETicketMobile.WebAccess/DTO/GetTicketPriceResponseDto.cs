using Newtonsoft.Json;

namespace ETicketMobile.WebAccess.DTO
{
    public class GetTicketPriceResponseDto
    {
        [JsonProperty("totalPrice")]
        public decimal TotalPrice { get; set; }
    }
}