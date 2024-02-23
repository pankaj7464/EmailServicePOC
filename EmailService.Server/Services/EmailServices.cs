
using EmailService.Server.Models;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using EmailService.Server.Uttils;

namespace EmailService.Server.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailServices(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }
        /// <summary>
        /// This method, SendEmail, sends an email using SMTP protocol. 
        /// It takes an EmailDto object as a parameter, containing details  recipient, subject, and body. 
        /// It then creates a MimeMessage, sets the sender, recipient, subject, and body of the email, 
        /// And sends it using an SMTP client after authenticating with the provided credentials.
        /// Finally, it disconnects from the SMTP server after sending the email
        /// </summary>
        /// <param name="request"></param>
        public void SendEmail(EmailDto request)
        {
            // Create a new MimeMessage
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_smtpSettings.Sender));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = request.Body,
            };
            using (var client = new SmtpClient())
            {
                client.Connect(_smtpSettings.Server, _smtpSettings.Port, SecureSocketOptions.StartTls);
                client.Authenticate(_smtpSettings.Sender, _smtpSettings.Password);
                client.Send(email);
                client.Disconnect(true);
            }

        }
    }
}
