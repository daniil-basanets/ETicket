using Newtonsoft.Json;

namespace ETicketMobile.WebAccess.DTO
{
    public class UserSignInRequestDto
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("succeeded")]
        public string ErrorMessage { get; set; }
    }
}