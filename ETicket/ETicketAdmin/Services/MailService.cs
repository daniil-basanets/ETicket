using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Threading.Tasks;


namespace ETicket.Admin.Services
{
    public class MailService
    {
        public void SendEmail(string email, string message)
        {
            if (email != null)
            {
                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress("Administrations", "dnazarenko817@gmail.com"));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = "Reminders";
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = message
                };

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("dnazarenko817@gmail.com", "1234567890@DN");
                    client.Send(emailMessage);

                    client.Disconnect(true);
                }
            }
        }
    }
}