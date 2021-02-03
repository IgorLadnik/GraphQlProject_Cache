using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using JwtAuthLib;
using AuthRolesLib;

namespace JwtLoginLib
{
    public class AuthenticationService
    {
        private JwtOptions _jwtOptions;

        public AuthenticationService(IConfiguration configuration) =>
            _jwtOptions = new JwtOptions(configuration);

        public Task<string> LoginAsync(string userName, UserAuthRole userAuthRole) =>
               Task.FromResult(JwtAuth.GenerateJwt(userName, $"{userAuthRole}", _jwtOptions));
    }
}
