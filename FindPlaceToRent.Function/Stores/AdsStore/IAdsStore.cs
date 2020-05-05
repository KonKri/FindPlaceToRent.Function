using System.Threading.Tasks;

namespace FindPlaceToRent.Function.Stores
{
    /// <summary>
    /// Where the ads are stored.
    /// </summary>
    public interface IAdsStore
    {
        /// <summary>
        /// Save new ad in storage.
        /// </summary>
        /// <returns></returns>
        Task SaveAdAsync();

        /// <summary>
        /// Get ads that are in storage.
        /// </summary>
        /// <param name="count">How many ads to retrieve</param>
        /// <returns></returns>
        Task GetAds(int count);
    }
}