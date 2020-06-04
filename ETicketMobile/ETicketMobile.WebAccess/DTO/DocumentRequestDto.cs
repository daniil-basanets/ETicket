using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicketMobile.WebAccess.DTO
{
    public class DocumentRequestDto
    {
        [JsonProperty("documentId")]
        public Guid DocumentId { get; set; }

        [JsonProperty("Number")]
        public string Number { get; set; }
    }
}
