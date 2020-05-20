using Newtonsoft.Json;

namespace ETicketMobile.WebAccess.DTO
{
    public class ConfirmEmailRequestDto
    {
        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Code")]
        public string ActivationCode { get; set; }
    }
}