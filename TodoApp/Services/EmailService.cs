
using MimeKit;
using System.Net.Mail;

namespace TodoApp.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("sujit", "sujitramdhakal59@gmail.com"));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = body
            };
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com",587,false);
                await client.AuthenticateAsync("sujitramdhakal59@gmail.com",
                    "xftwxdexjhvamuwx");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
