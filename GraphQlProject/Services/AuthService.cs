using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using GraphQlProject.Models;
using JwtHelperLib;

namespace GraphQlProject.Services
{
    public class AuthService
    {
        private JwtOptions _jwtOptions;

        public AuthService(IConfiguration configuration)
        {
            _jwtOptions = new JwtOptions(configuration);
        }

        public async Task<string> Login(string userName, UserAuthType userAuthType) =>
              JwtAuth.GenerateJwt(userName, $"{userAuthType}", _jwtOptions);
    }
}
