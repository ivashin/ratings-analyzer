using System.Linq;
using RatingsAnalyzer.Model;

namespace RatingsAnalyzer.Analytics
{
    public class CsvEntry
    {
        public CsvEntry(MovieData movie, Source source)
        {
            Movie = movie.Title;
            var rating = movie.MovieRatings.Single(r => r.Source == source);
            CriticsRating = rating.CriticsRating;
            AudienceRating = rating.AudienceRating;
        }

        public string Movie { get; set; }
        public double CriticsRating { get; set; }
        public double AudienceRating { get; set; }
    }
}
