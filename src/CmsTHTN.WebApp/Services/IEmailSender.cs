using CmsTHTN.WebApp.Models;

namespace CmsTHTN.WebApp.Services
{
    public interface IEmailSender
    {
        Task SendEmail(EmailData emailData);
    }
}
