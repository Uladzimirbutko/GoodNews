using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsAggregator.Core.Interfaces.Services
{
    public interface IRatingService
    {
        Task<double> RateNews(string newsText);
        Task<Dictionary<string, int>> GetVocabulary();
        Task<string> SendResponseToLemmatization(string? text);
        IEnumerable<string> ResponseDeserialization(string response);
        double Rate(Dictionary<string, int> vocabulary, IEnumerable<string> words);

    }
}