using System.Net.Mail;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
namespace AzureServiceBus.Service
{
    public class EmailService
    {

        public static async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("AzureServiceBus", "roberts200018@gmail.com"));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("roberts200018@gmail.com", "dzqo puee gacn rlip");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

    }
}
