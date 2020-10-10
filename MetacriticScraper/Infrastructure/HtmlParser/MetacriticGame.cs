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
        public string Platform { get; set; }

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
        /// Gets or sets the number of critic reviews.
        /// </summary>
        public string NumberOfCriticReviews { get; set; }

        /// <summary>
        /// Gets or sets the number of user reviews.
        /// </summary>
        public string NumberOfUserReviews { get; set; }

        /// <summary>
        /// Validates the game properties.
        /// </summary>
        /// <param name="detailPageValidation">Flag for validating the game entity in detail.</param>
        /// <returns>True if valid; false in otherwise.</returns>
        public bool IsValid(bool detailPageValidation = false)
        {
            // NumberOfUserReviews can be null in case the page includes "Please spend some time playing the game." kind of text, therefore, the following ignores it.
            if (Name == null ||
                Platform == null ||
                Url == null ||
                (!detailPageValidation && MetaScore == null) ||
                (!detailPageValidation && UserScore == null) ||
                ReleaseDate == null ||
                (detailPageValidation && NumberOfCriticReviews == null))
            {
                return false;
            }

            return true;
        }
    }
}
