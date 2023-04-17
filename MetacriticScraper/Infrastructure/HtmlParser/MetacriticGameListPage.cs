namespace MetacriticScraper.Infrastructure.HtmlParser
{
    /// <summary>
    /// Original text values of Metacritic game elements in the list page.
    /// </summary>
    public class MetacriticGameListPage : MetacriticGamePageBase
    {
        /// <summary>
        /// Gets or sets Metacritic score of the game.
        /// </summary>
        public string MetaScore { get; set; }

        /// <summary>
        /// Gets or sets user score of the game.
        /// </summary>
        public string UserScore { get; set; }

        /// <inheritdoc />
        public override bool IsValid() =>
            base.IsValid() &&
            MetaScore != null &&
            UserScore != null;
    }
}
