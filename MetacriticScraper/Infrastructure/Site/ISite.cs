using System.Collections.Generic;
using MetacriticScraper.Models;

namespace MetacriticScraper.Infrastructure.Site
{
    /// <summary>
    /// Game manager class for Metacritic site.
    /// </summary>
    public interface ISite
    {
        /// <summary>
        /// Gets the filtered game list.
        /// </summary>
        /// <param name="gameFilter">Filter to apply to all of the games in Metacritic.</param>
        /// <returns>Filtered game list.</returns>
        IList<Game> GetGames(GameFilter gameFilter);

        /// <summary>
        /// Gets the number of pages.
        /// </summary>
        /// <param name="gamePlatform">Game platform.</param>
        /// <returns>The number of pages in a specific platform.</returns>
        int GetNumberOfPages(GamePlatform gamePlatform);
    }
}
