using FindPlaceToRent.Core.Models.Ad;
using FindPlaceToRent.Core.Models.Configuration;
using FindPlaceToRent.Core.Services;
using FindPlaceToRent.Core.Services.Crawlers;
using FindPlaceToRent.Core.Services.Notifier;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FindPlaceToRent.Core
{
    public class SearcherAndNotifier
    {
        private readonly RealEstateWebSiteAdsListSettings _realEstateWebSiteAdsListSettings;
        private readonly IAdsCrawler _crawler;
        private readonly IProxyScraper _scraper;
        private readonly INotifier _notifier;
        private readonly CloudTable _adsTable;


        internal SearcherAndNotifier(RealEstateWebSiteAdsListSettings realEstateWebSiteAdsListSettings, IAdsCrawler crawler, IProxyScraper scraper, Notifier notifier, CloudTable adsTable)
        {
            _realEstateWebSiteAdsListSettings = realEstateWebSiteAdsListSettings;
            _crawler = crawler;
            _scraper = scraper;
            _notifier = notifier;
            _adsTable = adsTable;
        }

        /// <summary>
        /// Searches for new posted ads and sends email to notify users.
        /// </summary>
        /// <returns></returns>
        public async Task NotifyAsync()
        {
            // websrape Ads List page.
            var adsSummarizedPage = await _scraper.GetHtmlContentAsync(_realEstateWebSiteAdsListSettings.AdsListPageUrl);


            // get all links for ads from adsListPage
            var crawledAds = _crawler.GetAdsSummaries(adsSummarizedPage);


            // get saved ads from storage.
            var condition = TableQuery.GenerateFilterConditionForDate(
                                                            "Timestamp",
                                                            QueryComparisons.GreaterThan,
                                                            DateTimeOffset.UtcNow.AddMinutes(-15)
                                                            );

            var selectQuery = new TableQuery<CrawledAdSummary>()/*.Where(condition)*/;

            var savedAds = await _adsTable.ExecuteQuerySegmentedAsync(selectQuery, null);


            // find unseen ones.
            var newAds = crawledAds.Where(w => !savedAds.Any(a => a.Url == w.Url)).ToList();

            if (newAds.Count != 0)
            {
                // notify for new ads.
                await _notifier.SendNotificationForNewAdsAsync(newAds);

                // save new ads in storage.
                var insertBatchOperation = new TableBatchOperation();

                newAds.ForEach(a =>
                {
                    a.AssignParitionAndRowKey();
                    insertBatchOperation.Add(TableOperation.InsertOrReplace(a));
                });

                await _adsTable.ExecuteBatchAsync(insertBatchOperation);
            }
        }
    }
}