using MetacriticScraper.Infrastructure.HtmlParser;
using MetacriticScraper.Models;
using NUnit.Framework;
using Shouldly;

namespace MetacriticScraper.UnitTests.Infrastructure.HtmlParser
{
    /// <summary>
    /// Includes tests for <see cref="MetacriticGame"/>.
    /// </summary>
    public class MetacriticGameTests
    {
        [TestCase(null, null, GamePlatform.PC, null, null, null, false)]
        [TestCase(null, "Name", GamePlatform.PC, "Dec 16", "http://sample.org", "10", false)]
        [TestCase("100", null, GamePlatform.PC, "Dec 16", "http://sample.org", "10", false)]
        [TestCase("100", "Name", GamePlatform.PC, null, "http://sample.org", "10", false)]
        [TestCase("100", "Name", GamePlatform.PC, "Dec 16", null, "10", false)]
        [TestCase("100", "Name", GamePlatform.PC, "Dec 16", "http://sample.org", null, false)]
        [TestCase("100", "Name", GamePlatform.PC, "Dec 16", "http://sample.org", "10", true)]
        public void IsValid_UsingDifferentParameters_ReturnsCorrectResult(
            string metaScore,
            string name,
            GamePlatform platform,
            string releaseDate,
            string url,
            string userScore,
            bool expectedResult)
        {
            // Arrange
            var game = new MetacriticGame()
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
