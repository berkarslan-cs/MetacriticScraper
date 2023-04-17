namespace MetacriticScraper.Infrastructure.HtmlParser
{
    /// <summary>
    /// Base properties of a game in the HTML pages.
    /// </summary>
    public abstract class MetacriticGameBase
    {
        /// <summary>
        /// Gets or sets name of the game.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets platform of the game.
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// Gets or sets url of the game.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets release date of the game.
        /// </summary>
        public string ReleaseDate { get; set; }

        /// <summary>
        /// Validates the game properties.
        /// </summary>
        /// <returns>True if valid; false, otherwise.</returns>
        public virtual bool IsValid() =>
            Name != null &&
            Platform != null &&
            Url != null &&
            ReleaseDate != null;
    }
}
