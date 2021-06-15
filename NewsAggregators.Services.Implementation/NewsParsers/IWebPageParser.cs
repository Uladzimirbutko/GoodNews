using System.Collections.Generic;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.Services.Implementation.NewsParsers
{
    public interface IWebPageParser
    {
        IEnumerable<News> Parsing(string url);
        string Description(string description);
        string ImageParser(string url);
        string BodyParser(string bodyUrl);
    }
}