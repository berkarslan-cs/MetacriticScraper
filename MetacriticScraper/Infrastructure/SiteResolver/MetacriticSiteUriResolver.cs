using System;
using System.Collections.Generic;
using MetacriticScraper.Models;

namespace MetacriticScraper.Infrastructure.SiteResolver
{
    /// <inheritdoc />
    public class MetacriticSiteUriResolver : ISiteUriResolver
    {
        private static readonly string MetacriticPageUrlFormat = @"/browse/games/release-date/available/{0}/date?page={1}";
        private static readonly Dictionary<GamePlatform, string> GamePlatformAndUrlMapping = new Dictionary<GamePlatform, string>
        {
            { GamePlatform.PC, "pc" },
            { GamePlatform.PS4, "ps4" },
            { GamePlatform.XBoxOne, "xboxone" },
        };

        /// <inheritdoc />
        public Uri GetSiteUrl(GamePlatform gamePlatform, int pageId)
        {
            if (!GamePlatformAndUrlMapping.TryGetValue(gamePlatform, out var gamePlatformParam))
            {
                throw new ArgumentException("Game platform is not supported yet.", nameof(gamePlatform));
            }

            return new Uri(
                new Uri(CommonConstants.MetacriticSite),
                string.Format(MetacriticPageUrlFormat, gamePlatformParam, pageId));
        }
    }
}
