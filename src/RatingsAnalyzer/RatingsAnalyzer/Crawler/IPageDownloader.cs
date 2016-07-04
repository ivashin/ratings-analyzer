namespace RatingsAnalyzer.Crawler
{
    public interface IPageDownloader
    {
        string Get(string uri);
    }
}
