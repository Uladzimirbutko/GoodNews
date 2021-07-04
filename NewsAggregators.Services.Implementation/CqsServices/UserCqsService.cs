using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Interfaces.Services;
using NewsAggregator.DAL.CQRS.Commands.UserCommands;
using NewsAggregator.DAL.CQRS.Queries.UserQueries;
using Serilog;

namespace NewsAggregator.Services.Implementation.CqsServices
{
    public class UserCqsService :IUserService
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public UserCqsService(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            try
            {
                return await _mediator.Send(new GetAllUserQuery());
            }
            catch (Exception e)
            {
                Log.Error($"Error Get All User {e.Message}");
                return null;
            }
        }

        public async Task<UserDto> GetUserById(Guid id)
        {
            try
            {
                return await _mediator.Send(new GetUserByIdQuery(id));
            }
            catch (Exception e)
            {
                Log.Error($"Error get user by id {e.Message}");
                return null;
            }
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            try
            {
                return await _mediator.Send(new GetUserByEmailQuery(email));

            }
            catch (Exception e)
            {
                Log.Error($"Error get user by email {e.Message}");
                return null;
            }
        }

        public async Task<bool> CheckAuthIsValid(UserDto model)
        {
            try
            {
                var user = await _mediator.Send(new CheckAuthenticationQuery(model.Email, model.PasswordHash));
                return true;
            }
            catch (Exception e)
            {
                Log.Error(e, "Register was not successful");
                return false;
            }
        }

        public async Task<string> GetUserEmailByRefreshToken(string refreshToken)
        {
            try
            {
                var userEmail = await _mediator.Send(new GetUserEmailByRefreshTokenQuery(refreshToken));

                return userEmail;
            }
            catch (Exception e)
            {
                Log.Error(e, "Refresh token was not successful");
                return null;
            }
        }

        public string GetPasswordHash(string password)
        {
            var sha256 = new SHA256CryptoServiceProvider();
            var sha256data = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hashedPassword = Encoding.UTF8.GetString(sha256data);
            return hashedPassword;
        }

        public async Task<bool> RegisterUser(UserDto user)
        {
            try
            {
                var register = await _mediator.Send(new RegisterUserCommand(user));
                return true;
            }
            catch (Exception e)
            {
                Log.Error(e, "Register was not successful");
                return false;
            }
        }

        public async Task<string> GetUserRoleNameByEmail(string email)
        {
            try
            {
                var result = await _mediator.Send(new GetUserRoleNameByEmailQuery(email));
                return result;
            }
            catch (Exception e)
            {
                Log.Error($" GetUserRoleNameByEmail Error. {e} ");
                return null;
            }
        }
    }
}