using System;
using Autofac;
using CommandLine;
using Microsoft.EntityFrameworkCore;
using NLog;
using RatingsAnalyzer.Crawler;
using RatingsAnalyzer.Crawler.Metacritic;
using RatingsAnalyzer.DataAccess;
using RatingsAnalyzer.Util;

namespace RatingsAnalyzer
{
    class Runner
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private IContainer _iocContainer;

        static int Main(string[] args)
        {
            var runner = new Runner();
            return runner.Run(args);
        }

        public int Run(string[] args)
        {
            try
            {
                Logger.Debug("Command line: {0}", String.Join(" ", args));

                var options = new CommandLineOptions();
                var command = Command.None;
                if (!Parser.Default.ParseArguments(args, options, (verb, _) => command = verb.ConvertToEnum<Command>())
                    || command == Command.None)
                {
                    Logger.Error("Invalid command-line arguments");
                    var help = CommandLine.Text.HelpText.AutoBuild(options);
                    Console.WriteLine(help.ToString());
                    return 1;
                }

                Logger.Info("Starting application");
                _iocContainer = SetupIoCContainer();

                Logger.Info("Executing {0} command", command);
                switch (command)
                {
                    case Command.Get:
                        _iocContainer.Resolve<CrawlerEngine>().GetData(options.GetVerb.Results);
                        break;
                    case Command.Analyze:
                        break;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "Critical error has occurred. Please see application logs for more details.");
                return -1;
            }
            return 0;
        }

        private IContainer SetupIoCContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<CrawlerEngine>();
            builder.RegisterType<MetacriticCrawler>().As<ICrawler>();
            builder.RegisterType<MetacriticEntryParser>().As<IEntryParser>();
            builder.RegisterType<WebPageDownloader>().As<IPageDownloader>();
            builder.RegisterType<DbService>().As<IDbService>();
            builder.RegisterType<MovieRatingsContext>().AsSelf().As<DbContext>();
            return builder.Build();
        }
    }
}
