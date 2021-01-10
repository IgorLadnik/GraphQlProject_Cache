using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using GraphQlProject.Data;
using GraphQlHelperLib;
using JwtHelperLib;


namespace GraphQlProject.Services
{
    public class AuthService
    {
        private DbProvider<GraphQLDbContext> _dbProvider;
        private JwtOptions _jwtOptions;

        public AuthService(DbProvider<GraphQLDbContext> dbProvider, IConfiguration configuration)
        {
            _dbProvider = dbProvider;
            _jwtOptions = new JwtOptions(configuration);
        }

    public async Task<string> Login(string userName, string password)
        {
            var user = await _dbProvider.FetchAsync(dbContext => dbContext.Users
                            .Where(u => u.UserName == userName && u.Password == password)
                            .FirstOrDefault());
            return user != null ? JwtAuth.GenerateJwt($"{user.Id}", $"{user.Type}", _jwtOptions) : null;
        }
    }
}
