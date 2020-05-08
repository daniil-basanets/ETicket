using Newtonsoft.Json;

namespace ETicketMobile.WebAccess.DTO
{
    public class TokenDto
    {
        [JsonProperty("access_jwtToken")]
        public string AcessJwtToken { get; set; }

        [JsonProperty("refresh_jwtToken")]
        public string RefreshJwtToken { get; set; }

        [JsonProperty("succeeded")]
        public string Succeeded { get; set; }
    }
}