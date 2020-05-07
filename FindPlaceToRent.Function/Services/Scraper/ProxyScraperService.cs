using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FindPlaceToRent.Function.Services
{
    public class ProxyScraperService : IProxyScraperService
    {
        private readonly HttpClient _httpClient;
        private readonly string AccessKey;

        public ProxyScraperService(HttpClient httpClient, string accessKey)
        {
            _httpClient = httpClient;
            AccessKey = accessKey;
        }
        
        public async Task<HtmlDocument> GetHtmlContentAsync(string url)
        {
            // get page content.
            var pageAsStr = await _httpClient.GetStringAsync($"http://api.scrapestack.com/scrape?access_key={AccessKey}&url={url}");

            // create an html document object to get the data we need.
            var pageAsHtml = new HtmlDocument();
            pageAsHtml.LoadHtml(pageAsStr);

            return pageAsHtml;
        }
    }
}