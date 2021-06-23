using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Implementation;
using NewsAggregator.DAL.Repositories.Interfaces;
using Serilog;

namespace NewsAggregator.Services.Implementation.Services
{
    public class UserService :IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            return await _unitOfWork.Users.GetAll()
                .Select(n => _mapper.Map<UserDto>(n))
                .ToListAsync();
        }

        public async Task<IEnumerable<UserDto>> GetUserById(Guid id)
        {
            return await _unitOfWork.Users
                .FindBy(n => n.Id.Equals(id))
                .Select(n => _mapper.Map<UserDto>(n))
                .ToListAsync();
        }

        public async Task<IEnumerable<UserDto>> GetUsersByCommentId(Guid commentId)
        {
            //check this Metod.
            var userId = _unitOfWork.Comments
                .FindBy(n => n.Id != null)
                .Select(comment => comment.UserId);
            
            return await _unitOfWork.Users
                .FindBy(user => user.Id.Equals(userId))
                .Select(s => _mapper.Map<UserDto>(s)).ToListAsync();
        }

        
        public async Task<UserDto> GetUserByEmail(string email)
        {
            return _mapper.Map<UserDto>(await _unitOfWork.Users
                .FindBy(user => user.Email.Equals(email))
                .FirstOrDefaultAsync());
        }

        public string GetPasswordHash(string password)
        {
            var sha256 = new SHA256CryptoServiceProvider();
            var sha256data = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hashedPassword = Encoding.UTF8.GetString(sha256data);
            return hashedPassword;
        }

        public async Task<bool> RegisterUser(UserDto userDto)
        {
            try
            {
                var getRoleId =
                    (await _unitOfWork.Roles.FindBy(role => role.Name.Equals("User")).FirstOrDefaultAsync()).Id;

                await _unitOfWork.Users.Add( new User()
                {
                    Id = userDto.Id,
                    Email = userDto.Email,
                    FullName = userDto.FullName,
                    PasswordHash = userDto.PasswordHash,
                    Age = userDto.Age,
                    RoleId = getRoleId
                });

                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Log.Error("Error in Register User" + e.Message);
                return false;
            }
        }
    }
}