using HtmlAgilityPack;
using System.Threading.Tasks;

namespace FindPlaceToRent.Core.Services
{
    public interface IProxyScraper
    {
        /// <summary>
        /// Using a proxy, page under given url is being scrapped without blocking us.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>Returns an HtmlDocument object.</returns>
        Task<HtmlDocument> GetHtmlContentAsync(string url);
    }
}