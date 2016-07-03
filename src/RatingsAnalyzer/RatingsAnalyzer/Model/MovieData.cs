using System.Collections.Generic;

namespace RatingsAnalyzer.Model
{
    public class MovieData
    {
        public string Title { get; set; }
        public List<MovieRating> Ratings { get; set; }
    }
}
