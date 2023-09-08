using System.Net.Mail;
using System.Net;
using MailKit.Security;
using MimeKit;
using EmailSendWeb.Model;
using EmailSendWeb.Settings;
using Microsoft.Extensions.Options;

namespace EmailSendWeb.Mailservice
{
    public class EmailService:IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> mailSettings)
        {
            _emailSettings = mailSettings.Value;
        }
        public async Task SendEmailAsync(EmailInfo emailInfo)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_emailSettings.EMail);
            email.To.Add(MailboxAddress.Parse(emailInfo.EmailTo));
            email.Subject = emailInfo.Subject;
            var builder = new BodyBuilder();
            if (emailInfo.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in emailInfo.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = emailInfo.Body;
            email.Body = builder.ToMessageBody();
            var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSettings.EMail, _emailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

    }
}
