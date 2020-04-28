using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ETicket.WebAPI.Models.Identity
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // token publisher
        public const string AUDIENCE = "MyAuthClient"; // token consumer
        const string KEY = "mysupersecret_secretkey!123";   // secret key
        public const int LIFETIME = 30; // token`s lifetime - 30 minutes
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
