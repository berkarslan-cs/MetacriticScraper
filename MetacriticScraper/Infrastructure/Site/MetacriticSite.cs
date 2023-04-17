using System;
using System.Collections.Generic;
using System.Linq;
using MetacriticScraper.Infrastructure.HtmlParser;
using MetacriticScraper.Infrastructure.SiteResolver;
using MetacriticScraper.Models;

namespace MetacriticScraper.Infrastructure.Site
{
    /// <summary>
    /// Manages the Metacritic games using cache.
    /// </summary>
    public class MetacriticSite : IMetacriticSite
    {
        private readonly ISiteResolver siteResolver;
        private readonly IHtmlParser htmlParser;
        private readonly IDictionary<GamePlatform, int> cachedNumberOfPages = new Dictionary<GamePlatform, int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MetacriticSite"/> class.
        /// </summary>
        /// <param name="siteResolver">Site resolver.</param>
        /// <param name="htmlParser">Html parser.</param>
        public MetacriticSite(
            ISiteResolver siteResolver,
            IHtmlParser htmlParser)
        {
            this.siteResolver = siteResolver;
            this.htmlParser = htmlParser;
        }

        /// <inheritdoc />
        public IList<Game> GetGames(GameFilter gameFilter)
        {
            if (gameFilter == null)
            {
                throw new ArgumentNullException(nameof(gameFilter));
            }

            var result = new List<Game>();
            for (var pageIndex = 0; pageIndex < GetNumberOfPages(gameFilter.Platform); pageIndex++)
            {
                var htmlDocument = siteResolver.GetMetacriticGameListHtmlDocument(gameFilter.Platform, pageIndex);
                var gamesInPage = htmlParser.GetGames(htmlDocument);
                var filteredGames = GetFilteredGames(gameFilter, gamesInPage);
                result.AddRange(filteredGames);

                // Continue processing until we have a game which has a release date less than the given MinReleaseDate filter
                if (gamesInPage.Any(a => a.ReleaseDate < gameFilter.MinReleaseDate))
                {
                    break;
                }
            }

            // Get each game's details (Metacritic count etc.)
            for (var index = 0; index < result.Count; index++)
            {
                var game = result[index];
                var htmlDocument = siteResolver.GetGameHtmlDocument(game.Url);
                var gameWithDetails = htmlParser.GetGameDetails(htmlDocument);
                result[index] = gameWithDetails;
            }

            // Apply advanced filtering
            result = GetDetailedFilteredGames(gameFilter, result).ToList();
            return result;
        }

        /// <inheritdoc />
        public int GetNumberOfPages(GamePlatform gamePlatform)
        {
            if (!cachedNumberOfPages.ContainsKey(gamePlatform))
            {
                // Get the first page and scrape the last page id
                var htmlDocument = siteResolver.GetMetacriticGameListHtmlDocument(gamePlatform, 0);
                var numberOfPages = htmlParser.GetNumberOfPages(htmlDocument);
                cachedNumberOfPages[gamePlatform] = numberOfPages;
            }

            return cachedNumberOfPages[gamePlatform];
        }

        private static IEnumerable<Game> GetFilteredGames(
            GameFilter gameFilter,
            IList<Game> gamesInPage) =>
                gamesInPage.Where(
                    w => gameFilter.Platform == w.Platform &&
                    (gameFilter.MinMetaScore == null || gameFilter.MinMetaScore == 0 || w.MetaScore >= gameFilter.MinMetaScore) &&
                    (gameFilter.MinReleaseDate == null || w.ReleaseDate >= gameFilter.MinReleaseDate) &&
                    (gameFilter.MinUserScore == null || gameFilter.MinUserScore == 0 || w.UserScore >= gameFilter.MinUserScore));

        private static IEnumerable<Game> GetDetailedFilteredGames(
            GameFilter gameFilter,
            IList<Game> gamesInPage) =>
                GetFilteredGames(gameFilter, gamesInPage)
                    .Where(
                        w => (gameFilter.MinNumberOfCriticReviews == null ||
                            gameFilter.MinNumberOfCriticReviews == 0 ||
                            w.NumberOfCriticReviews >= gameFilter.MinNumberOfCriticReviews) &&
                        (gameFilter.MinNumberOfUserReviews == null ||
                            gameFilter.MinNumberOfUserReviews == 0 ||
                            w.NumberOfUserReviews >= gameFilter.MinNumberOfUserReviews));
    }
}
