using TASK_FLOW.NET.User.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TASK_FLOW.NET.Auth.JWT.Service.Interface;
using TASK_FLOW.NET.Auth.JWT.DTO;

namespace TASK_FLOW.NET.Auth.JWT.Service
{
    
    public class TokenService : ITokenService
    {
        private readonly string _secretKey;
        public TokenService(IConfiguration config)
        {
            this._secretKey = config.GetSection("JwtSettings").GetSection("secretKey").ToString();
        }

        public TokenPair GenerateToken(UsersModel user)
        {
            var keyBytes = Encoding.UTF8.GetBytes(this._secretKey);
            var signingCredential = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>
            {
                new Claim("sub", user.Id.ToString()),
                new Claim ("rol", user.Id.ToString())
            };
            var accessTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = signingCredential
            };
            var refreshTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(5),
                SigningCredentials = signingCredential
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var accessToken = tokenHandler.WriteToken(tokenHandler.CreateToken(accessTokenDescriptor));
            var refreshToken = tokenHandler.WriteToken(tokenHandler.CreateToken(refreshTokenDescriptor));
            var refreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken);

            return new TokenPair
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                RefreshTokenHasher = refreshTokenHash
            };

        }

        public bool ValidateRefreshToken(string refreshToken, string storedRefreshToken)
        {
            return BCrypt.Net.BCrypt.Verify(refreshToken, storedRefreshToken);
        }
        public int ValidateToken(string token, string claimType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.UTF8.GetBytes(this._secretKey);
            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                ValidateIssuer = false, 
                ValidateAudience = false,
                ValidateLifetime = true
            };

            var claimsPrincipal = tokenHandler.ValidateToken(token, parameters, out _);
            var claimValue = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;

            if (string.IsNullOrEmpty(claimValue)) throw new UnauthorizedAccessException("Invalid Token");

            return int.Parse(claimValue);
        }

    }
   
}
