using System;
using System.Runtime.Serialization;

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
        [IgnoreDataMember]
        public GamePlatform Platform { get; set; }

        /// <summary>
        /// Gets or sets url of the game.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets Metacritic score of the game.
        /// </summary>
        public int? MetaScore { get; set; }

        /// <summary>
        /// Gets or sets user score of the game.
        /// </summary>
        public decimal? UserScore { get; set; }

        /// <summary>
        /// Gets or sets release date of the game.
        /// </summary>
        public DateTime ReleaseDate { get; set; }
    }
}
