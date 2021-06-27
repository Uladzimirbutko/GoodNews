using System;
using System.Threading.Tasks;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.Core.Services.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<RefreshTokenDto> GenerateRefreshToken(Guid userId);
        Task<bool> CheckIsRefreshTokenIsValid(string requestToken);
    }
}