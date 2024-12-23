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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            this._secretKey = config.GetSection("JwtSettings").GetSection("secretKey").ToString();
            this._httpContextAccessor = httpContextAccessor;
        }

        public int GetIdFromToken()
        {
            var token = this._httpContextAccessor.HttpContext.Request.Cookies["Authentication"];
            if (token == null) throw new UnauthorizedAccessException("Token not found");

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            var Id = int.Parse(jwtToken?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value);
            if (Id == null) throw new UnauthorizedAccessException("Invalid Token");

            return Id;

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

            var accessToken = CreateToken(claims, signingCredential, DateTime.UtcNow.AddDays(2));
            var refreshToken = CreateToken(claims, signingCredential, DateTime.UtcNow.AddDays(5));

            var refreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken);

            return new TokenPair
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                RefreshTokenHasher = refreshTokenHash
            };
        }

        private string CreateToken(IEnumerable<Claim> claims, SigningCredentials signingCredentials, DateTime expiration)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiration,
                SigningCredentials = signingCredentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }

        public void ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.UTF8.GetBytes(this._secretKey);

            var principal = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            tokenHandler.ValidateToken(token, principal, out _);
        }

    }

}