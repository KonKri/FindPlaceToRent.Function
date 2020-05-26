using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FindPlaceToRent.Core.Services
{
    internal class EmailService : IEmailService
    {
        private readonly SmtpClient _client;
        private readonly string _from;
        private readonly string[] _to;

        public EmailService(SmtpClient client, string from, string[] to)
        {
            _client = client;
            _from = from;
            _to = to;
        }

        public async Task SendEmailAsync(string subject, string body, bool isBodyHtml = true)
        {
            try
            {
                // create a string of all recipients.
                var emailAddressesToSendTo = string.Join(",", _to);

                var msg = new MailMessage();
                msg.To.Add(emailAddressesToSendTo);
                msg.From = new MailAddress(_from);
                msg.Subject = subject;
                msg.Body = body;
                msg.IsBodyHtml = isBodyHtml;

                await _client.SendMailAsync(msg);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}