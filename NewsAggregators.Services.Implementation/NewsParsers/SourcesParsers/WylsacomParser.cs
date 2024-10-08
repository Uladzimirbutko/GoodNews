﻿using System;
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

            try
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
                        Rating = null

                    };

                    if (!string.IsNullOrEmpty(news.Body) && !string.IsNullOrEmpty(news.Summary))
                    {
                        newsCollection.Add(news);
                    }
                });
                //}
                return newsCollection;
            }
            catch (Exception e)
            {
                Log.Error($"Error Parse Onliner News {e.Message}");
                return null;
            }

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
                    .DocumentNode.SelectSingleNode("//section[@class='article__img']")?.OuterHtml;

                if (string.IsNullOrEmpty(nodes))
                {
                    return "/img/Wylsacom.jpg";
                }

                var patternLinkImg = "(https.*?jpg)";

                nodes = Regex.Match(nodes, patternLinkImg).Value;

                
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
                    .DocumentNode.SelectSingleNode("//div[@class='content__inner']")?.OuterHtml;

                if (string.IsNullOrEmpty(nodes))
                {
                    Log.Information($"Body Wylsacom {bodyUrl} is null");
                    return null;
                }

                nodes = nodes.Replace("<div class=\"content__inner\">", "<div class=\"align-content-center\">");
                nodes = nodes.Replace("padding-bottom:75%", "padding-bottom:10px");

                var embededPostRegex = "(<a class=\"embeded-post\"((?:.|\n)*)<\\/a>)";
                nodes = Regex.Replace(nodes, embededPostRegex, "");

                var oldImgValue = new List<string>()
                {
                    "<img loading=\"lazy\" class=\"alignnone size-full",
                };

                Parallel.ForEach(oldImgValue, old =>
                {
                    if (nodes.Contains(old))
                    {

                        if (nodes.Contains("<img loading=\"lazy\" class=\"alignnone size-full"))
                        {
                            nodes = nodes.Replace(old, "<img class=\"img-fluid\"");
                        }
                    }
                });

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