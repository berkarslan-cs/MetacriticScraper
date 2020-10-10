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
        /// <param name="detailPageValidation">Flag for validating the game entity for detail page.</param>
        /// <returns>Game model.</returns>
        Game ConvertToGameEntity(
            MetacriticGame game,
            bool detailPageValidation = false);
    }
}
