using System;
using System.ComponentModel.DataAnnotations;

namespace MetacriticScraper.Models
{
    /// <summary>
    /// Filter model to apply filtering on the Metacritic games.
    /// </summary>
    public class GameFilter
    {
        /// <summary>
        /// Gets or sets the platform filter.
        /// </summary>
        [Required]
        public GamePlatform Platform { get; set; }

        /// <summary>
        /// Gets or sets the platform filter.
        /// </summary>
        [Range(0, 100)]
        public decimal? MinMetaScore { get; set; }

        /// <summary>
        /// Gets or sets the platform filter.
        /// </summary>
        [Range(0, 100)]
        public decimal? MinUserScore { get; set; }

        /// <summary>
        /// Gets or sets the platform filter.
        /// </summary>
        public DateTime? MinReleaseDate { get; set; }
    }
}
