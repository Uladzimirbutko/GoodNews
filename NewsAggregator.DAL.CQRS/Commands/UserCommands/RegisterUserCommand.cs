using MediatR;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.DAL.CQRS.Commands.UserCommands
{
    public class RegisterUserCommand : IRequest<int>
    {
        public RegisterUserCommand(UserDto user)
        {
            User = user;
        }

        public UserDto User { get; set; }
    }
}