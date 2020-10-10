using System.Xml.XPath;
using MetacriticScraper.Models;

namespace MetacriticScraper.Infrastructure.SiteResolver
{
    /// <summary>
    /// Manages HTML document for metacritic site.
    /// </summary>
    public interface ISiteResolver
    {
        /// <summary>
        /// Gets the html document which includes list of games for the given game platform and page number.
        /// </summary>
        /// <param name="gamePlatform">Game platform to scrape.</param>
        /// <param name="pageId">Page index to scrape.</param>
        /// <returns>The html document.</returns>
        IXPathNavigable GetMetacriticGameListHtmlDocument(GamePlatform gamePlatform, int pageId);

        /// <summary>
        /// Gets the html document for a specific game.
        /// </summary>
        /// <param name="url">Url of the game.</param>
        /// <returns>The html document.</returns>
        IXPathNavigable GetGameHtmlDocument(string url);
    }
}
