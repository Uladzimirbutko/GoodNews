using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using HtmlAgilityPack;
using NewsAggregator.DAL.Core.Entities;
using Serilog;

namespace NewsAggregator.Services.Implementation.NewsParsers.SourcesParsers
{
    public class WylsacomParser : IWebPageParser
    {
        private readonly Guid _sourceId = new("965012AB-E3D6-4512-8A5C-6C3218FF99B0");

        public IEnumerable<News> Parsing(string url)
        {
            using var reader = XmlReader.Create(url);
            var feed = SyndicationFeed.Load(reader);
            reader.Close();

            var newsCollection = new ConcurrentBag<News>();

            Parallel.ForEach(feed.Items, item =>
            {
                //foreach (var item in feed.Items)
                //{
                var news = new News()
                {
                    Id = Guid.NewGuid(),
                    Article = item.Title.Text,
                    Body = BodyParser(item.Id),
                    Summary = Summary(item.Summary.Text),
                    PublicationDate = item.PublishDate.LocalDateTime,
                    RssSourceId = _sourceId,
                    Url = item.Id,
                    TitleImage = ImageParser(item.Id),
                    Category = item.Categories[0].Name.ToUpper(),

                };

                if (!String.IsNullOrEmpty(news.Body) && !String.IsNullOrEmpty(news.Summary))
                {
                    newsCollection.Add(news);
                }
            });
            //}
            return newsCollection;
        }

        public string Summary(string description)
        {
            var document = new HtmlDocument();
            document.LoadHtml(description);
            var res = document.DocumentNode.InnerText;

            return HttpUtility.HtmlDecode(res);
        }

        public string ImageParser(string url)
        {
            try
            {
                var nodes = new HtmlWeb().Load(url)?
                    .DocumentNode.SelectSingleNode("//section[@class='article__img']").OuterHtml;
                var patternLinkImg = "(https.*?jpg)";
                nodes = Regex.Match(nodes, patternLinkImg).Value;

                if (string.IsNullOrEmpty(nodes))
                {
                    return "/img/Wylsacom.jpg";
                }
                return nodes;
            }
            catch (Exception e)
            {
                Log.Error($"Error in Image wylsacomParser{e.Message}");
                return "/img/Wylsacom.jpg";
            }



        }

        public string BodyParser(string bodyUrl)
        {
            try
            {
                var nodes = new HtmlWeb().Load(bodyUrl)?
                    .DocumentNode.SelectSingleNode("//div[@class='content__inner']").OuterHtml;

                if (string.IsNullOrEmpty(nodes))
                {
                    Log.Information($"News Url {bodyUrl} is not invalid. Return null");
                    return null;
                }

                nodes = nodes.Replace("<div class=\"content__inner\">", "<div class=\"align-content-center\">");
                nodes = nodes.Replace("padding-bottom:75%", "padding-bottom:10px");

                var embededPostRegex = "(<a class=\"embeded-post\"((?:.|\n)*)<\\/a>)";

                nodes = Regex.Replace(nodes, embededPostRegex, "");
                return nodes;
            }

            catch (Exception e)
            {
                Log.Error($"Error in Onliner Parser in Body. Exception: {e}.Message: {e.Message}");
                return bodyUrl;
            }
        }
    }
}