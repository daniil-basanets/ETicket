using Newtonsoft.Json;

namespace ETicketMobile.WebAccess.DTO
{
    public class CreateNewPasswordRequestDto
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("NewPassword")]
        public string NewPassword { get; set; }

        [JsonProperty("ResetPasswordCode")]
        public string ResetPasswordCode { get; set; }
    }
}