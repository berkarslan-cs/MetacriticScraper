using MetacriticScraper.Infrastructure.HtmlParser;
using NUnit.Framework;
using Shouldly;

namespace MetacriticScraper.UnitTests.Infrastructure.HtmlParser
{
    /// <summary>
    /// Includes tests for <see cref="MetacriticGameListPage"/>.
    /// </summary>
    public class MetacriticGameListPageTests
    {
        [TestCase(null, null, "PC", null, null, null, false)]
        [TestCase(null, "Name", "PC", "September 8, 2020", "http://sample.org", "10", false)]
        [TestCase("100", null, "PC", "September 8, 2020", "http://sample.org", "10", false)]
        [TestCase("100", "Name", "PC", null, "http://sample.org", "10", false)]
        [TestCase("100", "Name", "PC", "September 8, 2020", null, "10", false)]
        [TestCase("100", "Name", "PC", "September 8, 2020", "http://sample.org", null, false)]
        [TestCase("100", "Name", "PC", "September 8, 2020", "http://sample.org", "10", true)]
        public void IsValid_UsingDifferentParameters_ReturnsCorrectResult(
            string metaScore,
            string name,
            string platform,
            string releaseDate,
            string url,
            string userScore,
            bool expectedResult)
        {
            // Arrange
            var game = new MetacriticGameListPage
            {
                MetaScore = metaScore,
                Name = name,
                Platform = platform,
                ReleaseDate = releaseDate,
                Url = url,
                UserScore = userScore,
            };

            // Act
            var result = game.IsValid();

            // Assert
            result.ShouldBe(expectedResult);
        }
    }
}
