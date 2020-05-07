using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FindPlaceToRent.Function.Services
{
    public class ProxyScraperService : IProxyScraperService
    {
        private readonly HttpClient _httpClient;

        public ProxyScraperService(HttpClient httpClient, string accessKey)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri($"http://api.scrapestack.com/scrape?access_key={accessKey}&url=");
        }

        public async Task<HtmlDocument> GetHtmlContent(string url)
        {
            // get page content.
            var pageAsStr = await _httpClient.GetStringAsync(url);

            // create an html document object to get the data we need.
            var pageAsHtml = new HtmlDocument();
            pageAsHtml.LoadHtml(pageAsStr);

            return pageAsHtml;
        }
    }
}