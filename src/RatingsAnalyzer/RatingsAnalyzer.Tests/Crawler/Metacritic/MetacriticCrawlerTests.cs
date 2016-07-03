using System.IO;
using System.Reflection;
using Moq;
using RatingsAnalyzer.Crawler;
using RatingsAnalyzer.Crawler.Metacritic;
using RatingsAnalyzer.Model;
using Xunit;

namespace RatingsAnalyzer.Tests.Crawler.Metacritic
{
    public class MetacriticCrawlerTests
    {
        private const string Page1ResourceName = "RatingsAnalyzer.Tests.Crawler.Metacritic.MetacriticMoviesList.html";
        private const string Page2ResourceName = "RatingsAnalyzer.Tests.Crawler.Metacritic.MetacriticMoviesList2.html";

        private Mock<IDownloader> _downloaderMock;

        public MetacriticCrawlerTests()
        {
            _downloaderMock = new Mock<IDownloader>();
            _downloaderMock.Setup(x => x.Get(It.IsAny<string>())).Returns((string uri) => GetHtml(uri)).Verifiable();
        }

        private static string GetHtml(string uri)
        {
            switch (uri)
            {
                case "http://www.metacritic.com/browse/movies/title/dvd":
                case "http://www.metacritic.com/browse/movies/title/dvd/a":
                    return GetResourceString(Page1ResourceName);
                case "http://www.metacritic.com/browse/movies/title/dvd%3Fpage=1":
                    return GetResourceString(Page2ResourceName);
                default:
                    return null;
            }
        }

        private static string GetResourceString(string resourceName)
        {
            string html;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            return html;
        }

        [Fact]
        public void TestEntryParsing()
        {
            var uriStrings = new[]
            {
                "http://www.metacritic.com/movie/horror",
                "http://www.metacritic.com/movie/999",
                "http://www.metacritic.com/movie/40-days-and-40-nights",
                "http://www.metacritic.com/movie/42"
            };

            var crawler = new MetacriticCrawler(_downloaderMock.Object, uri => new EntryParserMock(uri));
            var enumerable = crawler.GetEntries();
            var enumerator = enumerable.GetEnumerator();
            Assert.NotNull(enumerator);

            for (int i = 0; i < 4; i++)
            {
                enumerator.MoveNext();
                var entry = enumerator.Current;
                Assert.Equal(uriStrings[i], entry.Uri);
            }
        }

        [Fact]
        public void TestPagination()
        {
            _downloaderMock.ResetCalls();
            var crawler = new MetacriticCrawler(_downloaderMock.Object, uri => new EntryParserMock(uri));
            var enumerable = crawler.GetEntries();
            var enumerator = enumerable.GetEnumerator();
            Assert.NotNull(enumerator);

            enumerator.MoveNext(); // Getting first entry triggers the page download
            _downloaderMock.Verify(downloader => downloader.Get("http://www.metacritic.com/browse/movies/title/dvd"));
            enumerator.MoveNext(); // Two entries on the first page

            enumerator.MoveNext(); // Next entry triggers download of the second page for letter
            _downloaderMock.Verify(downloader => downloader.Get("http://www.metacritic.com/browse/movies/title/dvd%3Fpage=1"));
            enumerator.MoveNext(); // Two entries on the second page

            enumerator.MoveNext(); // Next entry triggers download for next letter
            _downloaderMock.Verify(downloader => downloader.Get("http://www.metacritic.com/browse/movies/title/dvd/a"));
        }

        private class EntryParserMock : IEntryParser
        {
            public string Uri { get; private set; }

            public EntryParserMock(string uri)
            {
                Uri = uri;
            }

            public MovieData Parse()
            {
                return null;
            }
        }
    }
}
