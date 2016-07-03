using System.Collections.Generic;

namespace RatingsAnalyzer.Crawler
{
    public interface ICrawler
    {
        IEnumerable<IEntryParser> GetEntries();
    }
}
