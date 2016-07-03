using CommandLine;

namespace RatingsAnalyzer
{
    class CommandLineOptions
    {
        [VerbOption("get", HelpText = "Extract data from an online source.")]
        public GetOptions GetOptions { get; set; }

        [VerbOption("analyze", HelpText = "Analyze extracted data.")]
        public AnalyzeOptions AnalyzeOptions { get; set; }

        [Option('s', "source", DefaultValue = Source.None, HelpText = "Data source")]
        public Source Source { get; set; }

        [Option('r', "results", DefaultValue = 100, HelpText = "Number of results to return")]
        public int Results { get; set; }
    }

    class GetOptions
    {
        
    }

    class AnalyzeOptions
    {
        
    }

    enum Command
    {
        None,
        Get,
        Analyze
    }

    enum Source
    {
        None,
        Metacritic,
        RottenTomatoes,
        Kinopoisk
    }
}
