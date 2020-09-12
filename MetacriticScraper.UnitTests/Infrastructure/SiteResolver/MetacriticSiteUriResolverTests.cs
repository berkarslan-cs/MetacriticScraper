using MetacriticScraper.Infrastructure.SiteResolver;
using MetacriticScraper.Models;
using NUnit.Framework;
using Shouldly;

namespace MetacriticScraper.UnitTests.Infrastructure.Site
{
    /// <summary>
    /// Includes tests for <see cref="MetacriticSiteUriResolver"/>.
    /// </summary>
    public class MetacriticSiteUriResolverTests
    {
        [Test]
        public void GetSiteUrl_ForPS4Endpoint_ReturnsSuccessfully()
        {
            // Arrange
            var siteUriResolver = new MetacriticSiteUriResolver();

            // Act
            var result = siteUriResolver.GetSiteUrl(GamePlatform.PS4, 0);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.AbsoluteUri.ShouldBe(@"https://www.metacritic.com/browse/games/release-date/available/ps4/date?view=condensed&page=0"));
        }
    }
}
