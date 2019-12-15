using System;
using HtmlAgilityPack;

namespace MetacriticScraper.Infrastructure.SiteResolver
{
    /// <inheritdoc />
    public class HtmlWebWrapper : IHtmlWebWrapper
    {
        private readonly HtmlWeb htmlWeb;

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlWebWrapper"/> class.
        /// </summary>
        /// <param name="htmlWeb">Html web third party instance.</param>
        public HtmlWebWrapper(HtmlWeb htmlWeb) => this.htmlWeb = htmlWeb;

        /// <inheritdoc />
        public HtmlDocument Load(Uri uri) => htmlWeb.Load(uri);
    }
}
