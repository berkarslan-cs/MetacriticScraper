using System;
using MetacriticScraper.Models;

namespace MetacriticScraper.Infrastructure.SiteResolver
{
    /// <summary>
    /// Resolves site url to scrape.
    /// </summary>
    public interface ISiteUriResolver
    {
        /// <summary>
        /// Evaluates the site url to scrape.
        /// </summary>
        /// <param name="gamePlatform">Game platform.</param>
        /// <param name="pageId">Page id in the site url to scrape.</param>
        /// <returns>Site uri to scrape.</returns>
        Uri GetSiteUrl(GamePlatform gamePlatform, int pageId);
    }
}
