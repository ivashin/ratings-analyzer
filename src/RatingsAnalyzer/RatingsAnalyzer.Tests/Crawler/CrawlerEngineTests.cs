using System.Linq;
using Moq;
using RatingsAnalyzer.Crawler;
using RatingsAnalyzer.DataAccess;
using RatingsAnalyzer.Model;
using Xunit;

namespace RatingsAnalyzer.Tests.Crawler
{
    public class CrawlerEngineTests
    {
        private const int EntriesCount = 10;
        private const int Threshold = 5;

        private readonly Mock<IEntryParser> _entryParserMock;
        private readonly Mock<ICrawler> _crawlerMock;
        private readonly Mock<IDbService> _dbServiceMock;

        public CrawlerEngineTests()
        {
            _entryParserMock = new Mock<IEntryParser>();
            _entryParserMock.Setup(x => x.Parse()).Returns(new MovieData()).Verifiable();
            _entryParserMock.Setup(x => x.Uri).Returns("http://www.example.com");

            _crawlerMock = new Mock<ICrawler>();
            _crawlerMock.Setup(crawler => crawler.GetEntries()).Returns(Enumerable.Repeat(_entryParserMock.Object, EntriesCount)).Verifiable();

            _dbServiceMock = new Mock<IDbService>();
            _dbServiceMock.Setup(db => db.SaveEntry(It.IsAny<MovieData>())).Verifiable();
        }

        [Fact]
        public void TestEnumeration()
        {
            _entryParserMock.ResetCalls();
            _crawlerMock.ResetCalls();
            _dbServiceMock.ResetCalls();

            var engine = new CrawlerEngine(_crawlerMock.Object, _dbServiceMock.Object);
            engine.GetData(0);

            _crawlerMock.Verify(crawler => crawler.GetEntries(), Times.Once);
            _entryParserMock.Verify(x => x.Parse(), Times.Exactly(EntriesCount));
            _dbServiceMock.Verify(db => db.SaveEntry(It.IsAny<MovieData>()), Times.Exactly(EntriesCount));
        }

        [Fact]
        public void TestEnumerationThreshold()
        {
            _entryParserMock.ResetCalls();
            _crawlerMock.ResetCalls();
            _dbServiceMock.ResetCalls();

            var engine = new CrawlerEngine(_crawlerMock.Object, _dbServiceMock.Object);
            engine.GetData(Threshold);

            _crawlerMock.Verify(crawler => crawler.GetEntries(), Times.Once);
            _entryParserMock.Verify(x => x.Parse(), Times.Exactly(Threshold));
            _dbServiceMock.Verify(db => db.SaveEntry(It.IsAny<MovieData>()), Times.Exactly(Threshold));
        }
    }
}
