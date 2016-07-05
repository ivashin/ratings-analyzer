using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using NLog;

namespace RatingsAnalyzer.Crawler.Metacritic
{
    public class MetacriticCrawler: ICrawler
    {
        private const string BaseUri = "http://www.metacritic.com";
        private const string EntryPointUri = "http://www.metacritic.com/browse/movies/title/dvd";

        private const string NumberPagesXPath = @"//div[@class='page_nav']/div[@class='page_nav_wrap']/div[@class='pages']/ul[@class='pages']/li";
        private const string LetterPagesXPath = @"//div[@class='letternav']/ul[@class='letternav']/li";
        private const string MovieEntryXPath = @"//ol[@class='list_products list_product_condensed']/li/div[@class='product_wrap']/div[@class='basic_stat product_title']/a";
        private const string ActiveLetterPageXPath = @".//span[@class='active']";

        private const string HrefAttribute = "href";
        private const string ClassAttribute = "class";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IPageDownloader _pageDownloader;
        private readonly Func<string, IEntryParser> _parserFactory;

        public MetacriticCrawler(IPageDownloader pageDownloader, Func<string, IEntryParser> parserFactory)
        {
            _pageDownloader = pageDownloader;
            _parserFactory = parserFactory;
        }

        public IEnumerable<IEntryParser> GetEntries()
        {
            var uri = EntryPointUri;
            while (uri != null)
            {
                Logger.Info("Processing page: {0}", uri);
                var html = _pageDownloader.Get(uri);
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                var rootNode = doc.DocumentNode;

                // Lazily iterate over all movie entries on the page
                var moviesList = rootNode.SelectNodes(MovieEntryXPath);
                Logger.Debug("Found {0} entries in {1}", moviesList.Count, uri);
                foreach (var entry in moviesList)
                {
                    var movieLink = entry.Attributes[HrefAttribute].Value;
                    yield return _parserFactory(BaseUri + movieLink);
                }

                uri = GetNextPageUri(rootNode);
            }
        }

        private string GetNextPageUri(HtmlNode rootNode)
        {
            // Iterate over pages for a single letter. If final page - then iterate over letters.
            return GetNextPageUri(rootNode, NumberPagesXPath,
                                  page => page.Attributes[ClassAttribute].Value.Contains("active_page"))
                ?? GetNextPageUri(rootNode, LetterPagesXPath,
                                  page => page.SelectSingleNode(ActiveLetterPageXPath) != null);
        }

        private string GetNextPageUri(HtmlNode rootNode, string xpath, Func<HtmlNode, bool> activePageSelector)
        {
            var numberPages = rootNode.SelectNodes(xpath);
            var activePage = numberPages.First(activePageSelector);
            var index = numberPages.IndexOf(activePage);
            if (index != numberPages.Count - 1)
            {
                var nextPage = numberPages[index + 1];
                var nextPageLink = nextPage.SelectSingleNode("./a").Attributes[HrefAttribute].Value;
                return BaseUri + nextPageLink;
            }
            return null;
        }
    }
}
