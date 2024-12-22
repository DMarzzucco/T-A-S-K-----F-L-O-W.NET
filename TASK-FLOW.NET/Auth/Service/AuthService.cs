using TASK_FLOW.NET.Auth.DTO;
using TASK_FLOW.NET.Auth.Service.Interface;
using TASK_FLOW.NET.User.Model;
using TASK_FLOW.NET.User.Service.Interface;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TASK_FLOW.NET.Auth.JWT.Service;
using TASK_FLOW.NET.Auth.Cookie.Service;
using TASK_FLOW.NET.Auth.JWT.Service.Interface;
using TASK_FLOW.NET.Auth.Cookie.Service.Interface;
namespace TASK_FLOW.NET.Auth.Service
{
    public class AuthService : IAuthService
    {
        private readonly string secretKey;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ITokenService _tokenService;
        private readonly ICookieService _cookieService;

        public AuthService(IConfiguration config, IUserService userService, IHttpContextAccessor httpContext, ITokenService tokenService, ICookieService cookieService)
        {
            this.secretKey = config.GetSection("JwtSettings").GetSection("secretKey").ToString();
            this._userService = userService;
            this._httpContext = httpContext;
            this._tokenService = tokenService;
            this._cookieService = cookieService;
        }

        public async Task<string> GenerateToken(UsersModel body)
        {
            var token = this._tokenService.GenerateToken(body);
            await this._userService.UpdateToken(body.Id, token.RefreshTokenHasher);

            this._cookieService.SetTokenCookies(this._httpContext.HttpContext.Response, token);
            return token.AccessToken;
            //var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            //var tokenHandler = new JwtSecurityTokenHandler();

            //var claims = new List<Claim>
            //{
            //    new Claim ("sub", body.Id.ToString()),
            //    new Claim ("rol", body.Roles.ToString())
            //};

            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(claims),
            //    Expires = DateTime.UtcNow.AddDays(2),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            //};
            //var refreshTokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(claims),
            //    Expires = DateTime.UtcNow.AddDays(5),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            //};

            //var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
            //var refreshTokenConfig = tokenHandler.CreateToken(refreshTokenDescriptor);

            //var accessToken = tokenHandler.WriteToken(tokenConfig);
            //var refreshToken = tokenHandler.WriteToken(refreshTokenConfig);

            //var hashRefreshToken = BCrypt.Net.BCrypt.HashPassword(refreshToken, 10);

            //await this._userService.UpdateToken(body.Id, hashRefreshToken);

            //this._httpContext.HttpContext.Response.Cookies.Append("Authentication", accessToken, new CookieOptions
            //{
            //    HttpOnly = true,
            //    Secure = true,
            //    Expires = DateTime.UtcNow.AddDays(2),
            //    SameSite = SameSiteMode.Strict
            //});

            //this._httpContext.HttpContext.Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
            //{
            //    HttpOnly = true,
            //    Secure = true,
            //    Expires = DateTime.UtcNow.AddDays(5),
            //    SameSite = SameSiteMode.Strict
            //});

            //return accessToken;
        }

        public async Task<string> GetProfile()
        {
            var user = await this.GetUserByCookie();
            return user.Username;
        }

        public async Task<UsersModel> GetUserByCookie()
        {
            //var token = this._httpContext.HttpContext.Request.Cookies["Authentication"];
            //if (token == null) throw new UnauthorizedAccessException("Token not found");

            //var tokenHandler = new JwtSecurityTokenHandler();
            //var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            //var userId = int.Parse(jwtToken?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value);

            //if (userId == null) throw new UnauthorizedAccessException("Invalid Token");
            var token = this._cookieService.GetCookies(this._httpContext.HttpContext.Request, "Authentication");
            if (string.IsNullOrEmpty(token)) throw new UnauthorizedAccessException("token not found");
            var userId = this._tokenService.ValidateToken(token, "sub");

            var user = await this._userService.GetById(userId);
            return user;
        }

        public async Task Logout()
        {
            var user = await this.GetUserByCookie();
            await this._userService.UpdateToken(user.Id, null);

            this._cookieService.ClearTokenCookies(this._httpContext.HttpContext.Response);
            //this._httpContext.HttpContext.Response.Cookies.Append("Authentication", "", new CookieOptions
            //{
            //    HttpOnly = true,
            //    Secure = true,
            //    Expires = DateTime.UnixEpoch,
            //    SameSite = SameSiteMode.Strict
            //});
            //this._httpContext.HttpContext.Response.Cookies.Append("RefreshToken", "", new CookieOptions
            //{
            //    HttpOnly = true,
            //    Secure = true,
            //    Expires = DateTime.UnixEpoch,
            //    SameSite = SameSiteMode.Strict
            //});
        }

        public async Task<UsersModel> RefreshTokenValidate(string refreshToken, int id)
        {
            var user = await this._userService.GetById(id);

            var match = BCrypt.Net.BCrypt.Equals(refreshToken, user.RefreshToken);
            if (match == null) throw new UnauthorizedAccessException();

            return user;
        }

        public async Task<UsersModel> ValidationUser(AuthPropsDTO body)
        {
            var user = await this._userService.FindByAuth("Username", body.Username);
            var passwordHasher = new PasswordHasher<UsersModel>();

            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, body.Password);

            if (verificationResult == PasswordVerificationResult.Failed) throw new UnauthorizedAccessException("Password wrong");

            return user;
        }
    }
}
