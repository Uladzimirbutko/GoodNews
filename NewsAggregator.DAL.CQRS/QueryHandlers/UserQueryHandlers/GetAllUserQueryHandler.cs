using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.CQRS.Queries.UserQueries;

namespace NewsAggregator.DAL.CQRS.QueryHandlers.UserQueryHandlers
{
    public class GetAllUserQueryHandler :
        IRequestHandler<GetAllUserQuery, IEnumerable<UserDto>>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;


        public GetAllUserQueryHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var news = await _dbContext.Users
                .Select(dto => _mapper.Map<UserDto>(dto))
                .ToListAsync(cancellationToken: cancellationToken);
            return news;
        }
    }
}