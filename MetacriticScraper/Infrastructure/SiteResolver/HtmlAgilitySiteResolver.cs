using System;
using System.Xml.XPath;
using HtmlAgilityPack;
using MetacriticScraper.Models;

namespace MetacriticScraper.Infrastructure.SiteResolver
{
    /// <inheritdoc />
    public class HtmlAgilitySiteResolver : ISiteResolver
    {
        private const int NumberOfRetrial = 3;
        private const string NoContentReturnedErrorMessage = "No content has returned.";
        private readonly ISiteUriResolver siteUrlFactory;
        private readonly IHtmlWebWrapper htmlWebWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlAgilitySiteResolver"/> class.
        /// </summary>
        /// <param name="siteUrlFactory">Site url factory to resolve the urls.</param>
        /// <param name="htmlWebWrapper">The wrapper class for <see cref="HtmlWeb" />.</param>
        public HtmlAgilitySiteResolver(
            ISiteUriResolver siteUrlFactory,
            IHtmlWebWrapper htmlWebWrapper)
        {
            this.siteUrlFactory = siteUrlFactory;
            this.htmlWebWrapper = htmlWebWrapper;
        }

        /// <inheritdoc />
        public IXPathNavigable GetMetacriticGameListHtmlDocument(GamePlatform gamePlatform, int pageId)
        {
            var siteUriToRequest = siteUrlFactory.GetSiteUrl(gamePlatform, pageId);
            return SendRequestWithRetrial(siteUriToRequest, NumberOfRetrial);
        }

        /// <inheritdoc />
        public IXPathNavigable GetGameHtmlDocument(string url) => SendRequestWithRetrial(new Uri(url), NumberOfRetrial);

        private IXPathNavigable SendRequestWithRetrial(Uri siteUriToRequest, int retrialCount)
        {
            try
            {
                var htmlDocument = htmlWebWrapper.Load(siteUriToRequest);
                if (string.IsNullOrWhiteSpace(htmlDocument?.Text))
                {
                    throw new Exception(NoContentReturnedErrorMessage);
                }

                return htmlDocument;
            }
            catch
            {
                if (retrialCount == 1)
                {
                    throw;
                }

                return SendRequestWithRetrial(siteUriToRequest, --retrialCount);
            }
        }
    }
}
