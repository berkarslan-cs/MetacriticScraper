using System;
using System.IO;
using System.Xml.XPath;
using HtmlAgilityPack;
using MetacriticScraper.Infrastructure.HtmlParser;
using MetacriticScraper.Models;
using MetacriticScraper.UnitTests.TestData;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace MetacriticScraper.UnitTests.Infrastructure.HtmlParser
{
    /// <summary>
    /// Includes tests for <see cref="MetacriticHtmlParser"/>.
    /// </summary>
    public class MetacriticHtmlParserTests
    {
        [Test]
        public void GetGames_WithWrongTypeOfHtmlDocument_ShouldThrowException()
        {
            // Arrange
            var converter = new Mock<MetacriticGameConverter>();
            var parser = new MetacriticHtmlParser(converter.Object);
            IXPathNavigable htmlDocument = null;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => parser.GetGames(
                htmlDocument,
                GamePlatform.PC));
        }

        [Test]
        public void GetGames_WithPreloadedHtmlFileWhichContainsTheFirstPageOfMetacritic_ShouldReturnTheGamesSuccessfully()
        {
            // Arrange
            const int expectedCountOfGamesInFirstPage = 200;
            var converter = new Mock<MetacriticGameConverter>();
            var parser = new MetacriticHtmlParser(converter.Object);
            var htmlDocument = new HtmlDocument();
            var html = File.ReadAllText(TestDataPaths.FirstPageHtml);
            htmlDocument.LoadHtml(html);

            // Act
            var result = parser.GetGames(
                htmlDocument,
                GamePlatform.PC);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Count.ShouldBe(expectedCountOfGamesInFirstPage));
        }

        [Test]
        public void GetNumberOfPages_WithWrongTypeOfHtmlDocument_ShouldThrowException()
        {
            // Arrange
            var converter = new Mock<MetacriticGameConverter>();
            var parser = new MetacriticHtmlParser(converter.Object);
            IXPathNavigable htmlDocument = null;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => parser.GetNumberOfPages(htmlDocument));
        }

        [Test]
        public void GetNumberOfPages_WithPreloadedHtmlFileWhichContainsTheFirstPageOfMetacritic_ShouldReturnTheLastPageSuccessfully()
        {
            // Arrange
            const int expectedLastPage = 232;
            var converter = new Mock<MetacriticGameConverter>();
            var parser = new MetacriticHtmlParser(converter.Object);
            var htmlDocument = new HtmlDocument();
            var html = File.ReadAllText(TestDataPaths.FirstPageHtml);
            htmlDocument.LoadHtml(html);

            // Act
            var result = parser.GetNumberOfPages(htmlDocument);

            // Assert
            result.ShouldBe(expectedLastPage);
        }
    }
}
