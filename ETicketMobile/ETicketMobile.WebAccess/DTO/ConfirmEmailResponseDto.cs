using Newtonsoft.Json;

namespace ETicketMobile.WebAccess.DTO
{
    public class ConfirmEmailResponseDto
    {
        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }
    }
}