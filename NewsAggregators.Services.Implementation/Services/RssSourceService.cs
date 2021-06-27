using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.DAL.Repositories.Implementation;

namespace NewsAggregator.Services.Implementation.Services
{
    public class RssSourceService : IRssSourceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RssSourceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<RssSourceDto>> GetAllRssSources()
        {
            return await _unitOfWork.Rss.GetAll()
                .Select(source => _mapper.Map<RssSourceDto>(source))
                .ToListAsync();
        }

        public async Task<RssSourceDto> GetRssSourceById(Guid id)
        {
            return _mapper.Map<RssSourceDto>(await _unitOfWork.Rss.GetById(id));
        }

        public async Task<IEnumerable<RssSourceDto>> GetRssSourcesByNameAndUrl(string name, string url)
        {
            return null;
        }
    }
}