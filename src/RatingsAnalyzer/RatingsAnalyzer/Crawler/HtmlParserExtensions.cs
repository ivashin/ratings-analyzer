using System;
using System.Globalization;
using HtmlAgilityPack;

namespace RatingsAnalyzer.Crawler
{
    public static class HtmlParserExtensions
    {
        public static string GetText(this HtmlNode node, string xpath)
        {
            var textNode = node.SelectSingleNode(xpath);
            return textNode.InnerText.Trim();
        }

        public static double? GetDouble(this HtmlNode node, string xpath)
        {
            var textNode = node.SelectSingleNode(xpath);
            double result;
            return textNode != null &&
                   Double.TryParse(textNode.InnerText.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture,out result)
                ? result
                : (double?) null;
        }

        public static int GetInt(this HtmlNode node, string xpath)
        {
            var textNode = node.SelectSingleNode(xpath);
            return Int32.Parse(textNode.InnerText.Trim(), CultureInfo.InvariantCulture);
        }
    }
}
