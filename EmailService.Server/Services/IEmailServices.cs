using EmailService.Server.Models;

namespace EmailService.Server.Services
{
    public interface IEmailServices
    {
        void SendEmail(EmailDto request);
    }
}
