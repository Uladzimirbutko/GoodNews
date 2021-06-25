using System.Collections.Generic;
using MediatR;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.DAL.CQRS.Queries.RssSourceQueries
{
    public class GetRssSourceByNameAndUrlQuery: IRequest<IEnumerable<RssSourceDto>>
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public GetRssSourceByNameAndUrlQuery(string name, string url)
        {
            Name = name;
            Url = url;
        }

    }
}