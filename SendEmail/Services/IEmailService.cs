using SendEmail.Models;

namespace SendEmail.Services
{
    public interface IEmailService
    {

        void SenqEmail(EmailDTO request);
    }
}
