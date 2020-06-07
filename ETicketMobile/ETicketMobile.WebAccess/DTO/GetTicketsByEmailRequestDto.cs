using Newtonsoft.Json;

namespace ETicketMobile.WebAccess.DTO
{
    public class GetTicketsByEmailRequestDto
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}