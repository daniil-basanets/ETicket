using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ETicketMobile.WebAccess.DTO
{
    public class TicketDto
    {
        [JsonProperty("ticketTypeName")]
        public string TicketType { get; set; }

        [JsonProperty("createdUTCDate")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("activatedUTCDate")]
        public DateTime? ActivatedAt { get; set; }

        [JsonProperty("expirationUTCDate")]
        public DateTime? ExpiredAt { get; set; }

        [JsonProperty("transactionRRN")]
        public string ReferenceNumber { get; set; }

        [JsonProperty("Areas")]
        public IEnumerable<string> TicketAreas { get; set; }
    }
}