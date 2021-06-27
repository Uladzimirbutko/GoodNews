using System;

namespace WebApplication.Authentication.JWT
{
    public class JwtAuthenticationResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}