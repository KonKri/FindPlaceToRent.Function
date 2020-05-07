using System;
using System.Threading.Tasks;
using FindPlaceToRent.Function.Services.Crawlers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FindPlaceToRent.Function.Functions
{
    public class CrawlAndNotifyFunction
    {
        private readonly IAdsCrawler _crawler;

        public CrawlAndNotifyFunction(IAdsCrawler crawler)
        {
            _crawler = crawler;
        }

        [FunctionName("CrawlAndNotify")]
        public async Task RunAsync([TimerTrigger("*/5 * * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Crawl started ad at: {DateTime.Now}");

            // crawl xe for all ads in page.
            await _crawler.GetAdsSummarizedAsync("https://www.xe.gr/property/search?Geo.area_id_new__hierarchy=82196&Item.area.from=20&Publication.age=1&Publication.level_num.from=1&System.item_type=re_residence&Transaction.price.to=350&Transaction.type_channel=117541&sort_by=System.creation_date&sort_direction=desc");

            // get saved ads from storage.

            // find unseen ones.

            // notify for new ads.

            // save new ads in storage.
        }
    }
}