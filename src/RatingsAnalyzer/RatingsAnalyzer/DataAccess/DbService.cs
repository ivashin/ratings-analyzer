using System;
using System.Linq;
using RatingsAnalyzer.Model;

namespace RatingsAnalyzer.DataAccess
{
    class DbService: IDbService
    {
        private readonly Func<MovieRatingsContext> _contextFactory;

        public DbService(Func<MovieRatingsContext> conextFactory)
        {
            _contextFactory = conextFactory;
        }

        public void SaveEntry(MovieData entry)
        {
            using (var db = _contextFactory())
            {
                db.Movies.Add(entry);
                db.SaveChanges();
            }
        }

        public IQueryable<MovieData> Query()
        {
            return null; //_context.Movies;
        }
    }
}
