using FindPlaceToRent.Core.Models.Ad;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FindPlaceToRent.Core.Services.Crawlers
{
    internal class AdsCrawler : IAdsCrawler
    {
        private readonly IProxyScraper _scraper;

        public AdsCrawler(IProxyScraper scraper)
        {
            _scraper = scraper;
        }

        public List<CrawledAdSummary> GetAdsSummaries(HtmlDocument htmlDocument)
        {
            // using xpath syntax, to get all div.article in page.
            return htmlDocument.DocumentNode.SelectNodes("//div[@class='resultItems']/article")
            .Select(s =>
            {
                return new CrawledAdSummary
                {
                    TitleAreaPrice = s.SelectSingleNode("./div[@class='articleInfo']/h1")?.InnerText
                                      .Replace(Environment.NewLine, String.Empty)
                                      .Replace("&euro;", "€")
                                      .Replace("  ", String.Empty)
                                      .Trim(),
                    
                    Characteristics = s.SelectSingleNode("./div[@class='articleInfo']/div[@class='characteristics']")?.InnerText
                                      .Replace(Environment.NewLine, String.Empty)
                                      .Replace("&euro;", "€")
                                      .Replace("  ", String.Empty)
                                      .Trim(),

                    Location = s.GetAttributeValue<string>("data-area", string.Empty),
                    Url = s.GetAttributeValue<string>("href", string.Empty)
                };
            })
            .ToList();
        }
    }
}