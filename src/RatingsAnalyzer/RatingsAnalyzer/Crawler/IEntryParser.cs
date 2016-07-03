using RatingsAnalyzer.Model;

namespace RatingsAnalyzer.Crawler
{
    public interface IEntryParser
    {
        string Uri { get; }
        MovieData Parse();
    }
}
