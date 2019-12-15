using System;
using System.Globalization;
using MetacriticScraper.Models;

namespace MetacriticScraper.Infrastructure.HtmlParser
{
    /// <inheritdoc />
    public class MetacriticGameConverter : IMetacriticGameConverter
    {
        private const string ToBeDecidedText = "tbd";
        private const string DefaultReleaseDateFormat = "yyyy MMMM dd";
        private static readonly string GameDataNotFoundErrorMessage = $"Game details cannot be got from the html document. The structure of the website might be changed.";

        /// <inheritdoc />
        public Game ConvertToGameEntity(MetacriticGame game)
        {
            if (!game.IsValid())
            {
                throw new Exception(GameDataNotFoundErrorMessage);
            }

            return new Game()
            {
                MetaScore = GetMetascore(game.MetaScore),
                Name = game.Name,
                Platform = game.Platform,
                ReleaseDateWithoutCorrectYear = GetReleaseDateWithoutCorrectYear(game.ReleaseDate),
                Url = GetUrl(game.Url),
                UserScore = GetUserScore(game.UserScore),
            };
        }

        private static decimal? GetUserScore(string userScore)
        {
            if (userScore.Equals(ToBeDecidedText, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }
            else
            {
                var parseSucceeded = decimal.TryParse(userScore, out var convertedUserScore);
                if (!parseSucceeded)
                {
                    throw new ArgumentException($"Unknown {nameof(userScore)}: {userScore}");
                }

                return convertedUserScore;
            }
        }

        private static decimal? GetMetascore(string metaScore)
        {
            if (metaScore.Equals(ToBeDecidedText, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }
            else
            {
                var parseSucceeded = decimal.TryParse(metaScore, out var convertedMetaScore);
                if (!parseSucceeded)
                {
                    throw new ArgumentException($"Unknown {nameof(metaScore)}: {metaScore}");
                }

                return convertedMetaScore;
            }
        }

        private static string GetUrl(string relativeUrl)
        {
            var uri = new Uri(new Uri(CommonConstants.MetacriticSite), relativeUrl);
            return uri.AbsolutePath;
        }

        private static DateTime GetReleaseDateWithoutCorrectYear(string releaseDate)
        {
            var releaseDateWithYear = $"{DateTime.Now.Year} {releaseDate}";
            var parseSucceeded = DateTime.TryParseExact(
                releaseDateWithYear,
                DefaultReleaseDateFormat,
                CultureInfo.CurrentCulture,
                DateTimeStyles.None,
                out var convertedReleaseDate);
            if (!parseSucceeded)
            {
                throw new ArgumentException($"Release date couldn't be parsed {nameof(releaseDateWithYear)}: {releaseDateWithYear}");
            }

            return convertedReleaseDate;
        }
    }
}
