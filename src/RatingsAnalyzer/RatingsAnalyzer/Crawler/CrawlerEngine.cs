﻿using System;
using System.Linq;
using NLog;

namespace RatingsAnalyzer.Crawler
{
    public class CrawlerEngine
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ICrawler _crawler;

        public CrawlerEngine(ICrawler crawler)
        {
            _crawler = crawler;
        }

        public void GetData(int count)
        {
            Logger.Info("Data extraction started.");
            var entries = _crawler.GetEntries();
            if (count > 0)
            {
                entries = entries.Take(count);
            }

            int errors = 0;
            int processedEntries = 0;
            foreach (var entry in entries) // Potential improvement: perform calls in parallel using PLINQ/TPL
            {
                try
                {
                    var data = entry.Parse();
                }
                catch (Exception e) // Single failed entry should not affect whole process
                {
                    errors++;
                    Logger.Error(e, "Error while processing entry {0}.", entry.Uri);
                }

                // TODO: save to DB

                processedEntries++;
                if (processedEntries % 100 == 0)
                {
                    Logger.Info("Processed {0} entries.", processedEntries);
                }
            }

            Logger.Info("Total entries processed: {0}. Errors: {1}", processedEntries, errors);
            Logger.Info("Data extraction ended.");
        }
    }
}