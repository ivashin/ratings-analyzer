using System;
using CommandLine;
using NLog;

namespace RatingsAnalyzer
{
    class Runner
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static int Main(string[] args)
        {
            var runner = new Runner();
            return runner.Run(args);
        }

        public int Run(string[] args)
        {
            try
            {
                Logger.Info("Starting application");
                Logger.Debug("Command line: {0}", String.Join(" ", args));

                var options = new CommandLineOptions();
                var command = Command.None;
                if (!Parser.Default.ParseArguments(args, options, (verb, _) => Enum.TryParse(verb, out command))
                    || command == Command.None)
                {
                    Logger.Error("Invalid command-line arguments");
                    var help = CommandLine.Text.HelpText.AutoBuild(options);
                    Console.WriteLine(help.ToString());
                    return 1;
                }

                switch (command)
                {
                    case Command.Get:
                        break;
                    case Command.Analyze:
                        break;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "Critical error has occurred.");
                return -1;
            }
            return 0;
        }
    }
}
