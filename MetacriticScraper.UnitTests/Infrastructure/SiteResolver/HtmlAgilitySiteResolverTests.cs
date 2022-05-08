using System;
using System.Net;
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
        private const string NotEmptyText = "NotEmpty";

        [Test]
        public void GetMetacriticGameListHtmlDocument_ForPCEndpoint_ReturnsSuccessfully()
        {
            // Arrange
            var siteUriResolverMock = new Mock<ISiteUriResolver>();
            var htmlWebWrapper = new Mock<IHtmlWebWrapper>();
            var siteResolver = new HtmlAgilitySiteResolver(
                siteUriResolverMock.Object,
                htmlWebWrapper.Object);
            var expectedHtmlDocument = new HtmlDocument();
            var statusCodeOk = HttpStatusCode.OK;
            htmlWebWrapper
                .Setup(s => s.Load(It.IsAny<Uri>(), out statusCodeOk))
                .Returns(expectedHtmlDocument);
            expectedHtmlDocument.Text = NotEmptyText;

            // Act
            var result = siteResolver.GetMetacriticGameListHtmlDocument(GamePlatform.PC, 0);

            // Assert
            result.ShouldBe(expectedHtmlDocument);
        }

        [Test]
        public void GetMetacriticGameListHtmlDocument_ProvidedWithWrongUrl_ExecutesRepeatedly()
        {
            // Arrange
            var siteUriResolverMock = new Mock<ISiteUriResolver>();
            var htmlWebWrapper = new Mock<IHtmlWebWrapper>();
            var siteResolver = new HtmlAgilitySiteResolver(
                siteUriResolverMock.Object,
                htmlWebWrapper.Object);
            var expectedHtmlDocument = new HtmlDocument();
            var statusCodeOk = HttpStatusCode.OK;
            htmlWebWrapper
                .SetupSequence(s => s.Load(It.IsAny<Uri>(), out statusCodeOk))
                .Throws<Exception>()
                .Throws<Exception>()
                .Returns(expectedHtmlDocument);
            expectedHtmlDocument.Text = NotEmptyText;

            // Act
            var result = siteResolver.GetMetacriticGameListHtmlDocument(GamePlatform.PC, 0);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(expectedHtmlDocument),
                () => htmlWebWrapper.Verify(
                    a => a.Load(It.IsAny<Uri>(), out statusCodeOk),
                    Times.Exactly(3)));
        }

        [Test]
        public void GetMetacriticGameListHtmlDocument_ProvidedWithWrongUrl_ThrowsExceptionAfterExecutingRepeatedly()
        {
            // Arrange
            var siteUriResolverMock = new Mock<ISiteUriResolver>();
            var htmlWebWrapper = new Mock<IHtmlWebWrapper>();
            var siteResolver = new HtmlAgilitySiteResolver(
                siteUriResolverMock.Object,
                htmlWebWrapper.Object);
            var statusCodeBadRequest = HttpStatusCode.BadRequest;
            htmlWebWrapper
                .SetupSequence(s => s.Load(It.IsAny<Uri>(), out statusCodeBadRequest))
                .Throws<Exception>()
                .Throws<Exception>()
                .Throws<Exception>()
                .Returns(new HtmlDocument());

            // Act & Assert
            Should.Throw<Exception>(() => siteResolver.GetMetacriticGameListHtmlDocument(GamePlatform.PC, 0));
        }

        [Test]
        public void GetGameHtmlDocument_ForAUrl_ReturnsSuccessfully()
        {
            // Arrange
            var htmlWebWrapper = new Mock<IHtmlWebWrapper>();
            var siteResolver = new HtmlAgilitySiteResolver(
                null,
                htmlWebWrapper.Object);
            var expectedHtmlDocument = new HtmlDocument();
            var statusCodeOk = HttpStatusCode.OK;
            htmlWebWrapper
                .Setup(s => s.Load(It.IsAny<Uri>(), out statusCodeOk))
                .Returns(expectedHtmlDocument);
            expectedHtmlDocument.Text = NotEmptyText;

            // Act
            var result = siteResolver.GetGameHtmlDocument(@"http://sample.com");

            // Assert
            result.ShouldBe(expectedHtmlDocument);
        }

        [Test]
        public void GetGameHtmlDocument_ForAUrlThatGivesError_ExecutesRepeatedly()
        {
            // Arrange
            var htmlWebWrapper = new Mock<IHtmlWebWrapper>();
            var siteResolver = new HtmlAgilitySiteResolver(
                null,
                htmlWebWrapper.Object);
            var expectedHtmlDocument = new HtmlDocument();
            var statusCodeOk = HttpStatusCode.OK;
            htmlWebWrapper
                .SetupSequence(s => s.Load(It.IsAny<Uri>(), out statusCodeOk))
                .Throws<Exception>()
                .Throws<Exception>()
                .Returns(expectedHtmlDocument);
            expectedHtmlDocument.Text = NotEmptyText;

            // Act
            var result = siteResolver.GetGameHtmlDocument(@"http://sample.com");

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(expectedHtmlDocument),
                () => htmlWebWrapper.Verify(
                    a => a.Load(It.IsAny<Uri>(), out statusCodeOk),
                    Times.Exactly(3)));
        }

        [Test]
        public void GetGameHtmlDocument_ForAUrlThatGivesError_ThrowsExceptionAfterExecutingRepeatedly()
        {
            // Arrange
            var htmlWebWrapper = new Mock<IHtmlWebWrapper>();
            var siteResolver = new HtmlAgilitySiteResolver(
                null,
                htmlWebWrapper.Object);
            var statusCodeBadRequest = HttpStatusCode.BadRequest;
            htmlWebWrapper
                .SetupSequence(s => s.Load(It.IsAny<Uri>(), out statusCodeBadRequest))
                .Throws<Exception>()
                .Throws<Exception>()
                .Throws<Exception>()
                .Returns(new HtmlDocument());

            // Act & Assert
            Should.Throw<Exception>(() => siteResolver.GetGameHtmlDocument(@"http://sample.com"));
        }
    }
}
