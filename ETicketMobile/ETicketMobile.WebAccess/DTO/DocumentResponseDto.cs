﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicketMobile.WebAccess.DTO
{
    public class DocumentResponseDto
    {
        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }
    }
}
