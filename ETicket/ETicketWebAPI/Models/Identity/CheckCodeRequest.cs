using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.WebAPI.Models.Identity
{
    public class CheckCodeRequest
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
