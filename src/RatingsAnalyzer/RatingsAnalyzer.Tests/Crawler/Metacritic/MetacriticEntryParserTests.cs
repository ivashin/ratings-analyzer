using System.IO;
using System.Reflection;
using Moq;
using RatingsAnalyzer.Crawler;
using RatingsAnalyzer.Crawler.Metacritic;
using RatingsAnalyzer.Model;
using Xunit;

namespace RatingsAnalyzer.Tests.Crawler.Metacritic
{
    public class MetacriticEntryParserTests
    {
        private const string ItemUri = "http://www.metacritic.com/movie/horror";
        private const string HtmlResourceName = "RatingsAnalyzer.Tests.Crawler.Metacritic.MetacriticMovieEntry.html";

        private readonly MovieData _result;

        public MetacriticEntryParserTests()
        {
            string html;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(HtmlResourceName))
            using (var reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            var downloaderMock = new Mock<IDownloader>();
            downloaderMock.Setup(x => x.Get(It.IsAny<string>())).Returns(html);

            var parser = new MetacriticEntryParser(downloaderMock.Object, ItemUri);
            _result = parser.Parse();
        }

        [Fact]
        public void TestTitleParsing()
        {
            Assert.Equal("#Horror", _result.Title);
        }

        [Fact]
        public void TestRatingParsing()
        {
            Assert.Single(_result.Ratings);
            Assert.NotNull(_result.Ratings[0]);
        }

        [Fact]
        public void TestUri()
        {
            Assert.Equal(ItemUri, _result.Ratings[0].Uri);
        }

        public void TestSource()
        {
            Assert.Equal(Source.Metacritic, _result.Ratings[0].Source);
        }

        [Fact]
        public void TestCriticsRating()
        {
            Assert.Equal(4.2, _result.Ratings[0].CriticsRating);
        }

        [Fact]
        public void TestCriticsRatingsCount()
        {
            Assert.Equal(7, _result.Ratings[0].CriticsRatingsCount);
        }

        [Fact]
        public void TestAudienceRating()
        {
            Assert.Equal(3.4, _result.Ratings[0].AudienceRating);
        }

        [Fact]
        public void TestAudienceRatingsCount()
        {
            Assert.Equal(11, _result.Ratings[0].AudienceRatingsCount);
        }
    }
}
