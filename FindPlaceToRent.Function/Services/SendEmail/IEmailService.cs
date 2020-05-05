using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FindPlaceToRent.Function.Services
{
    public interface IEmailService
    {
        // todo: add overload.
        Task SendEmailAsync();
    }
}