using System;
using System.Net;
using HtmlAgilityPack;

namespace MetacriticScraper.Infrastructure.SiteResolver
{
    /// <summary>
    /// Wrapper for <see cref="HtmlWeb"/>.
    /// </summary>
    public interface IHtmlWebWrapper
    {
        /// <summary>
        /// Loads html document from the given uri.
        /// </summary>
        /// <param name="uri">Uri to load the html from.</param>
        /// <param name="httpStatusCode">The HTTP status code of the response.</param>
        /// <returns>Html documment.</returns>
        HtmlDocument Load(Uri uri, out HttpStatusCode httpStatusCode);
    }
}
