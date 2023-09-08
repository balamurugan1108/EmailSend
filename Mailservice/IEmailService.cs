using EmailSendWeb.Model;

namespace EmailSendWeb.Mailservice
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailInfo emailInfo);
    }
}
