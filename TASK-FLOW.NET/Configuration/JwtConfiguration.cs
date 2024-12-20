using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using System.Text;

namespace TASK_FLOW.NET.Configuration
{
    public static class JwtConfiguration
    {
        public static IServiceCollection AddJWTAuthentication(this IServiceCollection service, IConfiguration configuration)
        {
            var secretKey = configuration.GetSection("JwtSettings").GetSection("secretKey").ToString();

            service.AddAuthentication(cfg =>
            {
                cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(cfg =>
            {
                var signgKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var signgCredentials = new SigningCredentials(signgKey, SecurityAlgorithms.HmacSha256Signature);

                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signgKey,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            return service;
        }
    }
}
