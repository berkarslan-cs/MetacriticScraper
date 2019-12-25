using System;
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
        /// <returns>Html documment.</returns>
        HtmlDocument Load(Uri uri);
    }
}
