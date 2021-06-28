using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Services.Implementation.NewsParsers.SourcesParsers;

namespace NewsAggregator.Services.Implementation.NewsParsers
{
    public class Parser
    {
        private readonly IMapper _mapper;

        public Parser(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IEnumerable<NewsDto> Parse(IEnumerable<RssSourceDto> sources, IEnumerable<string> newsUrls)
        {
            var newsInfos = new List<NewsDto>();


            Parallel.ForEach(sources, sourceDto =>
            {

                switch (sourceDto.SourceName)
                {
                    case "Onliner":
                        {
                            var parser = new OnlinerParser();

                            var parsedList = parser.Parsing(sourceDto.Url);
                            if (parsedList == null)
                            {
                                break;
                            }
                            newsInfos.AddRange(parsedList.Select(news => _mapper.Map<NewsDto>(news)));

                            break;
                        }

                    case "S13":
                        {
                            var parser = new S13Parser();

                            var parsedList = parser.Parsing(sourceDto.Url);
                            if (parsedList == null)
                            {
                                break;
                            }
                            newsInfos.AddRange(parsedList.Select(news => _mapper.Map<NewsDto>(news)));

                            break;
                        }

                    case "Wylsacom":
                        {
                            var parser = new WylsacomParser();

                            var parsedList = parser.Parsing(sourceDto.Url);
                            if (parsedList == null)
                            {
                                break;
                            }
                            newsInfos.AddRange(parsedList.Select(item => _mapper.Map<NewsDto>(item)));

                            break;
                        }
                }
            });

            var newsDtos = newsInfos.Where(news => !newsUrls.Any(n => n.Equals(news.Url)));

            return newsDtos;

        }
    }
}