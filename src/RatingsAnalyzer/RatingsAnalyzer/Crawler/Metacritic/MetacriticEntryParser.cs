using System;
using System.Collections.Generic;
using System.Globalization;
using HtmlAgilityPack;
using NLog;
using RatingsAnalyzer.Model;

namespace RatingsAnalyzer.Crawler.Metacritic
{
    public class MetacriticEntryParser : IEntryParser
    {
        private const string TitleXPath = @"//h1[@class='product_title']/a/span[@itemprop='name']/text()";
        private const string CriticsRatingXPath = @"//span[@itemprop='ratingValue']/text()";
        private const string CriticsRatingsCountXPath = @"//span[@class='count']/a/span[@itemprop='reviewCount']/text()";
        private const string AudienceRatingXPath = @"//div[@class='userscore_wrap feature_userscore']/a[@class='metascore_anchor']/div/text()";
        private const string AudienceRatingsCountXPath = @"//div[@class='score_summary']/div/div[@class='summary']/p/span[@class='count']/a/text()";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IDownloader _downloader;
        
        public MetacriticEntryParser(IDownloader downloader, string uri)
        {
            _downloader = downloader;
            Uri = uri;
        }

        public string Uri { get; private set; }

        public MovieData Parse()
        {
            Logger.Debug("Parsing {0}", Uri);
            var html = _downloader.Get(Uri);

            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var rootNode = doc.DocumentNode;

            var data = new MovieData();

            data.Title = rootNode.GetText(TitleXPath);

            var rating = new MovieRating();
            rating.Source = Model.Source.Metacritic;
            rating.Uri = Uri;

            rating.CriticsRating = rootNode.GetDouble(CriticsRatingXPath) / 10.0;
            rating.CriticsRatingsCount = rootNode.GetInt(CriticsRatingsCountXPath);

            rating.AudienceRating = rootNode.GetDouble(AudienceRatingXPath);
            var audienceRatingsCount = rootNode.GetText(AudienceRatingsCountXPath);
            rating.AudienceRatingsCount = Int32.Parse(audienceRatingsCount.Replace(" Ratings", ""), CultureInfo.InvariantCulture);

            data.Ratings = new List<MovieRating>();
            data.Ratings.Add(rating);
            return data;
        }
    }
}
