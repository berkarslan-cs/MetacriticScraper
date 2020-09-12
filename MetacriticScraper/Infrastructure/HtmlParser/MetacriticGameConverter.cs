using System;
using System.Globalization;
using MetacriticScraper.Models;

namespace MetacriticScraper.Infrastructure.HtmlParser
{
    /// <inheritdoc />
    public class MetacriticGameConverter : IMetacriticGameConverter
    {
        private const string ToBeDecidedText = "tbd";
        private const string DefaultReleaseDateFormat = "MMMM d, yyyy";
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
                Name = GetName(game.Name),
                Platform = game.Platform,
                ReleaseDate = GetReleaseDate(game.ReleaseDate),
                Url = GetUrl(game.Url),
                UserScore = GetUserScore(game.UserScore),
            };
        }

        private static string GetName(string name) =>
            name.Trim(new char[]
            {
                '\r',
                '\n',
                '\t',
                ' ',
            });

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

        private static int? GetMetascore(string metaScore)
        {
            if (metaScore.Equals(ToBeDecidedText, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }
            else
            {
                var parseSucceeded = int.TryParse(metaScore, out var convertedMetaScore);
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
            return uri.AbsoluteUri;
        }

        private static DateTime GetReleaseDate(string releaseDate)
        {
            var parseSucceeded = DateTime.TryParseExact(
                releaseDate.Trim(),
                DefaultReleaseDateFormat,
                CultureInfo.CurrentCulture,
                DateTimeStyles.None,
                out var convertedReleaseDate);
            if (!parseSucceeded)
            {
                throw new ArgumentException($"Release date couldn't be parsed {nameof(releaseDate)}: {releaseDate}");
            }

            return convertedReleaseDate;
        }
    }
}
