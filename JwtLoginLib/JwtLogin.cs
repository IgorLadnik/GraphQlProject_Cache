using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RepoInterfaceLib;
using RepoLib;
using JwtLoginLib.Data;
using JwtAuthLib;

namespace JwtLoginLib
{
    public static class JwtLogin
    {
        public static string GenerateJwt(string userId, string userType, JwtOptions jwtOpions)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOpions.SigningKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: jwtOpions.Issuer,
                audience: jwtOpions.Audience,
                claims: new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Role, userType),
                },
                //1 expires: DateTime.Now.AddHours(24),
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public static void AddJwtLogin(this IServiceCollection services, JwtOptions jwtOptions, string connectionString)
        {
            UserDbContext.ConnectionString = connectionString;
            services.AddSingleton<IRepo<UserDbContext>, Repo<UserDbContext>>();
            services.AddScoped<AuthenticationService, AuthenticationService>();

            JwtAuth.AddJwtAuth(services, jwtOptions);
        }
    }
}
