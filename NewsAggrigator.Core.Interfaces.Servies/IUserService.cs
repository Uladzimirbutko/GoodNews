using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsers();
        Task<UserDto> GetUserById(Guid id);
        string GetPasswordHash(string modelPassword);
        Task<bool> RegisterUser(UserDto model);
        Task<UserDto> GetUserByEmail(string email);
    }
}