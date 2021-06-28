using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NewsAggregator.Core.Services.Interfaces;

namespace WebApplication.Authentication.JWT
{
    public class JwtAuthentication : IJwtAuthentication
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IRefreshTokenService _refreshTokenService;

        public JwtAuthentication(IConfiguration configuration, IUserService userService, IRefreshTokenService refreshTokenService)
        {
            _configuration = configuration;
            _userService = userService;
            _refreshTokenService = refreshTokenService;
        }
        public async Task<JwtAuthenticationResult> GenerateTokens(string email, Claim[] claims)
        {

            var jwtToken = new JwtSecurityToken("NewsAggregator",
                "NewsAggregator",
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                    SecurityAlgorithms.HmacSha256Signature));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var userId = (await _userService.GetUserByEmail(email)).Id;
            var refreshToken = await _refreshTokenService.GenerateRefreshToken(userId);

            return new JwtAuthenticationResult()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
        }
    }
}