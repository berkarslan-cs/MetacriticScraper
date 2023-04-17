using System;
using MetacriticScraper.Infrastructure.HtmlParser;
using MetacriticScraper.Models;
using NUnit.Framework;
using Shouldly;

namespace MetacriticScraper.UnitTests.Infrastructure.HtmlParser
{
    /// <summary>
    /// Includes tests for <see cref="MetacriticGameConverter"/>.
    /// </summary>
    public class MetacriticGameConverterTests
    {
        [Test]
        public void ConvertToGameEntity_WithUnrecognizedParameters_ThrowsArgumentException()
        {
            // Arrange
            var converter = new MetacriticGameConverter();
            var game = new MetacriticGame();

            // Act & Assert
            Should.Throw<Exception>(() => converter.ConvertToGameEntity(game));
        }

        [TestCase("100", "Name", "PC", "September 8, 2020", "http://sample.org", "invalid")]
        [TestCase("invalid", "Name", "PC", "September 8, 2020", "http://sample.org", "10")]
        [TestCase("100", "Name", "PC", "invalid", "http://sample.org", "10")]
        public void ConvertToGameEntity_WithUnrecognizedParameters_ThrowsArgumentException(
            string metaScore,
            string name,
            string platform,
            string releaseDate,
            string url,
            string userScore)
        {
            // Arrange
            var converter = new MetacriticGameConverter();
            var game = new MetacriticGame
            {
                MetaScore = metaScore,
                Name = name,
                Platform = platform,
                ReleaseDate = releaseDate,
                Url = url,
                UserScore = userScore,
            };

            // Act & Assert
            Should.Throw<ArgumentException>(() => converter.ConvertToGameEntity(game));
        }

        [TestCase("TbD", "\r\t\nName\r\t\n", "PS4", "September 8, 2020", "/test/", "TbD", null, null, null, "Name", GamePlatform.PS4, "2020-09-08", "https://www.metacritic.com/test/", null, null, null)]
        [TestCase("TbD", "\r\t\nName\r\t\n", "PlayStation 4", "September 8, 2020", "/test/", "TbD", "", null, null, "Name", GamePlatform.PS4, "2020-09-08", "https://www.metacritic.com/test/", null, null, null)]
        [TestCase("100", "Name", "PS4", "September 8, 2020", "test", "5.5", "3", "4", 100, "Name", GamePlatform.PS4, "2020-09-08", "https://www.metacritic.com/test", 5.5, 3, 4)]
        public void ConvertToGameEntity_WithValidParameters_ReturnsSuccessfully(
            string metaScore,
            string name,
            string platform,
            string releaseDate,
            string url,
            string userScore,
            string numberOfCriticReviews,
            string numberOfUserReviews,
            int? expectedMetaScore,
            string expectedName,
            GamePlatform expectedPlatform,
            DateTime expectedReleaseDate,
            string expectedUrl,
            decimal? expectedUserScore,
            int? expectedNumberOfCriticReviews,
            int? expectedNumberOfUserReviews)
        {
            // Arrange
            var converter = new MetacriticGameConverter();
            var game = new MetacriticGame
            {
                MetaScore = metaScore,
                Name = name,
                Platform = platform,
                ReleaseDate = releaseDate,
                Url = url,
                UserScore = userScore,
                NumberOfCriticReviews = numberOfCriticReviews,
                NumberOfUserReviews = numberOfUserReviews,
            };

            // Act
            var result = converter.ConvertToGameEntity(game);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.MetaScore.ShouldBe(expectedMetaScore),
                () => result.Name.ShouldBe(expectedName),
                () => result.Platform.ShouldBe(expectedPlatform),
                () => result.ReleaseDate.ShouldBe(expectedReleaseDate),
                () => result.Url.ShouldBe(expectedUrl),
                () => result.UserScore.ShouldBe(expectedUserScore),
                () => result.NumberOfCriticReviews.ShouldBe(expectedNumberOfCriticReviews),
                () => result.NumberOfUserReviews.ShouldBe(expectedNumberOfUserReviews));
        }
    }
}
