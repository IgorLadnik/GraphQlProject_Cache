using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using JwtAuthLib;

namespace JwtLoginLib
{
    public class AuthenticationService
    {
        private JwtOptions _jwtOptions;

        public AuthenticationService(IConfiguration configuration)
        {
            _jwtOptions = new JwtOptions(configuration);
        }

        public async Task<string> LoginAsync(string userName, UserAuthType userAuthType) =>
              JwtLogin.GenerateJwt(userName, $"{userAuthType}", _jwtOptions);
    }
}
