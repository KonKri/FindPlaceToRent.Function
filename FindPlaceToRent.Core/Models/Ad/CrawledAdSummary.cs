using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace FindPlaceToRent.Core.Models.Ad
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

        public void AssignParitionAndRowKey()
        {
            this.PartitionKey = "ad";
            this.RowKey = Guid.NewGuid().ToString();
        }
    }
}