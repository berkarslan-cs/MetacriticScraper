using MetacriticScraper.Models;

namespace MetacriticScraper.Infrastructure.HtmlParser
{
    /// <summary>
    /// Original text values of Metacritic game elements.
    /// </summary>
    public class MetacriticGame
    {
        /// <summary>
        /// Gets or sets name of the game.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets platform of the game.
        /// </summary>
        public GamePlatform Platform { get; set; }

        /// <summary>
        /// Gets or sets url of the game.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets Metacritic score of the game.
        /// </summary>
        public string MetaScore { get; set; }

        /// <summary>
        /// Gets or sets user score of the game.
        /// </summary>
        public string UserScore { get; set; }

        /// <summary>
        /// Gets or sets release date of the game.
        /// </summary>
        public string ReleaseDate { get; set; }

        /// <summary>
        /// Validates the game properties.
        /// </summary>
        /// <returns>True if valid; false in otherwise.</returns>
        public bool IsValid()
        {
            if (Name == null ||
                Url == null ||
                MetaScore == null ||
                UserScore == null ||
                ReleaseDate == null)
            {
                return false;
            }

            return true;
        }
    }
}
