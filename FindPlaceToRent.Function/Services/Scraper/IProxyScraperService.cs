using HtmlAgilityPack;
using System.Threading.Tasks;

namespace FindPlaceToRent.Function.Services
{
    public interface IProxyScraperService
    {
        /// <summary>
        /// Using a proxy, page under given url is being scrapped without blocking us.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>Returns an HtmlDocument object.</returns>
        Task<HtmlDocument> GetHtmlContentAsync(string url);
    }
}