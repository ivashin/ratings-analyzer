using System.Net;
using NLog;

namespace RatingsAnalyzer.Crawler
{
    class WebPageDownloader: IPageDownloader
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public string Get(string uri)
        {
            Logger.Debug("Downloading {0}", uri);

            var webClient = new WebClient();
            webClient.Headers.Add("user-agent", "RatingsAnalyzer");
            return webClient.DownloadString(uri);
        }
    }
}
