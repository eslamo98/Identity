using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Identity.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var fromMail = _configuration["EmailSettings:FromEmail"];
            var fromPassword = _configuration["EmailSettings:FromPassword"];// App Password
            var smtpHost = _configuration["EmailSettings:SmtpHost"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);

           

            var message = new MailMessage
            {
                From = new MailAddress(fromMail),
                Subject = subject,
                Body = $"<html><body>{htmlMessage}</body></html>",
                IsBodyHtml = true,
            };

            message.To.Add(email);

            using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(fromMail, fromPassword);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;

                await smtpClient.SendMailAsync(message);
            }
        }
    }
}
