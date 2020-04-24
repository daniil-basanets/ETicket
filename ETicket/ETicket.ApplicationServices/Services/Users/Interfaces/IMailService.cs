using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.ApplicationServices.Services.Users.Interfaces
{
    public interface IMailService
    {
        public void SendEmail(string email, string message);
    }
}
