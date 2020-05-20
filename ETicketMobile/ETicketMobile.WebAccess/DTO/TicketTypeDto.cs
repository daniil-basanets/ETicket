using Newtonsoft.Json;

namespace ETicketMobile.WebAccess
{
    public class TicketTypeDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("typeName")]
        public string Name { get; set; }

        [JsonProperty("coefficient")]
        public decimal Coefficient { get; set; }

        [JsonProperty("DurationHours")]
        public int DurationHours { get; set; }
    }
}