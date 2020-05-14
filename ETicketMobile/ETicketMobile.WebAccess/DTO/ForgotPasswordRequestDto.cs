using Newtonsoft.Json;

namespace ETicketMobile.WebAccess.DTO
{
    public class ForgotPasswordRequestDto
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}