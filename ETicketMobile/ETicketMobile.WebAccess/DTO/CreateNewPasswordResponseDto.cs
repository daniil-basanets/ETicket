using Newtonsoft.Json;

namespace ETicketMobile.WebAccess.DTO
{
    public class CreateNewPasswordResponseDto
    {
        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }
    }
}