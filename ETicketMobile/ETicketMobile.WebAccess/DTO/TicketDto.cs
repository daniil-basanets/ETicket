using Newtonsoft.Json;

namespace ETicketMobile.WebAccess
{
    public class TicketDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("typeName")]
        public string Name { get; set; }

        [JsonProperty("coefficient")]
        public decimal Price { get; set; }

        [JsonProperty("DurationHours")]
        public int DurationHours { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }
    }
}