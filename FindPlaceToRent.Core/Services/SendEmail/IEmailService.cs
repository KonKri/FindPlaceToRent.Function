using System.Threading.Tasks;

namespace FindPlaceToRent.Core.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string subject, string body, bool isBodyHtml = true);
    }
}