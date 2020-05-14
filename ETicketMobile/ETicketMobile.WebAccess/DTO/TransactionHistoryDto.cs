using System;
using Newtonsoft.Json;

namespace ETicketMobile.WebAccess.DTO
{
    public class TransactionDto
    {
        [JsonProperty("referenceNumber")]
        public string ReferenceNumber { get; set; }

        [JsonProperty("totalPrice")]
        public decimal TotalPrice { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}