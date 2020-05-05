using System;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FindPlaceToRent.Function.Functions
{
    public class CrawlAndNotifyFunction
    {
        private readonly HttpClient _client;

        public CrawlAndNotifyFunction(HttpClient client)
        {
            _client = client;
        }

        [FunctionName("CrawlAndNotify")]
        public void Run([TimerTrigger("*/5 * * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Crawl started ad at: {DateTime.Now}");

            // crawl xe for all ads in page.

            // get saved ads from storage.

            // find unseen ones.

            // notify for new ads.

            // save new ads in storage.
        }
    }
}