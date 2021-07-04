using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<UserDto> GetUserById(Guid id);
        Task<UserDto> GetUserByEmail(string email);
        Task<bool> CheckAuthIsValid(UserDto model);
        Task<string> GetUserEmailByRefreshToken(string refreshToken);
        string GetPasswordHash(string password);
        Task<bool> RegisterUser(UserDto model);
        Task<string> GetUserRoleNameByEmail(string email);
    }
}