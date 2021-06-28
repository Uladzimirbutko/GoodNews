using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.CQRS.Queries.RoleQueries;

namespace NewsAggregator.DAL.CQRS.QueryHandlers.RoleQueryHandlers
{
    public class GetRoleByUserIdQueryHandler :
        IRequestHandler<GetRoleByUserIdQuery, RoleDto>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;


        public GetRoleByUserIdQueryHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<RoleDto> Handle(GetRoleByUserIdQuery request, CancellationToken cancellationToken)
        {
            var roleIdFromUser = _dbContext.Users.AsNoTracking()
                .FirstOrDefaultAsync(user1 => user1.Id.Equals(request.Id), cancellationToken: cancellationToken).Result.RoleId;

            var role = _mapper.Map<RoleDto>(await _dbContext.Roles
                    .FirstOrDefaultAsync(role1 => role1.Id.Equals(roleIdFromUser), cancellationToken: cancellationToken));
            
            return role;
        }
    }
}