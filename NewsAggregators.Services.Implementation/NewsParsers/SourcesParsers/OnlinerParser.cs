using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public class OnlinerParser : IWebPageParser
    {
        private readonly Guid _sourceId = new("AA1343DE-DB99-49A7-AF25-640F7B838B73");

        public IEnumerable<News> Parsing(string url)
        {
            using var reader = XmlReader.Create(url);
            var feed = SyndicationFeed.Load(reader);
            reader.Close();

            var newsCollection = new ConcurrentBag<News>();

            //Parallel.ForEach(feed.Items, item =>
            //{
            foreach (var item in feed.Items)
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
                    TitleImage = ImageParser(item.Id),
                    Category = item.Categories[0].Name

                };
                

                newsCollection.Add(news);

            }//);

            newsCollection.OrderByDescending(pd => pd.PublicationDate);

            return newsCollection;
        }

        public string Summary(string description)
        {
            var document = new HtmlDocument();
            document.LoadHtml(description);
            var res = document.DocumentNode.InnerText;

            return HttpUtility.HtmlDecode(res.Remove(res.IndexOf("Читать далее…")));
        }

        public string ImageParser(string url)
        {
            var nodes = new HtmlWeb().Load(url)?
                .DocumentNode.SelectSingleNode(("//div[@class='news-header__image']")).OuterHtml;
            var regex = new Regex(@"(https?:\/\/)?([\w-]{1,32}\.[\w-]{1,32})[^\s@]*jpeg").Match(nodes);

            return regex.Value;
        }

        public string BodyParser(string bodyUrl)
        {

            try
            {
                //var nodes = new HtmlWeb().Load(bodyUrl)?
                //    .DocumentNode.SelectSingleNode(("//div[@class='news-text']"))?
                //    .ChildNodes?.Nodes();
                var nodes = new HtmlWeb().Load(bodyUrl)?
                    .DocumentNode.SelectSingleNode(("//div[@class='news-text']"))?
                    .OuterHtml;
                if (nodes == null)
                {
                    throw new Exception($"News with Url {bodyUrl} not invalid");
                }

                #region old parsing
                //var aggregate = nodes
                //    .Where(n => n.ParentNode.Name == "p" || n.ParentNode.Name == "h2")
                //    .Aggregate("", (current, n) => current + $"{CheckingStringLength(n.InnerText)}");


                //var removeText = new List<string>()
                //{
                //    "Читайте дальше:",
                //    "в Каталоге",
                //    "Перепечатка текста",
                //    "Наш канал в",
                //    "Есть о чем рассказать?",
                //    "Знакомы с ситуацией?",
                //    "Пишите нам:",
                //    "Auto.Onliner",
                //    "(будет дополнено)",
                //    "Читайте также:"
                //};

                //foreach (var index in from remove in removeText where aggregate.Contains(remove) select aggregate.LastIndexOf(remove))
                //{
                //    aggregate = aggregate.Remove(index).TrimEnd();

                //    if (!aggregate.EndsWith("."))
                //    {
                //        var len = aggregate.Length;
                //        aggregate = aggregate.Insert(len - 1, ".");
                //    }
                //}

                //var decodeString = HttpUtility.HtmlDecode(aggregate);


                #endregion


                #region video replace



                // получаем блок iframe весь
                var iframeReplace = @"(<iframe.*?>)";

                nodes = Regex.Replace(nodes, iframeReplace, "");

                //var arrayNodes = Regex.Split(nodes, iframeReplace);


                //for (int i = 0; i < arrayNodes.Length; i++)
                //{
                //    if (arrayNodes[i].Contains("iframe"))
                //    {
                //        var videoLink = Regex.Match(arrayNodes[i], @"https(.*?)\s"); //забираем ссылку
                //        var oldIframe = arrayNodes[i];
                //        var newIframe = $"<iframe class=\"embed-responsive-item\" src=\"{videoLink.Value} allowfullscreen>";
                //        arrayNodes[i] = arrayNodes[i].Replace(oldIframe, newIframe);

                //    }
                //}

                //nodes = arrayNodes.Aggregate("", (s, s1) => s + s1);

                #endregion

                #region Replace styles and more



                var oldValue = new List<string>()
                {
                    "<img loading=\"lazy\" class=\"aligncenter\"",
                    "<img loading=\"lazy\" class=\"alignnone",
                };

                foreach (var old in oldValue)
                {

                    if (nodes.Contains(old))
                    {

                        if (nodes.Contains("<img loading=\"lazy\" class=\"aligncenter\""))
                        {
                            nodes = nodes.Replace(old, "<img class=\"img-fluid\"");
                        }
                        else if (nodes.Contains("<img loading=\"lazy\" class=\"alignnone"))
                        {
                            nodes = nodes.Replace(old, "<img class=\"img-fluid\"");
                        }
                        

                        

                    }
                }

                #endregion

                #region To remove unnecessary text

                var variantsOfRemoveText = new List<string>()
                {
                    "<h2><a href=\"https://catalog.onliner",
                    "<div class=\"news-widget news-widget_special\">",
                    "<h3><a href=\"https://catalog",
                    "<div class=\"news-widget\">",
                    "<p style=\"text-align: right;",
                    "<h2 style=\"text-align: center;",
                    "Auto.Onliner",
                    "<div class=\"news-vote\""


                };


                foreach (var remove in variantsOfRemoveText)
                {
                    if (nodes.Contains(remove))
                    {
                        var index = nodes.LastIndexOf(remove, StringComparison.Ordinal);
                        nodes = nodes.Remove(index);
                    }
                }

                #endregion

                return nodes;

            }

            catch (Exception e)
            {
                Log.Error($"Error in Onliner Parser in Body. Exception: {e}.Message: {e.Message}");
                return bodyUrl;
            }
        }

        public string CheckingStringLength(string str)
        {

            if (str.Length > 30)
            {
                str = str.Insert(0, "<p>");
            }
            return str;
        }

    }
}