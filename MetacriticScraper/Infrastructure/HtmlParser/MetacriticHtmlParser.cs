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
        // Game List selectors
        private const string GameListElementSelector = "//tr[@class='expand_collapse']";
        private const string GameListPlatformXPathSelector = "//div[contains(@class, 'platform')]/button";
        private const string GameListNameXPathSelector = ".//td[@class='details']/a/h3";
        private const string GameListUrlXPathSelector = ".//td[@class='details']/a";
        private const string GameListMetaScoreXPathSelector = ".//td[@class='score']/a/div";
        private const string GameListUserScoreXPathSelector = ".//td[@class='details']//div[contains(@class, 'metascore_w')]";
        private const string GameListReleaseDateXPathSelector = ".//td[@class='details']/span";
        private const string GameListLastPageXPathSelector = "//*[contains(@class, 'last_page')]/*[contains(@class, 'page_num')]";

        // Game Detail selectors
        private const string GameDetailPlatformXPathSelector = "//span[@class='platform']";
        private const string GameDetailNameXPathSelector = "//div[@class='product_title']/a/h1";
        private const string GameDetailUrlXPathSelector = "//div[@class='product_title']/a";
        private const string GameDetailMetaScoreXPathSelector = "//div[contains(@class, 'main_details')]//a[@class='metascore_anchor']//div[contains(@class, 'metascore_w')]";
        private const string GameDetailUserScoreXPathSelector = "//div[contains(@class, 'side_details')]//a[@class='metascore_anchor']//div[contains(@class, 'metascore_w')]";
        private const string GameDetailReleaseDateXPathSelector = "//li[contains(@class,'release_data')]/span[@class='data']";
        private const string GameDetailNumberOfCriticReviews = "//div[contains(@class, 'main_details')]//span[@class='count']/a/span";
        private const string GameDetailNumberOfUserReviews = "//div[contains(@class, 'side_details')]/div[@class='score_summary']//span[@class='count']/a";

        private const string StructureOfWebsiteMightBeChangedErrorMessage = "The structure of the website might be changed.";
        private const string AnchorLinkHrefAttributeName = "href";
        private static readonly string GamesNotFoundErrorMessage = $"Game list elements are not found in the html document. {StructureOfWebsiteMightBeChangedErrorMessage}";
        private static readonly string LastPageNotFoundErrorMessage = $"Last page not found in the html document. {StructureOfWebsiteMightBeChangedErrorMessage}";
        private readonly IMetacriticGameConverter metacriticGameConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetacriticHtmlParser"/> class.
        /// </summary>
        /// <param name="metacriticGameConverter">Converter that acts like a mapper.</param>
        public MetacriticHtmlParser(IMetacriticGameConverter metacriticGameConverter) => this.metacriticGameConverter = metacriticGameConverter;

        /// <inheritdoc/>
        public IList<Game> GetGames(IXPathNavigable xPathNavigableHtmlDocument)
        {
            if (xPathNavigableHtmlDocument is not HtmlDocument htmlDocument)
            {
                throw new ArgumentNullException(nameof(xPathNavigableHtmlDocument), $"Parameter should be non-empty {typeof(HtmlDocument).Name}");
            }

            var gameListElements = GetGameListElements(htmlDocument);
            var result = new List<Game>();
            foreach (var gameElement in gameListElements)
            {
                result.Add(GetGameFromListPage(gameElement));
            }

            return result;
        }

        /// <inheritdoc/>
        public int GetNumberOfPages(IXPathNavigable xPathNavigableHtmlDocument)
        {
            if (xPathNavigableHtmlDocument is not HtmlDocument htmlDocument)
            {
                throw new ArgumentNullException(nameof(xPathNavigableHtmlDocument), $"Parameter should be non-empty {typeof(HtmlDocument).Name}");
            }

            var lastPageText = htmlDocument.DocumentNode.SelectSingleNode(GameListLastPageXPathSelector)?.InnerText;
            var parseSuccessful = int.TryParse(lastPageText, out var lastPage);
            return !parseSuccessful ? throw new Exception(LastPageNotFoundErrorMessage) : lastPage;
        }

        /// <inheritdoc/>
        public Game GetGameDetails(IXPathNavigable xPathNavigableHtmlDocument) =>
            xPathNavigableHtmlDocument is not HtmlDocument htmlDocument
                ? throw new ArgumentNullException(nameof(xPathNavigableHtmlDocument), $"Parameter should be non-empty {typeof(HtmlDocument).Name}")
                : GetGameFromDetailPage(htmlDocument);

        private static HtmlNodeCollection GetGameListElements(HtmlDocument htmlDocument)
        {
            var gameListElements = htmlDocument.DocumentNode.SelectNodes(GameListElementSelector);
            return gameListElements ?? throw new Exception(GamesNotFoundErrorMessage);
        }

        private Game GetGameFromListPage(HtmlNode gameElement)
        {
            var metacriticGame = new MetacriticGameListPage
            {
                MetaScore = gameElement.SelectSingleNode(GameListMetaScoreXPathSelector)?.InnerText,
                Name = gameElement.SelectSingleNode(GameListNameXPathSelector)?.InnerText,
                Platform = gameElement.SelectSingleNode(GameListPlatformXPathSelector)?.InnerText,
                ReleaseDate = gameElement.SelectSingleNode(GameListReleaseDateXPathSelector)?.InnerText,
                Url = gameElement.SelectSingleNode(GameListUrlXPathSelector)?.GetAttributeValue(AnchorLinkHrefAttributeName, null),
                UserScore = gameElement.SelectSingleNode(GameListUserScoreXPathSelector)?.InnerText,
            };

            return metacriticGameConverter.ConvertToGameEntity(metacriticGame);
        }

        private Game GetGameFromDetailPage(HtmlDocument htmlDocument)
        {
            var metacriticGame = new MetacriticGameDetailPage
            {
                MetaScore = htmlDocument.DocumentNode.SelectSingleNode(GameDetailMetaScoreXPathSelector)?.InnerText,
                Name = htmlDocument.DocumentNode.SelectSingleNode(GameDetailNameXPathSelector)?.InnerText,
                Platform = htmlDocument.DocumentNode.SelectSingleNode(GameDetailPlatformXPathSelector)?.InnerText,
                ReleaseDate = htmlDocument.DocumentNode.SelectSingleNode(GameDetailReleaseDateXPathSelector)?.InnerText,
                Url = htmlDocument.DocumentNode.SelectSingleNode(GameDetailUrlXPathSelector)?.GetAttributeValue(AnchorLinkHrefAttributeName, null),
                UserScore = htmlDocument.DocumentNode.SelectSingleNode(GameDetailUserScoreXPathSelector)?.InnerText,
                NumberOfCriticReviews = htmlDocument.DocumentNode.SelectSingleNode(GameDetailNumberOfCriticReviews)?.InnerText,
                NumberOfUserReviews = htmlDocument.DocumentNode.SelectSingleNode(GameDetailNumberOfUserReviews)?.InnerText,
            };

            return metacriticGameConverter.ConvertToGameEntity(metacriticGame);
        }
    }
}
