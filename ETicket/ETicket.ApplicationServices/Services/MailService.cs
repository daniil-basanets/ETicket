using MimeKit;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using ETicket.ApplicationServices.Services.Interfaces;

namespace ETicket.ApplicationServices.Services
{
    public class MailService : IMailService
    {
        public void SendEmail(string email, string message, string subject)
        {
            if (email != null)
            {
                const int port = 587;
                var emailMessage = CreateMessage(email, message, subject);

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", port, false);
                    client.Authenticate("dnazarenko817@gmail.com", "1234567890@DN");
                    client.Send(emailMessage);

                    client.Disconnect(true);
                }
            }
        }

        private MimeMessage CreateMessage(string email, string message, string subject)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Administrations", "dnazarenko817@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            return emailMessage;
        }
    }
}
