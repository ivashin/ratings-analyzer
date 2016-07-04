using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RatingsAnalyzer.DataAccess;
using RatingsAnalyzer.Model;

namespace RatingsAnalyzer.Analytics
{
    public class RatingsAnalytics
    {
        private IDbService _dbService;

        public RatingsAnalytics(IDbService dbService)
        {
            _dbService = dbService;
        }

        public List<MovieData> GetUnderratedMovies(Source source, int results)
        {
            var result = _dbService.Query(
                movies => movies.Include(m => m.MovieRatings)
                                .OrderByDescending(m => m.MovieRatings.Where(r => r.Source == source)
                                                                      .Max(r => r.AudienceRating - r.CriticsRating))
                                .Take(results)
                                .ToList()
                    );
            return result;
        }

        public List<MovieData> GetOverratedMovies(Source source, int results)
        {
            var result = _dbService.Query(
                movies => movies.Include(m => m.MovieRatings)
                                .OrderByDescending(m => m.MovieRatings.Where(r => r.Source == source)
                                                                      .Max(r => r.CriticsRating - r.AudienceRating))
                                .Take(results)
                                .ToList()
                    );
            return result;
        } 
    }
}
