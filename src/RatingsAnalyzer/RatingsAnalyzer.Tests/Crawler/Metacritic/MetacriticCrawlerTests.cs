using System.IO;
using System.Reflection;
using Moq;
using RatingsAnalyzer.Crawler;
using RatingsAnalyzer.Crawler.Metacritic;

namespace RatingsAnalyzer.Tests.Crawler.Metacritic
{
    public class MetacriticCrawlerTests
    {
        private const string HtmlResourceName = "RatingsAnalyzer.Tests.Crawler.Metacritic.MetacriticMovieList.html";

        public MetacriticCrawlerTests()
        {
            string html;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(HtmlResourceName))
            using (var reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            var downloaderMock = new Mock<IDownloader>();
            downloaderMock.Setup(x => x.Get("http://www.metacritic.com/browse/movies/title/dvd")).Returns(html);

            var crawler = new MetacriticCrawler(() => downloaderMock.Object);
        }
    }
}
