using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

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

        private readonly IDownloader _downloader;
        private readonly Func<string, IEntryParser> _parserFactory;

        public MetacriticCrawler(IDownloader downloader, Func<string, IEntryParser> parserFactory)
        {
            _downloader = downloader;
            _parserFactory = parserFactory;
        }

        public IEnumerable<IEntryParser> GetEntries()
        {
            var uriBuilder = new UriBuilder(BaseUri);

            var uri = EntryPointUri;
            while (uri != null)
            {
                var html = _downloader.Get(uri);
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                var rootNode = doc.DocumentNode;

                // Lazily iterate over all movie entries on the page
                var moviesList = rootNode.SelectNodes(MovieEntryXPath);
                foreach (var entry in moviesList)
                {
                    uriBuilder.Path = entry.Attributes[HrefAttribute].Value;
                    yield return _parserFactory(uriBuilder.Uri.ToString());
                }

                uri = GetNextPageUri(rootNode);
            }
        }

        private string GetNextPageUri(HtmlNode rootNode)
        {
            var uriBuilder = new UriBuilder(BaseUri);

            // Iterate over pages for a single letter
            var numberPages = rootNode.SelectNodes(NumberPagesXPath);
            var activePage = numberPages.First(page => page.Attributes[ClassAttribute].Value.Contains("active_page"));
            var index = numberPages.IndexOf(activePage);
            if (index != numberPages.Count - 1)
            {
                var nextPage = numberPages[index + 1];
                uriBuilder.Path = nextPage.SelectSingleNode("./a").Attributes[HrefAttribute].Value;
                return uriBuilder.Uri.ToString();
            }

            // If last page for a letter, then iterate over letters
            var letterPages = rootNode.SelectNodes(LetterPagesXPath);
            activePage = letterPages.First(page => page.SelectSingleNode(ActiveLetterPageXPath) != null);
            index = letterPages.IndexOf(activePage);
            if (index != letterPages.Count - 1)
            {
                var nextPage = letterPages[index + 1];
                uriBuilder.Path = nextPage.SelectSingleNode("./a").Attributes[HrefAttribute].Value;
                return uriBuilder.Uri.ToString();
            }

            return null;
        }
    }
}
