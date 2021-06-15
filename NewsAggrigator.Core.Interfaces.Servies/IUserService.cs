using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<UserDto> GetUserById(Guid id);
        Task<UserDto> GetUserByCommentId(Guid commentId);
        Task<UserDto> GetUserByLogin(string login);
        Task<UserDto> GetUserByEmail(string email);
        string GetPasswordHash(string password);
        Task<bool> RegisterUser(UserDto model);
    }
}