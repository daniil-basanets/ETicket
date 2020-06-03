using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicketMobile.WebAccess.DTO
{
    public class GetUserRequestDto
    {
        [JsonProperty("succeeded")]
        public string Email { get; set; }
    }
}
