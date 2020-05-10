using Newtonsoft.Json;

namespace ETicketMobile.ServiceAccess
{
    public class SignUpItemDto
    {
        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}