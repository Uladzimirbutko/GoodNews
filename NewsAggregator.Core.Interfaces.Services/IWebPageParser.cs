using System.Threading.Tasks;

namespace NewsAggregator.Core.Interfaces.Services
{
    public interface IWebPageParser // parse - get raw data -> requested data
    {
        Task<string> Parse(string url);
    }
}