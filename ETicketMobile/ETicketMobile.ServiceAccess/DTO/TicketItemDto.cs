using Newtonsoft.Json;

namespace ETicketMobile.ServiceAccess
{
    public class TicketItemDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("typeName")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("duration_hours")]
        public int DurationHours { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }
    }
}