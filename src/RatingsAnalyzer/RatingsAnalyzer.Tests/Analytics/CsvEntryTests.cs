using System.Collections.Generic;
using RatingsAnalyzer.Analytics;
using RatingsAnalyzer.Model;
using Xunit;

namespace RatingsAnalyzer.Tests.Analytics
{
    public class CsvEntryTests
    {
        private readonly MovieData _expected;
        private readonly CsvEntry _actual;

        public CsvEntryTests()
        {
            _expected = new MovieData()
            {
                Title = "Slow West",
                MovieRatings = new List<MovieRating>(new[]
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
            };

            _actual = new CsvEntry(_expected, Source.Metacritic);
        }

        [Fact]
        public void TestTitle()
        {
            Assert.Equal(_expected.Title, _actual.Movie);
        }

        [Fact]
        public void TestCriticsRating()
        {
            Assert.Equal(_expected.MovieRatings[0].CriticsRating, _actual.CriticsRating);
        }

        [Fact]
        public void TestAudienceRatings()
        {
            Assert.Equal(_expected.MovieRatings[0].AudienceRating, _actual.AudienceRating);
        }
    }
}
