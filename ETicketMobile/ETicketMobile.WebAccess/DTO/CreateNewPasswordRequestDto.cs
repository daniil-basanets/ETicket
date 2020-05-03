using Newtonsoft.Json;

namespace ETicketMobile.WebAccess.DTO
{
    public class CreateNewPasswordRequestDto
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}