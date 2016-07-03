using System;
using System.Collections.Generic;

namespace RatingsAnalyzer.Crawler.Metacritic
{
    public class MetacriticCrawler: ICrawler
    {
        private const string EntryPointUri = "http://www.metacritic.com/browse/movies/title/dvd";

        private readonly Func<IDownloader> _downloaderFactory;

        public MetacriticCrawler(Func<IDownloader> downloaderFactory)
        {
            _downloaderFactory = downloaderFactory;
        }

        public IEnumerable<IEntryParser> GetEntries()
        {
            yield return new MetacriticEntryParser(_downloaderFactory, "");
        }
    }
}
