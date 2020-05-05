using Newtonsoft.Json;

namespace ETicketMobile.WebAccess.DTO
{
    public class SignUpResponseDto
    {
        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }
    }
}