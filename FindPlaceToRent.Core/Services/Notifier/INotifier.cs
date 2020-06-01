using FindPlaceToRent.Core.Models.Ad;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FindPlaceToRent.Core.Services.Notifier
{
    public interface INotifier
    {
        /// <summary>
        /// Sends notification email if there are new ads in the webpage.
        /// </summary>
        /// <param name="ads">List of ads information to send via email.</param>
        /// <returns></returns>
        Task SendNotificationForNewAdsAsync(List<CrawledAdSummary> ads);
    }
}