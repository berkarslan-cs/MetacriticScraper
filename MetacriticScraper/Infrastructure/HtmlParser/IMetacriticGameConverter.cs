using MetacriticScraper.Models;

namespace MetacriticScraper.Infrastructure.HtmlParser
{
    /// <summary>
    /// Converter to convert metacritic text values to more meaningful values.
    /// </summary>
    public interface IMetacriticGameConverter
    {
        /// <summary>
        /// Returns <see cref="Game"/> from the given <see cref="MetacriticGame"/>.
        /// </summary>
        /// <param name="game">Metacritic game.</param>
        /// <returns>Game model.</returns>
        Game ConvertToGameEntity(MetacriticGameListPage game);

        /// <summary>
        /// Returns <see cref="Game"/> from the given <see cref="MetacriticGame"/>.
        /// </summary>
        /// <param name="game">Metacritic game.</param>
        /// <returns>Game model.</returns>
        Game ConvertToGameEntity(MetacriticGameDetailPage game);
    }
}
