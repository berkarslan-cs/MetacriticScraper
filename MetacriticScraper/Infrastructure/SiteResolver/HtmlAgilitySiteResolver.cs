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
        private readonly ISiteUriResolver siteUrlFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlAgilitySiteResolver"/> class.
        /// </summary>
        /// <param name="siteUrlFactory">Site url factory to resolve the urls.</param>
        public HtmlAgilitySiteResolver(ISiteUriResolver siteUrlFactory) => this.siteUrlFactory = siteUrlFactory;

        /// <inheritdoc />
        public IXPathNavigable GetHtmlDocument(GamePlatform gamePlatform, int pageId)
        {
            var siteUriToRequest = siteUrlFactory.GetSiteUrl(gamePlatform, pageId);
            return SendRequestWithRetrial(siteUriToRequest, NumberOfRetrial);
        }

        private static IXPathNavigable SendRequestWithRetrial(Uri siteUriToRequest, int retrialCount)
        {
            try
            {
                var web = new HtmlWeb();
                return web.Load(siteUriToRequest);
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
