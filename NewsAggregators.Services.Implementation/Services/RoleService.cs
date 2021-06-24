using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Implementation;

namespace NewsAggregator.Services.Implementation.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<RoleDto>> GetRoles()
        {
           return await _unitOfWork.Roles.GetAll()
                    .Select(role => _mapper.Map<RoleDto>(role))
                    .ToListAsync();
        }

        public async Task<RoleDto> GetRoleByUserId(Guid userId)
        {
            
            return _mapper.Map<RoleDto>(await _unitOfWork.Roles
                .GetById((await _unitOfWork.Users
                    .GetById(userId)).RoleId));
            
        }

        public async Task ReplaceUserRole(Guid userId, RoleDto role)
        {
            var user = (await _unitOfWork.Users.GetById(userId));

            user.Role = _mapper.Map<Role>(role);

            await _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}