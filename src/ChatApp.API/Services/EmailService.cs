using System;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace ChatApp.API.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Sends an email using MimeKit and MailKit.
        /// </summary>
        /// <param name="toEmail">Recipient email address.</param>
        /// <param name="subject">Email subject.</param>
        /// <param name="htmlBody">HTML body of the email.</param>
        /// <returns>True if the email was sent successfully, false otherwise.</returns>
        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            var smtpServer = emailSettings.GetValue<string>("SmtpServer");
            var smtpPort = emailSettings.GetValue<int>("SmtpPort");
            var senderName = emailSettings.GetValue<string>("SenderName");
            var senderEmail = emailSettings.GetValue<string>("SenderEmail");
            var senderPassword = emailSettings.GetValue<string>("Password");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(senderName, senderEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;
            message.Body = new TextPart("html") 
            { 
                Text = htmlBody 
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(senderEmail, senderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
