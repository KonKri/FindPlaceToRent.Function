namespace FindPlaceToRent.Core.Models.Configuration
{
    public class ModuleConfiguration
    {
        public RealEstateWebSiteAdsListSettings RealEstateWebSiteAdsListSettings { get; set; }
        public string ScrapeStackApiKey { get; set; }
        public string ScraperApiApiKey { get; set; }
        public AzureStorageSettings AzureStorageSettings { get; set; }
        public SmtpSettings SmtpSettings { get; set; }
    }
}