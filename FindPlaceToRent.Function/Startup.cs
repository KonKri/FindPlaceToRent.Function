using FindPlaceToRent.Core;
using FindPlaceToRent.Core.Models.Configuration;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.IO;

// so that azure functions can find startup file.
[assembly: FunctionsStartup(typeof(FindPlaceToRent.Function.Startup))]

namespace FindPlaceToRent.Function
{
    /// <summary>
    /// When azure function is executed, first Startup is executed where IoC is taking place.
    /// </summary>
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // building configuration. Implemented that way because of azure issues.
            var executionContextOptions = builder.Services.BuildServiceProvider()
                                                          .GetService<IOptions<ExecutionContextOptions>>().Value;

            var appDirectory = executionContextOptions.AppDirectory;

            var findPlaceToRentConfig = new ConfigurationBuilder()
                                   .SetBasePath(appDirectory)
                                   .AddJsonFile("./secret.settings.json")
                                   .Build();

            // use depedency injection to startup module.
            builder.Services.AddFindPlaceToRentModule(options =>
            {
                // prepare our configuration.
                var realEstateWebSiteAdsListSettings = new RealEstateWebSiteAdsListSettings();
                findPlaceToRentConfig.Bind("RealEstateWebSiteAdsListSettings", realEstateWebSiteAdsListSettings);

                var smtpSettings = new SmtpSettings();
                findPlaceToRentConfig.Bind("SmtpSettings", smtpSettings);

                var azureStorageSettings = new AzureStorageSettings();
                findPlaceToRentConfig.Bind("AzureStorageSettings", azureStorageSettings);

                var htmlTemplateSettings = new HtmlTemplateSettings();
                findPlaceToRentConfig.Bind("HtmlTemplateSettings", htmlTemplateSettings);
                htmlTemplateSettings.FileDirectory = Path.Combine(appDirectory, htmlTemplateSettings.FileDirectory); // prepend sys path. maybe change it later.

                // pass configuration into FindPlaceToRent startup file.
                options.RealEstateWebSiteAdsListSettings = realEstateWebSiteAdsListSettings;
                options.SmtpSettings = smtpSettings;
                options.AzureStorageSettings = azureStorageSettings;
                options.HtmlTemplateSettings = htmlTemplateSettings;

                options.ScraperApiApiKey = findPlaceToRentConfig.GetValue<string>("ScraperApiApiKey");
                options.ScrapeStackApiKey = findPlaceToRentConfig.GetValue<string>("ScrapeStackApiKey");
            });
        }
    }
}