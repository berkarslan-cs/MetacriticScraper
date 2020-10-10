using MetacriticScraper.Infrastructure.HtmlParser;
using NUnit.Framework;
using Shouldly;

namespace MetacriticScraper.UnitTests.Infrastructure.HtmlParser
{
    /// <summary>
    /// Includes tests for <see cref="MetacriticGame"/>.
    /// </summary>
    public class MetacriticGameTests
    {
        [TestCase(false, null, null, "PC", null, null, null, null, null, false)]
        [TestCase(false, null, "Name", "PC", "September 8, 2020", "http://sample.org", "10", null, null, false)]
        [TestCase(false, "100", null, "PC", "September 8, 2020", "http://sample.org", "10", null, null, false)]
        [TestCase(false, "100", "Name", "PC", null, "http://sample.org", "10", null, null, false)]
        [TestCase(false, "100", "Name", "PC", "September 8, 2020", null, "10", null, null, false)]
        [TestCase(false, "100", "Name", "PC", "September 8, 2020", "http://sample.org", null, null, null, false)]
        [TestCase(false, "100", "Name", "PC", "September 8, 2020", "http://sample.org", "10", null, null, true)]
        [TestCase(true, null, "Name", "PC", "September 8, 2020", "http://sample.org", null, "3", "4", true)]
        [TestCase(true, "100", "Name", "PC", "September 8, 2020", "http://sample.org", "10", "3", null, false)]
        [TestCase(true, "100", "Name", "PC", "September 8, 2020", "http://sample.org", "10", null, "4", true)]
        public void IsValid_UsingDifferentParameters_ReturnsCorrectResult(
            bool detailPageValidation,
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
            var game = new MetacriticGame
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
            var result = game.IsValid(detailPageValidation);

            // Assert
            result.ShouldBe(expectedResult);
        }
    }
}
