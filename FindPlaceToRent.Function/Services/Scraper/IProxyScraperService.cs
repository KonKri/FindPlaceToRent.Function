using System.Threading.Tasks;
using HtmlAgilityPack;

namespace FindPlaceToRent.Function.Services
{
    public interface IProxyScraperService
    {
        /// <summary>
        /// Using a proxy, page under given url is being scrapped without blocking us.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<HtmlDocument> GetHtmlContent(string url);
    }
}