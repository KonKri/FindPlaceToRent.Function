using FindPlaceToRent.Core.Models.Ad;
using FindPlaceToRent.Core.Models.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FindPlaceToRent.Core.Services.Notifier
{
    public class Notifier : INotifier
    {
        private readonly IEmailService _emailService;
        private readonly RealEstateWebSiteAdsListSettings _realEstateWebSiteAdsListSettings;
        private readonly HtmlTemplateSettings _htmlSettings;

        public Notifier(IEmailService emailService, RealEstateWebSiteAdsListSettings realEstateWebSiteAdsListSettings, HtmlTemplateSettings htmlSettings)
        {
            _emailService = emailService;
            _realEstateWebSiteAdsListSettings = realEstateWebSiteAdsListSettings;
            _htmlSettings = htmlSettings;
        }

        public async Task SendNotificationForNewAdsAsync(List<CrawledAdSummary> ads)
        {
            // get template and use it for every ad.

            var htmlBodySectionTemplate = File.ReadAllText(_htmlSettings.FileDirectory);

            string htmlBody = string.Empty;

            ads.ForEach((e) =>
            {
                var ad = htmlBodySectionTemplate;

                ad = ad.Replace(_htmlSettings.UrlVarName, GetAbsoluteAdUrl(_realEstateWebSiteAdsListSettings.AdPageBaseUrl, e.Url))
                       .Replace(_htmlSettings.TitleAreaPriceVarName, e.TitleAreaPrice)
                       .Replace(_htmlSettings.LocationVarName, e.Location)
                       .Replace(_htmlSettings.CharacteristicsVarName, e.Characteristics);

                htmlBody += ad;
            });

            await _emailService.SendEmailAsync(subject: "Νέες Αγγελίες!", body: htmlBody);
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