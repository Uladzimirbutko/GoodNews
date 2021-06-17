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
    public class S13Parser :IWebPageParser
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
                var news = new News()
                {
                    Id = Guid.NewGuid(),
                    Article = item.Title.Text,
                    Body = BodyParser(item.Id),
                    Summary = Summary(item.Summary.Text),
                    PublicationDate = item.PublishDate.UtcDateTime,
                    RssSourceId = _sourceId,
                    Url = item.Id,
                    TitleImage = BodyParser(item.Summary.Text),
                    Category = item.Categories.ToString(),

                };
                newsCollection.Add(news);
            });
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
            var nodes = new HtmlWeb().Load(url)?
                .DocumentNode.SelectSingleNode(("//div[@class='content']"))?//<div class="content">
                .ChildNodes?.Nodes();
            return null;
        }

        public string BodyParser(string bodyUrl)
        {
            try
            {
                var nodes = new HtmlWeb().Load(bodyUrl)?
                    .DocumentNode.SelectSingleNode(("//div[@class='content']"))?
                    .ChildNodes?.Nodes();

                if (nodes == null)
                {
                    throw new Exception($"News with Url {bodyUrl} not invalid");
                }

                return HttpUtility.HtmlDecode(nodes
                    .Where(n => n.ParentNode.Name == "p") //|| n.ParentNode.Name == "h2")
                    .Where(i => !i.InnerText.StartsWith("Наш канал в") && !i.InnerText.StartsWith("Есть о чем рассказать?"))
                    .Aggregate("", (current, n) => current + n.InnerText));
            }

            catch (Exception e)
            {
                Log.Error($"Error in Onliner Parser in Body. Exception: {e}.Message: {e.Message}");
                return bodyUrl;
            }
        }
    }
}