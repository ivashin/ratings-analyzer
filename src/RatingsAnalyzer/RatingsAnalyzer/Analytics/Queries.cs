using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RatingsAnalyzer.DataAccess;
using RatingsAnalyzer.Model;

namespace RatingsAnalyzer.Analytics
{
    public interface IQueries
    {
        List<MovieData> GetUnderratedMovies(Source source, int results);
        List<MovieData> GetOverratedMovies(Source source, int results);
    }

    public class Queries : IQueries
    {
        private readonly IDbService _dbService;

        public Queries(IDbService dbService)
        {
            _dbService = dbService;
        }

        public List<MovieData> GetUnderratedMovies(Source source, int results)
        {
            var result = _dbService.QueryList(
                movies =>
                {
                    IQueryable<MovieData> query =
                        movies.Include(m => m.MovieRatings)
                              .Where(m => m.MovieRatings.Any(r => r.Source == source && r.AudienceRating > r.CriticsRating))
                              .OrderByDescending(m => m.MovieRatings.Where(r => r.Source == source)
                                                                    .Max(r => r.AudienceRating - r.CriticsRating));
                    if (results > 0)
                    {
                        query = query.Take(results);
                    }
                    return query;
                }
                );
            return result;
        }

        public List<MovieData> GetOverratedMovies(Source source, int results)
        {
            var result = _dbService.QueryList(
                movies =>
                {
                    IQueryable<MovieData> query =
                        movies.Include(m => m.MovieRatings)
                              .Where(m => m.MovieRatings.Any(r => r.Source == source && r.AudienceRating < r.CriticsRating))
                              .OrderByDescending(m => m.MovieRatings.Where(r => r.Source == source)
                                                                    .Max(r => r.CriticsRating - r.AudienceRating));
                    if (results > 0)
                    {
                        query = query.Take(results);
                    }
                    return query;
                }
                );
            return result;
        } 
    }
}
