using MetacriticScraper.Infrastructure.HtmlParser;
using NUnit.Framework;
using Shouldly;

namespace MetacriticScraper.UnitTests.Infrastructure.HtmlParser
{
    /// <summary>
    /// Includes tests for <see cref="MetacriticGameDetailPage"/>.
    /// </summary>
    public class MetacriticGameDetailPageTests
    {
        [TestCase(null, "Name", "PC", "September 8, 2020", "http://sample.org", null, "3", "4", true)]
        [TestCase("100", "Name", "PC", "September 8, 2020", "http://sample.org", "10", "3", null, false)]
        [TestCase("100", "Name", "PC", "September 8, 2020", "http://sample.org", "10", null, "4", true)]
        public void IsValid_UsingDifferentParameters_ReturnsCorrectResult(
            string metaScore,
            string name,
            string platform,
            string releaseDate,
            string url,
            string userScore,
            string numberOfUserReviews,
            string numberOfCriticReviews,
            bool expectedResult)
        {
            // Arrange
            var game = new MetacriticGameDetailPage
            {
                MetaScore = metaScore,
                Name = name,
                Platform = platform,
                ReleaseDate = releaseDate,
                Url = url,
                UserScore = userScore,
                NumberOfUserReviews = numberOfUserReviews,
                NumberOfCriticReviews = numberOfCriticReviews,
            };

            // Act
            var result = game.IsValid();

            // Assert
            result.ShouldBe(expectedResult);
        }
    }
}
