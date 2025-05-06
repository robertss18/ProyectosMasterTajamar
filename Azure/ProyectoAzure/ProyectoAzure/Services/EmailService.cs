using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
namespace ProyectoAzure.Services
{
    public static class EmailService
    {
        public static async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ProyectoAzure", "roberts200018@gmail.com"));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("roberts200018@gmail.com", "lizd fgvm yhid pcur");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);                
            }
        }
    }
}
