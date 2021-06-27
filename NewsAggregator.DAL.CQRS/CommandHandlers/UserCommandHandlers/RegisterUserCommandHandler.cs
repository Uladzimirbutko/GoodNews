using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.CQRS.Commands.UserCommands;

namespace NewsAggregator.DAL.CQRS.CommandHandlers.UserCommandHandlers
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, int>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public RegisterUserCommandHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var userRoleId = (await _dbContext.Roles
                .FirstOrDefaultAsync(role => role.Name.Equals("User"), cancellationToken)).Id;
            request.User.RoleId = userRoleId;

            await _dbContext.Users.AddAsync(_mapper.Map<User>(request.User), cancellationToken);

            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

    }
}