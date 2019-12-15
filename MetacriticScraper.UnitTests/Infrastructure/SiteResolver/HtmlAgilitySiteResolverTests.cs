using System;
using HtmlAgilityPack;
using MetacriticScraper.Infrastructure.SiteResolver;
using MetacriticScraper.Models;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace MetacriticScraper.UnitTests.Infrastructure.Site
{
    /// <summary>
    /// Includes tests for <see cref="HtmlAgilitySiteResolver"/>.
    /// </summary>
    public class HtmlAgilitySiteResolverTests
    {
        [Test]
        public void GetHtmlDocument_ForPCEndpoint_ReturnsSuccessfully()
        {
            // Arrange
            var siteUriResolverMock = new Mock<ISiteUriResolver>();
            var htmlWebWrapper = new Mock<IHtmlWebWrapper>();
            var siteResolver = new HtmlAgilitySiteResolver(
                siteUriResolverMock.Object,
                htmlWebWrapper.Object);
            var expectedHtmlDocument = new HtmlDocument();
            htmlWebWrapper
                .Setup(s => s.Load(It.IsAny<Uri>()))
                .Returns(expectedHtmlDocument);

            // Act
            var result = siteResolver.GetHtmlDocument(GamePlatform.PC, 0);

            // Assert
            result.ShouldBe(expectedHtmlDocument);
        }

        [Test]
        public void GetHtmlDocument_ProvidedWithWrongUrl_ExecutesRepeatedly()
        {
            // Arrange
            var siteUriResolverMock = new Mock<ISiteUriResolver>();
            var htmlWebWrapper = new Mock<IHtmlWebWrapper>();
            var siteResolver = new HtmlAgilitySiteResolver(
                siteUriResolverMock.Object,
                htmlWebWrapper.Object);
            var expectedHtmlDocument = new HtmlDocument();
            htmlWebWrapper
                .SetupSequence(s => s.Load(It.IsAny<Uri>()))
                .Throws<Exception>()
                .Throws<Exception>()
                .Returns(expectedHtmlDocument);

            // Act
            var result = siteResolver.GetHtmlDocument(GamePlatform.PC, 0);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(expectedHtmlDocument),
                () => htmlWebWrapper.Verify(
                    a => a.Load(It.IsAny<Uri>()),
                    Times.Exactly(3)));
        }

        [Test]
        public void GetHtmlDocument_ProvidedWithWrongUrl_ThrowsExceptionAfterExecutingRepeatedly()
        {
            // Arrange
            var siteUriResolverMock = new Mock<ISiteUriResolver>();
            var htmlWebWrapper = new Mock<IHtmlWebWrapper>();
            var siteResolver = new HtmlAgilitySiteResolver(
                siteUriResolverMock.Object,
                htmlWebWrapper.Object);
            htmlWebWrapper
                .SetupSequence(s => s.Load(It.IsAny<Uri>()))
                .Throws<Exception>()
                .Throws<Exception>()
                .Throws<Exception>()
                .Returns(new HtmlDocument());

            // Act & Assert
            Should.Throw<Exception>(() => siteResolver.GetHtmlDocument(GamePlatform.PC, 0));
        }
    }
}
