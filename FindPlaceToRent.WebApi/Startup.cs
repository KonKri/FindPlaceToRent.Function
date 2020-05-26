using System;
using System.IO;
using System.Reflection;
using FindPlaceToRent.Core;
using FindPlaceToRent.Core.Models.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace FindPlaceToRent.WebApi
{
    public class Startup
    {
        private readonly IConfiguration _findPlaceToRentConfig;

        public Startup()
        {
            _findPlaceToRentConfig = new ConfigurationBuilder()
                                   .SetBasePath(Environment.CurrentDirectory)
                                   .AddJsonFile("./findplacetorent.settings.json")
                                   .Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // use depedency injection to startup module.
            services.AddFindPlaceToRentModule(options =>
            {
                // prepare our configuration.
                var realEstateWebSiteAdsListSettings = new RealEstateWebSiteAdsListSettings();
                _findPlaceToRentConfig.Bind("RealEstateWebSiteAdsListSettings", realEstateWebSiteAdsListSettings);

                var smtpSettings = new SmtpSettings();
                _findPlaceToRentConfig.Bind("SmtpSettings", smtpSettings);

                var azureStorageSettings = new AzureStorageSettings();
                _findPlaceToRentConfig.Bind("AzureStorageSettings", azureStorageSettings);

                // pass configuration into FindPlaceToRent startup file.
                options.RealEstateWebSiteAdsListSettings = realEstateWebSiteAdsListSettings;
                options.SmtpSettings = smtpSettings;
                options.AzureStorageSettings = azureStorageSettings;

                options.ScraperApiApiKey = _findPlaceToRentConfig.GetValue<string>("ScraperApiApiKey");
                options.ScrapeStackApiKey = _findPlaceToRentConfig.GetValue<string>("ScrapeStackApiKey");
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                var openApiInfo = new OpenApiInfo
                {
                    Title = "Find Place To Rent WebApi",
                    Description = "This web api is uses the findplacetorent core library which " +
                    "crawls the realestate page and searches for new ads. If new ads exist they " +
                    "are stored and an email is sent to notify for new ads.",
                    Contact = new OpenApiContact
                    {
                        Email = "kon.kri@outlook.com",
                        Name = "Kon Kri",
                        Url = new Uri("https://github.com/KonKri")
                    },
                    Version = "v1"
                };

                c.SwaggerDoc("v1", openApiInfo);

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FindPlaceToRentWebApi"));
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}