using MediatR;
using NewsAggregator.Core.DataTransferObjects;

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