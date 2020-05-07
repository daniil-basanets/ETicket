using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace ETicket.ApplicationServices.DTOs
{
    public class AreaDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
        
    }
}