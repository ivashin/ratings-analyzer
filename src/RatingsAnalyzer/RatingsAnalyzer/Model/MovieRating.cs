namespace RatingsAnalyzer.Model
{
    public class MovieRating
    {
        public string Uri { get; set; }
        public Source Source { get; set; }
        public double CriticsRating { get; set; }
        public int CriticsRatingsCount { get; set; }
        public double AudienceRating { get; set; }
        public int AudienceRatingsCount { get; set; }
    }
}
