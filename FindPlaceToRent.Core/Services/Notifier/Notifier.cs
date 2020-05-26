using FindPlaceToRent.Core.Models.Ad;
using FindPlaceToRent.Core.Models.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FindPlaceToRent.Core.Services.Notifier
{
    internal class Notifier : INotifier
    {
        private readonly IEmailService _emailService;
        private readonly RealEstateWebSiteAdsListSettings _realEstateWebSiteAdsListSettings;

        public Notifier(IEmailService emailService, IOptions<RealEstateWebSiteAdsListSettings> realEstateWebSiteAdsListOptions)
        {
            _emailService = emailService;
            _realEstateWebSiteAdsListSettings = realEstateWebSiteAdsListOptions.Value;
        }

        public async Task SendNotificationForNewAdsAsync(List<CrawledAdSummary> ads)
        {
            // get template and use it for every ad.
            var htmlBodySectionTemplate = File.ReadAllText($"./wwwroot/newAdNotification.html");

            string htmlBody = string.Empty;

            ads.ForEach((e) =>
            {
                var ad = htmlBodySectionTemplate;
                
                ad = ad.Replace("{{url}}", $"{_realEstateWebSiteAdsListSettings.AdPageBaseUrl}/{e.Url}")
                       .Replace("{{titleAreaPrice}}", e.TitleAreaPrice)
                       .Replace("{{location}}", e.Location)
                       .Replace("{{characteristics}}", e.Characteristics);

                htmlBody += ad;
            });

            await _emailService.SendEmailAsync(subject: "Νέα Αγγελία", body: htmlBody);
        }
    }
}