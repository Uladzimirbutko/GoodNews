using System.Threading.Tasks;
using HtmlAgilityPack;
using NewsAggregator.Core.Services.Interfaces;

namespace NewsAggregator.Services.Implementation.NewsParser
{
    public class OnlinerParser : IWebPageParser
    {
        public async Task<string> Parse(string url)
        {
            
            var web = new HtmlWeb();

            var htmlDoc = web.Load(url);

            var node = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='news-text']");
            return "";
        }
    }
}