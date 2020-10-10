using System.Collections.Generic;
using System.Xml.XPath;
using MetacriticScraper.Models;

namespace MetacriticScraper.Infrastructure.HtmlParser
{
    /// <summary>
    /// Html parser class for Metacritic.
    /// </summary>
    public interface IHtmlParser
    {
        /// <summary>
        /// Parses the html document for a specific game and scrapes the details.
        /// </summary>
        /// <param name="xPathNavigableHtmlDocument">Html document to parse.</param>
        /// <returns>Game details.</returns>
        Game GetGameDetails(IXPathNavigable xPathNavigableHtmlDocument);

        /// <summary>
        /// Parses the html document and scrapes all of the game elements in it.
        /// </summary>
        /// <param name="xPathNavigableHtmlDocument">Html document to parse.</param>
        /// <returns>All of the game data in the html document with basic info.</returns>
        IList<Game> GetGames(IXPathNavigable xPathNavigableHtmlDocument);

        /// <summary>
        /// Gets the number of pages.
        /// </summary>
        /// <param name="xPathNavigableHtmlDocument">Html document to parse.</param>
        /// <returns>The number of pages in a specific platform.</returns>
        int GetNumberOfPages(IXPathNavigable xPathNavigableHtmlDocument);
    }
}
