namespace RatingsAnalyzer.Crawler
{
    public interface IDownloader
    {
        string Get(string uri);
    }
}
