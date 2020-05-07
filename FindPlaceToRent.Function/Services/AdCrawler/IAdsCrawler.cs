using FindPlaceToRent.Function.Models;
using System.Threading.Tasks;

namespace FindPlaceToRent.Function.Services.Crawlers
{
    /// <summary>
    /// Gets html content of requested page.
    /// </summary>
    public interface IAdsCrawler
    {
        /// <summary>
        /// Get all ads' info summarized from first page.
        /// </summary>
        /// <returns></returns>
        Task GetAdsSummarizedAsync(string adsUrl);

        /// <summary>
        /// Get details for specific ad we are intrested in.
        /// </summary>
        /// <param name="adUrl"></param>
        /// <returns></returns>
        Task<Ad> GetAdDetailsAsync(string adUrl);
    }
}