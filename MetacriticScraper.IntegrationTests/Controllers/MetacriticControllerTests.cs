using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using MetacriticScraper.Controllers;
using MetacriticScraper.Infrastructure.HtmlParser;
using MetacriticScraper.Infrastructure.Site;
using MetacriticScraper.Infrastructure.SiteResolver;
using MetacriticScraper.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shouldly;

namespace MetacriticScraper.IntegrationTests.Controllers
{
    /// <summary>
    /// Includes integration tests for <see cref="MetacriticController"/>.
    /// </summary>
    [TestFixture]
    public class MetacriticControllerTests
    {
        private IServiceProvider serviceProvider;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddTransient<IHtmlParser, MetacriticHtmlParser>();
            services.AddTransient<IMetacriticSite, MetacriticSite>();
            services.AddTransient<ISiteResolver, HtmlAgilitySiteResolver>();
            services.AddTransient<ISiteUriResolver, MetacriticSiteUriResolver>();
            services.AddTransient<IMetacriticGameConverter, MetacriticGameConverter>();
            services.AddTransient<IHtmlWebWrapper, HtmlWebWrapper>(_ => new HtmlWebWrapper(new HtmlWeb()));
            serviceProvider = services.BuildServiceProvider();
        }

        [Test]
        public void Get_ProvidedWithFilterParam_ShouldReturnTheGamesSuccessfully()
        {
            // Arrange
            var gameFilter = new GameFilter()
            {
                MinMetaScore = 10,
                MinReleaseDate = DateTime.Now.AddMonths(-1),
                MinUserScore = 1,
                Platform = GamePlatform.PC,
            };
            var metaCriticController = new MetacriticController(serviceProvider.GetService<IMetacriticSite>());

            // Act
            var result = metaCriticController.Get(gameFilter);

            // Assert
            var games = (result.Result as OkObjectResult)?.Value as IList<Game> ?? new List<Game>();
            result.ShouldSatisfyAllConditions(
                () => result.Result.ShouldBeOfType<OkObjectResult>(),
                () => games.ShouldNotBeEmpty(),
                () => games.FirstOrDefault()?.Name?.ShouldNotBeNull(),
                () => games.FirstOrDefault()?.Url?.ShouldNotBeNull());
        }
    }
}
