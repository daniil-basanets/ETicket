using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Threading.Tasks;


namespace ETicketAdmin.Services
{
    public class MailService
    {
        public async Task SendEmailAsync(string email, string message)
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
                    await client.ConnectAsync("smtp.gmail.com", 587, false);
                    await client.AuthenticateAsync("dnazarenko817@gmail.com", "1234567890@DN");
                    await client.SendAsync(emailMessage);

                    await client.DisconnectAsync(true);
                }
            }
        }
    }
}