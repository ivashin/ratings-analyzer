using System.ComponentModel.DataAnnotations.Schema;

namespace RatingsAnalyzer.Model
{
    public class MovieRating
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MovieRatingId { get; set; }
        public string Uri { get; set; }
        public Source Source { get; set; }
        public double? CriticsRating { get; set; }
        public int CriticsRatingsCount { get; set; }
        public double? AudienceRating { get; set; }
        public int AudienceRatingsCount { get; set; }

        public int MovieDataId { get; set; }
        public MovieData MovieData { get; set; }
    }
}
