using System;
using System.Collections.Generic;
using System.Xml.XPath;
using HtmlAgilityPack;
using MetacriticScraper.Models;

namespace MetacriticScraper.Infrastructure.HtmlParser
{
    /// <summary>
    /// Parses the html document using the HTML Agility library.
    /// </summary>
    public class MetacriticHtmlParser : IHtmlParser
    {
        private const string GameListElementSelector = "//*[contains(@class, 'list_products')]/*[contains(@class, 'game_product')]";
        private const string NameXPathSelector = "//*[contains(@class, 'product_title')]/a]";
        private const string MetaScoreXPathSelector = "//*[contains(@class, 'product_score')]/*[contains(@class, 'metascore_w')]";
        private const string UserScoreXPathSelector = "//*[contains(@class, 'product_avguserscore')]/*[contains(@class, 'textscore')]";
        private const string ReleaseDateXPathSelector = "//*[contains(@class, 'release_date')]/*[contains(@class, 'data')]";
        private const string LastPageXPathSelector = "//*[contains(@class, 'last_page')]/*[contains(@class, 'page_num')]";
        private const string StructureOfWebsiteMightBeChangedErrorMessage = "The structure of the website might be changed.";
        private const string AnchorLinkHrefAttributeName = "href";
        private static readonly string GamesNotFoundErrorMessage = $"Game list elements are not found in the html document. {StructureOfWebsiteMightBeChangedErrorMessage}";
        private static readonly string LastPageNotFoundErrorMessage = $"Last page not found in the html document. {StructureOfWebsiteMightBeChangedErrorMessage}";
        private readonly IMetacriticGameConverter metacriticGameConverter;
        private readonly GamePlatform platform;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetacriticHtmlParser"/> class.
        /// </summary>
        /// <param name="metacriticGameConverter">Converter that acts like a mapper.</param>
        /// <param name="platform">Game platform.</param>
        public MetacriticHtmlParser(
            IMetacriticGameConverter metacriticGameConverter,
            GamePlatform platform)
        {
            this.metacriticGameConverter = metacriticGameConverter;
            this.platform = platform;
        }

        /// <inheritdoc/>
        public IList<Game> GetGames(IXPathNavigable xPathNavigableHtmlDocument)
        {
            if (!(xPathNavigableHtmlDocument is HtmlDocument htmlDocument))
            {
                throw new ArgumentNullException(nameof(xPathNavigableHtmlDocument), $"Parameter should be non-empty {typeof(HtmlDocument).Name}");
            }

            var gameListElements = GetGameListElements(htmlDocument);
            var result = new List<Game>();
            foreach (var gameElement in gameListElements)
            {
                result.Add(GetGame(gameElement));
            }

            return result;
        }

        /// <inheritdoc/>
        public int GetNumberOfPages(IXPathNavigable xPathNavigableHtmlDocument)
        {
            if (!(xPathNavigableHtmlDocument is HtmlDocument htmlDocument))
            {
                throw new ArgumentNullException(nameof(xPathNavigableHtmlDocument), $"Parameter should be non-empty {typeof(HtmlDocument).Name}");
            }

            var lastPageText = htmlDocument.DocumentNode.SelectSingleNode(LastPageXPathSelector)?.InnerText;
            var parseSuccessful = int.TryParse(lastPageText, out var lastPage);
            if (!parseSuccessful)
            {
                throw new Exception(LastPageNotFoundErrorMessage);
            }

            return lastPage;
        }

        private static HtmlNodeCollection GetGameListElements(HtmlDocument htmlDocument)
        {
            var gameListElements = htmlDocument.DocumentNode.SelectNodes(GameListElementSelector);
            if (gameListElements == null)
            {
                throw new Exception(GamesNotFoundErrorMessage);
            }

            return gameListElements;
        }

        private Game GetGame(HtmlNode gameElement)
        {
            var metacriticGame = new MetacriticGame()
            {
                MetaScore = gameElement.SelectSingleNode(MetaScoreXPathSelector)?.InnerText,
                Name = gameElement.SelectSingleNode(NameXPathSelector)?.InnerText,
                Platform = platform,
                ReleaseDate = gameElement.SelectSingleNode(ReleaseDateXPathSelector)?.InnerText,
                Url = gameElement.SelectSingleNode(NameXPathSelector)?.GetAttributeValue(AnchorLinkHrefAttributeName, null),
                UserScore = gameElement.SelectSingleNode(UserScoreXPathSelector)?.InnerText,
            };

            return metacriticGameConverter.ConvertToGameEntity(metacriticGame);
        }
    }
}
