namespace MetacriticScraper.Infrastructure.HtmlParser
{
    /// <summary>
    /// Original text values of Metacritic game elements in the detail page of the game.
    /// </summary>
    public class MetacriticGameDetailPage : MetacriticGamePageBase
    {
        /// <summary>
        /// Gets or sets Metacritic score of the game.
        /// </summary>
        public string MetaScore { get; set; }

        /// <summary>
        /// Gets or sets user score of the game.
        /// </summary>
        public string UserScore { get; set; }

        /// <summary>
        /// Gets or sets the number of critic reviews.
        /// </summary>
        public string NumberOfCriticReviews { get; set; }

        /// <summary>
        /// Gets or sets the number of user reviews.
        /// </summary>
        public string NumberOfUserReviews { get; set; }

        /// <inheritdoc />
        public override bool IsValid() =>

            // NumberOfUserReviews can be null in case the page includes "Please spend some time playing the game." kind of text, therefore, the following ignores it.
            base.IsValid() &&
            NumberOfCriticReviews != null;
    }
}
