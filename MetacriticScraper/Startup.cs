﻿using System.Globalization;
using HtmlAgilityPack;
using MetacriticScraper.Infrastructure.HtmlParser;
using MetacriticScraper.Infrastructure.Site;
using MetacriticScraper.Infrastructure.SiteResolver;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MetacriticScraper
{
    /// <summary>
    /// Owin startup.
    /// </summary>
    public class Startup
    {
        private const string EnglishCultureName = "en-US";

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Configuration param.</param>
        public Startup(IConfiguration configuration) => Configuration = configuration;

        /// <summary>
        /// Gets application configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Service list.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // DI
            services.AddTransient<IHtmlParser, MetacriticHtmlParser>();
            services.AddTransient<IMetacriticSite, MetacriticSite>();
            services.AddTransient<ISiteResolver, HtmlAgilitySiteResolver>();
            services.AddTransient<ISiteUriResolver, MetacriticSiteUriResolver>();
            services.AddTransient<IMetacriticGameConverter, MetacriticGameConverter>();
            services.AddTransient<IHtmlWebWrapper, HtmlWebWrapper>(_ => new HtmlWebWrapper(new HtmlWeb()));
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application builder object.</param>
        /// <param name="env">Hosting environment.</param>
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

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseStaticFiles();

            // Set default culture for the threads
            var cultureInfo = new CultureInfo(EnglishCultureName);
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }
    }
}
