using FindPlaceToRent.Function.Models.Ad;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FindPlaceToRent.Function.Services.Notifier
{
    public interface INotifier
    {
        /// <summary>
        /// Sends notification email if there are new ads in the webpage.
        /// </summary>
        /// <returns></returns>
        Task SendNotificationForNewAdsAsync(List<CrawledAdSummary> ads);
    }
}