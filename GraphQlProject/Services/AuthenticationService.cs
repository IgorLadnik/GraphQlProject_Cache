using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using GraphQlProject.DTO;
using GraphQlProject.Models;
using GraphQlProject.Data;
using GraphQlHelperLib;

namespace GraphQlProject.Services
{
    public class AuthenticationService
    {
        private DbProvider<GraphQLDbContext> _dbProvider;
        private IConfiguration _configuration;

        public AuthenticationService(DbProvider<GraphQLDbContext> dbProvider, IConfiguration configuration)
        {
            _dbProvider = dbProvider;
            _configuration = configuration;
        }

        public async Task<string> Login(UserDTO userDTO)
        {
            var user = await _dbProvider.FetchAsync(dbContext => dbContext.Users
                            .Where(u => u.UserName == userDTO.UserName && u.Password == userDTO.Password)
                            .FirstOrDefault());
            return user != null ? GenerateJwt(user) : null;
        }

        private string GenerateJwt(User user)
        {
            var key = _configuration.GetValue<string>("JwtSigningKey");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                claims: new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Type.ToString()),
                },
                //1 expires: DateTime.Now.AddHours(24),
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
    }
}
