using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicketMobile.WebAccess.DTO
{
    public class PrivilegeRequestDto
    {
        [JsonProperty("privilegeId")]
        public int PrivilegeId { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }
    }
}
