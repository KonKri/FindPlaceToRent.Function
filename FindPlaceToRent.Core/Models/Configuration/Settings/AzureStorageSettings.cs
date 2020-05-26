namespace FindPlaceToRent.Core.Models.Configuration
{
    public class AzureStorageSettings
    {
        public string AzureTableStorageConnectionString { get; set; }
        public string TableName { get; set; }
    }
}