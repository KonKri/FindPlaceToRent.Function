using FindPlaceToRent.Function;
using FindPlaceToRent.Function.Services.Crawlers;
using FindPlaceToRent.Function.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using FindPlaceToRent.Function.Models.Configuration;
using FindPlaceToRent.Function.Services.Notifier;
using System.IO;

// so that azure functions can find startup file.
[assembly: FunctionsStartup(typeof(Startup))]

namespace FindPlaceToRent.Function
{
    /// <summary>
    /// When azure function is executed, first Startup is executed where IoC is taking place.
    /// </summary>
    public class Startup : FunctionsStartup
    {
        private readonly IConfigurationRoot _config;

        public Startup()
        {
            _config = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("./secrets.settings.json")
                                    .Build();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            // add configuration.
            builder.Services.Configure<RealEstateWebSiteAdsListSettings>(_config.GetSection("RealEstateWebSiteAdsListSettings"));
            builder.Services.Configure<SmtpSettings>(_config.GetSection("SmtpSettings"));

            // add services needed.
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IAdsCrawler, AdsCrawler>();
            builder.Services.AddSingleton<IProxyScraper>(o => new ProxyScraper(o.GetService<HttpClient>(), _config.GetValue<string>("ScraperApiApiKey")));
            builder.Services.AddScoped<INotifier, Notifier>();
            builder.Services.AddScoped<IEmailService, EmailService>(s =>
            {
                var client = new SmtpClient();
                var settings = new SmtpSettings();
                _config.Bind("SmtpSettings", settings);

                client.Host = settings.Server;
                client.Port = settings.Port;
                client.EnableSsl = settings.EnableSsl;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(settings.Username, settings.Password);

                return new EmailService(client, settings.From, settings.Recipients);
            });
        }
    }
}