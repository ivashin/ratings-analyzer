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

        private readonly Func<IDownloader> _downloaderFactory;
        private readonly string _uri;

        public MetacriticEntryParser(Func<IDownloader> downloaderFactory, string uri)
        {
            _downloaderFactory = downloaderFactory;
            _uri = uri;
        }

        public MovieData Parse()
        {
            Logger.Debug("Parsing {0}", _uri);
            var downloader = _downloaderFactory();
            var page = downloader.Get(_uri);

            var doc = new HtmlDocument();
            doc.LoadHtml(page);
            var rootNode = doc.DocumentNode;

            var data = new MovieData();

            data.Title = rootNode.GetText(TitleXPath);

            var rating = new MovieRating();
            rating.Source = Model.Source.Metacritic;
            rating.Uri = _uri;

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
