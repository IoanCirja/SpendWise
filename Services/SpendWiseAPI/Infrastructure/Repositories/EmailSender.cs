using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EmailSender:IEmailSender
    {
        private readonly string smtpServer = "smtp.gmail.com"; // Gmail SMTP server
        private readonly int smtpPort = 587; // Port for TLS
        private readonly string smtpUser = "spendwisenews@gmail.com"; // SpendWise email
        private readonly string smtpPassword = "lunz iezv vitc brvj"; // Gmail app password

        public async Task SendEmailAsync(List<string> toEmails, string subject, string body)
        {
            using (var smtpClient = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPassword),
                EnableSsl = true
            })
            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(smtpUser);
                foreach (var email in toEmails)
                {
                    mailMessage.To.Add(email);
                }
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;

                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}
