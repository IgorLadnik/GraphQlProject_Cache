using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace JwtHelperLib
{
    public class AuthenticationService
    {
        private JwtOptions _jwtOptions;

        public AuthenticationService(IConfiguration configuration)
        {
            _jwtOptions = new JwtOptions(configuration);
        }

        public async Task<string> LoginAsync(string userName, UserAuthType userAuthType) =>
              JwtAuth.GenerateJwt(userName, $"{userAuthType}", _jwtOptions);
    }

    public enum UserAuthType
    {
        Regular = 1,
        Admin = 2,
        SuperUser = 3
    }
}
