﻿using System;
using System.Linq;
﻿using System.Threading;
﻿using NLog;
﻿using RatingsAnalyzer.DataAccess;
﻿using RatingsAnalyzer.Model;

namespace RatingsAnalyzer.Crawler
{
    public class CrawlerEngine
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ICrawler _crawler;
        private readonly IDbService _dbService;

        public CrawlerEngine(ICrawler crawler, IDbService dbService)
        {
            _crawler = crawler;
            _dbService = dbService;
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
                    _dbService.SaveEntry(data);
                }
                catch (Exception e) // Single failed entry should not affect whole process
                {
                    errors++;
                    Logger.Error(e, "Error while processing entry {0}.", entry.Uri);
                }

                processedEntries++;
                if (processedEntries % 100 == 0)
                {
                    Logger.Info("Processed {0} entries. 10 seconds pause", processedEntries);
                    Thread.Sleep(TimeSpan.FromSeconds(10)); // Give their servers some rest :)
                }
            }

            Logger.Info("Total entries processed: {0}. Errors: {1}", processedEntries, errors);
            Logger.Info("Data extraction ended.");
        }
    }
}