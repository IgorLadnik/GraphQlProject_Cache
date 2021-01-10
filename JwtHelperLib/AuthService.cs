using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace JwtHelperLib
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

    public enum UserAuthType
    {
        Regular = 1,
        Admin = 2,
        SuperUser = 3
    }
}
