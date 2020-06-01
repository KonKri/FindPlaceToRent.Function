using FindPlaceToRent.Core;
using Microsoft.AspNetCore.Mvc;

namespace FindPlaceToRent.WebApi.Controllers
{
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly ISearcherAndNotifier _notifier;

        public NotificationController(ISearcherAndNotifier notifier)
        {
            _notifier = notifier;
        }

        /// <summary>
        /// Searches for new ads. If new ads are posted, they are stored in azure table storage, and an email container info about new ads is sent to the reciepients.
        /// </summary>
        /// <returns></returns>
        [Route("api/SearchAndNotify")]
        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> SearchAndNotifyAsync()
        {
            await _notifier.NotifyAsync();
            return Ok();
        }
    }
}