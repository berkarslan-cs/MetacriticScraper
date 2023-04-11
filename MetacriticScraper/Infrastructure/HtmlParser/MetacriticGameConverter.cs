using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using MetacriticScraper.Models;

namespace MetacriticScraper.Infrastructure.HtmlParser
{
    /// <inheritdoc />
    public partial class MetacriticGameConverter : IMetacriticGameConverter
    {
        private const string ToBeDecidedText = "tbd";
        private const string LongMonthReleaseDateFormat = "MMMM d, yyyy";
        private const string MidLengthedMonthReleaseDateFormat = "MMM d, yyyy";
        private static readonly string GameDataNotFoundErrorMessage = $"Game details cannot be got from the html document. The structure of the website might be changed.";
        private static readonly Dictionary<GamePlatform?, IList<string>> PlatformStringMappings = new ()
        {
            {
                GamePlatform.PC,
                new List<string>
                {
                    "PC",
                }
            },
            {
                GamePlatform.PS4,
                new List<string>
                {
                    "PS4",
                    "PlayStation 4",
                }
            },
            {
                GamePlatform.XBoxOne,
                new List<string>
                {
                    "Xbox One",
                }
            },
        };

        /// <inheritdoc />
        public Game ConvertToGameEntity(
            MetacriticGame game,
            bool detailPageValidation = false) => !game.IsValid(detailPageValidation)
                ? throw new Exception(GameDataNotFoundErrorMessage)
                : new Game
                {
                    MetaScore = GetMetascore(game.MetaScore),
                    Name = GetName(game.Name),
                    Platform = GetPlatform(game.Platform),
                    ReleaseDate = GetReleaseDate(game.ReleaseDate),
                    Url = GetUrl(game.Url),
                    UserScore = GetUserScore(game.UserScore),
                    GameDetail = new GameDetail
                    {
                        NumberOfCriticReviews = GetNumberOfCriticReviews(game.NumberOfCriticReviews),
                        NumberOfUserReviews = GetNumberOfUserReviews(game.NumberOfUserReviews),
                    },
                };

        private static string TrimTabNewLineSpaces(string toBeTrimmedString) =>
            toBeTrimmedString.Trim(new char[]
            {
                '\r',
                '\n',
                '\t',
                ' ',
            });

        private static int? GetNumberOfCriticReviews(string numberOfCriticReviews)
        {
            if (string.IsNullOrWhiteSpace(numberOfCriticReviews))
            {
                return null;
            }

            var parsed = int.TryParse(TrimTabNewLineSpaces(numberOfCriticReviews), out var reviewCountInt);
            return !parsed ? throw new Exception(GameDataNotFoundErrorMessage) : reviewCountInt;
        }

        private static int? GetNumberOfUserReviews(string numberOfUserReviews)
        {
            if (string.IsNullOrWhiteSpace(numberOfUserReviews))
            {
                return null;
            }

            var reviewCount = new string(
                TrimTabNewLineSpaces(numberOfUserReviews)
                    .TakeWhile(t => !t.Equals(' '))
                    .ToArray());
            var parsed = int.TryParse(reviewCount, out var reviewCountInt);
            return !parsed ? throw new Exception(GameDataNotFoundErrorMessage) : reviewCountInt;
        }

        private static GamePlatform GetPlatform(string platform)
        {
            var gamePlatform = PlatformStringMappings
                .FirstOrDefault(f => f.Value.Contains(TrimTabNewLineSpaces(platform)))
                .Key;
            return gamePlatform ?? throw new Exception(GameDataNotFoundErrorMessage);
        }

        private static string GetName(string name) => TrimTabNewLineSpaces(name);

        private static decimal? GetUserScore(string userScore)
        {
            if (string.IsNullOrWhiteSpace(userScore) || userScore.Equals(ToBeDecidedText, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }
            else
            {
                var parseSucceeded = decimal.TryParse(userScore, out var convertedUserScore);
                return !parseSucceeded ? throw new ArgumentException($"Unknown {nameof(userScore)}: {userScore}") : convertedUserScore;
            }
        }

        private static int? GetMetascore(string metaScore)
        {
            if (string.IsNullOrWhiteSpace(metaScore) || metaScore.Equals(ToBeDecidedText, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }
            else
            {
                var parseSucceeded = int.TryParse(metaScore, out var convertedMetaScore);
                return !parseSucceeded ? throw new ArgumentException($"Unknown {nameof(metaScore)}: {metaScore}") : convertedMetaScore;
            }
        }

        private static string GetUrl(string relativeUrl)
        {
            var uri = new Uri(new Uri(CommonConstants.MetacriticSite), relativeUrl);
            return uri.AbsoluteUri;
        }

        private static DateTime GetReleaseDate(string releaseDate)
        {
            // Trim the release date in a way that multiple spaces in the middle of the release date will get replaced with one.
            var pattern = RegexPatternForMultipleSpaces();
            releaseDate = pattern.Replace(releaseDate.Trim(), " ");

            var parseSucceeded = DateTime.TryParseExact(
                releaseDate,
                LongMonthReleaseDateFormat,
                CultureInfo.CurrentCulture,
                DateTimeStyles.None,
                out var convertedReleaseDate) ||
                DateTime.TryParseExact(
                releaseDate,
                MidLengthedMonthReleaseDateFormat,
                CultureInfo.CurrentCulture,
                DateTimeStyles.None,
                out convertedReleaseDate);
            return !parseSucceeded
                ? throw new ArgumentException($"Release date couldn't be parsed {nameof(releaseDate)}: {releaseDate}")
                : convertedReleaseDate;
        }

        [GeneratedRegex("[ ]{2,}")]
        private static partial Regex RegexPatternForMultipleSpaces();
    }
}
