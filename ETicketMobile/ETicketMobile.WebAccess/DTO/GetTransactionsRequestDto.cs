using Newtonsoft.Json;

namespace ETicketMobile.WebAccess.DTO
{
    public class GetTransactionsRequestDto
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}