using System.Collections.Generic;
using System.Linq;
using ApprovalTests;
using ApprovalTests.Reporters;
using Moq;
using RatingsAnalyzer.Analytics;
using RatingsAnalyzer.Model;
using Xunit;

namespace RatingsAnalyzer.Tests.Analytics
{
    public class AnalyticsEngineTests
    {
        private Mock<IQueries> _queriesMock;
        private IEnumerable<MovieData> _results; 

        public AnalyticsEngineTests()
        {
            _queriesMock = new Mock<IQueries>();
            _queriesMock.Setup(q => q.GetOverratedMovies(It.IsAny<Source>(), It.IsAny<int>())).Returns(() => _results.ToList());
            _queriesMock.Setup(q => q.GetUnderratedMovies(It.IsAny<Source>(), It.IsAny<int>())).Returns(() => _results.Reverse().ToList());

            _results = new List<MovieData>(new []
            {
                new MovieData()
                {
                    Title = "Slow West",
                    MovieRatings = new List<MovieRating>(new []
                    {
                        new MovieRating()
                        {
                            Source = Source.Metacritic,
                            Uri = "http://www.metacritic.com/movie/slow-west",
                            CriticsRating = 7.2,
                            CriticsRatingsCount = 27,
                            AudienceRating = 6.9,
                            AudienceRatingsCount = 70
                        },
                        new MovieRating()
                        {
                            Source = Source.RottenTomatoes,
                            Uri = "https://www.rottentomatoes.com/m/slow_west_2015/",
                            CriticsRating = 7.5,
                            CriticsRatingsCount = 121,
                            AudienceRating = 7.4,
                            AudienceRatingsCount = 10690
                        }
                    })
                },
                new MovieData()
                {
                    Title = "Sicario",
                    MovieRatings = new List<MovieRating>(new []
                    {
                        new MovieRating()
                        {
                            Source = Source.Metacritic,
                            Uri = "http://www.metacritic.com/movie/sicario",
                            CriticsRating = 8.1,
                            CriticsRatingsCount = 41,
                            AudienceRating = 8.1,
                            AudienceRatingsCount = 728
                        },
                        new MovieRating()
                        {
                            Source = Source.RottenTomatoes,
                            Uri = "https://www.rottentomatoes.com/m/sicario_2015/",
                            CriticsRating = 8.0,
                            CriticsRatingsCount = 229,
                            AudienceRating = 8.0,
                            AudienceRatingsCount = 62075
                        }
                    })
                }
            });
        }

        [Fact]
        [UseReporter(typeof(DiffReporter))]
        public void TestUnderratedQuery()
        {
            var engine = new AnalyticsEngine(_queriesMock.Object);
            var fileName = "UnderratedMovies.csv";
            engine.FindUnderratedMovies(0, fileName);
            Approvals.VerifyFile(fileName);
        }

        [Fact]
        [UseReporter(typeof(DiffReporter))]
        public void TestOverratedQuery()
        {
            var engine = new AnalyticsEngine(_queriesMock.Object);
            var fileName = "OverratedMovies.csv";
            engine.FindOverratedMovies(0, fileName);
            Approvals.VerifyFile(fileName);
        }
    }
}
