using System;
using Newtonsoft.Json;

namespace ETicketMobile.WebAccess.DTO
{
    public class BuyTicketResponseDto
    {
        [JsonProperty("result")]
        public string PayResult { get; set; }

        [JsonProperty("totalPrice")]
        public decimal TotalPrice { get; set; }

        [JsonProperty("boughtAt")]
        public DateTime BoughtAt { get; set; }
    }
}