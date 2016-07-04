using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RatingsAnalyzer.Model
{
    public class MovieData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MovieDataId { get; set; }
        public string Title { get; set; }
        public List<MovieRating> MovieRatings { get; set; }
    }
}
