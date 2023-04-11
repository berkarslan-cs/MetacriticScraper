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
        private const string Game1Url = @"http://sample1.org";
        private const string Game2Url = @"http://sample2.org";
        private const string Game3Url = @"http://sample3.org";
        private const string Game4Url = @"http://sample4.org";
        private const string Game5Url = @"http://sample5.org";
#pragma warning disable SA1311 // Static readonly fields should begin with upper-case letter
        private static readonly Game game1 = new ()
        {
            MetaScore = 100,
            Name = "1",
            Platform = GamePlatform.PC,
            ReleaseDate = new DateTime(DateTime.Now.Year, 1, 1),
            Url = Game1Url,
            UserScore = 10,
        };

        private static readonly Game game2 = new ()
        {
            MetaScore = 63,
            Name = "2",
            Platform = GamePlatform.PC,
            ReleaseDate = new DateTime(DateTime.Now.Year - 1, 12, 31),
            Url = Game2Url,
            UserScore = 6.3M,
        };

        private static readonly Game game3 = new ()
        {
            MetaScore = 59,
            Name = "3",
            Platform = GamePlatform.PC,
            ReleaseDate = new DateTime(DateTime.Now.Year - 1, 1, 1),
            Url = Game3Url,
            UserScore = 3.4M,
        };

        private static readonly Game game4 = new ()
        {
            MetaScore = 61,
            Name = "4",
            Platform = GamePlatform.PC,
            ReleaseDate = new DateTime(DateTime.Now.Year - 2, 12, 11),
            Url = Game4Url,
            UserScore = 5.9M,
        };

        private static readonly Game game5 = new ()
        {
            MetaScore = 41,
            Name = "5",
            Platform = GamePlatform.PS4,
            ReleaseDate = new DateTime(DateTime.Now.Year - 2, 12, 11),
            Url = Game5Url,
            UserScore = 4.1M,
        };

        private static readonly IList<Game> games = new List<Game>
        {
            game1,
            game2,
            game3,
            game4,
            game5,
        };

        private static readonly Game gameWithDetail1 = new ()
        {
            MetaScore = 100,
            Name = "1",
            Platform = GamePlatform.PC,
            ReleaseDate = new DateTime(DateTime.Now.Year, 1, 1),
            Url = Game1Url,
            UserScore = 10,
            GameDetail = new GameDetail
            {
                NumberOfCriticReviews = 7,
                NumberOfUserReviews = 10,
            },
        };

        private static readonly Game gameWithDetail2 = new ()
        {
            MetaScore = 63,
            Name = "2",
            Platform = GamePlatform.PC,
            ReleaseDate = new DateTime(DateTime.Now.Year - 1, 12, 31),
            Url = Game2Url,
            UserScore = 6.3M,
            GameDetail = new GameDetail
            {
                NumberOfUserReviews = 10,
            },
        };

        private static readonly Game gameWithDetail3 = new ()
        {
            MetaScore = 59,
            Name = "3",
            Platform = GamePlatform.PC,
            ReleaseDate = new DateTime(DateTime.Now.Year - 1, 1, 1),
            Url = Game3Url,
            UserScore = 3.4M,
            GameDetail = new GameDetail
            {
                NumberOfCriticReviews = 7,
            },
        };

        private static readonly Game gameWithDetail4 = new ()
        {
            MetaScore = 61,
            Name = "4",
            Platform = GamePlatform.PC,
            ReleaseDate = new DateTime(DateTime.Now.Year - 2, 12, 11),
            Url = Game4Url,
            UserScore = 5.9M,
            GameDetail = new GameDetail
            {
                NumberOfCriticReviews = 7,
                NumberOfUserReviews = 3,
            },
        };

        private static readonly Game gameWithDetail5 = new ()
        {
            MetaScore = 41,
            Name = "5",
            Platform = GamePlatform.PS4,
            ReleaseDate = new DateTime(DateTime.Now.Year - 2, 12, 11),
            Url = Game5Url,
            UserScore = 4.1M,
        };

        private static readonly IList<Game> gamesWithDetail = new List<Game>
        {
            gameWithDetail1,
            gameWithDetail2,
            gameWithDetail3,
            gameWithDetail4,
            gameWithDetail5,
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
            var game1HtmlDoc = new Mock<IXPathNavigable>();
            var game2HtmlDoc = new Mock<IXPathNavigable>();
            var game3HtmlDoc = new Mock<IXPathNavigable>();
            var game4HtmlDoc = new Mock<IXPathNavigable>();
            var game5HtmlDoc = new Mock<IXPathNavigable>();
            htmlParserMock
                .Setup(s => s.GetNumberOfPages(It.IsAny<IXPathNavigable>()))
                .Returns(1);
            htmlParserMock
                .Setup(s => s.GetGames(It.IsAny<IXPathNavigable>()))
                .Returns(games);
            siteResolverMock
                .Setup(s => s.GetGameHtmlDocument(Game1Url))
                .Returns(game1HtmlDoc.Object);
            siteResolverMock
                .Setup(s => s.GetGameHtmlDocument(Game2Url))
                .Returns(game2HtmlDoc.Object);
            siteResolverMock
                .Setup(s => s.GetGameHtmlDocument(Game3Url))
                .Returns(game3HtmlDoc.Object);
            siteResolverMock
                .Setup(s => s.GetGameHtmlDocument(Game4Url))
                .Returns(game4HtmlDoc.Object);
            siteResolverMock
                .Setup(s => s.GetGameHtmlDocument(Game5Url))
                .Returns(game5HtmlDoc.Object);
            htmlParserMock
                .Setup(s => s.GetGameDetails(game1HtmlDoc.Object))
                .Returns(gamesWithDetail[0]);
            htmlParserMock
                .Setup(s => s.GetGameDetails(game2HtmlDoc.Object))
                .Returns(gamesWithDetail[1]);
            htmlParserMock
                .Setup(s => s.GetGameDetails(game3HtmlDoc.Object))
                .Returns(gamesWithDetail[2]);
            htmlParserMock
                .Setup(s => s.GetGameDetails(game4HtmlDoc.Object))
                .Returns(gamesWithDetail[3]);
            htmlParserMock
                .Setup(s => s.GetGameDetails(game5HtmlDoc.Object))
                .Returns(gamesWithDetail[4]);

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
                    gameWithDetail1,
                    gameWithDetail2,
                    gameWithDetail3,
                    gameWithDetail4,
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
                    MinNumberOfUserReviews = 10,
                },
                new Game[]
                {
                    gameWithDetail1,
                    gameWithDetail2,
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
                    MinNumberOfUserReviews = 3,
                },
                new Game[]
                {
                    gameWithDetail1,
                    gameWithDetail2,
                    gameWithDetail4,
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
                    gameWithDetail5,
                },
            };
        }
    }
}
