using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using HtmlAgilityPack;
using NewsAggregator.DAL.Core.Entities;
using Serilog;

namespace NewsAggregator.Services.Implementation.NewsParsers.SourcesParsers
{
    public class S13Parser : IWebPageParser
    {
        private readonly Guid _sourceId = new("B696BEE6-4313-454D-B54D-36B214079C18");

        public IEnumerable<News> Parsing(string url)
        {
            using var reader = XmlReader.Create(url);
            var feed = SyndicationFeed.Load(reader);
            reader.Close();

            var newsCollection = new ConcurrentBag<News>();

            Parallel.ForEach(feed.Items, item =>
            {
            //    foreach (var item in feed.Items)
            //{
                var news = new News()
                {
                    Id = Guid.NewGuid(),
                    Article = item.Title.Text,
                    Body = BodyParser(item.Id),
                    Summary = Summary(item.Summary.Text),
                    PublicationDate = item.PublishDate.LocalDateTime.ToLocalTime(),
                    RssSourceId = _sourceId,
                    Url = item.Id,
                    TitleImage = "/img/S13.jpg",
                    Category = item.Categories[0].Name.ToUpper(),

                };
                if (!String.IsNullOrEmpty(news.Body) && !String.IsNullOrEmpty(news.Summary))

                {
                    newsCollection.Add(news);
                }
                //}
            });

            return newsCollection;
        }

        public string Summary(string summary)
        {
            try
            {
                var document = new HtmlDocument();
                document.LoadHtml(summary);
                var res = document.DocumentNode.InnerText;

                return HttpUtility.HtmlDecode(res);
            }
            catch (Exception e)
            {
                Log.Error($"Error in S13Parser in Summary. Exception: {e}.Message: {e.Message}");
                return summary;
            }
        }

        public string ImageParser(string url)
        {
            return null;
        }

        public string BodyParser(string bodyUrl)
        {
            try
            {
                var nodes = new HtmlWeb()
                    .Load(bodyUrl)?.DocumentNode
                    .SelectNodes("//div[@class='content']")
                    .Nodes();

                if (nodes == null)
                {
                    Log.Information($"News Url {bodyUrl} is not invalid");
                    return bodyUrl;
                }

                return  HttpUtility.HtmlDecode(nodes
                    .Where(n => n.Name == "p" || n.Name == "ul")
                    .Where(n => !n.InnerHtml.Contains("span"))
                    .Aggregate("", (current, n) => current + $"{CheckingStringLength(n.InnerHtml)} ")); 
                
            }

            catch (Exception e)
            {
                Log.Error($"Error in S13Parser in Body. Exception: {e}.Message: {e.Message}");
                return bodyUrl;
            }
        }
        private  string CheckingStringLength(string str)
        {

            if (str.Length > 30)
            {
                str = str.Insert(0, "<p>");
            }
            return str;
        }
    }
}