using System.Collections.Generic;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.Services.Implementation.NewsParsers.SourcesParsers
{
    public class WylsacomParser: IWebPageParser
    {
        public IEnumerable<News> Parsing(string url)
        {
            throw new System.NotImplementedException();
        }

        public string Summary(string description)
        {
            throw new System.NotImplementedException();
        }

        public string ImageParser(string url)
        {
            throw new System.NotImplementedException();
        }

        public string BodyParser(string bodyUrl)
        {
            throw new System.NotImplementedException();
        }
    }
}