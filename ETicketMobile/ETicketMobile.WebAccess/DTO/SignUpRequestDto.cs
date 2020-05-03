using Newtonsoft.Json;

namespace ETicketMobile.WebAccess.DTO
{
    // TODO ErrorMessage
    public class SignUpRequestDto
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}