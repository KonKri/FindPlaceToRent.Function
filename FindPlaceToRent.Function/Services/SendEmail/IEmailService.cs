using System.Threading.Tasks;

namespace FindPlaceToRent.Function.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string subject, string body, bool isBodyHtml = true);
    }
}