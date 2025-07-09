using System.Net.Mail;
using System.Net;
using Investly.PL.General.Services.IServices;

namespace Investly.PL.General.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public SmtpEmailSender(IConfiguration configuration)
        {
            _config = configuration;

        }
        public async Task SendEmailAsync(string toEmail, string Subject, string htmlBody)
        {
            var smtpHost = _config["EmailSettings:SmtpHost"];
            var smtpPort = int.Parse(_config["EmailSettings:SmtpPort"]);
            var enableSsl = bool.Parse(_config["EmailSettings:EnableSsl"]);
            var fromEmail = _config["EmailSettings:FromEmail"];
            var password = _config["EmailSettings:Password"];

            var msg = new MailMessage();
            msg.From = new MailAddress(fromEmail, "Air_Journey Blogs");

            msg.To.Add(toEmail);
            msg.Subject = Subject;
            msg.Body = htmlBody;
            msg.IsBodyHtml = true;

            using (var client = new SmtpClient(smtpHost, smtpPort))
            {
                client.EnableSsl = enableSsl;
                client.Credentials = new NetworkCredential(fromEmail, password);
                await client.SendMailAsync(msg);
            }

        }
    }
}
