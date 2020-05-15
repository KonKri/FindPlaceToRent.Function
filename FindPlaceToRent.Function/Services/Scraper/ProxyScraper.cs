using HtmlAgilityPack;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace FindPlaceToRent.Function.Services
{
    public class ProxyScraper : IProxyScraper
    {
        private readonly HttpClient _httpClient;
        private readonly string AccessKey;

        public ProxyScraper(HttpClient httpClient, string accessKey)
        {
            _httpClient = httpClient;
            AccessKey = accessKey;
        }

        public async Task<HtmlDocument> GetHtmlContentAsync(string url)
        {
            var urlUrlEncoded = HttpUtility.UrlEncode(url);

            string pageAsStr; int counter = 0;

            // get page content.
            do
            {
                pageAsStr = await _httpClient.GetStringAsync($"http://api.scraperapi.com?api_key={AccessKey}&url={urlUrlEncoded}");
                //pageAsStr = File.ReadAllText("./wwwroot/index.html");

                // mimic human behavior.
                if (counter >= 1)
                    Thread.Sleep(15000);

                counter++;

                // don't stress page.
                if (counter >= 2)
                    throw new OperationCanceledException("couldn't get page content.");

            } while (pageAsStr.Contains("Pardon Our Interruption"));

            // create an html document object to get the data we need.
            var pageAsHtml = new HtmlDocument();
            pageAsHtml.LoadHtml(pageAsStr);

            return pageAsHtml;
        }
    }
}