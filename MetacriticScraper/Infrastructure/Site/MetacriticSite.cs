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
                var htmlDocument = siteResolver.GetHtmlDocument(gameFilter.Platform, pageIndex);
                var gamesInPage = htmlParser.GetGames(htmlDocument, gameFilter.Platform);

                // Fix year of ReleaseDate property (Metacritic doesn't include year in the page so that we need to evaluate it)
                FixReleaseDate(gamesInPage);

                var filteredGames = GetFilteredGames(gameFilter, gamesInPage);
                result.AddRange(filteredGames);

                // Continue processing until we have a game which has a release date less than the given MinReleaseDate filter
                if (gamesInPage.Any(a => a.ReleaseDate < gameFilter.MinReleaseDate))
                {
                    break;
                }
            }

            return result;
        }

        /// <inheritdoc />
        public int GetNumberOfPages(GamePlatform gamePlatform)
        {
            // Get the first page and scrape the last page id
            var htmlDocument = siteResolver.GetHtmlDocument(gamePlatform, 0);
            return htmlParser.GetNumberOfPages(htmlDocument);
        }

        private static void FixReleaseDate(IList<Game> result)
        {
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;
            foreach (var game in result)
            {
                // The following means that the month is switched from January to December which means that we're in the previous year.
                if (game.ReleaseDateWithoutCorrectYear.Month > currentMonth)
                {
                    currentYear--;
                }

                currentMonth = game.ReleaseDateWithoutCorrectYear.Month;
                game.ReleaseDate = new DateTime(
                    currentYear,
                    game.ReleaseDateWithoutCorrectYear.Month,
                    game.ReleaseDateWithoutCorrectYear.Day);
            }
        }

        private static IEnumerable<Game> GetFilteredGames(
            GameFilter gameFilter,
            IList<Game> gamesInPage) =>
                gamesInPage.Where(
                    w => gameFilter.Platform == w.Platform &&
                    (gameFilter.MinMetaScore == null || w.MetaScore >= gameFilter.MinMetaScore) &&
                    (gameFilter.MinReleaseDate == null || w.ReleaseDate >= gameFilter.MinReleaseDate) &&
                    (gameFilter.MinUserScore == null || w.UserScore >= gameFilter.MinUserScore));
    }
}
