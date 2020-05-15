using Microsoft.WindowsAzure.Storage.Table;

namespace FindPlaceToRent.Function.Models.Ad
{
    /// <summary>
    /// Represents the url crawled from the web page and stored in table storage.
    /// </summary>
    public class CrawledAdSummary : TableEntity
    {
        public string TitleAreaPrice { get; set; }
        public string Characteristics { get; set; }
        public string Location { get; set; }
        public string Url { get; set; }
    }
}