using Newtonsoft.Json;

namespace ETicketMobile.WebAccess.DTO
{
    public class ResetPasswordRequestDto
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("code")]
        public string ActivationCode { get; set; }
    }
}