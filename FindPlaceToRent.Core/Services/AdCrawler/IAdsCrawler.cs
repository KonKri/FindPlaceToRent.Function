using FindPlaceToRent.Core.Models.Ad;
using HtmlAgilityPack;
using System.Collections.Generic;

namespace FindPlaceToRent.Core.Services.Crawlers
{
    /// <summary>
    /// Gets content from requested page's html.
    /// </summary>
    internal interface IAdsCrawler
    {
        /// <summary>
        /// Get all ads' urls from ads list page.
        /// </summary>
        /// <returns></returns>
        List<CrawledAdSummary> GetAdsSummaries(HtmlDocument htmlDocument);
    }
}