#if !NET35
using System.Threading.Tasks;

namespace Harry.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}

#endif