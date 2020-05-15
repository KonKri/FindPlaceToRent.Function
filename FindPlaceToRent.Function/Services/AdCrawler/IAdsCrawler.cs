using FindPlaceToRent.Function.Models;
using FindPlaceToRent.Function.Models.Ad;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FindPlaceToRent.Function.Services.Crawlers
{
    /// <summary>
    /// Gets content from requested page's html.
    /// </summary>
    public interface IAdsCrawler
    {
        /// <summary>
        /// Get all ads' urls from ads list page.
        /// </summary>
        /// <returns></returns>
        List<CrawledAdSummary> GetAdsSummaries(HtmlDocument htmlDocument);
    }
}