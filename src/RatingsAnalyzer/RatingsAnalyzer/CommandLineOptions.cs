using System.ComponentModel;
using CommandLine;

namespace RatingsAnalyzer
{
    class CommandLineOptions
    {
        [VerbOption("get", HelpText = "Extract data from an online source.")]
        public GetOptions GetVerb { get; set; }

        [VerbOption("analyze", HelpText = "Analyze extracted data.")]
        public AnalyzeOptions AnalyzeVerb { get; set; }
    }

    abstract class CommonOptions
    {
        [Option('r', "results", DefaultValue = 100, HelpText = "Number of results to return")]
        public int Results { get; set; }
    }

    class GetOptions: CommonOptions
    {
        
    }

    class AnalyzeOptions: CommonOptions
    {
        [Option('u', "underrated", HelpText = "Find underrated movies", DefaultValue = false, MutuallyExclusiveSet = "optionset")]
        public bool Underrated { get; set; }

        [Option('o', "overrated", HelpText = "Find overrated movies", DefaultValue = false, MutuallyExclusiveSet = "optionset")]
        public bool Overrated { get; set; }
        [Option('f', "file", HelpText = "CSV file to save results", Required = true)]
        public string FileName { get; set; }
    }

    enum Command
    {
        None,
        [Description("get")]
        Get,
        [Description("analyze")]
        Analyze
    }
}
