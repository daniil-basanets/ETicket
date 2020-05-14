using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.WebAPI.Models.Identity
{
    public static class SecretString
    {
        public static string GetSecretString()
        {
            Random rand = new Random();
            var secretNumber = rand.Next(1000, 9999);
            string secretString = $"{secretNumber}{(char)rand.Next('a', 'z')}{(char)rand.Next('a', 'z')}";

            return secretString;
        }
    }
}
