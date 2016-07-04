using System.Linq;
using RatingsAnalyzer.Model;

namespace RatingsAnalyzer.DataAccess
{
    public interface IDbService
    {
        void SaveEntry(MovieData entry);

        IQueryable<MovieData> Query();
    }
}
