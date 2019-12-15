using System;

namespace MetacriticScraper.Models
{
    /// <summary>
    /// Game model.
    /// </summary>
    public class Game
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
        public decimal? MetaScore { get; set; }

        /// <summary>
        /// Gets or sets user score of the game.
        /// </summary>
        public decimal? UserScore { get; set; }

        /// <summary>
        /// Gets or sets release date of the game. This field gets set using <see cref="ReleaseDateWithoutCorrectYear"/> in a later point in code.
        /// </summary>
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        /// Gets or sets release date with ignoring the year. The reason is that Metacritic doesn't show year in the page.
        /// </summary>
        public DateTime ReleaseDateWithoutCorrectYear { get; set; }
    }
}
