using FindPlaceToRent.Core.Models.Ad;
using FindPlaceToRent.Core.Models.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace FindPlaceToRent.Core.Services.Notifier
{
    public class Notifier : INotifier
    {
        private readonly IEmailService _emailService;
        private readonly RealEstateWebSiteAdsListSettings _realEstateWebSiteAdsListSettings;

        public Notifier(IEmailService emailService, RealEstateWebSiteAdsListSettings realEstateWebSiteAdsListOptions)
        {
            _emailService = emailService;
            _realEstateWebSiteAdsListSettings = realEstateWebSiteAdsListOptions;
        }

        public async Task SendNotificationForNewAdsAsync(List<CrawledAdSummary> ads)
        {
            // get template and use it for every ad.
            
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "wwwroot/newAdNotification.html");
            var htmlBodySectionTemplate = File.ReadAllText(path);

            string htmlBody = string.Empty;

            ads.ForEach((e) =>
            {
                var ad = htmlBodySectionTemplate;
                
                ad = ad.Replace("{{url}}", GetAbsoluteAdUrl(_realEstateWebSiteAdsListSettings.AdPageBaseUrl, e.Url))
                       .Replace("{{titleAreaPrice}}", e.TitleAreaPrice)
                       .Replace("{{location}}", e.Location)
                       .Replace("{{characteristics}}", e.Characteristics);

                htmlBody += ad;
            });

            await _emailService.SendEmailAsync(subject: "Νέα Αγγελία", body: htmlBody);
        }

        /// <summary>
        /// Combines base url and relative ad url to produce an absolute url formed correctly.
        /// </summary>
        /// <param name="baseUri"></param>
        /// <param name="relativeAdUrl"></param>
        /// <returns></returns>
        private string GetAbsoluteAdUrl(string baseUrl, string relativeAdUrl)
        {
            var baseUri = new Uri(baseUrl);
            var absoluteAdUri = new Uri(baseUri, relativeAdUrl);
            return absoluteAdUri.ToString();
        }
    }
}