using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using NLog;
using RatingsAnalyzer.Model;

namespace RatingsAnalyzer.Analytics
{
    public class AnalyticsEngine
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IQueries _queries;

        public AnalyticsEngine(IQueries queries)
        {
            _queries = queries;
        }

        public void FindOverratedMovies(int resultsCount, string fileName)
        {
            Logger.Debug("Executing 'GetOverratedMovies' query");
            ExecuteQuery(_queries.GetOverratedMovies, resultsCount, fileName);
        }

        public void FindUnderratedMovies(int resultsCount, string fileName)
        {
            Logger.Debug("Executing 'GetUnderratedMovies' query");
            ExecuteQuery(_queries.GetUnderratedMovies, resultsCount, fileName);
        }

        private void ExecuteQuery(Func<Source, int, List<MovieData>> query, int resultsCount, string fileName)
        {
            const Source source = Source.Metacritic;
            var results = query(source, resultsCount);
            Logger.Debug("Received {0} results", results.Count);

            Logger.Info("Saving results to {0}", fileName);
            using (var textWriter = new StreamWriter(fileName))
            using (var writer = new CsvWriter(textWriter))
            {
                writer.WriteHeader<CsvEntry>();
                writer.WriteRecords(results.Select(movie => new CsvEntry(movie, source)));
            }
        }
    }
}
