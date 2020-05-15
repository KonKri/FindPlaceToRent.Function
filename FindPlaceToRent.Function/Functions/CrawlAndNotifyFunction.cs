using System;
using System.Linq;
using System.Threading.Tasks;
using FindPlaceToRent.Function.Models.Ad;
using FindPlaceToRent.Function.Services;
using FindPlaceToRent.Function.Services.Crawlers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using FindPlaceToRent.Function.Services.Notifier;
using Microsoft.Extensions.Options;
using FindPlaceToRent.Function.Models.Configuration;

namespace FindPlaceToRent.Function.Functions
{
    public class CrawlAndNotifyFunction
    {
        private readonly RealEstateWebSiteAdsListSettings _realEstateWebSiteAdsListSettings;
        private readonly IAdsCrawler _crawler;
        private readonly IProxyScraper _scraper;
        private readonly INotifier _notifier;

        public CrawlAndNotifyFunction(IAdsCrawler crawler, IProxyScraper scraper, INotifier notifier, IOptions<RealEstateWebSiteAdsListSettings> realEstateWebSiteAdsListSettingsOption)
        {
            _crawler = crawler;
            _scraper = scraper;
            _notifier = notifier;
            _realEstateWebSiteAdsListSettings = realEstateWebSiteAdsListSettingsOption.Value;
        }

        [FunctionName("CrawlAndNotify")]
        public async Task RunAsync(
            [TimerTrigger("*/5 * * * * *")] TimerInfo myTimer,
            [Table("AdUrls", Connection = "AzureWebJobsStorage")] CloudTable adsTable,
            ILogger log)
        {
            log.LogInformation($"Crawl started at: {DateTime.Now}");

            // websrape Ads List page.
            var adsSummarizedPage = await _scraper.GetHtmlContentAsync(_realEstateWebSiteAdsListSettings.AdsListPageUrl);


            // get all links for ads from adsListPage
            var crawledAds = _crawler.GetAdsSummaries(adsSummarizedPage);


            // get saved ads from storage.
            var condition = TableQuery.GenerateFilterConditionForDate(
                                                            "timestamp",
                                                            QueryComparisons.Equal,
                                                            DateTime.UtcNow.AddMinutes(-5)
                                                            );

            var selectQuery = new TableQuery<CrawledAdSummary>().Where(condition);

            var savedAds = await adsTable.ExecuteQuerySegmentedAsync(selectQuery, null);


            // find unseen ones.
            var newAds = crawledAds.Where(w => !savedAds.Any(a => a.Url == w.Url)).ToList();


            // notify for new ads.
            await _notifier.SendNotificationForNewAdsAsync(newAds);

            // save new ads in storage.
            var insertBatchOperation = new TableBatchOperation();

            newAds.ForEach(a =>
            {
                insertBatchOperation.Add(TableOperation.InsertOrReplace(a));
            });

            await adsTable.ExecuteBatchAsync(insertBatchOperation);

            log.LogInformation($"Crawl ended at: {DateTime.Now}");
        }
    }
}