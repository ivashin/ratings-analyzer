using System;
using System.Linq;
using RatingsAnalyzer.Model;

namespace RatingsAnalyzer.DataAccess
{
    public interface IDbService
    {
        void SaveEntry(MovieData entry);

        T Query<T>(Func<IQueryable<MovieData>, T> query);
    }
}
