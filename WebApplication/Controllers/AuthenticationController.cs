using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;
using Serilog;
using WebApplication.Authentication.JWT;
using WebApplication.Request;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IJwtAuthentication _jwtAuthentication;
        private readonly IUserService _userService;
        private readonly IRefreshTokenService _refreshTokenService;

        public AuthenticationController (IJwtAuthentication authentication,
            IUserService userService, IRefreshTokenService refreshTokenService, IJwtAuthentication jwtAuthentication)
        {
            _userService = userService;
            _refreshTokenService = refreshTokenService;
            _jwtAuthentication = jwtAuthentication;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {

            try
            {
                if (await _userService.GetUserByEmail(request.Email) != null)
                {
                    return BadRequest("User with this email already existed");
                }

                var passwordHash = _userService.GetPasswordHash(request.Password);

                var isRegistrationSucceed = await _userService.RegisterUser(new UserDto()
                {
                    Id = Guid.NewGuid(),
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    FullName =  request.FullName,
                    Age = request.Age
                });

                if (isRegistrationSucceed)
                {
                    var jwtAuthResult = await GetJwt(request.Email);

                    return Ok(jwtAuthResult);
                }

                return BadRequest("Unsuccessful registration");
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var getUser = await _userService.GetUserByEmail(request.Email);

            if (getUser == null)
            {
                return BadRequest("No user");
            }
            var passwordHash = _userService.GetPasswordHash(request.Password);

            //await _userService.CheckAuthIsValid(new UserDto() { Email = request.Email, PasswordHash = passwordHash })
            if (getUser.PasswordHash == passwordHash)
            {
                var jwtAuthResult = await GetJwt(request.Email);
                return Ok(jwtAuthResult);
            }

            return BadRequest("Email or password is incorrect");
        }

        
        [AllowAnonymous]
        [HttpPost]
        [Route("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            if (!await _refreshTokenService.CheckIsRefreshTokenIsValid(request.RefreshToken))
            {
                return BadRequest("Invalid Refresh Token");
            }

            var userEmail = await _userService.GetUserEmailByRefreshToken(request.RefreshToken);
            if (!string.IsNullOrEmpty(userEmail))
            {
                var jwtAuthResult = await GetJwt(userEmail);
                return Ok(jwtAuthResult);
            }

            return BadRequest("Email or password is incorrect");
        }

        private async Task<JwtAuthenticationResult> GetJwt(string email)
        {
            var roleName = await _userService.GetUserRoleNameByEmail(email);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, roleName)
            };

            var jwtResult = await _jwtAuthentication.GenerateTokens(email, claims);

            return jwtResult;
        }
        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}
