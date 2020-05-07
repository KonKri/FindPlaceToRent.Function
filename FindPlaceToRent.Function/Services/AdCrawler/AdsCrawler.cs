using FindPlaceToRent.Function.Models;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FindPlaceToRent.Function.Services.Crawlers
{
    public class AdsCrawler : IAdsCrawler
    {
        private readonly IProxyScraperService _scraper;

        public AdsCrawler(IProxyScraperService scraper)
        {
            _scraper = scraper;
        }

        public async Task<Ad> GetAdDetailsAsync(string adUrl)
        {
            var adPage = await _scraper.GetHtmlContentAsync(adUrl);

            return new Ad();
        }

        public async Task GetAdsSummarizedAsync(string adsUrl)
        {
            var adsPage = await _scraper.GetHtmlContentAsync(adsUrl);

            // using xpath syntax, to get all div.article in page.
            var articles = adsPage.DocumentNode.SelectNodes("//div[@class='resultItems']/article").ToList();

            var adsList = new List<Ad>();
            foreach (var article in articles)
            {
                var adRelativeUrl = article.GetAttributeValue("href", string.Empty);
                var ad = await GetAdDetailsAsync(adRelativeUrl);
                adsList.Add(ad);
            }
        }
    }
}