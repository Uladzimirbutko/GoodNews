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
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserDto>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;


        public GetUserByEmailQueryHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<UserDto>(await _dbContext.Users.FirstOrDefaultAsync(source => source.Email.Equals(request.Email), cancellationToken: cancellationToken));
            return user;
        }
    }
}