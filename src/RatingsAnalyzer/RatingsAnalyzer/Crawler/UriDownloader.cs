using System.Net;
using NLog;

namespace RatingsAnalyzer.Crawler
{
    class UriDownloader: IDownloader
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public string Get(string uri)
        {
            Logger.Debug("Downloading {0}", uri);

            var webClient = new WebClient();
            // TODO: set headers
            return webClient.DownloadString(uri);
        }
    }
}
