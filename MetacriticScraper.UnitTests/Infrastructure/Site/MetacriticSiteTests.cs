using System;
using System.Collections.Generic;
using System.Xml.XPath;
using MetacriticScraper.Infrastructure.HtmlParser;
using MetacriticScraper.Infrastructure.Site;
using MetacriticScraper.Infrastructure.SiteResolver;
using MetacriticScraper.Models;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace MetacriticScraper.UnitTests.Infrastructure.Site
{
    /// <summary>
    /// Includes tests for <see cref="MetacriticSite"/>.
    /// </summary>
    public class MetacriticSiteTests
    {
#pragma warning disable SA1311 // Static readonly fields should begin with upper-case letter
        private static readonly Game game1 = new Game()
        {
            MetaScore = 100,
            Name = "1",
            Platform = GamePlatform.PC,
            ReleaseDateWithoutCorrectYear = new DateTime(DateTime.Now.Year, 1, 1), // Represents YYYY-1-1
            Url = @"http://sample1.org",
            UserScore = 10,
        };

        private static readonly Game game2 = new Game()
        {
            MetaScore = 63,
            Name = "2",
            Platform = GamePlatform.PC,
            ReleaseDateWithoutCorrectYear = new DateTime(DateTime.Now.Year, 12, 31), // Represents (YYYY-1)-12-31
            Url = @"http://sample2.org",
            UserScore = 6.3M,
        };

        private static readonly Game game3 = new Game()
        {
            MetaScore = 59,
            Name = "3",
            Platform = GamePlatform.PC,
            ReleaseDateWithoutCorrectYear = new DateTime(DateTime.Now.Year, 1, 1), // Represents (YYYY-1)-1-1
            Url = @"http://sample3.org",
            UserScore = 3.4M,
        };

        private static readonly Game game4 = new Game()
        {
            MetaScore = 61,
            Name = "4",
            Platform = GamePlatform.PC,
            ReleaseDateWithoutCorrectYear = new DateTime(DateTime.Now.Year, 12, 11), // Represents (YYYY-2)-12-11
            Url = @"http://sample4.org",
            UserScore = 5.9M,
        };

        private static readonly Game game5 = new Game()
        {
            MetaScore = 41,
            Name = "5",
            Platform = GamePlatform.PS4,
            ReleaseDateWithoutCorrectYear = new DateTime(DateTime.Now.Year, 12, 11), // Represents (YYYY-2)-12-11
            Url = @"http://sample4.org",
            UserScore = 4.1M,
        };

        private static readonly IList<Game> games = new List<Game>()
        {
            game1,
            game2,
            game3,
            game4,
            game5,
        };
#pragma warning restore SA1311 // Static readonly fields should begin with upper-case letter

        [Test]
        public void GetGames_WithNullParameters_ThrowsArgumentNullException()
        {
            // Arrange
            var siteResolverMock = new Mock<ISiteResolver>();
            var htmlParserMock = new Mock<IHtmlParser>();
            var site = new MetacriticSite(siteResolverMock.Object, htmlParserMock.Object);

            // Act & Arrange
            Should.Throw<ArgumentNullException>(() => site.GetGames(null));
        }

        [TestCaseSource(nameof(GetGamesParameters))]
        public void GetGames_WithCorrectParameters_ReturnsFilteredResults(
            GameFilter gameFilter,
            Game[] expectedGames)
        {
            // Arrange
            var siteResolverMock = new Mock<ISiteResolver>();
            var htmlParserMock = new Mock<IHtmlParser>();
            var site = new MetacriticSite(siteResolverMock.Object, htmlParserMock.Object);
            htmlParserMock
                .Setup(s => s.GetNumberOfPages(It.IsAny<IXPathNavigable>()))
                .Returns(1);
            htmlParserMock
                .Setup(s => s.GetGames(
                    It.IsAny<IXPathNavigable>(),
                    It.IsAny<GamePlatform>()))
                .Returns(games);

            // Act
            var result = site.GetGames(gameFilter);

            // Assert
            result.ShouldBe(expectedGames);
        }

        private static IEnumerable<object[]> GetGamesParameters()
        {
            yield return new object[]
            {
                new GameFilter(),
                new Game[]
                {
                    game1,
                    game2,
                    game3,
                    game4,
                },
            };
            yield return new object[]
            {
                new GameFilter()
                {
                    MinMetaScore = 63,
                    MinReleaseDate = new DateTime(DateTime.Now.Year - 1, 12, 31),
                    MinUserScore = 6.3M,
                    Platform = GamePlatform.PC,
                },
                new Game[]
                {
                    game1,
                    game2,
                },
            };
            yield return new object[]
            {
                new GameFilter()
                {
                    MinMetaScore = 59,
                    MinReleaseDate = new DateTime(DateTime.Now.Year - 2, 12, 11),
                    MinUserScore = 5.9M,
                    Platform = GamePlatform.PC,
                },
                new Game[]
                {
                    game1,
                    game2,
                    game4,
                },
            };
            yield return new object[]
            {
                new GameFilter()
                {
                    MinReleaseDate = new DateTime(DateTime.Now.Year - 2, 12, 11),
                    Platform = GamePlatform.PS4,
                },
                new Game[]
                {
                    game5,
                },
            };
        }
    }
}
