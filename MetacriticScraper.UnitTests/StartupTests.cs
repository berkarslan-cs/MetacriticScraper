using System.Linq;
using MetacriticScraper.Infrastructure.HtmlParser;
using MetacriticScraper.Infrastructure.Site;
using MetacriticScraper.Infrastructure.SiteResolver;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace MetacriticScraper.UnitTests
{
    /// <summary>
    /// Includes tests for <see cref="Startup"/>.
    /// </summary>
    public class StartupTests
    {
        [Test]
        public void ConfigureServices_ProvidedWithServicesCollection_RegistersDependenciesCorrectly()
        {
            // Arrange
            var configurationMock = new Mock<IConfiguration>();
            var serviceCollection = new ServiceCollection();
            var startup = new Startup(configurationMock.Object);

            // Act
            startup.ConfigureServices(serviceCollection);

            // Assert
            serviceCollection.ShouldSatisfyAllConditions(
                () => serviceCollection.FirstOrDefault(
                    a => a.ServiceType == typeof(IHtmlParser) &&
                    a.Lifetime == ServiceLifetime.Transient).ShouldNotBeNull(),
                () => serviceCollection.FirstOrDefault(
                    a => a.ServiceType == typeof(IMetacriticSite) &&
                    a.Lifetime == ServiceLifetime.Transient).ShouldNotBeNull(),
                () => serviceCollection.FirstOrDefault(
                    a => a.ServiceType == typeof(ISiteResolver) &&
                    a.Lifetime == ServiceLifetime.Transient).ShouldNotBeNull(),
                () => serviceCollection.FirstOrDefault(
                    a => a.ServiceType == typeof(ISiteUriResolver) &&
                    a.Lifetime == ServiceLifetime.Transient).ShouldNotBeNull(),
                () => serviceCollection.FirstOrDefault(
                    a => a.ServiceType == typeof(IMetacriticGameConverter) &&
                    a.Lifetime == ServiceLifetime.Transient).ShouldNotBeNull(),
                () => serviceCollection.FirstOrDefault(
                    a => a.ServiceType == typeof(IHtmlWebWrapper) &&
                    a.Lifetime == ServiceLifetime.Transient).ShouldNotBeNull());
        }
    }
}
