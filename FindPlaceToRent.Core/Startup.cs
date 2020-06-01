using FindPlaceToRent.Core.Models.Configuration;
using FindPlaceToRent.Core.Services;
using FindPlaceToRent.Core.Services.Crawlers;
using FindPlaceToRent.Core.Services.Notifier;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Net;
using System.Net.Mail;

namespace FindPlaceToRent.Core
{
    public static class Startup
    {
        public static IServiceCollection AddFindPlaceToRentModule(this IServiceCollection services, Action<ModuleConfiguration> moduleOptionsBuilder)
        {
            // populate options with delegate action.
            var moduleOptions = new ModuleConfiguration();
            moduleOptionsBuilder(moduleOptions);

            services.AddSingleton(moduleOptions.RealEstateWebSiteAdsListSettings);
            services.AddSingleton(moduleOptions.HtmlTemplateSettings);
            services.AddSingleton(new ProxyScraperSettings { AccessKey = moduleOptions.ScraperApiApiKey});

            services.AddHttpClient<IProxyScraper, ProxyScraper>();
            services.AddScoped<IAdsCrawler, AdsCrawler>();
            services.AddScoped<INotifier, Notifier>();
            services.AddScoped<CloudTable>(o =>
            {
                var storageAccount = CloudStorageAccount.Parse(moduleOptions.AzureStorageSettings.AzureTableStorageConnectionString);
                var tableClient = storageAccount.CreateCloudTableClient();

                return tableClient.GetTableReference(moduleOptions.AzureStorageSettings.TableName);
            });

            services.AddScoped<IEmailService, EmailService>(s =>
            {
                var client = new SmtpClient();
                client.Host = moduleOptions.SmtpSettings.Server;
                client.Port = moduleOptions.SmtpSettings.Port;
                client.EnableSsl = moduleOptions.SmtpSettings.EnableSsl;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(moduleOptions.SmtpSettings.Username, moduleOptions.SmtpSettings.Password);

                return new EmailService(client, moduleOptions.SmtpSettings.From, moduleOptions.SmtpSettings.Recipients);
            });

            services.AddScoped<ISearcherAndNotifier, SearcherAndNotifier>();

            return services;
        }
    }
}