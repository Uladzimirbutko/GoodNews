using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using NewsAggregator.DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.DAL.Repositories.Implementation;

namespace NewsAggregator.Services.Implementation
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
            return await _unitOfWork.Rss
                .FindBy(source => !string.IsNullOrEmpty(source.SourceName))
                .Select(source => _mapper.Map<RssSourceDto>(source))
                .ToListAsync();
        }

        public async Task<RssSourceDto> GetRssSourceById(Guid id)
        {
            var rss = await _unitOfWork.Rss.GetById(id);
            return _mapper.Map<RssSourceDto>(rss);
        }

        public async Task<IEnumerable<RssSourceDto>> GetRssSourcesByIds(IEnumerable<Guid> ids)
        {
            return await _unitOfWork.Rss.FindBy(source => ids
                    .Contains(source.Id))
                .Select(source => _mapper.Map<RssSourceDto>(source))
                .ToListAsync();
        }


        public async Task AddRssSource(RssSourceDto rssSource)
        {
            var add = _mapper.Map<RssSource>(rssSource);

            await _unitOfWork.Rss.Add(add);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<RssSourceDto> EditRssSource(RssSourceDto rssSource)
        {
            var oldRss = await _unitOfWork.Rss.GetById(rssSource.Id);

            var editRss = _mapper.Map<RssSource>(rssSource);

            await _unitOfWork.Rss.Remove(oldRss);
            await _unitOfWork.Rss.Add(editRss);
            await _unitOfWork.SaveChangesAsync();

            return await GetRssSourceById(editRss.Id);
        }

        public async Task DeleteRssSource(RssSourceDto rssSource)
        {
            var oldRss = await _unitOfWork.Rss.GetById(rssSource.Id);
            await _unitOfWork.Rss.Remove(oldRss);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}