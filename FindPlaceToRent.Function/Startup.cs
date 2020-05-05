using FindPlaceToRent.Function;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

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
        }
    }
}