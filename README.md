# Find Place To Rent

FindPlaceToRent is an Azure function running serverless every 30 minutes, web crawling through proxy Apis a real estate website. After retrieving apartments' location, rent and more, function checks whether some ads have been crawled before, and then notifies via email about the new ads.

Since Azure functions require an Azure Storage account, it would be logical that scanned ads can be stored in an Azure Table Storage Table. That reduces the cost of the overall solution.

In order to get the HTML content, Scraper Stack or Scraper Api could get the job done, and you can get the key quite easily.

In case you want to try it userself and customize to serve your needs, you might need `secrets.settings.json` Here is the template of  that file:

    {
      "RealEstateWebSiteAdsListSettings": {
        "AdsListPageUrl": "a-real-estate-site.com/somepage?filter=nofilter",
        "AdPageBaseUrl": "a-real-estate-site.com"
      },
    
      "ScrapeStackApiKey": "a-key",
      "ScraperApiApiKey": "another-key",
    
      "SmtpSettings": {
        "Server": "a-server",
        "Port": -2,
        "EnableSsl": maybe,
        "From": "someone",
        "Username": "a-user",
        "Password": "***",
        "Recipients": [ "who" ]
      }
    }
