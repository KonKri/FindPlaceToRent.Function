using FindPlaceToRent.Function;
using FindPlaceToRent.Function.Services.Crawlers;
using FindPlaceToRent.Function.Services.Scraper;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

// so that azure functions can find startup file.
[assembly: FunctionsStartup(typeof(Startup))]

namespace FindPlaceToRent.Function
{
    /// <summary>
    /// When azure function is executed, first Startup is executed where IoC is taking place.
    /// </summary>
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IAdsCrawler, AdsCrawler>();
            builder.Services.AddSingleton(o => new ProxyScraperService(o.GetRequiredService<HttpClient>(), "7f3bb9113f45580e893e376ea502b82e"));
        }
    }
}