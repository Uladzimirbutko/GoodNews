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
    public class OnlinerParser : IWebPageParser
    {
        private readonly Guid _sourceId = new ("aa1343de - db99 - 49a7-af25-640f7b838b73");

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
                    Summary = Description(item.Summary.Text),
                    PublicationDate = item.PublishDate.UtcDateTime,
                    RssSourceId = _sourceId,
                    Url = item.Id,
                    TitleImage = ImageParser(item.Summary.Text)

                    
                };
                newsCollection.Add(news);
            });
            return newsCollection;
        }

        public string Description(string description)
        {
            var document = new HtmlDocument();
            document.LoadHtml(description);
            var res = document.DocumentNode.InnerText;

            return HttpUtility.HtmlDecode(res.Remove(res.IndexOf("Больше...")));
        }

        public string ImageParser(string url)
        {
            return url.Split('"')[3];
        }

        public string BodyParser(string bodyUrl)
        {
            try
            {
                var nodes = new HtmlWeb().Load(bodyUrl)?
                    .DocumentNode.SelectSingleNode(("//div[@class='news-text']"))?
                    .ChildNodes?.Nodes();

                if (nodes == null)
                {
                    throw new Exception($"News with Url {bodyUrl} not invalid");
                }

                return HttpUtility.HtmlDecode(nodes
                    .Where(n => n.ParentNode.Name == "p" || n.ParentNode.Name == "h2")
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