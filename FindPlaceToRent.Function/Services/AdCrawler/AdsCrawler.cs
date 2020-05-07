using FindPlaceToRent.Function.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FindPlaceToRent.Function.Services.Crawlers
{
    public class AdsCrawler : IAdsCrawler
    {
        private readonly HttpClient _client;
        private readonly IProxyScraperService _scraper;

        public AdsCrawler(HttpClient client)
        {
            _client = client;

            // imitate browser behavior so website responses with 200.
            _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
            _client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
            _client.DefaultRequestHeaders.Add("Accept-Encoding", "deflate");
            _client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            _client.DefaultRequestHeaders.Add("Referer", "https://www.facebook.com/");
            _client.BaseAddress = new Uri("http://api.scrapestack.com/scrape?access_key=7f3bb9113f45580e893e376ea502b82e&url=");
        }

        public async Task<Ad> GetAdDetailsAsync(string adUrl)
        {
            var adPage = await GetHtmlContentAsync(adUrl);

            return new Ad();
        }

        public async Task GetAdsSummarizedAsync(string adsUrl)
        {
            var adsPage = await GetHtmlContentAsync(adsUrl);

            // using xpath syntax, to get all div.article in page.
            var articles = adsPage.DocumentNode.SelectNodes("//div[@class='resultItems']/article").ToList();

            var adsList = new List<Ad>();
            foreach (var article in articles)
            {
                var adRelativeUrl = article.GetAttributeValue("href", string.Empty);
                var ad = await GetAdDetailsAsync(adRelativeUrl);
                adsList.Add(ad);
                
                // imitate human behavior.
                Thread.Sleep(500);
            }
        }

        private async Task<HtmlDocument> GetHtmlContentAsync(string url)
        {
            // get page content.
            var pageAsStr = await _client.GetStringAsync(url);

            // create an html document object to get the data we need.
            var pageAsHtml = new HtmlDocument();
            pageAsHtml.LoadHtml(pageAsStr);

            return pageAsHtml;
        }
    }
}