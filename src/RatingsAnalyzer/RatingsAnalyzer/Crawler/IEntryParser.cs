using RatingsAnalyzer.Model;

namespace RatingsAnalyzer.Crawler
{
    public interface IEntryParser
    {
        MovieData Parse();
    }
}
