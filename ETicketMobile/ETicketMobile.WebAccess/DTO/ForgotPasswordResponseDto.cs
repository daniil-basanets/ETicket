using Newtonsoft.Json;

namespace ETicketMobile.WebAccess.DTO
{
    public class ForgotPasswordResponseDto
    {
        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }
    }
}