using System.Collections.Generic;
using System.Linq;
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
    public class GetAllRoleQueryHandler :
        IRequestHandler<GetAllRoleQuery, IEnumerable<RoleDto>>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;


        public GetAllRoleQueryHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleDto>> Handle(GetAllRoleQuery request, CancellationToken cancellationToken)
        {
            var role = await _dbContext.Roles
                .Select(dto => _mapper.Map<RoleDto>(dto))
                .ToListAsync( cancellationToken);
            return role;
        }
    }
}