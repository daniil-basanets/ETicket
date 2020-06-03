using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicketMobile.WebAccess.DTO
{
    public class GetUserResponseDto
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("age")]
        public string Age { get; set; }

        [JsonProperty("succeeded")]
        public string Email { get; set; }
    }
}
