using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Services.Implementation.NewsParsers.SourcesParsers;
using Serilog;

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
                    //case "Onliner": //выполнено
                    //    {
                    //        var parser = new OnlinerParser();

                    //        var parsedList = parser.Parsing(sourceDto.Url);

                    //        newsInfos.AddRange(parsedList.Select(news => _mapper.Map<NewsDto>(news)));

                    //        break;
                    //    }

                    //case "S13":
                    //    {
                    //        var parser = new S13Parser();

                    //        var parsedList = parser.Parsing(sourceDto.Url);

                    //        newsInfos.AddRange(parsedList.Select(news => _mapper.Map<NewsDto>(news)));

                    //        break;
                    //    }

                    //case "4pda":
                    //    {
                    //        var parser = new FourPdaParser();

                    //        var parsedList = parser.Parsing(sourceDto.Url);

                    //        newsInfos.AddRange(parsedList.Select(news => _mapper.Map<NewsDto>(news)));

                    //        break;
                    //    }


                    case "Wylsacom":
                        {
                            var parser = new WylsacomParser();

                            var parsedList = parser.Parsing(sourceDto.Url);

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