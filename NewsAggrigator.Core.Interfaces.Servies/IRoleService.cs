using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.Core.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetRoles();
        Task<RoleDto> GetRoleByUserId(Guid userId);
        Task ReplaceUserRole(Guid userId, RoleDto role);
    }
}