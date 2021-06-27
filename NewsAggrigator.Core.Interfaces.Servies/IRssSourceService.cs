using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.Core.Services.Interfaces
{
    public interface IRssSourceService
    {
        Task<IEnumerable<RssSourceDto>> GetAllRssSources();
        Task <RssSourceDto> GetRssSourceById(Guid id);

        Task<IEnumerable<RssSourceDto>> GetRssSourcesByNameAndUrl
            (string name, string url);



    }
}
