using System;
using System.Collections.Generic;
using System.Linq;
using RatingsAnalyzer.Model;

namespace RatingsAnalyzer.DataAccess
{
    public interface IDbService
    {
        void EnsureDbCreated();

        void SaveEntry(MovieData entry);

        List<T> QueryList<T>(Func<IQueryable<MovieData>, IEnumerable<T>> query);

        T QueryScalar<T>(Func<IQueryable<MovieData>, T> query);
    }
}
