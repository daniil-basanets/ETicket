using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ETicket.DataAccess.Domain.Entities
{
    public class SecretCode
    {
        [Key]
        public int Id { get; set; }

        public string Code { get; set; }
        public string Email { get; set; }
    }
}
